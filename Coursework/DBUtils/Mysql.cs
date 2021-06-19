using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Coursework.Model;
using Coursework.ViewModel;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace Coursework.DBUtils
{
    public static class Mysql
    {
        private const string Host = "localhost";
        private const string Database = "Coursework";
        private const string Port = "3306";
        private const string Username = "root";
        private const string Password = "root";


        public static MySqlConnection GetConnection()
        {
            return new MySqlConnection("Server=" + Host +
                                       ";Database=" + Database +
                                       ";port=" + Port +
                                       ";User Id=" + Username +
                                       ";password=" + Password);
        }

        public static List<string> ExecuteSql(string sql)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                MySqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = sql;
                List<string> table = new List<string>();
                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int i = 0;
                        string row = "";
                        while (reader.FieldCount > i)
                        {
                            row += reader.GetString(i) + "\n";
                            i++;
                        }
                        table.Add(row);
                    }
                }
                connection.Close();
                return table;
            }
        }
        public static ObservableCollection<University> ToUniversities(this List<string> table)
        {
            ObservableCollection<University> universities = new ObservableCollection<University>();
            foreach (string row in table)
            {
                string[] cells = row.Split('\n');
                universities.Add(new University(
                    int.Parse(cells[0]),
                    cells[1],
                    cells[2],
                    cells[3],
                    cells[4],
                    cells[5],
                    ExecuteSql($"select id,name,code,duration,currentId from SpecialtyView where uid={cells[0]};").ToSpecialties()
                    ));
            }
            return universities;
        }
        public static ObservableCollection<Specialty> ToSpecialties(this List<string> table)
        {
            ObservableCollection<Specialty> specialties = new ObservableCollection<Specialty>();
            foreach (string row in table)
            {
                string[] cells = row.Split('\n');
                specialties.Add(new Specialty(
                    int.Parse(cells[0]),
                    cells[1],
                    cells[2],
                    cells[3],
                    ExecuteSql($"select id,form,type,count,avgscore,year,price from specificspecialtyview where id_spec={cells[0]}").ToSpecificSpecialties().Normalise(),
                cells[4]));

            }

            return specialties;
        }
        public static ValueTuple<ObservableCollection<Budget>, ObservableCollection<Commerce>> ToSpecificSpecialties(this List<string> table)
        {
            ObservableCollection<Budget> budgets =
                new ObservableCollection<Budget>();
            ObservableCollection<Commerce> commerces =
                new ObservableCollection<Commerce>();
            foreach (string row in table)
            {
                string[] cells = row.Split('\n');
                if (cells[2].Equals("Бюджет"))
                {
                    budgets.Add(new Budget(
                         int.Parse(cells[0]),
                         cells[1],
                         cells[2],
                         int.Parse(cells[3]),
                         cells[4],
                         cells[5]));
                }
                else
                {
                    commerces.Add(new Commerce(
                         int.Parse(cells[0]),
                         cells[1],
                         cells[2],
                         int.Parse(cells[3]),
                         cells[4],
                         cells[5],
                         int.Parse(cells[6]))
                     );
                }
            }

            return new ValueTuple<ObservableCollection<Budget>,
                ObservableCollection<Commerce>>(budgets, commerces);
        }
        public static void UpdateSpecialty(int uid = 0, string newName = "", string newCode = "", string newDuration = "")
        {
            if (uid != 0)
            {
                List<string> specs =
                ExecuteSql(
                    "select id,name,code,duration from (select `university specialty`.id_university uid, specialty.id id,specialty.name\n" +
                    "name,specialty.code code,specialty.duration duration\nfrom specialty,`university specialty`\n" +
                    $"where specialty.id=`university specialty`.id_specialty) as Specs where uid={uid}; ");
                foreach (string spec in specs)
                {
                    var temp = spec.Split('\n');
                    if (temp[1] == newName || temp[2] == newCode || temp[3] == newDuration)
                    {
                        ExecuteNonQuerySql($"update specialty set name = '{newName}', code = '{newCode}', duration = '{newDuration}' where id = {spec[0]};");
                        return;
                    }
                }
                using (var connection = GetConnection())
                {
                    connection.Open();
                    // Sql
                    MySqlCommand cmd = connection.CreateCommand();
                    cmd.CommandText = $"insert into specialty(name, code, duration) value('{newName}','{newCode}','{newDuration}');";
                    cmd.ExecuteNonQuery();
                    cmd.CommandText =
                        $"select id from specialty where name=\'{newName}\' and code=\'{newCode}\' and duration=\'{newDuration}\'";
                    using (DbDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            cmd.CommandText = $"insert into `university specialty`(id_university,id_specialty) value({uid},{reader.GetString(0)});";
                        }
                    }
                    connection.Close();
                }
            }
        }
        public static bool InsertSpecialty(int uid = 0, string newName = "", string newCode = "", string newDuration = "")
        {
            List<string> specs =
                ExecuteSql($"select specialty.id, specialty.name, specialty.code, specialty.duration from specialty");
            foreach (string spec in specs)
            {
                var temp = spec.Split('\n');
                if (temp[1] == newName && temp[2] == newCode && temp[3] == newDuration)
                {
                    var cityIdlist = Mysql.ExecuteSql($"SELECT * FROM coursework.`university specialty` where `id_university`='{uid}' and `id_specialty`='{temp[0]}';");
                    var cityId = "";
                    if (cityIdlist != null && cityIdlist.Count > 0)
                    {
                        return false;
                    }
                    ExecuteSql($"insert into `university specialty`(id_university,id_specialty) value({uid},{temp[0]});");
                    return true;
                }
            }
            using (var connection = GetConnection())
            {
                var id = ExecuteNonQuerySqlWithId(
                    $"insert into specialty(name, code, duration) value('{newName}','{newCode}','{newDuration}');");
                ExecuteNonQuerySql($"insert into `university specialty`(id_university,id_specialty) value('{uid}','{id}');");
            }
            return true;
        }
        public static void RemoveSpecialty(string id, int uid, int specId)
        {
            
            ExecuteNonQuerySql($"DELETE FROM `university specialty` WHERE id= {id} and id_University={uid} and id_specialty={specId};");
        }
        public static void UpdateSpecSpecialty(int specId, int form, int type, int count, string avgscore, string year, string price = "")
        {
            ExecuteNonQuerySql($"update courseinfo set id_form='{form}', id_type='{type}', count='{count}', avg_score='{avgscore}', year='{year}' where courseinfo.id={specId};");
            if (price != "")
            {
                var priceId = ExecuteSql($"select id from price where id_course={specId}")[0].Replace("\n", "");
                if (priceId == "")
                {
                    ExecuteNonQuerySql($"insert into price(id_course,price) value('{specId}','{price}');");
                }
                else
                {
                    ExecuteNonQuerySql($"update price set price={price} where id={priceId}");
                }
            }
        }
        public static void InsertSpecSpecialty(int specId, int form, int type, int count, string avgscore, string year, string price = "")
        {
            var id = ExecuteNonQuerySqlWithId(
                "insert into courseinfo(id_currentspec, id_form, id_type, count, avg_score, year)" +
                $" value('{specId}','{form}','{type}','{count}','{avgscore}','{year}');");
            ExecuteNonQuerySql($"insert into price(id_course,price) value('{id}','{price}')");
        }
        public static void RemoveSpecSpecialty(int id)
        {
            ExecuteNonQuerySql($"delete from courseinfo where id={id};");
        }
        internal static void ExecuteNonQuerySql(string sql)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                MySqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                connection.Close();
            }
        }
        internal static string ExecuteNonQuerySqlWithId(string sql)
        {
            var id = "";
            using (var connection = GetConnection())
            {
                connection.Open();
                MySqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                cmd.CommandText = "select last_insert_id();";
                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        id = reader.GetString(0);
                    }
                }

                connection.Close();
            }

            return id;
        }
    }
}

static class TupleExtention
{

    public static ObservableCollection<SpecificSpecialty> Normalise(
        this ValueTuple<ObservableCollection<Budget>, ObservableCollection<Commerce>> tuple
        )
    {
        var specs = new ObservableCollection<SpecificSpecialty>();
        foreach (SpecificSpecialty spec in tuple.Item1)
        {
            if (spec is null)
                continue;
            specs.Add(spec);
        }
        foreach (SpecificSpecialty spec in tuple.Item2)
        {
            if (spec is null)
                continue;
            specs.Add(spec);
        }
        return specs;
    }
}
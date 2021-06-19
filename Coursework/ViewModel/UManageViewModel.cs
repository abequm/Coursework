using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Coursework.Model;
using Coursework.DBUtils;
using Coursework.Navigation;
using System.Windows.Controls;
using System.Windows.Media;

namespace Coursework.ViewModel
{
    class UManageViewModel : ViewModelBase
    {
        #region Variables

        // data binding
        private University _university;
        public University University
        {
            get
            {
                if (_university == null)
                {
                    _university = Mysql.ExecuteSql($"select * from universitysview where role='{Navigation.RoleId}';")
                        .ToUniversities()[0];
                    Uname = _university.Name;
                    Ufullname = _university.FullName;
                    Uregion = _university.Region;
                    UCity = _university.City;
                    Uaddress = _university.Address;
                }

                return _university;
            }
            set
            {
                if (value == null)
                {
                    SelectedSpecialty = null;
                    SelectedSpecSpecialty = null;
                    ClearSpecFields();
                }
                Set(ref _university, value);

            }
        }
        private Specialty _selectedSpecialty;
        public Specialty SelectedSpecialty
        {
            get => _selectedSpecialty;
            set
            {
                Set(ref _selectedSpecialty, value);
                if (_selectedSpecialty != null)
                {
                    Name = _selectedSpecialty.Name;
                    Code = _selectedSpecialty.Code;
                    Duration = _selectedSpecialty.Duration;
                }
                ClearSSpecFields();
            }
        }
        private SpecificSpecialty _selectedSpecSpecialty;
        public SpecificSpecialty SelectedSpecSpecialty
        {
            get => _selectedSpecSpecialty;
            set
            {
                Set(ref _selectedSpecSpecialty, value);
                if (_selectedSpecSpecialty != null)
                {
                    Id = _selectedSpecSpecialty.Id;
                    Form = _selectedSpecSpecialty.Form;
                    Type = _selectedSpecSpecialty.Type;
                    if (Type == "Коммерция")
                    {
                        Price = _selectedSpecSpecialty.Price.ToString();
                    }
                    Count = _selectedSpecSpecialty.Count.ToString();
                    AvgScore = _selectedSpecSpecialty.AvgScore;
                    Year = _selectedSpecSpecialty.Year;
                }
            }
        }
        // Specialty
        private string _name;
        public string Name
        {
            get => _name;
            set => Set(ref _name, value);
        }
        private string _code;
        public string Code
        {
            get => _code;
            set => Set(ref _code, value);
        }
        private string _duration;
        public string Duration
        {
            get => _duration;
            set => Set(ref _duration, value);
        }
        // SpecificSpecialty
        private int _id;

        public int Id
        {
            get => _id;
            set => Set(ref _id, value);
        }
        private string _form;
        public string Form
        {
            get => _form;
            set => Set(ref _form, value);
        }
        private string _type;
        public string Type
        {
            get => _type
            ;
            set => Set(ref _type
                , value);
        }
        private List<string> _types;
        public List<string> Types
        {
            get
            {
                if (_types == null)
                {
                    _types = Mysql.ExecuteSql("Select name from typedictionary;");
                    for (int i = 0; i < _types.Count; i++)
                    {
                        _types[i] = _types[i].Replace("\n", "");
                    }
                }

                return _types;
            }
            set => Set(ref _types, value);
        }
        private List<string> _forms;
        public List<string> Forms
        {
            get
            {
                if (_forms == null)
                {
                    _forms = Mysql.ExecuteSql("Select name from formdictionary;");
                    for (int i = 0; i < _forms.Count; i++)
                    {
                        _forms[i] = _forms[i].Replace("\n", "");
                    }
                }
                return _forms;
            }
            set => Set(ref _forms, value);
        }

        private string _price;

        public string Price
        {
            get => _price;
            set => Set(ref _price, value);
        }
        private string _count;
        public string Count
        {
            get => _count;
            set => Set(ref _count, value);
        }
        private string _avgscore;
        public string AvgScore
        {
            get => _avgscore;
            set => Set(ref _avgscore, value);
        }
        private string _year;
        public string Year
        {
            get => _year;
            set => Set(ref _year, value);
        }
        // UniversityProperties
        private string _Uname;

        public string Uname
        {
            get => _Uname;
            set => Set(ref _Uname, value);
        }

        private string _UCity;

        public string UCity
        {
            get => _UCity;
            set => Set(ref _UCity, value);
        }

        private string _Uregion;

        public string Uregion
        {
            get => _Uregion;
            set => Set(ref _Uregion, value);
        }

        private string _Ufullname;

        public string Ufullname
        {
            get => _Ufullname;
            set => Set(ref _Ufullname, value);
        }

        private string _Uaddress;

        public string Uaddress
        {
            get => _Uaddress;
            set => Set(ref _Uaddress, value);
        }

        #endregion

        #region Commands

        #region CommandBack


        public ICommand Back { get; }
        private bool CanBackExecute(object p) => true;

        private void OnBackExecuted(object p)
        {
            Navigation.SwitchView(typeof(WelcomeViewModel), Constants.Titles.WelcomeView, new[] { "Reset" });

        }

        #endregion

        #region CommandAddSpecialty

        public ICommand AddSpecialty { get; }

        private bool CanAddSpecialtyExecute(object p)
        {
            if (String.IsNullOrEmpty(Name)) return false;
            if (String.IsNullOrEmpty(Code)) return false;
            if (String.IsNullOrEmpty(Duration)) return false;
            return true;
        }

        private void OnAddSpecialtyExecuted(object p)
        {
            bool isSuccess = Mysql.InsertSpecialty(University.Id, Name, Code, Duration);
            if (isSuccess)
            {
                MessageBox.Show("Добавлено");
                University = null;
            }
            else
            {
                MessageBox.Show("Специальность уже существует");
            }
        }

        #endregion

        #region CommandEditSpecialty

        public ICommand EditSpecialty { get; }
        private bool CanEditSpecialtyExecute(object p)
        {

            if (String.IsNullOrEmpty(Name)) return false;
            if (String.IsNullOrEmpty(Code)) return false;
            if (String.IsNullOrEmpty(Duration)) return false;
            if (SelectedSpecialty == null) return false;
            if (Name == SelectedSpecialty.Name &&
                Code == SelectedSpecialty.Code &&
                Duration == SelectedSpecialty.Duration) return false;
            return true;
        }

        private void OnEditSpecialtyExecuted(object p)
        {
            Mysql.UpdateSpecialty(University.Id, Name, Code, Duration);
            University = null;
            MessageBox.Show("Изменено");

        }

        #endregion

        #region CommandRemoveSpecialty

        public ICommand RemoveSpecialty { get; }

        private bool CanRemoveSpecialtyExecute(object p)
        {
            if (SelectedSpecialty == null) return false;
            return true;
        }

        private void OnRemoveSpecialtyExecuted(object p)
        {
            Mysql.RemoveSpecialty(SelectedSpecialty.CurrentId, University.Id, SelectedSpecialty.Id);
            University = null;
            MessageBox.Show("Удалено");

        }

        #endregion

        #region CommandAddSpecSpecialty

        public ICommand AddSpecSpecialty { get; }

        private bool CanAddSpecSpecialtyExecute(object p)
        {
            if (SelectedSpecialty == null) return false;
            if (String.IsNullOrEmpty(Form)) return false;
            if (String.IsNullOrEmpty(Type)) return false;
            if (String.IsNullOrEmpty(Count)) return false;
            if (String.IsNullOrEmpty(AvgScore)) return false;
            if (String.IsNullOrEmpty(Year)) return false;
            if (Type == "Коммерция" && String.IsNullOrEmpty(Price)) return false;
            return true;
        }

        private void OnAddSpecSpecialtyExecuted(object p)
        {
            Mysql.InsertSpecSpecialty(SelectedSpecialty.Id, Forms.IndexOf(Form) + 1, Types.IndexOf(Type) + 1, int.Parse(Count),
                AvgScore.Replace(',', '.'), Year, string.IsNullOrEmpty(Price) ? "" : Price);
            University = null;
            MessageBox.Show("Добавлено");

        }

        #endregion

        #region CommandEditSpecSpecialty

        public ICommand EditSpecSpecialty { get; }

        private bool CanEditSpecSpecialtyExecute(object p)
        {
            if (SelectedSpecSpecialty == null && Id == 0) return false;
            if (string.IsNullOrEmpty(Form)) return false;
            if (string.IsNullOrEmpty(Type)) return false;
            if (Type == "Коммерция" && string.IsNullOrEmpty(Price)) return false;
            if (string.IsNullOrEmpty(Count)) return false;
            if (string.IsNullOrEmpty(AvgScore)) return false;
            if (string.IsNullOrEmpty(Year)) return false;
            if (Count == SelectedSpecSpecialty.Count.ToString() &&
                Year == SelectedSpecSpecialty.Year &&
                AvgScore == SelectedSpecSpecialty.AvgScore &&
                Form == SelectedSpecSpecialty.Form &&
                Type == SelectedSpecSpecialty.Type &&
                (Price == SelectedSpecSpecialty.Price.ToString()
                 || Type == "Бюджет")) return false;
            return true;
        }

        private void OnEditSpecSpecialtyExecuted(object p)
        {

            Mysql.UpdateSpecSpecialty(Id, Forms.IndexOf(Form) + 1, Types.IndexOf(Type) + 1, int.Parse(Count),
                AvgScore.Replace(',', '.'), Year, string.IsNullOrEmpty(Price) ? "" : Price);
            University = null;
            MessageBox.Show("Изменено");
        }

        #endregion

        #region CommandRemoveSpecSpecialty

        public ICommand RemoveSpecSpecialty { get; }

        private bool CanRemoveSpecSpecialtyExecute(object p)
        {
            if (SelectedSpecialty == null) return false;
            if (SelectedSpecSpecialty == null && Id == 0) return false;
            return true;
        }

        private void OnRemoveSpecSpecialtyExecuted(object p)
        {

            Mysql.RemoveSpecSpecialty(Id);
            MessageBox.Show("Удалено");
            University = null;
        }

        #endregion

        #region CommandEditUProperties

        public ICommand EditUProperties { get; }
        private bool CanEditUPropertiesExecute(object p)
        {
            if (string.IsNullOrEmpty(Uname)) return false;
            if (string.IsNullOrEmpty(UCity)) return false;
            if (string.IsNullOrEmpty(Uregion)) return false;
            if (string.IsNullOrEmpty(Uaddress)) return false;
            if (string.IsNullOrEmpty(Ufullname)) return false;
            if (Ufullname == University.FullName &&
                Uaddress == University.Address &&
                Uregion == University.Region &&
                UCity == University.City &&
                Uname == University.Name) return false;
            return true;
        }

        private void OnEditUPropertiesExecuted(object p)
        {
            var city = Mysql.ExecuteSql($"Select id from city where city='{UCity}';")[0].Split('\n')[0];
            var region= Mysql.ExecuteSql($"Select id from region where region='{Uregion}';")[0].Split('\n')[0];
            if (string.IsNullOrEmpty(city) || int.Parse(city) == 0)
            {
                city = Mysql.ExecuteNonQuerySqlWithId($"Insert into city(city) value('{UCity}');");
            }

            if (string.IsNullOrEmpty(region) || int.Parse(region) == 0)
            {
                region = Mysql.ExecuteNonQuerySqlWithId($"Insert into region(region) value('{Uregion}');");
            }
            Mysql.ExecuteNonQuerySql("update university set " +
                             $"name='{Uname}',fullname='{Ufullname}',address='{Uaddress}'," +
                             $"city='{city}',region='{region}' where id={University.Id};");
            string result = "\tИзменено\n";
            if (Uname != University.Name) result += University.Name + "\t=>\t" + Uname + "\n";
            if (Ufullname != University.FullName) result += University.FullName + "\t=>\t" + Ufullname + "\n";
            if (Uregion != University.Region) result += University.Region + "\t=>\t" + Uregion + "\n";
            if (UCity != University.City) result += University.City + "\t=>\t" + UCity + "\n";
            if (Uaddress != University.Address) result += University.Address + "\t=>\t" + Uaddress;
            MessageBox.Show(result);
            University = null;
        }

        #endregion

        #endregion

        #region Ctor
        public UManageViewModel(object nav, object[] args) : base(nav)
        {
            Back = new Action(OnBackExecuted, CanBackExecute);
            AddSpecialty = new Action(OnAddSpecialtyExecuted, CanAddSpecialtyExecute);
            EditSpecialty = new Action(OnEditSpecialtyExecuted, CanEditSpecialtyExecute);
            RemoveSpecialty = new Action(OnRemoveSpecialtyExecuted, CanRemoveSpecialtyExecute);
            AddSpecSpecialty = new Action(OnAddSpecSpecialtyExecuted, CanAddSpecSpecialtyExecute);
            EditSpecSpecialty = new Action(OnEditSpecSpecialtyExecuted, CanEditSpecSpecialtyExecute);
            RemoveSpecSpecialty = new Action(OnRemoveSpecSpecialtyExecuted, CanRemoveSpecSpecialtyExecute);
            EditUProperties = new Action(OnEditUPropertiesExecuted, CanEditUPropertiesExecute);
        }

        #endregion

        #region Methods
        void ClearSpecFields()
        {
            Name = null;
            Code = null;
            Duration = null;
        }
        void ClearSSpecFields()
        {
            Id = 0;
            Form = null;
            Type = null;
            Count = null;
            AvgScore = null;
            Year = null;
        }

        #endregion  
    }
}

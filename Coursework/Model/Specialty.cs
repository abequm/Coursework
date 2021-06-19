using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Documents.DocumentStructures;
using System.Windows.Media;

namespace Coursework.Model
{

    public class Specialty
    {
        private int _id;
        private string _name;
        private string _code;
        private string _durarion;

       private ObservableCollection<Budget> _budgets;
        private ObservableCollection<Commerce> _commerces;
        private ObservableCollection<SpecificSpecialty> _specSpecialty;
        public int Id { get => _id; set => _id = value; }
        public string Name { get => _name; set => _name = value; }
        public string Code { get => _code; set => _code = value; }
        public string Duration { get => _durarion; set => _durarion = value; }
        public string CurrentId;

        public ObservableCollection<Budget> Budgets { get => _budgets; set => _budgets = value; }
        public ObservableCollection<Commerce> Commerces { get => _commerces; set => _commerces = value; }
        public ObservableCollection<SpecificSpecialty> SpecSpecialty { get => _specSpecialty; set => _specSpecialty = value; }

        public ObservableCollection<SpecificSpecialty> NowSpec
        {
            get
            {
                var now = new ObservableCollection<SpecificSpecialty>();
                var currDate = DateTime.Now.Year;
                if (SpecSpecialty != null)
                {
                    foreach (var item in SpecSpecialty)
                    {
                        if (item.Year == currDate.ToString())
                            now.Add(item);

                    }
                }
                return now;
            }
        }
        public ObservableCollection<SpecificSpecialty> OldSpec
        {
            get
            {
                var now = new ObservableCollection<SpecificSpecialty>();
                var currDate = DateTime.Now.Year;
                foreach (var item in SpecSpecialty)
                {
                    if (item.Year != currDate.ToString())
                        now.Add(item);
                }
                return now;
            }
        }

        public string BCount { get => this.BudgetCount(); }
        public string CCount { get => this.CommerceCount(); }
        public int Count { get => this.SpecSpecialty.Count; }

        public Specialty(int id, string name, string code, string duration,
            (ObservableCollection<Budget> budgets, ObservableCollection<Commerce> commerces) tuple)
        {
            Id = id;
            Name = name;
            Code = code;
            Duration = duration;
            Budgets = tuple.budgets;
            Commerces = tuple.commerces;
            foreach (var commerce in Commerces)
            {
                SpecSpecialty.Add(commerce);
            }
            foreach (var budget in Budgets)
            {
                SpecSpecialty.Add(budget);
            }
        }
        public Specialty(int id, string name, string code, string duration, ObservableCollection<SpecificSpecialty> ss, string CurrId = null)
        {
            Id = id;
            Name = name;
            Code = code;
            Duration = duration;
            SpecSpecialty = ss;
            CurrentId = CurrId;
        }

        private string BudgetCount()
        {
            var result = mCount("Бюджет");
            if (result == 0)
                return "нету";
            return result.ToString();
        }
        private string CommerceCount()
        {
            var result = mCount("Коммерция");
            if (result == 0)
                return "нету";
            return result.ToString();
        }
        private int mCount(string type)
        {
            var count = 0;
            foreach (var spec in this.SpecSpecialty)
            {
                if (spec.Type == type)
                {
                    count++;
                }
            }
            return count;
        }

        private bool ContainsType(string type)
        {
            foreach (var spec in NowSpec)
            {
                if (spec.Type == type)
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsContainsCommerce { get => ContainsCommerce(); }
        private bool ContainsCommerce()
        {
            return ContainsType("Коммерция");
        }
        public bool IsContainsBudget { get => ContainsBudget(); }
        private bool ContainsBudget()
        {
            return ContainsType("Бюджет");
        }




        public Specialty(Specialty another)
        {
            ObservableCollection<SpecificSpecialty> refSpecSpecialty = new ObservableCollection<SpecificSpecialty>();
            try
            {
                for (int i = 0; i < another.SpecSpecialty.Count; i++)
                {
                    refSpecSpecialty.Add((SpecificSpecialty)another.SpecSpecialty[i].clone());
                }
            }
            catch (Exception e) { }
            Id = another.Id;
            Name = another.Name;
            Code = another.Code;
            Duration = another.Duration;
            SpecSpecialty=refSpecSpecialty;
        }
        public Object clone()
        {
            return new Specialty(this);
        }




    }
}

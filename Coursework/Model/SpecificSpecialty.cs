
using System;
using System.Globalization;

namespace Coursework.Model
{
    public class SpecificSpecialty
    {
        protected int _id;
        protected string _form;
        protected string _type;
        protected int _count;
        protected string _avgScore;
        protected string _year;
        private int _price;

        public int Id { get => _id; set => _id = value; }
        public string Form { get => _form; set => _form = value; }
        public string Type { get => _type; set => _type = value; }
        public int Count { get => _count; set => _count = value; }
        public string AvgScore { get => _avgScore; set => _avgScore = value; }
        public string Year
        {
            get => _year;
            set => _year = value;
        }


        public int Price { get => _price; set => _price = value; }

        public SpecificSpecialty(int id, string form, string type, int count, string avgScore, string year, int price = 0)
        {
            Id = id;
            Form = form;
            Type = type;
            Count = count;
            AvgScore = avgScore;
            Year = year;
            Price = price;
        }
        public SpecificSpecialty(SpecificSpecialty another)
        {
            Id = another.Id;
            Form = another.Form;
            Type = another.Type;
            Count = another.Count;
            AvgScore = another.AvgScore;
            Year = another.Year;
            Price = another.Price;
        }
        public Object clone()
        {
            return new SpecificSpecialty(this);
        }


    }
    public class Budget : SpecificSpecialty
    {
        public Budget(int id, string form, string type, int count, string avgScore, string year) : base(id, form, type, count, avgScore, year)
        { }
    }

    public class Commerce : SpecificSpecialty
    {
        public Commerce(int id, string form, string type, int count, string avgScore, string year, int price) : base(id, form, type, count, avgScore, year, price)
        { }
    }
}

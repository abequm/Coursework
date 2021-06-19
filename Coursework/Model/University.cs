using System;
using System.Collections.ObjectModel;
using Coursework.ViewModel;

namespace Coursework.Model
{
    public class University
    {
        private int _id;
        private string _address;
        private string _city;
        private string _region;
        private string _name;
        private string _fullName;
        private ObservableCollection<Specialty> _specialty;



        public int Id
        {
            get => _id;
            set => _id = value;
        }

        public string Address
        {
            get => _address;
            set => _address = value;
        }

        public string City
        {
            get => _city;
            set => _city = value;
        }

        public string Region
        {
            get => _region;
            set => _region = value;
        }

        public string Name
        {
            get => _name;
            set => _name = value;
        }

        public string FullName
        {
            get => _fullName;
            set => _fullName = value;
        }

        public ObservableCollection<Specialty> Specialty
        {
            get => _specialty;
            set => _specialty = value;
        }

        public int SpecCount
        {
            get => _specialty.Count;
        }


        public University(int id, string name,string fullName, string address, string city, string region,
            ObservableCollection<Specialty> specialty)
        {
            Id = id;
            Name = name;
            FullName = fullName;
            Address = address;
            City = city;
            Region = region;
            Specialty = specialty;
        }
        public University(University another)
        {
            ObservableCollection<Specialty> refSpecialty = new ObservableCollection<Specialty>();
            try
            {
                for (int i = 0; i < another.Specialty.Count; i++)
                {
                    refSpecialty .Add((Specialty)another.Specialty[i].clone());
                }
            }
            catch (Exception e) { }
            Id = another.Id;
            Name = another.Name;
            FullName = another.FullName;
            Address = another.Address;
            City = another.City;
            Region = another.Region;
            Specialty=refSpecialty;
        }
        public Object clone()
        {
            return new University(this);
        }
    }
}

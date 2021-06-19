using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using Coursework.DBUtils;
using Coursework.Model;

namespace Coursework.ViewModel
{
    internal class UniversityInfoViewModel : ViewModelBase
    {
        #region Variables

        private University _university;
        public University University
        {
            get => _university;
            set => Set(ref _university, value);
        }

        private string _type="Не указано";
        public string Type
        {
            get => _type;
            set => Set(ref _type, value);
        }

        private string _form="Не указано";
        public string Form
        {
            get => _form;
            set => Set(ref _form, value);
        }

        private string _fromAvgScore;
        public string FromAvgScore
        {
            get => _fromAvgScore;
            set => Set(ref _fromAvgScore, value);
        }

        private string _toAvgScore;
        public string ToAvgScore
        {
            get => _toAvgScore;
            set => Set(ref _toAvgScore, value);
        }

        private string _fromPrice;
        public string FromPrice
        {
            get => _fromPrice;
            set => Set(ref _fromPrice, value);
        }

        private string _toPrice;
        public string ToPrice
        {
            get => _toPrice;
            set => Set(ref _toPrice, value);
        }

        private ObservableCollection<Specialty> _Specialties;

        public ObservableCollection<Specialty> Specialties
        {
            get => _Specialties;
            set => Set(ref _Specialties, value);
        }

        #endregion

        #region CommandBack

        public ICommand Back { get; }
        private bool CanBackExecute(object p) => true;

        private void OnBackExecuted(object p)
        {
            Navigation.SwitchView(typeof(UniversitiesListViewModel), "Университеты");
        }

        #endregion

        #region CommandApplyFilter

        public ICommand ApplyFilter { get; }
        private bool CanApplyFilterExecute(object p) => true;

        private void OnApplyFilterExecuted(object p)
        {
            double fromScore = string.IsNullOrEmpty(FromAvgScore) ? 0 : double.Parse(FromAvgScore.Replace(".", ","));
            double toScore = string.IsNullOrEmpty(ToAvgScore) ? 5 : double.Parse(ToAvgScore.Replace(".", ","));
            double fromPrice = string.IsNullOrEmpty(FromPrice) ? 0 : Int32.Parse(FromPrice);
            double toPrice = string.IsNullOrEmpty(ToPrice) ? Int32.MaxValue : Int32.Parse(ToPrice);
            string typeCompare = string.IsNullOrEmpty(Type) ? null : Type;
            string formCompare = string.IsNullOrEmpty(Type) ? null : Form;
            if (!string.IsNullOrEmpty(typeCompare))
            {
                typeCompare = typeCompare == "Не указано" ? null : Type;
            }

            if (!string.IsNullOrEmpty(formCompare))
            {
                formCompare = formCompare == "Не указано" ? null : Form;
            }


            if (fromPrice == 0 && toPrice == Int32.MaxValue && typeCompare == null && formCompare == null && fromScore == 0 && toScore == 5)
            {
                DropFilter();
                return;
            }
            var specialties = new ObservableCollection<Specialty>();
            foreach (var specialty in University.Specialty)
            {
                foreach (var specificSpecialty in specialty.NowSpec)
                {
                    double avgScore = double.Parse(specificSpecialty.AvgScore);
                    int price = specificSpecialty.Price;
                    string type = specificSpecialty.Type;
                    string form = specificSpecialty.Form;
                    bool istypeselected = (_type != null)&&(_type != "Не указано");
                    bool isformselected = (_form != null)&&(_form != "Не указано");
                    typeCompare = istypeselected ? typeCompare : type;
                    formCompare = isformselected ? formCompare : form;

                    if (price >= fromPrice && price <= toPrice && avgScore >= fromScore && avgScore <= toScore && formCompare == form && typeCompare == type)
                    {
                        var foundedSpecialty = (Specialty)specialty.clone();
                        var specs = foundedSpecialty.NowSpec;
                        foundedSpecialty.SpecSpecialty = new ObservableCollection<SpecificSpecialty>();
                        foreach (var specificSpecialty1 in specs)
                        {
                            double avgScore2 = double.Parse(specificSpecialty1.AvgScore);
                            int price2 = specificSpecialty1.Price;
                            string type2 = specificSpecialty1.Type;
                            string form2 = specificSpecialty1.Form;
                            typeCompare = istypeselected ? typeCompare : type2;
                            formCompare = isformselected ? formCompare : form2;
                            int newComparePrice = specificSpecialty1.Price;
                            if (price2 >= fromPrice && price2 <= toPrice && avgScore2 >= fromScore &&
                                avgScore2 <= toScore && formCompare == form2 && typeCompare == type2)
                            {
                                foundedSpecialty.SpecSpecialty.Add(specificSpecialty1);
                            }
                        }
                        var i = foundedSpecialty.NowSpec.Count;
                        var j = foundedSpecialty.OldSpec.Count;
                        specialties.Add(foundedSpecialty);
                        break;
                    }
                }
            }
            Specialties = specialties;
        }

        #endregion
      
        #region Ctor

        public UniversityInfoViewModel(object nav, object[] args) : base(nav)
        {
            University = args[0] as University;
            Specialties = University.Specialty;

            Back = new Action(OnBackExecuted, CanBackExecute);
            ApplyFilter = new Action(OnApplyFilterExecuted, CanApplyFilterExecute);
        }

        public override ViewModelBase Switch(object[] args)
        {
            University = args[0] as University;
            Specialties = University.Specialty;
            return this;
        }

        #endregion

        #region Methods

        #region Filter

         private void DropFilter()
        {
            Specialties = new ObservableCollection<Specialty>();
            Specialties = University.Specialty;
        }

        #endregion

        #endregion
    }
}
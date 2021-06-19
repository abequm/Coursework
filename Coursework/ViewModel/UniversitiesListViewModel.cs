using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;
using Coursework.DBUtils;
using Coursework.Model;
using Coursework.Navigation;

namespace Coursework.ViewModel
{
    internal class UniversitiesListViewModel : ViewModelBase
    {
        #region Variables

        private ObservableCollection<University> _Universities;
        public ObservableCollection<University> Universities
        {
            get
            {
                if (_Universities == null)
                {
                    _Universities = Mysql.ExecuteSql("SELECT * FROM coursework.universitysview;").ToUniversities();
                }
                return _Universities;
            }
            set => Set(ref _Universities, value);
        }
        private ObservableCollection<University> _universitiesSearch;
        public ObservableCollection<University> universitiesSearch
        {
            get
            {
                if (_universitiesSearch != null)
                    return _universitiesSearch;
                return Universities;
            }
            set => Set(ref _universitiesSearch, value);
        }
        public ObservableCollection<object> SelectedListItem
        {
            set => SelectedListItem = value;
        }

        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set
            {
                Set(ref _searchText, value);
                if (!string.IsNullOrEmpty(_searchText))
                {
                    Search();
                    return;
                }
                universitiesSearch = null;
                return;
            }
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

        #region CommandSeeMore

        public ICommand SeeMore { get; }
        private bool CanSeeMoreExecute(object p) => true;

        private void OnSeeMoreExecuted(object p)
        {
            if (p.Equals(null)) return;
            Navigation.SwitchView(typeof(UniversityInfoViewModel), Constants.Titles.UInfoView, new[] { p });
        }

        #endregion

        #region CommandReport

        public ICommand Report { get; }
        private bool CanReportExecute(object p) => true;

        private void OnReportExecuted(object p)
        {
            Interop.Word.Create((University)p);
        }

        #endregion

        #endregion

        #region Ctor

        public UniversitiesListViewModel(object nav, object[] args) : base(nav)
        {
            SeeMore = new Action(OnSeeMoreExecuted, CanSeeMoreExecute);
            Back = new Action(OnBackExecuted, CanBackExecute);
            Report = new Action(OnReportExecuted, CanReportExecute);
        }

        #endregion

        #region Methods

        private void Search()
        {
            string compareStr = SearchText.ToLower();
            ObservableCollection<University> findedUniversities = new ObservableCollection<University>();
            universitiesSearch = null;
            foreach (var compareUniversity in universitiesSearch)
            {
                bool isCompared = false;
                if (compareUniversity.Name.ToLower().Contains(compareStr) ||
                    compareUniversity.FullName.ToLower().Contains(compareStr) ||
                    compareUniversity.Address.ToLower().Contains(compareStr) ||
                    compareUniversity.City.ToLower().Contains(compareStr) ||
                    compareUniversity.Region.ToLower().Contains(compareStr))
                {
                    isCompared = true;
                }
                foreach (var specialty in compareUniversity.Specialty)
                {
                    foreach (PropertyInfo prop in typeof(Specialty).GetProperties())
                    {
                        if (prop.Name == "SpecSpecialty")
                        {
                            foreach (var specificSpecialty in (ObservableCollection<SpecificSpecialty>)prop.GetValue(specialty))
                            {
                                foreach (PropertyInfo prop2 in typeof(SpecificSpecialty).GetProperties())
                                {
                                    string field = (prop2.GetValue(specificSpecialty) ?? "").ToString().ToLower();
                                    if (field.Contains(compareStr))
                                        isCompared = true;
                                }
                            }
                        }
                        else
                        {
                            string field = (prop.GetValue(specialty) ?? "").ToString().ToLower();
                            if (field.Contains(compareStr))
                                isCompared = true;
                        }
                    }
                }

                if (isCompared)
                {
                    findedUniversities.Add(compareUniversity);
                }
            }
            if (findedUniversities.Count > 0)
            {
                universitiesSearch = findedUniversities;
            }
            else
            {

                universitiesSearch = new ObservableCollection<University>();
            }


        } 

        #endregion
    }
}
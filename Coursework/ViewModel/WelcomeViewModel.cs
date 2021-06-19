using System.Linq.Expressions;
using System.Windows.Input;
using Coursework.ViewModel;
using Coursework.Navigation;

namespace Coursework.ViewModel
{
    internal class WelcomeViewModel : ViewModelBase
    {

        #region Commands

        #region CommandNavigateAuthorization

        public ICommand NavigateAuthorization { get; }
        private bool CanNavigateAuthorizationExecute(object p) => true;

        private void OnNavigateAuthorizationExecuted(object p)
        {
            Navigation.SwitchView(typeof(AuthorizationViewModel), Constants.Titles.AuthorizationView);
        }

        #endregion

        #region CommandNavigateUniversitysList

        public ICommand NavigateUniversitysList { get; }
        private bool CanNavigateUniversitysListExecute(object p) => true;

        private void OnNavigateUniversitysListExecuted(object p)
        {
            Navigation.SwitchView(typeof(UniversitiesListViewModel), Constants.Titles.UniversityListView);
        }

        #endregion

        #endregion

        #region Ctor

        public WelcomeViewModel(object nav, object[] args) : base(nav)
        {
            #region Commands

            NavigateAuthorization = new Action(OnNavigateAuthorizationExecuted, CanNavigateAuthorizationExecute);
            NavigateUniversitysList = new Action(OnNavigateUniversitysListExecuted, CanNavigateUniversitysListExecute);

            #endregion
        } 

        #endregion
    }
}
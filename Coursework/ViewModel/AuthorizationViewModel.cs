using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using Coursework.DBUtils;
using Coursework.Model;
using Coursework.Navigation;

namespace Coursework.ViewModel
{
    internal class AuthorizationViewModel : ViewModelBase
    {
        #region Variables

        private string _password;

        public string Password
        {
            get => _password;
            set
            {
                passwordError = "";
                Set(ref _password, value);
            }
        }

        private string _login;

        public string Login
        {
            get => _login;
            set
            {
                loginError = "";
                Set(ref _login, value);
            }
        }

        private string _loginError;

        public string loginError
        {
            get => _loginError;
            set => Set(ref _loginError, value);
        }

        private string _passwordError;

        public string passwordError
        {
            get => _passwordError;
            set => Set(ref _passwordError, value);
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

        #region CommandNavigateToUManage

        public ICommand NavigateToUManage { get; }
        private bool CanNavigateToUManageExecute(object p) => true;

        private void OnNavigateToUManageExecuted(object p)
        {
            if (string.IsNullOrEmpty(Password))
                passwordError = "Поле не заполнено";
            if (string.IsNullOrEmpty(Login))
            {
                loginError = "Поле не заполнено";
                return;
            }
            if (Mysql.ExecuteSql($"select * from administrators where administrators.login='{Login}'") is List<string> list)
            {
                if (list.Count == 0)
                {
                    loginError = "Некорректный логин";
                    return;
                }
                foreach (string row in list)
                {
                    if (row.Split('\n')[2] == Password)
                    {
                        var role = Mysql.ExecuteSql($"select role from universitysview where role='{int.Parse(row.Split('\n')[0])}';");
                        if (role.Count==0)
                        {
                            Navigation.SwitchView(typeof(SuperAdminViewModel), Constants.Titles.SuperAdminView,
                                role: int.Parse(row.Split('\n')[0]));
                            return;
                        }
                        Navigation.SwitchView(typeof(UManageViewModel), Constants.Titles.UManageView,
                            role: int.Parse(row.Split('\n')[0])); // correct pass
                        return;
                    }
                    else
                    {
                        passwordError = "Некорректный пароль";
                        return;// uncorrect pass
                    }
                }
            }

        }

        #endregion

        #region CommandNavigateToRegistration

        public ICommand NavigateToRegistration { get; }
        private bool CanNavigateToRegistrationExecute(object p) => true;

        private void OnNavigateToRegistrationExecuted(object p)
        {
            Navigation.SwitchView(typeof(RegistrationViewModel),Constants.Titles.RegistrationView);
        }

        #endregion

        #endregion


        public AuthorizationViewModel(object nav, object[] args) : base(nav)
        {
            #region Commands

            NavigateToUManage = new Action(OnNavigateToUManageExecuted, CanNavigateToUManageExecute);
            NavigateToRegistration = new Action(OnNavigateToRegistrationExecuted, CanNavigateToRegistrationExecute);
            Back = new Action(OnBackExecuted, CanBackExecute);

            #endregion

            loginError = "";
            passwordError = "";
        }
    }
}

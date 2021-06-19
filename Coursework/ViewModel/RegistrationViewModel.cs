using Coursework.DBUtils;
using System.Collections.Generic;
using System.Dynamic;
using System.Windows.Input;

namespace Coursework.ViewModel
{
    internal class RegistrationViewModel : ViewModelBase
    {
        #region Variables
        private string _login;
        public string login
        {
            get => _login;
            set => Set(ref _login, value);
        }
        private string _loginError;
        public string loginError
        {
            get => _loginError;
            set => Set(ref _loginError, value);
        }
        private string _password;
        public string password
        {
            get => _password;
            set => Set(ref _password, value);
        }
        private string _passwordError;
        public string passwordError
        {
            get => _passwordError;
            set => Set(ref _passwordError, value);

        }
        private string _name;
        public string name
        {
            get => _name;
            set => Set(ref _name, value);
        }
        private string _nameError;
        public string nameError
        {
            get => _nameError;
            set => Set(ref _nameError, value);
        }
        private string _fullname;
        public string fullname
        {
            get => _fullname;
            set => Set(ref _fullname, value);
        }
        private string _fullnameError;
        public string fullnameError
        {
            get => _fullnameError;
            set => Set(ref _fullnameError, value);

        }
        private string _city;
        public string city
        {
            get => _city;
            set => Set(ref _city, value);
        }
        private string _cityError;
        public string cityError
        {
            get => _cityError;
            set => Set(ref _cityError, value);

        }
        private string _region;
        public string region
        {
            get => _region;
            set => Set(ref _region, value);
        }
        private string _regionError;
        public string regionError
        {
            get => _regionError;
            set => Set(ref _regionError, value);

        }
        private string _address;
        public string address
        {
            get => _address;
            set => Set(ref _address, value);
        }
        private string _addressError;
        public string addressError
        {
            get => _addressError;
            set => Set(ref _addressError, value);
        }
        private string _registrateError;
        public string registrateError
        {
            get => _registrateError;
            set => Set(ref _registrateError, value);
        }

        private string Error = "Не заполнено";
        #endregion

        #region Commands

        #region CommandRegistrate

        public ICommand Registrate { get; }
        private bool CanRegistrateExecute(object p) => true;

        private void OnRegistrateExecuted(object p)
        {
            ClearErrors();
            if (string.IsNullOrEmpty(login))
            {
                loginError = Error;
            }
            if (string.IsNullOrEmpty(password))
            {
                passwordError = Error;
            }
            if (string.IsNullOrEmpty(region))
            {
                regionError = Error;
            }
            if (string.IsNullOrEmpty(city))
            {
                cityError = Error;
            }
            if (string.IsNullOrEmpty(address))
            {
                addressError = Error;
            }
            if (string.IsNullOrEmpty(name))
            {
                nameError = Error;
            }
            if (string.IsNullOrEmpty(fullname))
            {
                fullnameError = Error;
                return;
            }
            if (Mysql.ExecuteSql($"select login from administrators where administrators.login='{login}';") is List<string> list)
            {
                if (list.Count != 0)
                {
                    loginError = "Занято";
                    return;
                }

                var adminId = Mysql.ExecuteNonQuerySqlWithId($"insert into administrators(login,password) value('{login}','{password}');");
                var cityIdlist = Mysql.ExecuteSql($"select id from city where city='{city}';");
                var cityId = "";
                if (cityIdlist != null && cityIdlist.Count != 0)
                {
                    cityId = cityIdlist[0].Split('\n')[0];
                }
                var regionIdlist = Mysql.ExecuteSql($"select id from region where region='{region}';");
                var regionId = "";
                if (regionIdlist != null && regionIdlist.Count != 0)
                {
                    regionId = regionIdlist[0].Split('\n')[0];
                }
                if (string.IsNullOrEmpty(cityId))
                    cityId = Mysql.ExecuteNonQuerySqlWithId($"insert into city(city) value('{city}');");
                if (string.IsNullOrEmpty(regionId))
                    regionId = Mysql.ExecuteNonQuerySqlWithId($"insert into region(region) value('{region}');");
                var roleId =
                    Mysql.ExecuteNonQuerySqlWithId(
                        "insert into university(name,fullname,address,city,region,administrator) " +
                        $"value('{name}','{fullname}','{address}','{cityId}','{regionId}','{adminId}');");
                Navigation.SwitchView(typeof(UManageViewModel), Constants.Titles.UManageView, role: int.Parse(roleId));
            }

        }

        #endregion

        #region CommandBack

        public ICommand Back { get; }
        private bool CanBackExecute(object p) => true;

        private void OnBackExecuted(object p)
        {
            Navigation.SwitchView(typeof(WelcomeViewModel), Constants.Titles.WelcomeView, new[] { "Reset" });
        }

        #endregion

        #endregion

        public RegistrationViewModel(object nav, object[] args) : base(nav)
        {
            Back = new Action(OnBackExecuted, CanBackExecute);
            Registrate = new Action(OnRegistrateExecuted, CanRegistrateExecute);
            ClearErrors();
        }

        void ClearErrors()
        {
            addressError = "";
            passwordError = "";
            loginError = "";
            cityError = "";
            regionError = "";
            nameError = "";
            fullnameError = "";
        }
    }
}
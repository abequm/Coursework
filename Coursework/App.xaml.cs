using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Coursework.Navigation;
using Coursework.ViewModel;
using Coursework.View;

namespace Coursework
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            var nav = new NavigationCommand();
            var mw = new MainWindow()
            {
                DataContext = new MainViewModel(nav)
            };
            nav.SetMainVM(mw.DataContext as MainViewModel);
            nav.SwitchView(typeof(WelcomeViewModel),Constants.Titles.WelcomeView);
            mw.Show();
            base.OnStartup(e);
        }
    }
}

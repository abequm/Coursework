using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using Coursework.ViewModel;

namespace Coursework.Navigation
{
    class NavigationCommand
    {
        private MainViewModel MainVM;
        public int RoleId = 0;
        private IDictionary<Type, object> CachedViewModels = new Dictionary<Type, object>();
        public void SwitchView(Type viewModelType, string title = null, object[] args = null, int role = 0)
        {
            if (args != null && args.Contains("Reset"))
                ClearCache();
            if (CachedViewModels.ContainsKey(viewModelType))
                MainVM.SelectedViewModelBase = (CachedViewModels[viewModelType] as ViewModelBase).Switch(args);
            else
            {
                MainVM.SelectedViewModelBase = Activator.CreateInstance(viewModelType, new object[] { this, args }) as ViewModelBase;
                CachedViewModels[viewModelType] = MainVM.SelectedViewModelBase;
            }

            if (title != null)
                MainVM.Title = title;
            RoleId = role;
        }
        public void SetMainVM(MainViewModel mainVM)
        {
            MainVM = mainVM;
        }

        public void ClearCache()
        {
            CachedViewModels = new Dictionary<Type, object>();
        }

        public NavigationCommand()
        {

        }
    }
}
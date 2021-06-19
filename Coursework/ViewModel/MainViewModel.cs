using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Coursework.Navigation;
using Coursework.View;

namespace Coursework.ViewModel
{
    internal class MainViewModel : ViewModelBase
    {
        #region Variables

        #region Title

        private string _Title = "Справочник абитуриента";

        public string Title
        {
            get => _Title;
            set => Set(ref _Title, value);
        }

        #endregion

        #region SelectedView

        private ViewModelBase _selectedViewModelBase;

        public ViewModelBase SelectedViewModelBase
        {
            get => _selectedViewModelBase;
            set => Set(ref _selectedViewModelBase, value);
        }
        #endregion

        #region Navigation

        public ref NavigationCommand GetNavigation()
        {
            return ref Navigation;
        }

        #endregion

        #endregion

        public MainViewModel(object nav,object[] args=null) : base(nav)
        {

        }
    }
}


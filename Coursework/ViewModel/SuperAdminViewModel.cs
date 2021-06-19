using Coursework.Model;
using System.Collections.ObjectModel;

namespace Coursework.ViewModel
{
    internal class SuperAdminViewModel : ViewModelBase
    {
        #region Variables

        private University _selectedUniversity;
        public University SelectedUniversity
        {
            get => _selectedUniversity;
            set => Set(ref _selectedUniversity, value);
        }
        private ObservableCollection<University> _universities;
        public ObservableCollection<University> Universities
        {
            get => _universities;
            set => Set(ref _universities, value);
        }

        #endregion

        #region Commands

        #endregion

        #region Ctor
        public SuperAdminViewModel(object nav, object[] args) : base(nav)
        {
        }
        #endregion
    }
}
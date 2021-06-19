using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Coursework.Navigation;
using Coursework.View;

namespace Coursework.ViewModel
{

    #region ViewModelBase
    internal abstract class ViewModelBase : INotifyPropertyChanged
    {
        protected NavigationCommand Navigation;

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string PropertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }
        protected virtual bool Set<T>(ref T field, T value, [CallerMemberName] string PropertyName = null)
        {
            if (Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(PropertyName);
            return true;
        }


        public virtual ViewModelBase Switch(object[] args)
        {
            return this;
        }

        public ViewModelBase(object nav, object[] args = null)
        {
            Navigation = nav as NavigationCommand;
        }
    }
    #endregion

    #region Command

    #region IComandBase
    internal abstract class Command : ICommand
    {

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public abstract bool CanExecute(object parameter);

        public abstract void Execute(object parameter);
    }
    #endregion

    #region ComandRealisation
    internal class Action : Command
    {
        private readonly Action<object> _Execute;
        private readonly Func<object, bool> _CanExecute;
        public Action(Action<object> Execute, Func<object, bool> canExecute = null)
        {
            _Execute = Execute ?? throw new ArgumentNullException(nameof(Execute));
            _CanExecute = canExecute;
        }

        public override bool CanExecute(object parameter) => _CanExecute?.Invoke(parameter) ?? true;


        public override void Execute(object parameter) => _Execute(parameter);
    }
    #endregion

    #endregion

}

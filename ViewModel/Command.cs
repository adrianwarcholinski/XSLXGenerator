using System;
using System.Windows.Input;

namespace ViewModel
{
    public class Command : ICommand
    {
        public Command(Action execute) : this(execute, null) { }

        private readonly Action _execute;
        private readonly Func<bool> _canExecute;

        public Command(Action execute, Func<bool> canExecute)
        {
            this._execute = execute ?? throw new ArgumentNullException(nameof(execute));
            this._canExecute = canExecute;
        }
        public bool CanExecute(object parameter)
        {
            return _canExecute?.Invoke() ?? true;
        }

        public virtual void Execute(object parameter)
        {
            this._execute();
        }

        public event EventHandler CanExecuteChanged;

        internal void RaiseCanExecuteChanged()
        {
            this.CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
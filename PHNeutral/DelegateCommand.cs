using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PHNeutral
{
    /**
     * Standard XAML Binding Delegate Class
     * 
     * 
     **/
    class DelegateCommand<T> : ICommand
    {
        private readonly Action<T> execute;
        private readonly Func<T, bool> canExecute;

        public event EventHandler CanExecuteChanged;

        public DelegateCommand(Action<T> execute, Func<T, bool> canExecute)
        {
            if (execute == null) throw new ArgumentNullException("execute");
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public bool CanExecute(T parameter)
        {
            return this.canExecute != null && this.canExecute(parameter);
        }

        bool ICommand.CanExecute(object parameter)
        {
            return this.CanExecute((T)parameter);
        }

        public void Execute(T parameter)
        {
            this.execute(parameter);
        }

        void ICommand.Execute(object parameter)
        {
            this.Execute((T)parameter);
        }
    }
}

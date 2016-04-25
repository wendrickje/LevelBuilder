using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LevelBuilder.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class RelayCommand : ICommand
    {

        
		readonly Action<object> _execute;
		readonly Predicate<object> _canExecute;

		/// <summary>
		/// RelayCommand constructor
		/// </summary>
		public RelayCommand(Action<object> execute, Predicate<object> canExecute)
		{
			_execute = execute;
			_canExecute = canExecute;
		}

		/// <summary>
		/// RelayCommand constructor
		/// </summary>
		public RelayCommand(Action<object> execute) : this(execute, null) { }

		

		/// <summary>
		/// RelayCommand CanExecute implementation
		/// </summary>
		public bool CanExecute(object parameter)
		{
			return _canExecute == null || _canExecute(parameter);
		}

		/// <summary>
		/// RelayCommand Execute implementation
		/// </summary>
		public void Execute(object parameter)
		{
			_execute(parameter);
		}

        
        /// <summary>
        /// RelayCommand CanExecuteChanged implementation
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

    }
}

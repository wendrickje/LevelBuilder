using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LevelBuilder.ViewModel
{
    /// <summary>
    /// 
    /// </summary>
    public class Core : INotifyPropertyChanged
    {

        private bool _isLoading;
        
        /// <summary>
        /// 
        /// </summary>
        public bool IsLoading
        {
            get { return _isLoading; }
            set
            {
                _isLoading = value;
                OnPropertyChanged("IsLoading");
            }
        }


        #region INotifyPropertyChanged Members
        /// <summary>
        /// 
        /// </summary>
        /// <param name="prop"></param>
        protected void OnPropertyChanged(string prop)
        {
            var pch = PropertyChanged;
            if (pch == null) return;
            pch(this, new PropertyChangedEventArgs(prop));
        }


        /// <summary>
        /// 
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}

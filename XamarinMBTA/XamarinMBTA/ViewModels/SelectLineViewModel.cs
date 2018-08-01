using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace XamarinMBTA.ViewModels
{
    class SelectLineViewModel: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        
        private List<string>[] routes;
        public List<string>[] Routes
        {
            get
            {
                return this.routes;
            }
            set
            {
                routes = value;
                OnPropertyChanged(nameof(Routes));
            }
        }

        public SelectLineViewModel()
        {

        }


    }

}

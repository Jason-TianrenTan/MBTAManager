using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using XamarinMBTA.Globals;

namespace XamarinMBTA.ViewModels
{
    class MBTAViewModel: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        //
        private string _AlertTime;
        public string AlertTime
        {
            get
            {
                return this._AlertTime;
            }
            set
            {
                this._AlertTime = value;
                OnPropertyChanged(nameof(AlertTime));
            }
        }

        //
        private string _AlertCountLabel;
        public string AlertCountLabel
        {
            get
            {
                return this._AlertCountLabel;
            }
            set
            {
                this._AlertCountLabel = value;
                OnPropertyChanged(nameof(AlertCountLabel));
            }
        }

        //
        private string _AlertTitle;
        public string AlertTitle
        {
            get
            {
                return this._AlertTitle;
            }
            set
            {
                this._AlertTitle = value;
                OnPropertyChanged(nameof(AlertTitle));
            }
        }

        
        //Alert header
        //
        private string _AlertHeader;
        public string AlertHeader
        {
            get
            {
                return this._AlertHeader;
            }
            set
            {
                this._AlertHeader = value;
                OnPropertyChanged(nameof(AlertHeader));
            }
        }

        //Route
        //
        private string _RouteLabel;
        public string RouteLabel
        {
            get
            {
                return this._RouteLabel;
            }
            set
            {
                this._RouteLabel = value;
                OnPropertyChanged(nameof(RouteLabel));
            }
        }


        public MBTAViewModel()
        {
            AlertTime = "ALERT_TIME";
            AlertCountLabel = "Alerts(*)";
            AlertTitle = "ALERT_TITLE";
            AlertHeader = "ALERT HEADER";
            RouteLabel = "ROUTE";
            initData(); 
        }

        private void initData()
        {
            Task.Run(async () =>
            {
                Database.alertList = await DataQuery.getAlerts();
                
                if (Database.alertList.Count > 0)
                {
                    AlertTime = Database.alertList[0].attributes.updated_at.ToString("MM/dd/yyyy h:mm tt");
                    AlertCountLabel = "Alerts(" + Database.alertList.Count + ")";
                    AlertTitle = Database.alertList[0].attributes.effect;
                    AlertHeader = Database.alertList[0].attributes.short_header;
                    RouteLabel = Database.alertList[0].attributes.informed_entity[0].route;
                }
            }).Wait();
            
        }
    }

   
}

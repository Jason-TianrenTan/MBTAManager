using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using XamarinMBTA.Alerts;
using XamarinMBTA.Globals;

namespace XamarinMBTA.ViewModels
{
    class AlertDetailViewModel: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private int alertIndex = -1;
        private string _AlertTitle;
        public string AlertTitle
        {
            get
            {
                return _AlertTitle;
            }
            set
            {
                this._AlertTitle = value;
                OnPropertyChanged(nameof(AlertTitle));
            }
        }

        private string _AlertUpdateTime;
        public string AlertUpdateTime
        {
            get
            {
                return _AlertUpdateTime;
            }
            set
            {
                this._AlertUpdateTime = value;
                OnPropertyChanged(nameof(AlertUpdateTime));
            }
        }

        private string _LineColor;
        public string LineColor
        {
            get
            {
                return _LineColor;
            }
            set
            {
                this._LineColor = value;
                OnPropertyChanged(nameof(LineColor));
            }
        }

        private string _LineCode;
        public string LineCode
        {
            get
            {
                return _LineCode;
            }
            set
            {
                this._LineCode = value;
                OnPropertyChanged(nameof(LineCode));
            }
        }

        private string _AlertHeader;
        public string AlertHeader
        {
            get
            {
                return _AlertHeader;
            }
            set
            {
                _AlertHeader = value;
                OnPropertyChanged(nameof(AlertHeader));
            }
        }

        private string _AlertContent;
        public string AlertContent
        {
            get
            {
                return _AlertContent;
            }
            set
            {
                _AlertContent = value;
                OnPropertyChanged(nameof(AlertContent));
            }
        }


        public AlertDetailViewModel()
        {
            init();
        }


        public AlertDetailViewModel(int index)
        {
            init();
            setIndex(index);
        }

        private void init()
        {
            LineCode = "RL";
            LineColor = "#E51C23";
            AlertTitle = "Default Alert Title";
            AlertHeader = "Alert Header";
            AlertUpdateTime = "Last updated time";
            AlertContent = "ALERT DEFAULT CONTENT";
        }



        public void setIndex(int index)
        {
            alertIndex = index;
            Alert alert = Database.alertList[alertIndex];
            AlertTitle = alert.attributes.effect + "(" + alert.attributes.lifecycle + ")";
            AlertHeader = alert.attributes.header;
            AlertContent = alert.attributes.description;
            AlertUpdateTime = alert.attributes.updated_at.ToString("MM/dd/yyyy h:mm tt");
            string routeID = alert.attributes.informed_entity[0].route;
            var isNumeric = int.TryParse(routeID, out int n);
            if (isNumeric)
            {
                LineCode = "BUS";
                LineColor = Configs.lineColorMap[LineCode];
            }
            else
            {
                string LongName = Database.routeID_NameMap[routeID];
                if (Database.routeAbrevMap.ContainsKey(LongName))
                {
                    LineCode = Database.routeAbrevMap[LongName];
                    LineColor = Configs.lineColorMap[LineCode];
                }
                else
                {
                    LineCode = LongName;
                    LineColor = "#AEA29F";
                }
            }
        }

    }
}

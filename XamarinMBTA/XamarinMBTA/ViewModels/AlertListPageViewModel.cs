using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using XamarinMBTA.Alerts;
using XamarinMBTA.Globals;

namespace XamarinMBTA.ViewModels
{
    class AlertListPageViewModel: INotifyPropertyChanged
    {

        public class AlertDisplayModel
        {
            public int index { get; set; }
            public string update_time { get; set; }
            public string effect { get; set; }
            public string description { get; set; }
            public string LineCode { get; set; }
            public string LineColor { get; set; }
            public string header { get; set; }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public AlertListPageViewModel()
        {
            selectedLine = "All";
            displayAlertList = new ObservableCollection<AlertDisplayModel>();
        }

        public ObservableCollection<AlertDisplayModel> displayAlertList { get; set; }
        private string _selectedLine;
        public string selectedLine
        {
            get
            {
                return _selectedLine;
            }
            set
            {
                _selectedLine = value;
                OnPropertyChanged(nameof(selectedLine));
            }
        }


        private static AlertDisplayModel ConvertModel(Alert alert, int indx)
        {
            AlertDisplayModel ret = new AlertDisplayModel
            {
                effect = alert.attributes.effect,
                update_time = alert.attributes.updated_at.ToString("MM/dd/yyyy h:mm tt"),
                description = alert.attributes.description,
                header = alert.attributes.header,
                index = indx
            };
            string routeID = alert.attributes.informed_entity[0].route;
            if (!Database.routeID_NameMap.ContainsKey(routeID))
                return null;
            var isNumeric = int.TryParse(routeID, out int n);
            if (isNumeric)
            {
                ret.LineCode = "BUS";
                ret.LineColor = Configs.lineColorMap[ret.LineCode];
            }
            else
            {
                string LongName = Database.routeID_NameMap[routeID];
                if (Database.routeAbrevMap.ContainsKey(LongName))
                {
                    ret.LineCode = Database.routeAbrevMap[LongName];
                    ret.LineColor = Configs.lineColorMap[ret.LineCode];
                }
                else
                {
                    ret.LineCode = LongName;
                    ret.LineColor = "#AEA29F";
                }
            }
           
            return ret;
        }

        public void loadAlerts()
        { 
            displayAlertList = new ObservableCollection<AlertDisplayModel>();
            Task.Run(async () =>
            {
                if (!Database.routesLoaded)
                    Database.routeList = await DataQuery.getRoutes();
                if (!Database.alertsLoaded)
                    Database.alertList = await DataQuery.getAlerts();
               
            }).Wait();

            if (!selectedLine.Equals("All"))
            {
                for (int i = 0; i < Database.alertList.Count; i++)
                {
                    var alert = Database.alertList[i];
                    string routeID = alert.attributes.informed_entity[0].route;
                    if (Database.routeID_NameMap.ContainsKey(routeID))
                        if (Database.routeID_NameMap[routeID].Equals(selectedLine))
                            displayAlertList.Add(ConvertModel(alert, i));
                }
            }
            else for (int i = 0; i < Database.alertList.Count; i++)
                {
                    var result = ConvertModel(Database.alertList[i], i);
                    if (result != null)
                        displayAlertList.Add(result);
                }
            System.Diagnostics.Debug.WriteLine(displayAlertList.Count);
           
        }

    }
}

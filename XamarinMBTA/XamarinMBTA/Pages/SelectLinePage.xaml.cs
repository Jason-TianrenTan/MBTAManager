using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamarinMBTA.Alerts;
using XamarinMBTA.Globals;
using XamarinMBTA.UIViews;

namespace XamarinMBTA.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SelectLinePage : ContentPage
    {

        public class RouteModel {
            public string RouteName { get; set; }
            public string Resource { get; set; }
            public RouteModel(string routeName, string res)
            {
                RouteName = routeName;
                Resource = res;
            }
        }

        private string[] typeLabel = { "Subway", "Commuter Rail", "Bus", "Ferry" };
        private string[] resourceName = { "SUBWAY", "COMMUTER", "BUS", "FERRY"};
        private List<RouteModel> SubwayRouteModels;//stores route label name and icon resource
        private List<RouteModel>[] OtherRoutes;//stores route ids by type
        private void initLists()
        {

            OtherRoutes = new List<RouteModel>[typeLabel.Length];
            SubwayRouteModels = new List<RouteModel>();
            for (int i = 0; i < typeLabel.Length; i++)
                OtherRoutes[i] = new List<RouteModel>();

            foreach (Alert alert in Database.alertList)
            {
                string routeID = alert.attributes.informed_entity[0].route;
                if (Database.routeAlertCountMap.ContainsKey(routeID))
                    Database.routeAlertCountMap[routeID]++;
                else
                    Database.routeAlertCountMap.Add(routeID, 1);
            }


            foreach (var entry in Database.routeID_typeMap)
            {
                int type = entry.Value;
                string id = entry.Key;
                if (type == 0)
                    type = 1;
                type--;
                
                string labelName = Database.routeID_NameMap[id];
                int alert_count = 0;
                if (Database.routeAlertCountMap.ContainsKey(id) && Database.routeAlertCountMap[id] > 0)
                    alert_count = Database.routeAlertCountMap[id];
                if (alert_count > 0)
                {
                    string resource = "null";
                    if (type == 0)
                    {
                        string abrev = Database.routeAbrevMap[labelName];
                        resource = abrev + ".png";
                        SubwayRouteModels.Add(new RouteModel(labelName, resource));
                    }
                    else
                        OtherRoutes[type].Add(new RouteModel(labelName, null));
                }
                
            }

            

        }

        private Dictionary<ListView, int> listViewIndices;
        public SelectLinePage()
        {
            InitializeComponent();
            Task.Run(async () =>
            {
                if (!Database.routesLoaded)
                    Database.routeList = await DataQuery.getRoutes();
                if (!Database.alertsLoaded)
                    Database.alertList = await DataQuery.getAlerts();
            }).Wait();
            initLists();
            listViewIndices = new Dictionary<ListView, int>();

            for (int i = 0; i < 4; i++)
            {
                Label tLabel = new Label
                {
                    Text = typeLabel[i],
                    TextColor = Color.FromHex("#3F51B5"),
                    FontAttributes = FontAttributes.Bold,
                    FontSize = 28,
                    HorizontalTextAlignment = TextAlignment.Center,
                    VerticalTextAlignment = TextAlignment.Center,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center
                };

                BoxView splitter = new BoxView
                {
                    HeightRequest = 1,
                    Color = Color.LightGray
                };

                StackLayout stackLayout = new StackLayout
                {
                    Orientation = StackOrientation.Horizontal,
                    HeightRequest = 60
                };

                Image image = new Image
                {
                    HeightRequest = 35,
                    Source = resourceName[i]
                };
                stackLayout.Children.Add(image);
                stackLayout.Children.Add(tLabel);

                ListView tList = new ListView();
                tList.RowHeight = 40;
                tList.Margin = new Thickness(15, 0, 0, 0);
                listViewIndices.Add(tList, i);
                if (i == 0)
                {
                    tList.HeightRequest = SubwayRouteModels.Count * 40;
                    tList.ItemsSource = SubwayRouteModels;
                    tList.ItemTemplate = new DataTemplate(typeof(RouteViewCell));
                }
                else
                {
                    tList.HeightRequest = OtherRoutes[i].Count * 40;
                    tList.ItemsSource = OtherRoutes[i];
                    tList.ItemTemplate = new DataTemplate(typeof(OtherRouteCell));
                }

                tList.ItemSelected += (object sender, SelectedItemChangedEventArgs e) =>
                {
                    var item = e.SelectedItem as RouteModel;
                    int index = (tList.ItemsSource as List<RouteModel>).IndexOf(item);
                    int listIndex = listViewIndices[tList];
                    MessagingCenter.Send<App, string>
                        ((App)Xamarin.Forms.Application.Current, "LineSelected", item.RouteName);
                    Navigation.PopModalAsync();
                };

                MainStack.Children.Add(stackLayout);
                MainStack.Children.Add(splitter);
                MainStack.Children.Add(tList);
            }

            
		}

    }
}
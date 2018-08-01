using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace XamarinMBTA.UIViews
{
    class OtherRouteCell: ViewCell
    {
        public OtherRouteCell()
        {
            Label RouteLabel = new Label
            {
                FontSize = 22,
                TextColor = Color.FromHex("#7783CA"),
                Margin = new Thickness(40, 0, 0, 0),
                VerticalTextAlignment = TextAlignment.Center,
                HorizontalTextAlignment = TextAlignment.Start,
                HorizontalOptions = LayoutOptions.Start,
                VerticalOptions = LayoutOptions.Center
            };

            RouteLabel.SetBinding(Label.TextProperty, "RouteName");
            View = RouteLabel;
        }
    }
}

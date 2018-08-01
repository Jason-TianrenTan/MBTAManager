using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace XamarinMBTA.UIViews
{
    class RouteViewCell: ViewCell
    {

        public RouteViewCell()
        {
            var Image = new Image();
            Label RouteLabel = new Label
            {
                FontSize = 22,
                TextColor = Color.FromHex("#7783CA"),
                Margin = new Thickness(10, 0, 0, 0),
                VerticalTextAlignment = TextAlignment.Center,
                HorizontalTextAlignment = TextAlignment.Start,
                HorizontalOptions = LayoutOptions.Start,
                VerticalOptions = LayoutOptions.Center
            };
            RouteLabel.SetBinding(Label.TextProperty, "RouteName");
            Image.SetBinding(Image.SourceProperty, "Resource");

            StackLayout cellWrapper = new StackLayout
            {
                Orientation = StackOrientation.Horizontal
            };

            Image.HeightRequest = 30;
           
            cellWrapper.Children.Add(Image);
            cellWrapper.Children.Add(RouteLabel);
            View = cellWrapper;
        }

    }
}

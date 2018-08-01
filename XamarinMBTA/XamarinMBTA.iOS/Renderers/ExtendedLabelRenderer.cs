using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using XamarinMBTA;
using XamarinMBTA.iOS.Renderers;

[assembly: ExportRenderer(typeof(ExtendedLabel), typeof(ExtendedLabelRenderer))]
namespace XamarinMBTA.iOS.Renderers
{
    public class ExtendedLabelRenderer : LabelRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);

            if (Control != null && Element != null)
            {
                UpdateProperties((ExtendedLabel)Element);
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (sender != null)
            {
                UpdateProperties((ExtendedLabel)sender);
            }
        }

        private void UpdateProperties(ExtendedLabel extendedLabel)
        {
            if (!string.IsNullOrEmpty(extendedLabel.Text))
            {
                if (extendedLabel.MaxLines > 0)
                {
                    Control.Lines = extendedLabel.MaxLines;
                }

                LayoutSubviews();
            }
        }
    }
}
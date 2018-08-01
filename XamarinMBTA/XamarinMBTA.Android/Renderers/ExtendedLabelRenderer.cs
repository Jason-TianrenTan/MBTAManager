using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using XamarinMBTA;
using XamarinMBTA.Droid.Renderers;

[assembly: ExportRenderer(typeof(ExtendedLabel), typeof(ExtendedLabelRenderer))]
namespace XamarinMBTA.Droid.Renderers
{
    public class ExtendedLabelRenderer : LabelRenderer
    {
        public ExtendedLabelRenderer(Context context) : base(context)
        {
        }

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
                UpdateProperties((XamarinMBTA.ExtendedLabel)sender);
            }
        }

        private void UpdateProperties(ExtendedLabel extendedLabel)
        {
            if (extendedLabel.MaxLines > 0d)
            {
                Control.SetSingleLine(false);
                Control.SetMaxLines(extendedLabel.MaxLines);
            }
        }
    }
}
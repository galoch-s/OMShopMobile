using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;
using XFormsRadioButton.Android.Renderer;
using Xamarin.Forms.Platform.Android;
using OMShopMobile.CustomControls;


[assembly: ExportRenderer(typeof(CustomRadioButton), typeof(RadioButtonRenderer))]
namespace XFormsRadioButton.Android.Renderer
{

   //  using NativeRadioButton = RadioButton;

    public class RadioButtonRenderer: ViewRenderer<CustomRadioButton, RadioButton>
    {
        protected override void OnElementChanged(ElementChangedEventArgs<CustomRadioButton> e)
        {
            base.OnElementChanged(e);

            if(e.OldElement != null)
            {
                e.OldElement.PropertyChanged += ElementOnPropertyChanged;  
            }

            if(this.Control == null)
            {
                var radButton = new RadioButton(this.Context);
                radButton.CheckedChange += radButton_CheckedChange;
              
                this.SetNativeControl(radButton);
            }

            Control.Text = e.NewElement.Text;
            Control.Checked = e.NewElement.Checked;

            Element.PropertyChanged += ElementOnPropertyChanged;

			if (e.NewElement != null) {
				CustomRadioButton Content = e.NewElement as CustomRadioButton;
				SetTextColor (Content);
				SetTextSize (Content);
				this.Control.SetButtonDrawable (this.Resources.GetDrawable ("grey_checkbox_.png"));

				//if (Build.VERSION.SdkInt == BuildVersionCodes.JellyBean)
				//	this.Control.SetPadding (60, 15, 10, 15);
				//else
				//	this.Control.SetPadding (30, 15, 10, 15);
			}
        }

		private void SetTextColor(CustomRadioButton Content)
		{
			this.Control.SetTextColor(Content.TextColor.ToAndroid());
		}

		private void SetTextSize(CustomRadioButton Content)
		{
			this.Control.SetTextSize(global::Android.Util.ComplexUnitType.Dip, Content.FontSize);
		}

        void radButton_CheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            this.Element.Checked = e.IsChecked;
        }

        void ElementOnPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "Checked":
                    Control.Checked = Element.Checked;
					if (Element.Checked)
					this.Control.SetButtonDrawable (this.Resources.GetDrawable ("green_checkbox_.png"));
					else
					this.Control.SetButtonDrawable (this.Resources.GetDrawable ("grey_checkbox_.png"));
                    break;
                case "Text":
                    Control.Text = Element.Text;
                    break;
            }
        }
    }
}
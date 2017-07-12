using System;
using Xamarin.Forms;

namespace OMShopMobile
{
	public class ConactsInfoView : ContentView
	{
		public ConactsInfoView ()
		{
			WebView browser = new WebView { VerticalOptions = LayoutOptions.FillAndExpand };
			HtmlWebViewSource htmlSource = new HtmlWebViewSource();
//			<link rel="stylesheet" href="default.css">
			htmlSource.Html = @"<html>
<head>
<style type=""text/css"">
	@font-face{ font-family: MyriadProRegular; src: url("+ Device.OnPlatform("Fonts/MyriadProRegular.ttf", "MyriadProRegular.ttf", "MyriadProRegular.ttf") +@")}
   body,p {
	color: #333333;
	font-size: " + Utils.GetSize(14) + @"px;
  	font-family: MyriadProRegular;
  	text-align: justify;
}
h3 {
	font-size: " + Utils.GetSize(14) + @"px;
	margin-bottom: 5px;
	color: #009F9C;
}
.green {
	color: #009F9C;
}
  </style>
</head>
<body>
<h3>Наши контакты</h3>
Уважаемые клиенты! <br>
Используя приведенные ниже возможности Вы можете проконсультироваться по всем вопросам работы сайта, проведения оплаты, отгрузки товара. <br><br>
График работы специалистов контактного центра &laquo;Одежда-Мастер&raquo;: пн.-пт.: с 9-00 до 18-00. <br><br>
</head><body>";
			browser.Source = htmlSource;


			MyLabel lbl1 = new MyLabel { TextColor = ApplicationStyle.RedColor, FontAttributes = FontAttributes.Bold, LineSpacing = 2f, Text = "Наши контакты",  };
			MyLabel lbl2 = new MyLabel { TextColor = ApplicationStyle.TextColor, LineSpacing = 2f, Text = "Уважаемые клиенты!" };
			MyLabel lbl3 = new MyLabel { TextColor = ApplicationStyle.TextColor,
				LineSpacing = 1.2f,
				Text = "Используя приведенные ниже возможности Вы можете проконсультироваться по всем вопросам работы сайта, проведения оплаты, отгрузки товара.", 
				 };
			MyLabel lbl4 = new MyLabel { TextColor = ApplicationStyle.TextColor,
				LineSpacing = 1.2f,
				Text = "График работы специалистов контактного центра «Одежда-Мастер»: пн.-пт.: с 9-00 до 18-00.",  };

			StackLayout layoutContacts = new StackLayout { 
				Spacing = 10,
				Children = {
					GetContact(Device.OnPlatform("Info/skype.png", "skype.png", "skype.png"), "Skype для связи", "odezhda-master1", "skype:odezhda-master1?chat"),
					GetContact(Device.OnPlatform("Info/phone.png", "phone.png", "phone.png"), "Телефон горячей линии", "+7 (495) 204-1583", "tel:+79109960134"),
					GetContact(Device.OnPlatform("Info/phone.png", "phone.png", "phone.png"), "Для клиентов из Москвы и МО", "+7 (910) 996-0134", "tel:+79109960134"),
				}
			};
			StackLayout layoutMain = new StackLayout {
				Spacing = 0,
				Padding = new Thickness(8, 8),
				Children = {
					//					browser,
					lbl1,
					new Label { HeightRequest = 8, Text = " "},
					lbl2,
					new Label { HeightRequest = 8, Text = " "},
					lbl3,
					new Label {Text = " "},
					lbl4,
					new Label {Text = " "},
					layoutContacts
				}
			};
			Content = layoutMain;
		}

		StackLayout GetContact(string imgName, string title, string contact, string url)
		{
			Image img = new Image { Source = imgName };
			Label lblContact = new Label { Text = contact, TextColor = ApplicationStyle.TextColor };

			StackLayout layoutDesc = new StackLayout { 
				Padding = new Thickness(8, 0),
				Spacing = 0,
				Children = {
					new Label { Text = title, TextColor = ApplicationStyle.TextColor },
					lblContact,
				}
			};
			StackLayout layoutBody = new StackLayout { 
				Orientation = StackOrientation.Horizontal,
				Children = {
					img,
					layoutDesc
				}
			};
			TapGestureRecognizer tap = new TapGestureRecognizer();
			tap.CommandParameter = url;
			tap.Tapped += GoToUrl;
			layoutBody.GestureRecognizers.Add (tap);
			return layoutBody;
		}

		void GoToUrl (object sender, EventArgs e)
		{
			TappedEventArgs evnt = e as TappedEventArgs;
			try {
				if (Device.OS != TargetPlatform.WinPhone) {
					Device.OpenUri (new Uri ((string)evnt.Parameter));
				}
			}
			catch {
			}
		}
	}
}
//<table border="0">
//	<tr>
//	<td align="center"><img src="skype.png" width="32" height="32" border="0" alt="Skype" title="Skype для связи"></td>
//	<td><b>Skype:</b><br><div><a href="skype:odezhda-master1" target="_blank">odezhda-master1</a>odezhda-master1</div>
//	</td>
//	</tr>
//	<tr>
//	<td><img src="phone.png" width="32" height="32" border="0" alt="Телефон горячей линии" title="Телефон горячей линии"></td>
//	<td><b>Телефон горячей линии:</b>
//	<div><a href="tel:1855XAMARIN" target="_blank">+7&nbsp;(495)&nbsp;204-1583</a></div>
//</td>
//</tr>
//<tr>
//<td align="center"><img src="phone.png" width="32" height="32" border="0" alt="Для клиентов из Москвы и МО" title="Для клиентов из Москвы и МО"></td>
//	<td><b>Для клиентов из Москвы и МО</b>
//	<div>+7&nbsp;(910)&nbsp;996-0134</div>
//</td>
//</tr>
//</table>
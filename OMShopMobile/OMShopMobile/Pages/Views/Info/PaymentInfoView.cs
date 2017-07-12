using System;
using Xamarin.Forms;

namespace OMShopMobile
{
	public class PaymentInfoView : ContentView
	{
		public PaymentInfoView ()
		{
			WebView browser = new WebView();
			HtmlWebViewSource htmlSource = new HtmlWebViewSource();
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
<h3>Оплата товаров</h3>
Оплату счетов рекомендуем производить не позднее чем через 3-5 дней с момента получения счета на последний дозаказ. В случае&nbsp;<strong>просрочки платежа</strong>&nbsp;просим уведомить нас об этом!<br><br>"+
"Оплату нескольких счетов рекомендуем производить&nbsp;<strong>"+
"одной суммой</strong>. При оплате заказов просим&nbsp;<strong>ОБЯЗАТЕЛЬНО</strong>&nbsp;указывать&nbsp;<strong>в назначении платежа номера оплачиваемых "+
"заказов и оплачивать по реквизитам , указанным у вас в счете (БУДЬТЕ ВНИМАТЕЛЬНЫ!).<br><br> " +
"Об оплате</strong>&nbsp;необходимо&nbsp;<strong>уведомить</strong>&nbsp;нас"+
" на адрес нашей электронной почты. В теме письма необходимо указать «Оплата»- и указать отправлять ли Ваш заказ. <br><br>"+
"Перечисленные Вами деньги могут идти на наш расчетный счет&nbsp;<strong>до 3-х рабочих дней включительно</strong>. При получении нами Вашей оплаты, "+
				"статус Вашего заказа будет изменен на ОПЛАЧЕНО, о чем Вы получите уведомление на электронную почту.</head><body>";
			browser.Source = htmlSource;
			Content = browser;

//			Padding = new Thickness (8, 8);
//			Label lblName = new Label { 
//				Text = "Оплата", 
//				TextColor = ApplicationStyle.GreenColor 
//			};
//			Label lblDescription = new Label { 
//				Text = @"Оплату счетов рекомендуем производить не позднее чем через 3-5 дней с момента получения счета на последний дозаказ." + 
//"В случае просрочки платежа просим уведомить нас об этом! Оплату нескольких счетов рекомендуем производить одной суммой. " +
//"При оплате заказов просим ОБЯЗАТЕЛЬНО указывать в назначении платежа номера оплачиваемых заказов и оплачивать по реквизитам , указанным у вас в счете (БУДЬТЕ ВНИМАТЕЛЬНЫ!). " +
//"Об оплате необходимо уведомить нас на адрес нашей электронной почты. В теме письма необходимо указать «Оплата»- и указать отправлять ли Ваш заказ. " +
//"Перечисленные Вами деньги могут идти на наш расчетный счет до 3-х рабочих дней включительно. " +
//"При получении нами Вашей оплаты, статус Вашего заказа будет изменен на ОПЛАЧЕНО, о чем Вы получите уведомление на электронную почту."
//			};
//
//			StackLayout mainLayout = new StackLayout {
//				Children = {
//					lblName,
//					lblDescription,
//				}
//			};
//			Content = mainLayout;
		}
	}
}


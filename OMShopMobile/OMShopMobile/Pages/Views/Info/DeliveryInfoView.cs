using System;
using Xamarin.Forms;

namespace OMShopMobile
{
	public class DeliveryInfoView : ContentView
	{
		public DeliveryInfoView ()
		{
			WebView browser = new WebView();
			HtmlWebViewSource htmlSource = new HtmlWebViewSource();
			htmlSource.Html = @"
<html>
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
<h3>Доставка товаров</h3>
<p style=""margin:0;padding:0"">Для отправки груза из Москвы в регионы Вы можете воспользоваться услугами следующих транспортных компаний (<strong>оплата доставки осуществляется Вами при получении груза</strong>):<br><br></p>
	<p style=""text-align: left; margin:0; padding:0"">- ""Первая Экспедиционная компания"" <a href=""http://www.pecom.ru/ru/"" target=""_blank"">
			http://www.pecom.ru/ru/</a>&nbsp;(из г. Иваново)</p>
	- ""Деловые линии""&nbsp;<a href=""http://www.dellin.ru/"" target=""_blank"">http://www.dellin.ru/</a><br>
	- ""ОПТИМА"" <a href=""http://www.77-11.ru%20"">http://www.77-11.ru/</a><br>
	- ""КИТ"" <a href=""http://tk-kit.ru"">http://tk-kit.ru/</a><br>
	- ""ЭНЕРГИЯ"" <a href=""http://nrg-tk.ru"">http://nrg-tk.ru/</a><br>
	- ""Севертранс"" <a href=""http://severtrans-msk.ru/"">http://severtrans-msk.ru/</a><br>
	- ""Почта РФ"" <a href=""http://www.pochta.ru"">http://www.pochta.ru/</a><br><br>
	<p style=""margin:0;padding:0"">Оптима, Севертранс, - отправляем их раз в неделю по понедельникам. Энергия, Кит – отправляем ежедневно. Отправки ТК «Почта России» производятся 1 раз в неделю.<br><br>
Доставка товара&nbsp;<strong>ДО</strong>&nbsp;вышеуказанных транспортных компаний осуществляется бесплатно. Сопроводительные документы для ТК оформляются нами при подготовке Вашего заказа к отправке.<br><br>
	Также возможна доставка через ЕМС почту России&nbsp;<a href=""http://www.emspost.ru/ru/"" target=""_blank"">http://www.emspost.ru/ru/</a>&nbsp;. <br>
Отправка заказов через почту ЕМС осуществляется 1-2 раза в неделю (по мере накопления заказов). Оплата доставки осуществляется Вами при получении заказа.</p>
</head>
<body>";
			browser.Source = htmlSource;
			Content = browser;

			browser.Navigating += (sender, e) => {
				try {
					var uri = new Uri (e.Url);
					Device.OpenUri (uri);
				} catch (Exception) {
					
				}

				//e.Cancel = true;

			};
		}
	}
}


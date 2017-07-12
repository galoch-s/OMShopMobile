using System;
using Xamarin.Forms;
using System.Text.RegularExpressions;

namespace OMShopMobile
{
	public class ResetPasswordView : ContentView
	{
		ActivityIndicator activityIndicator;
		EntryExtension entEmail;
		StackLayout layoutMain;

		event EventHandler eventRefresh;

		public ResetPasswordView ()
		{
			entEmail = new EntryExtension { 
				Placeholder = "E-mail*",
				Keyboard = Keyboard.Email
			};

			Button btnLogin = new Button { Text = "Восстановить", HeightRequest = Utils.GetSize(43) };
			btnLogin.Clicked += (s, e) => {
				activityIndicator.IsRunning = true;
				ClickReset(s, entEmail.textBox.Text);
			};	

			Label lblMessage = new Label {
				TextColor = ApplicationStyle.PlaceholderColor,
				FontSize = Utils.GetSize(10),
				Text = "Укажите ваш емейл при регистрации и мы вышлем на него пароль"
			};

			StackLayout layoutButton = new StackLayout { 
				Spacing = 18,
				Padding = new Thickness(8, 10),
				Children = {
					lblMessage,
					btnLogin,
				}
			};

			activityIndicator = new ActivityIndicator
			{
				Color = Device.OnPlatform(Color.Black, Color.Gray, Color.Default),
				VerticalOptions = LayoutOptions.CenterAndExpand
			};
			layoutMain = new StackLayout { 
				Spacing = 0,
				Children = {
					entEmail,
					layoutButton,
					activityIndicator
				}
			};
			Content = layoutMain;
		}

		public async void ClickReset(object s, string email)
		{
			Content = layoutMain;

			string message = "";
			if (string.IsNullOrEmpty (email) || !Regex.IsMatch (email, Constants.RegexEmail)) {
				if (string.IsNullOrEmpty (email)) {
					message += "Введите " + entEmail.Placeholder.Trim(new char[] {'*'}) + "\r\n";
				} else {
					if (!Regex.IsMatch (email, Constants.RegexEmail))
						message += "Введите корректный Email\r\n";
				}
				message = message.Trim ();
				OnePage.mainPage.DisplayMessage (message);
				activityIndicator.IsRunning = false;
				return;
			}
			ContentAndHeads contentAndHeads;
			try {
				Content = layoutMain;
				contentAndHeads = await User.ReseltPasswordAsync (email);
			} catch (Exception) {
				eventRefresh = null;
				eventRefresh += (sender, e) => { 
					Button content = sender as Button;
					content.IsEnabled = false;
					ClickReset(s, email);
				};
				Content = OnePage.mainPage.ShowMessageError (eventRefresh);
				return;
			}
			if (contentAndHeads.requestStatus == System.Net.HttpStatusCode.OK) {
				OnePage.mainPage.DisplayMessage ("На ваш E-mail отправлено письмо с восстановлением пароля");
				OnePage.redirectApp.BackToHistory ();
			} else {
				switch (contentAndHeads.serverException.Code) {
				case 3: 
					OnePage.mainPage.DisplayMessage ("Пользователь не найден");
					break;
				default:
					OnePage.mainPage.DisplayMessage ("Не удалось отправить письмо с восстановлением пароля. Попробуте позже");
					break;
				}
			}
			activityIndicator.IsRunning = false;
		}
	}
}


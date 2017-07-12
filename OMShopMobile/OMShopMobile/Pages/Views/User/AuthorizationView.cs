using System;
using Xamarin.Forms;
using System.Text.RegularExpressions;

namespace OMShopMobile
{
	public class AuthorizationView : ContentView
	{
		public Button btnLogin;
		ActivityIndicator activityIndicator;

		public delegate void LoginEngineHandler(object sender, OperationEventArgs e) ;
		public LoginEngineHandler ButtonClick;

		public event EventHandler RegitrationClick;
		event EventHandler eventRefresh;

		EntryExtension entEmail;
		MyEntry entPassword;
		StackLayout layoutMain;

		public AuthorizationView ()
		{
			entEmail = new EntryExtension { 
				Placeholder = "E-mail*",
				Keyboard = Keyboard.Email,
			};

			entPassword = new MyEntry { 
				VerticalOptions = LayoutOptions.CenterAndExpand,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Placeholder = "Пароль*",
				IsPassword = true,
			};
			TapGestureRecognizer tapLayout = new TapGestureRecognizer();
			tapLayout.Tapped += (sender, e) => { entPassword.Focus(); };
			this.GestureRecognizers.Add(tapLayout);

			Button btnResetPassword = new Button { 
				TextColor = ApplicationStyle.GreenColor,
				BackgroundColor = Color.Transparent,
				FontSize = Utils.GetSize(14),
				Text = "Забыли?"
			};
			btnResetPassword.Clicked += OnClickReset;

			StackLayout layoutPassword = new StackLayout {
				HeightRequest = Utils.GetSize(43),
				Padding = new Thickness (8, 0),
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Orientation = StackOrientation.Horizontal,
				Children = {
					entPassword,
					btnResetPassword,
				}
			};

			StackLayout layoutBlockPassword = new StackLayout { 
				Spacing = 0,
				Children = {
					layoutPassword,
					new BoxView()
				}
			};


			btnLogin = new Button { Text = "Войти", HeightRequest = Utils.GetSize(43) };
			btnLogin.Clicked += (s, e) => {
				activityIndicator.IsRunning = true;
				ClickLogin(s, entEmail.textBox.Text, entPassword.Text );
			};

			Button btnGoToRegitration = new Button { 
				Text= "Зарегистрироваться",
				TextColor = ApplicationStyle.LabelColor,
				BackgroundColor = Color.White,
				HeightRequest = Utils.GetSize(43)
			};
			btnGoToRegitration.Clicked += (sender, e) => {
				RegitrationClick (this, null);
			};
			StackLayout layoutButton = new StackLayout { 
				Spacing = 0,
				Padding = new Thickness(8, 18),
				Children = {
					btnLogin,
					btnGoToRegitration
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
					layoutBlockPassword,
					layoutButton,
					activityIndicator
				}
			};

			Content = layoutMain;
		}

		public void GoToMain()
		{
			Content = layoutMain;
			entEmail.textBox.Text = "";
			entPassword.Text = "";
		}

		void OnClickReset (object sender, EventArgs e)
		{
			OnePage.redirectApp.AddTransition (PageName.Login, "Смена пароля");
			Content = new ResetPasswordView ();
		}

		public async void ClickLogin (object s, string email, string password)
		{
			Content = layoutMain;
			string message = "";
			if (string.IsNullOrEmpty (email) || !Regex.IsMatch (email, Constants.RegexEmail) || string.IsNullOrEmpty (password)) {
				if (string.IsNullOrEmpty (email)) {
					message += "Введите " + entEmail.Placeholder.Trim(new char[] {'*'}) + "\r\n";
				} else {
					if (!Regex.IsMatch (email, Constants.RegexEmail))
						message += "Введите корректный Email\r\n";
				}
				if (string.IsNullOrEmpty (password)) {
					message += "Введите " + entPassword.Placeholder.Trim(new char[] {'*'}) + "\r\n";
				}
				message = message.Trim ();
				OnePage.mainPage.DisplayMessage (message);
				activityIndicator.IsRunning = false;
				return;
			}
			btnLogin.IsEnabled = false;
			bool isLog;
			try {
				isLog = await User.LoginAsync (email, password);
			} catch (Exception ex) {
				if (eventRefresh == null)
					eventRefresh += (sender, e) => { 
					Button content = sender as Button;
					content.IsEnabled = false;
					ClickLogin (s, email, password);
				};
				Content = OnePage.mainPage.ShowMessageError (eventRefresh);
				return;
			}
			if (isLog) {
				message = "Вы успешно авторизованны";
				Session.SaveUser (User.Singleton);
			}
			else
				message = "Неверный логин/пароль";
			
			OnePage.mainPage.DisplayMessage (message);
			activityIndicator.IsRunning = false;

			ButtonClick (this, new OperationEventArgs (isLog));

			btnLogin.IsEnabled = true;
		}
	}
}


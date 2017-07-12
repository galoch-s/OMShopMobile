using System;
using Xamarin.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;

namespace OMShopMobile
{
	public class PersonalDataView : ScrollView
	{
		public Button btnLogin;

		StackLayout mainLayout;

		EntryExtension entFirstname;
		EntryExtension entLastname;
		PickerExtension pickerGender;
		EntryExtension entPhone;
		EntryExtension entStreet;
		EntryExtension entPostCode;
		EntryExtension entCity;
		PickerExtension pickerZone;
		PickerExtension pickerCountry;
		Label lblPassport;
		EntryExtension entPasportSeria;
		EntryExtension entPasportNomer;
		EntryExtension entPasportKemVidan;
		DatePickerExtension datePickerPasportKogdaVidan;

		EntryExtension entEmail;
		EntryExtension entPassword1;
		EntryExtension entPassword2;

		double OldThisHeight = 0;

		Dictionary<string, string> gender = new Dictionary<string, string>
		{
			{ "m", "Мужской" },
			{ "f", "Женский" },
		};

		private List<Zone> ZoneList;
		private List<Country> countriesList;
		bool IsRegistration { get; set;}

		event EventHandler eventRefresh;

		public event EventHandler SaveClick;

		void EditStringTextBox(object sender, TextChangedEventArgs e)
		{
			String val = e.NewTextValue;
			Entry entity = sender as Entry;

			if (!string.IsNullOrEmpty(val) && !Regex.IsMatch(val, Constants.RegexStringRU)) {
				entity.Text = e.OldTextValue; //Set the Old valu
			}
		}

		void EditPhoneTextBox(object sender, TextChangedEventArgs e)
		{
			String val = e.NewTextValue;
			Entry entity = sender as Entry;

			if (!string.IsNullOrEmpty(val) && !Regex.IsMatch(val, Constants.RegexPhone)) {
				entity.Text = e.OldTextValue; //Set the Old valu
			}
		}

		void EditNumberTextBox(object sender, TextChangedEventArgs e)
		{
			String val = e.NewTextValue;
			Entry entity = sender as Entry;

			if (!string.IsNullOrEmpty(val) && !Regex.IsMatch(val, Constants.RegexNumber)) {
				entity.Text = e.OldTextValue; //Set the Old valu
			}
		}

		public PersonalDataView (List<Zone> zoneList, bool isRegistration)
		{
//			Content = new MyEntry { Placeholder = "sdffsdfsdfds", BackgroundColor = Color.Red, VerticalOptions = LayoutOptions.Start };
//			BackgroundColor = Color.Red;
//			return;


//			(Forms.Context as Android.App.Activity).Window.SetSoftInputMode(Android.Views.SoftInput.AdjustNothing );

			VerticalOptions = LayoutOptions.Start;




			IsClippedToBounds = true;

			IsRegistration = isRegistration;
			ZoneList = zoneList;
			OnePage.bottomView.IsVisible = false;

			if (zoneList != null)
				countriesList = zoneList.Select (l => l.Countries).Distinct ().ToList<Country>();

			entFirstname = new EntryExtension { Placeholder = "Имя" };
			entFirstname.textBox.TextChanged += EditStringTextBox;
			entFirstname.textBox.SetBinding (MyEntry.TextProperty, "Address.Firstname");
//			entFirstname.Focused += (sender, e) => {
//				this.HeightRequest = 200;
//			};

			#region MyRegion

			entLastname = new EntryExtension { Placeholder = "Фамилия" };
			entLastname.textBox.TextChanged += EditStringTextBox;
			entLastname.textBox.SetBinding (MyEntry.TextProperty, "Address.Lastname");

			pickerGender = new PickerExtension { Placeholder = "Пол" };
			foreach (string value in gender.Values)	{
				pickerGender.textBox.Items.Add(value);
			}

			entPhone = new EntryExtension { Placeholder = "Телефон" };
			entPhone.textBox.TextChanged += EditPhoneTextBox;
			entPhone.textBox.Keyboard = Keyboard.Numeric;
			entPhone.textBox.SetBinding (MyEntry.TextProperty, "Phone");

			entStreet = new EntryExtension { Placeholder = "Улица" };
			//entStreet.textBox.TextChanged += EditStringTextBox;
			entStreet.textBox.SetBinding (MyEntry.TextProperty, "Address.Street");

			entPostCode = new EntryExtension { Placeholder = "Почтовый индекс" };
			entPostCode.textBox.TextChanged += EditNumberTextBox;
			entPostCode.textBox.Keyboard = Keyboard.Numeric;
			entPostCode.textBox.SetBinding (MyEntry.TextProperty, "Address.PostCode");

			entCity = new EntryExtension { Placeholder = "Город" };
			entCity.textBox.TextChanged += EditStringTextBox;
			entCity.textBox.SetBinding (MyEntry.TextProperty, "Address.City");


			pickerCountry = new PickerExtension { Placeholder = "Страна",  };
			pickerCountry.textBox.SelectedIndexChanged += OnSelectCountry;
			if (countriesList != null)
				foreach (Country item in countriesList)	{
					pickerCountry.textBox.Items.Add(item.Name);
				}

			pickerZone = new PickerExtension { Placeholder = "Регион", };

			lblPassport = new Label {
				Text = "Паспортные данные",
				VerticalOptions = LayoutOptions.CenterAndExpand,
				HorizontalOptions = LayoutOptions.Center,
			};
			StackLayout titlePassportLayout = new StackLayout {
				BackgroundColor = ApplicationStyle.LineColor,
				HeightRequest = Utils.GetSize(22),
				Padding = new Thickness(8, 0),
				Children = {
					lblPassport
				}	
			};

			entPasportSeria = new EntryExtension { Placeholder = "Серия" };
			entPasportSeria.textBox.TextChanged += EditNumberTextBox;
			entPasportSeria.textBox.Keyboard = Keyboard.Numeric;
			entPasportSeria.textBox.SetBinding (MyEntry.TextProperty, "Address.PasportSeria");

			entPasportNomer = new EntryExtension { Placeholder = "Номер" };
			entPasportNomer.textBox.TextChanged += EditNumberTextBox;
			entPasportNomer.textBox.Keyboard = Keyboard.Numeric;
			entPasportNomer.textBox.SetBinding (MyEntry.TextProperty, "Address.PasportNomer");

			entPasportKemVidan = new EntryExtension { Placeholder = "Кем выдан" };
			entPasportKemVidan.textBox.SetBinding (MyEntry.TextProperty, "Address.PasportKemVidan");

			datePickerPasportKogdaVidan = new DatePickerExtension { Format = "D", Placeholder = "Когда выдан" };

			entEmail = new EntryExtension { Placeholder = "E-mail", Keyboard = Keyboard.Email };
			entEmail.textBox.Keyboard = Keyboard.Email;
			entEmail.textBox.SetBinding (MyEntry.TextProperty, "Email");

			entPassword1 = new EntryExtension { Placeholder = "Пароль", IsPassword = true };
			entPassword1.textBox.SetBinding (MyEntry.TextProperty, "Password");

			entPassword2 = new EntryExtension { Placeholder = "Повторите пароль", IsPassword = true };
			entPassword2.textBox.SetBinding (MyEntry.TextProperty, "Password");


			StackLayout titleLoginLayout = new StackLayout {
				BackgroundColor = ApplicationStyle.LineColor,
				HeightRequest = Utils.GetSize(22),
				Padding = new Thickness(8, 0),
			};

			Button btnSave = new Button () { HeightRequest = Utils.GetSize(43) } ;
			if (isRegistration)
				btnSave.Text = "Зарегистрироваться";
			else
				btnSave.Text = "Зарегистрироваться";
			
			StackLayout layoutSave = new StackLayout { 
				Padding = new Thickness(8, 16),
				Children = {
					btnSave
				}
			};
			btnSave.Clicked += SaveData;

			mainLayout = new StackLayout {
				Spacing = 0,
				Children = {
					entFirstname,
					entLastname,
					pickerGender,
					entPhone,
					entStreet,
					entPostCode,
					entCity,
					pickerCountry,
					pickerZone,
					titlePassportLayout,
					entPasportSeria,
					entPasportNomer,
					entPasportKemVidan,
					datePickerPasportKogdaVidan,
					titleLoginLayout,
					entEmail,
					entPassword1,
					entPassword2,
					layoutSave,
					new StackLayout { HeightRequest = 300 }
				}
			};
			Content = mainLayout;
			#endregion
		

			foreach (var item in mainLayout.Children) {
				EntryExtension ent = item as EntryExtension;
				if (ent == null)
					continue;
				
				//ent.TextFocuced += OnTextFocused;
				//ent.TextUnFocuced += OnTextUnFocused;
			} 
		}

		async void OnTextFocused (object sender, EventArgs e)
		{
			//if (OldThisHeight == 0)
			//	OldThisHeight = this.Height;

			//this.HeightRequest = ((View)this.Parent).Height - this.Height * 0.43;

			//await this.ScrollToAsync(entPassword1, ScrollToPosition.End, true);

		}

		void OnTextUnFocused (object sender, EventArgs e)
		{
			//this.HeightRequest = OldThisHeight;
		}

		void OnSelectCountry (object sender, EventArgs e)
		{
			MyPicker element = sender as MyPicker;
			string value = element.Items [element.SelectedIndex];
			List<Zone> zoneOfCountiesList = ZoneList.Where (l => l.Countries.Name == value).ToList<Zone> ();
			zoneOfCountiesList = zoneOfCountiesList.OrderBy (l => l.Name).ToList();

			pickerZone.textBox.Items.Clear ();
			foreach (Zone item in zoneOfCountiesList) {
				pickerZone.textBox.Items.Add(item.Name);
			}
		}

		private bool Validate()
		{
			if (string.IsNullOrEmpty (entFirstname.textBox.Text?.Trim())) {
				OnePage.mainPage.DisplayMessage ("Введите " + entFirstname.Placeholder);
				return false;
			}

			if (string.IsNullOrEmpty (entLastname.textBox.Text?.Trim())) {
				OnePage.mainPage.DisplayMessage ("Введите " + entLastname.Placeholder);
				return false;
			}

			if (pickerGender.textBox.SelectedIndex == -1) {
				OnePage.mainPage.DisplayMessage ("Выберите " + pickerGender.Placeholder);
				return false;
			}

			if (string.IsNullOrEmpty (entPhone.textBox.Text?.Trim())) {
				OnePage.mainPage.DisplayMessage ("Введите " + entPhone.Placeholder);
				return false;
			}
			if (!Regex.IsMatch (entPhone.textBox.Text, Constants.RegexPhone)){
				OnePage.mainPage.DisplayMessage ("Введите корректный " + entPhone.Placeholder);
				return false;
			}

			if (string.IsNullOrEmpty (entStreet.textBox.Text?.Trim())) {
				OnePage.mainPage.DisplayMessage ("Введите " + entStreet.Placeholder);
				return false;
			}

			if (string.IsNullOrEmpty (entPostCode.textBox.Text?.Trim())) {
				OnePage.mainPage.DisplayMessage ("Введите " + entPostCode.Placeholder);
				return false;
			}
			if (!Regex.IsMatch (entPostCode.textBox.Text, @"\b[0-9]{6}\b")) {
				OnePage.mainPage.DisplayMessage ("Неправильный " + entPostCode.Placeholder + ".\r\nФормат XXXXXX");
				return false;
			}

			if (string.IsNullOrEmpty (entCity.textBox.Text?.Trim())) {
				OnePage.mainPage.DisplayMessage ("Введите " + entCity.Placeholder);
				return false;
			}

			if (pickerCountry.textBox.SelectedIndex == -1) {
				OnePage.mainPage.DisplayMessage ("Выберете " + pickerCountry.Placeholder);
				return false;
			}

			if (pickerZone.textBox.SelectedIndex == -1) {
				OnePage.mainPage.DisplayMessage ("Выберете " + pickerZone.Placeholder);
				return false;
			}

			if (string.IsNullOrEmpty (entPasportSeria.textBox.Text?.Trim())) {
				OnePage.mainPage.DisplayMessage ("Введите " + entPasportSeria.Placeholder);
				return false;
			}
			if (!Regex.IsMatch (entPasportSeria.textBox.Text, @"\b[0-9]{4}\b")) {
				OnePage.mainPage.DisplayMessage ("Неправильный " + entPasportSeria.Placeholder + ".\r\nФормат XXXX");
				return false;
			}

			if (string.IsNullOrEmpty (entPasportNomer.textBox.Text?.Trim())) {
				OnePage.mainPage.DisplayMessage ("Введите " + entPasportNomer.Placeholder);
				return false;
			}
			if (!Regex.IsMatch (entPasportNomer.textBox.Text, @"\b[0-9]{6}\b")) {
				OnePage.mainPage.DisplayMessage ("Неправильный " + entPasportNomer.Placeholder + ".\r\nФормат XXXXXX");
				return false;
			}

			if (string.IsNullOrEmpty (entPasportKemVidan.textBox.Text?.Trim())) {
				OnePage.mainPage.DisplayMessage ("Введите " + entPasportKemVidan.Placeholder);
				return false;
			}
			if (string.IsNullOrEmpty (datePickerPasportKogdaVidan.Text?.Trim())) {
				OnePage.mainPage.DisplayMessage ("Введите Когда выдан");
				return false;
			}

			if (string.IsNullOrEmpty (entEmail.textBox.Text?.Trim())) {
				OnePage.mainPage.DisplayMessage ("Введите " + entEmail.Placeholder);
				return false;
			}
			if (!Regex.IsMatch (entEmail.textBox.Text, Constants.RegexEmail)){
				OnePage.mainPage.DisplayMessage ("Введите корректный " + entEmail.Placeholder);
				return false;
			}

			if (string.IsNullOrEmpty (entPassword1.textBox.Text?.Trim())) {
				OnePage.mainPage.DisplayMessage ("Введите " + entPassword1.Placeholder);
				return false;
			}

			if (string.IsNullOrEmpty (entPassword2.textBox.Text?.Trim())) {
				OnePage.mainPage.DisplayMessage ("Введите " + entPassword2.Placeholder);
				return false;
			}

			if (entPassword1.textBox.Text != entPassword2.textBox.Text) {
				OnePage.mainPage.DisplayMessage ("Пароли не совпадают ");
				return false;
			}
			return true;
		}

		protected override void OnBindingContextChanged ()
		{
			base.OnBindingContextChanged ();

			User user = BindingContext as User;
			if (user.Address == null)
				return;
			if (!string.IsNullOrEmpty(user.Address.Gender))
				pickerGender.textBox.SelectedIndex = pickerGender.textBox.Items.IndexOf (gender[user.Address.Gender]);


			Country country = countriesList.SingleOrDefault (g => g.Id == user.Address.UserCountry);
			if (country != null) {
				pickerCountry.textBox.SelectedIndex = pickerCountry.textBox.Items.IndexOf (country.Name);
			
				string value = pickerCountry.textBox.Items [pickerCountry.textBox.SelectedIndex];
				List<Zone> zoneOfCountiesList = ZoneList.Where (l => l.Countries.Name == value).ToList<Zone> ();
				zoneOfCountiesList = zoneOfCountiesList.OrderBy (l => l.Name).ToList ();

				pickerZone.textBox.Items.Clear ();
				foreach (Zone item in zoneOfCountiesList) {
					pickerZone.textBox.Items.Add (item.Name);
				}

			
				pickerZone.textBox.SelectedIndex = 
					pickerZone.textBox.Items.IndexOf (zoneOfCountiesList.SingleOrDefault (g => g.Id == user.Address.UserZone).Name);

			}

			if (user.Address.PasportKogdaVidan != null && user.Address.PasportKogdaVidan != "0000-00-00" ) 
			{
				datePickerPasportKogdaVidan.textBox.Date = DateTime.Parse (user.Address.PasportKogdaVidan);
				datePickerPasportKogdaVidan.Placeholder = "";
			}
		}

		public async void SaveData (object sender, EventArgs e)
		{
			if (!Validate ()) return;
			
			Content = mainLayout;
			User user = (User)BindingContext;
			if (pickerGender.textBox.SelectedIndex != -1) {
				string genderValue = pickerGender.textBox.Items [pickerGender.textBox.SelectedIndex];
				user.Address.Gender = gender.SingleOrDefault (g => g.Value == genderValue).Key;
			}
			if (pickerZone.textBox.SelectedIndex != -1) {
				string zoneValue = pickerZone.textBox.Items [pickerZone.textBox.SelectedIndex];
				Zone zone = ZoneList.SingleOrDefault (g => g.Name == zoneValue);
				user.Address.UserZone = zone.Id;
				user.Address.UserCountry = zone.Countries.Id;
			}
			user.Address.PasportKogdaVidan = datePickerPasportKogdaVidan.textBox.Date.ToString("yyyy-MM-dd HH:mm");
			user.Firstname = user.Address.Firstname;
			user.Lastname = user.Address.Lastname;

			bool isSave = false;
			if (IsRegistration) {
				try {
					ContentAndHeads contentAndHeads = await User.Registration (user);
					if (contentAndHeads != null && contentAndHeads.requestStatus == System.Net.HttpStatusCode.OK)
						isSave = true;
					else {
						if (contentAndHeads.Content[0].Contains(Constants.ErrorRegistration))
							OnePage.mainPage.DisplayMessage("Email уже занят");
						else
							throw new Exception();
					}
					
				} catch (Exception) {
					if (eventRefresh == null)
						eventRefresh += (obj, evn) => { 
						Button content = sender as Button;
						content.IsEnabled = false;
						SaveData (sender, e);
					};
					Content = OnePage.mainPage.ShowMessageError (eventRefresh);
					return;
				}
			}
			else {
				try {
					isSave = await User.LoginAsync (user.Email, user.Password);
				} catch (Exception) {
					if (eventRefresh == null)
						eventRefresh += (obj, evn) => { 
						Button content = sender as Button;
						content.IsEnabled = false;
						SaveData (sender, e);
					};
					Content = OnePage.mainPage.ShowMessageError (eventRefresh);
					return;
				}

				if (isSave) {
					user.Email = null;
					user.Password = null;
					try {
						await User.SavePersonalData (user);
					} catch (Exception) {
						if (eventRefresh == null)
							eventRefresh += (obj, evn) => { 
							Button content = sender as Button;
							content.IsEnabled = false;
							SaveData (sender, e);
						};
						Content = OnePage.mainPage.ShowMessageError (eventRefresh);
						return;
					}
					user.HashKey = User.Singleton.HashKey;
					user.Email = User.Singleton.Email;
					user.Password = User.Singleton.Password;
				}
			}

			if (isSave) {
				User.Singleton = user;
				Session.SaveUser (user);
				if (IsRegistration) {
					OnePage.mainPage.DisplayMessage ("Вы успешно зарегистрированны");
					try {
						await User.LoginAsync (user.Email, user.Password);
					} catch (Exception) {
						if (eventRefresh == null)
							eventRefresh += (obj, evn) => { 
							Button content = sender as Button;
							content.IsEnabled = false;
							SaveData (sender, e);
						};
						Content = OnePage.mainPage.ShowMessageError (eventRefresh);
						return;
					}
				} else {
					OnePage.mainPage.DisplayMessage ("Вы успешно изменили данные");
//					await User.LoginAsync (user.Email, user.Password);
				}

				if (OnePage.redirectApp.GetCurrentTransition ().IsRedirectToBack) {
					OnePage.redirectApp.BackToHistory ();
					return;
				}

				if (SaveClick != null)
					SaveClick(this, null);
			}
		}
	}
}
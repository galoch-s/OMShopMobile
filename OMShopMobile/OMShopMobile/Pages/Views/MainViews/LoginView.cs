using System;

using Xamarin.Forms;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace OMShopMobile
{
	public class LoginView : ContentView
	{
		AuthorizationView loginTemplate;
		UserInfoView userInfoLayout;
		ActivityIndicator activityIndicator;

		event EventHandler eventRefresh;

		public LoginView ()
		{
			VerticalOptions = LayoutOptions.FillAndExpand;

			loginTemplate = new AuthorizationView ();
			loginTemplate.ButtonClick += ButtonLoginClick;
			loginTemplate.RegitrationClick += ButtonRegitrationClick;

			activityIndicator = new ActivityIndicator
			{
				Color = Device.OnPlatform(Color.Black, Color.Gray, Color.Default),
				IsRunning = true,
				VerticalOptions = LayoutOptions.CenterAndExpand,
			};
			Content = activityIndicator;

//			InitializationUserInfoAsync ();
			//AuthorizationOfSession ();
		}

		public void DataRefresh()
		{
			OrderRefresh ();
		}

		async void OrderRefresh()
		{
			try {
				User.Singleton.OrderStatusList = await OrderStatus.GetOrderStatusListAsync ();
			} catch (Exception) {
				if (eventRefresh == null)
					eventRefresh += (sender, e) => { 
					Button content = sender as Button;
					content.IsEnabled = false;
					OrderRefresh(); 
				};
				Content = OnePage.mainPage.ShowMessageError (eventRefresh);
				return;
			}
		}

		void InitializationUserInfoAsync()
		{
//			List<OrderStatus> orderStatus = await OrderStatus.GetOrderStatusListAsync ();
			userInfoLayout = new UserInfoView ();
			userInfoLayout.EventExitUser += ClickUserExit;
			userInfoLayout.EventPersonalData += PersonalDataClick;
			userInfoLayout.SetLayoutInfo ();
		}

		private void AuthorizationOfSession()
		{
			if (User.Singleton != null) {
				if (userInfoLayout == null)
					InitializationUserInfoAsync ();
				Content = userInfoLayout;
			} else {
				loginTemplate.GoToMain ();
				Content = loginTemplate;
			}
		}

		private void ClickUserExit (object sender, EventArgs e)
		{
			User.LoginExit ();
			OnePage.topView.lblPagePosition.Text = "Вход";
			OnePage.bottomView.IsVisible = false;
			userInfoLayout = null;
			loginTemplate.GoToMain();
			Content = loginTemplate;
			OnePage.topView.RefreshCountProduct();
		}

		public void ButtonLoginClick(object sender, OperationEventArgs e)
		{
			if (e.IsError) 
			{
				if (OnePage.redirectApp.GetCurrentTransition ().IsRedirectToBack) {
					OnePage.redirectApp.BackToHistory ();
					return;
				}
				if (userInfoLayout == null)
					InitializationUserInfoAsync ();

				Content = userInfoLayout;
				OnePage.bottomView.IsVisible = true;
				OnePage.redirectApp.GetCurrentTransition().CurrentPosition = "Мой профиль";
				OnePage.topView.lblPagePosition.Text = "Мой профиль";
				OnePage.topView.RefreshCountProduct();
			}
		}

		public void ButtonRegitrationClick(object sender, EventArgs e)
		{
			GoToPersonalData (true);
//			regitrationTemplate = new RegitrationTemplate ();
//			regitrationTemplate.RegistrationClick += RegistrationClick;
//			Content = regitrationTemplate;
		}

		private void RegistrationClick (object sender, EventArgs e)
		{
			Content = loginTemplate;
		}

		public void PersonalDataClick (object sender, EventArgs e)
		{
			GoToPersonalData (false);
		}

		public void GoToPersonalData(bool isRegistration)
		{
			if (isRegistration)
				OnePage.redirectApp.AddTransition (PageName.Login, "Зарегистрироваться", true);
			else
				OnePage.redirectApp.AddTransition (PageName.Login, "Мой профиль", true);

			Content = activityIndicator;
			OnePage.bottomView.IsVisible = false;
			GetPersonalData (isRegistration);
		}
		public async void GetPersonalData(bool isRegistration)
		{
			List<Zone> zoneList;
			try {
				zoneList = await Zone.GetZoneList ();
			} catch (Exception) {
				if (eventRefresh == null)
					eventRefresh += (sender, e) => { 
					Button content = sender as Button;
					content.IsEnabled = false;
					GetPersonalData(isRegistration); 
				};
				Content = OnePage.mainPage.ShowMessageError (eventRefresh);
				return;
			}
			//zoneList = zoneList.OrderBy(o=>o.Name).ToList();
			zoneList.RemoveAll (f => f.Name == "Все" && f.Id == 0);

			User user;
			if (User.Singleton != null) {
				try {
					user = await User.GetPersonalData ();
				} catch (Exception) {
					if (eventRefresh == null)
						eventRefresh += (sender, e) => { 
						Button content = sender as Button;
						content.IsEnabled = false;
						GetPersonalData(isRegistration); 
					};
					Content = OnePage.mainPage.ShowMessageError (eventRefresh);
					return;
				}
			} else
				user = new User ();
			
			if (user.Address == null)
				user.Address = new UserAddress ();
//			user.Email = "ya@ya.ya";
//			user.Password = "1";
//			user.Address.PasportKogdaVidan = DateTime.Now.AddDays(-12).ToString();
//			user.Address.Firstname = "sdfdsf";
//			user.Address.City = "sdfs";
//			user.Address.Gender = "m";
//			user.Address.Lastname = "222";
//			user.Address.PasportKemVidan = "sdf";
//			user.Address.PasportKogdaVidan = "11.11.2011";
//			user.Address.PasportNomer = "123123";
//			user.Address.PasportSeria = "1234";
//			user.Address.PostCode = "123123";
//			user.Address.Street = "asdas";
			PersonalDataView personalDataTemplate = new PersonalDataView (zoneList, isRegistration);
			personalDataTemplate.BindingContext = user;
			personalDataTemplate.SaveClick += SavePersonlDataClick;
			Content = personalDataTemplate;
		}

		private void SavePersonlDataClick (object sender, EventArgs e)
		{
			if (userInfoLayout == null)
				InitializationUserInfoAsync ();
			
			Content = userInfoLayout;
		}

		public void GotoPage(HistoryStep step)
		{
			if (User.Singleton == null) {
				loginTemplate.GoToMain();
				Content = loginTemplate;
				return;
			}

			if (userInfoLayout == null) {
				InitializationUserInfoAsync();
				Content = userInfoLayout;
				return;
			}
			Content = userInfoLayout;
			
			switch (step) {
			case HistoryStep.MyOrders:
				OnePage.topView.lblPagePosition.Text = "Мои заказы";
				userInfoLayout.GotoOrdersList (true);
				break;
			case HistoryStep.StatusOrders:
				OnePage.topView.lblPagePosition.Text = "Статус заказа";
				userInfoLayout.GotoStatusList (true);
				break;
			case HistoryStep.InfoOrder:
				OnePage.topView.lblPagePosition.Text = "Информация о заказе";
				userInfoLayout.GoToOrderResultView (true);
				break;
			case HistoryStep.OrdersList:
				OnePage.topView.lblPagePosition.Text = "Состав заказа";
				userInfoLayout.GoToOrdersList ();
				break;
			default:
				if (User.Singleton != null)
					userInfoLayout.SetMain ();
				break;
			}
		}
	}
}
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Xamarin.Forms;

namespace OMShopMobile
{
	public class Utils
	{
		public static string TruncateLongStringAtWord(string inputString, int maxChars, string postfix = "...")
		{
			if (maxChars <= 0)
				throw new ArgumentOutOfRangeException("maxChars");
			if (inputString == null || inputString.Length < maxChars)
				return inputString;

			int lastSpaceIndex = inputString.LastIndexOf(" ", maxChars);
			int substringLength = (lastSpaceIndex > 0) ? lastSpaceIndex : maxChars;
			string truncatedString = inputString.Substring(0, substringLength).Trim() + postfix;

			return truncatedString;
		}

		public static int GetSize(int size, double divider = 1.5)
		{
			return Device.Idiom == TargetIdiom.Phone ? size : (int)(size * App.ScaleWidth / divider);
		}

		public static double GetSize(double size, double divider = 1.5)
		{
			return Device.Idiom == TargetIdiom.Phone ? size : Math.Round ((size * App.ScaleWidth / divider), 2);
		}

		static string GetWeeks(List<Schedule> schedulesList, ref int numWeek)
		{
			string[] days = new string[] { "пн", "вт", "ср", "чт", "пт", "сб", "вс" };
			string strWeek = "";
			Schedule schedulePred = null;
			Schedule schedule = null;
			for (int i = numWeek; i < days.Length; i++) {
				schedule = schedulesList.FirstOrDefault(g => g.WeekDay == i);
				if (i == numWeek) {
					strWeek = days[i] + ".";
				}
				else if ((schedule != null && schedulePred == null) ||
				         (schedule == null && schedulePred != null) ||
				         (schedule != null && schedulePred != null && !schedule.IsEqualsTime(schedulePred))) {
						if (i - 1 != numWeek)
							strWeek += "-" + days[i-1] + ".";
					numWeek = i - 1;
					return strWeek;
				}
				schedulePred = schedule;
			}
			numWeek = days.Length - 1;
			return strWeek;
		}

		public static string GetProductAvailabilitySchedule(List<Schedule> schedulesList, Grid grid)
		{
			string[] days = new string[] { "пн", "вт", "ср", "чт", "пт", "сб", "вс"};
			//CultureInfo ci = new CultureInfo("ru-ru");
			//DateTimeFormatInfo dtfi = new DateTimeFormatInfo();
			//dtfi = ci.DateTimeFormat;
			string result = "";
			grid.Children.Clear();


			//string sss = GetWeeks(schedulesList, 0);

			//for (int i = 0; i < days.Length; i++) {
			int i = 0;
			int j = 0;
			while (i < days.Length)
			{
				string strWeeks = GetWeeks(schedulesList, ref i) + ":";
				//if (strWeeks.Length == 4)
				//	strWeeks += "        "; // 8 spaces
				result += strWeeks;
				grid.Children.Add(new Label { Text = strWeeks }, 0, j);
				//result += days[i] + ".:";
				Schedule schedule = schedulesList.FirstOrDefault(g => g.WeekDay == i);
				if (schedule != null) {
					if ((schedule.StartTime == 0 && schedule.StopTime == 0) || (schedule.StartTime == 86400 && schedule.StopTime == 86400)) {
						string ss = "не доступен для заказа";
						result += " " + ss + "\n";
						grid.Children.Add(new Label { Text = ss }, 1, j);
					} else {
						string strTime = "с " + new TimeSpan(0, 0, schedule.StartTime).ToString(@"hh\:mm")
							  + " до " + new TimeSpan(0, 0, schedule.StopTime).ToString(@"hh\:mm");
						result += " " + strTime + "\n";
						grid.Children.Add(new Label { Text = strTime }, 1, j);
					}
				} else {
					string ss = "доступен в любое время";
					result += " " + ss + "\n";
					grid.Children.Add(new Label { Text = ss }, 1, j);
				}
				i++;
				j++;
			}
			//for (i = 0; i < 7; i++) {
			//	grid.Children.Add(new Label { Text = days[i] }, 0, i);
			//	//result += dt.ToString("ddd", ci).ToLower() + ":\t";
			//	//result += days[i] + ".:";
			//	Schedule schedule = schedulesList.FirstOrDefault(g => g.WeekDay == i);
			//	if (schedule != null) {
			//		if ((schedule.StartTime == 0 && schedule.StopTime == 0) || (schedule.StartTime == 86400 && schedule.StopTime == 86400)) {
			//			//result += " не доступен для заказа\n";
			//			grid.Children.Add(new Label { Text = "заказы" }, 1, i);
			//			grid.Children.Add(new Label { Text = "не принимаются" }, 2, i);
			//		} else {
			//			//result += " с " + new TimeSpan(0, 0, schedule.StartTime).ToString(@"hh\:mm")
			//			//	  + " до " + new TimeSpan(0, 0, schedule.StopTime).ToString(@"hh\:mm") + "\n";

			//			grid.Children.Add(new Label { Text = "с " + new TimeSpan(0, 0, schedule.StartTime).ToString(@"hh\:mm") }, 1, i);
			//			grid.Children.Add(new Label { Text = "по " + new TimeSpan(0, 0, schedule.StopTime).ToString(@"hh\:mm") }, 2, i);
			//		}
			//	} else {
			//		//result += " доступен в любое время\n";
			//		grid.Children.Add(new Label { Text = "без" }, 1, i);
			//		grid.Children.Add(new Label { Text = "перерыва" }, 2, i);
			//	}
			//	//dt = dt.AddDays(1);
			//}
			//result = result.Replace("\t", "    ");
			return result;
		}
	}
}


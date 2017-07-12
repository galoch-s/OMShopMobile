using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace OMShopMobile
{
	public class Schedule
	{
		[JsonProperty("week_day")]
		public int WeekDay { get; set; }

		[JsonProperty("start_time")]
		public int StartTime { get; set; }

		[JsonProperty("stop_time")]
		public int StopTime { get; set; }

		public override bool Equals(object obj)
		{
			Schedule entity = obj as Schedule;
			return WeekDay == entity.WeekDay && StartTime == entity.StartTime && StopTime == entity.StopTime;
		}

		public bool IsEqualsTime(Schedule entity)
		{ 
			return StartTime == entity.StartTime && StopTime == entity.StopTime;
		}

		public static bool IsTimeOrder(List<Schedule> schedulesList)
		{
			if (schedulesList == null) return true;
			DateTime dateNow = DateTime.Now;
			int numberDay = (int)dateNow.DayOfWeek;
			if (numberDay == 0)
				numberDay = 7;
			numberDay--;

			Schedule schedule = schedulesList.FirstOrDefault(g => g.WeekDay == numberDay);
			if (schedule != null) {
				double seconds = dateNow.TimeOfDay.TotalSeconds;
				if (seconds < schedule.StartTime ||
					seconds > schedule.StopTime)
					return false;
			}
			return true;
		}
	}
}


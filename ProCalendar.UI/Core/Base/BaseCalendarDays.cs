using ProCalendar.UI.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProCalendar.UI.Core.Base
{
    public abstract class BaseCalendarDays<T> where T : ProCalendarToggleButton, new()
    {
        public BaseCalendarDays(T currentDay, params DateTime[] blackoutDays)
        {
            this.CurrentDay = currentDay;

            if (blackoutDays != null && blackoutDays.Any())
                this.BlackoutDays = new List<DateTime>(blackoutDays);

            this.Initialize();
        }

        private void Initialize()
        {
            this.BaseDays = new List<T>();

            int countDays = DateTime.DaysInMonth(CurrentDay.DateTime.Year, CurrentDay.DateTime.Month);
            for (int day = 1; day <= countDays; day++)
            {
                var dateTime = new DateTime(CurrentDay.DateTime.Year, CurrentDay.DateTime.Month, day);

                var dateTimeModel = new T()
                {
                    DateTime = dateTime,
                    IsWeekend = this.GetIsWeekend(dateTime),
                    IsBlackout = this.GetIsBlackout(dateTime),
                    IsSelected = this.CurrentDay.IsSelected,
                    IsDisabled = this.CurrentDay.IsDisabled,
                    IsToday = this.GetIsToday(dateTime)
                };

                this.BaseDays.Add(dateTimeModel);
            }
        }

        public virtual bool GetIsBlackout(DateTime dateTime)
        {
            if (BlackoutDays == null || !BlackoutDays.Any()) return false;

            var blackoutDay =
                this.BlackoutDays.FirstOrDefault(i =>
                    i.Year == dateTime.Year &&
                    i.Month == dateTime.Month &&
                    i.Day == dateTime.Day);

            return blackoutDay.Year != 0001;
        }

        public virtual bool GetIsWeekend(DateTime dateTime) =>
            dateTime.DayOfWeek == DayOfWeek.Saturday || dateTime.DayOfWeek == DayOfWeek.Sunday;

        public virtual bool GetIsToday(DateTime dateTime) =>
            dateTime.Year == DateTime.Now.Year &&
            dateTime.Month == DateTime.Now.Month &&
            dateTime.Day == DateTime.Now.Day;

        public IEnumerable<DateTime> BlackoutDays { get; set; }

        public T CurrentDay { get; set; }

        public IList<T> BaseDays { get; set; }
    }
}

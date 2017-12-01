using ProCalendar.UI.Controls;
using ProCalendar.UI.Core.Base;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProCalendar.UI.Core
{
    public sealed class CalendarDays<T> : BaseCalendarDays<T> where T : ProCalendarToggleButton, new()
    {
        public int ContentDaysCapacity
        {
            get => 42;
        }

        public CalendarDays() : this(new T()) { }

        public CalendarDays(T dateTimeModel, params DateTime[] blackoutDays) : base(dateTimeModel, blackoutDays)
        {
            this.Initialize();
        }

        private void Initialize()
        {
            int count = 0;
            this.Days = new List<T>();

            int dayOfWeek = (int)this.CurrentDay.DateTime.DayOfWeek;

            count = (dayOfWeek == 0) ? 6 : dayOfWeek - 1;
            if (count != 0)
                AddRemainingDates(count, this.CurrentDay.DateTime.AddDays(-count));

            foreach (var dateTimeModel in this.BaseDays)
                this.Days.Add(dateTimeModel);

            count = this.ContentDaysCapacity - this.Days.Count;
            AddRemainingDates(count, this.CurrentDay.DateTime.AddMonths(1));
        }

        private void AddRemainingDates(int daysCount, DateTime remainingDateTime)
        {
            for (int i = 0; i < daysCount; i++)
            {
                var dateTimeModel = new T()
                {
                    DateTime = remainingDateTime,
                    IsWeekend = this.GetIsWeekend(remainingDateTime),
                    IsBlackout = this.GetIsBlackout(remainingDateTime),
                    IsSelected = this.CurrentDay.IsSelected,
                    IsDisabled = this.CurrentDay.IsDisabled,
                    IsToday = this.GetIsToday(remainingDateTime)
                };

                this.Days.Add(dateTimeModel);
                remainingDateTime = remainingDateTime.AddDays(1);
            }
        }

        public IList<T> Days { get; set; }
    }
}

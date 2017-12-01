using ProCalendar.UI.Controls;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProCalendar.UI.Core
{
    public enum CalendarMonthsLoadingType
    {
        LoadingYears,
        LoadingMonths,
        LoadingDays
    }

    public sealed class CalendarMonths
    {
        public DateTime Min { get; set; }
        public DateTime Max { get; set; }
        public CalendarMonthsLoadingType ProListDatesLoadingType { get; set; }
        public CalendarMonths() : this(DateTime.Now, DateTime.Now.AddMonths(3), CalendarMonthsLoadingType.LoadingMonths, DateTime.Now.AddDays(3), DateTime.Now.AddDays(12))
        {

        }

        public CalendarMonths(DateTime min, DateTime max, CalendarMonthsLoadingType proListDatesLoadingType, params DateTime[] blackoutDays)
        {
            this.BlackoutDays = blackoutDays;
            this.ProListDatesLoadingType = proListDatesLoadingType;
            this.Min = min;
            this.Max = max;

            Initialize();
        }

        private void Initialize()
        {
            this.Months = new List<CalendarDays<ProCalendarToggleButton>>();

            switch (this.ProListDatesLoadingType)
            {
                case CalendarMonthsLoadingType.LoadingYears:
                    {
                        LoadYears();
                        break;
                    }
                case CalendarMonthsLoadingType.LoadingMonths:
                    {
                        LoadMonths();
                        break;
                    }
                case CalendarMonthsLoadingType.LoadingDays:
                    {
                        LoadDays();
                        break;
                    }
            }
        }

        private void LoadYears()
        {
            for (DateTime i = this.Min; i <= this.Max;)
            {
                for (int j = 1; j <= 12; j++)
                {
                    var dateTime = new DateTime(i.Year, j, 1);

                    var dateTimeModel = new ProCalendarToggleButton()
                    {
                        DateTime = dateTime,
                        IsWeekend = false,
                        IsBlackout = false,
                        IsSelected = false,
                        IsDisabled = false,
                        IsToday = false
                    };

                    this.Months.Add(new CalendarDays<ProCalendarToggleButton>(dateTimeModel));
                }
                i = i.AddYears(1);
            }
        }

        private void LoadMonths()
        {
            for (DateTime i = this.Min; i <= this.Max;)
            {
                var dateTime = new DateTime(i.Year, i.Month, 1);

                var dateTimeModel = new ProCalendarToggleButton()
                {
                    DateTime = dateTime,
                    IsWeekend = false,
                    IsBlackout = false,
                    IsSelected = false,
                    IsDisabled = false,
                    IsToday = false
                };

                this.Months.Add(new CalendarDays<ProCalendarToggleButton>(dateTimeModel, this.BlackoutDays.ToArray()));
                i = i.AddMonths(1);
            }
        }

        private void LoadDays()
        {
            //TODO: 
        }

        public IEnumerable<DateTime> BlackoutDays { get; set; }

        public IList<CalendarDays<ProCalendarToggleButton>> Months { get; set; }
    }
}

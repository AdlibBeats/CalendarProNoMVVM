using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace ProCalendar.UI.Controls
{
    public class ProCalendarToggleButton : ContentControl
    {
        #region Public Events

        public event RoutedEventHandler Selected;
        public event RoutedEventHandler Unselected;

        #endregion

        #region Public Cotr

        public ProCalendarToggleButton()
        {
            this.DefaultStyleKey = typeof(ProCalendarToggleButton);

            this.PointerPressed -= OnPointerPressed;
            this.PointerReleased -= OnPointerReleased;
            this.PointerEntered -= OnPointerEntered;
            this.PointerExited -= OnPointerExited;

            this.PointerPressed += OnPointerPressed;
            this.PointerReleased += OnPointerReleased;
            this.PointerEntered += OnPointerEntered;
            this.PointerExited += OnPointerExited;
        }

        #endregion

        #region Protected OnApplyTemplate

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            
            if (this.IsDisabled)
                UpdateIsDisable();
            else if (this.IsBlackout)
                UpdateIsBlackout();
            else
                UpdateIsSelected();

            UpdateIsToday();
            UpdateIsWeekend();
        }

        #endregion

        #region Private Handlers

        private void OnPointerPressed(object sender, PointerRoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, this.IsSelected ? "CheckedPressed" : "Pressed", true);

            this.IsSelected = !this.IsSelected;

            if (this.IsSelected)
                Selected?.Invoke(this, null);
            else
                Unselected?.Invoke(this, null);
        }

        private void OnPointerEntered(object sender, PointerRoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, this.IsSelected ? "CheckedPointerOver" : "PointerOver", true);
        }

        private void OnPointerExited(object sender, PointerRoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, this.IsSelected ? "CheckedNormal" : "Normal", true);
        }

        private void OnPointerReleased(object sender, PointerRoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, this.IsSelected ? "CheckedNormal" : "Normal", true);
        }

        #endregion

        #region Public Equals

        public bool Equals(DateTime dateTime) =>
            this.DateTime.Year == dateTime.Year &&
            this.DateTime.Month == dateTime.Month &&
            this.DateTime.Day == dateTime.Day;

        #endregion

        #region Private Updating Methods

        private void UpdateIsSelected()
        {
            this.IsEnabled = true;

            VisualStateManager.GoToState(this, this.IsSelected ? "CheckedNormal" : "Normal", true);
        }

        private void UpdateIsBlackout()
        {
            this.IsEnabled = !this.IsBlackout;

            VisualStateManager.GoToState(this, this.IsSelected ? "CheckedBlackouted" : "Blackouted", true);
        }

        private void UpdateIsDisable()
        {
            this.IsEnabled = !this.IsDisabled;

            VisualStateManager.GoToState(this, this.IsSelected ? "CheckedDisabled" : "Disabled", true);
        }

        private void UpdateIsWeekend()
        {
            VisualStateManager.GoToState(this, this.IsWeekend ? "IsWeekendTrue" : "IsWeekendFalse", true);
        }

        private void UpdateIsToday()
        {
            VisualStateManager.GoToState(this, this.IsToday ? "IsToodayTrue" : "IsToodayFalse", true);
        }

        private void UpdateDateTime()
        {
            this.Content = this.DateTime.ToString("dd");
        }

        #endregion

        #region Public Dependency Properties

        public bool IsSelected
        {
            get { return (bool)GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
        }

        public static readonly DependencyProperty IsSelectedProperty =
            DependencyProperty.Register("IsSelected", typeof(bool), typeof(ProCalendarToggleButton), new PropertyMetadata(false, OnIsSelectedChanged));

        private static void OnIsSelectedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var proCalendarToggleButton = d as ProCalendarToggleButton;
            if (proCalendarToggleButton == null) return;

            proCalendarToggleButton.UpdateIsSelected();
        }

        public bool IsBlackout
        {
            get { return (bool)GetValue(IsBlackoutProperty); }
            set { SetValue(IsBlackoutProperty, value); }
        }

        public static readonly DependencyProperty IsBlackoutProperty =
            DependencyProperty.Register("IsBlackout", typeof(bool), typeof(ProCalendarToggleButton), new PropertyMetadata(false, OnIsBlackoutChanged));

        private static void OnIsBlackoutChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var proCalendarToggleButton = d as ProCalendarToggleButton;
            if (proCalendarToggleButton == null) return;

            proCalendarToggleButton.UpdateIsBlackout();
        }

        public bool IsDisabled
        {
            get { return (bool)GetValue(IsDisabledProperty); }
            set { SetValue(IsDisabledProperty, value); }
        }

        public static readonly DependencyProperty IsDisabledProperty =
            DependencyProperty.Register("IsDisabled", typeof(bool), typeof(ProCalendarToggleButton), new PropertyMetadata(false, OnIsDisabledChanged));

        private static void OnIsDisabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var proCalendarToggleButton = d as ProCalendarToggleButton;
            if (proCalendarToggleButton == null) return;

            proCalendarToggleButton.UpdateIsDisable();
        }

        public bool IsWeekend
        {
            get { return (bool)GetValue(IsWeekendProperty); }
            set { SetValue(IsWeekendProperty, value); }
        }

        public static readonly DependencyProperty IsWeekendProperty =
            DependencyProperty.Register("IsWeekend", typeof(bool), typeof(ProCalendarToggleButton), new PropertyMetadata(false, OnIsWeekendChanged));

        private static void OnIsWeekendChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var proCalendarToggleButton = d as ProCalendarToggleButton;
            if (proCalendarToggleButton == null) return;

            proCalendarToggleButton.UpdateIsWeekend();
        }

        public bool IsToday
        {
            get { return (bool)GetValue(IsTodayProperty); }
            set { SetValue(IsTodayProperty, value); }
        }

        public static readonly DependencyProperty IsTodayProperty =
            DependencyProperty.Register("IsToday", typeof(bool), typeof(ProCalendarToggleButton), new PropertyMetadata(false, OnIsTodayChanged));

        private static void OnIsTodayChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var proCalendarToggleButton = d as ProCalendarToggleButton;
            if (proCalendarToggleButton == null) return;

            proCalendarToggleButton.UpdateIsToday();
        }

        public DateTime DateTime
        {
            get { return (DateTime)GetValue(DateTimeProperty); }
            set { SetValue(DateTimeProperty, value); }
        }

        public static readonly DependencyProperty DateTimeProperty =
            DependencyProperty.Register("DateTime", typeof(DateTime), typeof(ProCalendarToggleButton), new PropertyMetadata(DateTime.Now, lol));

        private static void lol(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var proCalendarToggleButton = d as ProCalendarToggleButton;
            if (proCalendarToggleButton == null) return;

            proCalendarToggleButton.UpdateDateTime();
        }

        #endregion
    }
}

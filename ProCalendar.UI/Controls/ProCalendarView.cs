using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media;

namespace ProCalendar.UI.Controls
{
    public class BaseListDates<T> where T : ProCalendarToggleButton, new()
    {
        public BaseListDates(T currentDay, params DateTime[] blackoutDays)
        {
            this.CurrentDay = currentDay;

            if (blackoutDays != null && blackoutDays.Any())
                this.BlackoutDays = new List<DateTime>(blackoutDays);

            this.Initialize();
        }

        private void Initialize()
        {
            this.CurrentDays = new List<T>();

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

                this.CurrentDays.Add(dateTimeModel);
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

        public List<DateTime> BlackoutDays { get; set; }

        public T CurrentDay { get; set; }

        public List<T> CurrentDays { get; set; }
    }

    public sealed class ListDates<T> : BaseListDates<T> where T : ProCalendarToggleButton, new()
    {
        public int ContentDaysCapacity
        {
            get => 42;
        }

        public ListDates() : this(new T()) { }

        public ListDates(T dateTimeModel, params DateTime[] blackoutDays) : base(dateTimeModel, blackoutDays)
        {
            this.Initialize();
        }

        private void Initialize()
        {
            int count = 0;
            this.ContentDays = new List<T>();

            int dayOfWeek = (int)this.CurrentDay.DateTime.DayOfWeek;

            count = (dayOfWeek == 0) ? 6 : dayOfWeek - 1;
            if (count != 0)
                AddRemainingDates(count, this.CurrentDay.DateTime.AddDays(-count));

            foreach (var dateTimeModel in this.CurrentDays)
                this.ContentDays.Add(dateTimeModel);

            count = this.ContentDaysCapacity - this.ContentDays.Count;
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

                this.ContentDays.Add(dateTimeModel);
                remainingDateTime = remainingDateTime.AddDays(1);
            }
        }

        public List<T> ContentDays { get; set; }
    }

    public enum ProListDatesLoadingType
    {
        LoadingYears,
        LoadingMonths,
        LoadingDays
    }

    public sealed class ProListDates
    {
        public DateTime Min { get; set; }

        public DateTime Max { get; set; }

        public ProListDatesLoadingType ProListDatesLoadingType { get; set; }

        public ProListDates() : this(DateTime.Now, DateTime.Now.AddMonths(3), ProListDatesLoadingType.LoadingMonths, DateTime.Now.AddDays(3), DateTime.Now.AddDays(12))
        {

        }

        public ProListDates(DateTime min, DateTime max, ProListDatesLoadingType proListDatesLoadingType, params DateTime[] blackoutDays)
        {
            this.BlackoutDays = blackoutDays;
            this.ProListDatesLoadingType = proListDatesLoadingType;
            this.Min = min;
            this.Max = max;

            Initialize();
        }

        private void Initialize()
        {
            this.ListDates = new List<ListDates<ProCalendarToggleButton>>();

            switch (this.ProListDatesLoadingType)
            {
                case ProListDatesLoadingType.LoadingYears:
                    {
                        LoadYears();
                        break;
                    }
                case ProListDatesLoadingType.LoadingMonths:
                    {
                        LoadMonths();
                        break;
                    }
                case ProListDatesLoadingType.LoadingDays:
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

                    this.ListDates.Add(new ListDates<ProCalendarToggleButton>(dateTimeModel));
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

                this.ListDates.Add(new ListDates<ProCalendarToggleButton>(dateTimeModel, this.BlackoutDays));
                i = i.AddMonths(1);
            }
        }

        private void LoadDays()
        {
            //TODO: 
        }

        public DateTime[] BlackoutDays { get; set; }

        public List<ListDates<ProCalendarToggleButton>> ListDates { get; set; }
    }

    public enum ProCalendarViewSelectionMode
    {
        None,
        Single,
        Multiple,
        Extended
    }

    public class ProCalendarView : Control
    {
        public event RoutedEventHandler SelectionChanged;
        public event RoutedEventHandler UnselectionChanged;

        public ProCalendarView()
        {
            this.DefaultStyleKey = typeof(ProCalendarView);
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            UpdateDaysOfWeekContent("DaysOfWeekContent");
            UpdateContentTemplateRoot("ContentFlipView");

            UpdateNavigationButtons("PreviousButtonVertical", -1, i => i.SelectedIndex > 0);
            UpdateNavigationButtons("NextButtonVertical", 1, i => i.Items.Count - 1 > i.SelectedIndex);
        }

        private void UpdateDaysOfWeekContent(string childName)
        {
            this.DaysOfWeekContent = this.GetTemplateChild(childName) as AdaptiveGridView;
            if (this.DaysOfWeekContent == null) return;

            this.DaysOfWeekContent.Items = new ListDates<ProCalendarToggleButton>().ContentDays;

            foreach (var item in this.DaysOfWeekContent.Items)
                item.Content = item.DateTime.ToString("ddd");

            //this.DaysOfWeekContent.ItemsSource = new ListDates().ContentDays;
        }

        private void UpdateContentTemplateRoot(string childName)
        {
            this.ContentTemplateRoot = this.GetTemplateChild(childName) as Selector;
            if (ContentTemplateRoot == null) return;

            //this.ContentTemplateRoot.ItemsSource = new ProListDates().ListDates;

            var items = new ProListDates().ListDates;

            for (int i = 0; i < items.Count; i++)
            {
                AdaptiveGridView adaptiveGridView = new AdaptiveGridView
                {
                    ItemWidth = 36,
                    ItemHeight = 36,
                    RowsCount = 6,
                    ColumnsCount = 7,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Top,
                    Items = items[i].ContentDays
                };

                this.ContentTemplateRoot.Items.Add(adaptiveGridView);
            }

            this.ContentTemplateRoot.Loaded -= ContentTemplateRoot_Loaded;
            this.ContentTemplateRoot.Loaded += ContentTemplateRoot_Loaded;

            this.ContentTemplateRoot.SelectionChanged -= ContentTemplateRoot_SelectionChanged;
            this.ContentTemplateRoot.SelectionChanged += ContentTemplateRoot_SelectionChanged;
        }

        private void UpdateNavigationButtons(string childName, int navigatedIndex, Predicate<Selector> func)
        {
            var navigationButton = this.GetTemplateChild(childName) as ButtonBase;
            if (navigationButton == null) return;

            navigationButton.Click -= OnNavigationButtonClick;
            navigationButton.Click += OnNavigationButtonClick;

            void OnNavigationButtonClick(object sender, RoutedEventArgs e)
            {
                if (func.Invoke(ContentTemplateRoot))
                    ContentTemplateRoot.SelectedIndex += navigatedIndex;
            }
        }

        private void ContentTemplateRoot_Loaded(object sender, RoutedEventArgs e)
        {
            if (!this.IsContentTemplateRootLoaded)
                OnLoadingUpdateChildren();

            this.IsContentTemplateRootLoaded = true;

            if (this.SelectedItem != null)
                LoadSelectedChildren(i => i.IsSelected);
            else
                LoadSelectedChildren(i => i.IsToday);
        }
        private void OnLoadingUpdateChildren()
        {
            if (this.ContentTemplateRoot.ItemsPanelRoot == null) return;

            for (int i = 0; i < this.ContentTemplateRoot.ItemsPanelRoot.Children.Count; i++)
            {
                var selectorItem = this.ContentTemplateRoot.ItemsPanelRoot.Children.ElementAtOrDefault(i) as SelectorItem;
                if (selectorItem == null) continue;

                var adaptiveGridView = selectorItem.ContentTemplateRoot as AdaptiveGridView;
                if (adaptiveGridView == null) continue;

                adaptiveGridView.SelectionChanged -= AdaptiveGridView_SelectionChanged;
                adaptiveGridView.SelectionChanged += AdaptiveGridView_SelectionChanged;
            }
        }

        private int _contentTemplateRoot_CurrentIndex = 0;
        private void ContentTemplateRoot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.ContentTemplateRoot.SelectedIndex > -1)
            {
                OnScrollingUpdateChildren();

                _contentTemplateRoot_CurrentIndex =
                    this.ContentTemplateRoot.SelectedIndex;
            }
        }

        private Panel GetItemsPanelRootFromIndex(int index)
        {
            if (this.ContentTemplateRoot.ItemsPanelRoot == null) return null;

            var selectorItem = this.ContentTemplateRoot.ItemsPanelRoot.Children.ElementAtOrDefault(index) as SelectorItem;
            if (selectorItem == null) return null;

            var adaptiveGridView = selectorItem.ContentTemplateRoot as AdaptiveGridView;
            if (adaptiveGridView == null) return null;

            var itemsPanelRoot = adaptiveGridView.ItemsPanelRoot as Panel;
            if (itemsPanelRoot == null) return null;

            return itemsPanelRoot;
        }

        private void OnScrollingUpdateChildren()
        {
            int index = _contentTemplateRoot_CurrentIndex < this.ContentTemplateRoot.SelectedIndex ? -1 : 1;

            var itemsPanelRoot = GetItemsPanelRootFromIndex(this.ContentTemplateRoot.SelectedIndex + index);
            if (itemsPanelRoot == null) return;

            var currentItemsPanelRoot = GetItemsPanelRootFromIndex(this.ContentTemplateRoot.SelectedIndex);

            for (int i = 0; i < itemsPanelRoot.Children.Count; i++)
            {
                var proCalendarToggleButton = itemsPanelRoot.Children.ElementAtOrDefault(i) as ProCalendarToggleButton;

                ScrollingUpdateChildren(currentItemsPanelRoot, proCalendarToggleButton);
            }
        }

        private void ScrollingUpdateChildren(Panel currentItemsPanelRoot, ProCalendarToggleButton proCalendarToggleButton)
        {
            if (currentItemsPanelRoot == null || proCalendarToggleButton == null) return;

            var currentProCalendarToggleButton = currentItemsPanelRoot.Children.FirstOrDefault(j =>
            {
                var toggleButton = j as ProCalendarToggleButton;
                if (toggleButton == null) return false;

                return toggleButton.Equals(proCalendarToggleButton.DateTime);
            }) as ProCalendarToggleButton;

            if (currentProCalendarToggleButton == null) return;

            if (proCalendarToggleButton.IsSelected && proCalendarToggleButton.IsToday)
            {
                currentProCalendarToggleButton.IsSelected = true;
                proCalendarToggleButton.IsSelected = false;

                currentProCalendarToggleButton.IsToday = true;
                proCalendarToggleButton.IsToday = false;
            }
            else if (proCalendarToggleButton.IsSelected)
            {
                currentProCalendarToggleButton.IsSelected = true;
                proCalendarToggleButton.IsSelected = false;
            }
            else if (proCalendarToggleButton.IsToday)
            {
                currentProCalendarToggleButton.IsToday = true;
                proCalendarToggleButton.IsToday = false;
            }
            else if (proCalendarToggleButton.IsDisabled)
            {
                currentProCalendarToggleButton.IsDisabled = true;
                proCalendarToggleButton.IsDisabled = false;
            }
            else if (proCalendarToggleButton.IsBlackout)
            {
                currentProCalendarToggleButton.IsBlackout = true;
                proCalendarToggleButton.IsBlackout = false;
            }
        }

        private void LoadSelectedChildren(Predicate<ProCalendarToggleButton> func)
        {
            for (int i = 0; i < this.ContentTemplateRoot.ItemsPanelRoot.Children.Count; i++)
            {
                var itemsPanelRoot = GetItemsPanelRootFromIndex(i);
                if (itemsPanelRoot == null) continue;

                for (int j = 0; j < itemsPanelRoot.Children.Count; j++)
                {
                    var proCalendarToggleButton = itemsPanelRoot.Children.ElementAtOrDefault(j) as ProCalendarToggleButton;
                    if (proCalendarToggleButton == null) continue;

                    if (func.Invoke(proCalendarToggleButton))
                    {
                        this.ContentTemplateRoot.SelectedIndex = i;
                        if (proCalendarToggleButton.DateTime.Day > 20)
                            return;
                    }
                    else
                        continue;
                }
            }
        }

        private void AdaptiveGridView_SelectionChanged(object sender, RoutedEventArgs e)
        {
            var selectedItem = sender as ProCalendarToggleButton;
            if (selectedItem == null) return;

            this.SelectedItem = selectedItem;

            OnSelectedChangedUpdateChildren();

            OnSelectedChangedUpdateEvents();
        }

        private void OnSelectedChangedUpdateEvents()
        {
            if (this.SelectedItem.IsSelected)
                SelectionChanged?.Invoke(this.SelectedItem, null);
            else
            {
                this.SelectedItem = null;
                UnselectionChanged?.Invoke(null, null);
            }
        }

        private void OnSelectedChangedUpdateChildren()
        {
            for (int i = 0; i < this.ContentTemplateRoot.ItemsPanelRoot.Children.Count; i++)
            {
                var itemsPanelRoot = GetItemsPanelRootFromIndex(i);
                if (itemsPanelRoot == null) continue;

                for (int j = 0; j < itemsPanelRoot.Children.Count; j++)
                {
                    var proCalendarToggleButton = itemsPanelRoot.Children.ElementAtOrDefault(j) as ProCalendarToggleButton;
                    if (proCalendarToggleButton == null) continue;

                    ChangeSelectedMode(i, proCalendarToggleButton);
                }
            }
        }

        private void ChangeSelectedMode(int index, ProCalendarToggleButton proCalendarToggleButton)
        {
            switch (this.SelectionMode)
            {
                case ProCalendarViewSelectionMode.None:
                    {
                        UpdateNoneMode(index, proCalendarToggleButton);
                        break;
                    }
                case ProCalendarViewSelectionMode.Single:
                    {
                        UpdateSingleMode(index, proCalendarToggleButton);
                        break;
                    }
                case ProCalendarViewSelectionMode.Multiple:
                    {
                        UpdateMultipleMode(index, proCalendarToggleButton);
                        break;
                    }
                case ProCalendarViewSelectionMode.Extended:
                    {
                        UpdateExtendedMode(index, proCalendarToggleButton);
                        break;
                    }
            }
        }

        private void UpdateNoneMode(int index, ProCalendarToggleButton proCalendarToggleButton)
        {
            //TODO: UpdateNoneMode();
        }

        private void UpdateSingleMode(int index, ProCalendarToggleButton proCalendarToggleButton)
        {
            if (this.SelectedItem.IsSelected && this.ContentTemplateRoot.SelectedIndex == index)
                proCalendarToggleButton.IsSelected = this.SelectedItem.Equals(proCalendarToggleButton.DateTime);
            else
                proCalendarToggleButton.IsSelected = false;
        }

        private void UpdateMultipleMode(int index, ProCalendarToggleButton proCalendarToggleButton)
        {
            //TODO: UpdateMultipleMode();
        }

        private void UpdateExtendedMode(int index, ProCalendarToggleButton proCalendarToggleButton)
        {
            //TODO: UpdateExtendedMode();
        }

        public Selector ContentTemplateRoot
        {
            get { return (Selector)GetValue(ContentTemplateRootProperty); }
            private set { SetValue(ContentTemplateRootProperty, value); }
        }

        public static readonly DependencyProperty ContentTemplateRootProperty =
            DependencyProperty.Register("ContentTemplateRoot", typeof(Selector), typeof(ProCalendarView), new PropertyMetadata(null));

        public bool IsContentTemplateRootLoaded
        {
            get { return (bool)GetValue(IsContentTemplateRootLoadedProperty); }
            private set { SetValue(IsContentTemplateRootLoadedProperty, value); }
        }

        public static readonly DependencyProperty IsContentTemplateRootLoadedProperty =
            DependencyProperty.Register("IsContentTemplateRootLoaded", typeof(bool), typeof(ProCalendarView), new PropertyMetadata(false));

        public AdaptiveGridView DaysOfWeekContent
        {
            get { return (AdaptiveGridView)GetValue(DaysOfWeekContentProperty); }
            private set { SetValue(DaysOfWeekContentProperty, value); }
        }

        public static readonly DependencyProperty DaysOfWeekContentProperty =
            DependencyProperty.Register("DaysOfWeekContent", typeof(AdaptiveGridView), typeof(ProCalendarView), new PropertyMetadata(null));

        public ProCalendarToggleButton SelectedItem
        {
            get { return (ProCalendarToggleButton)GetValue(SelectedItemProperty); }
            private set { SetValue(SelectedItemProperty, value); }
        }

        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register("SelectedItem", typeof(ProCalendarToggleButton), typeof(ProCalendarView), new PropertyMetadata(null));

        public ProCalendarViewSelectionMode SelectionMode
        {
            get { return (ProCalendarViewSelectionMode)GetValue(SelectionModeProperty); }
            set { SetValue(SelectionModeProperty, value); }
        }

        public static readonly DependencyProperty SelectionModeProperty =
            DependencyProperty.Register("SelectionMode", typeof(ProCalendarViewSelectionMode), typeof(ProCalendarView), new PropertyMetadata(ProCalendarViewSelectionMode.Single, OnSelectionModeChanged));

        private static void OnSelectionModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var proCalendarView = d as ProCalendarView;
            if (proCalendarView == null) return;

            //TODO: UpdateSelectionMode(newValue);
        }
    }
}

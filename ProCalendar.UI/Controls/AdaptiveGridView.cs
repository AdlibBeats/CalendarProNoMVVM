using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace ProCalendar.UI.Controls
{
    public class AdaptiveGridView : Control
    {
        public AdaptiveGridView()
        {
            this.DefaultStyleKey = typeof(AdaptiveGridView);
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.ItemsPanelRoot = this.GetTemplateChild("ItemsPanelRoot") as Grid;

            UpdateItemsPanelRootColumns(this.ColumnsCount);
            UpdateItemsPanelRootRows(this.RowsCount);
        }

        private void UpdateItemsPanelRootColumns(int value)
        {
            if (this.ItemsPanelRoot == null) return;

            this.ItemsPanelRoot.ColumnDefinitions.Clear();

            for (int column = 0; column < value; column++)
                this.ItemsPanelRoot.ColumnDefinitions.Add(new ColumnDefinition
                    { Width = new GridLength(0, GridUnitType.Auto) });
        }

        private void UpdateItemsPanelRootRows(int value)
        {
            if (this.ItemsPanelRoot == null) return;

            this.ItemsPanelRoot.RowDefinitions.Clear();

            for (int row = 0; row < value; row++)
                this.ItemsPanelRoot.RowDefinitions.Add(new RowDefinition
                    { Height = new GridLength(0, GridUnitType.Auto) });
        }

        private void UpdateItemsSource(object value)
        {
            if (this.ItemsPanelRoot == null) return;

            var itemsSource = value as IEnumerable<object>;
            if (itemsSource == null) return;

            int column = 0;
            int row = 0;

            int contentCount = this.RowsCount * this.ColumnsCount;

            int count = itemsSource.Count() < contentCount ? itemsSource.Count() : contentCount;

            for (int i = 0; i < count; i++)
            {
                var dataContext = itemsSource.ElementAtOrDefault(i);
                if (dataContext == null) continue;

                var itemTemplate = new Button();
                if (itemTemplate == null) return;

                Grid.SetColumn(itemTemplate, column);
                Grid.SetRow(itemTemplate, row);

                this.ItemsPanelRoot.Children.Add(itemTemplate);
                //this.ItemsPanelRoot.UpdateLayout();

                column++;
                if (column != this.ColumnsCount)
                    continue;

                column = 0;
                row++;
                if (row != this.RowsCount)
                    continue;
            }
        }

        private FrameworkElement GetItemTemplate(object dataContext)
        {
            if (this.ItemTemplate == null) return null;

            var frameworkElement = this.ItemTemplate.LoadContent() as FrameworkElement;
            if (frameworkElement == null) return null;

            frameworkElement.Width = this.ItemWidth;
            frameworkElement.Height = this.ItemHeight;
            frameworkElement.HorizontalAlignment = this.ItemHorizontalAlignment;
            frameworkElement.VerticalAlignment = this.ItemVerticalAlignment;
            frameworkElement.Margin = this.ItemMargin;
            //frameworkElement.DataContext = dataContext;

            var control = frameworkElement as Control;
            if (control == null) return frameworkElement;

            control.BorderBrush = this.ItemBorderBrush;
            control.BorderThickness = this.ItemBorderThickness;
            control.Foreground = this.ItemForeground;
            control.Background = this.ItemBackground;
            control.Padding = this.ItemPadding;

            //var dateTimeModel = dataContext as DateTimeModel;
            //if (dateTimeModel == null) return control;

            //var contentControl = control as ContentControl;
            //if (contentControl == null) return control;

            //contentControl.Content = dateTimeModel.DateTime.ToString("ddd");

            //var proCalendarToggleButton = contentControl as ProCalendarToggleButton;
            //if (proCalendarToggleButton == null) return contentControl;

            //proCalendarToggleButton.IsSelected = dateTimeModel.IsSelected;
            //proCalendarToggleButton.IsBlackout = dateTimeModel.IsBlackout;
            //proCalendarToggleButton.IsDisabled = dateTimeModel.IsDisabled;
            //proCalendarToggleButton.IsWeekend = dateTimeModel.IsWeekend;
            //proCalendarToggleButton.IsToday = dateTimeModel.IsToday;
            //proCalendarToggleButton.DateTime = dateTimeModel.DateTime;

            //proCalendarToggleButton.Selected -= OnSelected;
            //proCalendarToggleButton.Selected += OnSelected;

            //void OnSelected(object sender, RoutedEventArgs e) =>
            //    SelectionChanged?.Invoke(sender, null);

            return proCalendarToggleButton;
        }

        public Grid ItemsPanelRoot
        {
            get { return (Grid)GetValue(ItemsPanelRootProperty); }
            private set { SetValue(ItemsPanelRootProperty, value); }
        }

        public static readonly DependencyProperty ItemsPanelRootProperty =
            DependencyProperty.Register("ItemsPanelRoot", typeof(Grid), typeof(AdaptiveGridView), new PropertyMetadata(null));

        public DataTemplate ItemTemplate
        {
            get { return (DataTemplate)GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }

        public static readonly DependencyProperty ItemTemplateProperty =
            DependencyProperty.Register("ItemTemplate", typeof(DataTemplate), typeof(AdaptiveGridView), new PropertyMetadata(null));

        public object ItemsSource
        {
            get { return GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.RegisterAttached("ItemsSource", typeof(object), typeof(AdaptiveGridView), new PropertyMetadata(null, OnItemsSourceChanged));

        private static void OnItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var adaptiveGridView = d as AdaptiveGridView;
            if (adaptiveGridView == null) return;

            adaptiveGridView.UpdateItemsSource(e.NewValue);
        }

        public int RowsCount
        {
            get { return (int)GetValue(RowsCountProperty); }
            set { SetValue(RowsCountProperty, value); }
        }

        public static readonly DependencyProperty RowsCountProperty =
            DependencyProperty.RegisterAttached("RowsCount", typeof(int), typeof(AdaptiveGridView), new PropertyMetadata(0, OnRowsCountChanged));

        private static void OnRowsCountChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var adaptiveGridView = d as AdaptiveGridView;
            if (adaptiveGridView == null) return;

            adaptiveGridView.UpdateItemsPanelRootRows((int)e.NewValue);
        }

        public int ColumnsCount
        {
            get { return (int)GetValue(ColumnsCountProperty); }
            set { SetValue(ColumnsCountProperty, value); }
        }

        public static readonly DependencyProperty ColumnsCountProperty =
            DependencyProperty.RegisterAttached("ColumnsCount", typeof(int), typeof(AdaptiveGridView), new PropertyMetadata(0, OnColumnsCountChanged));

        private static void OnColumnsCountChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var adaptiveGridView = d as AdaptiveGridView;
            if (adaptiveGridView == null) return;

            adaptiveGridView.UpdateItemsPanelRootColumns((int)e.NewValue);
        }

        #region Item Properties

        public Brush ItemBorderBrush
        {
            get { return (Brush)GetValue(ItemBorderBrushProperty); }
            set { SetValue(ItemBorderBrushProperty, value); }
        }

        public static readonly DependencyProperty ItemBorderBrushProperty =
            DependencyProperty.Register("ItemBorderBrush", typeof(Brush), typeof(AdaptiveGridView), new PropertyMetadata(new SolidColorBrush(Colors.Gray)));

        public Thickness ItemBorderThickness
        {
            get { return (Thickness)GetValue(ItemBorderThicknessProperty); }
            set { SetValue(ItemBorderThicknessProperty, value); }
        }

        public static readonly DependencyProperty ItemBorderThicknessProperty =
            DependencyProperty.Register("ItemBorderThickness", typeof(Thickness), typeof(AdaptiveGridView), new PropertyMetadata(new Thickness(0, 0, 0.5, 0.5)));

        public Brush ItemForeground
        {
            get { return (Brush)GetValue(ItemForegroundProperty); }
            set { SetValue(ItemForegroundProperty, value); }
        }

        public static readonly DependencyProperty ItemForegroundProperty =
            DependencyProperty.RegisterAttached("ItemForeground", typeof(Brush), typeof(AdaptiveGridView), new PropertyMetadata(new SolidColorBrush(Colors.Black)));

        public Brush ItemBackground
        {
            get { return (Brush)GetValue(ItemBackgroundProperty); }
            set { SetValue(ItemBackgroundProperty, value); }
        }

        public static readonly DependencyProperty ItemBackgroundProperty =
            DependencyProperty.RegisterAttached("ItemBackground", typeof(Brush), typeof(AdaptiveGridView), new PropertyMetadata(new SolidColorBrush(Colors.White)));

        public double ItemWidth
        {
            get { return (double)GetValue(ItemWidthProperty); }
            set { SetValue(ItemWidthProperty, value); }
        }

        public static readonly DependencyProperty ItemWidthProperty =
            DependencyProperty.RegisterAttached("ItemWidth", typeof(double), typeof(AdaptiveGridView), new PropertyMetadata(36));

        public double ItemHeight
        {
            get { return (double)GetValue(ItemHeightProperty); }
            set { SetValue(ItemHeightProperty, value); }
        }

        public static readonly DependencyProperty ItemHeightProperty =
            DependencyProperty.RegisterAttached("ItemHeight", typeof(double), typeof(AdaptiveGridView), new PropertyMetadata(36));

        public HorizontalAlignment ItemHorizontalAlignment
        {
            get { return (HorizontalAlignment)GetValue(ItemHorizontalAlignmentProperty); }
            set { SetValue(ItemHorizontalAlignmentProperty, value); }
        }

        public static readonly DependencyProperty ItemHorizontalAlignmentProperty =
            DependencyProperty.Register("ItemHorizontalAlignment", typeof(HorizontalAlignment), typeof(AdaptiveGridView), new PropertyMetadata(1));

        public VerticalAlignment ItemVerticalAlignment
        {
            get { return (VerticalAlignment)GetValue(ItemVerticalAlignmentProperty); }
            set { SetValue(ItemVerticalAlignmentProperty, value); }
        }

        public static readonly DependencyProperty ItemVerticalAlignmentProperty =
            DependencyProperty.Register("ItemVerticalAlignment", typeof(VerticalAlignment), typeof(AdaptiveGridView), new PropertyMetadata(1));

        public Thickness ItemMargin
        {
            get { return (Thickness)GetValue(ItemMarginProperty); }
            set { SetValue(ItemMarginProperty, value); }
        }

        public static readonly DependencyProperty ItemMarginProperty =
            DependencyProperty.RegisterAttached("ItemMargin", typeof(Thickness), typeof(AdaptiveGridView), new PropertyMetadata(new Thickness(0)));

        public Thickness ItemPadding
        {
            get { return (Thickness)GetValue(ItemPaddingProperty); }
            set { SetValue(ItemPaddingProperty, value); }
        }

        public static readonly DependencyProperty ItemPaddingProperty =
            DependencyProperty.RegisterAttached("ItemPadding", typeof(Thickness), typeof(AdaptiveGridView), new PropertyMetadata(new Thickness(0)));

        #endregion
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace ProCalendar.UI.Controls
{
    public sealed class AdaptiveGridView : Control, IEnumerable<FrameworkElement>
    {
        #region Public Events

        public event RoutedEventHandler SelectionChanged;

        #endregion

        #region Private Fields 

        private int _currentColumn = 0;
        private int _currentRow = 0;

        #endregion

        #region Public Cotr
        public AdaptiveGridView()
        {
            this.DefaultStyleKey = typeof(AdaptiveGridView);
        }
        #endregion

        #region Protected OnApplyTemplate
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.ItemsPanelRoot = this.GetTemplateChild("ItemsPanelRoot") as Grid;

            UpdateColumnsCount();
            UpdateRowsCount();
            UpdateItems();
            UpdateItemWidth();
            UpdateItemHeigh();
        }
        #endregion

        #region Private Updating Methods
        private void UpdateColumnsCount()
        {
            if (this.ItemsPanelRoot == null) return;

            this.ItemsPanelRoot.ColumnDefinitions.Clear();

            for (int column = 0; column < this.ColumnsCount; column++)
                this.ItemsPanelRoot.ColumnDefinitions.Add(new ColumnDefinition
                    { Width = new GridLength(0, GridUnitType.Auto) });
        }

        private void UpdateRowsCount()
        {
            if (this.ItemsPanelRoot == null) return;

            this.ItemsPanelRoot.RowDefinitions.Clear();

            for (int row = 0; row < this.RowsCount; row++)
                this.ItemsPanelRoot.RowDefinitions.Add(new RowDefinition
                    { Height = new GridLength(0, GridUnitType.Auto) });
        }

        private void UpdateItems()
        {
            if (this.Items == null) return;
            if (this.ItemsPanelRoot?.Children != null)
                this.ItemsPanelRoot.Children.Clear();

            foreach (var item in this.Items)
                Add(item);
        }

        private void UpdateItemWidth()
        {
            if (this.ItemsPanelRoot?.Children == null) return;

            foreach (FrameworkElement child in this.ItemsPanelRoot.Children)
                child.Width = this.ItemWidth;
        }

        private void UpdateItemHeigh()
        {
            if (this.ItemsPanelRoot?.Children == null) return;

            foreach (FrameworkElement child in this.ItemsPanelRoot.Children)
                child.Height = this.ItemHeight;
        }
        #endregion

        #region Public Dependency Properties
        public IEnumerable<FrameworkElement> Items
        {
            get { return (IEnumerable<FrameworkElement>)GetValue(ItemsProperty); }
            set { SetValue(ItemsProperty, value); }
        }

        public static readonly DependencyProperty ItemsProperty =
            DependencyProperty.Register("Items", typeof(IEnumerable<FrameworkElement>), typeof(AdaptiveGridView), new PropertyMetadata(null, OnItemsChanged));

        private static void OnItemsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var adaptiveGridView = d as AdaptiveGridView;
            if (adaptiveGridView == null) return;

            adaptiveGridView.UpdateItems();
        }

        public Grid ItemsPanelRoot
        {
            get { return (Grid)GetValue(ItemsPanelRootProperty); }
            private set { SetValue(ItemsPanelRootProperty, value); }
        }

        public static readonly DependencyProperty ItemsPanelRootProperty =
            DependencyProperty.Register("ItemsPanelRoot", typeof(Grid), typeof(AdaptiveGridView), new PropertyMetadata(null));

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

            adaptiveGridView.UpdateRowsCount();
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

            adaptiveGridView.UpdateColumnsCount();
        }

        public double ItemWidth
        {
            get { return (double)GetValue(ItemWidthProperty); }
            set { SetValue(ItemWidthProperty, value); }
        }

        public static readonly DependencyProperty ItemWidthProperty =
            DependencyProperty.RegisterAttached("ItemWidth", typeof(double), typeof(AdaptiveGridView), new PropertyMetadata(36d, OnItemWidthChanged));

        private static void OnItemWidthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var adaptiveGridView = d as AdaptiveGridView;
            if (adaptiveGridView == null) return;

            adaptiveGridView.UpdateItemWidth();
        }

        public double ItemHeight
        {
            get { return (double)GetValue(ItemHeightProperty); }
            set { SetValue(ItemHeightProperty, value); }
        }

        public static readonly DependencyProperty ItemHeightProperty =
            DependencyProperty.RegisterAttached("ItemHeight", typeof(double), typeof(AdaptiveGridView), new PropertyMetadata(36d, OnItemHeightChanged));

        private static void OnItemHeightChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var adaptiveGridView = d as AdaptiveGridView;
            if (adaptiveGridView == null) return;

            adaptiveGridView.UpdateItemHeigh();
        }
        #endregion

        #region Public IList Methods & Properties
        public IEnumerator<FrameworkElement> GetEnumerator()
        {
            if (this.ItemsPanelRoot?.Children == null) yield return null;

            foreach (FrameworkElement child in this.ItemsPanelRoot.Children)
                yield return child;
        }

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

        public void Add(FrameworkElement item)
        {
            if (_currentRow == this.RowsCount)
                return;

            if (item == null) return;
            if (this.ItemsPanelRoot?.Children == null) return;

            item.Width = this.ItemWidth;
            item.Height = this.ItemHeight;

            var proCalendarToggleButton = item as ProCalendarToggleButton;
            if (proCalendarToggleButton != null)
            {
                proCalendarToggleButton.Selected -= OnSelected;
                proCalendarToggleButton.Selected += OnSelected;

                proCalendarToggleButton.Unselected -= OnSelected;
                proCalendarToggleButton.Unselected += OnSelected;

                void OnSelected(object sender, RoutedEventArgs e) =>
                    SelectionChanged?.Invoke(sender, null);
            }

            Grid.SetColumn(item, _currentColumn);
            Grid.SetRow(item, _currentRow);

            _currentColumn++;
            if (_currentColumn == this.ColumnsCount)
            {
                _currentColumn = 0;
                _currentRow++;
            }

            this.ItemsPanelRoot.Children.Add(item);
        }

        public int Count
        {
            get
            {
                if (this.ItemsPanelRoot?.Children == null)
                    return 0;
                else
                    return this.ItemsPanelRoot.Children.Count;
            }
        }

        public int IndexOf(FrameworkElement item)
        {
            if (item == null) return -1;
            if (this.ItemsPanelRoot?.Children == null) return -1;

            return this.ItemsPanelRoot.Children.IndexOf(item);
        }

        public void RemoveAt(int index)
        {
            if (this.ItemsPanelRoot?.Children == null) return;

            this.ItemsPanelRoot.Children.RemoveAt(index);
        }

        public void Clear()
        {
            if (this.ItemsPanelRoot?.Children == null) return;

            this.ItemsPanelRoot.Children.Clear();
        }

        public bool Contains(FrameworkElement item)
        {
            if (item == null) return false;
            if (this.ItemsPanelRoot?.Children == null) return false;

            return this.ItemsPanelRoot.Children.Contains(item);
        }

        public void CopyTo(FrameworkElement[] array, int arrayIndex)
        {
            if (this.ItemsPanelRoot?.Children == null) return;

            this.ItemsPanelRoot.Children.CopyTo(array, arrayIndex);
        }

        public bool Remove(FrameworkElement item)
        {
            if (item == null) return false;
            if (this.ItemsPanelRoot?.Children == null) return false;

            return this.ItemsPanelRoot.Children.Remove(item);
        }
        #endregion
    }
}

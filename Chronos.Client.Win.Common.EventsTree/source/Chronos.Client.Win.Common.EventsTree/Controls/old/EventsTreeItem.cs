﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Chronos.Client.Common.EventsTree;
using Chronos.Client.Win.Converters;

namespace Chronos.Client.Win.Common.EventsTree.Controls
{
    [TemplatePart(Name = HeaderPanelPartName, Type = typeof(Panel))]
    [TemplatePart(Name = ChildrenIndicatorBorderPartName, Type = typeof(Border))]
    [TemplatePart(Name = PercentsColorIndicatorBorderPartName, Type = typeof(Border))]
    [TemplatePart(Name = PercentTextBlockPartName, Type = typeof(TextBlock))]
    [TemplatePart(Name = TimeTextBlockPartName, Type = typeof(TextBlock))]
    [TemplatePart(Name = HitsTextBlockPartName, Type = typeof(TextBlock))]
    [TemplatePart(Name = NameTextBlockPartName, Type = typeof(TextBlock))]
    [TemplatePart(Name = ChildrenItemsControlPartName, Type = typeof(ItemsControl))]
    public class EventsTreeItem : Control, IDisposable
    {
        private const string HeaderPanelPartName = "HeaderPanel";
        private const string ChildrenIndicatorBorderPartName = "ChildrenIndicatorBorder";
        private const string PercentsColorIndicatorBorderPartName = "PercentsColorIndicatorBorder";
        private const string PercentTextBlockPartName = "PercentTextBlock";
        private const string TimeTextBlockPartName = "TimeTextBlock";
        private const string HitsTextBlockPartName = "HitsTextBlock";
        private const string NameTextBlockPartName = "NameTextBlock";
        private const string ChildrenItemsControlPartName = "ChildrenItemsControl";

        public static readonly DependencyProperty IsHoveredProperty;
        public static readonly DependencyProperty IsSelectedProperty;

        private readonly IEvent _event;
        private readonly bool _hasChildren;
        private Panel _headerPanel;
        private Border _childrenIndicatorBorder;
        private Border _percentsColorIndicatorBorder;
        private TextBlock _percentTextBlock;
        private TextBlock _timeTextBlock;
        private TextBlock _hitsTextBlock;
        private TextBlock _nameTextBlock;
        private ItemsControl _childrenItemsControl;
        private bool _areChildrenInitialized;
        private bool _isExpanded;
        private bool _notifySelection;
        private readonly EventsTree _treeView;
        private readonly IEventFormatter _eventFormatter;

        static EventsTreeItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(EventsTreeItem), new FrameworkPropertyMetadata(typeof(EventsTreeItem)));
            IsHoveredProperty = DependencyProperty.Register("IsHovered", typeof(bool), typeof(EventsTreeItem), new PropertyMetadata(OnIsHoveredPropertyChanged));
            IsSelectedProperty = DependencyProperty.Register("IsSelected", typeof(bool), typeof(EventsTreeItem), new PropertyMetadata(OnIsSelectedPropertyChanged));
        }

        public EventsTreeItem(IEvent @event, IEventFormatter eventFormatter, EventsTree treeView)
        {
            _event = @event;
            _hasChildren = @event.HasChildren;
            _treeView = treeView;
            _eventFormatter = eventFormatter;
            _notifySelection = true;
        }

        public bool IsHovered
        {
            get { return (bool)GetValue(IsHoveredProperty); }
            set { SetValue(IsHoveredProperty, value); }
        }

        public bool IsSelected
        {
            get { return (bool)GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
        }

        public IEvent Event
        {
            get { return _event; }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            //------------------
            _headerPanel = GetTemplateChild(HeaderPanelPartName) as Panel;
            if (_headerPanel != null)
            {
                _headerPanel.MouseLeftButtonDown += ContainerGridOnMouseDown;
                _headerPanel.MouseEnter += ContainerGridOnMouseEnter;
                _headerPanel.MouseLeave += ContainerGridOnMouseLeave;
            }
            //------------------
            _childrenIndicatorBorder = GetTemplateChild(ChildrenIndicatorBorderPartName) as Border;
            if (_childrenIndicatorBorder != null)
            {
                _childrenIndicatorBorder.Visibility = _hasChildren ? Visibility.Visible : Visibility.Hidden;
            }
            //------------------
            _percentsColorIndicatorBorder = GetTemplateChild(PercentsColorIndicatorBorderPartName) as Border;
            if (_percentsColorIndicatorBorder != null)
            {
                _percentsColorIndicatorBorder.Background = new SolidColorBrush(PercentsToColorConverter.Convert(_event.Percent));
            }
            //------------------
            _percentTextBlock = GetTemplateChild(PercentTextBlockPartName) as TextBlock;
            if (_percentTextBlock != null)
            {
                _percentTextBlock.Text = string.Format("{0}%", _event.Percent);
            }
            //------------------
            _timeTextBlock = GetTemplateChild(TimeTextBlockPartName) as TextBlock;
            if (_timeTextBlock != null)
            {
                _timeTextBlock.Text = string.Format("{0}ms", _event.Time);
            }
            //------------------
            _hitsTextBlock = GetTemplateChild(HitsTextBlockPartName) as TextBlock;
            if (_hitsTextBlock != null)
            {
                _hitsTextBlock.Text = string.Format("{0}hits", _event.Hits);
            }
            //------------------
            _nameTextBlock = GetTemplateChild(NameTextBlockPartName) as TextBlock;
            if (_nameTextBlock != null)
            {
                _nameTextBlock.Text = _eventFormatter.FormatName(_event);
            }
            //------------------
            _childrenItemsControl = GetTemplateChild(ChildrenItemsControlPartName) as ItemsControl;
            if (_childrenItemsControl != null)
            {
                _childrenItemsControl.Visibility = Visibility.Collapsed;
            }
        }

        private void ContainerGridOnMouseEnter(object sender, MouseEventArgs e)
        {
            IsHovered = true;
        }

        private void ContainerGridOnMouseLeave(object sender, MouseEventArgs e)
        {
            IsHovered = false;
        }

        private void ContainerGridOnMouseDown(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            if (_isExpanded)
            {
                Collapse();
            }
            else
            {
                Expand();
            }
            IsSelected = true;
        }

        private void InitializeChildren()
        {
            if (_areChildrenInitialized)
            {
                return;
            }
            if (_childrenItemsControl != null)
            {
                if (_event.HasChildren)
                {
                    List<EventsTreeItem> items = new List<EventsTreeItem>();
                    foreach (IEvent @event in _event.Children)
                    {
                        EventsTreeItem item = new EventsTreeItem(@event, _eventFormatter, _treeView);
                        items.Add(item);
                        _treeView.EventsTable.Add(@event, item);
                    }
                    _childrenItemsControl.ItemsSource = items;
                    DispatcherExtensions.DoEvents();
                }
                _areChildrenInitialized = true;
            }
        }

        ~EventsTreeItem()
        {
            Dispose();
        }

        public EventsTreeItem Navigate(IEvent targetEvent)
        {
            Expand();
            if (_event == targetEvent)
            {
                return this;
            }
            if (_childrenItemsControl == null)
            {
                Collapse();
                return null;
            }
            foreach (EventsTreeItem item in _childrenItemsControl.Items)
            {
                EventsTreeItem result = item.Navigate(targetEvent);
                if (result != null)
                {
                    return result;
                }
            }
            Collapse();
            return null;
        }

        public void Expand()
        {
            InitializeChildren();
            if (_childrenItemsControl != null)
            {
                _childrenItemsControl.Visibility = Visibility.Visible;
                DispatcherExtensions.DoEvents();
            }
            _isExpanded = true;
        }

        public void Collapse()
        {
            InitializeChildren();
            if (_childrenItemsControl != null)
            {
                _childrenItemsControl.Visibility = Visibility.Collapsed;
                DispatcherExtensions.DoEvents();
            }
            if (_childrenItemsControl != null)
            {
                foreach (EventsTreeItem child in _childrenItemsControl.Items)
                {
                    child.Collapse();
                }
            }
            _isExpanded = false;
        }

        public void Dispose()
        {
            ThreadStart threadStart = () =>
            {
                if (_headerPanel != null)
                {
                    _headerPanel.MouseDown -= ContainerGridOnMouseDown;
                }
            };
            Dispatcher.Invoke(threadStart);
        }

        public EventsTreeItem FindItem(IEvent @event)
        {
            if (_childrenItemsControl == null)
            {
                return null;
            }
            foreach (EventsTreeItem item in _childrenItemsControl.Items)
            {
                if (item.Event == @event)
                {
                    return item;
                }
            }
            return null;
        }

        private static void OnIsHoveredPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            EventsTreeItem item = (EventsTreeItem)sender;
            bool newValue = (bool)e.NewValue;
            bool oldValue = (bool)e.OldValue;
            if (newValue == oldValue)
            {
                return;
            }
            if (newValue)
            {
                item._treeView.HoveredEvent = item.Event;
            }
            else if (item._treeView.HoveredEvent == item.Event)
            {
                item._treeView.HoveredEvent = null;
            }
        }

        internal void SetIsSelectedInternal(bool value)
        {
            _notifySelection = false;
            IsSelected = value;
            _notifySelection = true;
        }

        private static void OnIsSelectedPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            EventsTreeItem item = (EventsTreeItem)sender;
            if (!item._notifySelection)
            {
                return;
            }
            bool newValue = (bool)e.NewValue;
            bool oldValue = (bool)e.OldValue;
            if (newValue == oldValue)
            {
                return;
            }
            if (newValue)
            {
                item._treeView.SetSelectedEventInternal(item.Event);
            }
            else if (item._treeView.SelectedEvent == item.Event)
            {
                item._treeView.SetSelectedEventInternal(null);
            }
        }
    }
}

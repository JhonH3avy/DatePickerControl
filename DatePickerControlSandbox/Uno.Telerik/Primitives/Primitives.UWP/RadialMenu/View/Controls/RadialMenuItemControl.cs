﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.UI.Automation.Peers;
using Telerik.UI.Xaml.Controls;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Automation.Peers;
using Windows.UI.Xaml.Input;

namespace Telerik.UI.Xaml.Controls.Primitives.Menu
{
    /// <summary>
    /// Represents the custom Control implementation used to visualize a <see cref="RadialMenuItem"/> and its children.
    /// </summary>
    public partial class RadialMenuItemControl : RadControl
    {
        /// <summary>
        /// Identifies the <see cref="Header"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register(nameof(Header), typeof(object), typeof(RadialMenuItemControl), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="IconContent"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IconContentProperty =
            DependencyProperty.Register(nameof(IconContent), typeof(object), typeof(RadialMenuItemControl), new PropertyMetadata(null));

        internal RadialSegment Segment;

        /// <summary>
        /// Identifies the <see cref="Loading"/> dependency property.
        /// </summary>
        private static readonly DependencyProperty LoadingProperty =
            DependencyProperty.Register(nameof(Loading), typeof(bool), typeof(RadialMenuItemControl), new PropertyMetadata(true, OnLoadingChanged));

        private bool isPointerOver;

        /// <summary>
        /// Initializes a new instance of the <see cref="RadialMenuItemControl" /> class.
        /// </summary>
        public RadialMenuItemControl()
        {
            this.DefaultStyleKey = typeof(RadialMenuItemControl);
        }

        /// <summary>
        /// Gets or sets the visual icon content of the current <see cref="RadialMenuItem"/>.
        /// </summary>
        public object IconContent
        {
            get
            {
                return (object)GetValue(IconContentProperty);
            }
            set
            {
                this.SetValue(IconContentProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the visual header of the current <see cref="RadialMenuItem"/>.
        /// </summary>
        public object Header
        {
            get
            {
                return (object)GetValue(HeaderProperty);
            }
            set
            {
                this.SetValue(HeaderProperty, value);
            }
        }

        internal new bool Loading
        {
            get
            {
                return (bool)this.GetValue(LoadingProperty);
            }
            set
            {
                this.SetValue(LoadingProperty, value);
            }
        }

        internal bool IsPointerOver
        {
            get
            {
                return this.isPointerOver;
            }
            set
            {
                this.isPointerOver = value;
                this.UpdateVisualState(true);
            }
        }

        internal void Update()
        {
            this.UpdateVisualState(true);
        }

        /// <summary>
        /// Builds the current visual state for this instance.
        /// </summary>
        protected override string ComposeVisualStateName()
        {
            string commonVisualState = base.ComposeVisualStateName();
            string selectionVisualState = (this.Segment != null && this.Segment.TargetItem.IsSelected) ? "Selected" : commonVisualState;
            commonVisualState = (this.Segment != null && this.IsPointerOver) ? "PointerOver" : selectionVisualState;
            commonVisualState = this.IsEnabled ? commonVisualState : base.ComposeVisualStateName();

            commonVisualState = this.Loading ? "Loading" : commonVisualState;

            return commonVisualState;
        }

        /// <summary>
        /// Called before the KeyDown event occurs.
        /// </summary>
        /// <param name="e">The data for the event.</param>
        protected override void OnKeyDown(KeyRoutedEventArgs e)
        {
            e.Handled = this.HandleKeyDown(e.Key);
            base.OnKeyDown(e);
        }

        /// <inheritdoc />
        protected override AutomationPeer OnCreateAutomationPeer()
        {
            var parent = ElementTreeHelper.FindVisualAncestor<RadRadialMenu>(this);
            if (parent != null)
            {
                return new RadialMenuItemControlAutomationPeer(this, parent);
            }

            return base.OnCreateAutomationPeer();
        }

        private static void OnLoadingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as RadialMenuItemControl;

            control.UpdateVisualState(true);
        }

        private bool HandleKeyDown(VirtualKey key)
        {
            if (this.Segment != null)
            {
                switch (key)
                {
                    case VirtualKey.Enter:
                    case VirtualKey.Space:
                        if (this.Segment.TargetItem.Selectable)
                        {
                            this.Segment.TargetItem.IsSelected = !this.Segment.TargetItem.IsSelected;
                        }

                        return true;
                    case VirtualKey.Left:
                        var canNavigateToNextItem = this.Segment.TargetItem.ParentItem != null
                            ? this.Segment.TargetItem.Index + 1 < this.Segment.TargetItem.ParentItem.ChildItems.Count
                            : this.Segment.TargetItem.Index + 1 < this.Segment.TargetItem.Owner.MenuItems.Count;
                        if (canNavigateToNextItem)
                        {
                            FocusManager.TryMoveFocus(FocusNavigationDirection.Next);
                        }

                        return true;
                    case VirtualKey.Right:
                        if (this.Segment.TargetItem.Index != 0)
                        {
                            FocusManager.TryMoveFocus(FocusNavigationDirection.Previous);
                        }

                        return true;
                }
            }

            return false;
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Core;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

#if NETFX_CORE
using DefaultPresenter = Windows.UI.Xaml.Controls.TextBlock;
#else
using DefaultPresenter = Windows.UI.Xaml.Controls.Border;
#endif

namespace Telerik.UI.Xaml.Controls.Input.Calendar
{
    // TODO: extract abstract ContentLayer class
    internal class XamlContentLayer : CalendarLayer
    {
        internal Dictionary<CalendarCellModel, DefaultPresenter> realizedCalendarCellDefaultPresenters;
        internal Queue<DefaultPresenter> recycledContainers;

        internal Dictionary<CalendarCellModel, FrameworkElement> realizedCalendarCellTemplatedPresenters;

        private Canvas animatableContentPanel;
        private Canvas contentPanelHost;

        public XamlContentLayer()
        {
            this.contentPanelHost = new Canvas();
            this.contentPanelHost.Background = new SolidColorBrush(Colors.Transparent);

            this.animatableContentPanel = new Canvas();
            this.contentPanelHost.Children.Add(this.animatableContentPanel);

            this.recycledContainers = new Queue<DefaultPresenter>();
            this.realizedCalendarCellDefaultPresenters = new Dictionary<CalendarCellModel, DefaultPresenter>();
            this.realizedCalendarCellTemplatedPresenters = new Dictionary<CalendarCellModel, FrameworkElement>();
        }

        internal Panel AnimatableContainer
        {
            get
            {
                return this.animatableContentPanel;
            }
        }

        internal Panel VisualContainer
        {
            get
            {
                return this.contentPanelHost;
            }
        }

        protected internal override UIElement VisualElement
        {
            get
            {
                return this.contentPanelHost;
            }
        }

        internal void UpdateUI(IEnumerable<CalendarCellModel> cellsToUpdate = null)
        {
            if (cellsToUpdate == null)
            {
                cellsToUpdate = this.Owner.Model.CalendarCells;
            }

            this.ClearTemplatedVisuals(cellsToUpdate);
            this.RecycleDefaultVisuals(cellsToUpdate);

            foreach (CalendarCellModel cell in cellsToUpdate)
            {
                FrameworkElement element = this.GetCalendarCellVisual(cell);

                if (element != null)
                {
                    RadRect layoutSlot = cell.layoutSlot;

                    // Tag is set only for default visuals
                    if (element.Tag is CalendarCellModel)
                    {
                        layoutSlot = XamlContentLayerHelper.ApplyLayoutSlotAlignment(element, layoutSlot);
                        XamlContentLayer.ArrangeUIElement(element, layoutSlot, false);
                    }
                    else
                    {
                        XamlContentLayer.ArrangeUIElement(element, layoutSlot);
                    }
                }
            }

			foreach (DefaultPresenter visual in this.recycledContainers)
            {
                visual.Visibility = Visibility.Collapsed;
            }

            this.UpdateCalendarViewClip();
        }

        internal void RecycleAllVisuals()
        {
            this.ClearTemplatedVisuals(this.realizedCalendarCellTemplatedPresenters.Keys.ToList());
            this.RecycleDefaultVisuals(this.realizedCalendarCellDefaultPresenters.Keys.ToList());
        }

        internal void UpdateCalendarViewClip()
        {
            RadRect clipRect = this.Owner.Model.AnimatableContentClip;
            RectangleGeometry clip = new RectangleGeometry();
            clip.Rect = new Rect(clipRect.X, clipRect.Y, clipRect.Width, clipRect.Height);

            this.contentPanelHost.Clip = clip;
        }

        protected internal override void AddVisualChild(UIElement child)
        {
            if (this.animatableContentPanel != null)
            {
                this.animatableContentPanel.Children.Add(child);
            }
        }

        protected internal override void RemoveVisualChild(UIElement child)
        {
            if (this.animatableContentPanel != null)
            {
                this.animatableContentPanel.Children.Remove(child);
            }
        }

        protected internal override void AttachUI(Panel parent)
        {
            base.AttachUI(parent);

            this.contentPanelHost.SizeChanged += this.AnimatableContentHostSizeChanged;
        }

        protected internal override void DetachUI(Panel parent)
        {
            this.contentPanelHost.SizeChanged -= this.AnimatableContentHostSizeChanged;

            base.DetachUI(parent);
        }

        private static bool ShouldRenderDefaultVisual(CalendarCellModel cell)
        {
            return cell.Context.CellTemplate == null;
        }

        private void AnimatableContentHostSizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.UpdateCalendarViewClip();
        }

        private void RecycleDefaultVisuals(IEnumerable<CalendarCellModel> cellsRangeToUpdate)
        {
            foreach (CalendarCellModel cellModel in cellsRangeToUpdate)
            {
				DefaultPresenter visual;

                if (this.realizedCalendarCellDefaultPresenters.TryGetValue(cellModel, out visual))
                {
                    this.realizedCalendarCellDefaultPresenters.Remove(cellModel);
                    this.recycledContainers.Enqueue(visual);

#if NETFX_CORE
					visual.ClearValue(TextBlock.StyleProperty);
#else
					// TODO UNO
					(visual.Child as TextBlock).ClearValue(TextBlock.StyleProperty);
#endif
				}
            }
        }

        private void ClearTemplatedVisuals(IEnumerable<CalendarCellModel> cellsRangeToUpdate)
        {
            if (this.realizedCalendarCellTemplatedPresenters.Count == 0)
            {
                return;
            }

            foreach (CalendarCellModel cellModel in cellsRangeToUpdate)
            {
                FrameworkElement templatedVisual;

                if (this.realizedCalendarCellTemplatedPresenters.TryGetValue(cellModel, out templatedVisual))
                {
                    this.RemoveVisualChild(templatedVisual);
                    this.realizedCalendarCellTemplatedPresenters.Remove(cellModel);
                }
            }
        }

        private FrameworkElement GetCalendarCellVisual(CalendarCellModel cell)
        {
            if (ShouldRenderDefaultVisual(cell))
            {
                return this.GetDefaultVisual(cell);
            }

            return this.GetTemplatedVisual(cell);
        }

        private FrameworkElement GetTemplatedVisual(CalendarCellModel cell)
        {
            DataTemplate cellTemplate = cell.Context.CellTemplate;
            FrameworkElement templatedVisual = cellTemplate.LoadContent() as FrameworkElement;
            if (templatedVisual == null)
            {
                return null;
            }

            templatedVisual.DataContext = cell;

            this.AddVisualChild(templatedVisual);
            this.realizedCalendarCellTemplatedPresenters.Add(cell, templatedVisual);

            return templatedVisual;
        }

        private FrameworkElement GetDefaultVisual(CalendarCellModel cell)
        {
#if NETFX_CORE
			TextBlock visual;

			if (this.recycledContainers.Count > 0)
			{
			    visual = this.recycledContainers.Dequeue();
			    visual.ClearValue(TextBlock.VisibilityProperty);
			    this.realizedCalendarCellDefaultPresenters.Add(cell, visual);
			}
			else
			{
			    visual = this.CreateDefaultVisual();
			    this.realizedCalendarCellDefaultPresenters.Add(cell, visual);
			}

			XamlContentLayerHelper.PrepareDefaultVisual(visual, cell);

			return visual;
#else
			// TODO UNO
			DefaultPresenter visual;

			if (this.recycledContainers.Count > 0)
			{
				visual = this.recycledContainers.Dequeue();
				visual.ClearValue(Border.VisibilityProperty);
				this.realizedCalendarCellDefaultPresenters.Add(cell, visual);
			}
			else
			{
				visual = this.CreateDefaultVisual();
				this.realizedCalendarCellDefaultPresenters.Add(cell, visual);
			}

			XamlContentLayerHelper.PrepareDefaultVisualUno(visual, cell);

			return visual;
#endif
		}

		private DefaultPresenter CreateDefaultVisual()
        {
#if NETFX_CORE
			TextBlock textBlock = new TextBlock();

			this.AddVisualChild(textBlock);

			return textBlock;
#else
			// TODO UNO
			TextBlock textBlock = new TextBlock();
			var border = new Border { Child = textBlock };

			this.AddVisualChild(border);

			return border;
#endif
		}
	}
}

﻿using Telerik.UI.Xaml.Controls.Input.Calendar;
using Windows.UI.Xaml.Automation.Peers;
using Windows.UI.Xaml.Automation.Provider;

namespace Telerik.UI.Automation.Peers
{
    /// <summary>
    /// AutomationPeer info class for calendar cells showing months, years and year ranges.
    /// </summary>
    public class CalendarInvokableCellInfoAutomationPeer : CalendarCellInfoBaseAutomationPeer, IInvokeProvider
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CalendarInvokableCellInfoAutomationPeer"/> class.
        /// </summary>
        /// <param name="parent">Parent CalendarViewHostAutomationPeer.</param>
        /// <param name="cellModel">The model of the calendar cell.</param>
        internal CalendarInvokableCellInfoAutomationPeer(CalendarViewHostAutomationPeer parent, CalendarCellModel cellModel) : base(parent, cellModel)
        {
            this.CalendarCellModel = cellModel;
        }

        internal CalendarCellModel CalendarCellModel { get; set; }

        /// <summary>
        /// IInvokeProvider implementation.
        /// </summary>
        public void Invoke()
        {
            if (this.CalendarViewHostPeer.CalendarOwner != null)
            {
                this.CalendarViewHostPeer.CalendarOwner.RaiseCellTapCommand(this.CalendarCellModel);
                this.RaiseAutomationEvent(AutomationEvents.InvokePatternOnInvoked);
            }
        }

        /// <inheritdoc />
        protected override object GetPatternCore(PatternInterface patternInterface)
        {
            if (patternInterface == PatternInterface.Invoke)
            {
                return this;
            }

            return base.GetPatternCore(patternInterface);
        }
    }
}

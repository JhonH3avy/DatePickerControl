﻿using Telerik.UI.Xaml.Controls.Primitives.Pagination;
using Windows.UI.Xaml.Automation.Peers;

namespace Telerik.UI.Automation.Peers
{
    /// <summary>
    /// Automation Peer for the PaginationListControl class.
    /// </summary>
    public class PaginationListControlAutomationPeer : ListBoxAutomationPeer
    {
        /// <summary>
        /// Initializes a new instance of the PaginationListControlAutomationPeer class.
        /// </summary>
        /// <param name="owner">The PaginationListControl that is associated with this PaginationListControlAutomationPeer.</param>
        public PaginationListControlAutomationPeer(PaginationListControl owner) 
            : base(owner)
        {
        }

        /// <inheritdoc />
        protected override string GetClassNameCore()
        {
            return nameof(Telerik.UI.Xaml.Controls.Primitives.Pagination.PaginationListControl);
        }

        /// <inheritdoc />
        protected override string GetHelpTextCore()
        {
            return nameof(Telerik.UI.Xaml.Controls.Primitives.Pagination.PaginationListControl);
        }

        /// <inheritdoc />
        protected override string GetLocalizedControlTypeCore()
        {
            return "pagination list control";
        }
    }
}

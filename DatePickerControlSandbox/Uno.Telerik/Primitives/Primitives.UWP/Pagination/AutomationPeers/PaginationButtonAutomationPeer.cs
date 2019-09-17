﻿using Telerik.UI.Xaml.Controls.Primitives.Pagination;
using Windows.UI.Xaml.Automation.Peers;

namespace Telerik.UI.Automation.Peers
{
    /// <summary>
    /// Automation Peer for the PaginationButton class.
    /// </summary>
    public class PaginationButtonAutomationPeer : ButtonAutomationPeer
    {
        /// <summary>
        /// Initializes a new instance of the PaginationButtonAutomationPeer class.
        /// </summary>
        /// <param name="owner">The PaginationButton that is associated with this PaginationButtonAutomationPeer.</param>
        public PaginationButtonAutomationPeer(PaginationButton owner) 
            : base(owner)
        {
        }

        /// <inheritdoc />
        protected override string GetClassNameCore()
        {
            return nameof(Telerik.UI.Xaml.Controls.Primitives.Pagination.PaginationButton);
        }

        /// <inheritdoc />
        protected override string GetHelpTextCore()
        {
            return nameof(Telerik.UI.Xaml.Controls.Primitives.Pagination.PaginationButton);
        }

        /// <inheritdoc />
        protected override string GetLocalizedControlTypeCore()
        {
            return "pagination button";
        }
    }
}

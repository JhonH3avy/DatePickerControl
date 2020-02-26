using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using DatePickerControlSandbox.Shared.Models;

namespace DatePickerControlSandbox.Shared.Helpers
{
    public class ParameterTemplateSelector : DataTemplateSelector
    {
        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            if (container is FrameworkElement && item != null && item is ReportParameterHolder holder)
            {
                if (holder.ParameterType == TypeCode.Boolean) return BoolHolder;
                if (holder.ParameterType == TypeCode.Integer) return IntHolder;
                if (holder.ParameterType == TypeCode.DateTime) return DateHolder;
                return StringHolder;
            }

            return null;
        }

        public DataTemplate DateHolder { get; set; }
        public DataTemplate BoolHolder { get; set; }
        public DataTemplate StringHolder { get; set; }
        public DataTemplate IntHolder { get; set; }
    }
}

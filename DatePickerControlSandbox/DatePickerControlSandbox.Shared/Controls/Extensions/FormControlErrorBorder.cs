using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;


namespace DatePickerControlSandbox.Shared.Controls.Extensions
{
    public class FormControlErrorBorder
    {
        public static readonly DependencyProperty HasErrorsProperty =
            DependencyProperty.RegisterAttached("HasErrors", typeof(bool), typeof(FormControlErrorBorder), new PropertyMetadata(false, HasErrorsChangedCallback));

        private static void HasErrorsChangedCallback(DependencyObject dependencyobject, DependencyPropertyChangedEventArgs args)
        {
            var element = dependencyobject as Control;
            if ((bool)args.NewValue)
            {
                ShowBorder(element);
            }
            else
            {
                HideBorder(element);
            }
        }

        private static void HideBorder(Control element)
        {
            if (element is IFormErrorBorder formControl)
            {
                formControl.BorderBrush = new SolidColorBrush(Color.FromArgb(102, 0, 0, 0));
            }
            else
            {
                element.BorderBrush = new SolidColorBrush(Color.FromArgb(102, 0, 0, 0));
            }
        }

        private static void ShowBorder(Control element)
        {
            if (element is IFormErrorBorder formControl)
            {
                formControl.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 255, 0, 0));
            }
            else
            {
                element.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 255, 0, 0));
            }
        }

        public static bool GetHasErrors(DependencyObject target)
        {
            return (bool)target.GetValue(HasErrorsProperty);
        }

        public static void SetHasErrors(DependencyObject target, bool hasErrors)
        {
            target.SetValue(HasErrorsProperty, hasErrors);
        }
    }

    public interface IFormErrorBorder
    {
        Brush BorderBrush { get; set; }
    }
}

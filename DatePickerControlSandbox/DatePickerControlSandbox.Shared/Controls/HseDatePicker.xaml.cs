using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Telerik.UI.Xaml.Controls.Input;
using Telerik.UI.Xaml.Controls.Input.Calendar;

namespace DatePickerControlSandbox.Shared.Controls
{
    public sealed partial class HseDatePicker : UserControl
    {
        public HseDatePicker()
        {
            this.InitializeComponent();
            DataContext = this;
            InputTextBox.AddHandler(KeyDownEvent, new KeyEventHandler((s, e) =>
            {
                if (e.Key == Windows.System.VirtualKey.Enter || e.Key == Windows.System.VirtualKey.Space)
                {
                    //DatePickerPart.SelectedDateRange = new CalendarDateRange(SelectedDateTime, SelectedDateTime);
                    //DatePickerPart.DisplayDate = SelectedDateTime;
                    UpdateSelectedDateTimeString(SelectedDateTime.ToString("dd/MM/yyyy"));
                }
                else if (e.Key == Windows.System.VirtualKey.Delete)
                {
                    SelectedDateTime = new DateTime();
                    //DatePickerPart.SelectedDateRange = null;
                    //DatePickerPart.MoveToDate(DateTime.Now);
                    UpdateSelectedDateTimeString("");
                }
            }), true);
        }

        public static readonly DependencyProperty PopupVisibilityProperty = DependencyProperty.Register(
            "PopupVisibility", typeof(bool), typeof(HseDatePicker), new PropertyMetadata(false));

        public bool PopupVisibility
        {
            get => (bool)GetValue(PopupVisibilityProperty);
            set => SetValue(PopupVisibilityProperty, value);
        }


        public static readonly DependencyProperty SelectedDateTimeProperty = DependencyProperty.Register(
            "SelectedDateTime", typeof(DateTime), typeof(HseDatePicker), new PropertyMetadata(null, SelectedValueChanged));

        private static void SelectedValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is HseDatePicker picker)
            {
                //picker.DatePickerPart.SelectedDateRange = new CalendarDateRange(picker.SelectedDateTime, picker.SelectedDateTime);
                //picker.DatePickerPart.DisplayDate = picker.SelectedDateTime;
            }
        }

        public DateTime SelectedDateTime
        {
            get => (DateTime)GetValue(SelectedDateTimeProperty);
            set
            {
                SetValue(SelectedDateTimeProperty, value);
                Value = SelectedDateTime;
            }
        }

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            "Value", typeof(DateTime), typeof(HseDatePicker), new PropertyMetadata(default(DateTime)));

        public DateTime Value
        {
            get => (DateTime)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        private void ShowPopupOffsetClicked(object sender, RoutedEventArgs e)
        {
            if (!StandardPopup.IsOpen) { StandardPopup.IsOpen = true; }
        }

        private void ClosePopupClicked(object sender, RoutedEventArgs e)
        {
            if (StandardPopup.IsOpen) { StandardPopup.IsOpen = false; }
        }

        private void CalendarView_OnCurrentDatesChanged(object sender, CurrentSelectionChangedEventArgs args)
        {
            if (args.NewSelection == null)
            {
                return;
            }
            SelectedDateTime = args.NewSelection.Value;
            UpdateSelectedDateTimeString(SelectedDateTime.ToString("dd/MM/yyyy"));
            ClosePopupClicked(this, null);
        }

        private void UpdateSelectedDateTimeString(string updatedString)
        {
            InputTextBox.Text = updatedString;
        }

        private void TextBox_OnBeforeTextChanging(TextBox sender, TextBoxBeforeTextChangingEventArgs args)
        {
            var quickDateRegex = new Regex(@"^([0-9]{0,2})(?:[./]?)([0-9]{0,2})(?:[./]?)([0-9]{0,4})(?:\s?)([0-9]{0,2})(?:[.:]?)([0-9]{0,2})$");
            var dateRegex = new Regex(@"[0-9]{1,2}\/?[0-9]{1,2}\/?[0-9]{4}\s?[0-9]{1,2}:?[0-9]{2}:?[0-9]{2}\s?(?:[Aa][Mm]|[Pp][Mm])?\s?[+-]?[0-9]{1,2}:?[0-9]{2}");
            if (quickDateRegex.IsMatch(args.NewText))
            {
                var groups = quickDateRegex.Match(args.NewText).Groups;
                var day = groups[1].Value;
                var month = groups[2].Value;
                var year = groups[3].Value;
                var hour = groups[4].Value;
                var minute = groups[5].Value;
                var parsedDay = string.IsNullOrEmpty(day) || day == "0" ? DateTime.Now.Day : int.Parse(day);
                var parsedMonth = string.IsNullOrEmpty(month) || month == "0" ? DateTime.Now.Month : int.Parse(month);
                var parsedYear = int.Parse(DateTime.Now.Year.ToString().Substring(0, DateTime.Now.Year.ToString().Length - year.Length) + year);
                var parsedHour = string.IsNullOrEmpty(hour) ? 12 : int.Parse(hour);
                var parsedMinute = string.IsNullOrEmpty(minute) ? 0 : int.Parse(minute);
                SelectedDateTime = new DateTime(parsedYear, parsedMonth, parsedDay, parsedHour, parsedMinute, 0);

            }
            else if (dateRegex.IsMatch(args.NewText))
            {
                args.Cancel = false;
            }
            else
            {
                args.Cancel = true;
            }
        }

        private void InputTextBox_OnLostFocus(object sender, RoutedEventArgs e)
        {
            UpdateSelectedDateTimeString(SelectedDateTime.ToString("dd/MM/yyyy"));
        }
    }
}

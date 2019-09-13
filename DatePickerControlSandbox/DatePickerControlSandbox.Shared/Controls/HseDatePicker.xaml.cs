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
        private DateTime tempDateTime;

        public HseDatePicker()
        {
            this.InitializeComponent();
            DataContext = this;
            tempDateTime = new DateTime();
            InputTextBox.AddHandler(KeyDownEvent, new KeyEventHandler((s, e) =>
            {
                if (e.Key == Windows.System.VirtualKey.Enter)
                {
                    DatePickerPart.SelectedDateRange = new CalendarDateRange(SelectedDateTime.Date, SelectedDateTime.Date);
                    DatePickerPart.MoveToDate(SelectedDateTime.Date);

                    //TimePickerPart.Time = SelectedDateTime.TimeOfDay;
                    HourPart.Text = SelectedDateTime.Hour.ToString();
                    MinutePart.Text = SelectedDateTime.Minute.ToString();
                    UpdateSelectedDateTimeString(SelectedDateTime.ToString());
                }
                else if (e.Key == Windows.System.VirtualKey.Delete)
                {
                    SelectedDateTime = new DateTimeOffset();
                    //TimePickerPart.Time = new TimeSpan(12, 0, 0);
                    HourPart.Text = "12";
                    MinutePart.Text = "00";
                    UpdateSelectedDateTimeString("");
                    DatePickerPart.SelectedDateRange = null;
                    DatePickerPart.MoveToDate(DateTime.Now);
                }
            }), true);
        }

        public static readonly DependencyProperty PopupVisibilityProperty = DependencyProperty.Register(
            "PopupVisibility", typeof(bool), typeof(HseDatePicker), new PropertyMetadata(false));

        public bool PopupVisibility
        {
            get { return (bool) GetValue(PopupVisibilityProperty); }
            set { SetValue(PopupVisibilityProperty, value); }
        }

        public static readonly DependencyProperty SelectedDateTimeProperty = DependencyProperty.Register(
            "SelectedDateTime", typeof(DateTimeOffset), typeof(HseDatePicker), new PropertyMetadata(default(DateTimeOffset)));

        public DateTimeOffset SelectedDateTime
        {
            get { return (DateTimeOffset) GetValue(SelectedDateTimeProperty); }
            set { SetValue(SelectedDateTimeProperty, value); }
        }

        public static readonly DependencyProperty OffsetProperty = DependencyProperty.Register(
            "Offset", typeof(TimeSpan), typeof(HseDatePicker), new PropertyMetadata(default(TimeSpan)));

        public TimeSpan Offset
        {
            get { return (TimeSpan) GetValue(OffsetProperty); }
            set { SetValue(OffsetProperty, value); }
        }

        public static readonly DependencyProperty SelectedDateTimeStringProperty = DependencyProperty.Register(
            "SelectedDateTimeString", typeof(string), typeof(HseDatePicker), new PropertyMetadata(default(string)));

        public string SelectedDateTimeString
        {
            get { return (string) GetValue(SelectedDateTimeStringProperty); }
            set { SetValue(SelectedDateTimeStringProperty, value); }
        }

        private void ShowPopupOffsetClicked(object sender, RoutedEventArgs e)
        {
            if (!StandardPopup.IsOpen) { StandardPopup.IsOpen = true; }
        }

        private void ClosePopupClicked(object sender, RoutedEventArgs e)
        {
            if (StandardPopup.IsOpen) { StandardPopup.IsOpen = false; }
        }

        //private void CalendarView_OnSelectedDatesChanged(CalendarView sender, CalendarViewSelectedDatesChangedEventArgs args)
        //{
        //    if (args.AddedDates.Count <= 0) return;
        //    var selectedDate = args.AddedDates[0];
        //    if (selectedDate == null) return;
        //    var builder = new DateTimeOffset(selectedDate.Year, selectedDate.Month, selectedDate.Day, SelectedDateTime.Hour, SelectedDateTime.Minute, 0, SelectedDateTime.Offset);
        //    SelectedDateTime = builder;
        //    UpdateSelectedDateTimeString(SelectedDateTime.ToString());
        //}

        private void CalendarView_OnCurrentDatesChanged(object sender, CurrentSelectionChangedEventArgs args)
        {
            Console.WriteLine("Callback call");
            if (args.NewSelection == null) return;
            Console.WriteLine("There is a new value");
            var selectedDate = args.NewSelection.Value;
            var builder = new DateTimeOffset(selectedDate.Year, selectedDate.Month, selectedDate.Day, SelectedDateTime.Hour, SelectedDateTime.Minute, 0, SelectedDateTime.Offset);
            SelectedDateTime = builder;
            UpdateSelectedDateTimeString(SelectedDateTime.ToString());
        }

        //private void TimePicker_OnTimeChanged(object sender, TimePickerValueChangedEventArgs e)
        //{
        //    var selectedTime = e.NewTime;
        //    if (selectedTime == null) return;
        //    var builder = new DateTimeOffset(SelectedDateTime.Year, SelectedDateTime.Month, SelectedDateTime.Day, selectedTime.Hours, selectedTime.Minutes, 0, SelectedDateTime.Offset);
        //    SelectedDateTime = builder;
        //    UpdateSelectedDateTimeString(SelectedDateTime.ToString());
        //}

        private void UpdateSelectedDateTimeString(string updatedString)
        {
            SelectedDateTimeString = updatedString;
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
                var parsedDay = string.IsNullOrEmpty(day) || day == "0" ? DateTimeOffset.Now.Day : int.Parse(day);
                var parsedMonth = string.IsNullOrEmpty(month) || month == "0" ? DateTimeOffset.Now.Month : int.Parse(month);
                var parsedYear = int.Parse(DateTimeOffset.Now.Year.ToString().Substring(0, DateTimeOffset.Now.Year.ToString().Length - year.Length) + year);
                var parsedHour = string.IsNullOrEmpty(hour) ? 12 : int.Parse(hour);
                var parsedMinute = string.IsNullOrEmpty(minute) ? 0 : int.Parse(minute);
                var builder = new DateTimeOffset(parsedYear, parsedMonth, parsedDay, parsedHour, parsedMinute, 0, Offset);
                SelectedDateTime = builder;
                
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

        private void TimePickerHourPart_OnBeforeTextChanging(TextBox sender, TextBoxBeforeTextChangingEventArgs args)
        {
            var numberRegex = new Regex(@"^([0-9]|1[0-9]|2[0-3])?$");
            if (numberRegex.IsMatch(args.NewText))
            {
                var builder = new DateTimeOffset(SelectedDateTime.Year, SelectedDateTime.Month, SelectedDateTime.Day,
                    string.IsNullOrEmpty(args.NewText) ? 0 : int.Parse(args.NewText), SelectedDateTime.Minute, 0, SelectedDateTime.Offset);
                SelectedDateTime = builder;
                UpdateSelectedDateTimeString(SelectedDateTime.ToString());
            }
            else
            {
                args.Cancel = true;
            }
        }

        private void TimePickerMinutePart_OnBeforeTextChanging(TextBox sender, TextBoxBeforeTextChangingEventArgs args)
        {
            var numberRegex = new Regex(@"^([0-5]?[0-9]?)$");
            if (numberRegex.IsMatch(args.NewText))
            {
                var builder = new DateTimeOffset(SelectedDateTime.Year, SelectedDateTime.Month, SelectedDateTime.Day, SelectedDateTime.Hour,
                    string.IsNullOrEmpty(args.NewText) ? 0 : int.Parse(args.NewText), 0, SelectedDateTime.Offset);
                SelectedDateTime = builder;
                UpdateSelectedDateTimeString(SelectedDateTime.ToString());
            }
            else
            {
                args.Cancel = true;
            }
        }
    }
}

﻿using System;
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
                    DatePickerPart.DisplayDate = SelectedDateTime.Date;
                    UpdateSelectedDateTimeString(SelectedDateTime.ToString("d"));
                }
                else if (e.Key == Windows.System.VirtualKey.Delete)
                {
                    SelectedDateTime = new DateTimeOffset();
                    DatePickerPart.SelectedDateRange = null;
                    DatePickerPart.MoveToDate(DateTime.Now);
                    UpdateSelectedDateTimeString("");
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

        private void CalendarView_OnCurrentDatesChanged(object sender, CurrentSelectionChangedEventArgs args)
        {
            Console.WriteLine("Callback call");
            if (args.NewSelection == null) return;
            Console.WriteLine("There is a new value");
            var selectedDate = args.NewSelection.Value;
            Console.WriteLine($"Selected date: {selectedDate.ToString("dd/MM/yyyy")}");
            var builder = new DateTimeOffset(selectedDate.Year, selectedDate.Month, selectedDate.Day, SelectedDateTime.Hour, SelectedDateTime.Minute, 0, SelectedDateTime.Offset);
            SelectedDateTime = builder;
            UpdateSelectedDateTimeString(SelectedDateTime.ToString("dd/MM/yyyy"));
        }
        
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
    }
}

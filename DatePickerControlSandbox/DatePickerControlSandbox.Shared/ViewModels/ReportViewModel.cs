using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;
using DatePickerControlSandbox.Shared.Models;
using DatePickerControlSandbox.Shared.Services;

namespace DatePickerControlSandbox.Shared.ViewModels
{
    public class ReportViewModel : BaseViewModel, IOKCancelViewModel
    {
        private IList<ReportParameterHolder> reportParameters;
        public IList<ReportParameterHolder> ReportParameters
        {
            get => reportParameters;
            set
            {
                reportParameters = value;
                OnPropertyChanged();
            }
        }

        private bool isBusy;
        public bool IsBusy
        {
            get => isBusy;
            set
            {
                if (value != isBusy)
                {
                    isBusy = value;
                    OnPropertyChanged();
                    OkCommand?.InvalidateCanExecute();
                    CancelCommand?.InvalidateCanExecute();
                }
            }
        }

        public ReportViewModel()
        {
            var service = new ReportService();
            ReportParameters = service.GetReports();
            IsBusy = true;
            Title = "Test report";
            OkText = "OK";
            CancelText = "Cancel";
            //OkCommand = new SimpleCommand(async (param) =>
            //{
            //    if (!IsValid() && param is ContentDialogClosingEventArgs args)
            //    {
            //        await new MessageDialog("Please complete all the mandatory parameters and correct any validation errors").ShowAsync();
            //        args.Cancel = true;
            //    }
            //});
            //CancelCommand = new SimpleCommand((param) =>
            //{
            //    Console.WriteLine("Cancel command invoked");
            //});
        }

        public void Dispose()
        {
            
        }

        public bool OK(ContentDialogClosingEventArgs args)
        {
            if (!IsValid())
            {
                foreach (var holder in ReportParameters)
                {
                    holder.InvokeErrorsChanged(null);
                }
                new MessageDialog("Please complete all the mandatory parameters and correct any validation errors").ShowAsync();
                args.Cancel = true;
                return false;
            }

            return true;
        }

        public bool Cancel()
        {
            return true;
        }

        public bool OkCanExecute(object obj)
        {
            return !IsBusy;
        }

        public bool CancelCanExecute(object obj)
        {
            return !IsBusy;
        }

        public IInvalidateCommand OkCommand { get; set; }
        public IInvalidateCommand CancelCommand { get; set; }
        public string OkText { get; }
        public string CancelText { get; }
        public object View { get; set; }
        public string Title { get; }

        private bool IsValid()
        {
            foreach (var holder in ReportParameters)
            {
                holder.Validate();
            }
            return ReportParameters.Where(p => p.Visible).Aggregate(true, (current, h) => {
                Console.WriteLine($"Holder:{h.Name} Has Errors:{h.HasErrors} Value:{h.Value}");
                return current && !h.HasErrors;
            });
        }
    }
}

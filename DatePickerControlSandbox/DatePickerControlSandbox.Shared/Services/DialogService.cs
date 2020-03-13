using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml.Controls;
using DatePickerControlSandbox.Shared.ViewModels;

namespace DatePickerControlSandbox.Shared.Services
{
    class DialogService
    {
        public static readonly DialogService Instance = new DialogService();

        DialogService()
        {
        }

        public async void Show(object dialogContent, IOKCancelViewModel viewModel)
        {
            var contentDialog = new ContentDialog
            {
                Content = dialogContent,
                PrimaryButtonText = "Ok",
                CloseButtonText = "Cancel",
                MinWidth = 1000,
            };
            if (viewModel != null)
            {
                Console.WriteLine("hooking");
                contentDialog.DataContext = viewModel;
                contentDialog.Closing += (sender, args) =>
                {
                    Console.WriteLine("Closing " + args.Result);

                    if (args.Result == ContentDialogResult.Primary)
                    {
                        viewModel.OK(args);
                    }
                };
                //contentDialog.PrimaryButtonCommand = viewModel.OkCommand;
                //contentDialog.SecondaryButtonCommand = viewModel.CancelCommand;
            }
            var res = await contentDialog.ShowAsync(ContentDialogPlacement.Popup);
            //if (res == ContentDialogResult.Primary)
            //{
            //    viewModel.OkCommand.Execute(null);
            //}
            //else
            //{
            //    viewModel.CancelCommand.Execute(null);
            //}
        }
    }
}

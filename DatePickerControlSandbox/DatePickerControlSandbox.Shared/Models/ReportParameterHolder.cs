using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using DatePickerControlSandbox.Shared.Resources;
using DatePickerControlSandbox.Shared.ViewModels;

namespace DatePickerControlSandbox.Shared.Models
{
    public class ReportParameterHolder : BaseViewModel
    {
        private readonly Dictionary<String, List<ValidationResult>> errors = new Dictionary<string, List<ValidationResult>>();

        public TypeCode ParameterType { get; set; }

        public string Name { get; set; }

        private object value;
        public object Value
        {
            get => value;
            set
            {
                this.value = value;
                OnPropertyChanged();
                Validate();
            }
        }

        public string Prompt { get; set; }

        public bool Visible { get; set; }

        public bool Nullable { get; set; }

        public bool HasValue => Value != null;

        public bool HasErrors => errors.Count > 0;

        public bool Validate()
        {
            Console.WriteLine($"Doing validation");
            errors.Clear();
            //if (!Nullable && !HasValue)
            //{
            //    errors.Add("Value", new List<ValidationResult>
            //    {
            //        new ValidationResult("Value is mandatory", new [] {"Value"})
            //    });
            //    InvokeErrorsChanged(new DataErrorsChangedEventArgs("Value"));
            //}
            //if (HasValue && ParameterType == TypeCode.DateTime
            //             && Value is DateTime dateTime
            //             && (dateTime < StaticParameters.MinDate || dateTime > StaticParameters.MaxDate))
            //{
            //    errors.Add("Value", new List<ValidationResult>
            //    {
            //        new ValidationResult(StaticParameters.DateInvalidMessage, new [] {"Value"})
            //    });
            //    InvokeErrorsChanged(new DataErrorsChangedEventArgs("Value"));
            //}
            //if (HasErrors)
            //{
            //    Console.WriteLine($"Name:{Name} Value:{Value}");
            //    foreach (var error in errors)
            //    {
            //        foreach (var innerError in error.Value)
            //        {
            //            Console.WriteLine($"ErrorKey:{error.Key} ErrorValue:{innerError.ErrorMessage}");
            //        }
            //    }
            //}
            errors.Add("Value", new List<ValidationResult>
            {
                new ValidationResult("Value is mandatory", new [] {"Value"})
            });
            InvokeErrorsChanged(new DataErrorsChangedEventArgs("Value"));
            return HasErrors;
        }
        //public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public void InvokeErrorsChanged(DataErrorsChangedEventArgs e)
        {
            //ErrorsChanged?.Invoke(this, e);
            OnPropertyChanged(nameof(HasErrors));
        }
    }

    public enum TypeCode
    {
        Integer,
        Boolean,
        String,
        DateTime
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace DatePickerControlSandbox.Shared.Models
{
    public class ValidationResult
    {
        public static readonly ValidationResult Success;

        protected ValidationResult(ValidationResult validationResult)
        {
            ErrorMessage = validationResult.ErrorMessage;
            MemberNames = validationResult.MemberNames;
        }

        public ValidationResult(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }

        public ValidationResult(string errorMessage, IEnumerable<string> memberNames)
        {
            ErrorMessage = errorMessage;
            MemberNames = memberNames;
        }

        public string ErrorMessage { get; set; }

        public IEnumerable<string> MemberNames { get; }

        public override string ToString()
        {
            return ErrorMessage;
        }
    }
}

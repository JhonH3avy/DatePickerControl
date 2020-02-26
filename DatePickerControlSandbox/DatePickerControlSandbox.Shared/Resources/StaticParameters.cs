using System;
using System.Collections.Generic;
using System.Text;

namespace DatePickerControlSandbox.Shared.Resources
{
    public static class StaticParameters
    {
        public static readonly string DateInvalidMessage = "";

        public static readonly DateTime MaxDate = new DateTime(10000, 12, 31, 23, 59, 59);

        public static readonly DateTime MinDate = new DateTime(1900, 1, 1, 0, 0, 0);
    }
}

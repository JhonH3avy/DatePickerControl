using System;
using System.Collections.Generic;
using System.Text;
using DatePickerControlSandbox.Shared.Models;
using TypeCode = DatePickerControlSandbox.Shared.Models.TypeCode;

namespace DatePickerControlSandbox.Shared.Services
{
    public class ReportService
    {
        public IList<ReportParameterHolder> GetReports()
        {
            return new List<ReportParameterHolder>
            {
                new ReportParameterHolder{Name = "BusinessUnit", Prompt = "Business Unit", ParameterType = TypeCode.String, Nullable = false, Visible = true},
                new ReportParameterHolder{Name = "CompanyName", Prompt = "Company Name", ParameterType = TypeCode.String, Nullable = false, Visible = true},
                new ReportParameterHolder{Name = "StartDate", Prompt = "Start Date", ParameterType = TypeCode.DateTime, Nullable = false, Visible = true}
            };
        }
    }
}

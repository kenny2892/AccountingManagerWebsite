using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AccountingWebsite.ViewModels
{
    public class ReportsViewModel
    {
        public string DefaultOption { get; } = "--Blank--";
        public Dictionary<string, string[]> CategoryOptions { get; set; }
        public DropdownCheckboxViewModel Banks { get; set; }
        public DropdownCheckboxViewModel Vendors { get; set; }
        public string[] TimeframeOptions { get; set; }

        public ReportsViewModel(Dictionary<string, string[]> categoryOptions, DropdownCheckboxViewModel banks, DropdownCheckboxViewModel vendors)
        {
            CategoryOptions = categoryOptions;
            Banks = banks;
            Vendors = vendors;
            TimeframeOptions = new string[] { "This Week", "Last Week", "This Month", "Last Month", "This Year", "Last Year", "Custom" };
        }
    }
}

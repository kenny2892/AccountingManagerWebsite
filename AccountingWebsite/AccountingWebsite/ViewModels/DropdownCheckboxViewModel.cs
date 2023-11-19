using System.Collections.Generic;

namespace AccountingWebsite.ViewModels
{
    public class DropdownCheckboxViewModel
    {
        public List<string> FormNames { get; set; }
        public List<string> Values { get; set; }
        public string Prompt { get; set; }
    }
}

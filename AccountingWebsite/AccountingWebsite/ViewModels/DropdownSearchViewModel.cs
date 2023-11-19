using System.Collections.Generic;

namespace AccountingWebsite.ViewModels
{
    public class DropdownSearchViewModel
    {
        public string DropdownValueID { get; set; }
        public string PromptText { get; set; }
        public string SelectedOption { get; set; }
        public List<string> Options { get; set; } = new List<string>();
    }
}

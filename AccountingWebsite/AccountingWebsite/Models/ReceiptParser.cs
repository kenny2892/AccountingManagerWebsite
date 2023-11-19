using AccountingWebsite.ViewModels;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Tesseract;

namespace AccountingWebsite.Models
{
    public class ReceiptParser
    {
        public static ReceiptEditViewModel ParseDetails(List<Vendor> vendors, string filePath)
        {
            var receiptImg = new Bitmap(filePath);
            var receiptText = GetTextFromBitmap(receiptImg);
            receiptText = FixCommonMispellings(receiptText);
            var matchingVendor = vendors.Where(vendor => 
                vendor.ReceiptKeyphrases.Any(keyphrase => Regex.Match(receiptText, $@"\b{keyphrase}\b", RegexOptions.IgnoreCase).Success)).FirstOrDefault();
            var amount = FindTotal(receiptText.Split("\n").ToList(), matchingVendor);
            var purchaseDate = FindDate(receiptText.Split("\n").ToList());

            return new ReceiptEditViewModel() { FileName = Path.GetFileName(filePath), Vendor = matchingVendor != null ? matchingVendor.Name : "", PurchaseDate = purchaseDate, Amount = amount };
        }

        private static TesseractEngine MakeTesseractEngine()
        {
            TesseractEngine engine = new TesseractEngine(@"C:\Users\Kelly\Documents\Tesseract Language Data\tessdata", "eng");
            engine.SetVariable("tessedit_char_blacklist", "£¥"); // Blacklist characters
            engine.SetVariable("tessedit_pageseg_mode", "PSM_SINGLE_BLOCK"); // Treat page as 1 block of text
            engine.SetVariable("debug_level", "0"); // Disables debug messages
            engine.SetVariable("debug_file", "NUL"); // Redirects output to null

            return engine;
        }

        private static string GetTextFromBitmap(Bitmap image, TesseractEngine engine = null)
        {
            if(engine is null)
            {
                engine = MakeTesseractEngine();
            }

            Pix picture = PixConverter.ToPix(image);
            picture = picture.Deskew();
            using Page page = engine.Process(picture);
            return page.GetText().Replace("TOTA!", "TOTAL");
        }

        private static List<string> GetHtmlTextElements(string htmlText)
        {
            List<string> text = new List<string>();
            var doc = new HtmlDocument();
            doc.LoadHtml(htmlText);

            FindAllInnerText(doc.DocumentNode, text);
            return text;
        }

        private static void FindAllInnerText(HtmlNode toSearch, List<string> text)
        {
            if(!String.IsNullOrEmpty(toSearch.InnerHtml) && !toSearch.InnerHtml.Contains("<"))
            {
                text.Add(toSearch.InnerHtml);
            }

            var elementsToCheck = toSearch.Descendants().Where(element => element.ParentNode == toSearch && !element.Name.Contains("#"));
            foreach(var element in elementsToCheck)
            {
                FindAllInnerText(element, text);
            }
        }

        private static string FixCommonMispellings(string receiptText)
        {
            List<(string, string)> corrections = new List<(string, string)>();
            corrections.Add(("tota]", "total"));
            corrections.Add(("totai", "total"));
            corrections.Add(("magstercard", "mastercard"));

            foreach(var pair in corrections)
            {
                receiptText = receiptText.Replace(pair.Item1, pair.Item2, StringComparison.CurrentCultureIgnoreCase);
            }

            return receiptText;
        }

        private static double FindTotal(List<string> receiptLines, Vendor vendor)
        {
            if(vendor is null)
            {
                return 0.0;
            }

            Regex priceRegex = new Regex("[\\d]+[ ]*[.,][ ]*[\\d]+");
            string price = "";

            // Search using the entered keyphrases
            foreach(string keyphrase in vendor.ReceiptTotalKeyphrases.Select(keyphrase => keyphrase.ToLower()))
            {
                if(receiptLines.Any(line => line.ToLower().Contains(keyphrase) && priceRegex.IsMatch(line)))
                {
                    string line = receiptLines.Where(line => line.ToLower().Contains(keyphrase) && priceRegex.IsMatch(line)).First();
                    price = priceRegex.Match(line).Value.Replace(" ", "").Replace(",", ".");
                }
            }

            // Search for the word 'Total'
            if(String.IsNullOrEmpty(price) && receiptLines.Any(line => HasTheWordTotalOnly(line) && priceRegex.IsMatch(line)))
            {
                string line = receiptLines.Where(line => HasTheWordTotalOnly(line) && priceRegex.IsMatch(line)).First();
                price = priceRegex.Match(line).Value.Replace(" ", "").Replace(",", ".");
            }

            if(double.TryParse(price, out double num))
            {
                return num;
            }

            return 0.0;
        }

        private static bool HasTheWordTotalOnly(string line)
        {
            return line.ToLower().Contains("total") && !line.ToLower().Contains("sub") && !line.ToLower().Contains("tax") && !line.ToLower().Contains("item") &&
                !line.ToLower().Contains("tip") && !Regex.IsMatch(line, ".total");
        }

        private static DateTime FindDate(List<string> receiptLines)
        {
            Regex dateNumRegex = new Regex("(\\s|^|:)[\\d]{1,2}[-/][\\d]{1,2}[-/][\\d]{2,4}");
            Regex backwardsDateNumRegex = new Regex("(\\s|^|:)[\\d]{4}[-/][\\d]{1,2}[-/][\\d]{1,2}");
            Regex dateNumWithLetterInFrontRegex = new Regex("(B|O)[\\d]{1,2}[-/][\\d]{1,2}[-/][\\d]{2,4}"); // In case an 0 gets read as a B or O
            Regex dateWordRegex = new Regex("(?:jan(?:uary)?|feb(?:ruary)?|mar(?:ch)?|apr(?:il)?|may|jun(?:e)?|jul(?:y)?|aug(?:ust)?|sep(?:tember)?|oct(?:ober)?|(nov|dec)(?:ember)?) [\\d]{1,2}[,]{0,1} \\d\\d\\d\\d");

            string date = "";
            foreach(string line in receiptLines)
            {
                if(dateNumRegex.IsMatch(line))
                {
                    date = dateNumRegex.Match(line).Value.Trim().Replace(":", "");

                    if(!DateTime.TryParse(date, out DateTime result1))
                    {
                        date = "";
                    }

                    else
                    {
                        break;
                    }
                }

                else if(backwardsDateNumRegex.IsMatch(line))
                {
                    date = backwardsDateNumRegex.Match(line).Value;

                    if(!DateTime.TryParse(date, out DateTime result1))
                    {
                        date = "";
                    }

                    else
                    {
                        break;
                    }
                }

                else if(dateNumWithLetterInFrontRegex.IsMatch(line))
                {
                    date = Regex.Replace(dateNumWithLetterInFrontRegex.Match(line).Value, "[A-Za-z]", "");

                    if(!DateTime.TryParse(date, out DateTime result1))
                    {
                        date = "";
                    }

                    else
                    {
                        break;
                    }
                }

                else if(dateWordRegex.IsMatch(line.ToLower()))
                {
                    date = dateWordRegex.Match(line.ToLower()).Value;

                    if(!DateTime.TryParse(date, out DateTime result1))
                    {
                        date = "";
                    }

                    else
                    {
                        break;
                    }
                }
            }

            if(DateTime.TryParse(date, out DateTime result))
            {
                return result;
            }

            return DateTime.Parse("1/1/1990");
        }
    }
}

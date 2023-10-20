using AccountingWebsite.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace AccountingWebsite.Controllers
{
    public class BaseController : Controller
    {
        public string GetThemeName()
        {
            return GetCookieString("Theme");
        }

        public int GetThemeIndex(string theme)
        {
            int index = 0;
            var themes = Enum.GetNames(typeof(Themes));

            for(int i = 0; i < themes.Length; i++)
            {
                if(themes[i] == theme)
                {
                    index = i;
                    break;
                }
            }

            return index;
        }

        public bool SetTheme(string themeToSetTo)
        {
            return SetCookieString("Theme", themeToSetTo);
        }

        public string GetCookieString(string key)
        {
            if(key is null)
            {
                return "";
            }

            string value = HttpContext.Request.Cookies[key];
            return !String.IsNullOrEmpty(value) ? value : "";
        }

        public bool SetCookieString(string key, string value, int expireTimeInMinutes = -1)
        {
            if(String.IsNullOrEmpty(key) || value is null)
            {
                return false;
            }

            CookieOptions option = new CookieOptions();
            option.SameSite = SameSiteMode.Strict;

            if(expireTimeInMinutes > 0)
            {
                option.Expires = DateTime.Now.AddMinutes(expireTimeInMinutes);
            }

            else
            {
                option.Expires = DateTime.Now.AddMonths(12);
            }

            Response.Cookies.Append(key, value, option);
            return true;
        }
    }
}

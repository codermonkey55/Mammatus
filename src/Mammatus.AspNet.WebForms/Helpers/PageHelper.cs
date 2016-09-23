using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Mammatus.Extensions;

namespace Mammatus.AspNet.WebForms.Helpers
{
    public class PageHelper
    {
        public static void LockPage(Page page, object[] obj)
        {
            Control htmlForm = null;
            foreach (Control ctl in page.Controls)
            {
                if (ctl is HtmlForm)
                {
                    htmlForm = ctl;
                    break;
                }
            }

            if (htmlForm != null)
                foreach (Control ctl in htmlForm.Controls)
                {
                    if (IsContains(obj, ctl) == false)
                    {
                        LockControl(page, ctl);
                    }
                    else
                    {
                        UnLockControl(page, ctl);
                    }
                }
        }

        public static void UnLockPage(Page page, object[] obj)
        {
            Control htmlForm = null;
            foreach (Control ctl in page.Controls)
            {
                if (ctl is HtmlForm)
                {
                    htmlForm = ctl;
                    break;
                }
            }

            if (htmlForm != null)
                foreach (Control ctl in htmlForm.Controls)
                {
                    if (IsContains(obj, ctl) == false)
                    {
                        UnLockControl(page, ctl);
                    }
                    else
                    {
                        LockControl(page, ctl);
                    }
                }
        }

        private static void LockControl(Page page, Control ctl)
        {
            //WebControl
            if (ctl is Button || ctl is CheckBox || ctl is HyperLink || ctl is LinkButton
                || ctl is ListControl || ctl is TextBox)
            {
                ((WebControl)ctl).Enabled = false;


                if (ctl is TextBox)
                {
                    if (((TextBox)ctl).TextMode == TextBoxMode.MultiLine)
                    {
                        ((TextBox)ctl).Enabled = true;
                        ((TextBox)ctl).ReadOnly = true;
                    }
                }
            }

            //HtmlControl
            var file = ctl as HtmlInputFile;
            if (file != null)
            {
                file.Disabled = true;
            }
        }

        private static void UnLockControl(Page page, Control ctl)
        {
            //WebControl
            if (ctl is Button || ctl is CheckBox || ctl is HyperLink || ctl is LinkButton
                || ctl is ListControl || ctl is TextBox)
            {
                ((WebControl)ctl).Enabled = true;

                if (ctl is TextBox)
                {
                    ((TextBox)ctl).ReadOnly = false;
                }
            }

            //HtmlControl
            if (ctl is HtmlInputFile)
            {
                ((HtmlInputFile)ctl).Disabled = false;
            }
        }

        private static bool IsContains(object[] obj, Control ctl)
        {
            foreach (Control c in obj)
            {
                if (c.ID == ctl.ID)
                {
                    return true;
                }
            }
            return false;
        }

        public static Page GetCurrentPage()
        {
            return (Page)HttpContext.Current.Handler;
        }

        public static string GetPageName()
        {
            string url = HttpContext.Current.Request.RawUrl;
            var start = url.LastIndexOf("/", StringComparison.Ordinal) + 1;
            var end = url.IndexOf("?", StringComparison.Ordinal);
            if (end <= 0)
            {
                return url.Substring(start, url.Length - start);
            }
            else
            {
                return url.Substring(start, end - start);
            }
        }

        public static string GetQueryString(string queryStringName)
        {
            if ((HttpContext.Current.Request.QueryString[queryStringName] != null) &&
                (HttpContext.Current.Request.QueryString[queryStringName] != "undefined"))
            {
                return HttpContext.Current.Request.QueryString[queryStringName].Trim();
            }
            return "";
        }

        public void Redirect(string url)
        {
            Page page = GetCurrentPage();
            page.Response.Redirect(url);
        }

        public static string GetRelativeLevel()
        {
            string applicationPath = HttpContext.Current.Request.ApplicationPath;
            if (applicationPath != null && applicationPath.Trim() == "/")
            {
                applicationPath = "";
            }

            int i = applicationPath == "" ? 1 : 2;
            return "";//Nandasoft.Helper.NDHelperString.Repeat("../", Nandasoft.Helper.NDHelperString.RepeatTime(HttpContext.Current.Request.Path, "/") - i);
        }

        public static void WriteScript(string script)
        {
            Page page = GetCurrentPage();

            // NDGridViewScriptFirst(page.Form.Controls, page);

            //ScriptManager.RegisterStartupScript(page, page.GetType(), System.Guid.NewGuid().ToString(), script, true);

        }

        public static int GetClientBrowserVersion()
        {
            string USER_AGENT = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_USER_AGENT"];

            if (USER_AGENT.IndexOf("MSIE", StringComparison.Ordinal) < 0) return -1;

            string version = USER_AGENT.Substring(USER_AGENT.IndexOf("MSIE", StringComparison.Ordinal) + 5, 1);

            if (!StringExtensions.IsInt(version))
                return -1;

            return Convert.ToInt32(version);
        }

    }
}

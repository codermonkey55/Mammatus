using System.Web;

namespace Mammatus.Library.Web
{
    /// <summary>
    /// JavaScript 操作类
    /// </summary>
    public class JavaScriptPlus
    {
        public JavaScriptPlus()
        { }

        public static void ResponseScript(System.Web.UI.Page page, string script)
        {
            page.ClientScript.RegisterStartupScript(page.GetType(), "message", "<script language='javascript' defer>" + script + "</script>");

        }

        public static void GoHistory(int value)
        {

            string js = @"<Script language='JavaScript'>
                    history.go({0});  
                  </Script>";
            HttpContext.Current.Response.Write(string.Format(js, value));

        }


        public static void CloseWindow()
        {

            string js = @"<Script language='JavaScript'>
                    parent.opener=null;window.close();  
                  </Script>";
            HttpContext.Current.Response.Write(js);
            HttpContext.Current.Response.End();

        }


        public static void RefreshParent(string url)
        {

            string js = @"<script>try{top.location=""" + url + @"""}catch(e){location=""" + url + @"""}</script>";
            HttpContext.Current.Response.Write(js);

        }



        public static void RefreshOpener()
        {

            string js = @"<Script language='JavaScript'>
                    opener.location.reload();
                  </Script>";
            HttpContext.Current.Response.Write(js);

        }


        public static void JavaScriptLocationHref(string url)
        {

            string js = @"<Script language='JavaScript'>
                    window.location.replace('{0}');
                  </Script>";
            js = string.Format(js, url);
            HttpContext.Current.Response.Write(js);

        }


        public static void ShowModalDialogWindow(string webFormUrl, int width, int height, int top, int left)
        {

            string features = "dialogWidth:" + width.ToString() + "px"
                + ";dialogHeight:" + height.ToString() + "px"
                + ";dialogLeft:" + left.ToString() + "px"
                + ";dialogTop:" + top.ToString() + "px"
                + ";center:yes;help=no;resizable:no;status:no;scroll=yes";
            ShowModalDialogWindow(webFormUrl, features);

        }


        public static void ShowModalDialogWindow(string webFormUrl, string features)
        {
            string js = ShowModalDialogJavascript(webFormUrl, features);
            HttpContext.Current.Response.Write(js);
        }

        public static string ShowModalDialogJavascript(string webFormUrl, string features)
        {

            string js = @"<script language=javascript>							
							showModalDialog('" + webFormUrl + "','','" + features + "');</script>";
            return js;

        }

        public static void OpenWebFormSize(string url, int width, int heigth, int top, int left)
        {

            string js = @"<Script language='JavaScript'>window.open('" + url + @"','','height=" + heigth + ",width=" + width + ",top=" + top + ",left=" + left + ",location=no,menubar=no,resizable=yes,scrollbars=yes,status=yes,titlebar=no,toolbar=no,directories=no');</Script>";

            HttpContext.Current.Response.Write(js);

        }


        public static void JavaScriptExitIfream(string url)
        {
            string js = @"<Script language='JavaScript'>
                    parent.window.location.replace('{0}');
                  </Script>";
            js = string.Format(js, url);
            HttpContext.Current.Response.Write(js);
        }
    }
}

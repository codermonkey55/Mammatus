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
        /// <summary>
        /// 输出自定义脚本信息
        /// </summary>
        /// <param name="page">当前页面指针，一般为this</param>
        /// <param name="script">输出脚本</param>
        public static void ResponseScript(System.Web.UI.Page page, string script)
        {
            page.ClientScript.RegisterStartupScript(page.GetType(), "message", "<script language='javascript' defer>" + script + "</script>");

        }
         回到历史页面
        /// <summary>
        /// 回到历史页面
        /// </summary>
        /// <param name="value">-1/1</param>
        public static void GoHistory(int value)
        {
            
            string js = @"<Script language='JavaScript'>
                    history.go({0});  
                  </Script>";
            HttpContext.Current.Response.Write(string.Format(js, value));
            
        }
        
         关闭当前窗口
        /// <summary>
        /// 关闭当前窗口
        /// </summary>
        public static void CloseWindow()
        {
            
            string js = @"<Script language='JavaScript'>
                    parent.opener=null;window.close();  
                  </Script>";
            HttpContext.Current.Response.Write(js);
            HttpContext.Current.Response.End();
            
        }
        
         刷新父窗口
        /// <summary>
        /// 刷新父窗口
        /// </summary>
        public static void RefreshParent(string url)
        {
            
            string js = @"<script>try{top.location=""" + url + @"""}catch(e){location=""" + url + @"""}</script>";
            HttpContext.Current.Response.Write(js);
            
        }
        
         刷新打开窗口
        /// <summary>
        /// 刷新打开窗口
        /// </summary>
        public static void RefreshOpener()
        {
            
            string js = @"<Script language='JavaScript'>
                    opener.location.reload();
                  </Script>";
            HttpContext.Current.Response.Write(js);
            
        }
        
         转向Url指定的页面
        /// <summary>
        /// 转向Url指定的页面
        /// </summary>
        /// <param name="url">连接地址</param>
        public static void JavaScriptLocationHref(string url)
        {
            
            string js = @"<Script language='JavaScript'>
                    window.location.replace('{0}');
                  </Script>";
            js = string.Format(js, url);
            HttpContext.Current.Response.Write(js);
            
        }
        
         打开指定大小位置的模式对话框
        /// <summary>
        /// 打开指定大小位置的模式对话框
        /// </summary>
        /// <param name="webFormUrl">连接地址</param>
        /// <param name="width">宽</param>
        /// <param name="height">高</param>
        /// <param name="top">距离上位置</param>
        /// <param name="left">距离左位置</param>
        public static void ShowModalDialogWindow(string webFormUrl, int width, int height, int top, int left)
        {
            
            string features = "dialogWidth:" + width.ToString() + "px"
                + ";dialogHeight:" + height.ToString() + "px"
                + ";dialogLeft:" + left.ToString() + "px"
                + ";dialogTop:" + top.ToString() + "px"
                + ";center:yes;help=no;resizable:no;status:no;scroll=yes";
            ShowModalDialogWindow(webFormUrl, features);
            
        }
        
         打开模式对话框
        /// <summary>
        /// 打开模式对话框
        /// </summary>
        /// <param name="webFormUrl">链接地址</param>
        /// <param name="features"></param>
        public static void ShowModalDialogWindow(string webFormUrl, string features)
        {
            string js = ShowModalDialogJavascript(webFormUrl, features);
            HttpContext.Current.Response.Write(js);
        }

        /// <summary>
        /// 打开模式对话框
        /// </summary>
        /// <param name="webFormUrl"></param>
        /// <param name="features"></param>
        /// <returns></returns>
        public static string ShowModalDialogJavascript(string webFormUrl, string features)
        {
            
            string js = @"<script language=javascript>							
							showModalDialog('" + webFormUrl + "','','" + features + "');</script>";
            return js;
            
        }
        
         打开指定大小的新窗体
        /// <summary>
        /// 打开指定大小的新窗体
        /// </summary>
        /// <param name="url">地址</param>
        /// <param name="width">宽</param>
        /// <param name="heigth">高</param>
        /// <param name="top">头位置</param>
        /// <param name="left">左位置</param>
        public static void OpenWebFormSize(string url, int width, int heigth, int top, int left)
        {
            
            string js = @"<Script language='JavaScript'>window.open('" + url + @"','','height=" + heigth + ",width=" + width + ",top=" + top + ",left=" + left + ",location=no,menubar=no,resizable=yes,scrollbars=yes,status=yes,titlebar=no,toolbar=no,directories=no');</Script>";

            HttpContext.Current.Response.Write(js);
            
        }
        
        /// <summary>
        /// 页面跳转（跳出框架）
        /// </summary>
        /// <param name="url"></param>
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

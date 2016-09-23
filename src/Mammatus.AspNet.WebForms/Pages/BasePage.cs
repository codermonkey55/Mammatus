using System;
using System.IO;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Mammatus.AspNet.WebForms.Pages
{
    public class BasePage : Page
    {
        public BasePage()
        {
            //TODO:
        }

        public static string Title = "";
        public static string keywords = "";
        public static string description = "";

        protected override void OnInit(EventArgs e)
        {
            if (Session["admin"] == null || Session["admin"].ToString().Trim() == "")
            {
                Response.Redirect("login.aspx");
            }
            base.OnInit(e);
        }

        protected void ExportData(string stringContent, string fileName)
        {

            fileName = fileName + DateTime.Now.Year + DateTime.Now.Month + DateTime.Now.Day + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second + DateTime.Now.Millisecond;

            Response.Clear();
            Response.Charset = "gb2312";
            Response.ContentType = "application/ms-excel";
            Response.ContentEncoding = Encoding.UTF8;
            Page.EnableViewState = false;
            Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName + ".xls");
            Response.Write("<html><head><meta http-equiv=Content-Type content=\"text/html; charset=utf-8\">");
            Response.Write(stringContent);
            Response.Write("</body></html>");
            Response.End();
        }

        public void ExportData(GridView obj)
        {
            try
            {
                string style = "";
                if (obj.Rows.Count > 0)
                {
                    style = @"<style> .text { mso-number-format:\@; } </script> ";
                }
                else
                {
                    style = "no data.";
                }

                Response.ClearContent();
                DateTime dt = DateTime.Now;
                string filename = dt.Year + dt.Month.ToString() + dt.Day + dt.Hour + dt.Minute + dt.Second;
                Response.AddHeader("content-disposition", "attachment; filename=ExportData" + filename + ".xls");
                Response.ContentType = "application/ms-excel";
                Response.Charset = "GB2312";
                Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);
                obj.RenderControl(htw);
                Response.Write(style);
                Response.Write(sw.ToString());
                Response.End();
            }
            catch
            {
                // ignored
            }
        }
    }
}

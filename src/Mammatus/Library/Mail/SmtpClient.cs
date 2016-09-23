using System;
using System.Text;
using System.IO;
using System.Net.Sockets;
using System.Collections;

namespace Mammatus.Library.Mail_Xofly
{

    /// <summary>
    /// 发送邮件
    /// </summary>
    //--------------------调用-----------------------
    //MailAttachments ma=new MailAttachments();
    //ma.Add(@"附件地址");
    //MailMessage mail = new MailMessage();
    //mail.Attachments=ma;
    //mail.Body="你好";
    //mail.AddRecipients("zjy99684268@163.com");
    //mail.From="zjy99684268@163.com";
    //mail.FromName="zjy";
    //mail.Subject="Hello";
    //SmtpClient sp = new SmtpClient();
    //sp.SmtpServer = "smtp.163.com";
    //sp.Send(mail, "zjy99684268@163.com", "123456");
    //------------------------------------------------
    public class SmtpClient
    {
        #region 构造函数
        public SmtpClient()
        { }

        public SmtpClient(string _smtpServer)
        {
            _SmtpServer = _smtpServer;
        }
        #endregion

        #region 私有字段
        private string errmsg;
        private string _SmtpServer;
        #endregion

        #region 公有属性
        /// <summary>
        /// 错误消息反馈
        /// </summary>
        public string ErrMsg
        {
            get { return errmsg; }
        }

        /// <summary>
        /// 邮件服务器
        /// </summary>
        public string SmtpServer
        {
            set { _SmtpServer = value; }
            get { return _SmtpServer; }
        }
        #endregion

        public bool Send(MailMessage mailMessage, string username, string password)
        {
            SmtpServerHelper helper = new SmtpServerHelper();
            if (helper.SendEmail(_SmtpServer, 25, username, password, mailMessage))
                return true;
            else
            {
                errmsg = helper.ErrMsg;
                return false;
            }
        }
    }

}
using System;
using System.Text;
using System.IO;
using System.Net.Sockets;
using System.Collections;

namespace Mammatus.Library.Mail_Xofly
{

    /// <summary>
    /// 邮件信息
    /// </summary>
    public class MailMessage
    {
        #region 构造函数
        public MailMessage()
        {
            _Recipients = new ArrayList();        //收件人列表
            _Attachments = new MailAttachments(); //附件
            _BodyFormat = MailFormat.HTML;        //缺省的邮件格式为HTML
            _Priority = MailPriority.Normal;
            _Charset = "GB2312";
        }
        #endregion

        #region 私有字段
        private int _MaxRecipientNum = 30;
        private string _From;      //发件人地址
        private string _FromName;  //发件人姓名
        private IList _Recipients; //收件人
        private MailAttachments _Attachments;//附件
        private string _Body;      //内容
        private string _Subject;   //主题
        private MailFormat _BodyFormat;     //邮件格式
        private string _Charset = "GB2312"; //字符编码格式
        private MailPriority _Priority;     //邮件优先级
        #endregion

        #region 公有属性
        /// <summary>
        /// 设定语言代码，默认设定为GB2312，如不需要可设置为""
        /// </summary>
        public string Charset
        {
            get { return _Charset; }
            set { _Charset = value; }
        }

        /// <summary>
        /// 最大收件人
        /// </summary>
        public int MaxRecipientNum
        {
            get { return _MaxRecipientNum; }
            set { _MaxRecipientNum = value; }
        }

        /// <summary>
        /// 发件人地址
        /// </summary>
        public string From
        {
            get { return _From; }
            set { _From = value; }
        }

        /// <summary>
        /// 发件人姓名
        /// </summary>
        public string FromName
        {
            get { return _FromName; }
            set { _FromName = value; }
        }

        /// <summary>
        /// 内容
        /// </summary>
        public string Body
        {
            get { return _Body; }
            set { _Body = value; }
        }

        /// <summary>
        /// 主题
        /// </summary>
        public string Subject
        {
            get { return _Subject; }
            set { _Subject = value; }
        }

        /// <summary>
        /// 附件
        /// </summary>
        public MailAttachments Attachments
        {
            get { return _Attachments; }
            set { _Attachments = value; }
        }

        /// <summary>
        /// 优先权
        /// </summary>
        public MailPriority Priority
        {
            get { return _Priority; }
            set { _Priority = value; }
        }

        /// <summary>
        /// 收件人
        /// </summary>
        public IList Recipients
        {
            get { return _Recipients; }
        }

        /// <summary>
        /// 邮件格式
        /// </summary>
        public MailFormat BodyFormat
        {
            set { _BodyFormat = value; }
            get { return _BodyFormat; }
        }
        #endregion

        #region 公共方法
        /// <summary>
        /// 增加一个收件人地址
        /// </summary>
        /// <param name="recipient">收件人的Email地址</param>
        public void AddRecipients(string recipient)
        {
            if (_Recipients.Count < MaxRecipientNum)
            {
                _Recipients.Add(recipient);
            }
        }

        /// <summary>
        /// 增加多个收件人地址
        /// </summary>
        /// <param name="recipient">收件人的Email地址集合</param>
        public void AddRecipients(params string[] recipient)
        {
            if (recipient == null)
            {
                throw (new ArgumentException("收件人不能为空."));
            }
            else
            {
                for (int i = 0; i < recipient.Length; i++)
                {
                    AddRecipients(recipient[i]);
                }
            }
        }
        #endregion
    }

}
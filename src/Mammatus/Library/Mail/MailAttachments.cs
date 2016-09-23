using System;
using System.Text;
using System.IO;
using System.Net.Sockets;
using System.Collections;

namespace Mammatus.Library.Mail_Xofly
{

    /// <summary>
    /// 添加附件
    /// </summary>
    public class MailAttachments
    {
        #region 构造函数
        public MailAttachments()
        {
            _Attachments = new ArrayList();
        }
        #endregion

        #region 私有字段
        private IList _Attachments;
        private const int MaxAttachmentNum = 10;
        #endregion

        #region 索引器
        public string this[int index]
        {
            get { return (string)_Attachments[index]; }
        }
        #endregion

        #region 公共方法
        /// <summary>
        /// 添加邮件附件
        /// </summary>
        /// <param name="FilePath">附件的绝对路径</param>
        public void Add(params string[] filePath)
        {
            if (filePath == null)
            {
                throw (new ArgumentNullException("非法的附件"));
            }
            else
            {
                for (int i = 0; i < filePath.Length; i++)
                {
                    Add(filePath[i]);
                }
            }
        }

        /// <summary>
        /// 添加一个附件,当指定的附件不存在时，忽略该附件，不产生异常。
        /// </summary>
        /// <param name="filePath">附件的绝对路径</param>
        public void Add(string filePath)
        {
            if (System.IO.File.Exists(filePath))
            {
                if (_Attachments.Count < MaxAttachmentNum)
                {
                    _Attachments.Add(filePath);
                }
            }
        }

        /// <summary>
        /// 清除所有附件
        /// </summary>
        public void Clear()
        {
            _Attachments.Clear();
        }

        /// <summary>
        /// 获取附件个数
        /// </summary>
        public int Count
        {
            get { return _Attachments.Count; }
        }
        #endregion
    }

}
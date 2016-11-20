using System;
using System.Collections;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace Mammatus.Library.Mail
{

    public class MailAttachments
    {
        public MailAttachments()
        {
            _attachments = new ArrayList();
        }

        private readonly IList _attachments;
        private const int MaxAttachmentNum = 10;

        public string this[int index]
        {
            get { return (string)_attachments[index]; }
        }

        public void Add(params string[] filePath)
        {
            if (filePath == null)
            {
                throw (new ArgumentNullException(""));
            }
            else
            {
                for (int i = 0; i < filePath.Length; i++)
                {
                    Add(filePath[i]);
                }
            }
        }

        public void Add(string filePath)
        {
            if (System.IO.File.Exists(filePath))
            {
                if (_attachments.Count < MaxAttachmentNum)
                {
                    _attachments.Add(filePath);
                }
            }
        }

        public void Clear()
        {
            _attachments.Clear();
        }

        public int Count
        {
            get { return _attachments.Count; }
        }
    }

}
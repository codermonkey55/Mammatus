using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace Mammatus.Documents.iTextSharp.Pdf
{
    //-------------------------------------------------------------------------------
    //PDFOperation pdf = new PDFOperation();
    //pdf.Open(new FileStream(path, FileMode.Create));
    //pdf.SetBaseFont(@"C:\Windows\Fonts\SIMHEI.TTF");
    //pdf.AddParagraph("（：" + DateTime.Now + "）", 15, 1, 20, 0, 0);
    //pdf.Close();
    //-------------------------------------------------------------------------------------
    public class PdfOperation
    {
        public PdfOperation()
        {
            _rect = PageSize.A4;
            _document = new Document(_rect);
        }

        public PdfOperation(string type)
        {
            SetPageSize(type);
            _document = new Document(_rect);
        }

        public PdfOperation(string type, float marginLeft, float marginRight, float marginTop, float marginBottom)
        {
            SetPageSize(type);
            _document = new Document(_rect, marginLeft, marginRight, marginTop, marginBottom);
        }

        private Font _font;
        private Rectangle _rect;
        private readonly Document _document;
        private BaseFont _basefont;

        public void SetBaseFont(string path)
        {
            _basefont = BaseFont.CreateFont(path, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
        }

        public void SetFont(float size)
        {
            _font = new Font(_basefont, size);
        }

        public void SetPageSize(string type)
        {
            switch (type.Trim())
            {
                case "A4":
                    _rect = PageSize.A4;
                    break;
                case "A8":
                    _rect = PageSize.A8;
                    break;
            }
        }

        public void GetInstance(Stream os)
        {
            PdfWriter.GetInstance(_document, os);
        }

        public void Open(Stream os)
        {
            GetInstance(os);
            _document.Open();
        }

        public void Close()
        {
            _document.Close();
        }

        public void AddParagraph(string content, float fontsize)
        {
            SetFont(fontsize);
            Paragraph pra = new Paragraph(content, _font);
            _document.Add(pra);
        }

        public void AddParagraph(string content, float fontsize, int alignment, float spacingAfter, float spacingBefore, float multipliedLeading)
        {
            SetFont(fontsize);
            Paragraph pra = new Paragraph(content, _font);
            pra.Alignment = alignment;
            if (spacingAfter != 0)
            {
                pra.SpacingAfter = spacingAfter;
            }
            if (spacingBefore != 0)
            {
                pra.SpacingBefore = spacingBefore;
            }
            if (multipliedLeading != 0)
            {
                pra.MultipliedLeading = multipliedLeading;
            }
            _document.Add(pra);
        }

        public void AddImage(string path, int alignment, float newWidth, float newHeight)
        {
            Image img = Image.GetInstance(path);
            img.Alignment = alignment;
            if (newWidth != 0)
            {
                img.ScaleAbsolute(newWidth, newHeight);
            }
            else
            {
                if (img.Width > PageSize.A4.Width)
                {
                    img.ScaleAbsolute(_rect.Width, img.Width * img.Height / _rect.Height);
                }
            }
            _document.Add(img);
        }

        public void AddAnchorReference(string content, float fontSize, string reference)
        {
            SetFont(fontSize);
            Anchor auc = new Anchor(content, _font);
            auc.Reference = reference;
            _document.Add(auc);
        }

        public void AddAnchorName(string content, float fontSize, string name)
        {
            SetFont(fontSize);
            Anchor auc = new Anchor(content, _font);
            auc.Name = name;
            _document.Add(auc);
        }

    }
}
using Mammatus.Helpers;
using Mammatus.Library.Image.Enums;
using System;
using System.IO;
using System.Windows;

namespace Mammatus.Library.Image
{
    public class ImageInfo
    {
        public Size GraphicSize { get; private set; }

        public long FileSize { get; private set; }

        public ImageFileType Type { get; private set; }

        public int BlankHeaderLength { get; private set; }

        public bool IsMetaImage => (Type == ImageFileType.Wmf || Type == ImageFileType.Emf);

        public ImageInfo(string path)
        {
            var size = default(Size);

            using (var stream = File.OpenRead(path))
            {
                if (CheckSizeMain(stream, out size))
                {
                    GraphicSize = size;
                }
            }
        }

        public ImageInfo(Stream stream)
        {
            var size = default(Size);

            if (CheckSizeMain(stream, out size))
            {
                GraphicSize = size;
            }
        }

        private bool CheckSizeMain(Stream stream, out Size size)
        {

            FileSize = stream.Length;

            if (FileSize < 16)
            {
                // too small
                size = default(Size);
                return false;
            }

            var type = BinaryHelper.ReadInt16(stream, true);
            BlankHeaderLength = 0;

            while (type == 0x0000
                && BlankHeaderLength < 8)
            {
                type = BinaryHelper.ReadInt16(stream, true);
                BlankHeaderLength += 2;
            }

            switch (type)
            {

                case 0x424D:
                    //BMP: 0x42,0x4D_"BM"
                    return GetBmpSize(stream, out size);
                case 0xFFD8:
                    //JPG: 0xFF,0xD8
                    return GetJpegSize(stream, out size);
                case 0x4749:
                    //GIF: 0x47,0x49_"GI"
                    return GetGifSize(stream, out size);
                case 0x8950:
                    //PNG: 0x89,0x50_'P'
                    return GetPngSize(stream, out size);
                case 0x3842:
                    //PSD: 0x38,0x42_"8B"
                    return GetPsdSize(stream, out size);
                case 0xD7CD:
                    //WMF: 0xD7,0xCD
                    return GetWmfSize(stream, out size);
                case 0x0100:
                    //EMF: 0x01,0x00
                    return GetEmfSize(stream, out size);
                case 0x4949:
                    //TIFF-I:0x49,0x49
                    return GetTiffSize(stream, false, out size);
                case 0x4D4D:
                    //TIFF-M:0x4D,0x4D
                    return GetTiffSize(stream, true, out size);
            }
            size = default(Size);
            return false;
        }

        /// <summary>
        /// BMP
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        private bool GetBmpSize(Stream stream, out Size size)
        {

            var fileSize = BinaryHelper.ReadInt32(stream, false);//0x02
            SetStreamPosition(stream, 0x0E);
            var infoSize = BinaryHelper.ReadInt32(stream, false);//0x0E

            if (infoSize < 12)
            {
                //Invalid Header
                size = default(Size);
                return false;
            }

            if (infoSize == 12)
            {
                // OS2 Ver 1.x
                var width = BinaryHelper.ReadInt16(stream, false);
                var height = Math.Abs(BinaryHelper.ReadInt16(stream, false));
                size = new Size(width, height);
            }
            else
            {
                // OS2 Ver 2.x,  Windows V3, V4, V5
                var width = BinaryHelper.ReadInt32(stream, false);
                var height = Math.Abs(BinaryHelper.ReadInt32(stream, false));
                size = new Size(width, height);
            }
            Type = ImageFileType.Bmp;
            return true;

        }

        /// <summary>
        /// JPEG
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        private bool GetJpegSize(Stream stream, out Size size)
        {
            while (stream.Position < stream.Length)
            {

                var marker = BinaryHelper.ReadInt16(stream, true);
                if (marker >= 0xFFC0 && marker <= 0xFFCF
                    && marker != 0xFFC4 && marker != 0xFFC8
                    && marker != 0xFFCC)
                {
                    break;
                }

                if (marker == 0xFFD9)// End Of Image
                {
                    size = default(Size);
                    return false;
                }


                var segmentLength = BinaryHelper.ReadInt16(stream, true);

                var pos = GetStreamPosition(stream) + segmentLength - 2;

                SetStreamPosition(stream, pos);
                if (pos + 4 >= stream.Length)
                {
                    size = default(Size);
                    return false;
                }
            }

            SetStreamPosition(stream, GetStreamPosition(stream) + 3);
            var height = BinaryHelper.ReadInt16(stream, true);
            var width = BinaryHelper.ReadInt16(stream, true);

            size = new Size(width, height);

            Type = ImageFileType.Jpeg;
            return true;

        }

        /// <summary>
        /// GIF
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        private bool GetGifSize(Stream stream, out Size size)
        {
            var type = new byte[4];
            stream.Read(type, 0, type.Length);//0x02
            if (type[0] != 'F' || type[1] != '8'
                || (type[2] != '7' && type[2] != '9') || type[3] != 'a')
            {
                //This is not GIF
                size = default(Size);
                return false;
            }

            var width = BinaryHelper.ReadInt16(stream, false);
            var height = BinaryHelper.ReadInt16(stream, false);
            size = new Size(width, height);

            Type = ImageFileType.Gif;
            return true;
        }


        /// <summary>
        /// PNG
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        private bool GetPngSize(Stream stream, out Size size)
        {
            var type = new byte[6];
            stream.Read(type, 0, type.Length);//0x02
            if (type[0] != 'N' || type[1] != 'G'
                || type[2] != (byte)0x0D || type[3] != (byte)0x0A
                || type[4] != (byte)0x1A || type[5] != (byte)0x0A)
            {
                //This is not PNG
                size = default(Size);
                return false;
            }

            SetStreamPosition(stream, 0x0C);

            var ihdr = new byte[4];
            stream.Read(ihdr, 0, ihdr.Length);//0x0C
            if (ihdr[0] != 'I' || ihdr[1] != 'H'
                || ihdr[2] != 'D' || ihdr[3] != 'R')
            {
                //This is not PNG
                size = default(Size);
                return false;
            }

            var width = BinaryHelper.ReadInt32(stream, true);
            var height = BinaryHelper.ReadInt32(stream, true);
            size = new Size(width, height);

            Type = ImageFileType.Png;
            return true;

        }

        /// <summary>
        /// PSD
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        private bool GetPsdSize(Stream stream, out Size size)
        {
            var type = new byte[4];
            stream.Read(type, 0, type.Length);//0x02
            if (type[0] != 'P' || type[1] != 'S'
                || type[2] != (byte)0x00 || type[3] != (byte)0x01)
            {
                //This is not PSD
                size = default(Size);
                return false;
            }

            SetStreamPosition(stream, 0x0E);

            var height = BinaryHelper.ReadInt32(stream, true);
            var width = BinaryHelper.ReadInt32(stream, true);

            size = new Size(width, height);

            Type = ImageFileType.Psd;
            return true;
        }

        /// <summary>
        /// WMF
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        private bool GetWmfSize(Stream stream, out Size size)
        {
            var type = new byte[2];
            stream.Read(type, 0, type.Length);//0x02
            if (type[0] != (byte)0xC6 || type[1] != (byte)0x9A)
            {
                //This is not WMF
                size = default(Size);
                return false;
            }

            SetStreamPosition(stream, 0x0A);

            var left = BinaryHelper.ReadInt16(stream, false);
            var top = BinaryHelper.ReadInt16(stream, false);
            var right = BinaryHelper.ReadInt16(stream, false);
            var bottom = BinaryHelper.ReadInt16(stream, false);

            var width = Math.Abs(right - left);
            var height = Math.Abs(bottom - top);


            size = new Size(width, height);

            Type = ImageFileType.Wmf;
            return true;
        }

        /// <summary>
        /// EMF
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        private bool GetEmfSize(Stream stream, out Size size)
        {
            var type = new byte[2];
            stream.Read(type, 0, type.Length);//0x02
            if (type[0] != (byte)0x00 || type[1] != (byte)0x00)
            {
                //This is not EMF
                size = default(Size);
                return false;
            }

            SetStreamPosition(stream, 0x08);

            var left = BinaryHelper.ReadInt32(stream, false);
            var top = BinaryHelper.ReadInt32(stream, false);
            var right = BinaryHelper.ReadInt32(stream, false);
            var bottom = BinaryHelper.ReadInt32(stream, false);

            var width = Math.Abs(right - left) + 1;
            var height = Math.Abs(bottom - top) + 1;


            SetStreamPosition(stream, 0x28);

            var sign = new byte[4];
            stream.Read(sign, 0, sign.Length);//0x0C
            if (sign[0] != (byte)0x20 || sign[1] != 'E'
                || sign[2] != 'M' || sign[3] != 'F')
            {
                //This is not EMF
                size = default(Size);
                return false;
            }

            size = new Size(width, height);

            Type = ImageFileType.Emf;
            return true;
        }

        /// <summary>
        /// TIFF
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="isBigEndian"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        private bool GetTiffSize(Stream stream, bool isBigEndian, out Size size)
        {
            var version = BinaryHelper.ReadInt16(stream, isBigEndian);
            if (version != 0x2A)
            {
                size = default(Size);
                return false;
            }

            var offset = BinaryHelper.ReadInt32(stream, isBigEndian);

            SetStreamPosition(stream, offset);

            var entryCount = BinaryHelper.ReadInt16(stream, isBigEndian);

            int? width = null;
            int? height = null;

            var position = GetStreamPosition(stream);

            for (var i = 0; i < entryCount; i++)
            {

                var tagId = BinaryHelper.ReadInt16(stream, isBigEndian);

                if (tagId == 0x100 || tagId == 0x101)
                {

                    var dataType = BinaryHelper.ReadInt16(stream, isBigEndian);
                    var count = BinaryHelper.ReadInt32(stream, isBigEndian);

                    int num;

                    switch (dataType)
                    {
                        case 3:
                            //2byte
                            num = BinaryHelper.ReadInt16(stream, isBigEndian);
                            break;
                        case 4:
                            //4byte
                            num = BinaryHelper.ReadInt32(stream, isBigEndian);
                            break;
                        default:
                            continue;
                    }

                    if (tagId == 0x100)
                    {
                        width = num;
                    }
                    else
                    {
                        height = num;
                    }

                }

                if (width.HasValue && height.HasValue)
                {
                    size = new Size(width.Value, height.Value);
                    Type = ImageFileType.Tiff;
                    return true;
                }

                SetStreamPosition(stream, position + (i + 1) * 12);
            }

            size = default(Size);
            return false;

        }

        private void SetStreamPosition(Stream stream, long position)
        {
            stream.Position = position + BlankHeaderLength;
        }

        private long GetStreamPosition(Stream stream)
        {
            return stream.Position - BlankHeaderLength;
        }
    }
}

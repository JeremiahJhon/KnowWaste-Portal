using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Web;

namespace Web.Framework.Models
{
    public class ModelCommon
    {
        //Document Path
        public static string DocumentPath { get; set; }

        //For supporting images
        public static string ImagePath { get; set;}

        public static void CreateImage(string dFilename, int lnWidth, int lnHeight, string dName)
        {
            Bitmap bmpOut = null;

            try
            {
                Bitmap loBMP = new Bitmap(dFilename);
                double lnRatio = 0;

                int lnNewWidth = 0;
                int lnNewHeight = 0;

                ImageCodecInfo jgpEncoder = GetEncoder(ImageFormat.Jpeg);
                System.Drawing.Imaging.Encoder myEncoder = System.Drawing.Imaging.Encoder.Quality;
                EncoderParameters myEncoderParameters = new EncoderParameters(1);

                EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, 50L);
                myEncoderParameters.Param[0] = myEncoderParameter;

                if ((loBMP.Width < lnWidth) & (loBMP.Height < lnHeight))
                {
                    loBMP.SetResolution(72, 72);
                    loBMP.Save(dName, jgpEncoder, myEncoderParameters);
                    return;
                }

                if (loBMP.Width > loBMP.Height)
                {
                    lnRatio = lnWidth / (double)loBMP.Width;
                    lnNewWidth = lnWidth;

                    int lnTemp = (int)(loBMP.Height * lnRatio);
                    lnNewHeight = lnTemp;
                }
                else
                {
                    lnRatio = lnHeight / (double)loBMP.Height;
                    lnNewHeight = lnHeight;

                    int lnTemp = (int)(loBMP.Width * lnRatio);
                    lnNewWidth = lnTemp;
                }

                bmpOut = new Bitmap(loBMP, lnNewWidth, lnNewHeight);
                bmpOut.SetResolution(72, 72);
                bmpOut.Save(dName, jgpEncoder, myEncoderParameters);
                loBMP.Dispose();
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }

        private static ImageCodecInfo GetEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();

            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }

        public static void CreateThumbnail(string dFilename, int lnWidth, int lnHeight, string dName)
        {
            Bitmap bmpOut = null;

            try
            {
                Bitmap loBMP = new Bitmap(dFilename);
                ImageFormat loFormat = loBMP.RawFormat;
                double lnRatio = 0;

                int lnNewWidth = 0;
                int lnNewHeight = 0;

                ImageCodecInfo jgpEncoder = GetEncoder(ImageFormat.Jpeg);
                System.Drawing.Imaging.Encoder myEncoder = System.Drawing.Imaging.Encoder.Quality;
                EncoderParameters myEncoderParameters = new EncoderParameters(1);

                EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, 75L);
                myEncoderParameters.Param[0] = myEncoderParameter;

                if ((loBMP.Width < lnWidth) & (loBMP.Height < lnHeight))
                {
                    loBMP.SetResolution(72, 72);
                    loBMP.Save(dName, jgpEncoder, myEncoderParameters);
                    return;
                }

                if (loBMP.Width < loBMP.Height)
                {
                    lnRatio = lnWidth / (double)loBMP.Width;
                    lnNewWidth = lnWidth;
                    int lnTemp = (int)(loBMP.Height * lnRatio);
                    lnNewHeight = lnTemp;
                }
                else
                {
                    lnRatio = lnHeight / (double)loBMP.Height;
                    lnNewHeight = lnHeight;
                    int lnTemp = (int)(loBMP.Width * lnRatio);
                    lnNewWidth = lnTemp;
                }

                bmpOut = new Bitmap(loBMP, lnNewWidth, lnNewHeight);
                if (loBMP.Width > loBMP.Height)
                {
                    bmpOut = bmpOut.Clone(new Rectangle((bmpOut.Width - lnWidth) / 2, 0, lnWidth, lnHeight), bmpOut.PixelFormat);
                }
                else
                {
                    bmpOut = bmpOut.Clone(new Rectangle(0, 0, lnWidth, lnHeight), bmpOut.PixelFormat);
                }

                bmpOut.SetResolution(72, 72);
                bmpOut.Save(dName, jgpEncoder, myEncoderParameters);
                loBMP.Dispose();
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }
    }
}
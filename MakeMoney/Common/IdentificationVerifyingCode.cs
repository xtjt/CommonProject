using System;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using iTextSharp.text;
using iTextSharp.text.pdf;
using MakeMoney.Enum;
using Svg;

namespace MakeMoney.Common
{
    /// <summary>
    /// 识别验证码
    /// </summary>
    public static class IdentificationVerifyingCode
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="svg"></param>
        /// <param name="typeid">参考：“http://www.ruokuai.com/home/pricetype”</param>
        /// <param name="imageType"></param>
        /// <returns></returns>
        public static string GetVerifyingCodeBySvg(string svg, string typeid,
            ImageTypeEnum imageType = ImageTypeEnum.Png)
        {
            if (string.IsNullOrWhiteSpace(svg) || string.IsNullOrWhiteSpace(typeid))
            {
                return string.Empty;
            }

            MemoryStream originStream = new MemoryStream(Encoding.UTF8.GetBytes(svg));
            MemoryStream newStream = new MemoryStream();

            Svg.SvgDocument tSvgObj = SvgDocument.Open<SvgDocument>(originStream);

            var type = string.Empty;

            switch (imageType)
            {
                case ImageTypeEnum.Png:
                    type = "image/png";
                    tSvgObj.Draw().Save(newStream, ImageFormat.Png);
                    break;
                case ImageTypeEnum.Jpeg:
                    type = "image/jpeg";
                    tSvgObj.Draw().Save(newStream, ImageFormat.Jpeg);
                    break;
                case ImageTypeEnum.Pdf:
                    type = "application/pdf";
                    PdfWriter tWriter = null;
                    Document tDocumentPdf = null;
                    try
                    {
                        tSvgObj.Draw().Save(newStream, ImageFormat.Png);
                        tDocumentPdf = new Document(new Rectangle((float) tSvgObj.Width, (float) tSvgObj.Height));
                        tDocumentPdf.SetMargins(0.0f, 0.0f, 0.0f, 0.0f);
                        iTextSharp.text.Image tGraph = iTextSharp.text.Image.GetInstance(newStream.ToArray());
                        tGraph.ScaleToFit((float) tSvgObj.Width, (float) tSvgObj.Height);

                        newStream = new MemoryStream();
                        tWriter = PdfWriter.GetInstance(tDocumentPdf, newStream);
                        tDocumentPdf.Open();
                        tDocumentPdf.NewPage();
                        tDocumentPdf.Add(tGraph);
                        tDocumentPdf.CloseDocument();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        tDocumentPdf?.Dispose();
                        tDocumentPdf?.Close();
                        tWriter?.Dispose();
                        tWriter?.Close();
                        originStream.Dispose();
                        originStream.Close();
                    }

                    break;
                case ImageTypeEnum.Svg:
                    type = "image/svg+xml";
                    newStream = originStream;
                    break;
            }

            return RuoKuaiVerifyingCode(newStream, typeid, type.Split('/')[1]);
        }

        private static string RuoKuaiVerifyingCode(Stream stream, string typeid, string imageType = "png")
        {
            if (stream == null || string.IsNullOrWhiteSpace(typeid))
            {
                return string.Empty;
            }

            var guid = Guid.NewGuid().ToString("N");

            var filePath = System.AppDomain.CurrentDomain.BaseDirectory + $@"/ImageFile/";
            var fileName = filePath + $@"{guid}.{imageType}";

            if (!Directory.Exists(filePath))
                Directory.CreateDirectory(filePath);

            try
            {
                System.Drawing.Image image = System.Drawing.Image.FromStream(stream, true, true);
                image.Save(fileName);

                byte[] data = File.ReadAllBytes(fileName);


                //todo :
                var zx = Console.ReadLine();
                return zx;










                return 若快.ParseVerificationCode(data, typeid);
            }
            catch (Exception ex)
            {
                Console.WriteLine("异常：RuoKuaiVerifyingCode：" + ex.Message);
            }
            finally
            {
                if (File.Exists(fileName))
                {
                    File.Delete(fileName);
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="typeid">参考：“http://www.ruokuai.com/home/pricetype”</param>
        /// <param name="imageType"></param>
        /// <returns></returns>
        public static string GetVerifyingCodeByStream(Stream stream, string typeid, string imageType = "png")
        {
            if (stream == null || string.IsNullOrWhiteSpace(typeid))
            {
                return string.Empty;
            }

            return RuoKuaiVerifyingCode(stream, typeid, imageType);
        }
    }
}
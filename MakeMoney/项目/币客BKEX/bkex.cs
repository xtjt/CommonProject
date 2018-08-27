using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MakeMoney.Common;
using MakeMoney.Enum;
using Newtonsoft.Json;
using RestSharp;

namespace MakeMoney.项目.币客BKEX
{
    public static class bkex
    {
        public static string CaptureImageVerificationCode()
        {
            for (var i = 0; i <= 30; i++)
            {
                //获取验证码
                var stream = GetCaptcha();
                if (stream != null)
                {
                    //识别验证码
                    var verifyingCode = IdentificationVerifyingCode.GetVerifyingCodeByStream(stream, "3040", "jpeg");
                    stream.Close();

                    return verifyingCode;
                }
            }

            return string.Empty;
        }

        public static string MobileSendMessage(string mobile)
        {
            if (string.IsNullOrWhiteSpace(mobile))
            {
                return string.Empty;
            }

            for (var i = 0; i <= 20; i++)
            {
                var message = SendMessage(mobile);

                if (message == "OK")
                    return "OK";
            }

            return string.Empty;
        }

        private static string SendMessage(string mobile)
        {
            try
            {
                var client = new RestClient("https://www.bkex.com/api/send_verify_register");
                var request = new RestRequest(Method.POST);
                request.AddHeader("timeout", "20000");
                request.AddHeader("cookie",
                    "JSESSIONID=4260879C71C7277A6A4E041B776FCDF5; SERVERID=c1ba0190021e5f5c28890a5e7b37a0df|1528686541|1528686541; __cdnuid=353a40e584d83e8dcfd39460d1d61d4d");
                request.AddHeader("content-type",
                    "multipart/form-data; boundary=----WebKitFormBoundary7MA4YWxkTrZu0gW");
                request.AddParameter("multipart/form-data; boundary=----WebKitFormBoundary7MA4YWxkTrZu0gW",
                    "------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name=\"username\"\r\n\r\n" +
                    mobile +
                    "\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name=\"telCountryCode\"\r\n\r\n86\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW--",
                    ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);

                if (response != null && !string.IsNullOrWhiteSpace(response.Content))
                {

                    try
                    {
                        var result = JsonConvert.DeserializeObject<CommonModel>(response.Content);
                        if (result.msg == "success")
                        {
                            //SERVERID = c1ba0190021e5f5c28890a5e7b37a0df | 1528620148 | 1528620147
                            //GlobalClass.ResponseCookie =
                            //    string.Join(";", response.Cookies.Select(m => m.Name + "=" + m.Value));
                            //var JSESSIONID = responseCookieSplit.Where(m => m.Split('=')[0].Trim().ToUpper() == "JSESSIONID")
                            //    .Select(m => m.Split(';')[0]);

                            //var SERVERID = responseCookieSplit.Where(m => m.Split('=')[0].Trim().ToUpper() == "SERVERID")
                            //    .Select(m => m.Split(';')[0]);

                            //var __cdnuid = responseCookieSplit.Where(m => m.Split('=')[0].Trim().ToUpper() == "__cdnuid")
                            //    .Select(m => m.Split(';')[0]);

                            //var cookie = GlobalClass.ResponseCookie.Split(';');

                            ////GlobalClass.ResponseCookie= GlobalClass.ResponseCookie

                            //var SERVERID = response.Cookies.Where(m => m.Name == "SERVERID")
                            //    .Select(m => m.Name + "=" + m.Value).FirstOrDefault();

                            //GlobalClass.ResponseCookie = cookie[0] + "; " + cookie[2] + "; " + SERVERID;

                            return "OK";
                        }
                    }
                    catch (Exception ex)
                    {
                        return string.Empty;
                    }
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                Console.WriteLine("SendMessage：异常：" + ex.Message);
                return string.Empty;
            }
        }

        private static Stream GetCaptcha()
        {
            try
            {
                //string random = "";
                //for (var i = 0; i < 13; i++)
                //{
                //    random += new Random(Guid.NewGuid().GetHashCode()).Next(0, 10);
                //}

                //var client = new RestClient($@"https://www.bkex.com/api/captcha?t={random}");
                //var request = new RestRequest(Method.GET);
                //request.AddHeader("timeout", "20000");
                //request.AddHeader("cookie", "JSESSIONID=4260879C71C7277A6A4E041B776FCDF5; SERVERID=c1ba0190021e5f5c28890a5e7b37a0df|1528686541|1528686541; __cdnuid=353a40e584d83e8dcfd39460d1d61d4d");
                //IRestResponse response = client.Execute(request);

                //if (response != null && response.RawBytes.Length>0)
                //{
                //    Stream stream = new MemoryStream(response.RawBytes);

                //    return stream;
                //}

                //return null;


                string random = "";

                for (var i = 0; i < 13; i++)
                {
                    random += new Random(Guid.NewGuid().GetHashCode()).Next(0, 10);
                }

                var url = $@"https://www.bkex.com/api/captcha?t={random}";

                //JSESSIONID = 4260879C71C7277A6A4E041B776FCDF5; SERVERID = c1ba0190021e5f5c28890a5e7b37a0df | 1528686541 | 1528686541; __cdnuid = 353a40e584d83e8dcfd39460d1d61d4d

                var zx1 = new Cookie("__cdnuid", "353a40e584d83e8dcfd39460d1d61d4d", "/", ".bkex.com");
                var zx2 = new Cookie("JSESSIONID", "4260879C71C7277A6A4E041B776FCDF5", "/", ".bkex.com");
                var zx3 = new Cookie("SERVERID", "c1ba0190021e5f5c28890a5e7b37a0df | 1528686541 | 1528686541", "/",
                    ".bkex.com");
                var cookie = new CookieContainer();
                cookie.Add(zx1);
                cookie.Add(zx2);
                cookie.Add(zx3);

                HttpWebRequest request = (HttpWebRequest) WebRequest.Create(url);

                request.Method = "GET";
                //request.ContentType = "text/html;charset=UTF-8";
                //request.CookieContainer = cookie;
                request.Timeout = 20000;

                //request.Referer = "https://www.bkex.com/";

                HttpWebResponse response = (HttpWebResponse) request.GetResponse();
                Stream myResponseStream = response.GetResponseStream();


                return myResponseStream;

            }
            catch (Exception ex)
            {
                Console.WriteLine("GetCaptcha：异常：" + ex.Message);
                return null;
            }
        }

        public static string MultipleRegister(string mobile, string verificationCode, string imageVerificationCode)
        {
            if (string.IsNullOrWhiteSpace(mobile) || string.IsNullOrWhiteSpace(verificationCode) ||
                string.IsNullOrWhiteSpace(imageVerificationCode))
            {
                return string.Empty;
            }

            for (var i = 0; i <= 5; i++)
            {
                var message = Register(mobile, verificationCode, imageVerificationCode);

                if (message == "OK")
                    return "OK";
            }

            return string.Empty;
        }

        public static string Register(string mobile, string verificationCode, string imageVerificationCode)
        {
            try
            {

                //密码应该为8至20位非纯数字字符
                var guid = Guid.NewGuid().ToString("N");
                var password = guid.Substring(0, new Random(Guid.NewGuid().GetHashCode()).Next(8, 19));

                var client = new RestClient("https://www.bkex.com/api/users/register");
                var request = new RestRequest(Method.POST);
                request.AddHeader("timeout", "20000");
                request.AddHeader("cookie",
                    "JSESSIONID=4260879C71C7277A6A4E041B776FCDF5; SERVERID=c1ba0190021e5f5c28890a5e7b37a0df|1528686541|1528686541; __cdnuid=353a40e584d83e8dcfd39460d1d61d4d");
                request.AddHeader("content-type",
                    "multipart/form-data; boundary=----WebKitFormBoundary7MA4YWxkTrZu0gW");
                request.AddParameter("multipart/form-data; boundary=----WebKitFormBoundary7MA4YWxkTrZu0gW",
                    "------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name=\"username\"\r\n\r\n" +
                    mobile +
                    "\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name=\"password\"\r\n\r\n" +
                    password +
                    "\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name=\"verifyCode\"\r\n\r\n" +
                    verificationCode +
                    "\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name=\"captcha\"\r\n\r\n" +
                    imageVerificationCode +
                    "\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name=\"telCountryCode\"\r\n\r\n86\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name=\"inviteCode\"\r\n\r\nr5QuO103\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW--",
                    ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);

                if (response != null && !string.IsNullOrWhiteSpace(response.Content))
                {
                    try
                    {
                        if (response.Content == "ok")
                        {
                            GlobalClass.ValidCount++;

                            var sql =
                                $@"insert AccountInfo(AIMobile,AIPwd,AIProjectId,AIDescribe,AIWebSite) values('{mobile}','{password}','{ItemEnum.bkex.GetHashCode()}','{CommonHelper.GetEnumDescription(ItemEnum.bkex)}','{"bkex.com"}')";
                            var count = new DatabaseHelper().AddOrUpdate(sql);
                            if (count > 0)
                            {
                                return "OK";
                            }

                            Console.WriteLine("异常：Register：数据新增失败");

                            return string.Empty;
                        }
                    }
                    catch (Exception ex)
                    {
                        return string.Empty;
                    }
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Register：异常：" + ex.Message);
                return string.Empty;
            }
        }
    }

    //public class CaptchaModel
    //{
    //    public string error { get; set; }
    //    public ResultModel result { get; set; }
    //}

    //public class ResultModel
    //{
    //    public string captcha { get; set; }
    //}

    public class CommonModel
    {
        public string data { get; set; }
        public string msg { get; set; }
        public string code { get; set; }
    }
}
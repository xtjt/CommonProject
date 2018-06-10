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
                //var url =
                //    $@"https://www.bkex.com/api/send_verify_register";

                //var zx1 = new Cookie("__cdnuid", "9f01158114da4258dc3b351ce44743d8", "/", ".bkex.com");
                //var zx2 = new Cookie("JSESSIONID", "92186E6AB082F939923C4EDD821C5DBC", "/", ".bkex.com");
                ////var zx3 = new Cookie("SERVERID", "c1ba0190021e5f5c28890a5e7b37a0df|1528620148|1528620147", "/", ".bkex.com");
                ////SERVERID = c1ba0190021e5f5c28890a5e7b37a0df | 1528620148 | 1528620147
                //var cookie = new CookieContainer();
                //cookie.Add(zx1);
                //cookie.Add(zx2);
                ////cookie.Add(zx3);



                ////var client = new RestClient("https://www.bkex.com/api/send_verify_register");
                ////var request = new RestRequest(Method.POST);
                ////request.AddHeader("postman-token", "fc7dcdf7-15a0-3306-bdda-4f115c6d675d");
                ////request.AddHeader("referer", "https://www.bkex.com/");
                ////request.AddHeader("cookie",
                ////    "__cdnuid=9f01158114da4258dc3b351ce44743d8; JSESSIONID=92186E6AB082F939923C4EDD821C5DBC");
                ////request.AddHeader("cache-control", "no-cache");
                ////request.AddHeader("content-type", "multipart/form-data; boundary=----WebKitFormBoundary7MA4YWxkTrZu0gW");
                ////request.AddParameter("multipart/form-data; boundary=----WebKitFormBoundary7MA4YWxkTrZu0gW",
                ////    "------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name=\"username\"\r\n\r\n" +
                ////    mobile +
                ////    "\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name=\"telCountryCode\"\r\n\r\n86\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW--",
                ////    ParameterType.RequestBody);
                ////IRestResponse response = client.Execute(request);

                //HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

                //request.Method = "GET";
                //request.ContentType = "text/html;charset=UTF-8";
                //request.CookieContainer = cookie;
                //request.Timeout = 10000;

                //request.Referer = "https://www.bkex.com/";

                //HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                //Stream myResponseStream = response.GetResponseStream();

                ////SERVERID = c1ba0190021e5f5c28890a5e7b37a0df | 1528617470 | 1528617470; Path =/
                //string responseCookie = response.Headers.Get("Set-Cookie");

                //if (!string.IsNullOrWhiteSpace(responseCookie))
                //{
                //    GlobalClass.ResponseCookie = responseCookie.Split(';')[0];
                //}


                ////StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
                ////string retString = myStreamReader.ReadToEnd();
                ////myStreamReader.Close();
                ////myResponseStream.Close();

                //return myResponseStream;


                var client = new RestClient("https://www.bkex.com/api/send_verify_register");
                var request = new RestRequest(Method.POST);
                request.AddHeader("postman-token", "fc7dcdf7-15a0-3306-bdda-4f115c6d675d");
                request.AddHeader("timeout", "10000");
                request.AddHeader("referer", "https://www.bkex.com/");
                request.AddHeader("cookie",
                    "__cdnuid=9f01158114da4258dc3b351ce44743d8; JSESSIONID=92186E6AB082F939923C4EDD821C5DBC" +
                    $@"; {GlobalClass.ResponseCookie}");
                request.AddHeader("cache-control", "no-cache");
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
                            GlobalClass.ResponseCookie = response.Cookies.Where(m => m.Name == "SERVERID")
                                .Select(m => m.Name + "=" + m.Value).FirstOrDefault().ToString();


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

                //string phone = "";

                //for (var i = 0; i < 13; i++)
                //{
                //    phone += new Random(Guid.NewGuid().GetHashCode()).Next(0, 10);
                //}

                //var client = new RestClient($@"https://www.bkex.com/api/captcha?t={phone}");
                //var request = new RestRequest(Method.GET);
                //request.AddHeader("postman-token", "987e043a-aa4e-02b1-84d9-0ae34f50463c");
                //request.AddHeader("referer", "https://www.bkex.com/");
                //request.AddHeader("cookie", "__cdnuid=9f01158114da4258dc3b351ce44743d8; JSESSIONID=92186E6AB082F939923C4EDD821C5DBC");
                //request.AddHeader("cache-control", "no-cache");
                //IRestResponse response = client.Execute(request);


                //byte[] imageBytes = Convert.FromBase64String(response.Content);
                ////读入MemoryStream对象  
                //MemoryStream memoryStream = new MemoryStream(imageBytes, 0, imageBytes.Length);
                //memoryStream.Write(imageBytes, 0, imageBytes.Length);

                ////IdentificationVerifyingCode.GetVerifyingCodeByStream(memoryStream, "3040", "jpeg");


                //return memoryStream;


                string phone = "";

                for (var i = 0; i < 13; i++)
                {
                    phone += new Random(Guid.NewGuid().GetHashCode()).Next(0, 10);
                }

                var url =
                    $@"https://www.bkex.com/api/captcha?t={phone}";

                var zx1 = new Cookie("__cdnuid", "9f01158114da4258dc3b351ce44743d8", "/", ".bkex.com");
                var zx2 = new Cookie("JSESSIONID", "92186E6AB082F939923C4EDD821C5DBC", "/", ".bkex.com");
                //var zx3 = new Cookie("SERVERID", "c1ba0190021e5f5c28890a5e7b37a0df|1528620148|1528620147", "/", ".bkex.com");
                //SERVERID = c1ba0190021e5f5c28890a5e7b37a0df | 1528620148 | 1528620147
                var cookie = new CookieContainer();
                cookie.Add(zx1);
                cookie.Add(zx2);
                //cookie.Add(zx3);

                HttpWebRequest request = (HttpWebRequest) WebRequest.Create(url);

                request.Method = "GET";
                request.ContentType = "text/html;charset=UTF-8";
                request.CookieContainer = cookie;
                request.Timeout = 10000;

                request.Referer = "https://www.bkex.com/";

                HttpWebResponse response = (HttpWebResponse) request.GetResponse();
                Stream myResponseStream = response.GetResponseStream();

                //SERVERID = c1ba0190021e5f5c28890a5e7b37a0df | 1528617470 | 1528617470; Path =/
                string responseCookie = response.Headers.Get("Set-Cookie");

                if (!string.IsNullOrWhiteSpace(responseCookie))
                {
                    GlobalClass.ResponseCookie = responseCookie.Split(';')[0];
                    //GlobalClass.ResponseCookie = responseCookie;
                }


                //StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
                //string retString = myStreamReader.ReadToEnd();
                //myStreamReader.Close();
                //myResponseStream.Close();

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
                request.AddHeader("postman-token", "6d18efca-2f54-3f34-9e27-d4c954cc1992");
                request.AddHeader("timeout", "10000");
                request.AddHeader("referer", "https://www.bkex.com/");
                //request.AddHeader("cookie",
                //    "__cdnuid=9f01158114da4258dc3b351ce44743d8; JSESSIONID=92186E6AB082F939923C4EDD821C5DBC");

                request.AddHeader("cookie",
                    $@"__cdnuid=9f01158114da4258dc3b351ce44743d8; JSESSIONID=92186E6AB082F939923C4EDD821C5DBC; {GlobalClass.ResponseCookie}");

                request.AddHeader("cache-control", "no-cache");
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
                    "\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name=\"telCountryCode\"\r\n\r\n86\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name=\"inviteCode\"\r\n\r\n" +
                    "r5QuO103" + "\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW--",
                    ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);

                if (response != null && !string.IsNullOrWhiteSpace(response.Content))
                {
                    try
                    {
                        var result = JsonConvert.DeserializeObject<CommonModel>(response.Content);

                        //{ "msg":"图形验证码错误.","code":-1,"data":null}
                        //验证码校验失败

                        if (result.msg == "success")
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
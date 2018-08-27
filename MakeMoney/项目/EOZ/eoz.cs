using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using MakeMoney.Common;
using Newtonsoft.Json;
using RestSharp;

namespace MakeMoney.项目.EOZ
{
    public static class eoz
    {
        public static string CaptureImageVerificationCode()
        {
            //获取验证码
            var stream = GetCaptcha();
            if (stream != null)
            {
                //识别验证码
                var verifyingCode = IdentificationVerifyingCode.GetVerifyingCodeByStream(stream, "2040", "jpeg");
                stream.Close();

                return verifyingCode;
            }

            return string.Empty;
        }

        private static Stream GetCaptcha()
        {
            try
            {
                var url = string.Empty;


                GlobalClass.Random += 1;

                var client = new RestClient($@"http://eoz.one/user/captcha?refresh=1&_={GlobalClass.Random}");
                var request = new RestRequest(Method.GET);
                //request.AddHeader("cookie", $@"PHPSESSID={GlobalClass.Cookie}");
                IRestResponse response = client.Execute(request);

                //{ "hash1":434,"hash2":434,"url":"\/user\/captcha?v=5b1f834c85ecd"}

                GlobalClass.Cookie = response.Cookies.Where(m => m.Name == "PHPSESSID").FirstOrDefault().Value;


                if (response != null && !string.IsNullOrWhiteSpace(response.Content))
                {
                    var result = JsonConvert.DeserializeObject<CommonModel>(response.Content);
                    url = result.url;
                }

                if (string.IsNullOrWhiteSpace(url))
                    return null;




                var zx1 = new Cookie("PHPSESSID", GlobalClass.Cookie, "/", ".eoz.one");

                var cookie = new CookieContainer();
                cookie.Add(zx1);

                HttpWebRequest request2 = (HttpWebRequest)WebRequest.Create($@"http://eoz.one{url}");

                request2.Method = "GET";
                //request2.ContentType = "text/html;charset=UTF-8";
                request2.CookieContainer = cookie;
                request2.Timeout = 5000;


                HttpWebResponse response2 = (HttpWebResponse)request2.GetResponse();
                Stream myResponseStream = response2.GetResponseStream();


                var zx = response2.Headers.Get("Set-Cookie");

                return myResponseStream;





                //var client2 = new RestClient($@"http://eoz.one{url}");
                //var request2 = new RestRequest(Method.GET);
                ////request2.AddHeader("cookie", $@"Hm_lvt_28aadc52a97ba78cf35f028e4c3cf751=1528790821; PHPSESSID={GlobalClass.Cookie}; Hm_lpvt_28aadc52a97ba78cf35f028e4c3cf751=1528791243");
                //request.AddHeader("cookie", $@"PHPSESSID={GlobalClass.Cookie}");

                //IRestResponse response2 = client2.Execute(request2);

                //GlobalClass.Cookie = response2.Cookies.Where(m => m.Name == "PHPSESSID").FirstOrDefault().Value;

                //if (response2 != null && response2.RawBytes.Length > 0)
                //{
                //    Stream stream = new MemoryStream(response2.RawBytes);

                //    return stream;
                //}

                return null;

            }
            catch (Exception ex)
            {
                Console.WriteLine("GetCaptcha：异常：" + ex.Message);
                return null;
            }
        }

        public static string Register(string mobile, string imageVerificationCode)
        {
            try
            {

                //密码应该为8至20位非纯数字字符
                var guid = Guid.NewGuid().ToString("N");
                var password = guid.Substring(0, new Random(Guid.NewGuid().GetHashCode()).Next(8, 19));

                var client = new RestClient("http://eoz.one/user/register");
                var request = new RestRequest(Method.POST);
                request.AddHeader("cookie", $@"PHPSESSID={GlobalClass.Cookie}");
                request.AddHeader("content-type", "multipart/form-data; boundary=----WebKitFormBoundary7MA4YWxkTrZu0gW");
                request.AddParameter("multipart/form-data; boundary=----WebKitFormBoundary7MA4YWxkTrZu0gW",
                    "------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name=\"RegisterForm[tel_number]\"\r\n\r\n" +
                    mobile +
                    "\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name=\"RegisterForm[password]\"\r\n\r\n" +
                    password +
                    "\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name=\"RegisterForm[password2]\"\r\n\r\n" +
                    password +
                    "\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name=\"RegisterForm[verifyCode]\"\r\n\r\n" +
                    imageVerificationCode +
                    "\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name=\"RegisterForm[dialcode]\"\r\n\r\n86\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name=\"RegisterForm[countrycode]\"\r\n\r\ncn\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW--",
                    ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);

                if (response != null && !string.IsNullOrWhiteSpace(response.Content))
                {
                    try
                    {
                        //"验证码错误"

                        var result = JsonConvert.DeserializeObject<CommonModel>(response.Content);

                        //{ "msg":"图形验证码错误.","code":-1,"data":null}
                        //验证码校验失败

                        //if (result.msg == "success")
                        //{
                        //    GlobalClass.ValidCount++;

                        //    var sql =
                        //        $@"insert AccountInfo(AIMobile,AIPwd,AIProjectId,AIDescribe,AIWebSite) values('{mobile}','{password}','{ItemEnum.bkex.GetHashCode()}','{CommonHelper.GetEnumDescription(ItemEnum.bkex)}','{"bkex.com"}')";
                        //    var count = new DatabaseHelper().AddOrUpdate(sql);
                        //    if (count > 0)
                        //    {
                        //        return "OK";
                        //    }

                        //    Console.WriteLine("异常：Register：数据新增失败");

                        //    return string.Empty;
                        //}
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

    public class CommonModel
    {
        public string hash1 { get; set; }
        public string hash2 { get; set; }
        public string url { get; set; }
    }
}

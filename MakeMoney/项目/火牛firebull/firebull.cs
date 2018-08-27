using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MakeMoney.Common;
using MakeMoney.Enum;
using Newtonsoft.Json;
using RestSharp;

namespace MakeMoney.项目.火牛firebull
{
    public static class firebull
    {
        public static string MobileSendMessage(string mobile)
        {
            var client = new RestClient("https://www.feixun.tv/api/v1/user/getCheckCode.json?ak=b81a898ee6aa2471726471206c75b690&appName=firebull&appVersion=1.2.9&isWifi=1&partner=0173&phone="+ mobile + "&platform=ios&scene=login&sid=f6607de327b83770d6193ec8d0cae0f0&smid=201808272219353eb2cbe2a5fdff93fe3ae9cf7d6448a0011d22999cd580b4&ticket=&ts=1535387843");
            var request = new RestRequest(Method.GET);
            request.AddHeader("token", "Eyq6KwBnLZQjvGRj");
            request.AddHeader("accept-language", "zh-Hans-CN;q=1, en-CN;q=0.9, en-US;q=0.8");
            request.AddHeader("user-agent", "Bull/1.2.9 (iPhone; iOS 11.4.1; Scale/2.00)");
            request.AddHeader("accept", "*/*");
            request.AddHeader("connection", "keep-alive");
            request.AddHeader("host", "www.feixun.tv");
            IRestResponse response = client.Execute(request);

            if (response != null && !string.IsNullOrWhiteSpace(response.Content))
            {
                var result = JsonConvert.DeserializeObject<CommonModel>(response.Content);
                if (result.success == "true")
                {
                    return "OK";
                }
            }

            return string.Empty;

        }

        public static string Register(string mobile, string verificationCode)
        {
            try
            {
                for (var i = 0; i < 50; i++)
                {
                    var code = RegisterMultiple(mobile, verificationCode);
                    Thread.Sleep(5000);
                    if (code != "OK")
                    {
                        continue;
                    }

                    break;
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Register：异常：" + ex.Message);
                return string.Empty;
            }
        }

        private static string RegisterMultiple(string mobile, string verificationCode)
        {
            try
            {
                var client = new RestClient("https://www.feixun.tv/api/v1/user/login.json");
                var request = new RestRequest(Method.POST);
                request.AddHeader("content-type", "application/x-www-form-urlencoded");
                request.AddHeader("token", "Eyq6KwBnLZQjvGRj");
                request.AddHeader("accept-language", "zh-Hans-CN;q=1, en-CN;q=0.9, en-US;q=0.8");
                request.AddHeader("user-agent", "Bull/1.2.9 (iPhone; iOS 11.4.1; Scale/2.00)");
                request.AddHeader("accept", "*/*");
                request.AddHeader("connection", "keep-alive");
                request.AddHeader("accept-encoding", "br, gzip, deflate");
                request.AddHeader("host", "www.feixun.tv");
                //request.AddParameter("application/x-www-form-urlencoded", "ak=d46b08f215a6dceb493252e8d324260f&appName=firebull&appVersion=1.2.9&checkCode="+ verificationCode + "&invitationCode=9916393&isWifi=1&partner=0173&phone="+ mobile + "&platform=ios&sid=f6607de327b83770d6193ec8d0cae0f0&smid=201808272219353eb2cbe2a5fdff93fe3ae9cf7d6448a0011d22999cd580b4&ticket=&ts=1535384470", ParameterType.RequestBody);
                //request.AddParameter("application/x-www-form-urlencoded", "ak%3De0d4c1b3baab4ca69e204a9b5d82a8d4%26appName%3Dfirebull%26appVersion%3D1.2.9%26checkCode%3D"+ verificationCode + "%26invitationCode%3D9916393%26isWifi%3D1%26partner%3D0173%26phone%3D"+ mobile + "%26platform%3Dios%26sid%3Df6607de327b83770d6193ec8d0cae0f0%26smid%3D201808272219353eb2cbe2a5fdff93fe3ae9cf7d6448a0011d22999cd580b4%26ticket%3D%26ts%3D1535387399=", ParameterType.RequestBody);
                //request.AddParameter("application/json", "ak=d46b08f215a6dceb493252e8d324260f&appName=firebull&appVersion=1.2.9&checkCode=" + verificationCode + "&invitationCode=9916393&isWifi=1&partner=0173&phone=" + mobile + "&platform=ios&sid=f6607de327b83770d6193ec8d0cae0f0&smid=201808272219353eb2cbe2a5fdff93fe3ae9cf7d6448a0011d22999cd580b4&ticket=&ts=1535384470", ParameterType.RequestBody);
                request.AddParameter("application/x-www-form-urlencoded", "ak=e0d4c1b3baab4ca69e204a9b5d82a8d4&appName=firebull&appVersion=1.2.9&checkCode="+ verificationCode + "&invitationCode=9916393&isWifi=1&partner=0173&phone="+ mobile + "&platform=ios&sid=f6607de327b83770d6193ec8d0cae0f0&smid=201808272219353eb2cbe2a5fdff93fe3ae9cf7d6448a0011d22999cd580b4&ticket=&ts=1535387399\r\n", ParameterType.RequestBody);

                IRestResponse response = client.Execute(request);

                if (response != null && !string.IsNullOrWhiteSpace(response.Content))
                {
                    try
                    {
                        var result = JsonConvert.DeserializeObject<CommonModel>(response.Content);
                        if (result.success == "true")
                        {
                            GlobalClass.ValidCount++;

                            var sql =
                                $@"insert AccountInfo(AIMobile,AIPwd,AIProjectId,AIDescribe,AIWebSite) values('{mobile}','','{ItemEnum.firebull.GetHashCode()}','{CommonHelper.GetEnumDescription(ItemEnum.firebull)}','{"firebull.io"}')";
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
                Console.WriteLine("RegisterMultiple：异常：" + ex.Message);
                return string.Empty;
            }
        }


    }

    public class CommonModel
    {
        public string success { get; set; }
        //public string isSuccess { get; set; }
        //public string message { get; set; }
    }
}
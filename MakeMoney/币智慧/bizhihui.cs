using Newtonsoft.Json;
using RestSharp;
using System;

namespace MakeMoney.币智慧
{
    public static class bizhihui
    {
        public static string SendVerificationCode(string mobile)
        {
            //获取验证码
            var svg = GetCaptcha();
            if (string.IsNullOrWhiteSpace(svg))
            {
                return string.Empty;
            }

            //识别验证码
            var verifyingCode = IdentificationVerifyingCode.GetVerifyingCode(svg, "3040");
            if (string.IsNullOrWhiteSpace(verifyingCode))
            {
                return string.Empty;
            }

            //发送短信
            return SendMessage(mobile, verifyingCode);
        }

        private static string SendMessage(string mobile, string verifyingCode)
        {
            var client = new RestClient($@"https://i.bizhihui.vip/api/user/{mobile}/code?code=1527492345361&captcha={verifyingCode}&debug=false");
            var request = new RestRequest(Method.GET);
            request.AddHeader("postman-token", "5f83b84f-915a-88d2-28a1-39e5b7f1ee6e");
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("referer", "https://i.bizhihui.vip/invite/oiBOGp");
            request.AddHeader("cookie", "Hm_lvt_bdfb5a0d594bde83c7c3db4f18d0194c=1527491441; koa:sess=vgIWWvP3o5iiNG2LNEJizNRLqp4eLODs; Hm_lpvt_bdfb5a0d594bde83c7c3db4f18d0194c=1527492337");
            IRestResponse response = client.Execute(request);

            if (response != null && !string.IsNullOrWhiteSpace(response.Content))
            {
                var result = JsonConvert.DeserializeObject<CommonModel>(response.Content);
                if (result.error == "0")
                {
                    return "OK";
                }
            }

            return string.Empty;
        }

        private static string GetCaptcha()
        {
            var client = new RestClient("https://i.bizhihui.vip/api/user/captcha?debug=false");
            var request = new RestRequest(Method.GET);
            request.AddHeader("postman-token", "0b355ef6-7d00-73b9-10ac-2c7be19de104");
            request.AddHeader("cache-control", "no-cache");



            request.AddHeader("cookie2", "Hm_lvt_bdfb5a0d594bde83c7c3db4f18d0194c=1527479746,1527624345; koa:sess=vgIWWvP3o5iiNG2LNEJizNRLqp4eLODs; Hm_lpvt_bdfb5a0d594bde83c7c3db4f18d0194c=1527701080");


            //request.AddHeader("sessionid", "13815271314");

            request.AddHeader("referer", "https://i.bizhihui.vip/invite/oiBOGp");
            request.AddHeader("cookie", "Hm_lvt_bdfb5a0d594bde83c7c3db4f18d0194c=1527491441; koa:sess=vgIWWvP3o5iiNG2LNEJizNRLqp4eLODs; Hm_lpvt_bdfb5a0d594bde83c7c3db4f18d0194c=1527492337");
            IRestResponse response = client.Execute(request);

            if (response != null && !string.IsNullOrWhiteSpace(response.Content))
            {
                var result = JsonConvert.DeserializeObject<CaptchaModel>(response.Content);
                if (result.error == "0")
                {
                    return result.result.captcha;
                }
            }

            return string.Empty;
        }

        public static string Register(string mobile, string verificationCode)
        {
            //密码要6 - 18位，由字母或数字组成
            var guid = Guid.NewGuid().ToString("N");
            var password = guid.Substring(0, new Random(Guid.NewGuid().GetHashCode()).Next(6, 19));

            var client = new RestClient("https://i.bizhihui.vip/api/user?debug=false");
            var request = new RestRequest(Method.POST);
            request.AddHeader("postman-token", "0795d3f6-45af-8420-4c00-daab7f625d6f");
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("timeout", "5000");
            request.AddHeader("content-type", "application/json");



            request.AddHeader("cookie2", "Hm_lvt_bdfb5a0d594bde83c7c3db4f18d0194c=1527479746,1527624345; koa:sess=vgIWWvP3o5iiNG2LNEJizNRLqp4eLODs; Hm_lpvt_bdfb5a0d594bde83c7c3db4f18d0194c=1527701080");


            //request.AddHeader("sessionid", "13815271314");

            request.AddHeader("referer", "https://i.bizhihui.vip/invite/oiBOGp");
            request.AddHeader("cookie",
                "Hm_lvt_bdfb5a0d594bde83c7c3db4f18d0194c=1527491441; koa:sess=vgIWWvP3o5iiNG2LNEJizNRLqp4eLODs; Hm_lpvt_bdfb5a0d594bde83c7c3db4f18d0194c=1527492337");
            request.AddParameter("application/json",
                string.Format(
                    "{{\"phone\": \"{0}\",\"password\": \"{1}\",\"code\": \"{2}\",\"eth_url\": \"\",\"ic\": \"oiBOGp\"}}",
                    mobile, password, verificationCode), ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);

            if (response != null && !string.IsNullOrWhiteSpace(response.Content))
            {
                var result = JsonConvert.DeserializeObject<CommonModel>(response.Content);

                if (result.error != "4")
                {
                    return "OK";
                }
            }

            return string.Empty;
        }
    }

    public class CaptchaModel
    {
        public string error { get; set; }
        public ResultModel result { get; set; }
    }

    public class ResultModel
    {
        public string captcha { get; set; }
    }

    public class CommonModel
    {
        public string error { get; set; }
        public string message { get; set; }
        public string code { get; set; }
    }
}
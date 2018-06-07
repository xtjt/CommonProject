using System;
using System.Configuration;
using System.Threading;
using RestSharp;

namespace MakeMoney
{
    public static class 易码
    {
        /// <summary>
        /// 登录接口
        /// </summary>
        /// <returns></returns>
        public static string GetToken()
        {
            var username = ConfigurationManager.AppSettings["Yima_username"];
            var password = ConfigurationManager.AppSettings["Yima_password"];
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                Console.WriteLine("异常：GetToken：易码配置有误");
                return string.Empty;
            }

            var client =
                new RestClient(
                    $@"http://api.fxhyd.cn/UserInterface.aspx?action=login&username={username}&password={password}");
            var request = new RestRequest(Method.GET);
            request.AddHeader("postman-token", "1b2b7fce-d37b-870c-aa78-86d6b0248cf5");
            request.AddHeader("cache-control", "no-cache");
            IRestResponse response = client.Execute(request);

            //登录成功：success | token
            //登录失败：错误代码，请根据不同错误代码进行不同的处理。

            if (response != null && !string.IsNullOrWhiteSpace(response.Content))
            {
                if (response.Content.Split('|')[0].ToLower() == "success")
                {
                    return response.Content.Split('|')[1];
                }
                else
                {
                    Console.WriteLine("异常：GetToken：" + response.Content);
                }

            }
            else
            {
                Console.WriteLine("异常：GetToken");
            }

            return string.Empty;
        }

        /// <summary>
        /// 获取账户信息接口
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static string GetAccount(string token)
        {
            var client = new RestClient($@"http://api.fxhyd.cn/UserInterface.aspx?action=getaccountinfo&token={token}");
            var request = new RestRequest(Method.GET);
            request.AddHeader("postman-token", "06145a7c-dda2-acd5-f14f-d835a2494cb6");
            request.AddHeader("cache-control", "no-cache");
            IRestResponse response = client.Execute(request);

            //请求参数format != 1：success | 用户名 | 账户状态 | 账户等级 | 账户余额 | 冻结金额 | 账户折扣 | 获取号码最大数量，“|”是分隔符（默认返回数据格式）
            //请求参数format = 1：success | JSON格式数据
            //请求失败：错误代码，请根据不同错误代码进行不同的处理。

            if (response != null && !string.IsNullOrWhiteSpace(response.Content))
            {
                if (response.Content.Split('|')[0].ToLower() == "success")
                {
                    //success | zxxtjt | 1 | 1 | 10.0000 | 10.0000 | 1.000 | 20
                    return response.Content.Substring(8);
                }
                else
                {
                    Console.WriteLine("异常：GetAccount：" + response.Content);
                }
            }
            else
            {
                Console.WriteLine("异常：GetAccount");
            }

            return string.Empty;
        }

        /// <summary>
        /// 获取手机号码接口
        /// </summary>
        /// <param name="token"></param>
        /// <param name="itemid">参考：易码官网查询获取</param>
        /// <returns></returns>
        public static string GetPhoneNumber(string token, string itemid)
        {
            var client = new RestClient($@"http://api.fxhyd.cn/UserInterface.aspx?action=getmobile&token={token}&itemid={itemid}");
            var request = new RestRequest(Method.GET);
            request.AddHeader("postman-token", "4783287d-7af8-c477-5ef1-d4f1df58a15a");
            request.AddHeader("cache-control", "no-cache");
            IRestResponse response = client.Execute(request);

            //获取成功：success | 手机号码
            //请求失败：错误代码，请根据不同错误代码进行不同的处理。

            if (response != null && !string.IsNullOrWhiteSpace(response.Content))
            {
                if (response.Content.Split('|')[0].ToLower() == "success")
                {
                    return response.Content.Split('|')[1];
                }
                else
                {
                    Console.WriteLine("异常：GetPhoneNumber：" + response.Content);
                }
            }
            else
            {
                Console.WriteLine("异常：GetPhoneNumber");
            }

            return string.Empty;
        }

        public static string GetMessage(string token, string itemid, string mobile)
        {
            for (var i = 0; i < 13; i++)
            {
                if (i != 0)
                {
                    Thread.Sleep(5000);
                }

                var result = MultipleMessage(token, itemid, mobile);

                if (!string.IsNullOrWhiteSpace(result))
                {
                    if (result.Split('|')[0].ToLower() == "success")
                    {
                        return result.Split('|')[1];
                    }
                    else if (result == "3001")
                    {
                        continue;
                    }
                    else
                    {
                        Console.WriteLine("异常：GetMessage：" + result);
                    }
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// 获取短信接口
        /// </summary>
        /// <param name="token"></param>
        /// <param name="itemid"></param>
        /// <param name="mobile"></param>
        /// <returns></returns>
        private static string MultipleMessage(string token, string itemid, string mobile)
        {
            var client = new RestClient($@"http://api.fxhyd.cn/UserInterface.aspx?action=getsms&token={token}&itemid={itemid}&mobile={mobile}&release=1");
            var request = new RestRequest(Method.GET);
            request.AddHeader("postman-token", "27beda41-9359-e37d-93cf-4ce6c7ce423a");
            request.AddHeader("cache-control", "no-cache");
            IRestResponse response = client.Execute(request);

            //收到短信：success | 短信内容
            //短信尚未到达：3001，应继续调用取短信接口，直到超时为止。
            //请求失败：错误代码，请根据不同错误代码进行不同的处理。

            if (response != null && !string.IsNullOrWhiteSpace(response.Content))
            {
                return response.Content;
            }
            else
            {
                Console.WriteLine("异常：MultipleMessage");
            }

            return string.Empty;
        }

        /// <summary>
        /// 释放手机号码接口
        /// </summary>
        /// <param name="token"></param>
        /// <param name="itemid"></param>
        /// <param name="mobile"></param>
        public static void ReleasePhoneNumber(string token, string itemid, string mobile)
        {
            var client = new RestClient($@"http://api.fxhyd.cn/UserInterface.aspx?action=release&token={token}&itemid={itemid}&mobile={mobile}");
            var request = new RestRequest(Method.GET);
            request.AddHeader("postman-token", "33c0bcf0-fa51-cef1-a6c6-05521d463385");
            request.AddHeader("cache-control", "no-cache");
            IRestResponse response = client.Execute(request);

            //释放成功：success
            //请求失败：错误代码，请根据不同错误代码进行不同的处理。
        }
    }
}
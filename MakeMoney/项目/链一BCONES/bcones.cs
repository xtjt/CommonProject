using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MakeMoney.Common;
using MakeMoney.Enum;
using Newtonsoft.Json;
using RestSharp;

namespace MakeMoney.项目.链一BCONES
{
    public static class bcones
    {
        public static string MobileSendMessage(string mobile)
        {
            var client = new RestClient("https://api.bcones.com/message/message/sendMessage");
            var request = new RestRequest(Method.POST);
            request.AddHeader("referer", "https://www.bcone.vip/v/register");
            request.AddHeader("origin", "https://www.bcone.vip");
            request.AddHeader("content-type", "application/json");
            request.AddParameter("application/json", "{\r\n\t\"cellphone\": \""+ mobile + "\",\r\n\t\"from_type\": 1\r\n}", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);

            if (response!=null&&!string.IsNullOrWhiteSpace(response.Content))
            {
                var result = JsonConvert.DeserializeObject<CommonModel>(response.Content);
                if (result.isSuccess == "true")
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

                //密码应该为8至20位非纯数字字符
                var guid = Guid.NewGuid().ToString("N");
                var password = guid.Substring(0, new Random(Guid.NewGuid().GetHashCode()).Next(8, 19));

                var client = new RestClient("https://api.bcones.com/user/member/register");
                var request = new RestRequest(Method.POST);
                request.AddHeader("content-type", "application/json");
                request.AddParameter("application/json",
                    "{\r\n\t\"cellphone\": \"" + mobile + "\",\r\n\t\"country\": 1,\r\n\t\"pwd\": \"" + password +
                    "\",\r\n\t\"pwd1\": \"" + password +
                    "\",\r\n\t\"recommendCode\": \"mqBfWE\",\r\n\t\"source\": \"1\",\r\n\t\"verCode\": \"" +
                    verificationCode + "\",\r\n\t\"verr\": false\r\n}", ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);

                if (response != null && !string.IsNullOrWhiteSpace(response.Content))
                {
                    try
                    {
                        var result = JsonConvert.DeserializeObject<CommonModel>(response.Content);
                        if (result.isSuccess == "true")
                        {
                            GlobalClass.ValidCount++;

                            var sql =
                                $@"insert AccountInfo(AIMobile,AIPwd,AIProjectId,AIDescribe,AIWebSite) values('{mobile}','{password}','{ItemEnum.bcones.GetHashCode()}','{CommonHelper.GetEnumDescription(ItemEnum.bcones)}','{"bcones.com"}')";
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

    public class CommonModel
    {
        public string flag { get; set; }
        public string isSuccess { get; set; }
        public string message { get; set; }
    }
}

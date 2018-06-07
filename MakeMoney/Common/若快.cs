using System;
using System.Collections.Generic;
using System.Configuration;
using System.Xml;

namespace MakeMoney.Common
{
    public static class 若快
    {
        /// <summary>
        /// 解析验证码
        /// </summary>
        /// <param name="data"></param>
        /// <param name="typeid"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string ParseVerificationCode(byte[] data, string typeid, string type)
        {
            if (data.Length <= 0 || string.IsNullOrWhiteSpace(typeid) || string.IsNullOrWhiteSpace(type))
            {
                Console.WriteLine("异常：ParseVerificationCode：若快配置有误");
                return string.Empty;
            }

            var username = ConfigurationManager.AppSettings["Ruokuai_username"];
            var password = ConfigurationManager.AppSettings["Ruokuai_password"];
            var softid = ConfigurationManager.AppSettings["Ruokuai_softid"];
            var softkey = ConfigurationManager.AppSettings["Ruokuai_softkey"];
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password) ||
                string.IsNullOrWhiteSpace(softid) || string.IsNullOrWhiteSpace(softkey))
            {
                Console.WriteLine("异常：ParseVerificationCode：若快配置有误");
                return string.Empty;
            }

            //必要的参数
            var param = new Dictionary<object, object>
            {
                {"username", username},
                {"password", password},
                {"typeid", typeid},
                {"timeout", "90"},
                {"softid", softid},
                {"softkey", softkey}
            };

            //提交服务器
            string httpResult = RuoKuaiHttp.Post("http://api.ruokuai.com/create.xml", param, data);

            XmlDocument xmlDoc = new XmlDocument();
            try
            {
                xmlDoc.LoadXml(httpResult);
            }
            catch (Exception ex)
            {
                Console.WriteLine("异常：ParseVerificationCode：返回格式有误：" + ex.Message);
            }

            XmlNode idNode = xmlDoc.SelectSingleNode("Root/Id");
            XmlNode resultNode = xmlDoc.SelectSingleNode("Root/Result");
            XmlNode errorNode = xmlDoc.SelectSingleNode("Root/Error");
            string result = string.Empty;
            string topidid = string.Empty;
            if (resultNode != null && idNode != null)
            {
                topidid = idNode.InnerText;
                result = resultNode.InnerText;

                return result;
            }
            else if (errorNode != null)
            {
                Console.WriteLine("异常：ParseVerificationCode：识别错误：" + errorNode.InnerText);
            }
            else
            {
                Console.WriteLine("异常：ParseVerificationCode：未知问题");
            }

            return string.Empty;
        }
    }
}
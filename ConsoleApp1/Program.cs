



using System;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Security.Policy;
using System.Text;
using IronPython.Hosting;
using MakeMoney.Common;
using MakeMoney.Enum;
using MakeMoney.项目.币智慧;
using Microsoft.Scripting.Hosting;
using RestSharp;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            var zx1 = ItemEnum.bizhihui;
            var zx2 = ItemEnum.bizhihui.GetHashCode();
            //var zx3 = bizhihui.GetEnumDescription(ItemEnum.bizhihui);

            var sql =
                $@"insert AccountInfo(AIMobile,AIPwd,AIProjectId,AIDescribe,AIWebSite) values(1,1,1,1,1)";
            //var count = new DatabaseHelper().AddOrUpdate(sql);
            //Data Source =.\SQLEXPRESS; Database = ASPNETDB; User id = sa; PWD = 123" providerName="System.Data.SqlClient

            var _connString = ConfigurationManager.ConnectionStrings["DBConnectionString"].ConnectionString;
            SqlConnection conn = new SqlConnection(_connString); //Initial Catalog后面跟你数据库的名字  
                //.\SQLEXPRESS

            conn.Open();
            SqlCommand cmd = new SqlCommand(sql, conn);
            int result = cmd.ExecuteNonQuery(); //result接收受影响行数，也就是说result大于0的话表示添加成功  
            conn.Close();
            cmd.Dispose();
        


         






            string path = new DirectoryInfo("../../../").FullName + $@"/PythonHelper/PythonHelper/PythonHelper.py";
            ; //当前应用程序路径的上级目录





            //return;

            for (var i = 0; i <= 100; i++)
            {
             



                //ScriptRuntime pyRuntime = Python.CreateRuntime(); //创建一下运行环境
                //dynamic obj = pyRuntime.UseFile(path); //调用一个Python文件
                //string num1 = "13157045772", num2 = "ber4";

                //string sum = obj.getMessage(num1, num2); //调用Python文件中的求和函数
                //Console.Write("Sum:");
                //Console.WriteLine(sum);













                //var client = new RestClient("https://i.bizhihui.vip/api/user/captcha?debug=false");
                //var request = new RestRequest(Method.GET);
                //request.AddHeader("postman-token", "788c5081-dca0-0e4c-82c9-ad8888475938");
                //request.AddHeader("cache-control", "no-cache");
                //request.AddHeader("referer", "https://i.bizhihui.vip/invite/oqawlC");
                //request.AddHeader("cookie", "Hm_lvt_bdfb5a0d594bde83c7c3db4f18d0194c=1527491441; koa:sess=vgIWWvP3o5iiNG2LNEJizNRLqp4eLODs; Hm_lpvt_bdfb5a0d594bde83c7c3db4f18d0194c=1527492337");
                //IRestResponse response = client.Execute(request);

                Console.Write("输入验证码：");

                var code = Console.ReadLine().Trim();

                var url = $@"https://i.bizhihui.vip/api/user/13157045772/code?code=1527492345361&captcha={code}&debug=false";

                HttpGet(url,string.Empty);




                //var zxzx = "https://i.bizhihui.vip/api/user/13157045772/code?code=1527492345361&captcha=" + code +
                //           "&debug=false";
                //var client =
                //    new RestClient(
                //        "https://i.bizhihui.vip/api/user/13157045772/code?code=1527492345361&captcha=h1pv&debug=false");
                //var request = new RestRequest(Method.GET);
                //request.AddHeader("postman-token", "2f58ee7b-d823-15ed-6653-8f1f83d09937");
                //request.AddHeader("cache-control", "no-cache");
                //request.AddHeader("referer", "https://i.bizhihui.vip/invite/oqawlC");
                //request.AddHeader("cookie",
                //    "Hm_lvt_bdfb5a0d594bde83c7c3db4f18d0194c=1527491441; koa:sess=vgIWWvP3o5iiNG2LNEJizNRLqp4eLODs; Hm_lpvt_bdfb5a0d594bde83c7c3db4f18d0194c=1527492337");
                //IRestResponse response = client.Execute(request);

            }
        }

        private static string HttpPost(string Url, string postDataStr)
        {
            //var client = new RestClient("https://i.bizhihui.vip/api/user?debug=false");
            //var request = new RestRequest(Method.POST);
            //request.AddHeader("postman-token", "4e47120b-9839-5d25-ec2d-bc2233fefd90");
            //request.AddHeader("cache-control", "no-cache");
            //request.AddHeader("content-type", "application/json");
            //request.AddHeader("referer", "https://i.bizhihui.vip/invite/oqawlC");
            //request.AddHeader("cookie", "Hm_lvt_bdfb5a0d594bde83c7c3db4f18d0194c=1527491441; koa:sess=vgIWWvP3o5iiNG2LNEJizNRLqp4eLODs; Hm_lpvt_bdfb5a0d594bde83c7c3db4f18d0194c=1527492337");
            //request.AddParameter("application/json", "{"phone": "17601475279","password": "love1","code": "2770","eth_url": "","ic": "oqawlC"rn}", ParameterType.RequestBody);
            //IRestResponse response = client.Execute(request);




            postDataStr =
                "\"application / json\": \"{ \"phone\": \"17601475279\",\"password\": \"love1\",\"code\": \"2770\",\"eth_url\": \"\",\"ic\": \"oqawlC";


            HttpWebRequest request = (HttpWebRequest) WebRequest.Create(Url);
            request.Method = "POST";

            var zx1 = new Cookie("Hm_lvt_bdfb5a0d594bde83c7c3db4f18d0194c", "1527491441", "/", ".bizhihui.vip");
            var zx2 = new Cookie("koa:sess", "vgIWWvP3o5iiNG2LNEJizNRLqp4eLODs", "/", ".bizhihui.vip");

            var zx3 = new Cookie("Hm_lpvt_bdfb5a0d594bde83c7c3db4f18d0194c", "1527492337", "/", ".bizhihui.vip");
            //new Cookie(name, value, path, domain);

            var cookie = new CookieContainer();




            cookie.Add(zx1);
            cookie.Add(zx2);
            cookie.Add(zx3);





            //request.ContentType = "application/x-www-form-urlencoded";

            request.ContentType = "application/json";

            request.ContentLength = Encoding.UTF8.GetByteCount(postDataStr);
            request.CookieContainer = cookie;
            request.Timeout = 5000;
            //request.Host = "i.bizhihui.vip";

            //request.CookieContainer = cookie;
            Stream myRequestStream = request.GetRequestStream();
            StreamWriter myStreamWriter = new StreamWriter(myRequestStream, Encoding.GetEncoding("gb2312"));



            myStreamWriter.Write(postDataStr);
            myStreamWriter.Close();

            request.Referer = "https://i.bizhihui.vip/invite/oqawlC";

            HttpWebResponse response = (HttpWebResponse) request.GetResponse();

            response.Cookies = cookie.GetCookies(response.ResponseUri);
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();

            return retString;
        }

        public static string HttpGet(string Url, string postDataStr)
        {

            //var client = new RestClient("https://i.bizhihui.vip/api/user/13157045772/code?code=1527492345361&captcha=ber4&debug=false");
            //var request = new RestRequest(Method.GET);
            //request.AddHeader("postman-token", "0cdf4b77-b2e1-64cc-ccf5-cb62d9cbe6a2");
            //request.AddHeader("cache-control", "no-cache");
            //request.AddHeader("referer", "https://i.bizhihui.vip/invite/oqawlC");
            //request.AddHeader("cookie", "Hm_lvt_bdfb5a0d594bde83c7c3db4f18d0194c=1527491441; koa:sess=vgIWWvP3o5iiNG2LNEJizNRLqp4eLODs; Hm_lpvt_bdfb5a0d594bde83c7c3db4f18d0194c=1527492337");
            //IRestResponse response = client.Execute(request);


            //var Cookie = $@"PHPSESSID = q00o8srgl6gomqs830t9cfuhvs; eth = {code}";

            var zx1 = new Cookie("Hm_lvt_bdfb5a0d594bde83c7c3db4f18d0194c", "1527491441", "/", ".bizhihui.vip");
            var zx2 = new Cookie("koa:sess", "vgIWWvP3o5iiNG2LNEJizNRLqp4eLODs", "/", ".bizhihui.vip");

            var zx3 = new Cookie("Hm_lpvt_bdfb5a0d594bde83c7c3db4f18d0194c", "1527492337", "/", ".bizhihui.vip");
            //new Cookie(name, value, path, domain);

            var cookie = new CookieContainer();




            cookie.Add(zx1);
            cookie.Add(zx2);
            cookie.Add(zx3);

            HttpWebRequest request =
                (HttpWebRequest) WebRequest.Create(Url + (string.IsNullOrWhiteSpace(postDataStr) ? "" : "?") +
                                                   postDataStr);

            request.Method = "GET";
            request.ContentType = "text/html;charset=UTF-8";
            request.CookieContainer = cookie;
            request.Timeout = 5000;

            request.Referer = "https://i.bizhihui.vip/invite/oqawlC";

            HttpWebResponse response = (HttpWebResponse) request.GetResponse();
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();

            return retString;
        }





    }
}

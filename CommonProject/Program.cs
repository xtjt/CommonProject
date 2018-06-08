using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;

namespace CommonProject
{
    class Program
    {




        static void Main(string[] args)
        {
            var client = new RestClient("https://i.bizhihui.vip/api/user/captcha?debug=false");
            var request = new RestRequest(Method.GET);
            request.AddHeader("postman-token", "3fc9753c-af79-d180-ede2-197ea06c6632");
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("referer", "https://i.bizhihui.vip/invite/oiBOGp");
            request.AddHeader("cookie",
                "Hm_lvt_bdfb5a0d594bde83c7c3db4f18d0194c=1527491441; koa:sess=vgIWWvP3o5iiNG2LNEJizNRLqp4eLODs; Hm_lpvt_bdfb5a0d594bde83c7c3db4f18d0194c=1527492337");
            IRestResponse response = client.Execute(request);

            var captcha = Console.ReadLine().Trim();

            var zx =
                "https://i.bizhihui.vip/api/user/13157045772/code?code=1527492345361&captcha="+ captcha + "&debug=false";


           var  client2 = new RestClient(zx);
           var  request2 = new RestRequest(Method.GET);
            request.AddHeader("postman-token", "97a1825e-aef4-9a0a-0cb4-3fa9b559d2f3");
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("referer", "https://i.bizhihui.vip/invite/oiBOGp");
            request.AddHeader("cookie",
                "Hm_lvt_bdfb5a0d594bde83c7c3db4f18d0194c=1527491441; koa:sess=vgIWWvP3o5iiNG2LNEJizNRLqp4eLODs; Hm_lpvt_bdfb5a0d594bde83c7c3db4f18d0194c=1527492337");
            IRestResponse response2 = client2.Execute(request2);




        }
    }
}

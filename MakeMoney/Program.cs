using MakeMoney.币智慧;
using RestSharp;
using System;
using System.Threading;

namespace MakeMoney
{
    class Program
    {
        static void Main(string[] args)
        {
            var token = 易码.GetToken(); //token:0058389490b192cd9560715cfdeb8ab32452930d

            //success | 用户名 | 账户状态 | 账户等级 | 账户余额 | 冻结金额 | 账户折扣 | 获取号码最大数量
            //success | zxxtjt | 1 | 1 | 0.0000 | 0.0000 | 1.000 | 20

            var valid = 0;

            for (var i = 0; ; i++)
            {
                Console.WriteLine("当前次数：" + (i + 1) + "  有效次数：" + (i - valid));

                try
                {
                    if (i % 100 == 0)
                    {
                        var account = 易码.GetAccount(token);
                        if (string.IsNullOrWhiteSpace(account))
                        {
                            continue;
                        }

                        var amount = Convert.ToDecimal(account.Split('|')[3]);

                        if (amount <= 10)
                        {
                            Console.WriteLine("异常：易码余额不足");
                        }
                        else if (amount <= 0)
                        {
                            Console.ReadKey();
                        }
                    }

                    var itemEnum = ItemEnum.bizhihui; // 币智慧：16486

                    var itemid = itemEnum.GetHashCode().ToString();

                    var mobile = 易码.GetPhoneNumber(token, itemid);

                    switch (itemEnum)
                    {
                        case ItemEnum.bizhihui:
                            var response = bizhihui.SendVerificationCode(mobile);
                            if (response != "OK")
                            {
                                continue;
                            }

                            break;
                        default:
                            continue;
                    }

                    var message = 易码.GetMessage(token, itemid, mobile);
                    if (string.IsNullOrWhiteSpace(message))
                    {
                        continue;
                    }




                    //15543491284



                    switch (itemEnum)
                    {
                        case ItemEnum.bizhihui:
                            bizhihui.Register(mobile, message);
                            break;
                    }

                    易码.ReleasePhoneNumber(token, itemid, mobile);
                }
                catch (Exception ex)
                {
                    valid++;
                    Console.WriteLine("异常：" + ex.Message);
                }

                //Thread.Sleep(2000);

                //if (i % 10 == 0)
                //{ Thread.Sleep(10000); }
                //else
                //{
                //Thread.Sleep(500);
                //}
            }
        }
    }
}
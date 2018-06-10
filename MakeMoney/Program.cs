﻿using RestSharp;
using System;
using System.Threading;
using MakeMoney.Common;
using MakeMoney.Enum;
using MakeMoney.项目.币客BKEX;
using MakeMoney.项目.币智慧;

namespace MakeMoney
{
    public sealed class GlobalClass
    {
        public static int ValidCount=0;

        public static string ResponseCookie = string.Empty;
    }

    class Program
    {
        public static int Test = 0;

        static void Main(string[] args)
        {
            var token = 易码.GetToken(); //token:0058389490b192cd9560715cfdeb8ab32452930d

            //success | 用户名 | 账户状态 | 账户等级 | 账户余额 | 冻结金额 | 账户折扣 | 获取号码最大数量
            //success | zxxtjt | 1 | 1 | 0.0000 | 0.0000 | 1.000 | 20

            //var valid = 0;

            for (var i = 0;; i++)
            {
                Console.WriteLine("当前次数：" + (i + 1) + "  有效次数：" + GlobalClass.ValidCount);

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

                    //var itemEnum = ItemEnum.bizhihui; // 币智慧：16486
                    var itemEnum = ItemEnum.bkex; // 币客BKEX：16486 

                    var itemid = itemEnum.GetHashCode().ToString();

                    //var mobile = 易码.GetPhoneNumber(token, itemid);


                    var mobile = "15061815279";

                    if (itemEnum == ItemEnum.bizhihui)
                    {
                        // 第一步：根据验证码发送短信
                        var response = bizhihui.SendVerificationCode(mobile);
                        if (response != "OK")
                        {
                            continue;
                        }

                        // 第二步：获取短信验证码
                        var message = 易码.GetMessage(token, itemid, mobile);
                        if (string.IsNullOrWhiteSpace(message))
                        {
                            continue;
                        }

                        // 第三步：注册
                        var verificationCode = message.Substring(9, 4);
                        bizhihui.Register(mobile, verificationCode);
                    }
                    else if (itemEnum == ItemEnum.bkex)
                    {
                        // 第一步：获取图形验证码并识别验证码
                        var imageVerificationCode = bkex.CaptureImageVerificationCode();
                        if (string.IsNullOrWhiteSpace(imageVerificationCode))
                        {
                            continue;
                        }

                        // 第二步：发送短信验证码
                        var response = bkex.MobileSendMessage(mobile);
                        if (response != "OK")
                        {
                            continue;
                        }

                        // 第三步：获取手机验证码
                        //var message = 易码.GetMessage(token, itemid, mobile);


                        var message = Console.ReadLine();


                        if (string.IsNullOrWhiteSpace(message))
                        {
                            continue;
                        }

                        //var verificationCode = message.Substring(12, 6);


                        bkex.MultipleRegister(mobile, message, imageVerificationCode);
                    }

                    易码.ReleasePhoneNumber(token, itemid, mobile);
                }
                catch (Exception ex)
                {
                    //valid++;
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
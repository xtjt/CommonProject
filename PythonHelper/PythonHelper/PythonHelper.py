import requests

def getMessage(iphone,captcha):
    url = "https://i.bizhihui.vip/api/user/"+iphone+"/code"

    querystring = {"code":"1527492345361","captcha":captcha,"debug":"false"}

    headers = {
        'cookie': "Hm_lvt_bdfb5a0d594bde83c7c3db4f18d0194c=1527491441; koa:sess=vgIWWvP3o5iiNG2LNEJizNRLqp4eLODs; Hm_lpvt_bdfb5a0d594bde83c7c3db4f18d0194c=1527492337",
        'referer': "https://i.bizhihui.vip/invite/oiBOGp",
        'cache-control': "no-cache",
        'postman-token': "1e93d6fc-b6f0-cfc3-a7b5-2ef5878da781"
        }

    response = requests.request("GET", url, headers=headers, params=querystring)

    return response.text;
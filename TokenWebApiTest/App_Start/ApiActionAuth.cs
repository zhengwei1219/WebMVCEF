using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using TokenWebApiTest.Models;
using TokenWebApiTest.CommonCS;

namespace TokenWebApiTest.App_Start
{
    public class ApiActionAuth : AuthorizeAttribute
    {
        public override void OnAuthorization(HttpActionContext context)
        {
            var authHeader = context.Request.Headers.FirstOrDefault(a => a.Key == "ApiAuthorization");//获取接收的Token

            if (context.Request.Headers == null || !context.Request.Headers.Any() || authHeader.Key == null || string.IsNullOrEmpty(authHeader.Value.FirstOrDefault()))
            {
                Throw401Exception(context, "NoToken");
                return;
            }

            var sendToken = authHeader.Value.FirstOrDefault();

            //url获取token 
            var now = Math.Round((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds + 5);//当前的时间戳
            var dictPayload = DecodeToken(sendToken);

            if (dictPayload == null)
            {
                Throw401Exception(context, "InvalidToken");
            }
            double iat = dictPayload["iat"];
            double exp = dictPayload["exp"];
            //检查令牌的有效期
            if (!(iat < now && now < exp))//如果当前时间戳不再Token声明周期范围内，则返回Token过期
            {
                Throw401Exception(context, "TokenTimeout");
            }
            //获取Token的自定义键值对
            int UserId = dictPayload["UserId"];
            string UserName = dictPayload["UserName"];
            string UserPwd = dictPayload["UserPwd"];
            string UserRole = dictPayload["UserRole"];

            //把toke用户数据放到 HttpContext.Current.User 里
            ClientUserData clientUserData = new ClientUserData()
            {
                UserId = UserId,
                UserName = UserName,
                UserPwd = UserPwd,
                UserRole = UserRole

            };
            if (HttpContext.Current != null)
            {
                HttpContext.Current.User = new UserPrincipal(clientUserData);
            }

        }

        private static IDictionary<string, dynamic> DecodeToken(string token)
        {
            try
            {
                var dictPayload = JWT.JsonWebToken.DecodeToObject(token, CommonToken.SecretKey) as IDictionary<string, dynamic>;
                return dictPayload;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private static void Throw401Exception(HttpActionContext actionContext, string exceptionString)
        {
            var response = HttpContext.Current.Response;
            throw new HttpResponseException(
           actionContext.Request.CreateErrorResponse(System.Net.HttpStatusCode.Unauthorized, exceptionString ?? "Unauthorized"));
        }
        private static string RequestToString(HttpRequestMessage request)
        {
            var message = new StringBuilder();
            if (request.Method != null)
                message.Append(request.Method);

            if (request.RequestUri != null)
                message.Append(" ").Append(request.RequestUri);
            return message.ToString();
        }
    }
}
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SIE.Configuration;
using SIE.Context;
using SIE.Core.ApiLogs;
using SIE.DataPortal;
using SIE.Localization;
using SIE.Log;
using SIE.Logging;
using SIE.Security;
using SIE.Serialization;
using System;
using System.Collections.Specialized;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace SIE.WebApiHost.Controllers
{
    /// <summary>
    /// 数据门户请求接口
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class DataPortalController : ControllerBase
    {
        /// <summary>
        /// 获取客户段ip，尝试通过"X-Forwarded-For"读取经过代理的原始请求ip
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static string GetClientIp(HttpContext context)
        {
            var ip = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            if (string.IsNullOrEmpty(ip))
            {
                ip = context.Connection.RemoteIpAddress.MapToIPv4().ToString();
            }
            return ip;
        }

        /// <summary>
        /// POST方式调用API
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("Invoke")]
        public ApiResponse Invoke([FromBody] ApiRequest request)
        {
            DistributionContext.Clear();
            var ip = GetClientIp(HttpContext);
            // 获取当前的HttpContext对象
            var context = HttpContext;

            if (request.Context == null)
                request.Context = new HybridDictionary();

            var requestLog = new RequestLog();
            requestLog.ApiRequest = request;
            requestLog.Ip = ip;
            requestLog.RequestType = RequestType.Api;
            request.Context[Context.ContextKeys.ClientIPAddress] = ip;

            //本地化赋值
            SetApiLanguage(request.Context);

            using (var apiLogRequest = new ApiRequestLog(request))
            {
                Action<Exception> handler = e =>
                {
                    System.Diagnostics.Debug.WriteLine(e);
                    apiLogRequest.HasException = YesNo.Yes;
                    requestLog.IsSuccess = false;
                    requestLog.Exception = e;
                };
                try
                {
                    ApiResponse response = null;
                    using (new RequestLogStopwatch(requestLog))
                    {
                        response = DataPortalManager.Server.Invoke(request, handler);                       
                        requestLog.ApiResponse = response;
                    }
                    apiLogRequest.SetApiResponse(response);
                    return response;
                }
                finally
                {
                    DistributionContext.Clear();
                }
            }
        }

        /// <summary>
        /// POST方式调用API
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("Execute")]
        public string Execute([FromBody] string request)
        {
            DistributionContext.Clear();
            var data = GetRequest(request);
            if (data.Context == null)
                data.Context = new HybridDictionary();

            string ip = string.Empty;
            if (data.Context.Contains(ContextKeys.ClientIPAddress))
            {
                ip = data.Context[ContextKeys.ClientIPAddress].ToString();
            }
            else
            {
                ip = GetClientIp(HttpContext);
                data.Context[Context.ContextKeys.ClientIPAddress] = ip;
            }
            var query = HttpContext.Request.Query;
            var requestLog = new RequestLog();
            requestLog.DataPortalRequest = data;
            requestLog.Query = query;
            requestLog.Ip = ip;
            requestLog.RequestType = RequestType.Other;

            //本地化赋值
            SetApiLanguage(data.Context);

            Action<Exception> handler = e =>
            {
                System.Diagnostics.Debug.WriteLine(e);
                requestLog.IsSuccess = false;
                requestLog.Exception = e;
            };
            try
            {
                DataPortalResponse response = null;
                using (new RequestLogStopwatch(requestLog))
                {
                    response = DataPortalManager.Server.Execute(data, handler);
                    requestLog.DataPortalResponse = response;
                }
                return CompressResponse(response);
            }
            finally
            {
                DistributionContext.Clear();
            }
        }

        string CompressResponse(DataPortalResponse response)
        {
            var bytes = Serializer.Serialize(response);
            MemoryStream memoryStream = new MemoryStream();
            using (GZipStream gzStream = new GZipStream(memoryStream, CompressionMode.Compress, true))
                gzStream.Write(bytes, 0, bytes.Length);
            return Convert.ToBase64String(memoryStream.ToArray());
        }

        DataPortalRequest GetRequest(string request)
        {
            try
            {
                var compressedBytes = Convert.FromBase64String(request);
                MemoryStream memoryStream = new MemoryStream(compressedBytes, 0, compressedBytes.Length);
                MemoryStream decompressedStream = new MemoryStream();
                using (GZipStream decompressionStream = new GZipStream(memoryStream, CompressionMode.Decompress))
                    decompressionStream.CopyTo(decompressedStream);
                return (DataPortalRequest)Serializer.Deserialize(decompressedStream.ToArray());
            }
            catch (Exception exc)
            {
                LogService.LoggerError.Error("请求内容无法反序列化:".L10N() + request, exc);
                throw new SecurityException("数据门户请求的内容无法反序列化".L10N());
            }
        }

        /// <summary>
        /// 设置本地化语言
        /// </summary>
        /// <param name="context"></param>
        void SetApiLanguage(HybridDictionary context)
        {
            //语言类型默认取配置文件ConfigKeys.DevCulture的值,如果没有，则取中文
            var culture = RT.Service.Resolve<CultureController>().GetDevCulture();
            if (context[ContextKeys.Language] != null)
                culture = context[ContextKeys.Language].ToString();
            RT.ResourceService.Culture = culture;
        }
    }
}

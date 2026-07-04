using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Org.BouncyCastle.Crypto.Tls;
using SIE.Common.Schdules;
using SIE.DataPortal;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.KZ.Base.Interfaces.Enums;
using SIE.KZ.Base.SmomControl;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.ListAtts
{
    /// <summary>
    /// 考勤数据 控制器
    /// </summary>
    public partial class ListAttController : DomainController
    {
        /// <summary>
        /// 执行查询
        /// </summary>
        /// <param name="listAttCriteria"></param>
        /// <returns></returns>
        public virtual EntityList<ListAtt> Fetch(ListAttCriteria listAttCriteria)
        {
            var q = Query<ListAtt>();
            //部门名称
            if (!listAttCriteria.DeptName.IsNullOrEmpty())
            {
                q.Where(p => p.DeptName.Contains("%" + listAttCriteria.DeptName + "%"));
            }
            //区域名称
            if (!listAttCriteria.AreaName.IsNullOrEmpty())
            {
                q.Where(p => p.AreaName.Contains("%" + listAttCriteria.AreaName + "%"));
            }
            //人员编号
            if (!listAttCriteria.Pin.IsNullOrEmpty())
            {
                q.Where(p => p.Pin.Contains("%" + listAttCriteria.Pin + "%"));
            }
            //人员姓名
            if (!listAttCriteria.Name.IsNullOrEmpty())
            {
                q.Where(p => p.Name.Contains("%" + listAttCriteria.Name + "%"));
            }
            //记录设备触发时间
            if (listAttCriteria.EventTimeS.HasValue)
            {
                q.Where(p => p.EventTime >= listAttCriteria.EventTimeS);
            }
            //记录设备触发时间
            if (listAttCriteria.EventTimeE.HasValue)
            {
                q.Where(p => p.EventTime <= listAttCriteria.EventTimeE);
            }

            return q.OrderBy(listAttCriteria.OrderInfoList).ToList(listAttCriteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }



        /// <summary>
        /// 执行调度。从外部接口获取考勤数据并推送至各工厂，如果禁用库存组织则推送至各基地
        /// </summary>
        /// <returns></returns>
        public virtual async Task<string> ExecJob(object param)
        {
            //查找所有工厂
            var smomControlSettingDic = RT.Service.Resolve<SmomBaseController>().GetSmomControlSettings().ToDictionary(p => p.FactoryCode);
            if (smomControlSettingDic == null || smomControlSettingDic.Count == 0) { throw new ValidationException("没有维护SMOM总控配置数据!".L10N()); }


            //由于数据实体禁用了库存组织，这里获取各个工厂的FactoryUrl后去重。
            List<string> FactoryUrls = new List<string>();
            foreach (var item in smomControlSettingDic) { FactoryUrls.Add(item.Value.FactoryUrl); }
            List<string> ServerUrls = FactoryUrls.Distinct().ToList();
            List<ApiServerInfo> apiServerInfoList = new List<ApiServerInfo>();
            foreach (var url in ServerUrls)
            {
                ApiServerInfo apiServerInfo = new ApiServerInfo();
                apiServerInfo.ApiUrl = url;
                apiServerInfo.ApiIpPort = url;
                try { apiServerInfo.ApiIpPort = url.Substring(url.IndexOf("http://") + "http://".Length, url.IndexOf("/api/") - "http://".Length); } catch { }
                PostResData postResData = new PostResData
                {
                    DataRows = 0,
                    AddRows = 0,
                    FailRows = 0,
                    IgnoreRow = 0
                };
                apiServerInfo.ApiPostResData = postResData;
                apiServerInfoList.Add(apiServerInfo);
            }


            //调度参数处理
            var p = param as SendGroupListAttDataToFactoryJobParameter;
            string startDate = System.DateTime.Now.AddDays(p.Days > 0 ? (p.Days - 1) * -1 : 0).ToString("yyyy-MM-dd");
            string endDate = System.DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
            string requestServer = p.RequestServer.Substring(0, (p.RequestServer + "?").IndexOf("?"));


            //请求接口，获取数据，并推送至各工厂/基地
            int pageNo = 0;
            while (true)
            {
                pageNo += 1;
                string requestUrl = $"{p.RequestServer}?startDate={startDate}&endDate={endDate}&pageNo={pageNo}&pageSize=200&access_token={p.AccessToken}";
                HttpResponseMessage response = await _httpClient.GetAsync(requestUrl);
                response.EnsureSuccessStatusCode();
                string responseText = await response.Content.ReadAsStringAsync();
                dynamic resdata = JsonConvert.DeserializeObject(responseText);
                if (resdata.data == null || resdata.data.Count <= 0) { break; }
                string infoJson = JsonConvert.SerializeObject(resdata.data);
                List<ListAttInfo> listAttInfos = ConvertToAttInfoList(infoJson);
                foreach (ApiServerInfo apiServerInfo in apiServerInfoList)
                {
                    PostResData postResData = await CallPostApiAsync(apiServerInfo.ApiUrl, listAttInfos);
                    apiServerInfo.ApiPostResData.DataRows += postResData.DataRows;
                    apiServerInfo.ApiPostResData.AddRows += postResData.AddRows;
                    apiServerInfo.ApiPostResData.FailRows += postResData.FailRows;
                    apiServerInfo.ApiPostResData.IgnoreRow += postResData.IgnoreRow;
                }
            }

            //拼装返回值，给日志记录
            string jobRes = "";
            foreach (ApiServerInfo apiServerInfo in apiServerInfoList)
            {
                jobRes += $"基地服务器[{apiServerInfo.ApiIpPort}],总数据[{apiServerInfo.ApiPostResData.DataRows}]条，添加成功[{apiServerInfo.ApiPostResData.AddRows}]条，添加失败[{apiServerInfo.ApiPostResData.FailRows}]条，忽略已存在[{apiServerInfo.ApiPostResData.IgnoreRow}]条。\r\n";
            }
            return jobRes;
        }






        /// <summary>
        /// HttpClient实例。复用HttpClient实例（避免频繁创建导致TCP端口耗尽）
        /// </summary>
        private static readonly HttpClient _httpClient = new HttpClient(new HttpClientHandler { ServerCertificateCustomValidationCallback = (request, cert, chain, errors) => true });

        /// <summary>
        /// 将JSON字符串转换为List<ListAttInfo>对象
        /// </summary>
        /// <param name="jsonStr"></param>
        /// <returns></returns>
        public static List<ListAttInfo> ConvertToAttInfoList(string jsonStr)
        {
            try
            {              
                var resolver = new CustomListAttContractResolver();

                var settings = new JsonSerializerSettings
                {
                    // 使用自定义的ContractResolver实例
                    ContractResolver = resolver,
                    // 处理日期格式（适配JSON中的"2026-02-11 09:11:48"格式）
                    DateFormatString = "yyyy-MM-dd HH:mm:ss",
                    // 忽略JSON中可能存在的多余字段（避免反序列化报错）
                    MissingMemberHandling = MissingMemberHandling.Ignore,
                    // 允许空值转换到可空类型（如DateTime?）
                    NullValueHandling = NullValueHandling.Ignore
                };

                // 反序列化JSON到List<ListAttInfo>
                var attInfoList = JsonConvert.DeserializeObject<List<ListAttInfo>>(jsonStr, settings);

                // 确保返回非空列表
                return attInfoList ?? new List<ListAttInfo>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"转换失败：{ex.Message}");
                return new List<ListAttInfo>();
            }
        }

        /// <summary>
        /// 自定义ContractResolver，继承DefaultContractResolver并重写ResolveContract方法
        /// </summary>
        private class CustomListAttContractResolver : DefaultContractResolver
        {
            public override JsonContract ResolveContract(Type type)
            {
                // 1. 调用基类获取默认契约
                var baseContract = base.ResolveContract(type);

                // 2. 仅处理ListAttInfo类型，且仅当契约是实体类契约（JsonObjectContract）时才处理
                if (type == typeof(ListAttInfo) && baseContract is JsonObjectContract objectContract)
                {
                    // 3. 安全访问Properties（仅JsonObjectContract才有此属性）
                    // 检查字段是否存在，避免空引用异常
                    if (objectContract.Properties.TryGetValue("DataId", out var dataIdProperty))
                    {
                        // JSON的id → 实体的DataId
                        dataIdProperty.PropertyName = "id";
                    }
                    // 返回转换后的实体契约
                    return objectContract;
                }
                // 非目标类型则返回原契约
                return baseContract;
            }
        }

        /// <summary>
        /// 调用POST接口(ListAttController.SaveListAtt)
        /// </summary>
        /// <param name="apiUrl"></param>
        /// <param name="listAttInfos"></param>
        /// <returns></returns>
        private async Task<PostResData> CallPostApiAsync(string apiUrl, List<ListAttInfo> listAttInfos)
        {           
            //未发布时本地调试
            if (apiUrl.ToLower().Contains("localhost".ToLower())) { apiUrl = "http://localhost:8080/api/dataportal/invoke"; }

            try
            {
                var requestModel = new ApiRequestModel
                {
                    ApiType = typeof(ListAttController).Name,
                    Method = nameof(SaveListAtt),
                    Context = new ApiContext
                    {
                        InvOrgId = 1,
                        Language = "zh-CN"
                    },
                    Parameters = new List<ParameterItem>
                    {
                        new ParameterItem
                        {
                            Value = listAttInfos
                        }
                    }
                };

                string jsonBody = JsonConvert.SerializeObject(requestModel, Formatting.None);
                var content = new StringContent(jsonBody, System.Text.Encoding.UTF8, "application/json");

               
                HttpResponseMessage response = await _httpClient.PostAsync(apiUrl, content);
                response.EnsureSuccessStatusCode();               
                string responseText = await response.Content.ReadAsStringAsync();
                JObject responseJson = JObject.Parse(responseText);
                PostResData resultData = responseJson["Result"]?.ToObject<PostResData>() ?? new PostResData();
                return resultData;

            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"接口请求失败：{ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"内部异常：{ex.InnerException.Message}");
                }
                return new PostResData();
            }
            catch (JsonSerializationException ex)
            {
                Console.WriteLine($"参数序列化失败：{ex.Message}");
                return new PostResData();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"未知错误：{ex.Message}");
                return new PostResData();
            }
        }



    }
}

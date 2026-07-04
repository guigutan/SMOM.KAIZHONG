using Newtonsoft.Json;
using SIE.Api;
using SIE.Barcodes.Printables;
using SIE.Barcodes.WipBatchs;
using SIE.Common.Configs;
using SIE.Common.NumberRules;
using SIE.Common.Prints;
using SIE.Core.Common;
using SIE.Core.Prints;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.KZ.Print.Common;
using SIE.Logging;
using SIE.MES.LineAndon;
using SIE.MES.TaskManagement.Configs;
using SIE.MES.TaskManagement.Dispatchs;
using SIE.Rbac.InvOrgs;
using SIE.Security;

namespace SIE.KZ.Print
{
    /// <summary>
    /// 打印控制器
    /// </summary>
    public class PrinterController : DomainController, IPrint
    {
        private ILog log = Logging.LogManager.GetLogger("print_logger");
        /// <summary>
        /// 获取打印机
        /// </summary>      
        /// <returns>获取打印机</returns>
        [ApiService("获取打印机")]
        [return: ApiReturn("返回打印机集合：List<string>")]
        public virtual List<string> GetPrinters()
        {
            List<string> str = new List<string>();
            foreach (string printerName in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
            {
                str.Add(printerName);
            }
            return str;
            //return GetPrintsWithDrivername("");
        }

        /// <summary>
        /// 获取标签打印模板
        /// </summary>      
        /// <returns>获取标签打印模板</returns>
        [ApiService("获取标签打印模板")]
        [return: ApiReturn("返回标签打印模板集合：Dictionary<string,double>")]
        public virtual Dictionary<double, string> GetLabelTemplates()
        {
            Dictionary<double, string> templates = new Dictionary<double, string>();

            var query = Query<PrintTemplate>().Where(p => p.State == State.Enable && p.PrintType == PrintType.Label);
            var qList = query.ToList();
            qList.ForEach(q =>
            {
                templates.Add(q.Id, q.FileName);
            });

            return templates;
        }

        /// <summary>
        /// Code 1:WMI搜索示例
        /// <summary>
        /// <param name="strDrivername">驱动名称</param>
        /// <returns>返回找到的打印机列表</returns>
        /// <remarks>strDrivername支持”%“以及”_“通配符查询，类似于SQL语句中的查询<remarks>
        public virtual List<string> GetPrintsWithDrivername(string strDrivername)
        {
            List<string> scPrinters = new List<string>();
            string strcheck = "";
            if (strDrivername != "" && strDrivername != "*")
                strcheck = " where DriverName like \'" + strDrivername + "\'";
            string searchQuery = "SELECT Name FROM Win32_Printer" + strcheck;

            return scPrinters;
        }

        /// <summary>
        /// 标签打印
        /// </summary>
        /// <param name="data"></param>
        [ApiService("标签打印(分布集群API打印)")]
        //[AllowAnonymous]
        public virtual void PrintLabels(WipBatchData data)
        {
            if (data == null)
                throw new ValidationException("提交数据异常".FormatArgs());
            PrintTemplate template;
            // 1.根据配置项得到对应模板
            if (data.PrintTemplateId > 0)
            {
                var templateId = data.PrintTemplateId;
                template = RF.GetById<PrintTemplate>(templateId);
                if (template == null)
                    throw new ValidationException("打印模板对应ID[{0}]不存在，请检查".FormatArgs(templateId));
            }
            else
            {
                template = GetConfigPrintTemplate(data);
            }

            if (data.PrinterName.IsNullOrEmpty() && data.ResourceCode.IsNotEmpty())
            {
                //根据产线与安灯区域获取打印机
                var andonLine = RT.Service.Resolve<AndonLineController>().GetAndonLineByMachineCode(data.ResourceCode);
                if (andonLine != null)
                    data.PrinterName = andonLine.PrinterIp;
            }
            if (data.PrinterName.IsNullOrEmpty())
                throw new ValidationException("资源[{0}]不存在或未配置打印机IP".L10nFormat(data.ResourceCode));


            //打印日志
            if (data.PrintLogId == 0)
            {
                var log = new PrintLog()
                {
                    InvOrgId = RT.InvOrg,
                    DeviceCode = data.DeviceCode,
                    DeviceName = data.DeviceName,
                    PrinterName = data.PrinterName,
                    PrintTemplateId = template.Id,
                    PrintType = data.Data?.FirstOrDefault()?.GetType()?.FullName,
                    DataKey = string.Join(",", data.Data?.Select(p => p.BatchNo))
                };
                log.GenerateId();
                data.InvOrgId = RT.InvOrg ?? 0;
                data.PrintLogId = log.Id;
                log.PrintData = JsonConvert.SerializeObject(data);
                RF.Save(log);
            }

            //调用打印服务API
            string apiType = "PrinterController";
            string method = "PrintDatas";
            string url = RT.Config.Get<string>("PrintServer.Url", "http://localhost:8080/api/dataportal/invoke");
            SmomParam[] parameters = new List<SmomParam>(){
                new SmomParam() { Value = data }
            }.ToArray();
            var ret = SmomPost<string>(apiType, method, url, parameters);
        }

        private static object lockPrint = new object();

        /// <summary>
        /// 标签打印
        /// </summary>
        /// <param name="data"></param>
        [ApiService("标签打印")]
        [AllowAnonymous]
        public virtual void PrintDatas(WipBatchData data)
        {
            //lock (lockPrint)
            //{
            try
            {
                var no = data.Data?.FirstOrDefault()?.BatchNo;
                if (data.InvOrgId > 0 && RT.InvOrg != data.InvOrgId)
                    RT.InvOrg = data.InvOrgId;

                PrintTemplate template;
                // 1.根据配置项得到对应模板
                if (data.PrintTemplateId > 0)
                {
                    var templateId = data.PrintTemplateId;
                    template = RF.GetById<PrintTemplate>(templateId);
                }
                else
                {
                    template = GetConfigPrintTemplate(data);
                }
                //打印数据
                var labelNos = data.Data?.Select(p => p.BatchNo).ToList();
                var labels = RT.Service.Resolve<WipBatchController>().GetWipBatches(labelNos).OrderBy(p => p.BatchNo).ToList();

                // 2.根据类型获取报表类型
                var report = HostReportFactory.Current.GetReportByExtension(template.Type);
                // 3.要打印的数据对象实例
                var printable = new WipBatchPrintable();
                // 4.根据配置获取的打印机名称
                //var printName = "Adobe PDF";
                // 5.调用直接打印api
                //log.Info("print start {0} [{1}]".FormatArgs(data.PrinterName, no));

                labels.ForEach(f =>
                {
                    f.PrintProcessCode = data.Data?.FirstOrDefault(p => p.BatchNo == f.BatchNo)?.ProcessCode;
                });

                report.Print(printable, template.Content, data.PrinterName, () =>
                {
                    return labels;
                }, () =>
                {
                    //保存打印日志
                    if (data.PrintLogId > 0)
                        UpdatePrintLogState(data.PrintLogId, "OK");
                });

                //log.Info("print completed {0} [{1}]".FormatArgs(data.PrinterName, no));
            }
            catch (Exception ex)
            {
                //保存打印日志
                if (data.PrintLogId > 0)
                    UpdatePrintLogState(data.PrintLogId, "NG", ex.Message);
                else
                    throw;
            }

            //}
        }

        /// <summary>
        /// Api打印测试
        /// </summary>
        /// <param name="uuid"></param>
        /// <param name="printerName"></param>
        /// <param name="barcode"></param>
        [ApiService("Api打印测试")]
        //[AllowAnonymous]
        public virtual void PrintTest(string uuid, string printerName, string barcode)
        {

            // 1.根据配置项得到对应模板
            var config = RT.Service.Resolve<DispatchController>().GetDispatchConfig();
            var template = config?.GoodLabelTemplate;
            //var template = RT.Service.Resolve<BarcodeController>().GetDefaultTemplate();
            if (template == null)
                throw new ValidationException("未配置默认打印模板，请在派工管理-配置-维护良品标签打印模板".L10N());
            // 1.1模板验证规则
            //if (template.State == State.Disable)
            //    throw new ValidationException("模板已被禁用，请选择启用的模板".L10N());
            // 2.根据类型获取报表类型
            var report = HostReportFactory.Current.GetReportByExtension(template.Type);
            // 3.要打印的数据对象实例
            var printable = new BarcodePrintable();
            // 4.根据配置获取的打印机名称
            //var printName = "Adobe PDF";
            // 5.调用直接打印api
            report.Print(printable, template.Content, printerName, () =>
            {
                return new List<WipBatch>() {
                    new WipBatch() { BatchNo = barcode}
                };
            });
        }

        /// <summary>
        /// 补打
        /// </summary>
        /// <param name="logId"></param>
        public void RePrintData(double logId)
        {
            var log = RF.GetById<PrintLog>(logId);
            if (log == null)
                throw new ValidationException("日志Id[{0}]不存在".FormatArgs(logId));
            var data = JsonConvert.DeserializeObject<WipBatchData>(log.PrintData);
            if (data == null)
                throw new ValidationException("数据反序列化失败".FormatArgs());

            data.InvOrgId = log.InvOrgId ?? 0;
            data.PrintLogId = logId;
            data.PrintTemplateId = log.PrintTemplateId ?? 0;
            PrintLabels(data);
        }

        public static T SmomPost<T>(string apiType, string method, string url, params SmomParam[] parameters)
        {
            try
            {
                SmomRequestData requestData = new SmomRequestData();
                requestData.ApiType = apiType;
                requestData.Method = method;
                requestData.Context = new SmomContext();
                requestData.Parameters = parameters;
                var requestJson = JsonConvert.SerializeObject(requestData);
                var responseJson = HttpClientHelper.HttpPost(url, requestJson);
                var segpResponseData = JsonConvert.DeserializeObject<SegpResponseData<T>>(responseJson);
                if (segpResponseData?.Success != true)
                    throw new ValidationException("打印异常：{0}".L10nFormat(segpResponseData?.Message));
                return segpResponseData.Result;
            }
            catch (Exception ex)
            {
                throw new ValidationException("请求异常：{0}".L10nFormat(ex.Message));
            }

        }

        /// <summary>
        /// 获取配置模板
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        public virtual PrintTemplate GetConfigPrintTemplate(WipBatchData data)
        {
            PrintTemplate template = null;

            //优先使用工序配置模板
            var templateId = data.Data?.FirstOrDefault()?.PrintTemplateId ?? 0;
            if (templateId > 0)
            {
                template = RF.GetById<PrintTemplate>(templateId);
                return template;
            }
            var invOrg = RT.Service.Resolve<InvOrgController>().GetByCode(RT.InvOrg ?? 0);
            var config = RT.Service.Resolve<DispatchController>().GetDispatchConfig();
            var wipBatch = RT.Service.Resolve<WipBatchController>().GetWipBatch(data.Data?.FirstOrDefault()?.BatchNo);//RF.GetById<WipBatch>(data.Data.FirstOrDefault().BatchNo);
            if (wipBatch == null)
                throw new ValidationException("当前组织[{0}]不存在该标签[{1}]".L10nFormat(invOrg?.ExternalId, data.Data?.FirstOrDefault()?.BatchNo));

            if (wipBatch.DispatchTaskId != null)
            {
                var task = RT.Service.Resolve<DispatchController>().GetDispatchTask(wipBatch.DispatchTaskId.Value);
                if (task != null)
                {
                    //优先使用工序模板
                    if (task.Process != null && task.Process.PrintTemplate != null)
                        template = task.Process.PrintTemplate;
                    else
                    {
                        //根据产品的物料类型，找到配置项
                        TypeConfigValue configValue = Query<TypeConfigValue>().Where(p => p.Type == task.Product.Mtart).FirstOrDefault();
                        ItemTypeConfigValue itemTypeConfig = ConfigService.GetConfig(new ItemTypeConfig(), typeof(DispatchTask), configValue);
                        if (itemTypeConfig != null && itemTypeConfig.ProcessPrintTemplate != null)
                        {
                            template = itemTypeConfig.ProcessPrintTemplate;
                        }
                    }
                }
            }

            if (template == null)
            {
                //2830工厂的时候，用良品标签和可疑品标签,2810的时候，用绕包和非绕包标签模板
                if (invOrg.ExternalId != "2810")
                {

                    if (data.Data?.FirstOrDefault()?.IsSuspectProduct == true)
                    {
                        template = config.SuspectLabelTemplate;
                        if (template == null)
                            throw new ValidationException("未配置可疑品标签打印模板，请先维护【派工管理】配置项中【可疑品标签】模板".L10N());
                    }
                    else
                    {
                        template = config.GoodLabelTemplate;
                        if (template == null)
                            throw new ValidationException("未配置良品标签打印模板，请先维护【派工管理】配置项中【良品标签】模板".L10N());
                    }
                }
                else
                {
                    if (wipBatch == null)
                        throw new ValidationException("标签号{0}不存在!".L10nFormat(data.Data?.FirstOrDefault()?.BatchNo));
                    if (wipBatch.WorkOrder.Product.Zmc.Contains("绕包"))
                    {
                        template = config.EntanglePrintTemplate;
                        if (template == null)
                            throw new ValidationException("未配置绕包标签打印模板，请先维护【派工管理】配置项中【绕包标签】模板".L10N());
                    }
                    else
                    {
                        template = config.UnEntanglePrintTemplate;
                        if (template == null)
                            throw new ValidationException("未配置非绕包标签打印模板，请先维护【派工管理】配置项中【非绕包标签】模板".L10N());
                    }
                }
            }

            return template;

        }

        /// <summary>
        /// 更新打印日志状态
        /// </summary>
        /// <param name="printLogId"></param>
        /// <param name="state"></param>
        /// <param name="remark"></param>
        public virtual void UpdatePrintLogState(double printLogId, string state, string? remark = null)
        {
            DB.Update<PrintLog>().Set(p => p.PrintState, state).Set(p => p.Remark, remark).Where(p => p.Id == printLogId).Execute();
        }
    }

}

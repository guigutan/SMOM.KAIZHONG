using MailKit.Search;
using NPOI.SS.Formula.Functions;
using NPOI.XSSF.Streaming.Values;
using SIE.Api;
using SIE.Common;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Items;
using SIE.Items.ProductFamilys;
using SIE.Tech.Processs;
using SIE.Tech.Routings.ApiModels;
using SIE.Tech.Routings.ImportRoutings;
using SIE.Tech.Routings.Technologys;
using SIE.Tech.Routings.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace SIE.Tech.Routings
{
    /// <summary>
    /// 工艺路线控制器.API接口
    /// </summary>
    public partial class RoutingController : DomainController
    {
        /// <summary>
        /// 工序列表
        /// </summary>
        private readonly List<ProcessInfo> _processList = new List<ProcessInfo>();


        /// <summary>
        /// 结果集合"结果"-"ResultTypeForDesign"
        /// </summary>
        private readonly Dictionary<string, ResultTypeForDesign> _resultDic = new Dictionary<string, ResultTypeForDesign>
        {
            { "通过".L10N(), ResultTypeForDesign.Pass },
            { "失败".L10N(), ResultTypeForDesign.Fail },
            { "任意".L10N(), ResultTypeForDesign.Any },
            { "自定义".L10N(), ResultTypeForDesign.Custom }
        };

        /// <summary>
        /// 产品族分类"产品族名称"-"Id"
        /// </summary>
        private readonly Dictionary<string, double> _categoryDic = new Dictionary<string, double>();

        /// <summary>
        /// 工艺路线字典，工艺路线名称-工艺路线ID
        /// 值为空代表新工艺路线
        /// </summary>
        private readonly Dictionary<string, double?> _routingDic = new Dictionary<string, double?>();

        /// <summary>
        /// 验证分类
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private double GetCategory(string value)
        {
            if (!_categoryDic.ContainsKey(value))
            {
                var cate = RT.Service.Resolve<ProductFamilyController>().GetProductFamilyCateByName(value);
                if (cate != null)
                    _categoryDic.Add(value, cate.Id);
                else
                    throw new ValidationException("[{0}]不存在".L10nFormat(value));
            }

            return _categoryDic[value];
        }

        /// <summary>
        ///获取工艺路线
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private double? GetRouting(string value)
        {
            if (!_routingDic.ContainsKey(value))
            {
                var routing = RT.Service.Resolve<RoutingController>().GetRoutingByName(value);
                if (routing != null)
                    _routingDic.Add(value, routing.Id);
                else
                    _routingDic.Add(value, null);
            }

            return _routingDic[value];
        }

        /// <summary>
        /// 创建工艺路线
        /// </summary>
        /// <param name="routingInterfaceModels">工艺路线接口数据</param>
        public virtual void CreateProcessRouting(List<RoutingInterfaceModel> routingInterfaceModels)
        {
            foreach (var routingInterfaceModel in routingInterfaceModels)
            {
                RoutingImportSaveViewModel routingViewModel = new RoutingImportSaveViewModel();
                routingViewModel.RowNum = routingInterfaceModel.routingSummaries.RowNum;
                routingViewModel.Category = routingInterfaceModel.routingSummaries.Category;
                routingViewModel.RoutingName = routingInterfaceModel.routingSummaries.RoutingName;
                routingViewModel.RoutingDesc = routingInterfaceModel.routingSummaries.RoutingDesc;
                routingViewModel.CategoryId = routingInterfaceModel.routingSummaries.CategoryId;
                routingViewModel.RoutingId = routingInterfaceModel.routingSummaries.RoutingId;
                routingViewModel.IsPass = routingInterfaceModel.routingSummaries.IsPass;
                foreach (var processInfo in routingInterfaceModel.routingProcessDetais)
                {
                    ProcessViewModel process = new ProcessViewModel();
                    process.IsBatch = processInfo.IsBatch;
                    process.ProcessId = processInfo.ProcessId;
                    process.ProcessType = processInfo.ProcessType;
                    process.ProcessName = processInfo.ProcessName;
                    process.SortOrder = processInfo.SortOrder;
                    process.SortOrderBack = processInfo.SortOrderBack;
                    process.Result = processInfo.Result;
                    process.ParameterId = processInfo.ParameterId;
                    process.ResultDesc = processInfo.ResultDesc;
                    process.Script = processInfo.Script;
                    process.CanChoose = processInfo.CanChoose;
                    process.IsRepeat = processInfo.IsRepeat;
                    process.IsCreateSku = processInfo.IsCreateSku;
                    process.IsGenerateTask = processInfo.IsGenerateTask;
                    process.IsRequirementTask = processInfo.IsRequirementTask;
                    routingViewModel.ProcessDetailModelList.Add(process);
                }
                ImportRouting(routingViewModel);
            }
            
        }



        /// <summary>
        /// 导入工艺路线
        /// </summary>
        /// <param name="processRoutingInterFaceParamas"></param>
        /// <exception cref="ValidationException"></exception>
        [ApiService("工艺路线对外导入接口")]
        public virtual void ImportProcessRouting([ApiParameter("工艺路线接口参数")] List<ProcessRoutingInterFaceParamas> processRoutingInterFaceParamas)
        {
            if (!processRoutingInterFaceParamas.Any()) { throw new ValidationException("参数列表为空！调用失败".L10N()); }
            RoutingInterfaceModel routingInterfaceModel = null;

            List<RoutingInterfaceModel> processRoutings = new List<RoutingInterfaceModel>();
            int i = 0;
            bool isBatch = false;
            foreach (var processRoutingParama in processRoutingInterFaceParamas)
            {
                if (processRoutingParama.ProcessName.IsNullOrEmpty())
                {
                    throw new ValidationException("工序名称不能为空！".L10N());
                }

                if (routingInterfaceModel != null && !processRoutingParama.Category.IsNullOrEmpty() &&!processRoutingParama.RoutingName.IsNullOrEmpty())//上一个已经接收
                {
                    processRoutings.Add(routingInterfaceModel);
                }
                var processInfo = GetValidateProcess(processRoutingParama.ProcessName);
                if (!processRoutingParama.Category.IsNullOrEmpty() && !processRoutingParama.RoutingName.IsNullOrEmpty())
                {
                    

                    routingInterfaceModel = new RoutingInterfaceModel();
                    routingInterfaceModel.routingSummaries = new RoutingSummaries();
                    routingInterfaceModel.routingSummaries.RowNum = i;
                    routingInterfaceModel.routingSummaries.Category = processRoutingParama.Category;
                    routingInterfaceModel.routingSummaries.RoutingName = processRoutingParama.RoutingName;
                    routingInterfaceModel.routingSummaries.RoutingDesc = processRoutingParama.RoutDesc;
                    routingInterfaceModel.routingSummaries.IsPass = true;
                    routingInterfaceModel.routingSummaries.CategoryId = GetCategory(processRoutingParama.Category);
                    routingInterfaceModel.routingSummaries.RoutingId = GetRouting(processRoutingParama.RoutingName);
                    if (processInfo == null)
                    {
                        routingInterfaceModel.routingSummaries.IsPass = false;
                        continue;
                    }

                    isBatch = processInfo.IsBatch;
                    if (processInfo.IsBatch != isBatch)
                        throw new ValidationException("{0}工艺路线只能添加{0}工序".L10nFormat(isBatch ? "批次".L10N() : "单体".L10N()));
                    if (string.IsNullOrEmpty(processRoutingParama.Category))
                        throw new ValidationException("产品族分类不能为空".L10N());
                    if (string.IsNullOrEmpty(processRoutingParama.RoutingName))
                        throw new ValidationException("工艺路线名称不能为空".L10N());
                }
                RoutingProcessDetai process = new RoutingProcessDetai();

                process.IsBatch = processInfo.IsBatch;
                process.ProcessId = processInfo.Id;
                process.ProcessType = processInfo.Type;
                process.ProcessName = processRoutingParama.ProcessName;
                process.SortOrder = int.Parse(processRoutingParama.SortOrder);
                process.SortOrderBack = int.Parse(processRoutingParama.SortOrderBack);
                var resultInfo = GetProessResult(processRoutingParama.Result, processInfo, processRoutingParama.ResultDesc);
                process.Result = resultInfo.Result;
                process.ParameterId = resultInfo.Id;
                process.ResultDesc = resultInfo.Describe;
                process.Script = resultInfo.Script;
                process.CanChoose = YesNoToBool(processRoutingParama.Choose);
                process.IsRepeat = YesNoToBool(processRoutingParama.Repeat);
                process.IsCreateSku = YesNoToBool(processRoutingParama.Sku);
                process.IsGenerateTask = YesNoToBool(processRoutingParama.GenerateTask);
                process.IsRequirementTask=YesNoToBool(processRoutingParama.IsRequirementTask);
                if (routingInterfaceModel == null)
                {
                    throw new ValidationException("工艺路线信息不完整".L10N());
                }

                routingInterfaceModel.routingProcessDetais.Add(process);

                if (i == processRoutingInterFaceParamas.Count-1)//最后一行判断为结束 接收模型
                {
                    processRoutings.Add(routingInterfaceModel);
                }
                i++;
            }
            this.CreateProcessRouting(processRoutings);

        }
        /// <summary>
        /// 获取工序参数信息
        /// </summary>
        /// <param name="key">工序结果</param>
        /// <param name="info">工序信息</param>
        /// <param name="desc">结果描述，自定义类型根据描述取</param>
        /// <returns>工序参数信息</returns>
        private ParamterInfo GetProessResult(string key, ProcessInfo info, string desc)
        {
            ResultTypeForDesign result;
            _resultDic.TryGetValue(key, out result);
            ParamterInfo resultInfo;
            if (result == ResultTypeForDesign.Custom)
            {
                if (desc.IsNullOrEmpty())
                    throw new ValidationException("工序结果为[{0}]，结果描述不能为空".L10nFormat(ResultTypeForDesign.Custom.ToLabel()));
                resultInfo = info.ParamterList.FirstOrDefault(p => p.Result == result && p.Describe == desc);
            }
            else
                resultInfo = info.ParamterList.FirstOrDefault(p => p.Result == result);
            if (resultInfo == null)
                throw new ValidationException("工序[{0}]不存在结果描述为[{1}]的采集步骤".L10nFormat(info.ProcessName, result == ResultTypeForDesign.Custom ? desc : result.ToLabel()));
            return resultInfo;
        }


        /// <summary>
        /// 是否批次的工序
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private bool IsBatchProcess(ProcessType type)
        {
            return type == ProcessType.BatchAssembly || type == ProcessType.BatchFix || type == ProcessType.BatchPacking || type == ProcessType.BatchPqc;
        }

        /// <summary>
        /// 获取缓存的数据
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private ProcessInfo GetProcess(string name)
        {
            return _processList.FirstOrDefault(p => p.ProcessName == name);
        }

        /// <summary>
        /// 获取验证工序
        /// </summary>
        /// <param name="processName"></param>
        /// <returns></returns>
        private ProcessInfo GetValidateProcess(string processName)
        {
            var processInfo = GetProcess(processName);
            if (processInfo == null)
            {
                var process = RT.Service.Resolve<ProcessController>().GetProcess(processName);
                if (process != null)
                {
                    var info = new ProcessInfo() { Id = process.Id, ProcessName = processName, IsBatch = IsBatchProcess(process.Type.Value), Type = process.Type.Value };
                    process.ParameterList.ForEach(e => info.ParamterList.Add(new ParamterInfo() { Id = e.Id, Result = e.Type, Describe = e.Description, Script = e.Script }));
                    _processList.Add(info);
                    processInfo = info;
                }
            }
            return processInfo;
        }

        /// <summary>
        /// 是否转换布尔
        /// </summary>
        /// <param name="yesNo">是否</param>
        /// <returns>布尔值</returns>
        bool? YesNoToBool(string yesNo)
        {
            bool? result = null;
            if (!yesNo.IsNullOrEmpty())
                result = yesNo == "是";
            return result;
        }

    }
    /// <summary>
    /// 工序信息
    /// </summary>
    [Serializable]
    internal class ProcessInfo
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ProcessInfo()
        {
            ParamterList = new List<ParamterInfo>();
        }

        /// <summary>
        /// 工序ID
        /// </summary>
        public double Id { get; set; }

        /// <summary>
        /// 工序名称
        /// </summary>
        public string ProcessName { get; set; }

        /// <summary>
        /// 是否批次工序
        /// </summary>
        public bool IsBatch { get; set; }

        /// <summary>
        /// 工序类型
        /// </summary>
        public ProcessType Type { get; set; }

        /// <summary>
        /// 工序结果集合
        /// </summary>
        public List<ParamterInfo> ParamterList { get; set; }
    }
}
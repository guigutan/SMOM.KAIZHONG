using SIE.Common.ImportHelper;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Items.ProductFamilys;
using SIE.Tech.Processs;
using SIE.Tech.Routings.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace SIE.Tech.Routings.ImportRoutings
{
    /// <summary>
    /// 导入工单 处理逻辑类
    /// </summary>
    [Services.Service(FallbackType = typeof(ImportRoutingHandle), ServiceLifeStyle = Services.ServiceLifeStyle.Transient)]
    public class ImportRoutingHandle : IDisposable, IBusinessImport
    {
        /// <summary>
        /// 导入列名列表
        /// </summary>
        private List<string> columnNameList = new List<string> { "*产品族分类", "*工艺路线名称", "工艺路线描述", "*工序名称", "*序列", "*返回序列", "*结果(通过,失败,任意,自定义)", "结果描述", "是否可选", "是否重复", "是否创建SKU", "是否生成工序任务" };

        #region 私有属性
        /// <summary>
        /// 导入成功列
        /// </summary>
        public static readonly string ImportSuccess = "_importSuccess";

        /// <summary>
        /// 产品族分类"产品族名称"-"Id"
        /// </summary>
        private readonly Dictionary<string, double> _categoryDic = new Dictionary<string, double>();

        /// <summary>
        /// 工序列表
        /// </summary>
        private readonly List<ProcessInfo> _processList = new List<ProcessInfo>();

        /// <summary>
        /// 结果集合"结果"-"ResultTypeForDesign"
        /// </summary>
        private readonly Dictionary<string, ResultTypeForDesign> _resultDic = new Dictionary<string, ResultTypeForDesign>
        {
            { "通过", ResultTypeForDesign.Pass },
            { "失败", ResultTypeForDesign.Fail },
            { "任意", ResultTypeForDesign.Any },
            { "自定义", ResultTypeForDesign.Custom }
        };

        /// <summary>
        /// 工艺路线字典，工艺路线名称-工艺路线ID
        /// 值为空代表新工艺路线
        /// </summary>
        private readonly Dictionary<string, double?> _routingDic = new Dictionary<string, double?>();

        #endregion

        /// <summary>
        /// 导入模板的列头名
        /// </summary>
        public List<string> ColumnNameList
        {
            get
            {
                return columnNameList;
            }

            set
            {
                columnNameList = value;
            }
        }

        /// <summary>
        /// 列的标准验证 (列名 列对应验证 )
        /// </summary>
        public Dictionary<string, ValidColumn> ColumnValidList { get; set; }

        /// <summary>
        /// 创建列
        /// </summary>
        /// <returns>IBusinessImport</returns>
        public IBusinessImport CreaetColumnValid()
        {
            this.ColumnValidList = new Dictionary<string, ValidColumn>
            {
                { "*产品族分类", new ValidColumn(ImportDataType._String, false, ValidateCategory) },
                { "*工艺路线名称", new ValidColumn(ImportDataType._String, false, ValidateRouting) },
                { "工艺路线描述", new ValidColumn(ImportDataType._String, false, 240) },
                { "*工序名称", new ValidColumn(ImportDataType._String, true, ValidateProcess) },
                { "*序列", new ValidColumn(ImportDataType._Int, true, ValidateSeq, true) },
                { "*返回序列", new ValidColumn(ImportDataType._Int, true, ValidateBackSeq, true) },
                { "*结果(通过,失败,任意,自定义)", new ValidColumn(ImportDataType._String, true, ValidateResult) },
                { "结果描述", new ValidColumn(ImportDataType._String, false, true) },
                { "是否可选", new ValidColumn(ImportDataType._String, false, ValidateYesNo) },
                { "是否重复", new ValidColumn(ImportDataType._String, false, ValidateYesNo) },
                { "是否创建SKU", new ValidColumn(ImportDataType._String, false, ValidateYesNo) },
                { "是否生成工序任务", new ValidColumn(ImportDataType._String, false, ValidateYesNo) }
            };
            return this;
        }

        /// <summary>
        /// 释放数据
        /// </summary>
        public void Dispose()
        {
            _categoryDic.Clear();
            _resultDic.Clear();
        }

        /// <summary>
        /// 处理业务数据
        /// </summary>
        /// <param name="drs">需要处理的数据集</param>
        public void ProcessBusinessDataHandle(DataRow[] drs)
        {
            if (drs == null || !drs.Any()) return;
            DataRow[] allRows = drs[0].Table.AsEnumerable().ToArray();
            RoutingImportSaveViewModel routing = new RoutingImportSaveViewModel();
            bool isBatch = false;
            for (int i = 0; i < allRows.Length; i++)
            {
                try
                {
                    DataRow row = allRows[i];
                    var category = row.Field<string>(ColIndex("*产品族分类"));
                    var routingName = row.Field<string>(ColIndex("*工艺路线名称"));
                    var routDesc = row.Field<string>(ColIndex("工艺路线描述"));
                    var processName = row.Field<string>(ColIndex("*工序名称"));
                    var sortOrder = row.Field<string>(ColIndex("*序列"));
                    var sortOrderBack = row.Field<string>(ColIndex("*返回序列"));
                    var result = row.Field<string>(ColIndex("*结果(通过,失败,任意,自定义)"));
                    var resultDesc = row.Field<string>(ColIndex("结果描述"));
                    var choose = row.Field<string>(ColIndex("是否可选"));
                    var repeat = row.Field<string>(ColIndex("是否重复"));
                    var sku = row.Field<string>(ColIndex("是否创建SKU"));
                    var generateTask = row.Field<string>(ColIndex("是否生成工序任务"));
                   var requirementTask = row.Field<string>(ColIndex("是否需求任务清单"));
                    ProcessViewModel process = new ProcessViewModel();
                    var processInfo = GetProcess(processName);
                    if (!category.IsNullOrEmpty() && !routingName.IsNullOrEmpty())
                    {
                        routing = new RoutingImportSaveViewModel();
                        routing.RowNum = i;
                        routing.Category = category;
                        routing.RoutingName = routingName;
                        routing.RoutingDesc = routDesc;
                        routing.IsPass = true;
                        routing.CategoryId = GetCategory(category);
                        routing.RoutingId = GetRouting(routingName);
                        if (processInfo == null)
                        {
                            routing.IsPass = false;
                            continue;
                        }

                        isBatch = processInfo.IsBatch;
                    }

                    if (!row[ImportDataHandle.MessageColumnName].ToString().IsNullOrEmpty())
                    {
                        routing.IsPass = false;
                        continue;
                    }

                    if (processInfo.IsBatch != isBatch)
                        throw new ValidationException("{0}工艺路线只能添加{0}工序".L10nFormat(isBatch ? "批次" : "单体"));
                    if (string.IsNullOrEmpty(routing.Category) && string.IsNullOrEmpty(category))
                        throw new ValidationException("产品族分类不能为空".L10N());
                    if (string.IsNullOrEmpty(routing.RoutingName) && string.IsNullOrEmpty(routingName))
                        throw new ValidationException("工艺路线名称不能为空".L10N());
                    process.IsBatch = processInfo.IsBatch;
                    process.ProcessId = processInfo.Id;
                    process.ProcessType = processInfo.Type;
                    process.ProcessName = processName;
                    process.SortOrder = int.Parse(sortOrder);
                    process.SortOrderBack = int.Parse(sortOrderBack);
                    var resultInfo = GetProessResult(result, processInfo, resultDesc);
                    process.Result = resultInfo.Result;
                    process.ParameterId = resultInfo.Id;
                    process.ResultDesc = resultInfo.Describe;
                    process.Script = resultInfo.Script;
                    process.CanChoose = YesNoToBool(choose);
                    process.IsRepeat = YesNoToBool(repeat);
                    process.IsCreateSku = YesNoToBool(sku);
                    process.IsGenerateTask = YesNoToBool(generateTask);
                    process.IsRequirementTask = YesNoToBool(requirementTask);
                    routing.ProcessDetailModelList.Add(process);
                    DataRow nextRow = i == allRows.Length - 1 ? null : allRows[i + 1];
                    if (nextRow == null || (nextRow != null && !nextRow[0].ToString().IsNullOrEmpty() && !nextRow[1].ToString().IsNullOrEmpty()))
                    {
                        try
                        {
                            if (routing.IsPass)
                            {
                                RT.Service.Resolve<RoutingController>().ImportRouting(routing);
                            }
                        }
                        catch (Exception exc)
                        {
                            routing.IsPass = false;
                            SetRowError(allRows, routing.RowNum, exc);
                        }
                    }
                }
                catch (Exception exc)
                {
                    routing.IsPass = false;
                    SetRowError(allRows, i, exc);
                }
            }
        }

        /// <summary>
        /// 设置错误信息
        /// </summary>
        /// <param name="rows">行集合</param>
        /// <param name="rowNum">错误行号</param>
        /// <param name="exc">异常信息</param>
        private void SetRowError(DataRow[] rows, int rowNum, Exception exc)
        {
            var baseExc = exc.GetBaseException();
            if (baseExc is ValidationException)
                rows[rowNum][ImportDataHandle.MessageColumnName] += (baseExc as ValidationException).Message;
            else
                rows[rowNum][ImportDataHandle.MessageColumnName] += exc.Message;
        }

        #region 获取值
        /// <summary>
        /// 根据产品族分类名称取值
        /// </summary>
        /// <param name="key">产品族名称</param>
        /// <returns>产品族分类Id</returns>
        private double GetCategory(string key)
        {
            if (_categoryDic.ContainsKey(key))
                return _categoryDic[key];
            return 0;
        }

        /// <summary>
        /// 根据工序名称取值
        /// </summary>
        /// <param name="name">工序名称</param>
        /// <returns>工序信息</returns>
        private ProcessInfo GetProcess(string name)
        {
            return _processList.FirstOrDefault(p => p.ProcessName == name);
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

        /// <summary>
        /// 获取工艺路线ID
        /// </summary>
        /// <param name="routing">工艺路线名称</param>
        /// <returns>工艺路线ID</returns>
        double? GetRouting(string routing)
        {
            double? result = null;
            _routingDic.TryGetValue(routing, out result);
            return result;
        }

        /// <summary>
        /// 获取指定列名的索引
        /// </summary>
        /// <param name="columnName">指定列名</param>
        /// <returns>返回对应索引</returns>
        private int ColIndex(string columnName)
        {
            return columnNameList.IndexOf(columnName);
        }
        #endregion

        #region 属性验证
        /// <summary>
        /// 验证产品
        /// </summary>
        /// <param name="obj">产品编码</param>
        /// <param name="messageTip">错误信息</param>
        /// <param name="row">验证数据的当前行对象</param>
        /// <returns>验证通过返回true，不通过返回false</returns>
        private bool ValidateCategory(object obj, out string messageTip, DataRow row)
        {
            string value = obj.ToString();
            messageTip = string.Empty;
            if (ValidateIsNull(ref messageTip, value, "产品族分类"))
                return false;
            if (!_categoryDic.ContainsKey(value))
            {
                var cate = RT.Service.Resolve<ProductFamilyController>().GetProductFamilyCateByName(value);
                if (cate != null)
                    _categoryDic.Add(value, cate.Id);
                else
                    messageTip = "[{0}]不存在".L10nFormat(value);
            }

            return _categoryDic.ContainsKey(value);
        }

        /// <summary>
        /// 验证结果(1:通过2:失败3:任意4:自定义)
        /// </summary>
        /// <param name="obj">结果值</param>
        /// <param name="messageTip">错误消息</param>
        /// <param name="row">当前行</param>
        /// <returns>验证通过返回true，不通过返回false</returns>
        private bool ValidateRouting(object obj, out string messageTip, DataRow row)
        {
            string value = obj.ToString();
            messageTip = string.Empty;
            if (ValidateIsNull(ref messageTip, value, "工艺路线"))
                return false;
            if (!_routingDic.ContainsKey(value))
            {
                var routing = RT.Service.Resolve<RoutingController>().GetRoutingByName(value);
                if (routing != null)
                    _routingDic.Add(value, routing.Id);
                else
                    _routingDic.Add(value, null);
            }

            return _routingDic.ContainsKey(value);
        }

        /// <summary>
        /// 验证产品
        /// </summary>
        /// <param name="obj">产品编码</param>
        /// <param name="messageTip">错误信息</param>
        /// <param name="row">验证数据的当前行对象</param>
        /// <returns>验证通过返回true，不通过返回false</returns>
        private bool ValidateProcess(object obj, out string messageTip, DataRow row)
        {
            bool isValid = true;
            string value = obj.ToString();
            messageTip = string.Empty;
            if (ValidateIsNull(ref messageTip, value, "工序"))
                return false;
            var processInfo = GetProcess(value);
            if (processInfo == null)
            {
                var process = RT.Service.Resolve<ProcessController>().GetProcess(value);
                if (process != null)
                {
                    var info = new ProcessInfo() { Id = process.Id, ProcessName = value, IsBatch = IsBatchProcess(process.Type.Value), Type = process.Type.Value };
                    process.ParameterList.ForEach(e => info.ParamterList.Add(new ParamterInfo() { Id = e.Id, Result = e.Type, Describe = e.Description, Script = e.Script }));
                    _processList.Add(info);
                }
                else
                {
                    messageTip = "[{0}]不存在".L10nFormat(value);
                    isValid = false;
                }
            }

            return isValid;
        }

        /// <summary>
        /// 验证结果(1:通过2:失败3:任意4:自定义)
        /// </summary>
        /// <param name="obj">结果值</param>
        /// <param name="messageTip">错误消息</param>
        /// <param name="row">当前行</param>
        /// <returns>验证通过返回true，不通过返回false</returns>
        private bool ValidateResult(object obj, out string messageTip, DataRow row)
        {
            bool isValid = true;
            string value = obj.ToString();
            messageTip = string.Empty;
            if (ValidateIsNull(ref messageTip, value, "结果"))
                return false;
            if (!_resultDic.ContainsKey(value))
            {
                messageTip = "不能设置为其它值".L10N();
                isValid = false;
            }

            return isValid;
        }

        /// <summary>
        /// 验证是否
        /// </summary>
        /// <param name="obj">值</param>
        /// <param name="messageTip">错误信息</param>
        /// <param name="dr">数据行</param>
        /// <returns>验证通过返回true，不通过返回false</returns>
        private bool ValidateYesNo(object obj, out string messageTip, DataRow dr)
        {
            string value = obj.ToString();
            messageTip = string.Empty;
            if (value == "是" || value == "否" || value.IsNullOrEmpty())
                return true;
            messageTip = "只能选择：是、否".L10N();
            return false;
        }

        /// <summary>
        /// 验证下一序号是否合法
        /// </summary>
        /// <param name="obj">下一序号值</param>
        /// <param name="messageTip">错误信息</param>
        /// <param name="dr">数据行</param>
        /// <returns>验证通过返回true，不通过返回false</returns>
        private bool ValidateBackSeq(object obj, out string messageTip, DataRow dr)
        {
            string value = obj.ToString();
            messageTip = string.Empty;
            if (ValidateIsNull(ref messageTip, value, "返回序列"))
                return false;
            return ValidateInt(value, ref messageTip);
        }

        /// <summary>
        /// 验证序号是否合法
        /// </summary>
        /// <param name="obj">序号值</param>
        /// <param name="messageTip">错误信息</param>
        /// <param name="dr">数据行</param>
        /// <returns>验证通过返回true，不通过返回false</returns>
        private bool ValidateSeq(object obj, out string messageTip, DataRow dr)
        {
            string value = obj.ToString();
            messageTip = string.Empty;
            if (ValidateIsNull(ref messageTip, value, "序列"))
                return false;
            return ValidateInt(value, ref messageTip);
        }

        /// <summary>
        /// 验证是否整型
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="messageTip">错误信息</param>
        /// <returns>整型返回true，否则返回false</returns>
        private bool ValidateInt(string value, ref string messageTip)
        {
            int result;
            if (int.TryParse(value, out result))
                return true;
            messageTip = "只能是整型".L10N();
            return false;
        }

        /// <summary>
        /// 验证数据是否为空
        /// </summary>
        /// <param name="str">错误信息</param>
        /// <param name="value">值</param>
        /// <param name="colunmName">字段名称</param>
        /// <returns>是空返回true，否则返回false</returns>
        private bool ValidateIsNull(ref string str, string value, string colunmName)
        {
            if (!value.IsNullOrEmpty())
                return false;
            str = "{0}不能为空".L10nFormat(colunmName);
            return true;
        }

        /// <summary>
        /// 是否批次工序
        /// </summary>
        /// <param name="type">工序类型</param>
        /// <returns>批次工序返回true，非批次返回false</returns>
        bool IsBatchProcess(ProcessType type)
        {
            return type == ProcessType.BatchAssembly || type == ProcessType.BatchFix || type == ProcessType.BatchPacking || type == ProcessType.BatchPqc;
        }
        #endregion
    }

    /// <summary>
    /// 工序信息
    /// </summary>
    [Serializable]
    public class ProcessInfo
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
        /// 工序编码
        /// </summary>
        public string ProcessCode { get; set; }

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

    /// <summary>
    /// 工序结果信息
    /// </summary>
    [Serializable]
    public class ParamterInfo
    {
        /// <summary>
        /// 工序参数ID
        /// </summary>
        public double Id { get; set; }

        /// <summary>
        /// 工序结果
        /// </summary>
        public ResultTypeForDesign Result { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Describe { get; set; }

        /// <summary>
        /// 脚本
        /// </summary>
        public string Script { get; set; }

        public string Condition { get; set; }
    }
}
using SIE.Common.Catalogs;
using SIE.Common.ImportHelper;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Items;
using SIE.MES.WIP.Runtime;
using SIE.Packages.ItemLabels;
using SIE.Resources.ProcessSegments;
using SIE.Tech.Processs;
using SIE.Tech.Routings;
using SIE.Tech.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using static IronPython.Modules._ast;

namespace SIE.MES.Routings.RoutingBoms.ImportBoms
{
    /// <summary>
    /// 工序bom明细导入
    /// </summary>
    [Services.Service(FallbackType = typeof(ImportRoutingBomDetailHandle), ServiceLifeStyle = Services.ServiceLifeStyle.Transient)]
    public class ImportRoutingBomDetailHandle : IDisposable, IBusinessImport
    {
        private const string NOT_EXISTS = "[{0}]不存在";
        private const string ROUTING_VERSION = "工艺路线版本";
        /// <summary>
        /// 导入列名列表
        /// </summary>
        private List<string> columnNameList =
            new List<string> { "产品编码", "工艺路线", ROUTING_VERSION, "工段", "物料编码","物料扩展属性", "工序名称",
                "工步","单位用量", "是否附件",  "备注" };

        #region 私有属性
        /// <summary>
        /// 导入成功列
        /// </summary>
        public static readonly string ImportSuccess = "_importSuccess";

        /// <summary>
        /// 产品编码
        /// </summary>
        private readonly Dictionary<string, double> _productDict = new Dictionary<string, double>();

        /// <summary>
        /// 工艺路线版本列表
        /// </summary>
        private readonly Dictionary<string, double> _versionDict = new Dictionary<string, double>();

        /// <summary>
        /// 工段字典列表
        /// </summary>
        private readonly Dictionary<string, double> _segmentDict = new Dictionary<string, double>();

        /// <summary>
        /// 工艺路线字典，工艺路线名称-工艺路线ID
        /// 值为空代表新工艺路线
        /// </summary>
        private readonly Dictionary<string, double> _routingDict = new Dictionary<string, double>();

        /// <summary>
        /// 物料编码
        /// </summary>
        private readonly Dictionary<string, double> _itemDict = new Dictionary<string, double>();

        /// <summary>
        /// 工序
        /// </summary>
        private readonly Dictionary<string, double> _processDict = new Dictionary<string, double>();

        /// <summary>
        /// 工步
        /// </summary>
        private readonly Dictionary<string, double> _workStepDict = new Dictionary<string, double>();

        /// <summary>
        /// 工序bom主表Id
        /// </summary>
        private readonly Dictionary<string, double> routingBomDictionary = new Dictionary<string, double>();

        /// <summary>
        /// 工序bom
        /// </summary>
        private readonly Dictionary<string, BomDetailViewModel> _bomDetailDict = new Dictionary<string, BomDetailViewModel>();

        /// <summary>
        /// 物料单机用量总和
        /// </summary>
        private readonly Dictionary<string, decimal> _amountDict = new Dictionary<string, decimal>();

        /// <summary>
        /// 工艺路线工序清单
        /// </summary>
        private readonly Dictionary<double, List<RoutingProcess>> routingProcessDictionary
            = new Dictionary<double, List<RoutingProcess>>();
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
                { "产品编码", new ValidColumn(ImportDataType._String, true, ValidateProduct) },
                { "工艺路线", new ValidColumn(ImportDataType._String, true, ValidateRouting) },
                { ROUTING_VERSION, new ValidColumn(ImportDataType._String, true, ValidateVersion) },
                { "工段", new ValidColumn(ImportDataType._String, false, ValidateSegment) },
                { "物料编码", new ValidColumn(ImportDataType._String, true, ValidateMaterialCode) },
                //{ "物料扩展属性", new ValidColumn(ImportDataType._String, true, true) },
                { "工序名称", new ValidColumn(ImportDataType._String, true, ValidateProcess) },
                //{ "工步", new ValidColumn(ImportDataType._String, false, ValidateWorkStep) },
                { "单位用量", new ValidColumn(ImportDataType._String, true, ValidateAmount) },
                { "是否附件", new ValidColumn(ImportDataType._String, false, ValidateIsAttachment) },
                { "备注", new ValidColumn(ImportDataType._String, false, 250) }
            };
            return this;
        }

        /// <summary>
        /// 释放数据
        /// </summary>
        public void Dispose()
        {
            _routingDict.Clear();
            _segmentDict.Clear();
            _productDict.Clear();
            _versionDict.Clear();
        }

        /// <summary>
        /// 处理业务数据
        /// </summary>
        /// <param name="drs">需要处理的数据集</param>
        public void ProcessBusinessDataHandle(DataRow[] drs)
        {
            if (drs == null || !drs.Any())
            {
                return;
            }

            DataRow[] allRows = drs[0].Table.AsEnumerable().ToArray();

            double attId = 0;
            List<RoutingBomDetail> boms = new List<RoutingBomDetail>();
            List<RoutingBomImportRecord> records = new List<RoutingBomImportRecord>();

            var processIds = _processDict.Values.Distinct().ToList();
            var lstWorkStep = RT.Service.Resolve<ProcessController>().GetWorkSteps(processIds);


            List<string> itemCodeList = new List<string>();
            drs.ForEach(row =>
            {
                itemCodeList.Add(row.Field<string>(ColIndex("物料编码")).Trim());
            });
            // 物料
            var itemList = RT.Service.Resolve<ItemLabelController>().GetItemByCode(itemCodeList);
            // 物料扩展属性子表
            var itemExtPropList = RT.Service.Resolve<ItemController>().GetItemPropertyValueList(itemCodeList);
            // 属性定义ids
            var definitionIds = itemExtPropList.Select(x => x.DefinitionId).ToList();
            // 属性列表
            var definitionList = RT.Service.Resolve<ItemController>().GetItemPropertyDefinitionList(definitionIds);
            var catalogTypeIds = definitionList.Where(p => p.PropertyType == ItemPropertyType.Catalog).Select(p => p.CatalogTypeId).ToList();
            // 快码明细
            var catalogList = RT.Service.Resolve<ItemController>().GetCatalogList(catalogTypeIds);


            for (int index = 0; index < drs.Length; index++)
            {
                try
                {
                    DataRow row = drs[index];

                    var productCode = row.Field<string>(ColIndex("产品编码")).Trim();
                    var routingName = row.Field<string>(ColIndex("工艺路线")).Trim();
                    var routingVersion = row.Field<string>(ColIndex(ROUTING_VERSION)).Trim();
                    var segmentName = row.Field<string>(ColIndex("工段")).Trim();
                    var itemCode = row.Field<string>(ColIndex("物料编码")).Trim();
                    var itemExtPropName = row.Field<string>(ColIndex("物料扩展属性")).Trim();
                    var processName = row.Field<string>(ColIndex("工序名称")).Trim();
                    var workStepName = row.Field<string>(ColIndex("工步")).Trim();
                    var amount = row.Field<string>(ColIndex("单位用量")).Trim();
                    var attachment = row.Field<string>(ColIndex("是否附件")).Trim();
                    var desc = row.Field<string>(ColIndex("备注")).Trim();

                    var parentId = row[ImportDataHandle.ParentId];
                    double.TryParse(parentId.ToString(), out attId);

                    RoutingBomDetail bom = new RoutingBomDetail();

                    // 工段
                    double? segmentId = null;
                    if (_segmentDict.ContainsKey(segmentName))
                    {
                        segmentId = _segmentDict[segmentName];
                    }

                    // 取主表ID                        
                    string key = productCode + "," + routingName + "," + routingVersion + "," + segmentId;
                    double routingBomId = routingBomDictionary[key];
                    bom.RoutingBomId = routingBomId;

                    double productId = _productDict[productCode];

                    // 物料编码
                    double itemId = _itemDict[itemCode];
                    var rowItem = itemList.FirstOrDefault(p => p.Id == itemId);
                    bom.MaterialId = itemId;

                    // 工序
                    double processId = _processDict[processName];

                    //工艺路线版本
                    string routingKey = routingName + "," + routingVersion;
                    var routingVersionId = _versionDict[routingKey];

                    if (!routingProcessDictionary.ContainsKey(routingVersionId))
                    {
                        // 校验工艺路线工序和工序bom工序是否一致
                        var routingProcesses = RT.Service.Resolve<RoutingBomController>()
                          .GetRoutingProcesses(routingVersionId);

                        routingProcessDictionary.Add(routingVersionId, routingProcesses.ToList());
                    }

                    CheckValues(lstWorkStep, processName, workStepName, bom, key, processId, routingVersionId);

                    // 单位用量
                    bom.Amount = decimal.Parse(amount);
                    bom.IsAttachment = attachment == "是";
                    bom.Description = desc;
                    bom.AttachmentId = attId;

                    // 取得工单BOM 自动带出主料编码

                    string keyBom = productId.ToString() + "," + itemId + "," + segmentId ?? "";
                    BomDetailViewModel bomDetail = null;
                    if (!_bomDetailDict.TryGetValue(keyBom, out bomDetail)
                        && !string.IsNullOrWhiteSpace(bomDetail.MainMaterialCode))
                    {
                        bom.MainMaterial = RT.Service.Resolve<RoutingBomController>()
                            .GetMainMaterial(productId, itemId, segmentId);
                    }

                    if (rowItem.EnableExtendProperty && itemExtPropName.IsNullOrEmpty())
                    {
                        throw new ValidationException("物料启用扩展属性！".L10N());
                    }
                    else if (rowItem.EnableExtendProperty && itemExtPropName.IsNotEmpty())
                    {
                        string itemExtPropErrorMsg = string.Empty;
                        var itemExtProp = ExtPropIsActive(itemExtPropList, definitionList, catalogList, rowItem, itemExtPropName, out itemExtPropErrorMsg);
                        if (itemExtPropErrorMsg.IsNotEmpty())
                        {
                            throw new ValidationException("{0}".FormatArgs(itemExtPropErrorMsg)); 
                        }
                        else
                        {
                            bom.ItemExtProp = itemExtProp; 
                            bom.ItemExtPropName = itemExtPropName;
                        }
                    }
                    else
                    {
                        //
                    }
                    boms.Add(bom);
                }
                catch (Exception exc)
                {
                    SetRowError(allRows, index, exc);
                }
            }

            //每个主键生成一条导入记录
            foreach (string key in routingBomDictionary.Keys)
            {
                var mainId = routingBomDictionary[key];
                //导入记录
                RoutingBomImportRecord imlRecord = new RoutingBomImportRecord()
                {
                    RoutingBomId = mainId,
                    ImportDate = RF.Find<RoutingBomImportRecord>().GetDbTime(),
                    OperatorId = RT.IdentityId,
                    AttachmentId = attId
                };
                records.Add(imlRecord);
            }

            // 保存
            if (boms.Count > 0)
            {
                RT.Service.Resolve<RoutingBomController>().SaveRoutingBomDetail(boms, records);
            }
        }

        private string ExtPropIsActive(EntityList<ItemPropertyValue> itemExtPropList, EntityList<ItemPropertyDefinition> definitionList, EntityList<Catalog> catalogList, Items.Item item, string itemExtPropName, out string itemExtPropErrorMsg)
        {
            if (item.EnableExtendProperty && itemExtPropName.IsNullOrEmpty())
            {
                itemExtPropErrorMsg = "物料需启用扩展属性;";
                return string.Empty;
            }
            else
            {

                // 获取属性定义Ids
                var itemExtPropDefIds = itemExtPropList.Where(p => p.ItemId == item.Id).Select(p => p.DefinitionId).Distinct().ToList();
                var itemDefinitionList = definitionList.Where(p => itemExtPropDefIds.Contains(p.Id)).ToList();
                // 属性名称
                var definitionNames = itemDefinitionList.Select(p => p.Name).Distinct().ToList();
                // 导入数据的物料扩展属性名称
                var proNames = itemExtPropName.Split(';').ToList();
                if (definitionNames.Count != proNames.Count)
                {
                    itemExtPropErrorMsg = "物料扩展属性不全或多余;";
                    return string.Empty;
                }
                else
                {
                    // 物料拓展属性是否合格
                    var check = true;
                    foreach (var pro in proNames)
                    {
                        string key = string.Empty;
                        string value = string.Empty;
                        try
                        {
                            key = pro.Split(':')[0];
                            value = pro.Split(':')[1];
                        }
                        catch
                        {
                            check = false;
                            break;
                        }
                        var itemDefinition = itemDefinitionList.Find(p => p.Name == key);

                        if (itemDefinition != null)
                        {
                            if (itemDefinition.PropertyType == ItemPropertyType.Catalog)
                            {
                                var itemCata = catalogList.FirstOrDefault(p => p.Code == value && p.CatalogTypeId == itemDefinition.CatalogTypeId);
                                if (itemCata == null)
                                {
                                    check = false;
                                    break;

                                }
                            }
                            else
                            {
                                var itemExtPropValue = itemExtPropList.First(p => p.ItemId == item.Id && p.DefinitionId == itemDefinition.Id && p.Value == value)?.Value;
                                if (value != itemExtPropValue)
                                {
                                    check = false;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            check = false;
                            break;

                        }
                    }
                    if (check)
                    {
                        var exProp = string.Empty;
                        proNames.ForEach(proName =>
                        {
                            var keyId = itemDefinitionList.Find(p => p.Name == proName.Split(':')[0])?.Id.ToString();
                            if (exProp.IsNotEmpty())
                            {
                                exProp += ';';
                            }
                            exProp += keyId + ':' + proName.Split(':')[1];
                        });
                        itemExtPropErrorMsg = string.Empty;
                        return exProp;
                    }
                    else
                    {
                        itemExtPropErrorMsg = "物料扩展属性不合法;";
                        return string.Empty;
                    }
                }
            }
        }


        private void CheckValues(EntityList<WorkStep> lstWorkStep, string processName, string workStepName, RoutingBomDetail bom, string key, double processId, double routingVersionId)
        {
            List<RoutingProcess> routingProcessList = routingProcessDictionary[routingVersionId];

            if (!routingProcessList.Any(x => x.ProcessId == processId))
            {
                throw new ValidationException("[{0}] 和主表工艺路线中的工序不一致。".L10nFormat(processName));
            }

            bom.RoutingProcessId = routingProcessList.FirstOrDefault(x => x.ProcessId == processId).Id;

            // 工步
            if (string.IsNullOrWhiteSpace(workStepName))
            {
                if (lstWorkStep.Any(x => x.ProcessId == processId))
                {
                    throw new ValidationException(",工序[{0}]对应的工步信息存在，工步信息为必填项。".L10nFormat(processName));
                }
            }
            else
            {
                double workStepId = 0;
                string workStepKey = processName + "," + workStepName;
                if (_workStepDict.TryGetValue(workStepKey, out workStepId))
                {
                    bom.WorkStepId = workStepId;
                }
                else
                {
                    var workStep = lstWorkStep.FirstOrDefault(p => p.ProcessId == processId && p.Name == workStepName);
                    if (workStep == null)
                    {
                        throw new ValidationException(",工序[{0}]对应的工步[{1}]信息不存在。".L10nFormat(processName, workStepName));
                    }

                    _workStepDict.Add(key, workStep.Id);

                    bom.WorkStepId = workStep.Id;
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
            {
                rows[rowNum][ImportDataHandle.MessageColumnName] += (baseExc as ValidationException).Message;
            }
            else
            {
                rows[rowNum][ImportDataHandle.MessageColumnName] += exc.Message;
            }
        }

        #region 获取值


        /// <summary>
        /// 获取工艺路线ID
        /// </summary>
        /// <param name="routing">工艺路线名称</param>
        /// <returns>工艺路线ID</returns>
        double? GetRouting(string routing)
        {
            double result;
            if (_routingDict.TryGetValue(routing, out result))
                return result;
            else
                return null;
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
        /// 验证产品编码
        /// </summary>
        /// <param name="obj">输入参数</param>
        /// <param name="messageTip">错误信息</param>
        /// <param name="row">验证数据的当前行对象</param>
        /// <returns>验证通过返回true，不通过返回false</returns>
        private bool ValidateProduct(object obj, out string messageTip, DataRow row)
        {
            string value = obj.ToString().Trim();
            messageTip = string.Empty;
            if (ValidateIsNull(ref messageTip, value, "产品编码"))
                return false;
            if (!_productDict.ContainsKey(value))
            {
                var item = RT.Service.Resolve<ItemController>().GetItem(value);
                if (item != null)
                    _productDict.Add(value, item.Id);
                else
                    messageTip = NOT_EXISTS.L10nFormat(value);
            }

            return _productDict.ContainsKey(value);
        }

        /// <summary>
        /// 验证工艺路线
        /// </summary>
        /// <param name="obj">结果值</param>
        /// <param name="messageTip">错误消息</param>
        /// <param name="row">当前行</param>
        /// <returns>验证通过返回true，不通过返回false</returns>
        private bool ValidateRouting(object obj, out string messageTip, DataRow row)
        {
            bool isValid = true;
            string value = obj.ToString();
            messageTip = string.Empty;
            if (ValidateIsNull(ref messageTip, value, "工艺路线"))
                return false;
            if (!_routingDict.ContainsKey(value))
            {
                var routing = RT.Service.Resolve<RoutingController>().GetRoutingByName(value);
                if (routing != null)
                    _routingDict.Add(value, routing.Id);
                else
                {
                    messageTip = "工艺路线[{0}]不存在".L10nFormat(value);
                    isValid = false;
                }
            }

            return isValid;
        }

        /// <summary>
        /// 验证工艺路线版本
        /// </summary>
        /// <param name="obj">输入参数</param>
        /// <param name="messageTip">错误信息</param>
        /// <param name="row">验证数据的当前行对象</param>
        /// <returns>验证通过返回true，不通过返回false</returns>
        private bool ValidateVersion(object obj, out string messageTip, DataRow row)
        {
            bool isValid = true;
            string value = obj.ToString().Trim();
            messageTip = string.Empty;
            if (ValidateIsNull(ref messageTip, value, ROUTING_VERSION))
            {
                return false;
            }

            string routing = row.Field<string>(ColIndex("工艺路线")).Trim();
            double? routingId = GetRouting(routing);
            if (routing == null || routingId == null)
            {
                messageTip = "[{0}]对应的工艺路线不存在".L10nFormat(value);
                return false;
            }
            string key = routing + "," + value;
            if (!_versionDict.ContainsKey(key))
            {
                var rv = RT.Service.Resolve<RoutingController>().GetRoutingVersion((double)routingId, value);
                if (rv != null)
                {
                    _versionDict.Add(key, rv.Id);
                }
                else
                {
                    messageTip = NOT_EXISTS.L10nFormat(value);
                    isValid = false;
                }
            }

            return isValid;
        }

        /// <summary>
        /// 工段验证
        /// </summary>
        /// <param name="obj">结果值</param>
        /// <param name="messageTip">错误消息</param>
        /// <param name="row">当前行</param>
        /// <returns>验证通过返回true，不通过返回false</returns>
        private bool ValidateSegment(object obj, out string messageTip, DataRow row)
        {
            bool isValid = true;
            string value = obj.ToString();
            messageTip = string.Empty;

            if (!ValidateIsNull(ref messageTip, value, "工段") && !_segmentDict.ContainsKey(value))
            {
                ProcessSegment ps = RT.Service.Resolve<ProcessSegmentController>().GetProcessSegmentByName(value);
                if (ps != null)
                {
                    _segmentDict.Add(value, ps.Id);
                }
                else
                {
                    messageTip = NOT_EXISTS.L10nFormat(value);
                    isValid = false;
                }
            }


            return isValid;
        }
        /// <summary>
        /// 验证物料编码
        /// </summary>
        /// <param name="obj">输入参数</param>
        /// <param name="messageTip">错误信息</param>
        /// <param name="row">验证数据的当前行对象</param>
        /// <returns>验证通过返回true，不通过返回false</returns>
        private bool ValidateMaterialCode(object obj, out string messageTip, DataRow row)
        {
            bool isValid = true;
            string value = obj.ToString().Trim();
            messageTip = string.Empty;
            if (ValidateIsNull(ref messageTip, value, "物料编码"))
            {
                return false;
            }

            if (!_itemDict.ContainsKey(value))
            {
                var item = RT.Service.Resolve<ItemController>().GetItem(value);
                if (item != null)
                {
                    _itemDict.Add(value, item.Id);
                }
                else
                {
                    messageTip = NOT_EXISTS.L10nFormat(value);
                    isValid = false;
                }
            }

            // 校验主表信息是否存在
            if (isValid)
            {
                double prvId;
                // 如果dictionary里没有则从数据库读取是否存在
                if (!TryGetRoutingBomId(row, out prvId))
                {
                    messageTip = ",工艺路线版本和工段对应的主表数据不存在";
                    isValid = false;
                }

                // 校验产品BOM是否存在当前物料
                if (isValid)
                {
                    BomDetailViewModel bomDetail = GetProductBom(row);

                    if (bomDetail == null)
                    {
                        messageTip = ",物料[{0}]在产品BOM中不存在".L10nFormat(value);
                        isValid = false;
                    }
                }

            }

            return isValid;
        }

        /// <summary>
        /// 验证工序
        /// </summary>
        /// <param name="obj">输入参数</param>
        /// <param name="messageTip">错误信息</param>
        /// <param name="row">验证数据的当前行对象</param>
        /// <returns>验证通过返回true，不通过返回false</returns>
        private bool ValidateProcess(object obj, out string messageTip, DataRow row)
        {
            bool isValid = true;
            string value = obj.ToString();
            messageTip = string.Empty;
            if (ValidateIsNull(ref messageTip, value, "工序名称"))
            {
                return false;
            }

            if (!_processDict.ContainsKey(value))
            {
                var item = RT.Service.Resolve<ProcessController>().GetProcess(value);
                if (item != null)
                {
                    var routingName = row.Field<string>(ColIndex("工艺路线")).Trim();
                    var routingVersion = row.Field<string>(ColIndex(ROUTING_VERSION)).Trim();

                    double routingVersionId;
                    string verKey = routingName + ',' + routingVersion;
                    if (!_versionDict.TryGetValue(verKey, out routingVersionId))
                    {
                        // 工艺路线版本不存在
                        return false;
                    }

                    if (!routingProcessDictionary.ContainsKey(routingVersionId))
                    {
                        // 校验工艺路线工序和工序bom工序是否一致
                        var routingProcesses = RT.Service.Resolve<RoutingBomController>()
                          .GetRoutingProcesses(routingVersionId);

                        routingProcessDictionary.Add(routingVersionId, routingProcesses.ToList());
                    }

                    List<RoutingProcess> routingProcessList = routingProcessDictionary[routingVersionId];

                    if (!routingProcessList.Any(x => x.ProcessId == item.Id))
                    {
                        messageTip = ("[{0}] 和主表工艺路线中的工序不一致。".L10nFormat(value));
                        isValid = false;
                    }
                    else
                    {
                        _processDict.Add(value, item.Id);
                    }
                }
                else
                {
                    messageTip = NOT_EXISTS.L10nFormat(value);
                    isValid = false;
                }
            }

            return isValid;
        }

        /// <summary>
        /// 验证单位用量
        /// </summary>
        /// <param name="obj">输入参数</param>
        /// <param name="messageTip">错误信息</param>
        /// <param name="row">验证数据的当前行对象</param>
        /// <returns>验证通过返回true，不通过返回false</returns>
        private bool ValidateAmount(object obj, out string messageTip, DataRow row)
        {
            string value = obj.ToString();
            messageTip = string.Empty;
            if (ValidateIsNull(ref messageTip, value, "单位用量"))
                return false;

            decimal amount = 0;
            if (!decimal.TryParse(value, out amount))
            {
                messageTip = "[{0}]是非法的数字。".L10nFormat(value);
                return false;
            }
            if (amount <= 0)
            {
                messageTip = "必须大于0。".L10nFormat(value);
                return false;
            }

            // 校验物料单位用量总和不能超过产品bom的单位用量。
            var itemCode = row.Field<string>(ColIndex("物料编码")).Trim();
            BomDetailViewModel bomDetail = GetProductBom(row);
            if (bomDetail != null)
            {
                decimal unitQty = bomDetail.UnitQty;
                decimal amountTotal = 0;
                double itemId;
                if (!_itemDict.TryGetValue(itemCode, out itemId))
                {
                    return true; // 不存在返回继续
                }

                double mainId;
                // 如果dictionary里没有则从数据库读取是否存在
                if (!GetMainId(row, out mainId))
                {
                    return true;
                }

                if (!_amountDict.TryGetValue(itemCode, out amountTotal))
                {
                    //// 数据库统一删掉，所以起始数量是0，如从数据库读取历史数据做校验： RT.Service.Resolve<RoutingBomController>().GetRountingBomUnitQty(mainId, itemId);
                    amountTotal = 0;
                    _amountDict.Add(itemCode, amountTotal);
                }
                amountTotal += amount;
                if (amountTotal > unitQty + 0.000000001M)
                {
                    messageTip = "，当前物料的单位用量总和[{0}]超过了产品bom的定义[{1}]。".L10nFormat(amountTotal, unitQty);
                    return false;
                }
                _amountDict[itemCode] = amountTotal;
            }
            return true;
        }

        /// <summary>
        /// 获取产品bom
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        private BomDetailViewModel GetProductBom(DataRow row)
        {
            var productCode = row.Field<string>(ColIndex("产品编码")).Trim();
            var itemCode = row.Field<string>(ColIndex("物料编码")).Trim();
            var segmentName = row.Field<string>(ColIndex("工段")).Trim();

            double productId;
            if (!_productDict.TryGetValue(productCode, out productId))
            {
                return null; // 不存在返回继续
            }
            double itemId;
            if (!_itemDict.TryGetValue(itemCode, out itemId))
            {
                return null; // 不存在返回继续
            }

            double? segmentId = null;
            if (_segmentDict.ContainsKey(segmentName))
                segmentId = _segmentDict[segmentName];
            string keyBom = productId.ToString() + "," + itemId + "," + segmentId ?? "";
            BomDetailViewModel bomDetail = null;
            if (!_bomDetailDict.TryGetValue(keyBom, out bomDetail))
            {
                // 取得工单BOM明细
                bomDetail = RT.Service.Resolve<RoutingBomController>()
                    .GetRoutingBomDetailViewModel(productId, itemId, segmentId);
                if (bomDetail != null)
                    _bomDetailDict.Add(keyBom, bomDetail);
            }
            return bomDetail;
        }

        /// <summary>
        /// 取主表Id
        /// </summary>
        /// <param name="row"></param>
        /// <param name="mainId"></param>
        /// <returns></returns>
        private bool GetMainId(DataRow row, out double mainId)
        {
            mainId = 0;
            var productCode = row.Field<string>(ColIndex("产品编码")).Trim();
            var routingName = row.Field<string>(ColIndex("工艺路线")).Trim();
            var routingVersion = row.Field<string>(ColIndex(ROUTING_VERSION)).Trim();
            var segmentName = row.Field<string>(ColIndex("工段")).Trim();

            double productId;
            if (!_productDict.TryGetValue(productCode, out productId))
            {
                return false; // 不存在返回继续
            }
            double routingId;
            if (!_routingDict.TryGetValue(routingName, out routingId))
            {
                return false; // 不存在返回继续
            }
            double routingVersionId;
            string verKey = routingName + ',' + routingVersion;
            if (!_versionDict.TryGetValue(verKey, out routingVersionId))
            {
                // 工艺路线版本失败的话不继续校验主表信息，返回true
                return false;
            }

            double? segmentId = null;
            if (_segmentDict.ContainsKey(segmentName))
                segmentId = _segmentDict[segmentName];

            string keyMain = productCode + "," + routingName + "," + routingVersion + "," + segmentId;

            // 如果dictionary里没有则从数据库读取是否存在
            if (!routingBomDictionary.TryGetValue(keyMain, out mainId))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 验证是否附件
        /// </summary>
        /// <param name="obj">输入参数</param>
        /// <param name="messageTip">错误信息</param>
        /// <param name="row">验证数据的当前行对象</param>
        /// <returns>验证通过返回true，不通过返回false</returns>
        private bool ValidateIsAttachment(object obj, out string messageTip, DataRow row)
        {
            string value = obj.ToString();
            messageTip = string.Empty;
            string[] values = { "是", "否" };
            if (!values.Contains(value))
            {
                messageTip = "是否附件只允许输入【是、否】。".L10nFormat(value);
                return false;
            }

            return true;
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
            {
                return false;
            }

            str = "{0}不能为空".L10nFormat(colunmName);
            return true;
        }
        /// <summary>
        /// 获取主表Id是否存在
        /// </summary>
        /// <param name="row">数据行</param>
        /// <param name="rountingBomId">输出Id</param>
        /// <returns></returns>
        public bool TryGetRoutingBomId(DataRow row, out double rountingBomId)
        {
            bool bRet = false;
            rountingBomId = 0;
            var productCode = row.Field<string>(ColIndex("产品编码")).Trim();
            var routingName = row.Field<string>(ColIndex("工艺路线")).Trim();
            var routingVersion = row.Field<string>(ColIndex(ROUTING_VERSION)).Trim();
            var segmentName = row.Field<string>(ColIndex("工段")).Trim();

            double productId;
            if (!_productDict.TryGetValue(productCode, out productId))
            {
                return false; // 不存在返回继续
            }

            double routingId;
            if (!_routingDict.TryGetValue(routingName, out routingId))
            {
                return false; // 不存在返回继续
            }

            double routingVersionId;
            string verKey = routingName + ',' + routingVersion;
            if (!_versionDict.TryGetValue(verKey, out routingVersionId))
            {
                // 工艺路线版本失败的话不继续校验主表信息，返回true
                return false;
            }

            double? segmentId = null;
            if (_segmentDict.ContainsKey(segmentName))
            {
                segmentId = _segmentDict[segmentName];
            }

            string key = productCode + "," + routingName + "," + routingVersion + "," + segmentId;

            // 如果dictionary里没有则从数据库读取是否存在
            if (!routingBomDictionary.ContainsKey(key))
            {
                var rontingBom = RT.Service.Resolve<RoutingBomController>()
                    .GetRoutingBom(productId, routingId, routingVersionId, segmentId);

                if (rontingBom != null)
                {
                    bRet = true;
                    rountingBomId = rontingBom.Id;
                    routingBomDictionary.Add(key, rontingBom.Id);
                }
            }
            else
            {
                rountingBomId = routingBomDictionary[key];
                bRet = true;
            }
            return bRet;
        }


        #endregion
    }

}

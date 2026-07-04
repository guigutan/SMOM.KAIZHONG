using SIE.Common.Catalogs;
using SIE.Common.ImportHelper;
using SIE.Core.Items;
using SIE.Core.WorkOrders;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Items;
using SIE.MES.RoutingSettings;
using SIE.MES.WIP.Runtime;
using SIE.MES.WorkOrders.Events;
using SIE.Packages.ItemLabels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace SIE.MES.WorkOrders.ImportWorkOrders
{
    /// <summary>
    /// 导入工单 处理逻辑类
    /// </summary>
    [Services.Service(FallbackType = typeof(ImportWorkOrderHandle), ServiceLifeStyle = Services.ServiceLifeStyle.Transient)]
    public class ImportWorkOrderHandle : IDisposable, IBusinessImport
    {
        #region 私有属性
        /// <summary>
        /// 产品信息"产品编码"-"产品Id"
        /// </summary>
        private Dictionary<string, double> productCodeDic = new Dictionary<string, double>();

        /// <summary>
        /// 工单类型范围
        /// </summary>
        private Dictionary<string, Enum> workOrderTypeRange = new Dictionary<string, Enum>();

        /// <summary>
        /// 车间信息"车间编码"-"车间Id"
        /// </summary>
        private Dictionary<string, double> workShopCodeDic = new Dictionary<string, double>();

        /// <summary>
        /// 资源信息"资源编码"-"资源Id"
        /// </summary>
        private Dictionary<string, double> resourceCodeDic = new Dictionary<string, double>();

        /// <summary>
        /// 上级工单信息"上级工单编码"-"上级工单Id"
        /// </summary>
        private Dictionary<string, double> parentCodeDic = new Dictionary<string, double>();

        /// <summary>
        /// ERP工单信息"ERP工单编码"-"ERP工单Id"
        /// </summary>
        private Dictionary<string, double> erpWOCodeDic = new Dictionary<string, double>();
        #endregion

        /// <summary>
        /// 导入模板的列头名
        /// </summary>
        public List<string> ColumnNameList { get; set; } = new List<string> { "工单编号", "产品编码", "扩展属性值", "计划数量", "工单类型", "计划开始时间", "计划完成时间", "车间编码", "资源编码", "上级工单编码", "ERP工单号", "客户订单号", "销售订单号", "订单数量", "工厂" };

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
                { "工单编号", new ValidColumn(ImportDataType._String, true, 80) },
                { "产品编码", new ValidColumn(ImportDataType._Custom, true, ValidProduct) },
                { "扩展属性值", new ValidColumn(ImportDataType._Custom, false, true) },
                { "计划数量", new ValidColumn(ImportDataType._Double, true, true) },
                { "工单类型", new ValidColumn(ImportDataType._Enum, true, ValidWOType) },
                { "计划开始时间", new ValidColumn(ImportDataType._Date, true, true) },
                { "计划完成时间", new ValidColumn(ImportDataType._Date, true, true) },
                { "车间编码", new ValidColumn(ImportDataType._Custom, false, ValidWorkShop) },
                { "资源编码", new ValidColumn(ImportDataType._Custom, false, ValidResource) },
                { "上级工单编码", new ValidColumn(ImportDataType._Custom, false, ValidParentWorkOrder) },
                { "ERP工单号", new ValidColumn(ImportDataType._Custom, false, ValidErpWorkOrder) },
                { "客户订单号", new ValidColumn(ImportDataType._String, false, 80) },
                { "销售订单号", new ValidColumn(ImportDataType._String, false, 80) },
                { "订单数量", new ValidColumn(ImportDataType._Double, true, true) },
                { "工厂", new ValidColumn(ImportDataType._String, true, true) }
            };

            return this;
        } 

        /// <summary>
        /// 释放数据
        /// </summary>
        public void Dispose()
        {
            productCodeDic.Clear();
            workOrderTypeRange.Clear();
            workShopCodeDic.Clear();
            resourceCodeDic.Clear();
            parentCodeDic.Clear();
            erpWOCodeDic.Clear();
        }

        /// <summary>
        /// 处理业务数据
        /// </summary>
        /// <param name="drs">需要处理的数据集</param>
        public void ProcessBusinessDataHandle(DataRow[] drs)
        {
            var ctl = RT.Service.Resolve<WorkOrderController>();

            

            List<string> itemCodeList = new List<string>();

            drs.ForEach(row =>
            {
                itemCodeList.Add(row.Field<string>(ColIndex("产品编码")).Trim());
            });
            // 物料
            var itemList = RT.Service.Resolve<ItemLabelController>().GetItemByCode(itemCodeList);
            // 物料扩展属性子表
            var itemExtPropList = RT.Service.Resolve<Items.ItemController>().GetItemPropertyValueList(itemCodeList);
            // 属性定义ids
            var definitionIds = itemExtPropList.Select(x => x.DefinitionId).ToList();
            // 属性列表
            var definitionList = RT.Service.Resolve<Items.ItemController>().GetItemPropertyDefinitionList(definitionIds);
            var catalogTypeIds = definitionList.Where(p => p.PropertyType == ItemPropertyType.Catalog).Select(p => p.CatalogTypeId).ToList();
            // 快码明细
            var catalogList = RT.Service.Resolve<Items.ItemController>().GetCatalogList(catalogTypeIds);

            //循环检验每一行数据
            foreach (var mainDataItem in drs)
            {
                var no = mainDataItem.Field<string>(ColIndex("工单编号"));
                var product = mainDataItem.Field<string>(ColIndex("产品编码"));
                var productExtPropName = mainDataItem.Field<string>(ColIndex("扩展属性值"));
                var planQty = mainDataItem.Field<string>(ColIndex("计划数量"));
                var workOrderType = mainDataItem.Field<string>(ColIndex("工单类型"));
                var planBeginDate = mainDataItem.Field<string>(ColIndex("计划开始时间"));
                var planEndDate = mainDataItem.Field<string>(ColIndex("计划完成时间"));
                var workShopCode = mainDataItem.Field<string>(ColIndex("车间编码"));
                var resourceCode = mainDataItem.Field<string>(ColIndex("资源编码"));
                var parentCode = mainDataItem.Field<string>(ColIndex("上级工单编码"));
                var erpWoNo = mainDataItem.Field<string>(ColIndex("ERP工单号"));
                var customerOrderNo = mainDataItem.Field<string>(ColIndex("客户订单号"));
                var saleOrderNo = mainDataItem.Field<string>(ColIndex("销售订单号"));
                var orderQty = mainDataItem.Field<string>(ColIndex("订单数量"));
                var factoryName = mainDataItem.Field<string>(ColIndex("工厂"));

                double productId = GetProductCode(product);
                WorkOrderType orderType = GetWOType(workOrderType);
                double? workShopId = GetWorkShopCode(workShopCode);
                double? resourceId = GetResourceCode(resourceCode);
                double? parentId = GetParentCode(parentCode);
                double? erpWorkOrderId = GetERPWOCode(erpWoNo);

                DateTime dt;

                //判断主数据是否存在
                WorkOrder wo = ctl.GetWorkOrder(no);
                var workOrderPropertyChanged = RT.Service.Resolve<ErpWorkOrderPropertyChanged>();
                var factoryId = RT.Service.Resolve<Resources.Enterprises.EnterpriseController>().GetFactoryIdByName(factoryName);
                if(factoryId == null)
                    throw new ValidationException("你没有工厂[{0}]的权限或者工厂名称不存在".L10nFormat(factoryName));
                using (var tran = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
                {
                    //如果不能新增记录错误信息
                    try
                    {
                        if (wo == null)
                        {
                            wo = new WorkOrder();
                            wo.PropertyChanged += workOrderPropertyChanged.WorkOrderOnPropertyChanged;
                            wo.Template = new Core.Items.LabelPrintTemplate();
                            wo.ProductId = productId;
                            wo.FactoryId = factoryId;
                            wo.PlanQty = decimal.Parse(planQty);

                            wo.Type = orderType;
                            wo.State = WorkOrderState.Release;
                            wo.IsPause = YesNo.No;

                            if (ValidColumn.TransferDate(planBeginDate, out dt))
                            {
                                wo.PlanBeginDate = dt;
                            }
                            else if (TransferDate(planBeginDate, out dt))
                            {
                                wo.PlanBeginDate = dt;
                            }
                            var rowItem = itemList.FirstOrDefault(p => p.Id == productId);
                            if (rowItem.EnableExtendProperty && productExtPropName.IsNullOrEmpty())
                            {
                                throw new ValidationException("物料启用扩展属性！".L10N());
                            }
                            else
                            {
                                string itemExtPropErrorMsg = string.Empty;
                                var productExtProp = ExtPropIsActive(itemExtPropList, definitionList, catalogList, rowItem, productExtPropName, out itemExtPropErrorMsg);
                                if (itemExtPropErrorMsg.IsNotEmpty())
                                {
                                    throw new ValidationException("{0}".FormatArgs(itemExtPropErrorMsg));
                                }
                                else
                                {
                                    wo.ItemExtProp = productExtProp;
                                    wo.ItemExtPropName = productExtPropName;
                                }
                            }
                            double? productRoutingId = null;
                            double? resourceRoutingId = null;
                            // 产品工艺路线
                            var productRouting = RT.Service.Resolve<RoutingSettingController>().GetProductRoutings(new List<double> { productId});
                            productRoutingId = productRouting.FirstOrDefault()?.RoutingId;
                            // 资源工艺路线
                            if (resourceId != null)
                            {
                                var resourceRouting = RT.Service.Resolve<RoutingSettingController>().GetResourceRoutings(new List<double> { (double)resourceId });
                                resourceRoutingId = resourceRouting.FirstOrDefault()?.RoutingId;
                            }
                            if (productRoutingId == null && resourceRoutingId == null)
                            {
                                throw new ValidationException("未设置产品工艺路线或产线工艺路线！".L10N());
                            }
                            else
                            {
                                var version = productRoutingId != null ? RT.Service.Resolve<RoutingSettingController>().GetRoutingDefaultVersions(new List<double> { (double)productRoutingId }) : RT.Service.Resolve<RoutingSettingController>().GetRoutingDefaultVersions(new List<double> { (double)resourceRoutingId });
                                wo.VersionId = version.FirstOrDefault()?.Id;
                            }

                            wo.WorkShopId = workShopId;
                            wo.ResourceId = resourceId;
                            wo.ParentId = parentId;
                            wo.ErpWorkOrderId = erpWorkOrderId;
                            wo.CustomerOrderNo = customerOrderNo;
                            wo.SaleOrderNo = saleOrderNo;
                            wo.OrderQty = decimal.Parse(orderQty);
                            wo.MakerId = RT.IdentityId;
                            wo.MakeDate = RF.Find<WorkOrder>().GetDbTime();
                            wo.No = no;
                            if (ValidColumn.TransferDate(planEndDate, out dt))
                            {
                                wo.PlanEndDate = dt;
                            }
                            else if (TransferDate(planEndDate, out dt))
                            {
                                wo.PlanEndDate = dt;
                            }
                        }
                        else
                        {
                            throw new ValidationException("工单编号重复".L10N());
                        }

                        wo.TemplateId = wo.Template.Id;
                        RT.Service.Resolve<WorkOrderController>().SaveWorkOrder(wo, wo.Template, WorkOrderLogType.Create, "MES导入");
                    }
                    catch (Exception exc)
                    {
                        string strMsg = AppRuntime.Location.ConnectDataDirectly ? exc.Message : exc.InnerException.Message;
                        mainDataItem[ImportDataHandle.MessageColumnName] = mainDataItem[ImportDataHandle.MessageColumnName] + strMsg;
                        continue;
                    }

                    tran.Complete();
                }
            }
        }

        #region 引用类型的属性从字典中取值
        /// <summary>
        /// 根据产品编码取值
        /// </summary>
        /// <param name="key">产品编码</param>
        /// <returns>产品Id</returns>
        private double GetProductCode(string key)
        {
            if (productCodeDic.ContainsKey(key))
                return productCodeDic[key];
            return 0;
        }

        /// <summary>
        /// 根据工单类型取值
        /// </summary>
        /// <param name="key">工单类型 string</param>
        /// <returns>工单类型 double?</returns>
        private WorkOrderType GetWOType(string key)
        {
            if (workOrderTypeRange.ContainsKey(key))
                return (WorkOrderType)workOrderTypeRange[key];
            return WorkOrderType.Mass;
        }

        /// <summary>
        /// 根据车间编码取值
        /// </summary>
        /// <param name="key">车间编码 string</param>
        /// <returns>车间编码 double?</returns>
        private double? GetWorkShopCode(string key)
        {
            if (workShopCodeDic.ContainsKey(key))
                return workShopCodeDic[key];
            return null;
        }

        /// <summary>
        /// 根据资源编码取值
        /// </summary>
        /// <param name="key">资源编码 string</param>
        /// <returns>资源编码 double?</returns>
        private double? GetResourceCode(string key)
        {
            if (resourceCodeDic.ContainsKey(key))
                return resourceCodeDic[key];
            return null;
        }

        /// <summary>
        /// 根据上级工单编码取值
        /// </summary>
        /// <param name="key">上级工单编码 string</param>
        /// <returns>上级工单编码 double?</returns>
        private double? GetParentCode(string key)
        {
            if (parentCodeDic.ContainsKey(key))
                return parentCodeDic[key];
            return null;
        }

        /// <summary>
        /// 根据ERP工单号取值
        /// </summary>
        /// <param name="key">关键词</param>
        /// <returns>ERP工单号Id</returns>
        private double? GetERPWOCode(string key)
        {
            if (erpWOCodeDic.ContainsKey(key))
                return erpWOCodeDic[key];
            return null;
        }
        #endregion

        /// <summary>
        /// 获取指定列名的索引
        /// </summary>
        /// <param name="columnName">指定列名</param>
        /// <returns>返回对应索引</returns>
        private int ColIndex(string columnName)
        {
            return ColumnNameList.IndexOf(columnName);
        }

        /// <summary>
        /// 日期转换
        /// </summary>
        /// <param name="tdate">来源日期</param>
        /// <param name="outdate">转出日期</param>
        /// <returns>bool</returns>
        private bool TransferDate(string tdate, out DateTime outdate)
        {
            string[] format = { "dd-M月-yyyy" };
            bool flag = DateTime.TryParseExact(tdate, format, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out outdate);
            return flag;
        }

        #region 基础验证
        /// <summary>
        /// 验证产品
        /// </summary>
        /// <param name="obj">产品编码</param>
        /// <param name="messageTip">错误信息</param>
        /// <param name="row">验证数据的当前行对象</param>
        /// <returns>返回是否验证通过</returns>
        private bool ValidProduct(object obj, out string messageTip, DataRow row)
        {
            return ValidWorkOrderDataHelper.ValidProduct(ref productCodeDic, obj.ToString(), out messageTip);
        }

        /// <summary>
        /// 验证工单类型
        /// </summary>
        /// <param name="obj">工单类型</param>
        /// <param name="messageTip">错误信息</param>
        /// <param name="row">验证数据的当前行对象</param>
        /// <returns>返回是否验证通过</returns>
        private bool ValidWOType(object obj, out string messageTip, DataRow row)
        {
            return ValidWorkOrderDataHelper.ValidWorkOrderType(ref workOrderTypeRange, obj.ToString(), out messageTip);
        }

        /// <summary>
        /// 验证车间
        /// </summary>
        /// <param name="obj">车间编码</param>
        /// <param name="messageTip">错误信息</param>
        /// <param name="row">验证数据的当前行对象</param>
        /// <returns>返回是否验证通过</returns>
        private bool ValidWorkShop(object obj, out string messageTip, DataRow row)
        {
            return ValidWorkOrderDataHelper.ValidWorkShop(ref workShopCodeDic, obj.ToString(), out messageTip);
        }

        /// <summary>
        /// 验证资源
        /// </summary>
        /// <param name="obj">资源编码</param>
        /// <param name="messageTip">错误信息</param>
        /// <param name="row">验证数据的当前行对象</param>
        /// <returns>返回是否验证通过</returns>
        private bool ValidResource(object obj, out string messageTip, DataRow row)
        {
            return ValidWorkOrderDataHelper.ValidResource(ref resourceCodeDic, obj.ToString(), out messageTip);
        }

        /// <summary>
        /// 验证上级工单
        /// </summary>
        /// <param name="obj">上级工单编码</param>
        /// <param name="messageTip">错误信息</param>
        /// <param name="row">验证数据的当前行对象</param>
        /// <returns>返回是否验证通过</returns>
        private bool ValidParentWorkOrder(object obj, out string messageTip, DataRow row)
        {
            return ValidWorkOrderDataHelper.ValidParentWorkOrder(ref parentCodeDic, obj.ToString(), out messageTip);
        }

        /// <summary>
        /// 验证ERP工单
        /// </summary>
        /// <param name="obj">工单编码</param>
        /// <param name="messageTip">错误信息</param>
        /// <param name="row">验证数据的当前行对象</param>
        /// <returns>返回是否验证通过</returns>
        private bool ValidErpWorkOrder(object obj, out string messageTip, DataRow row)
        {
            return ValidWorkOrderDataHelper.ValidERPWorkOrder(ref erpWOCodeDic, obj.ToString(), out messageTip);
        }
        #endregion


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
                var proNames = itemExtPropName.Split(';',StringSplitOptions.RemoveEmptyEntries).ToList();
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
    }
}
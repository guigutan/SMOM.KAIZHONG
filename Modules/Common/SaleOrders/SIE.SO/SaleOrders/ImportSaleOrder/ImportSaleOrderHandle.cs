using SIE.Common.ImportHelper;
using SIE.CSM.Customers;
using SIE.Domain;
using SIE.Items;
using SIE.Resources.Employees;
using SIE.Resources.Enterprises;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;

namespace SIE.SO.SaleOrders.ImportSaleOrder
{
    /// <summary>
    /// 导入料号检验标准 处理逻辑类
    /// </summary>
    [Services.Service(FallbackType = typeof(ImportSaleOrderHandle), ServiceLifeStyle = Services.ServiceLifeStyle.Transient)]
    public class ImportSaleOrderHandle : IDisposable, IBusinessImport
    {
        #region 私有属性
        /// <summary>
        /// 物料信息"物料编码"-"物料Id"
        /// </summary>
        private Dictionary<string, Item> itemCodeDic = new Dictionary<string, Item>();
        /// <summary>
        /// 客户信息"-客户名称"
        /// </summary>
        private Dictionary<string, Customer> CustomerNameDic = new Dictionary<string, Customer>();
        /// <summary>
        /// 销售人员"-销售名称"
        /// </summary>
        private Dictionary<string, Employee> EmployeeNameDic = new Dictionary<string, Employee>();
        /// <summary>
        /// 库存组织
        /// </summary>
        private Dictionary<string, Enterprise> EnterpriseCodeDic = new Dictionary<string, Enterprise>();
        /// <summary>
        /// 单位
        /// </summary>
        private Dictionary<string, Unit> UnitNameDic = new Dictionary<string, Unit>();
        /// <summary>
        /// 销售订单--已存在则会被记录
        /// </summary>
        private Dictionary<string, SaleOrder> SaleOrderCodeDic = new Dictionary<string, SaleOrder>();
        /// <summary>
        /// 行业类型快码
        /// </summary>
        private Dictionary<string, string> IndustryTypeNameDic = null;
        /// <summary>
        /// 订单类型快码
        /// </summary>
        private Dictionary<string, string> OrderTypeNameDic = null;
        /// <summary>
        /// 产品类型快码
        /// </summary>
        private Dictionary<string, string> ProductTypeNameDic = null;
        /// <summary>
        /// 产品等级快码
        /// </summary>
        private Dictionary<string, string> ProductLevelNameDic = null;
        /// <summary>
        /// 特殊工艺快码
        /// </summary>
        private Dictionary<string, string> SpecialProcessNameDic = null;
        #endregion

        /// <summary>
        /// 导入模板的列名
        /// </summary>
        public List<string> ColumnNameList { get; set; } = new List<string>
        {
            "序号","销售订单", "行号", "客户编号", "销售人员",
            "物料编码","物料名称", "版本号", "行业类型", "订单类型",  "产品类型","产品等级", "是否新单", "数量",  "单位",
            "库存组织代号", "库存组织名称", "MI完成时间",  "总面积M2","大板尺寸",   "开料PNL数", "SETPNL数", "PCSPNL数", "客户交期"
        };

        /// <summary>
        /// 列的标准验证 (列名 列对应验证 )
        /// </summary>
        public Dictionary<string, ValidColumn> ColumnValidList { get; set; }

        /// <summary>
        /// 创建导入料号检验标准对象
        /// </summary>
        /// <returns>返回料号检验标准对象</returns>
        public IBusinessImport CreaetColumnValid()
        {
            this.ColumnValidList = new Dictionary<string, ValidColumn>();
            this.ColumnValidList.Add("序号", new ValidColumn(ImportDataType._String, true, true));
            this.ColumnValidList.Add("销售订单", new ValidColumn(ImportDataType._String, true, ValidSaleOrder));
            this.ColumnValidList.Add("行号", new ValidColumn(ImportDataType._String, true, true));
            this.ColumnValidList.Add("客户编号", new ValidColumn(ImportDataType._String, true, ValidCustomer));
            this.ColumnValidList.Add("销售人员", new ValidColumn(ImportDataType._String, true, ValidEmployee));
            this.ColumnValidList.Add("物料编码", new ValidColumn(ImportDataType._String, true, ValidItem));
            this.ColumnValidList.Add("物料名称", new ValidColumn(ImportDataType._String, false, true));
            this.ColumnValidList.Add("版本号", new ValidColumn(ImportDataType._String, true, 40, true));
            this.ColumnValidList.Add("行业类型", new ValidColumn(ImportDataType._String, true, ValidIndustryType));
            this.ColumnValidList.Add("订单类型", new ValidColumn(ImportDataType._String, true, ValidOrderType));
            this.ColumnValidList.Add("产品类型", new ValidColumn(ImportDataType._String, true, ValidProductType));
            this.ColumnValidList.Add("产品等级", new ValidColumn(ImportDataType._String, true, ValidProductLevel));
            this.ColumnValidList.Add("是否新单", new ValidColumn(ImportDataType._Bool, true, true));
            this.ColumnValidList.Add("数量", new ValidColumn(ImportDataType._Int, true, ValidInt));
            this.ColumnValidList.Add("单位", new ValidColumn(ImportDataType._String, true, ValidUnit));
            this.ColumnValidList.Add("库存组织代号", new ValidColumn(ImportDataType._String, false, ValidEnterprise));
            this.ColumnValidList.Add("库存组织名称", new ValidColumn(ImportDataType._String, false, true));
            this.ColumnValidList.Add("MI完成时间", new ValidColumn(ImportDataType._String, true, true));
            this.ColumnValidList.Add("总面积M2", new ValidColumn(ImportDataType._Double, true, true));
            this.ColumnValidList.Add("大板尺寸", new ValidColumn(ImportDataType._String, true, true));
            this.ColumnValidList.Add("开料PNL数", new ValidColumn(ImportDataType._Double, true, true));
            this.ColumnValidList.Add("SETPNL数", new ValidColumn(ImportDataType._Double, true, true));
            this.ColumnValidList.Add("PCSPNL数", new ValidColumn(ImportDataType._Double, true, true));
            this.ColumnValidList.Add("客户交期", new ValidColumn(ImportDataType._String, true, true));
            return this;
        }

        /// <summary>
        /// 处理业务数据
        /// </summary>
        /// <param name="drs">需要处理的数据集</param>
        public void ProcessBusinessDataHandle(DataRow[] drs)
        {
            if (drs.Length <= 0) return;
            // 1、按销售订单、客户编号、销售人员分组
            var mainDataList = from g in drs
                               group g by new
                               {
                                   SaleOrderCode = g.Field<string>(ColIndex("销售订单")),
                                   CustomerCode = g.Field<string>(ColIndex("客户编号")),
                                   EmployeeCode = g.Field<string>(ColIndex("销售人员")),
                               }
                               into TempSaleOrder
                               select new
                               {
                                   SaleOrderCode = TempSaleOrder.Key.SaleOrderCode,
                                   CustomerCode = TempSaleOrder.Key.CustomerCode,
                                   EmployeeCode = TempSaleOrder.Key.EmployeeCode,
                                   DetailInfo = TempSaleOrder
                               };
            List<string> CodeList = RT.Service.Resolve<SaleOrderController>().GetSalesOrderNos(mainDataList.Count());
            Int32 Index = 0;
            //// 循环检验每一行主数据
            foreach (var mainDataItem in mainDataList)
            {
                SaleOrder saleOrder = null;
                if (SaleOrderCodeDic.Count > 0 && SaleOrderCodeDic.ContainsKey(mainDataItem.SaleOrderCode))
                {
                    saleOrder = SaleOrderCodeDic[mainDataItem.SaleOrderCode];
                }
                if (saleOrder == null)
                {
                    saleOrder = new SaleOrder();
                    saleOrder.Code = mainDataItem.SaleOrderCode;
                    saleOrder.RegisterDateTime = DateTime.Now;
                    //自建 导入 都是自建  接口
                    saleOrder.OrderSourceType = OrderSourceType.Manual;
                    saleOrder.Customer = GetCustomer(mainDataItem.CustomerCode);
                    saleOrder.Employee = EmployeeNameDic[mainDataItem.EmployeeCode];
                    saleOrder.CreateBy = RT.Identity.Id;
                    saleOrder.CreateDate = DateTime.Now;

                    List<SaleOrderDetail> DetailList = ProcessDetailData(saleOrder, mainDataItem.DetailInfo.ToList());
                    saleOrder.SaleOrderDetailList.AddRange(DetailList);
                    try
                    {
                        RF.Save(saleOrder);
                    }
                    catch (Exception ex)
                    {
                        string strMsg = ex.GetBaseException()?.Message;
                        ImportExtension.BatchAppendText(mainDataItem.DetailInfo.ToList(), ImportDataHandle.MessageColumnName, strMsg);
                        continue;
                    }
                }
                else
                {
                    List<SaleOrderDetail> DetailList = ProcessDetailData(saleOrder, mainDataItem.DetailInfo.ToList());
                    saleOrder.SaleOrderDetailList.AddRange(DetailList);
                    try
                    {
                        RF.Save(saleOrder);
                    }
                    catch (Exception ex)
                    {
                        string strMsg = ex.GetBaseException()?.Message;
                        ImportExtension.BatchAppendText(mainDataItem.DetailInfo.ToList(), ImportDataHandle.MessageColumnName, strMsg);
                        continue;
                    }
                }
                Index++;
            }
        }

        /// <summary>
        /// 处理销售订单明细的数据
        /// </summary>
        /// <param name="mainData">主表数据对象</param>
        /// <param name="detailRows">明细数据集合</param>
        private List<SaleOrderDetail> ProcessDetailData(SaleOrder mainData, List<DataRow> detailRows)
        {
            List<SaleOrderDetail> list = new List<SaleOrderDetail>();
            //// 循环检验每一行明细数据
            foreach (var detailDataItem in detailRows)
            {
                SaleOrderDetail SaleDetail = InitSaleOrderDetail(detailDataItem);
                SaleDetail.SaleOrderId = mainData.Id;

                list.Add(SaleDetail);
                //try
                //{
                //    RF.Save(SaleDetail);
                //}
                //catch (Exception ex)
                //{
                //    string strMsg = ex.GetBaseException()?.Message;
                //    detailDataItem[ImportDataHandle.MessageColumnName] = detailDataItem[ImportDataHandle.MessageColumnName] + strMsg;
                //}
            }
            return list;
        }

        /// <summary>
        /// 销售订单初始化
        /// </summary>
        /// <param name="dataItem">销售订单数据行</param>
        /// <returns>销售订单</returns>
        private SaleOrderDetail InitSaleOrderDetail(DataRow dataItem)
        {
            var LineNo = dataItem.Field<string>(ColIndex("行号"));
            var Version = dataItem.Field<string>(ColIndex("版本号"));
            var itemCode = dataItem.Field<string>(ColIndex("物料编码"));
            var IndustryType = dataItem.Field<string>(ColIndex("行业类型"));
            var OrderType = dataItem.Field<string>(ColIndex("订单类型"));
            var ProductType = dataItem.Field<string>(ColIndex("产品类型"));
            var ProductLevel = dataItem.Field<string>(ColIndex("产品等级"));
            var IsNew = dataItem.Field<string>(ColIndex("是否新单"));
            var Qty = dataItem.Field<string>(ColIndex("数量"));
            var Unit = dataItem.Field<string>(ColIndex("单位"));
            var EnterpriseCode = dataItem.Field<string>(ColIndex("库存组织代号"));
            var MiDateTime = dataItem.Field<string>(ColIndex("MI完成时间"));
            var Area = dataItem.Field<string>(ColIndex("总面积M2"));
            var PlateSize = dataItem.Field<string>(ColIndex("大板尺寸"));
            var MaterialPnl = dataItem.Field<string>(ColIndex("开料PNL数"));
            var SetPnl = dataItem.Field<string>(ColIndex("SETPNL数"));
            var PcsPnl = dataItem.Field<string>(ColIndex("PCSPNL数"));
            var RequireDelivery = dataItem.Field<string>(ColIndex("客户交期"));

            DateTime dt = DateTime.Now;
            var saleOrderDetail = new SaleOrderDetail();
            saleOrderDetail.LineNo = LineNo;
            saleOrderDetail.Version = Version;
            saleOrderDetail.Item = GetItemCode(itemCode);

            saleOrderDetail.IndustryType = IndustryTypeNameDic[IndustryType];
            saleOrderDetail.OrderType = OrderTypeNameDic[OrderType];
            saleOrderDetail.ProductType = ProductTypeNameDic[ProductType];
            saleOrderDetail.ProductLevel = ProductLevelNameDic[ProductLevel];


            saleOrderDetail.IsNew = IsNew == "是" ? true : false;
            saleOrderDetail.Qty = decimal.Parse(Qty);
            saleOrderDetail.Unit = UnitNameDic[Unit];
            saleOrderDetail.Enterprise = GetEnterpriseCode(EnterpriseCode);
            if (ValidColumn.TransferDate(MiDateTime, out dt))
            {
                saleOrderDetail.MiDateTime = dt;
            }
            else if (TransferDate(MiDateTime, out dt))
            {
                saleOrderDetail.MiDateTime = dt;
            }
            saleOrderDetail.Area = decimal.Parse(Area);
            saleOrderDetail.PlateSize = PlateSize;
            saleOrderDetail.MaterialPnl = decimal.Parse(MaterialPnl);
            saleOrderDetail.SetPnl = decimal.Parse(SetPnl);
            saleOrderDetail.PcsPnl = decimal.Parse(PcsPnl);
            saleOrderDetail.LineState = LineState.NEW;
            if (ValidColumn.TransferDate(RequireDelivery, out dt))
            {
                saleOrderDetail.RequireDelivery = dt;
            }
            else if (TransferDate(RequireDelivery, out dt))
            {
                saleOrderDetail.RequireDelivery = dt;
            }
            return saleOrderDetail;

        }


        #region 基础验证

        /// <summary>
        /// 验证物料
        /// </summary>
        /// <param name="obj">物料编码</param>
        /// <param name="messageTip">错误信息</param>
        /// <param name="dr">验证数据的当前行对象</param>
        /// <returns>返回是否验证通过</returns>
        private bool ValidItem(object obj, out string messageTip, DataRow dr)
        {
            var code = obj.ToString();
            var flag = ValidSaleOrderDataHelper.ValidItem(ref itemCodeDic, code, out messageTip);
            return flag;
        }

        /// <summary>
        /// 验证客户名称
        /// </summary>
        /// <param name="obj">客户名称</param>
        /// <param name="messageTip">错误信息</param>
        /// <param name="dr">验证数据的当前行对象</param>
        /// <returns>返回是否验证通过</returns>
        private bool ValidCustomer(object obj, out string messageTip, DataRow dr)
        {
            return ValidSaleOrderDataHelper.ValidCustomer(ref CustomerNameDic, obj.ToString(), out messageTip);
        }

        /// <summary>
        /// 验证单位名称
        /// </summary>
        /// <param name="obj">客户名称</param>
        /// <param name="messageTip">错误信息</param>
        /// <param name="dr">验证数据的当前行对象</param>
        /// <returns>返回是否验证通过</returns>
        private bool ValidUnit(object obj, out string messageTip, DataRow dr)
        {
            return ValidSaleOrderDataHelper.ValidUnit(ref UnitNameDic, obj.ToString(), out messageTip);
        }

        /// <summary>
        /// 验证销售人员
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="messageTip"></param>
        /// <param name="dr"></param>
        /// <returns></returns>
        private bool ValidEmployee(object obj, out string messageTip, DataRow dr)
        {
            return ValidSaleOrderDataHelper.ValidEmployee(ref EmployeeNameDic, obj.ToString(), out messageTip);
        }

        /// <summary>
        /// 验证库存组织
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="messageTip"></param>
        /// <param name="dr">验证数据的当前行对象</param>
        /// <returns></returns>
        private bool ValidEnterprise(object obj, out string messageTip, DataRow dr)
        {
            return ValidSaleOrderDataHelper.ValidEnterprise(ref EnterpriseCodeDic, obj.ToString(), out messageTip);
        }

        /// <summary>
        /// 验证销售订单明细是否已存在数据库
        /// </summary>
        /// <returns>返回是否验证通过</returns>
        public bool ValidSaleOrder(object obj, out string messageTip, DataRow dr)
        {
            bool isValid = true;
            messageTip = string.Empty;
            String Code = obj.ToString();
            String LineNo = dr["行号"].ToString();
            //记录已经有的销售订单
            if (!SaleOrderCodeDic.ContainsKey(Code))
            {
                SaleOrder sale = RT.Service.Resolve<SaleOrderController>().GetSaleOrderCode(Code);
                if (sale != null)
                {
                    SaleOrderCodeDic.Add(Code, sale);
                }
            }
            SaleOrderDetail saleOrderDetail = RT.Service.Resolve<SaleOrderController>().GetSaleDetail(Code, LineNo);
            if (saleOrderDetail != null)
            {
                messageTip = "销售订单明细行行号重复".L10N();
                isValid = false;
            }
            return isValid;
        }

        /// <summary>
        /// 验证行业类型快码
        /// </summary>
        /// <param name="obj">行业类型编码</param>
        /// <param name="messageTip">错误信息</param>
        /// <param name="dr">验证数据的当前行对象</param>
        /// <returns>返回是否验证通过</returns>
        public bool ValidIndustryType(object obj, out string messageTip, DataRow dr)
        {
            var flag = ValidSaleOrderDataHelper.ValidCategory(ref IndustryTypeNameDic, obj.ToString(), out messageTip, SaleOrderDetail.INDUSTRYTYPE);
            return flag;
        }

        /// <summary>
        /// 验证订单类型快码
        /// </summary>
        /// <param name="obj">验证订单名称</param>
        /// <param name="messageTip">错误信息</param>
        /// <param name="dr">验证数据的当前行对象</param>
        /// <returns>返回是否验证通过</returns>
        public bool ValidOrderType(object obj, out string messageTip, DataRow dr)
        {
            return ValidSaleOrderDataHelper.ValidCategory(ref OrderTypeNameDic, obj.ToString(), out messageTip, SaleOrderDetail.ORDERTYPE);
        }

        /// <summary>
        /// 产品类型快码
        /// </summary>
        /// <param name="obj">验证产品类型</param>
        /// <param name="messageTip">错误信息</param>
        /// <param name="dr">验证数据的当前行对象</param>
        /// <returns>返回是否验证通过</returns>

        public bool ValidProductType(object obj, out string messageTip, DataRow dr)
        {
            return ValidSaleOrderDataHelper.ValidCategory(ref ProductTypeNameDic, obj.ToString(), out messageTip, SaleOrderDetail.PRODUCTTYPE);
        }

        /// <summary>
        /// 产品等级快码
        /// </summary>
        /// <param name="obj">验证产品等级</param>
        /// <param name="messageTip">错误信息</param>
        /// <param name="dr">验证数据的当前行对象</param>
        /// <returns>返回是否验证通过</returns>
        public bool ValidProductLevel(object obj, out string messageTip, DataRow dr)
        {
            return ValidSaleOrderDataHelper.ValidCategory(ref ProductLevelNameDic, obj.ToString(), out messageTip, SaleOrderDetail.PRODUCTLEVEL);
        }

        /// <summary>
        /// 验证数量
        /// </summary>
        /// <param name="obj">验证数量</param>
        /// <param name="messageTip">错误信息</param>
        /// <param name="dr">验证数据的当前行对象</param>
        /// <returns>返回是否验证通过</returns>
        private bool ValidInt(object obj, out string messageTip, DataRow dr)
        {
            string qty = obj.ToString();
            bool isValid = true;
            messageTip = string.Empty;
            if (!string.IsNullOrEmpty(qty))
            {
                var reg = new Regex(@"^[0-9]\d*$");
                var result = reg.IsMatch(qty);
                if (!result)
                {
                    messageTip = "必须是正整数".L10N();
                    isValid = false;
                }
            }
            return isValid;
        }

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

        #endregion

        #region 引用类型的属性从字典中取值
        /// <summary>
        /// 根据物料编码取值
        /// </summary>
        /// <param name="key">物料编码</param>
        /// <returns>返回物料</returns>
        private Item GetItemCode(string key)
        {
            if (itemCodeDic.ContainsKey(key))
                return itemCodeDic[key];
            return null;
        }

        /// <summary>
        /// 根据客户名称取值
        /// </summary>
        /// <param name="key">客户名称</param>
        /// <returns>返回客户</returns>
        private Customer GetCustomer(string key)
        {
            if (CustomerNameDic.ContainsKey(key))
                return CustomerNameDic[key];

            return null;
        }

        /// <summary>
        /// 根据组织编号取值
        /// </summary>
        /// <param name="key">客户名称</param>
        /// <returns>返回客户</returns>
        private Enterprise GetEnterpriseCode(string key)
        {
            if (EnterpriseCodeDic.ContainsKey(key))
                return EnterpriseCodeDic[key];

            return null;
        }

        /// <summary>
        /// 根据组织编号取值
        /// </summary>
        /// <param name="key">客户名称</param>
        /// <returns>返回客户</returns>
        private Employee GetEmployeeName(string key)
        {
            if (EmployeeNameDic.ContainsKey(key))
                return EmployeeNameDic[key];

            return null;
        }
        #endregion

        /// <summary>
        /// 释放数据
        /// </summary>
        public void Dispose()
        {
            itemCodeDic.Clear();
            CustomerNameDic.Clear();
            EmployeeNameDic.Clear();
            EmployeeNameDic.Clear();
            EnterpriseCodeDic.Clear();
            UnitNameDic.Clear();
            SaleOrderCodeDic.Clear();
            IndustryTypeNameDic.Clear();
            OrderTypeNameDic.Clear();
            ProductTypeNameDic.Clear();
            GC.SuppressFinalize(this);
        }
    }
}

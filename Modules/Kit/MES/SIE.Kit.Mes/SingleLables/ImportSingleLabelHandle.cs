using SIE.Common.ImportHelper;
using SIE.CSM.Suppliers;
using SIE.Domain;
using SIE.Items;
using SIE.Kit.MES.SingleLabels;
using SIE.Resources.Employees;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace SIE.Packages.SingleLables
{
    /// <summary>
    /// 导入单体条码
    /// </summary>
    [Services.Service(FallbackType = typeof(ImportSingleLabelHandle), ServiceLifeStyle = Services.ServiceLifeStyle.Transient)]
    public class ImportSingleLabelHandle : IDisposable, IBusinessImport
    {
        /// <summary>
        /// 列名
        /// </summary>
        public List<string> ColumnNameList { get; set; } = new List<string>
        {
            "条码*", "批次标签", "打印时间", "来源ID", "来源号", "员工编码*", "条码来源类型", "物料编码*", "标签状态", "供应商编码*"
        };

        #region 私有属性
        /// <summary>
        /// 条码
        /// </summary>
        private Dictionary<string, double> dicSn = null;

        /// <summary>
        /// 用户编码*
        /// </summary>
        private Dictionary<string, double> dicUser = null;

        /// <summary>
        /// 条码来源类型
        /// </summary>
        private Dictionary<string, Enum> dicSourceType = null;

        /// <summary>
        /// 物料
        /// </summary>
        private Dictionary<string, double> dicItem = null;

        /// <summary>
        /// 标签状态
        /// </summary>
        private Dictionary<string, Enum> dicLabelState = null;

        /// <summary>
        /// 供应商编码*
        /// </summary>
        private Dictionary<string, double> dicSupplier = null;

        #endregion

        /// <summary>
        /// 列的标准验证 (列名 列对应验证 )
        /// </summary>
        public Dictionary<string, ValidColumn> ColumnValidList { get; set; }

        /// <summary>
        /// 创建列的验证
        /// </summary>
        /// <returns>返回当前对象</returns>
        public IBusinessImport CreaetColumnValid()
        {
            this.ColumnValidList = new Dictionary<string, ValidColumn>();
            this.ColumnValidList.Add("条码*", new ValidColumn(ImportDataType._Custom, true, ValidSn));
            this.ColumnValidList.Add("批次标签", new ValidColumn(ImportDataType._String, false, true));
            this.ColumnValidList.Add("打印时间", new ValidColumn(ImportDataType._String, false, true)); ////_DateTime
            this.ColumnValidList.Add("来源ID", new ValidColumn(ImportDataType._Double, false, 40));
            this.ColumnValidList.Add("来源号", new ValidColumn(ImportDataType._String, false, true));
            this.ColumnValidList.Add("员工编码*", new ValidColumn(ImportDataType._Custom, true, ValidUser));
            this.ColumnValidList.Add("条码来源类型", new ValidColumn(ImportDataType._Custom, true, ValidSourceType));
            this.ColumnValidList.Add("物料编码*", new ValidColumn(ImportDataType._Custom, true, ValidItem));
            this.ColumnValidList.Add("标签状态", new ValidColumn(ImportDataType._Custom, true, ValidLabelState));
            this.ColumnValidList.Add("供应商编码*", new ValidColumn(ImportDataType._Custom, true, ValidSupplier));

            return this;
        }

        #region 基础数据验证
        /// <summary>
        /// 验证供应商编码
        /// </summary>
        /// <param name="obj">供应商编码</param>
        /// <param name="messageTip">错误信息</param>
        /// <param name="dr">数据行</param>
        /// <returns>是否验证通过</returns>
        private bool ValidSupplier(object obj, out string messageTip, DataRow dr)
        {
            bool isValid = true;
            messageTip = string.Empty;
            if (dicSupplier == null)
            {
                dicSupplier = new Dictionary<string, double>();
            }

            if (!dicSupplier.ContainsKey(obj.ToString()))
            {
                var supplier = RT.Service.Resolve<SupplierController>().GetSupplier(obj.ToString());
                if (supplier != null)
                {
                    dicSupplier.Add(obj.ToString(), supplier.Id);
                }
                else
                {
                    messageTip = "供应商{0}不存在供应商表中".L10nFormat(obj.ToString());
                    isValid = false;
                }
            }

            return isValid;
        }

        /// <summary>
        /// 验证标签状态
        /// </summary>
        /// <param name="obj">标准状态Label</param>
        /// <param name="messageTip">错误信息</param>
        /// <param name="dr">数据行</param>
        /// <returns>是否验证通过</returns>
        private bool ValidLabelState(object obj, out string messageTip, DataRow dr)
        {
            bool isValid = true;
            messageTip = string.Empty;
            if (dicLabelState == null)
            {
                dicLabelState = ImportExtension.GetEnumLabel(typeof(LabelState), string.Empty);
            }

            if (!dicLabelState.ContainsKey(obj.ToString()))
            {
                messageTip = string.Concat("只能选择".L10N(), string.Join(",", dicLabelState.Keys));
                isValid = false;
            }

            return isValid;
        }

        /// <summary>
        /// 验证物料
        /// </summary>
        /// <param name="obj">物料编码</param>
        /// <param name="messageTip">错误信息</param>
        /// <param name="dr">数据行</param>
        /// <returns>是否验证通过</returns>
        private bool ValidItem(object obj, out string messageTip, DataRow dr)
        {
            bool isValid = true;
            messageTip = string.Empty;
            if (dicItem == null)
            {
                dicItem = new Dictionary<string, double>();
            }

            if (!dicItem.ContainsKey(obj.ToString()))
            {
                var item = RT.Service.Resolve<ItemController>().GetItem(obj.ToString());
                if (item != null)
                {
                    dicItem.Add(obj.ToString(), item.Id);
                }
                else
                {
                    messageTip = "物料{0}不存在物料表中".L10nFormat(obj.ToString());
                    isValid = false;
                }
            }

            return isValid;
        }

        /// <summary>
        /// 验证来源类型
        /// </summary>
        /// <param name="obj">来源类型Label</param>
        /// <param name="messageTip">错误信息</param>
        /// <param name="dr">数据行</param>
        /// <returns>是否验证通过</returns>
        private bool ValidSourceType(object obj, out string messageTip, DataRow dr)
        {
            bool isValid = true;
            messageTip = string.Empty;
            if (dicSourceType == null)
            {
                dicSourceType = ImportExtension.GetEnumLabel(typeof(SingleLabelSourceType), string.Empty);
            }

            if (!dicSourceType.ContainsKey(obj.ToString()))
            {
                messageTip = string.Concat("只能选择".L10N(), string.Join(",", dicSourceType.Keys));
                isValid = false;
            }

            return isValid;
        }

        /// <summary>
        /// 验证用户
        /// </summary>
        /// <param name="obj">员工编码</param>
        /// <param name="messageTip">错误信息</param>
        /// <param name="dr">数据行</param>
        /// <returns>是否验证通过</returns>
        private bool ValidUser(object obj, out string messageTip, DataRow dr)
        {
            bool isValid = true;
            messageTip = string.Empty;
            if (dicUser == null)
            {
                dicUser = new Dictionary<string, double>();
            }

            if (!dicUser.ContainsKey(obj.ToString()))
            {
                var employeeId = RT.Service.Resolve<EmployeeController>().GetEmployeeId(obj.ToString());
                if (employeeId != null)
                {
                    dicUser.Add(obj.ToString(), employeeId.Value);
                }
                else
                {
                    messageTip = "员工{0}不存在".L10nFormat(obj.ToString());
                    isValid = false;
                }
            }

            return isValid;
        }

        /// <summary>
        /// 验证单体条码Sn
        /// </summary>
        /// <param name="obj">单体条码</param>
        /// <param name="messageTip">错误信息</param>
        /// <param name="dr">数据行</param>
        /// <returns>验证是否通过</returns>
        private bool ValidSn(object obj, out string messageTip, DataRow dr)
        {
            bool isValid = true;
            messageTip = string.Empty;
            if (dicSn == null)
            {
                dicSn = new Dictionary<string, double>();
            }

            if (!dicSn.ContainsKey(obj.ToString()))
            {
                var sn = RT.Service.Resolve<SingleLabelContorller>().GetSingleLabel(obj.ToString());
                if (sn != null)
                {
                    dicSn.Add(obj.ToString(), sn.Id);
                    messageTip = "条码{0}在单体列表中重复".L10nFormat(obj.ToString());
                    isValid = false;
                }
            }
            else
            {
                messageTip = "条码{0}在单体列表中重复".L10nFormat(obj.ToString());
                isValid = false;
            }

            return isValid;
        }
        #endregion

        /// <summary>
        /// 释放数据
        /// </summary>
        public void Dispose()
        {
            if (dicSn != null)
            {
                dicSn.Clear();
                dicSn = null;
            }

            if (dicUser != null)
            {
                dicUser.Clear();
                dicUser = null;
            }

            if (dicSourceType != null)
            {
                dicSourceType.Clear();
                dicSourceType = null;
            }

            if (dicItem != null)
            {
                dicItem.Clear();
                dicItem = null;
            }

            if (dicLabelState != null)
            {
                dicLabelState.Clear();
                dicLabelState = null;
            }

            if (dicSupplier != null)
            {
                dicSupplier.Clear();
                dicSupplier = null;
            }
        }

        /// <summary>
        /// 业务数据处理
        /// </summary>
        /// <param name="drs">数据集合</param>
        public void ProcessBusinessDataHandle(DataRow[] drs)
        {
            var mainDataList = from g in drs
                               select new
                               {
                                   Sn = g.Field<string>(ColIndex("条码*")),
                                   BatchCode = g.Field<string>(ColIndex("批次标签")),
                                   PrintDate = g.Field<string>(ColIndex("打印时间")),
                                   SourceId = g.Field<string>(ColIndex("来源ID")),
                                   SourceNo = g.Field<string>(ColIndex("来源号")),
                                   User = g.Field<string>(ColIndex("员工编码*")),
                                   SourceType = g.Field<string>(ColIndex("条码来源类型")),
                                   Item = g.Field<string>(ColIndex("物料编码*")),
                                   LabelState = g.Field<string>(ColIndex("标签状态")),
                                   Supplier = g.Field<string>(ColIndex("供应商编码*")),
                                   DetailInfo = g
                               };
            foreach (var mainDataItem in mainDataList)
            {
                if (mainDataItem.Sn.IsNullOrEmpty())
                    continue;

                var controller = RT.Service.Resolve<SingleLabelContorller>();

                // 判断主数据是否存在
                SingleLabel singleLabel = controller.GetSingleLabel(mainDataItem.Sn);
                if (singleLabel == null)
                {
                    ////string strErrorMessage = string.Empty;
                    DateTime printDate;
                    singleLabel = new SingleLabel();
                    singleLabel.Sn = mainDataItem.Sn;
                    singleLabel.BatchCode = mainDataItem.BatchCode;
                    if (ValidColumn.TransferDate(mainDataItem.PrintDate, out printDate))
                    {
                        singleLabel.PrintDate = printDate;
                    }
                    else if (TransferDate(mainDataItem.PrintDate, out printDate))
                    {
                        singleLabel.PrintDate = printDate;
                    }

                    singleLabel.SourceId = mainDataItem.SourceId;
                    singleLabel.SourceNo = mainDataItem.SourceNo;
                    singleLabel.EmployeeId = dicUser[mainDataItem.User];
                    singleLabel.SourceType = (SingleLabelSourceType)dicSourceType[mainDataItem.SourceType];
                    singleLabel.ItemId = dicItem[mainDataItem.Item];
                    singleLabel.LabelState = (LabelState)dicLabelState[mainDataItem.LabelState];
                    singleLabel.SupplierId = dicSupplier[mainDataItem.Supplier];
                }

                // 如果不能新增记录错误信息
                try
                {
                    RF.Save(singleLabel);
                }
                catch (Exception ex)
                {
                    string strMsg = AppRuntime.Location.ConnectDataDirectly ? ex.Message : ex.InnerException.Message;
                    AppendErrorMsg(mainDataItem.DetailInfo, ImportDataHandle.MessageColumnName, strMsg);
                    ////continue;
                }
            }
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
            string[] format = { "yyyy/MM/dd HH:mm:ss" };
            bool flag = DateTime.TryParseExact(tdate, format, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out outdate);
            return flag;
        }

        /// <summary>
        /// 给保存错误的数据行记录错误数据信息
        /// </summary>
        /// <param name="row">行</param>
        /// <param name="columnName">列名</param>
        /// <param name="errorMsg">错误信息</param>
        private void AppendErrorMsg(DataRow row, string columnName, string errorMsg)
        {
            row[columnName] += errorMsg;
        }
    }
}

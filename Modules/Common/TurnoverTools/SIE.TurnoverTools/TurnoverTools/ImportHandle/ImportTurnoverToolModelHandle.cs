using SIE.Common.Catalogs;
using SIE.Common.ImportHelper;
using SIE.CSM.Customers;
using SIE.CSM.Suppliers;
using SIE.Domain;
using SIE.Packages.Boxs;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;

namespace SIE.TurnoverTools.TurnoverTools.ImportHandle
{
    /// <summary>
    /// 设备型号-导入逻辑处理
    /// </summary>
    [Services.Service(FallbackType = typeof(ImportTurnoverToolModelHandle), ServiceLifeStyle = Services.ServiceLifeStyle.Transient)]
    public class ImportTurnoverToolModelHandle : IDisposable, IBusinessImport
    {
        private List<string> columnNameList = new List<string>{
            "编码*", "名称*", "类型编码*", "默认容量*","客户编码", "专用容器", "长度(cm)", "宽(cm)","高(cm)", "供应商编码"
        };

        /// <summary>
        /// 导入模板的列头名
        /// </summary>
        public List<string> ColumnNameList
        {
            get { return columnNameList; }
            set { columnNameList = value; }
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


        /// <summary>
        /// 列的标准验证 (列名 列对应验证 )
        /// </summary>
        public Dictionary<string, ValidColumn> ColumnValidList { get; set; }


        /// <summary>
        /// 创建导入对象
        /// </summary>
        /// <returns></returns>
        public IBusinessImport CreaetColumnValid()
        {
            this.ColumnValidList = new Dictionary<string, ValidColumn>();
            this.ColumnValidList.Add("编码*", new ValidColumn(ImportDataType._String, true, true));
            this.ColumnValidList.Add("名称*", new ValidColumn(ImportDataType._String, true, true));
            this.ColumnValidList.Add("类型编码*", new ValidColumn(ImportDataType._String, true, ValidType));
            this.ColumnValidList.Add("默认容量*", new ValidColumn(ImportDataType._Int, true, ValidateCapacity));
            this.ColumnValidList.Add("客户编码", new ValidColumn(ImportDataType._String, false, ValidCustomer));

            this.ColumnValidList.Add("专用容器", new ValidColumn(ImportDataType._String, false, true));
            this.ColumnValidList.Add("长度(cm)", new ValidColumn(ImportDataType._Double, false, ValidateDouble));
            this.ColumnValidList.Add("宽(cm)", new ValidColumn(ImportDataType._Double, false, ValidateDouble));
            this.ColumnValidList.Add("高(cm)", new ValidColumn(ImportDataType._Double, false, ValidateDouble));
            this.ColumnValidList.Add("供应商编码", new ValidColumn(ImportDataType._String, false, ValidSupplier));

            return this;
        }

        /// <summary>
        /// 数据回收
        /// </summary>
        public void Dispose()
        {
            ColumnNameList = null;
            dicType = null;
            dicCustomer = null;
            dicSupplier = null;
        }

        /// <summary>
        /// 导入数据处理
        /// </summary>
        /// <param name="drs"></param>
        public void ProcessBusinessDataHandle(DataRow[] drs)
        {
            for (int i = 0; i < drs.Count(); i++)
            {
                var data = drs[i];
                try
                {
                    var customer = data.Field<string>(ColIndex("客户编码"));
                    var supplier = data.Field<string>(ColIndex("供应商编码"));
                    var capacity = data.Field<string>(ColIndex("默认容量*"));
                    int.TryParse(capacity, out int intCapacity);
                    var length = data.Field<string>(ColIndex("长度(cm)"));
                    var width = data.Field<string>(ColIndex("宽(cm)"));
                    var height = data.Field<string>(ColIndex("高(cm)"));
                    double? customerId = null;
                    if (dicCustomer != null && dicCustomer.ContainsKey(customer))
                        customerId = dicCustomer[customer];
                    double? supplierId = null;
                    if (dicSupplier != null && dicSupplier.ContainsKey(supplier))
                        supplierId = dicSupplier[supplier];
                    var model = new TurnoverToolModel()
                    {
                        Code = data.Field<string>(ColIndex("编码*")),
                        Name = data.Field<string>(ColIndex("名称*")),
                        ToolType = data.Field<string>(ColIndex("类型编码*")),
                        DefaultCapacity = intCapacity,
                        CustomerId = customerId,
                        IsDedicated = data.Field<string>(ColIndex("专用容器")) == "是",
                        SupplierId = supplierId
                    };
                    if (!length.IsNullOrEmpty())
                        model.Length = decimal.Parse(length);
                    if (!width.IsNullOrEmpty())
                        model.Width = decimal.Parse(width);
                    if (!height.IsNullOrEmpty())
                        model.Height = decimal.Parse(height);
                    RF.Save(model);
                }
                catch (Exception exc)
                {
                    data[ImportDataHandle.MessageColumnName] = exc.Message;
                    Debug.WriteLine(exc);
                }
            }
        }

        #region 数据验证

        /// <summary>
        /// 类型
        /// </summary>
        Dictionary<string, double> dicType;

        /// <summary>
        /// 客户
        /// </summary>
        Dictionary<string, double> dicCustomer;

        /// <summary>
        /// 供应商
        /// </summary>
        Dictionary<string, double> dicSupplier;

        #region 验证客户  ValidCustomer
        /// <summary>
        /// 验证客户
        /// </summary>
        /// <param name="obj">客户</param>
        /// <param name="messageTip">提示信息</param>
        /// <param name="dr">数据行</param>
        /// <returns>验证通过返回true，否则返回false</returns>
        public bool ValidCustomer(object obj, out string messageTip, DataRow dr)
        {
            bool isValid = true;
            messageTip = string.Empty;
            var context = obj.ToString();
            if (dicCustomer == null)
            {
                dicCustomer = RT.Service.Resolve<CustomerController>().GetEnableCustomers(null, "").ToDictionary(c => c.Code, c => c.Id);
            }

            if (context.IsNotEmpty() && !dicCustomer.ContainsKey(context))
            {
                messageTip = "{0}不存在于系统".L10nFormat(context);
                isValid = false;
            }

            return isValid;
        }
        #endregion

        #region 验证供应商  ValidSupplier
        /// <summary>
        /// 验证供应商
        /// </summary>
        /// <param name="obj">值</param>
        /// <param name="messageTip">提示信息</param>
        /// <param name="dr">数据行</param>
        /// <returns>验证通过返回true，否则返回false</returns>
        public bool ValidSupplier(object obj, out string messageTip, DataRow dr)
        {
            bool isValid = true;
            messageTip = string.Empty;
            var context = obj.ToString();
            if (dicSupplier == null)
            {
                dicSupplier = RT.Service.Resolve<SupplierController>().GetAllSupplier().ToDictionary(c => c.Code, c => c.Id);
            }

            if (!dicSupplier.ContainsKey(context))
            {
                messageTip = "{0}不存在于系统".L10nFormat(context);
                isValid = false;
            }

            return isValid;
        }
        #endregion

        #region 验证类型  ValidType
        /// <summary>
        /// 验证周转箱类型
        /// </summary>
        /// <param name="obj">值</param>
        /// <param name="messageTip">提示信息</param>
        /// <param name="dr">数据行</param>
        /// <returns>验证通过返回true，否则返回false</returns>
        public bool ValidType(object obj, out string messageTip, DataRow dr)
        {
            bool isValid = true;
            messageTip = string.Empty;
            var context = obj.ToString();
            if (dicType == null)
            {
                dicType = RT.Service.Resolve<CatalogController>().GetCatalogList(TurnoverBox.BoxTypeCatalog).ToDictionary(c => c.Code, c => c.Id);
            }

            if (!dicType.ContainsKey(context))
            {
                messageTip = "{0}不存在于系统".L10nFormat(context);
                isValid = false;
            }

            return isValid;
        }

        /// <summary>
        /// 验证double类型数据
        /// </summary>
        /// <param name="obj">值</param>
        /// <param name="messageTip">提示信息</param>
        /// <param name="dr">数据行</param>
        /// <returns>验证通过返回true，否则返回false</returns>
        private bool ValidateDouble(object obj, out string messageTip, DataRow dr)
        {
            bool isValid = true;
            messageTip = string.Empty;
            var context = obj.ToString();
            if (!context.IsNullOrEmpty() && !decimal.TryParse(context, out decimal result))
            {
                messageTip = "{0}格式不正确".L10nFormat(context);
                isValid = false;
            }
            return isValid;
        }

        /// <summary>
        /// 验证产品容量
        /// </summary>
        /// <param name="obj">值</param>
        /// <param name="messageTip">提示信息</param>
        /// <param name="dr">数据行</param>
        /// <returns>验证通过返回true，否则返回false</returns>
        private bool ValidateCapacity(object obj, out string messageTip, DataRow dr)
        {
            bool isValid = true;
            messageTip = string.Empty;
            var context = obj.ToString();
            if (!context.IsNullOrEmpty() && !int.TryParse(context, out int result))
            {
                messageTip = "{0}格式不正确".L10nFormat(context);
                isValid = false;
            }
            return isValid;
        }
        #endregion

        #endregion

    }
}
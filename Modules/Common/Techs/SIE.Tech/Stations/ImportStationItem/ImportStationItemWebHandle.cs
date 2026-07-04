using SIE.Common.ImportHelper;
using SIE.Domain;
using SIE.Items;
using System;
using System.Collections.Generic;
using System.Data;

namespace SIE.Tech.Stations.ImportStationItem
{
    /// <summary>
    /// 导入工位物料 处理逻辑类
    /// </summary>
    [Services.Service(FallbackType = typeof(ImportStationItemWebHandle), ServiceLifeStyle = Services.ServiceLifeStyle.Transient)]
    public class ImportStationItemWebHandle : IDisposable, IBusinessImport
    {
        /// <summary>
        /// 导入列名列表
        /// </summary>
        private List<string> columnNameList = new List<string> { "物料编码", "物料名称", "预警值", "容量值" };

        #region 私有属性
        /// <summary>
        /// 产品信息"产品编码"-"产品Id"
        /// </summary>
        private Dictionary<string, double> productCodeDic = new Dictionary<string, double>();

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
                { "物料编码", new ValidColumn(ImportDataType._Custom, true, ValidProduct) },
                { "物料名称", new ValidColumn(ImportDataType._String, true, 80) },
                { "预警值", new ValidColumn(ImportDataType._String, true, 80) },
                { "容量值", new ValidColumn(ImportDataType._String, true, 80) }
            };

            return this;
        }

        /// <summary>
        /// 释放数据
        /// </summary>
        public void Dispose()
        {
            productCodeDic.Clear();
        }

        /// <summary>
        /// 处理业务数据
        /// </summary>
        /// <param name="drs">需要处理的数据集</param>
        public void ProcessBusinessDataHandle(DataRow[] drs)
        {
            if (drs.Length == 0) return;
            var ctl = RT.Service.Resolve<StationController>();
            var stationIdstr = drs[0][ImportDataHandle.ParentId].ToString().Trim();
            if (string.IsNullOrEmpty(stationIdstr)) return;
            var stationId = double.Parse(stationIdstr);
            foreach (var mainDataItem in drs)
            {
                var itemCode = mainDataItem.Field<string>(ColIndex("物料编码"));
                var warning = mainDataItem.Field<string>(ColIndex("预警值"));
                var capacity = mainDataItem.Field<string>(ColIndex("容量值"));
                double productId = GetProductCode(itemCode);
                bool ifExist = ctl.IsExistsStationItem(productId, stationId);

                decimal cap = 0;
                decimal warn = 0;
                if (!decimal.TryParse(capacity, out cap) || !decimal.TryParse(warning, out warn) || cap <= 0 || warn <= 0)
                {
                    mainDataItem[ImportDataHandle.MessageColumnName] = "预警值和容量值必须是大于0的数字".L10N();
                    continue;
                }

                if (ifExist)
                {
                    mainDataItem[ImportDataHandle.MessageColumnName] = "工位已有该物料编号".L10N();
                    continue;
                }

                using (var tran = DB.TransactionScope(TechEntityDataProvider.ConnectionStringName))
                {
                    //如果不能新增记录错误信息
                    try
                    {
                        var stationItem = new StationItem();
                        stationItem.ItemId = productId;
                        stationItem.StationId = stationId;
                        stationItem.Capacity = cap;
                        stationItem.Warning = warn;
                        RF.Save(stationItem);
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
        #endregion

        /// <summary>
        /// 获取指定列名的索引
        /// </summary>
        /// <param name="columnName">指定列名</param>
        /// <returns>返回对应索引</returns>
        private int ColIndex(string columnName)
        {
            return columnNameList.IndexOf(columnName);
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
            bool isValid = true;
            messageTip = string.Empty;
            if (productCodeDic == null)
            {
                productCodeDic = new Dictionary<string, double>();
            }

            if (!productCodeDic.ContainsKey(obj.ToString()))
            {
                Item product = RT.Service.Resolve<ItemController>().GetItemFromCode(obj.ToString());
                if (product != null)
                {
                    productCodeDic.Add(obj.ToString(), product.Id);
                }
                else
                {
                    messageTip = "不存在于系统".L10N();
                    isValid = false;
                }
            }

            return isValid;
        }
        #endregion      
    }
}
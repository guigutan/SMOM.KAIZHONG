using SIE.Common.ImportHelper;
using SIE.Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace SIE.Warehouses.ImportHandles
{
    /// <summary>
    /// 导入
    /// </summary>
    [Services.Service(FallbackType = typeof(RoutewayImportHandle), ServiceLifeStyle = Services.ServiceLifeStyle.Transient)]
    public class RoutewayImportHandle : IDisposable, IBusinessImport
    {
        /// <summary>
        /// 导入列名
        /// </summary>
        public List<string> ColumnNameList { get; set; } = new List<string>
        {
           "编码", "名称",  "仓库编码", "库区编码", "所属巷道号","描述"
        };

        #region 私有属性
        /// <summary>
        /// 立库库区
        /// </summary>
        private readonly List<string> autoAreaCode = new List<string>();

        private Dictionary<string, double> dicWarehouse = new Dictionary<string, double>();

        private Dictionary<string, double> dicArea = new Dictionary<string, double>();

        private readonly List<string> wayCodes = new List<string>();

        private readonly List<string> wayNames = new List<string>();
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
            this.ColumnValidList.Add(ColumnNameList[0], new ValidColumn(ImportDataType._String, true, VaildCode));    // 编码
            this.ColumnValidList.Add(ColumnNameList[1], new ValidColumn(ImportDataType._String, true, VaildName));          // 名称
            this.ColumnValidList.Add(ColumnNameList[2], new ValidColumn(ImportDataType._String, true, VaildWarehouse));           // 仓库编码
            this.ColumnValidList.Add(ColumnNameList[3], new ValidColumn(ImportDataType._String, true, VaildArea));      // 库区编码
            this.ColumnValidList.Add(ColumnNameList[4], new ValidColumn(ImportDataType._Int, true, VaildNum));            // 巷道
            this.ColumnValidList.Add(ColumnNameList[5], new ValidColumn(ImportDataType._String, false, true));            // 描述

            return this;
        }

        /// <summary>
        /// 释放数据
        /// </summary>
        public void Dispose()
        {
            if (dicWarehouse != null)
            {
                dicWarehouse.Clear();
                dicWarehouse = null;
            }

            if (dicArea != null)
            {
                dicArea.Clear();
                dicArea = null;
            }

            if (autoAreaCode != null)
            {
                autoAreaCode.Clear();
            }

            if (wayCodes != null)
            {
                wayCodes.Clear();
            }

            if (wayNames != null)
                wayNames.Clear();
        }

        #region 验证数据
        /// <summary>
        /// 验证仓库
        /// </summary>
        /// <param name="obj">行号</param>
        /// <param name="messageTip">信息提示</param>
        /// <param name="dr">当前行数据</param>
        /// <returns>是否验证通过</returns>
        private bool VaildWarehouse(object obj, out string messageTip, DataRow dr)
        {
            messageTip = string.Empty;

            string warehouseCode = obj.ToString();
            if (!dicWarehouse.ContainsKey(warehouseCode))
            {
                var wh = RT.Service.Resolve<WarehouseController>().GetWarehouseByCode(warehouseCode);


                if (wh != null)
                {
                    dicWarehouse.Add(warehouseCode, wh.Id);

                }
                else
                {
                    messageTip = "仓库不存在或已禁用;".L10N();
                    AppendErrorMsg(dr, ImportDataHandle.MessageColumnName, messageTip);
                }
            }

            return true;
        }

        /// <summary>
        /// 库区验证
        /// </summary>           
        private bool VaildArea(object obj, out string messageTip, DataRow dr)
        {
            messageTip = string.Empty;

            string area = obj.ToString();
            string whCode = dr["仓库编码"].ToString();
            if (!dicArea.ContainsKey(area))
            {
                var ar = RT.Service.Resolve<WarehouseController>().GetEffectiveArea(whCode, area);
                if (ar != null)
                {
                    dicArea.Add(area, ar.Id);
                    if (ar.IsAutomatedArea)
                        autoAreaCode.Add(area);
                }
                else
                {
                    messageTip = "库区不存在或已禁用冻结，或不属于该仓库;".L10N();
                    AppendErrorMsg(dr, ImportDataHandle.MessageColumnName, messageTip);
                }
            }

            return true;
        }

        /// <summary>
        /// 编码验证
        /// </summary>           
        private bool VaildCode(object obj, out string messageTip, DataRow dr)
        {
            messageTip = string.Empty;

            string code = obj.ToString();

            if (!wayCodes.Contains(code))
            {
                if (RT.Service.Resolve<WarehouseController>().GetRouteway(code, "") != null)
                {
                    messageTip = "编码已存在;".L10N();
                    AppendErrorMsg(dr, ImportDataHandle.MessageColumnName, messageTip);
                }
                else
                {
                    try{
                        RT.Service.Resolve<WcsAddressController>().CheckAddr(code);
                        wayCodes.Add(code);
                    }
                    catch (Exception ex)
                    {
                        messageTip = ex.Message;
                        AppendErrorMsg(dr, ImportDataHandle.MessageColumnName, messageTip);
                    }
                  
                }
            }
            else
            {
                messageTip = "编码重复;".L10N();
                AppendErrorMsg(dr, ImportDataHandle.MessageColumnName, messageTip);
            }

            return true;
        }

        /// <summary>
        /// 名称验证
        /// </summary>           
        private bool VaildName(object obj, out string messageTip, DataRow dr)
        {
            messageTip = string.Empty;

            string name = obj.ToString();

            if (!wayNames.Contains(name))
            {
                if (RT.Service.Resolve<WarehouseController>().GetRouteway("", name) != null)
                {
                    messageTip = "名称重复;".L10N();
                    AppendErrorMsg(dr, ImportDataHandle.MessageColumnName, messageTip);
                }
                else
                    wayNames.Add(name);
            }
            else
            {
                messageTip = "名称重复;".L10N();
                AppendErrorMsg(dr, ImportDataHandle.MessageColumnName, messageTip);
            }

            return true;
        }

        /// <summary>
        /// 巷道号验证
        /// </summary>           
        private bool VaildNum(object obj, out string messageTip, DataRow dr)
        {
            messageTip = string.Empty;

            string name = obj.ToString();
            int i;
            if (!int.TryParse(name, out i) || i < 1)
            {
                messageTip = "巷道号必须是大于0的整数;".L10N();
                AppendErrorMsg(dr, ImportDataHandle.MessageColumnName, messageTip);
            }
            return true;
        }

        #endregion

        /// <summary>
        /// 业务数据处理
        /// </summary>
        /// <param name="drs">数据集合</param>
        public void ProcessBusinessDataHandle(DataRow[] drs)
        {
            if (drs.Length == 0) return;
            if (drs.Any(p => !string.IsNullOrEmpty(p[ImportDataHandle.MessageColumnName].ToString()))) return;
            var mainDataList = from g in drs
                               select new
                               {
                                   Code = g.Field<string>(ColIndex("编码")),
                                   Name = g.Field<string>(ColIndex("名称")),
                                   WarehouseCode = g.Field<string>(ColIndex("仓库编码")),
                                   AreaCode = g.Field<string>(ColIndex("库区编码")),
                                   Desc = g.Field<string>(ColIndex("描述")),
                                   Num = g.Field<string>(ColIndex("所属巷道号")),
                                   DetailInfo = g
                               };
            using (var tran = DB.TransactionScope(WareHouseEntityDataProvider.ConnectionStringName))
            {
                EntityList<Routeway> list = new EntityList<Routeway>();
                foreach (var mainDataItem in mainDataList)
                {

                    Routeway routeway = new Routeway()
                    {
                        Code = mainDataItem.Code,
                        Name = mainDataItem.Name,
                        WarehouseId = dicWarehouse.GetValue<double>(mainDataItem.WarehouseCode, 0),
                        StorageAreaId = dicArea.GetValue<double>(mainDataItem.AreaCode, 0),
                        Description = mainDataItem.Desc,
                        RoutewayNumber = int.Parse(mainDataItem.Num)
                    };
                    list.Add(routeway);
                }
                RF.Save(list);
                tran.Complete();
            }
        }

        /// <summary>
        /// 获取指定列名的索引
        /// </summary>
        /// <param name="columnName">指定列名</param>
        /// <returns>返回对应索引</returns>
        public virtual int ColIndex(string columnName)
        {
            return ColumnNameList.IndexOf(columnName);
        }

        /// <summary>
        /// 给保存错误的数据行记录错误数据信息
        /// </summary>
        /// <param name="row">行</param>
        /// <param name="columnName">列名</param>
        /// <param name="errorMsg">错误信息</param>
        public virtual void AppendErrorMsg(DataRow row, string columnName, string errorMsg)
        {
            row[columnName] += errorMsg;
        }
    }
}

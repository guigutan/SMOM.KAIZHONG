using SIE.Common;
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
    [Services.Service(FallbackType = typeof(StorageLocationRouteImportHandle), ServiceLifeStyle = Services.ServiceLifeStyle.Transient)]
    public class StorageLocationRouteImportHandle : IDisposable, IBusinessImport
    {
        /// <summary>
        /// 导入列名
        /// </summary>
        public List<string> ColumnNameList { get; set; } = new List<string>
        {
            "仓库编码","库位编码","巷道编码"
        };

        #region 私有属性

        private Dictionary<string, double> dicWarehouse = new Dictionary<string, double>();

        private Dictionary<string, Routeway> dicRouteway = new Dictionary<string, Routeway>();

        private Dictionary<string, StorageLocation> dicLoc = new Dictionary<string, StorageLocation>();


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
            this.ColumnValidList.Add(ColumnNameList[0], new ValidColumn(ImportDataType._String, true, VaildWarehouse));    // 仓库编码
            this.ColumnValidList.Add(ColumnNameList[1], new ValidColumn(ImportDataType._String, true, true));    // 库位编码
            this.ColumnValidList.Add(ColumnNameList[2], new ValidColumn(ImportDataType._String, true, VaildRouteway));          // 巷道编码            
            return this;
        }

        /// <summary>
        /// 释放数据
        /// </summary>
        public void Dispose()
        {
            if (dicRouteway != null)
            {
                dicRouteway.Clear();
                dicRouteway = null;
            }
            if (dicLoc != null)
            {
                dicLoc.Clear();
                dicLoc = null;
            }
            if (dicWarehouse != null)
            {
                dicWarehouse.Clear();
                dicWarehouse = null;
            }

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
        /// 巷道验证
        /// </summary>           
        private bool VaildRouteway(object obj, out string messageTip, DataRow dr)
        {
            messageTip = string.Empty;
            var ctl = RT.Service.Resolve<WarehouseController>();
            string routeCode = obj.ToString();
            string whcode = dr["仓库编码"].ToString();
            var wh = dicWarehouse.GetValue<double>(whcode, 0);
            if (dicWarehouse.Count == 0)
            {
                return true;
            }

            string loc = dr["库位编码"].ToString();
            var locEntity = dicLoc.GetValue<StorageLocation>(loc, null);
            if (locEntity == null)
            {
                locEntity = ctl.GetLocation(loc, wh);
                if (locEntity == null)
                {
                    messageTip = "当前仓库没有该库位;".L10N();
                    AppendErrorMsg(dr, ImportDataHandle.MessageColumnName, messageTip);
                    return true;
                }
                else
                {
                    dicLoc.Add(loc, locEntity);
                }
            }
            var routeEntity = dicRouteway.GetValue<Routeway>(routeCode, null);
            if (routeEntity == null)
            {
                routeEntity = ctl.GetRouteway(locEntity.WarehouseId, routeCode);
            }
            if (routeEntity == null || routeEntity.WarehouseId != locEntity.WarehouseId)
            {
                messageTip = "巷道不存在或所在仓库跟库位所在仓库不一样;".L10N();
                AppendErrorMsg(dr, ImportDataHandle.MessageColumnName, messageTip);
            }
            else if (routeEntity.StorageAreaId != locEntity.AreaId)
            {
                messageTip = "巷道所在库区跟库位所在库区不一样;".L10N();
                AppendErrorMsg(dr, ImportDataHandle.MessageColumnName, messageTip);
            }
            else
            {
                locEntity.RoutewayId = routeEntity.Id;
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
            if (drs[0].Table.Rows.Count != drs.Length || drs.Any(p => !string.IsNullOrEmpty(p[ImportDataHandle.MessageColumnName].ToString()))) return;

            //var mainDataList = from g in drs
            //                   select new
            //                   {
            //                       StorageCode = g.Field<string>(ColIndex("库位编码")),
            //                       RoutewayCode = g.Field<string>(ColIndex("巷道编码")),
            //                       DetailInfo = g
            //                   };
            using (var tran = DB.TransactionScope(WareHouseEntityDataProvider.ConnectionStringName))
            {
                var locEntitys = dicLoc.Select(f => f.Value).AsEntityList();
                RF.Save(locEntitys);
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

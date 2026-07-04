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
    [Services.Service(FallbackType = typeof(LogicAreaImportHandle), ServiceLifeStyle = Services.ServiceLifeStyle.Transient)]
    public class LogicAreaImportHandle : IDisposable, IBusinessImport
    {
        /// <summary>
        /// 导入列名
        /// </summary>
        public List<string> ColumnNameList { get; set; } = new List<string>
        {
           "编码","名称","是否立库库区", "仓库编码","描述","库位编码"
        };

        #region 私有属性

        /// <summary>
        /// 记录分区表头信息
        /// </summary>
        private readonly Dictionary<string, LogicArea> dicLogic = new Dictionary<string, LogicArea>();

        /// <summary>
        /// 记录分区编码和库位Id
        /// </summary>
        private readonly Dictionary<string, List<double>> dicLocAndLogic = new Dictionary<string, List<double>>();

        /// <summary>
        /// 记录仓库信息
        /// </summary>
        private Dictionary<string, double> dicWarehouse = new Dictionary<string, double>();

        /// <summary>
        /// 记录库位信息
        /// </summary>
        private Dictionary<string, StorageLocation> dicStorage = new Dictionary<string, StorageLocation>();

        private readonly Dictionary<string, double> dicLoc = new Dictionary<string, double>();
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
            this.ColumnValidList.Add(ColumnNameList[1], new ValidColumn(ImportDataType._String, true, true));          // 名称
            this.ColumnValidList.Add(ColumnNameList[2], new ValidColumn(ImportDataType._Bool, true, true));            // 是否立库库区
            this.ColumnValidList.Add(ColumnNameList[3], new ValidColumn(ImportDataType._String, true, VaildWarehouse)); // 仓库编码   
            this.ColumnValidList.Add(ColumnNameList[4], new ValidColumn(ImportDataType._String, false, true));   // 描述          
            this.ColumnValidList.Add(ColumnNameList[5], new ValidColumn(ImportDataType._String, false, VaildStorage));            // 库位编码
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

            if (dicStorage != null)
            {
                dicStorage.Clear();
                dicStorage = null;
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
            string code = dr["编码"].ToString();
            var logic = dicLogic.GetValue<LogicArea>(code);
            if (dicLogic.Count == 0 || logic == null)
                return true;
            string warehouseCode = obj.ToString();
            if (logic.WarehouseCode.IsNotEmpty() && logic.WarehouseCode != warehouseCode)
            {
                messageTip = "编码在系统已存在，但仓库不是{0};".L10nFormat(warehouseCode);
                AppendErrorMsg(dr, ImportDataHandle.MessageColumnName, messageTip);
            }
            else
            {
                var whId = dicWarehouse.GetValue<double>(warehouseCode, 0);
                if (whId == 0)
                {
                    var wh = RT.Service.Resolve<WarehouseController>().GetWarehouseByCode(warehouseCode);
                    if (wh != null)
                    {
                        whId = wh.Id;
                        dicWarehouse.Add(warehouseCode, wh.Id);
                    }
                    else
                    {
                        messageTip = "仓库不存在或已禁用;".L10N();
                        AppendErrorMsg(dr, ImportDataHandle.MessageColumnName, messageTip);
                    }
                }
                if (logic.WarehouseId > 0 && logic.WarehouseId != whId)
                {
                    messageTip = "当前编码已有记录，并且仓库不是{0};".L10nFormat(code, warehouseCode);
                    AppendErrorMsg(dr, ImportDataHandle.MessageColumnName, messageTip);
                }
                else
                {
                    logic.WarehouseId = whId;
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
            string loc = dr["库位编码"].ToString();
            string name = dr["名称"].ToString();
            if (!dicLogic.ContainsKey(code))
            {
                var logic = RT.Service.Resolve<WarehouseController>().GetLogicArea(code, "");
                if (logic == null)
                {
                    if (RT.Service.Resolve<WarehouseController>().GetLogicArea("", name) != null)
                    {
                        messageTip = "名称已存在".L10N();
                        AppendErrorMsg(dr, ImportDataHandle.MessageColumnName, messageTip);
                    }
                    else
                    {
                        dicLogic.Add(code, new LogicArea() { Code = code, Name = name });
                    }
                }
                else
                {
                    if (loc.IsNullOrEmpty())
                    {
                        messageTip = "当前编码已在系统存在".L10nFormat(name);
                        AppendErrorMsg(dr, ImportDataHandle.MessageColumnName, messageTip);
                    }
                    else
                    {
                        if (logic.Name != name)
                        {
                            messageTip = "当前编码在系统存在，但对应不是名称{0}".L10nFormat(name);
                            AppendErrorMsg(dr, ImportDataHandle.MessageColumnName, messageTip);
                        }
                        else
                        {
                            dicLogic.Add(code, logic);
                        }
                    }
                }
            }
            else
            {
                var logic = dicLogic.GetValue<LogicArea>(code);
                if (logic.PersistenceStatus == PersistenceStatus.New && loc.IsNullOrEmpty())
                {
                    messageTip = "将要新增的记录重复，编码{0}".L10nFormat(code);
                    AppendErrorMsg(dr, ImportDataHandle.MessageColumnName, messageTip);
                }
                else if (logic.Name != name)
                {
                    messageTip = "当前编码已有记录，并且名称是{0}".L10nFormat(logic.Name);
                    AppendErrorMsg(dr, ImportDataHandle.MessageColumnName, messageTip);
                }
                else
                {
                    //
                }
            }

            return true;
        }

        /// <summary>
        /// 验证库位
        /// </summary>     
        private bool VaildStorage(object obj, out string messageTip, DataRow dr)
        {
            messageTip = string.Empty;
            string locCode = obj.ToString();
            string code = dr["编码"].ToString();
            string wh = dr["仓库编码"].ToString();
            bool isAutomatedArea = dr["是否立库库区"]?.ToString() == "是";
            if (dicWarehouse.Count == 0)
            {
                return true;
            }

            double whId = dicWarehouse.GetValue<double>(wh);

            double locId = 0;
            var locEntity = dicStorage.GetValue<StorageLocation>(locCode, null);
            if (locEntity == null)
            {
                locEntity = RT.Service.Resolve<WarehouseController>().GetEffectiveLocation(whId, locCode);
                if (locEntity == null)
                {
                    messageTip = "库位不存在或已冻结禁用或不在当前仓库;".L10N();
                    AppendErrorMsg(dr, ImportDataHandle.MessageColumnName, messageTip);
                    return true;
                }
                else
                {
                    dicStorage.Add(locCode, locEntity);
                    dicLoc.Add(locEntity.Code, locEntity.Id);
                    locId = locEntity.Id;
                }
            }
            else
            {
                locId = locEntity.Id;
            }

            if (isAutomatedArea != locEntity.IsAutomatedStorage)
            {
                messageTip = isAutomatedArea ? "库位需是立库库位;".L10N() : "库位需是非立库库位;".L10N();
                AppendErrorMsg(dr, ImportDataHandle.MessageColumnName, messageTip);
                return true;
            }

            var dicLogicLocs = dicLocAndLogic.FirstOrDefault(f => f.Key == code).Value;
            if (dicLogicLocs == null || !dicLogicLocs.Contains(locId))
            {
                if (dicLogicLocs == null)
                {
                    dicLogicLocs = new List<double>() { locId };
                    dicLocAndLogic.Add(code, dicLogicLocs);
                }
                else
                {
                    dicLogicLocs.Add(locId);
                }
            }
            else
            {
                messageTip = "当前编码已有库位{0};".L10nFormat(locCode);
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
            if (drs.Length == 0)
            {
                return;
            }

            if (drs.Any(p => !string.IsNullOrEmpty(p[ImportDataHandle.MessageColumnName].ToString())))
            {
                return;
            }

            var mainDataList = from g in drs
                               select new
                               {
                                   Code = g.Field<string>(ColIndex("编码")),
                                   Name = g.Field<string>(ColIndex("名称")),
                                   WarehouseCode = g.Field<string>(ColIndex("仓库编码")),
                                   Desc = g.Field<string>(ColIndex("描述")),
                                   LocCode = g.Field<string>(ColIndex("库位编码")),
                                   IsAutomatedArea = g.Field<string>(ColIndex("是否立库库区")),
                                   DetailInfo = g
                               };
            using (var tran = DB.TransactionScope(WareHouseEntityDataProvider.ConnectionStringName))
            {
                foreach (var mainDataItem in mainDataList)
                {
                    var logic = dicLogic.GetValue<LogicArea>(mainDataItem.Code);
                    if (logic.PersistenceStatus == PersistenceStatus.New)
                    {
                        logic.IsAutomatedArea = mainDataItem.IsAutomatedArea == "是";
                        logic.Description = mainDataItem.Desc;
                        logic.State = State.Enable;
                        logic.GenerateId();
                        RF.Save(logic);
                        logic.PersistenceStatus = PersistenceStatus.Unchanged;
                    }
                    if (mainDataItem.LocCode.IsNotEmpty())
                    {
                        LogicAreaLocation logicAreaLocation = new LogicAreaLocation()
                        {
                            LogicAreaId = logic.Id,
                            StorageLocationId = dicLoc.GetValue<double>(mainDataItem.LocCode)
                        };
                        RF.Save(logicAreaLocation);
                    }
                }
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
            if(row == null)
            {
                return;
            }
            row[columnName] += errorMsg;
        }
    }
}

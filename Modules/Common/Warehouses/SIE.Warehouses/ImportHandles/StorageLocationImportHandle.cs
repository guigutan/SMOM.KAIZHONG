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
    [Services.Service(FallbackType = typeof(StorageLocationImportHandle), ServiceLifeStyle = Services.ServiceLifeStyle.Transient)]
    public class StorageLocationImportHandle : IDisposable, IBusinessImport
    {
        /// <summary>
        /// 导入列名
        /// </summary>
        public List<string> ColumnNameList { get; set; } = new List<string>
        {
            "仓库编码", "库区编码", "排编号", "层编号", "列编号", "深度","巷道编码", "库位编码","库位名称","最深度库位","重量KG", "长M","宽M", "高M"
        };

        #region 私有属性
        /// <summary>
        /// 立库库区
        /// </summary>
        private readonly List<string> autoAreaCode = new List<string>();

        private Dictionary<string, double> dicWarehouse = new Dictionary<string, double>();

        private Dictionary<string, double> dicArea = new Dictionary<string, double>();

        private Dictionary<string, Routeway> dicRouteway = new Dictionary<string, Routeway>();

        private readonly Dictionary<string, double> dicRouteWh = new Dictionary<string, double>();

        private readonly List<string> storageCode = new List<string>();

        private readonly List<string> warehouseAndStorages = new List<string>();


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
            this.ColumnValidList.Add(ColumnNameList[1], new ValidColumn(ImportDataType._String, true, VaildArea));          // 库区编码
            this.ColumnValidList.Add(ColumnNameList[2], new ValidColumn(ImportDataType._PositiveInt, false, true));           // 排编号
            this.ColumnValidList.Add(ColumnNameList[3], new ValidColumn(ImportDataType._PositiveInt, false, true));      // 层编号
            this.ColumnValidList.Add(ColumnNameList[4], new ValidColumn(ImportDataType._PositiveInt, false, true));            // 列编号
            this.ColumnValidList.Add(ColumnNameList[5], new ValidColumn(ImportDataType._PositiveInt, false, true));            // 深度
            this.ColumnValidList.Add(ColumnNameList[6], new ValidColumn(ImportDataType._String, false, VaildRouteway));            // 巷道编码
            //this.ColumnValidList.Add(ColumnNameList[7], new ValidColumn(ImportDataType._String, false, VaildStorage));            // 库位编码
            this.ColumnValidList.Add(ColumnNameList[9], new ValidColumn(ImportDataType._Bool, true, true));            // 最深度库位
            this.ColumnValidList.Add(ColumnNameList[10], new ValidColumn(ImportDataType._Double, false, VaildDecimal));            // 重量
            this.ColumnValidList.Add(ColumnNameList[11], new ValidColumn(ImportDataType._Double, false, VaildDecimal));            // 长
            this.ColumnValidList.Add(ColumnNameList[12], new ValidColumn(ImportDataType._Double, false, VaildDecimal));            // 宽
            this.ColumnValidList.Add(ColumnNameList[13], new ValidColumn(ImportDataType._Double, false, VaildDecimal));            // 高
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

            if (dicRouteway != null)
            {
                dicRouteway.Clear();
                dicRouteway = null;
            }

            if (autoAreaCode != null)
            {
                autoAreaCode.Clear();
            }

            if (storageCode != null)
            {
                storageCode.Clear();
            }

            if (warehouseAndStorages != null)
                warehouseAndStorages.Clear();
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
            string onlyOneArea = whCode + "," + area;
            if (!dicArea.ContainsKey(onlyOneArea))
            {
                var ar = RT.Service.Resolve<WarehouseController>().GetEffectiveArea(whCode, area);
                if (ar != null)
                {
                    dicArea.Add(onlyOneArea, ar.Id);
                    if (ar.IsAutomatedArea)
                        autoAreaCode.Add(onlyOneArea);
                }
                else
                {
                    messageTip = "库区不存在或已禁用冻结，或不属于该仓库;".L10N();
                    AppendErrorMsg(dr, ImportDataHandle.MessageColumnName, messageTip);
                }
            }
            if (autoAreaCode.Contains(onlyOneArea))
            {
                int a, b, c, d;
                if (dr["排编号"].ToString() == "" || dr["层编号"].ToString() == "" || dr["列编号"].ToString() == ""
                    || dr["深度"].ToString() == ""
                    || !(int.TryParse(dr["排编号"].ToString(), out a) && a > 0)
                    || !(int.TryParse(dr["层编号"].ToString(), out b) && b > 0)
                    || !(int.TryParse(dr["列编号"].ToString(), out c) && c > 0)
                    || !(int.TryParse(dr["深度"].ToString(), out d) && d >= 0)
                    )
                {
                    messageTip = "立库库位必须填写排层列深度;且必须大于0".L10N();
                    AppendErrorMsg(dr, ImportDataHandle.MessageColumnName, messageTip);
                }
            }

            if (messageTip.IsNullOrEmpty())
            {
                return VaildStorage(out messageTip, dr);
            }
            else
                return true;
        }

        /// <summary>
        /// 库位验证
        /// </summary>           
        private bool VaildStorage(out string messageTip, DataRow dr)
        {
            messageTip = string.Empty;
            string area = dr["库区编码"].ToString();
            string store = dr["库位编码"].ToString();
            string wh = dr["仓库编码"].ToString();
            string code = "Cell:" + dr["排编号"] + "_" + dr["层编号"] + "_" + dr["列编号"] + "_" + dr["深度"] + "_" + dr["库区编码"] + "_" + dr["仓库编码"];
            string onlyOneArea = store + "," + area;
            if (autoAreaCode.Contains(onlyOneArea))
            {
                if (store.IsNotEmpty())
                {
                    messageTip = "库区是立库，库位编码自动生成，不需要填写;".L10N();
                    AppendErrorMsg(dr, ImportDataHandle.MessageColumnName, messageTip);
                }
                else
                {
                    if (dr["排编号"].ToString() == "" || dr["层编号"].ToString() == "" || dr["列编号"].ToString() == "" || dr["深度"].ToString() == "")
                    {
                        messageTip = "库区是立库，排层列深度必须填写;".L10N();
                        AppendErrorMsg(dr, ImportDataHandle.MessageColumnName, messageTip);
                    }
                }
                if (storageCode.Contains(code))
                {
                    messageTip = "检测到重复的库位编码;".L10N();
                    AppendErrorMsg(dr, ImportDataHandle.MessageColumnName, messageTip);
                }
                else
                {
                    if (RT.Service.Resolve<WarehouseController>().CheckIsExistLoction(wh, code))
                    {
                        messageTip = "按排层列深度仓库库区组成的编码已存在;".L10N();
                        AppendErrorMsg(dr, ImportDataHandle.MessageColumnName, messageTip);
                    }
                    else
                    {
                        storageCode.Add(code);
                    }
                }
            }
            else
            {
                if (store.IsNullOrEmpty() && (dr["排编号"].ToString() == "" || dr["层编号"].ToString() == "" || dr["列编号"].ToString() == "" || dr["深度"].ToString() == ""))
                {
                    messageTip = "库位编码必须填写;".L10N();
                    AppendErrorMsg(dr, ImportDataHandle.MessageColumnName, messageTip);
                    return true;
                }
                if (store.IsNotEmpty())
                {
                    if (storageCode.Contains(dr["仓库编码"] + "_" + store))
                    {
                        messageTip = "检测到重复的库位编码;".L10N();
                        AppendErrorMsg(dr, ImportDataHandle.MessageColumnName, messageTip);
                    }
                    else
                    {
                        if (RT.Service.Resolve<WarehouseController>().CheckIsExistLoction(wh, store))
                        {
                            messageTip = "库位编码已存在;".L10N();
                            AppendErrorMsg(dr, ImportDataHandle.MessageColumnName, messageTip);
                        }
                        else
                        {
                            storageCode.Add(dr["仓库编码"] + "_" + store);
                        }
                    }
                }
                else
                {
                    if (storageCode.Contains(code))
                    {
                        messageTip = "检测到重复的库位编码;".L10N();
                        AppendErrorMsg(dr, ImportDataHandle.MessageColumnName, messageTip);
                    }
                    else
                    {
                        if (RT.Service.Resolve<WarehouseController>().CheckIsExistLoction(wh, code))
                        {
                            messageTip = "按排层列深度仓库库区组成的编码已存在;".L10N();
                            AppendErrorMsg(dr, ImportDataHandle.MessageColumnName, messageTip);
                        }
                        else
                        {
                            storageCode.Add(code);
                        }
                    }
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

            string routeCode = obj.ToString();
            string wh = dr["仓库编码"].ToString();
            string area = dr["库区编码"].ToString();
            string onlyOneArea = wh + "," + area;
            var areaId = dicArea.GetValue<double>(onlyOneArea);
            var whId = dicWarehouse.GetValue<double>(wh);
            var dicWhId = dicRouteWh.GetValue<double>(routeCode, 0);
            if (dicWhId > 0 && dicWhId != whId)
            {
                messageTip = "巷道不在当前仓库;".L10N();
                AppendErrorMsg(dr, ImportDataHandle.MessageColumnName, messageTip);
            }
            else
            {

                if (!dicRouteway.ContainsKey(routeCode))
                {
                    var wayd = RT.Service.Resolve<WarehouseController>().GetRouteway(whId, routeCode);
                    dicRouteway.Add(routeCode, wayd);
                }
                var way = dicRouteway.GetValue<Routeway>(routeCode, null);
                if (way == null)
                {
                    messageTip = "巷道[{0}]不存在或不在当前仓库[{2},Id={1}];".L10nFormat(routeCode, whId, wh);
                    AppendErrorMsg(dr, ImportDataHandle.MessageColumnName, messageTip);
                }
                else
                {
                    if (way.StorageAreaId != areaId)
                    {
                        messageTip = "巷道所在库区不是[{0}];".L10nFormat(area);
                        AppendErrorMsg(dr, ImportDataHandle.MessageColumnName, messageTip);
                    }
                    else if (!dicRouteWh.ContainsKey(routeCode))
                    {
                        dicRouteWh.Add(routeCode, whId);
                    }
                }

            }
            return true;
        }

        /// <summary>
        /// 验证数量
        /// </summary>            
        private bool VaildDecimal(object obj, out string messageTip, DataRow dr)
        {
            messageTip = string.Empty;
            decimal q;
            if (!decimal.TryParse(obj.ToString(), out q) || q <= 0)
            {
                messageTip = "重量、长宽高必须大于0;".L10N();
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
            if (drs.Length != drs[0].Table.Rows.Count) return;
            if (drs.Any(p => !string.IsNullOrEmpty(p[ImportDataHandle.MessageColumnName].ToString()))) return;
            var mainDataList = from g in drs
                               select new
                               {
                                   WarehouseCode = g.Field<string>(ColIndex("仓库编码")),
                                   AreaCode = g.Field<string>(ColIndex("库区编码")),
                                   Row = g.Field<string>(ColIndex("排编号")),
                                   s = g.Field<string>(ColIndex("层编号")),
                                   Column = g.Field<string>(ColIndex("列编号")),
                                   Deep = g.Field<string>(ColIndex("深度")),
                                   RoutewayCode = g.Field<string>(ColIndex("巷道编码")),
                                   StorageCode = g.Field<string>(ColIndex("库位编码")),
                                   StorageName = g.Field<string>(ColIndex("库位名称")),
                                   IsMaxDepth = g.Field<string>(ColIndex("最深度库位")),
                                   Weight = g.Field<string>(ColIndex("重量KG")),
                                   Long = g.Field<string>(ColIndex("长M")),
                                   Width = g.Field<string>(ColIndex("宽M")),
                                   Height = g.Field<string>(ColIndex("高M")),
                                   DetailInfo = g
                               };
            using (var tran = DB.TransactionScope(WareHouseEntityDataProvider.ConnectionStringName))
            {
                EntityList<StorageLocation> list = new EntityList<StorageLocation>();
                Dictionary<double, StorageLocationInfo> dic = new Dictionary<double, StorageLocationInfo>();
                Dictionary<double, StorageLocationLayinInfo> dicLay = new Dictionary<double, StorageLocationLayinInfo>();
                foreach (var mainDataItem in mainDataList)
                {
                    string code = "Cell:" + mainDataItem.Row + "_" + mainDataItem.s + "_" + mainDataItem.Column + "_" + mainDataItem.Deep + "_" + mainDataItem.AreaCode + "_" + mainDataItem.WarehouseCode;
                    string name = mainDataItem.Row + "排" + mainDataItem.s + "层" + mainDataItem.Column + "列" + mainDataItem.Deep + "深度" + mainDataItem.AreaCode + "区" + mainDataItem.WarehouseCode + "仓";
                    if (mainDataItem.StorageCode.IsNotEmpty())
                    {
                        code = mainDataItem.StorageCode;
                        if (mainDataItem.StorageName.IsNotEmpty())
                        {
                            name = mainDataItem.StorageName;
                        }
                        else
                        {
                            name = "[" + mainDataItem.StorageCode + "]库位";
                        }
                        
                    }
                    string onlyOneArea = mainDataItem.WarehouseCode + "," + mainDataItem.AreaCode;
                    StorageLocation storageLocation = new StorageLocation()
                    {
                        Code = code,
                        AreaId = dicArea.GetValue<double>(onlyOneArea, 0),
                        WarehouseId = dicWarehouse.GetValue<double>(mainDataItem.WarehouseCode, 0),
                        RowNo = mainDataItem.Row.IsNullOrEmpty() ? 0 : int.Parse(mainDataItem.Row),
                        ColumnNo = mainDataItem.Column.IsNullOrEmpty() ? 0 : int.Parse(mainDataItem.Column),
                        LayerNo = mainDataItem.s.IsNullOrEmpty() ? 0 : int.Parse(mainDataItem.s),
                        State = State.Enable,
                        Depth = mainDataItem.Deep.IsNullOrEmpty() ? 0 : int.Parse(mainDataItem.Deep),
                        RoutewayId = dicRouteway.GetValue<Routeway>(mainDataItem.RoutewayCode, null)?.Id,
                        Name = name,
                        IsMaxDepth = mainDataItem.IsMaxDepth == "是"
                    };
                    storageLocation.GenerateId();
                    decimal l = 0, w = 0, h = 0, wt = 0;

                    var info = new StorageLocationInfo();
                    if (decimal.TryParse(mainDataItem.Long, out l) && l > 0)
                        info.Length = l;
                    if (decimal.TryParse(mainDataItem.Width, out w) && w > 0)
                        info.Width = w;
                    if (decimal.TryParse(mainDataItem.Height, out h) && h > 0)
                        info.Height = h;
                    var lay = new StorageLocationLayinInfo();
                    if (decimal.TryParse(mainDataItem.Weight, out wt) && wt > 0)
                        lay.WeightLimit = wt;

                    dic.Add(storageLocation.Id, info);
                    dicLay.Add(storageLocation.Id, lay);
                    list.Add(storageLocation);
                }
                RF.Save(list);
                var infos = RT.Service.Resolve<WarehouseController>().GetStorageLocationInfos(list.Select(f => f.Id).ToList());
                infos.ForEach(p =>
                {
                    var st = dic.GetValue<StorageLocationInfo>(p.StorageLocationId, null);
                    if (st != null)
                    {
                        p.Width = st.Width;
                        p.Height = st.Height;
                        p.Length = st.Length;
                    }
                });
                RF.Save(infos);
                var lays = RT.Service.Resolve<WarehouseController>().GetStorageLocationLayinInfos(list.Select(f => f.Id).ToList());
                lays.ForEach(p =>
                {
                    var st = dicLay.GetValue<StorageLocationLayinInfo>(p.StorageLocationId, null);
                    if (st != null)
                    {
                        p.WeightLimit = st.WeightLimit;
                    }
                });
                RF.Save(lays);

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

using SIE.Common;
using SIE.Common.Configs;
using SIE.Common.Configs.CommonConfigs;
using SIE.Common.NumberRules;
using SIE.Core.Common;
using SIE.Data;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Inventory.Commom;
using SIE.Inventory.Onhands;
using SIE.Inventory.Piles.Data;
using SIE.Inventory.Task;
using SIE.Inventory.TransactionProcessing;
using SIE.Items;
using SIE.Packages.Boxs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.Inventory.Piles
{
    /// <summary>
    /// 垛表控制器
    /// </summary>
    public partial class PileController : DomainController
    {
        /// <summary>
        /// 获取垛表信息
        /// </summary>
        /// <param name="criteria">查询实体</param>
        /// <returns>垛表信息</returns>
        public virtual EntityList<Pile> GetPiles(PileCriteria criteria)
        {
            var q = Query<Pile>();
            //////仓库权限关联查询           
            if (!string.IsNullOrEmpty(criteria.Code))
                q.Where(p => p.Code.Contains(criteria.Code));
            if (!string.IsNullOrEmpty(criteria.Model))
                q.Where(p => p.Model.Contains(criteria.Model));
            if (criteria.PileState.HasValue)
                q.Where(p => p.PileState == criteria.PileState.Value);
            if (criteria.TurnoverContainer.HasValue)
                q.Where(p => p.TurnoverContainer == criteria.TurnoverContainer);
            if (!string.IsNullOrEmpty(criteria.BillNo))
                q.Where(p => p.BillNo.Contains(criteria.BillNo));
            if (!string.IsNullOrEmpty(criteria.CurLocation))
                q.Where(p => p.CurLocation.Contains(criteria.CurLocation));
            if (criteria.ItemState.HasValue)
                q.Where(p => p.ItemState == criteria.ItemState);

            //创建时间
            if (criteria.CreateDate.BeginValue.HasValue)
                q.Where(p => p.CreateDate >= criteria.CreateDate.BeginValue.Value);

            if (criteria.CreateDate.EndValue.HasValue)
                q.Where(p => p.CreateDate <= criteria.CreateDate.EndValue.Value);

            if (criteria.ItemId.HasValue || criteria.LotId.HasValue || criteria.WarehouseId.HasValue || criteria.StorageAreaId.HasValue || criteria.StorageLocationId.HasValue)
            {
                ////垛明细
                var pileDtl = RF.Find<PileDetail>().EntityMeta;
                string isPhantom = pileDtl.Property(PhantomEntityExtension.IS_PHANTOMProperty).ColumnMeta.ColumnName;
                var pileDtlTabel = pileDtl.TableMeta.TableName;
                var pileId = pileDtl.Property(PileDetail.PileIdProperty).ColumnMeta.ColumnName;

                ////LPN库存
                var onhand = RF.Find<LotLpnOnhand>().EntityMeta;
                var onhandTabel = onhand.TableMeta.TableName;

                StringBuilder dtlCriteria = new StringBuilder();
                StringBuilder onhandCriteria = new StringBuilder();
                const string andSql = " AND {0} = {1}";
                if (criteria.ItemId.HasValue)
                {
                    var itemId = pileDtl.Property(PileDetail.ItemIdProperty).ColumnMeta.ColumnName;
                    dtlCriteria.Append(string.Format(andSql, itemId, criteria.ItemId.Value));

                    itemId = onhand.Property(LotLpnOnhand.ItemIdProperty).ColumnMeta.ColumnName;
                    onhandCriteria.Append(string.Format(andSql, itemId, criteria.ItemId.Value));
                }
                if (criteria.LotId.HasValue)
                {
                    var lotId = pileDtl.Property(PileDetail.LotIdProperty).ColumnMeta.ColumnName;
                    dtlCriteria.Append(string.Format(andSql, lotId, criteria.LotId.Value));

                    lotId = onhand.Property(LotLpnOnhand.LotIdProperty).ColumnMeta.ColumnName;
                    onhandCriteria.Append(string.Format(andSql, lotId, criteria.LotId.Value));
                }

                if (criteria.WarehouseId.HasValue)
                {
                    var warehouseId = pileDtl.Property(PileDetail.WarehouseIdProperty).ColumnMeta.ColumnName;
                    dtlCriteria.Append(string.Format(andSql, warehouseId, criteria.WarehouseId.Value));

                    warehouseId = onhand.Property(LotLpnOnhand.WarehouseIdProperty).ColumnMeta.ColumnName;
                    onhandCriteria.Append(string.Format(andSql, warehouseId, criteria.WarehouseId.Value));
                }
                if (criteria.StorageAreaId.HasValue)
                {
                    var areaId = pileDtl.Property(PileDetail.StorageAreaIdProperty).ColumnMeta.ColumnName;
                    dtlCriteria.Append(string.Format(andSql, areaId, criteria.StorageAreaId.Value));

                    areaId = onhand.Property(LotLpnOnhand.StorageAreaIdProperty).ColumnMeta.ColumnName;
                    onhandCriteria.Append(string.Format(andSql, areaId, criteria.StorageAreaId.Value));
                }
                if (criteria.StorageLocationId.HasValue)
                {
                    var locId = pileDtl.Property(PileDetail.StorageLocationIdProperty).ColumnMeta.ColumnName;
                    dtlCriteria.Append(string.Format(andSql, locId, criteria.StorageLocationId.Value));

                    locId = onhand.Property(LotLpnOnhand.StorageLocationIdProperty).ColumnMeta.ColumnName;
                    onhandCriteria.Append(string.Format(andSql, locId, criteria.StorageLocationId.Value));
                }

                StringBuilder sb = new StringBuilder();
                sb.Append(" (EXISTS (");
                sb.Append(string.Format(@"SELECT 1 FROM {0} d WHERE d.{1} = t0.id AND d.{2} = 0 {3}", pileDtlTabel, pileId, isPhantom, dtlCriteria));

                sb.Append(" ) OR EXISTS (");

                sb.Append(string.Format(@"SELECT 1 FROM {0} l WHERE l.{1} = t0.code AND l.{2} = 0 {3}", onhandTabel, onhand.Property(LotLpnOnhand.LpnProperty).ColumnMeta.ColumnName, isPhantom, onhandCriteria));

                sb.Append(" )) ");
                if (sb.Length > 0)
                    q.Where(p => p.SQL<bool>(new FormattedSql(sb.ToString())));
            }

            var result = q.ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
            return result;
        }

        /// <summary>
        /// 获取垛表信息
        /// </summary>
        /// <param name="lpnList">垛码集合</param>
        /// <returns>垛表信息</returns>
        public virtual EntityList<Pile> GetPiles(List<string> lpnList, EagerLoadOptions elo = null)
        {
            if (elo == null)
                elo = new EagerLoadOptions().LoadWithViewProperty();
            return lpnList.SplitContains(lpns =>
            {
                return Query<Pile>().Where(p => lpns.Contains(p.Code)).ToList(null, elo);
            });
        }

        /// <summary>
        /// 获取垛表信息
        /// </summary>
        /// <param name="lpnList">垛码集合</param>
        /// <returns>垛表信息</returns>
        public virtual List<PileStateData> GetPileStateData(List<string> lpnList)
        {
            List<PileStateData> rst = new List<PileStateData>();
            lpnList.SplitDataExecute(lpns =>
           {
               rst.AddRange(Query<Pile>().Where(p => lpns.Contains(p.Code)).Select(p => new { p.Code, p.ItemState, p.PileState }).ToList<PileStateData>());
           });
            return rst;
        }

        /// <summary>
        /// 获取垛表信息
        /// </summary>
        /// <param name="ids">ID集合</param>
        /// <returns>垛表信息</returns>
        public virtual EntityList<Pile> GetPileByIds(List<double> ids)
        {
            if (ids == null || ids.Count == 0)
            {
                return new EntityList<Pile>();
            }
            return ids.SplitContains(sons =>
            {
                return Query<Pile>().Where(p => sons.Contains(p.Id)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
        }

        /// <summary>
        /// 获取垛表编码配置
        /// </summary>
        /// <returns>垛表编码配置</returns>
        /// <exception cref="ValidationException">异常信息</exception>
        public virtual NoConfigValue GetPileCodeConfig()
        {
            var config = ConfigService.GetConfig(new NoConfig(), typeof(Pile));
            if (config == null || config.NumberRuleId == null)
            {
                throw new ValidationException("未配置编码规则!".L10N());
            }

            return config;
        }

        /// <summary>
        /// 获取垛表编码
        /// </summary>
        /// <param name="qty">新建Asn单号数量</param>
        /// <returns>垛表编码集合</returns>
        public virtual List<string> GetPileCodes(int qty)
        {
            var config = GetPileCodeConfig();

            return RT.Service.Resolve<NumberRuleController>().GenerateSegment(config.NumberRuleId.Value, qty).ToList();
        }

        /// <summary>
        /// 获取垛表数据
        /// </summary>
        /// <param name="code">编码</param>
        /// <returns>垛表</returns>
        public virtual Pile GetPile(string code)
        {
            return Query<Pile>().Where(p => p.Code == code).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取垛表日志
        /// </summary>
        /// <param name="pileId">垛表ID</param>
        /// <param name="sortInfo">排序</param>
        /// <param name="pagingInfo">分页</param>
        /// <returns>垛表日志</returns>
        public virtual EntityList<PileLog> GetPileLogs(double pileId, IList<OrderInfo> sortInfo, PagingInfo pagingInfo)
        {
            var query = Query<PileLog>()
                .Join<Pile>((p, l) => p.PileCode == l.Code && l.Id == pileId);
            return query.OrderBy(sortInfo).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取垛明细（创建/在库/出库）
        /// </summary>
        /// <param name="pileId">垛ID</param>
        /// <param name="sortInfo">排序</param>
        /// <param name="pagingInfo">分页</param>
        /// <returns>垛明细</returns>
        public virtual EntityList<PileDetailViewModel> GetPileDetailViewModels(double pileId, IList<OrderInfo> sortInfo, PagingInfo pagingInfo)
        {
            ////在库垛信息
            var result = new EntityList<PileDetailViewModel>();
            var query = Query<LotLpnOnhand>()
                .Join<Pile>((o, p) => p.Code == o.Lpn && p.Id == pileId)
                .Join<Item>((o, m) => m.Id == o.ItemId)
                .Join<Item, Unit>((m, u) => m.UnitId == u.Id)
                ;
            query.Where(p => p.Qty > 0 && p.Lpn != "*");
            query.Select<Pile, Item, Unit>((o, p, m, u) => new
            {
                Sn = p.SQL<string>("null sn"),
                ItemCode = m.Code,
                ItemName = m.Name,
                SpecificationModel = m.SpecificationModel,
                Qty = o.Qty,
                ItemUnitName = u.Name,
                ItemExtPropName = o.ItemExtPropName,
                LotCode = o.LotCode,
                StorerCode = o.StorerCode,
                ProjectNo = o.ProjectNo,
                TaskNo = o.TaskNo,
                OnhandState = o.State,
                ItemState = p.SQL<bool>("1 ItemState"),
                Warehouse = o.Warehouse.Name,//+ "[" + o.Warehouse.Code + "]"
                StorageArea = o.StorageArea.Name,//+ "[" + o.StorageArea.Code + "]",
                StorageLocation = o.StorageLocation.Name //+ "[" + o.StorageLocation.Code + "]"
            });
            var list = query.ToList<PileDetailViewModel>();
            result.AddRange(list);

            ////创建、出库垛信息
            var dtlQuery = Query<PileDetail>()
                .LeftJoin<Lot>((d, l) => d.LotId == l.Id)
                .Join<Item>((d, m) => m.Id == d.ItemId)
                .Join<Item, Unit>((m, u) => m.UnitId == u.Id);

            dtlQuery.Select<Lot, Item, Unit>((d, l, m, u) => new
            {
                Sn = d.Sn,
                ItemCode = m.Code,
                ItemName = m.Name,
                SpecificationModel = m.SpecificationModel,
                Qty = d.Qty,
                ItemUnitName = u.Name,
                ItemExtPropName = d.ItemExtPropName,
                LotCode = l.Code,
                StorerCode = d.StorerCode,
                ProjectNo = d.ProjectNo,
                TaskNo = d.TaskNo,
                OnhandState = d.OnhandState,
                ItemState = d.ItemState,
                Warehouse = d.Warehouse.Code,
                StorageArea = d.StorageArea.Code,
                StorageLocation = d.StorageLocation.Code
            });

            dtlQuery.Where(d => d.PileId == pileId);
            var dtList = dtlQuery.ToList<PileDetailViewModel>();

            result.AddRange(dtList);

            return result.OrderBy(p => p.ItemState).AsEntityList();
        }

        /// <summary>
        /// 闲置垛
        /// </summary>
        /// <param name="pile">垛</param>
        private void UnusedPile(Pile pile)
        {
            ////垛状态为闲置时，清空如下栏位：“重量、高” 跟永学确认先不清空单据和业务类型(2022.04.07)
            pile.ItemState = null;
            ////pile.BillNo = null;
            ////pile.BusinessType = null;
            pile.Weight = null;
            pile.Height = null;
            pile.Width = pile.ModelWidth;
            pile.Length = pile.ModelLength;
        }

        /// <summary>
        /// 创建垛信息
        /// </summary>
        /// <param name="lpn">LPN</param>
        /// <param name="boxState">垛状态</param>
        /// <param name="itemState">物料状态</param>
        /// <param name="data">垛表信息</param>
        /// <param name="businessType">业务类型</param>
        private Pile CreatePile(string lpn, BoxState boxState, ItemState itemState, PileData data, BusinessType? businessType = null)
        {
            var pile = new Pile();
            pile.Code = lpn;
            pile.ItemState = itemState;
            pile.PileState = boxState;
            pile.TurnoverContainer = false;
            if (data != null)
            {
                pile.BillNo = data.BillNo;
                pile.CurLocation = data.CurLocation;
                pile.Weight = data.Weight;
                pile.Length = data.Length;
                pile.Width = data.Width;
                pile.Height = data.Height;
                pile.Model = data.Model;
                pile.ModelName = data.ModelName;
            }
            if (pile.BillNo.IsNotEmpty())
            {
                pile.BusinessType = businessType;
            }

            if (boxState == BoxState.Unused)
            {
                //创建物料状态为空,垛状态为闲置的垛 
                pile.ItemState = null;
                pile.PileState = BoxState.Unused;
            }

            RF.Save(pile);
            CreatePileLog(pile, PileOpType.Create);
            return pile;
        }

        /// <summary>
        /// 创建垛信息（存在型号）
        /// </summary>
        /// <param name="lpn">垛号</param>
        /// <param name="lpnModel">型号</param>
        /// <param name="billNo">单据号</param>
        /// <param name="boxState">多状态</param>
        /// <param name="itemState">物料状态</param>
        /// <param name="businessType">业务类型</param>
        public virtual Pile CreatePile(string lpn, string lpnModel, string billNo, BoxState boxState, ItemState itemState, BusinessType? businessType)
        {
            var boxCtl = RT.Service.Resolve<BoxController>();

            TurnoverBoxModel model;
            PileData data = new PileData();
            if (billNo.IsNotEmpty())
            {
                data.BillNo = billNo;
            }
            if (lpnModel.IsNotEmpty())
            {
                model = boxCtl.GetTurnoverBoxModel(lpnModel);
                if (model == null)
                {
                    throw new ValidationException("周转箱型号[{0}]不存在".L10nFormat(lpnModel));
                }
                data.Height = model?.Height;
                data.Width = model?.Width;
                data.Length = model?.Length;
                data.Model = model?.Code;
                data.ModelName = model?.Name;
            }
            var newPile = CreatePile(lpn, boxState, itemState, data, businessType);
            return newPile;
        }

        /// <summary>
        /// 更新垛信息
        /// </summary>
        /// <param name="lpn">垛号</param>    
        /// <param name="acton">委托</param>
        /// <returns>更新行数</returns>
        public virtual int UpdatePile(string lpn, Action<IEntityUpdate<Pile>> acton = null)
        {
            var query = DB.Update<Pile>();
            acton?.Invoke(query);
            query.Where(p => p.Code == lpn);
            var count = query.Execute();
            if (count > 0)
            {
                var pile = GetPile(lpn);
                CreatePileLog(pile, PileOpType.Upate);
            }
            return count;
        }

        /// <summary>
        /// 更新垛表信息
        /// </summary>
        /// <param name="piles">垛表</param>
        /// <param name="data">垛表数据</param>
        /// <param name="businessType">业务类型</param>
        private void UpdatePileDatas(List<Pile> piles, PileData data, BusinessType? businessType)
        {
            var onhandCtl = RT.Service.Resolve<InvOnhandController>();

            List<string> lpns = piles.Select(p => p.Code).ToList();

            //有库存的LPN集合
            List<string> hasLpns = onhandCtl.GetLotLpnOnhandByLpns(lpns);
            if (hasLpns.Any())
            {
                List<Pile> hasPiles = piles.Where(p => hasLpns.Contains(p.Code)).ToList();
                UpdatePiles(hasPiles, BoxState.Inuse, ItemState.InStore, data, businessType);
            }

            //没有库存的LPN集合
            List<string> noLpns = lpns.Except(hasLpns).ToList();
            if (noLpns.Any())
            {
                List<Pile> noPiles = piles.Where(p => noLpns.Contains(p.Code)).ToList();
                noPiles.ForEach(pile =>
                {
                    RecyclePile(pile);
                });
            }
        }

        /// <summary>
        /// 更新垛表
        /// </summary>
        /// <param name="piles">垛</param>
        /// <param name="boxState">状态</param>
        /// <param name="itemState">物料状态</param>
        /// <param name="data">垛表信息</param>
        /// <param name="businessType">业务类型</param>
        /// <exception cref="ValidationException"></exception>
        private void UpdatePiles(List<Pile> piles, BoxState? boxState, ItemState? itemState, PileData data, BusinessType? businessType)
        {
            if (piles == null || !piles.Any())
            {
                return;
            }
            var ids = piles.Select(p => p.Id).ToList();
            var query = DB.Update<Pile>();
            if (data != null)
            {
                if (data.CurLocation.IsNotEmpty())
                {
                    query.Set(p => p.CurLocation, data.CurLocation);
                }
                if (data.Weight.HasValue)
                {
                    query.Set(p => p.Weight, data.Weight);
                }
                if (data.Length.HasValue)
                {
                    query.Set(p => p.Length, data.Length);
                }
                if (data.Width.HasValue)
                {
                    query.Set(p => p.Width, data.Width);
                }
                if (data.Height.HasValue)
                {
                    query.Set(p => p.Height, data.Height);
                }
                if (data.BillNo.IsNotEmpty())
                {
                    query.Set(p => p.BillNo, data.BillNo);
                }
            }

            if (boxState.HasValue)
            {
                query.Set(p => p.PileState, boxState.Value);
            }
            if (itemState.HasValue)
            {
                query.Set(p => p.ItemState, itemState);
            }
            if (!itemState.HasValue && boxState == BoxState.Unused)
            {
                query.Set(p => p.ItemState, itemState);
            }
            if (businessType.HasValue)
            {
                query.Set(p => p.BusinessType, businessType);
            }
            query.Set(p => p.UpdateBy, RT.IdentityId).Set(p => p.UpdateDate, DateTime.Now);
            query.Where(p => ids.Contains(p.Id));
            var count = query.Execute();
            if (count > 0)
            {
                CreatePileLogs(piles, PileOpType.Upate, boxState, itemState, data, businessType);
            }
            if (boxState.HasValue)
            {
                var turnCodes = piles.Where(a => a.TurnoverContainer).Select(a => a.Code).ToList();
                RT.Service.Resolve<BoxController>().UpdateTurnoverBox(turnCodes, boxState.Value);
            }
        }

        /// <summary>
        /// 更新垛表
        /// </summary>
        /// <param name="piles">更新垛表</param>
        /// <param name="billNo">单据号</param>
        /// <param name="businessType">业务类型</param>
        public virtual void UpdatePiles(EntityList<Pile> piles, string billNo, BusinessType? businessType)
        {
            PileData data = new PileData() { BillNo = billNo };
            UpdatePileDatas(piles.ToList(), data, businessType);
        }

        /// <summary>
        /// 更新垛表信息
        /// </summary>
        /// <param name="piles">垛表列表</param>
        /// <param name="billNo">单据号</param>
        /// <param name="businessType">业务类型</param>
        /// <param name="boxState">垛表状态</param>
        /// <param name="itemState">物料状态</param>
        public virtual void UpdatePiles(EntityList<Pile> piles, string billNo, BusinessType? businessType, BoxState boxState, ItemState itemState)
        {
            List<double> ids = piles.Select(p => p.Id).ToList();
            var query = DB.Update<Pile>();
            query.Set(p => p.BillNo, billNo)
                 .Set(p => p.PileState, boxState)
                 .Set(p => p.ItemState, itemState)
                 .Set(p => p.BusinessType, businessType)
                 .Where(p => ids.Contains(p.Id));

            PileData data = new PileData() { BillNo = billNo };

            var count = query.Execute();
            if (count > 0)
            {
                CreatePileLogs(piles.ToList(), PileOpType.Upate, boxState, itemState, data, businessType);
            }
        }

        /// <summary>
        /// 更新垛表
        /// </summary>
        /// <param name="lpns">垛</param>
        /// <param name="billNo">单据号</param>
        /// <param name="businessType">业务类型</param>
        /// <returns>垛表</returns>
        public virtual void UpdatePiles(List<string> lpns, string billNo, BusinessType? businessType)
        {
            lpns = lpns.Where(p => p != "*" && p.IsNotEmpty()).Distinct().ToList();
            if (!lpns.Any())
            {
                return;
            }

            var piles = GetPiles(lpns);
            if (!piles.Any())
            {
                return;
            }
            PileData data = new PileData();
            if (billNo.IsNotEmpty())
            {
                data.BillNo = billNo;
            }

            UpdatePileDatas(piles.ToList(), data, businessType);
        }

        /// <summary>
        /// 更新垛状态
        /// </summary>
        /// <param name="lpns">lpn</param>
        /// <param name="state">状态</param>
        public virtual void UpdatePileState(List<string> lpns, BoxState state)
        {
            DB.Update<Pile>().Set(p => p.PileState, state).Where(p => lpns.Contains(p.Code)).Execute();
        }

        /// <summary>
        /// 批量生成垛表数据
        /// </summary>
        /// <param name="model">型号</param>
        /// <param name="qty">数量</param>
        public virtual EntityList<Pile> BatchGeneratePileData(TurnoverBoxModel model, int qty)
        {
            var onhandCtl = RT.Service.Resolve<InvOnhandController>();

            List<string> pileCodes = GetPileCodes(qty);

            List<string> hasCodes = new List<string>();

            //验证是否在垛表存在
            var hasPile = GetPiles(pileCodes);
            if (hasPile.Any())
            {
                hasCodes.AddRange(hasPile.Select(p => p.Code).ToList());
            }

            //验证是否在库存表中存在
            var onhands = onhandCtl.GetLotLpnOnHands(pileCodes);
            if (onhands.Any())
            {
                hasCodes.AddRange(onhands.Select(p => p.Lpn).Distinct().ToList());
            }

            List<string> hasPileCodes = hasCodes.Distinct().ToList();
            if (hasPileCodes.Any())
            {
                pileCodes.AddRange(GetPileCodes(hasPileCodes.Count));
            }

            EntityList<Pile> pileList = new EntityList<Pile>();
            EntityList<PileLog> logs = new EntityList<PileLog>();

            foreach (var code in pileCodes.Where(p => !hasPileCodes.Contains(p)))
            {
                Pile pile = new Pile();
                pile.Code = code;
                pile.Model = model.Code;
                pile.ModelName = model.Name;
                pile.PileState = BoxState.Unused;
                pile.TurnoverContainer = false;
                pile.Width = model.Width;
                pile.Length = model.Length;
                pile.Height = model.Height;

                pileList.Add(pile);

                //写入日志
                logs.Add(CreatePileLog(pile, PileOpType.Create, false));
            }

            ////批量保存
            RT.Service.Resolve<SIE.Core.Common.Controllers.CommonController>().BatchInsertSave(pileList);
            RT.Service.Resolve<SIE.Core.Common.Controllers.CommonController>().BatchInsertSave(logs);

            return pileList;
        }

        /// <summary>
        /// 出库回写垛明细信息
        /// </summary>
        /// <param name="invCollectDataList">垛更新数据</param>       
        public virtual void InsertPileDtlFromOutStorage(List<InvCollectData> invCollectDataList)
        {

            List<PileUpdateData> datas = new List<PileUpdateData>();
            invCollectDataList.ForEach(p =>
            {
                datas.Add(new PileUpdateData
                {
                    Lpn = p.stockTrans.FromLpn,
                    Qty = p.stockTrans.Qty,
                    ItemId = p.item.Id,
                    ItemExtProp = p.ItemExtProp,
                    ItemExtPropName = p.ItemExtPropName,
                    LotId = p.lot.Id,
                    ProjectNo = p.baseTransactionData.ProjectNo,
                    TaskNo = p.baseTransactionData.TaskNo,
                    StorerCode = p.baseTransactionData.StorerCode,
                    BillNo = p.baseTransactionData.BillNo
                });
            });

            var lpns = datas.Where(p => p.Lpn.IsNotEmpty() && p.Lpn != "*").Select(f => f.Lpn).Distinct().ToList();
            if (!lpns.Any())
            {
                return;
            }
            var piles = GetPiles(lpns);
            if (lpns.Count != piles.Count)
            {
                throw new ValidationException("LPN[{0}]在垛表不存在".L10nFormat(string.Join(",", lpns.Where(f => !piles.Select(a => a.Code).Contains(f)))));
            }

            datas.GroupBy(f => f.Lpn).ForEach(p =>
               {
                   var onhands = RT.Service.Resolve<InvOnhandController>().CheckHasOnhand(new List<string>() { p.Key });
                   if (!onhands)
                   {//没有库存才写入                   
                       var pile = piles.FirstOrDefault(f => f.Code == p.Key);
                       pile.ItemState = ItemState.OutStore;
                       pile.BusinessType = BusinessType.OutStorage;
                       pile.BillNo = p.First().BillNo;
                       p.ToList().ForEach(a =>
                       {
                           PileDetail pileDetail = new PileDetail()
                           {
                               ItemId = a.ItemId,
                               LotId = a.LotId,
                               ItemExtProp = a.ItemExtProp,
                               ItemExtPropName = a.ItemExtPropName,
                               ItemState = ItemState.OutStore,
                               Qty = a.Qty,
                               ProjectNo = a.ProjectNo,
                               TaskNo = a.TaskNo,
                               StorerCode = a.StorerCode,
                           };
                           pile.PileDetailList.Add(pileDetail);

                           //if (pile.TurnoverContainer)
                           //{
                           //    RT.Service.Resolve<BoxController>().BoxItemBindingFromOut(a.Lpn, pileDetail.ItemId.Value, pileDetail.Qty);
                           //}
                       });

                   }
               });
            RF.Save(piles);
            CreatePileLogs(piles.ToList(), PileOpType.Upate);
        }

        /// <summary>
        /// 取消出库更新垛数据
        /// </summary>
        /// <param name="lpns">LPN</param>
        public virtual void UpdatePileDtlFromCancelOut(List<string> lpns)
        {
            var piles = GetPiles(lpns);
            piles.ForEach(p =>
            {
                p.ItemState = ItemState.InStore;
                p.BusinessType = BusinessType.CanelOutStorage;
                p.PileDetailList.ForEach(a => a.PersistenceStatus = PersistenceStatus.Deleted);
            });
            RF.Save(piles);
            CreatePileLogs(piles.ToList(), PileOpType.Upate);
        }

        /// <summary>
        /// 解绑垛信息
        /// </summary>
        /// <param name="lpn">LPN</param>
        public virtual void UnBindingPile(string lpn)
        {
            var pile = GetPile(lpn);
            ////闲置垛赋值
            UnusedPile(pile);
            if (pile.TurnoverContainer)
            {
                RT.Service.Resolve<BoxController>().UpdateTurnoverBox(pile.Code, BoxState.Unused);
            }

            pile.PileState = BoxState.Unused;
            RF.Save(pile);
            CreatePileLog(pile, PileOpType.Upate);
        }

        /// <summary>
        /// 垛回收
        /// </summary>
        /// <param name="pile">垛信息</param>
        public virtual void RecyclePile(Pile pile)
        {
            using (var tran = DB.TransactionScope(InveEntityDataProvider.ConnectionStringName))
            {
                pile.ItemState = null;
                pile.BillNo = null;
                pile.BusinessType = null;
                pile.PileState = BoxState.Unused;
                pile.PileDetailList.ForEach(p => p.PersistenceStatus = PersistenceStatus.Deleted);
                CreatePileLog(pile, PileOpType.Upate);
                RF.Save(pile);
                if (pile.TurnoverContainer)
                {
                    DB.Update<TurnoverBox>().Set(p => p.State, BoxState.Unused).Where(p => p.Code == pile.Code).Execute();
                }
                tran.Complete();
            }
        }

        /// <summary>
        /// 创建垛日志
        /// </summary>
        /// <param name="pile">垛表信息</param>
        /// <param name="opType">操作类型</param>
        /// <param name="isSave">是否立即保存</param>
        /// <param name="boxState">垛状态</param>
        /// <param name="itemState">物料状态</param>
        /// <param name="data">垛数据</param>
        /// <param name="businessType">业务类型</param>
        public virtual PileLog CreatePileLog(Pile pile, PileOpType opType, bool isSave = true, BoxState? boxState = null, ItemState? itemState = null, PileData data = null, BusinessType? businessType = null)
        {
            if (pile == null)
            {
                return new PileLog();
            }
            var log = new PileLog();
            log.PileCode = pile.Code;
            log.PileOpType = opType;
            log.BillNo = data != null ? data.BillNo : pile.BillNo;
            log.BusinessType = businessType.HasValue ? businessType : pile.BusinessType;
            log.CurLocation = pile.CurLocation;
            log.Weight = pile.Weight;
            log.Length = pile.Length;
            log.Width = pile.Width;
            log.Height = pile.Height;
            if (itemState.HasValue)
            {
                log.ItemState = itemState;
            }
            else
            {
                log.ItemState = pile.ItemState;
            }

            if (!itemState.HasValue && boxState == BoxState.Unused)
            {
                log.ItemState = null;
            }

            log.PileState = boxState.HasValue ? boxState.Value : pile.PileState;

            if (isSave)
            {
                RF.Save(log);
            }

            return log;
        }

        /// <summary>
        /// 创建垛日志
        /// </summary>
        /// <param name="piles">垛表信息</param>
        /// <param name="opType">操作类型</param>
        /// <param name="boxState">垛状态</param>
        /// <param name="itemState">物料状态</param>
        /// <param name="data">垛数据</param>
        /// <param name="businessType">业务类型</param>
        public virtual void CreatePileLogs(List<Pile> piles, PileOpType opType, BoxState? boxState = null, ItemState? itemState = null, PileData data = null, BusinessType? businessType = null)
        {
            if (piles == null)
            {
                return;
            }
            var logs = new EntityList<PileLog>();
            foreach (var pile in piles)
            {
                logs.Add(CreatePileLog(pile, opType, false, boxState, itemState, data, businessType));
            }

            if (logs.Any())
            {
                RT.Service.Resolve<SIE.Core.Common.Controllers.CommonController>().BatchInsertSave(logs);
            }
        }

        /// <summary>
        /// 通过垛ID查找明细
        /// </summary>
        /// <param name="PileId">垛ID</param>
        /// <param name="itemState">物料状态</param>
        /// <returns>垛明细</returns>
        public virtual EntityList<PileDetail> GetPileDetailsByPileId(double PileId, ItemState itemState)
        {
            return Query<PileDetail>().Where(p => p.PileId == PileId && p.ItemState == itemState).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 通过垛ID查找明细
        /// </summary>
        /// <param name="PileId">垛ID</param>      
        /// <returns>垛明细</returns>
        public virtual EntityList<PileDetail> GetPileDetailsByPileId(double PileId)
        {
            return Query<PileDetail>().Where(p => p.PileId == PileId).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取明细数据
        /// </summary>
        /// <param name="sn">条码</param>
        /// <param name="itemState">物料状态</param>
        /// <returns>垛明细</returns>
        public virtual EntityList<PileDetail> GetPileDetailsBySn(string sn, ItemState itemState)
        {
            return Query<PileDetail>().Where(p => p.Sn == sn && p.ItemState == itemState).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取明细数据
        /// </summary>
        /// <param name="topSns">条码</param>
        /// <returns>垛明细</returns>
        public virtual EntityList<PileDetail> GetPileDetailsBySn(List<string> topSns)
        {
            return topSns.SplitContains(sns =>
            {
                return Query<PileDetail>().Where(p => sns.Contains(p.Sn)).ToList();
            });
        }

        /// <summary>
        /// 收货删除创建状态的明细
        /// </summary>
        /// <param name="pileIds">垛号信息</param>
        public virtual void DeletePileDetails(List<double> pileIds)
        {
            var query = DB.Delete<PileDetail>().Where(p => pileIds.Contains(p.PileId) && p.ItemState == ItemState.Create);
            query.Execute();
        }

        /// <summary>
        /// 根据条码获取垛表明细条码
        /// </summary>
        /// <param name="sns">条码</param>
        /// <returns>明细绑定条码</returns>
        public virtual List<string> GetPileDtlSnsBySn(List<string> sns)
        {
            return DataProcessEx.SplitContains(sns, tempSns =>
             {
                 var query = Query<PileDetail>().Where(p => tempSns.Contains(p.Sn));
                 var list = query.ToList(null, null);
                 return list.Select(p => p.Sn).Distinct().ToList();
             });
        }

        /// <summary>
        /// 根据条码获取垛表明细行
        /// </summary>
        /// <param name="sn">条码</param>
        /// <returns>明细行数</returns>
        public virtual int GetPileDtlCountBySn(string sn)
        {
            var query = Query<PileDetail>().Where(p => p.Sn == sn);
            return query.Count();
        }

        /// <summary>
        /// 根据条码获取垛表明细行
        /// </summary>
        /// <param name="sns">条码</param>
        /// <returns>明细行数</returns>
        public virtual int GetPileDtlCountBySn(List<string> sns)
        {
            int count = 0;
            sns.SplitDataExecute(tempSns =>
            {
                var query = Query<PileDetail>().Where(p => tempSns.Contains(p.Sn));
                count += query.Count();
            });
            return count;
        }

        /// <summary>
        /// 根据垛ID查找垛
        /// </summary>
        /// <param name="pileId">垛ID</param>
        public virtual Pile GetPileByPileID(double pileId)
        {
            return Query<Pile>().Where(p => p.Id == pileId).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 校验目标LPN
        /// </summary>
        /// <param name="lpn">lpn</param>
        /// <param name="orderNo">发运单号</param>
        public virtual void ValidatePileBillNo(string lpn, string orderNo)
        {
            var pile = RT.Service.Resolve<PileController>().GetPile(lpn);
            ValidatePileBillNo(pile, orderNo);
        }

        /// <summary>
        /// 立库拣货-校验目标LPN
        /// </summary>
        /// <param name="pile">垛号</param>
        /// <param name="orderNo"></param>
        /// <exception cref="ValidationException"></exception>
        public virtual void ValidatePileBillNo(Pile pile, string orderNo)
        {
            if (pile == null)
            {
                throw new ValidationException("垛不存在".L10N());
            }
            if (pile.PileState == BoxState.Unused && pile.ItemState == null || pile.PileState == BoxState.Inuse && pile.ItemState == ItemState.InStore)
            {
                if (pile.BillNo.IsNotEmpty() && pile.BillNo != orderNo)
                {
                    throw new ValidationException("垛[{0}]单据号与来源单据号[{1}]不一致".L10nFormat(pile.BillNo, orderNo));
                }
            }
            else
            {
                throw new ValidationException("垛[{0}]不可用".L10N());
            }
        }

        /// <summary>
        /// 根据条码验证并获取垛表明细条码
        /// </summary>
        /// <param name="sns">条码</param>
        /// <returns>明细绑定条码</returns>
        /// <exception cref="ValidationException">条码的最上级包装[{0}]在垛表中已绑定LPN</exception>
        public virtual void ValidatePileDtlSns(List<string> sns)
        {
            var pileSns = GetPileDtlCountBySn(sns);
            if (pileSns > 0)
            {
                throw new ValidationException("条码的最上级包装[{0}]在垛表中已绑定LPN".L10nFormat(string.Join(",", sns)));
            }
        }

        /// <summary>
        /// 验证垛型号与来源型号是否一致
        /// </summary>
        /// <param name="lpn">垛号</param>
        /// <param name="validateModelCode">验证型号</param>
        /// <returns>True:型号一致 False:型号不一致</returns>
        public virtual bool ValidatePileModel(string lpn, string validateModelCode)
        {
            var pile = GetPile(lpn);
            if (pile == null)
            {
                return false;
            }

            if (!pile.Model.Equals(validateModelCode))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 验证垛号数据
        /// </summary>
        /// <param name="lpn">垛号</param>
        /// <exception cref="ValidationException">异常信息</exception>
        public virtual Pile ValidatePileData(string lpn)
        {
            if (lpn.IsNullOrEmpty() || lpn == "*")
            {
                return null;
            }

            string errMsg = "垛号:[{0}]不存在垛表中".L10nFormat(lpn);
            Pile pile = GetPile(lpn);
            if (pile == null)
            {
                throw new ValidationException(errMsg);
            }

            if (!(pile.PileState == BoxState.Unused || (pile.PileState == BoxState.Inuse && pile.ItemState == ItemState.InStore)))
            {
                errMsg = "垛号:[{0}]在垛表的状态必须是闲置，或者垛状态为使用中且物料状态为在库".L10nFormat(lpn);
                throw new ValidationException(errMsg);
            }

            return pile;
        }

        /// <summary>
        /// 批量验证垛号数据
        /// </summary>
        /// <param name="lpns">垛号集合</param>
        /// <returns>垛号</returns>
        /// <exception cref="ValidationException">异常信息</exception>
        public virtual void ValidateBatchPileData(List<string> lpns)
        {
            if (!lpns.Any(f => f != "*"))
            {
                return;
            }

            var piles = GetPileStateData(lpns);
            if (!piles.Any())
            {
                var errMsg = "垛号:[{0}]不存在垛表中".L10nFormat(string.Join(",", lpns));
                throw new ValidationException(errMsg);
            }

            if (!piles.All(p => p.PileState == BoxState.Unused || (p.PileState == BoxState.Inuse && p.ItemState == ItemState.InStore)))
            {
                List<string> codes = piles.Where(p => !(p.PileState == BoxState.Unused || (p.PileState == BoxState.Inuse && p.ItemState == ItemState.InStore))).Select(p => p.Code).ToList();
                var errMsg = "垛号:[{0}]在垛表的状态必须是闲置，或者垛状态为使用中且物料状态为在库".L10nFormat(string.Join(",", codes));
                throw new ValidationException(errMsg);
            }
        }

        /// <summary>
        /// 验证垛状态
        /// </summary>
        /// <param name="pile">垛表</param>
        /// <exception cref="ValidationException"></exception>
        public virtual void ValidatePileState(Pile pile)
        {
            if (pile == null)
            {
                return;
            }
            ////在垛表的状态为闲置
            ////如果是周转容器，进一步验证LPN在周转箱模块的状态为闲置
            if (pile.PileState != BoxState.Unused && pile.PileState != BoxState.Inuse)
            {
                throw new ValidationException("LPN[{0}]不是闲置或使用中状态".L10nFormat(pile.Code));
            }
        }

        /// <summary>
        /// 根据任务操作类型，获取垛表业务类型
        /// </summary>
        /// <param name="operationType"></param>
        /// <returns></returns>
        public virtual BusinessType? GetBusinessTypeByTaskType(OperationType operationType)
        {
            BusinessType? businessType = null;
            switch (operationType)
            {
                case OperationType.PutOn:
                    businessType = BusinessType.InStorage;
                    break;
                case OperationType.PullOff:
                case OperationType.Pick:
                case OperationType.PickBack:
                    businessType = BusinessType.OutStorage;
                    break;
                case OperationType.Move:
                    businessType = BusinessType.Move;
                    break;
                case OperationType.Allot:
                case OperationType.AllotPick:
                    businessType = BusinessType.Allocate;
                    break;
                case OperationType.Check:
                case OperationType.CheckBack:
                    businessType = BusinessType.Count;
                    break;
                case OperationType.Carry:
                    businessType = BusinessType.Carry;
                    break;
                default:
                    break;
            }

            return businessType;
        }
    }
}

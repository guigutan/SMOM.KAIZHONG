using Newtonsoft.Json;
using SIE.Common;
using SIE.Common.Configs;
using SIE.Common.Configs.CommonConfigs;
using SIE.Common.NumberRules;
using SIE.Domain;
using SIE.Domain.Query;
using SIE.Domain.Validation;
using SIE.EMS.SpareParts.ApiModels;
using SIE.EMS.SpareParts.Applys;
using SIE.EMS.SpareParts.Applys.Controllers;
using SIE.EMS.SpareParts.Applys.Details;
using SIE.EMS.SpareParts.OutDepotHandovers;
using SIE.EMS.SpareParts.OutDepots.Criterias;
using SIE.EMS.SpareParts.OutDepots.Details;
using SIE.EMS.SpareParts.OutDepots.Enums;
using SIE.EMS.SpareParts.OutDepots.ViewModels;
using SIE.Equipments.Configs;
using SIE.Equipments.Enums;
using SIE.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.EMS.SpareParts.OutDepots.Controllers
{
    /// <summary>
    /// 备件出库单控制器
    /// </summary>
    public class OutDepotController : DomainController
    {
        #region Criteria左侧查询视图查询条件查询功能
        /// <summary>
        /// 根据查询器查询
        /// </summary>
        /// <param name="criteria">备件出库查询视图</param>
        /// <returns>备件出库集合</returns>
        public virtual EntityList<OutDepot> GetOutDepotList(OutDepotCriteria criteria)
        {
            var query = DB.Query<OutDepot>();

            if (criteria.No.IsNotEmpty())
            {
                query.Where(p => p.No.Contains(criteria.No));
            }
            if (criteria.OutDepotType != null)
            {
                query.Where(p => p.OutDepotType == criteria.OutDepotType);
            }
            if (criteria.OutDepotState != null)
            {
                query.Where(p => p.OutDepotState == criteria.OutDepotState);
            }
            if (criteria.QualityStatus != null)
            {
                query.Where(p => p.QualityStatus == criteria.QualityStatus);
            }
            if (criteria.SourceNo.IsNotEmpty())
            {
                query.Where(p => p.SourceNo.Contains(criteria.SourceNo));
            }
            if (criteria.ReleDoc.IsNotEmpty())
            {
                query.Where(p => p.ReleDoc.Contains(criteria.ReleDoc));
            }
            if (criteria.GetDepartment != null)
            {
                query.Where(p => p.GetDepartmentId == criteria.GetDepartmentId);
            }
            if (criteria.Warehouse != null)
            {
                query.Where(p => p.WarehouseId == criteria.WarehouseId);
            }

            if (criteria.CreateDate.BeginValue.HasValue)
            {
                query.Where(p => p.CreateDate >= criteria.CreateDate.BeginValue);
            }
            if (criteria.CreateDate.EndValue.HasValue)
            {
                query.Where(p => p.CreateDate <= criteria.CreateDate.EndValue);
            }


            EntityList<OutDepot> list;


            EagerLoadOptions eagerLoad = new EagerLoadOptions().LoadWithViewProperty()
                                  .LoadWith(OutDepot.OutDepotDetailListProperty);
            if (criteria.SparePartId.HasValue || criteria.SparePartName.IsNotEmpty())
            {
                IQuery iquery = query.ToQuery();

                //主表ID
                var equipAcountIdColumn = iquery.MainTable.FindColumn(OutDepot.IdProperty);

                var f = QueryFactory.Instance;

                //设备台账子查询
                var subqueryOfOutDepotDetail = CreateSubQueryOfOutDepotDetail(equipAcountIdColumn, f, criteria.SparePartId, criteria.SparePartName);
                var subqueryOfPartOutDepotDetail = CreateSubQueryOfPartOutDepotDetail(equipAcountIdColumn, f, criteria.SparePartId, criteria.SparePartName);

                iquery.Where = f.And(iquery.Where, f.Or(f.Exists(subqueryOfOutDepotDetail), f.Exists(subqueryOfPartOutDepotDetail)));
                list = (EntityList<OutDepot>)query.Repository.QueryList(iquery, criteria.PagingInfo, eagerLoad: eagerLoad);
            }
            else
            {
                list = query.OrderBy(criteria.OrderInfoList)
                  .ToList(criteria.PagingInfo, eagerLoad);
            }

            foreach (var outDepot in list)
            {
                int requireSumCount = outDepot.OutDepotDetailList.Select(p => p.RequireCount).Sum();
                int pickedSumCount = outDepot.OutDepotDetailList.Select(p => p.PickedCount).Sum();
                outDepot.IsNeedPick = requireSumCount > pickedSumCount;
            }
            return list;
        }

        private static IQuery CreateSubQueryOfOutDepotDetail(IColumnNode idColumn, QueryFactory f, double? sparePartId, String sparePartName)
        {
            var query = DB.Query<OutDepotDetail>("odd")
                .Join<SparePart>("sp1", (odd, sp1) => (odd.SparePartId == sp1.Id));

            if (sparePartId.HasValue)
            {
                query.Where<SparePart>((odd, sp1) => sp1.Id == sparePartId);
            }

            if (sparePartName.IsNotEmpty())
            {
                query.Where<SparePart>((odd, sp1) => sp1.SparePartName.Contains(sparePartName));
            }

            var subQuery = query.ToQuery();

            var dataRepo = RF.Find<OutDepotDetail>();
            var _mainTable = f.Table(dataRepo, "odd");
            IColumnNode mainIdColumn = _mainTable.Column(OutDepotDetail.OutDepotIdProperty);

            subQuery.Where = subQuery.Where.And(f.Constraint(mainIdColumn, idColumn));

            var isPhantomColumn = subQuery.MainTable.FindColumn(PhantomEntityExtension.IS_PHANTOMProperty);

            if (isPhantomColumn != null)
            {
                subQuery.Where = f.And(subQuery.Where, isPhantomColumn.Equal(BooleanBoxes.False));
            }

            return subQuery;
        }


        private static IQuery CreateSubQueryOfPartOutDepotDetail(IColumnNode idColumn, QueryFactory f, double? sparePartId, String sparePartName)
        {
            var query = DB.Query<PartOutDepotDetail>("podd")
                .Join<SparePart>("sp1", (odd, sp1) => (odd.SparePartId == sp1.Id));

            if (sparePartId.HasValue)
            {
                query.Where<SparePart>((odd, sp1) => sp1.Id == sparePartId);
            }

            if (sparePartName.IsNotEmpty())
            {
                query.Where<SparePart>((odd, sp1) => sp1.SparePartName.Contains(sparePartName));
            }

            var subQuery = query.ToQuery();

            var dataRepo = RF.Find<PartOutDepotDetail>();
            var _mainTable = f.Table(dataRepo, "podd");
            IColumnNode mainIdColumn = _mainTable.Column(PartOutDepotDetail.OutDepotIdProperty);

            subQuery.Where = subQuery.Where.And(f.Constraint(mainIdColumn, idColumn));

            var isPhantomColumn = subQuery.MainTable.FindColumn(PhantomEntityExtension.IS_PHANTOMProperty);

            if (isPhantomColumn != null)
            {
                subQuery.Where = f.And(subQuery.Where, isPhantomColumn.Equal(BooleanBoxes.False));
            }

            return subQuery;
        }
        #endregion

        #region 备件出库编号生成规则
        /// <summary>
        /// 获取自动生成备件出库编号No
        /// </summary>
        /// <returns>编号</returns>
        public virtual string GetNo()
        {
            //业务逻辑代码，此处生成自动编号
            var config = ConfigService.GetConfig<NoConfigValue>(new NoConfig(), typeof(OutDepot));
            if (config == null || config.BacodeRule == null)
                throw new ValidationException("未找到备件出库单号生成规则,请检查规则配置".L10N());
            var code = RT.Service.Resolve<NumberRuleController>().GenerateSegment(config.BacodeRule.Id, 1).FirstOrDefault();

            return code;
        }
        #endregion

        #region 备件交接编号生成规则
        /// <summary>
        /// 获取自动生成备件交接编号No
        /// </summary>
        /// <returns>编号</returns>
        public virtual string GetOutDepotHandoverNo()
        {
            //业务逻辑代码，此处生成自动编号
            var config = ConfigService.GetConfig<NoConfigValue>(new NoConfig(), typeof(OutDepotHandover));
            if (config == null || config.BacodeRule == null)
                throw new ValidationException("未找到备件交接单号生成规则,请检查规则配置".L10N());
            var code = RT.Service.Resolve<NumberRuleController>().GenerateSegment(config.BacodeRule.Id, 1).FirstOrDefault();

            return code;
        }
        #endregion

        #region 获取单条数据


        //有序列号的查找仓库

        //无序列号的查找仓库


        /// <summary>
        /// 获取库位
        /// </summary>
        /// <param name="batchNoRefId"></param>
        /// <returns>库位</returns>
        public virtual string GetOutLocal(double batchNoRefId)
        {
            var storeSummaryDepot = RF.GetById<StoreSummaryLot>(batchNoRefId, new EagerLoadOptions().LoadWithViewProperty());

            if (storeSummaryDepot == null)
            {
                return null;
            }

            var storesummary = DB.Query<StoreDetail>().Where(p => p.BatchNumber.Contains(storeSummaryDepot.BatchNumber)).FirstOrDefault();

            var result = new
            {
                OutLocal = storeSummaryDepot?.StorageLocation,
                SparePartSite = storeSummaryDepot?.StorageLocation,
                OutLocalCount = storeSummaryDepot?.GoodNumber,
                UnitPrice = storesummary?.UnitPrice,
                DepotPartCount = storeSummaryDepot?.GoodNumber
            };
            return JsonConvert.SerializeObject(result);
        }
        #endregion

        #region 获取备件出库单的相关‘集合’包括（备件，型号，仓库，库位）

        /// <summary>
        /// 根据备件出库单备件申请明细Id筛选备件
        /// </summary>
        /// <param name="releDoc">申请单号</param>
        /// <param name="modelId">设备型号ID</param>
        /// <param name="pagingInfo">分页实体</param>
        /// <returns></returns>
        public virtual EntityList<SparePart> GetSpareParts(string releDoc, double? modelId, PagingInfo pagingInfo)
        {
            //查询出库单，备件申请明细的备件
            var list = Query<SparePart>()
                .Where(p => p.State == State.Enable)
                .WhereIf(modelId != null, p => p.SpartEquipModelId == null || p.SpartEquipModelId == modelId)
                .Exists<ApplyDetail>((x, y) => y.Where(p => p.SparePartApp.No == releDoc && x.Id == p.SparePartId))
                .ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());

            //当没有出库申请明细，或明细对应备件备禁用时，查询所有可用备件
            if (list.Count == 0)
                list = Query<SparePart>().Where(p => p.State == State.Enable)
                    .WhereIf(modelId != null, p => p.SpartEquipModelId == null || p.SpartEquipModelId == modelId)
                    .ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());

            return list;
        }


        /// <summary>
        /// 根据申请单明细获取批次号集合
        /// </summary>
        /// <param name="partOutDepotDetail"></param>
        /// <returns></returns>
        public virtual EntityList<StoreSummaryLot> GetSummaryBacthByDetail(PartOutDepotDetail partOutDepotDetail, PagingInfo pagingInfo)
        {
            var queryer = DB.Query<StoreSummaryLot>()
                      .Join<StoreSummary>((d, s) => s.Id == d.StoreSummaryId &&
                      s.SparePartId == partOutDepotDetail.SparePartId)
                      .Where(p => p.WarehouseId == partOutDepotDetail.OutDepot.WarehouseId
                       && p.GoodNumber > 0);


            var storeSummaryDetails = queryer.OrderBy(p => p.CreateDate)
                   .ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
            return storeSummaryDetails;
        }


        /// <summary>
        /// 根据申请单明细获取序列号集合
        /// </summary>
        /// <param name="partOutDepotDetail"></param>
        /// <returns></returns>
        public virtual EntityList<StoreSummaryDetail> GetSummarySeriaByDetail(PartOutDepotDetail partOutDepotDetail, PagingInfo pagingInfo)
        {
            partOutDepotDetail.LoadProperty(PartOutDepotDetail.BatchNoRefProperty, partOutDepotDetail.BatchNoRef);

            var queryer = DB.Query<StoreSummaryDetail>().Where(p => p.StoreStatus == OrdNumStoreStatus.In);

            if (partOutDepotDetail.OutDepot.WarehouseId != 0)
            {
                queryer.Where(p => p.WarehouseId == partOutDepotDetail.OutDepot.WarehouseId);
            }

            var outDepot = RF.GetById<OutDepot>(partOutDepotDetail.OutDepotId);
            //如果存在点检保养维修 出库则是不良品
            if (outDepot != null && (outDepot.OutDepotType == OutDepotType.Check || outDepot.OutDepotType == OutDepotType.Maintain || outDepot.OutDepotType == OutDepotType.Repair))
            {
                queryer.Where(p => p.OdNbStatus == OdNbStatus.GoodProduct);
            }
            queryer.Join<StoreSummary>((d, s) => s.Id == d.StoreSummaryId &&
                     s.SparePartId == partOutDepotDetail.SparePartId);

            var storeSummaryDetails = queryer.OrderBy(p => p.CreateDate)
                     .ToList(pagingInfo, null);
            return storeSummaryDetails;
        }

        #endregion

        #region 删除按钮
        /// <summary>
        /// 删除出库单
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public virtual int DeleteOut(List<double> ids)
        {
            int count = 0;
            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                DB.Delete<OutDepotDetail>().Where(s => ids.Contains(s.OutDepotId)).Execute();
                DB.Delete<SerializeNo>().Where(s => ids.Contains(s.OutDepotId)).Execute();
                DB.Delete<SupplierInfo>().Where(s => ids.Contains(s.OutDepotId)).Execute();
                DB.Delete<PartOutDepotDetail>().Where(s => ids.Contains(s.OutDepotId)).Execute();

                count = DB.Delete<OutDepot>().Where(s => ids.Contains(s.Id)).Execute();
                trans.Complete();
            }
            return count;
        }

        /// <summary>
        /// 删除申请单明细
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public virtual int DeleteOutDetail(List<double> ids)
        {
            int count =
                DB.Delete<OutDepotDetail>().Where(s => ids.Contains(s.Id)).Execute();
            return count;
        }
        /// <summary>
        /// 删除供应商
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public virtual int DeleteSupp(List<double> ids)
        {
            int count =
                DB.Delete<SupplierInfo>().Where(s => ids.Contains(s.Id)).Execute();
            return count;
        }
        /// <summary>
        /// 删除序列号
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public virtual int DeleteOutSeria(List<double> ids)
        {
            int
            count = DB.Delete<SerializeNo>().Where(s => ids.Contains(s.Id)).Execute();
            return count;
        }

        /// <summary>
        /// 删除出库明细
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public virtual int DeletePartOut(List<double> ids)
        {
            int
            count = DB.Delete<PartOutDepotDetail>().Where(s => ids.Contains(s.Id)).Execute();
            return count;
        }
        #endregion

        #region 保存按钮
        /// <summary>
        /// 保存出库单
        /// </summary>
        /// <param name="outDepot">出库单</param>
        public virtual void SaveOutDepot(OutDepot outDepot)
        {
            //outDepot.LoadProperty(OutDepot.PartOutDepotDetailListProperty, outDepot.PartOutDepotDetailList);
            //if (outDepot.PartOutDepotDetailList.Count == 0)
            //{
            //    throw new ValidationException("至少需要一条出库明细");
            //}

            //foreach (var item in outDepot.OutDepotDetailList)
            //{
            //    int sum = 0;
            //    var sparePartOutList = outDepot.PartOutDepotDetailList.Where(p => p.SparePartId == item.SparePartId).ToList();
            //    foreach (var ite in sparePartOutList)
            //    {
            //        sum += ite.OutDepotCount;
            //    }
            //    item.OutDepotCount = sum;

            //}
            if (outDepot.OutDepotType == OutDepotType.DgMaintain && outDepot.SupplierInfoList.Count == 0)
            {
                throw new ValidationException("该出货单是委托，需要至少一条供应商信息".L10N());
            }
            RF.Save(outDepot);
        }
        #endregion

        #region 出库
        /// <summary>
        /// 出库
        /// </summary>
        /// <param name="outDepot">出库单</param>
        public virtual void OutDepotComp(OutDepot outDepot)
        {
            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                //行锁
                DB.Update<OutDepot>().Where(p => p.Id == outDepot.Id).Set(p => p.UpdateBy, RT.IdentityId).Execute();
                //查询申请单数据
                var elo = new EagerLoadOptions().LoadWith(ApplyDetail.SparePartProperty);
                var appDetailList = Query<ApplyDetail>().Where(p => p.SparePartApp.No == outDepot.ReleDoc).ToList(null, elo);

                //校验
                if (outDepot.PartOutDepotDetailList.Count == 0)
                    throw new ValidationException("至少需要一条出货明细".L10N());
                if (outDepot.PartOutDepotDetailList.Any(p => p.OutDepotCount <= 0))
                    throw new ValidationException("出库数量必须大于0".L10N());
                if (outDepot.PartOutDepotDetailList.Any(p => p.SparePart.ControlMethod == SpareParts.Enums.ControlMethod.Sn && p.SeriaNo.IsNullOrEmpty()))
                    throw new ValidationException("存在序列号管控的备件项，没有选择序列号".L10N());
                var seriaNos = outDepot.PartOutDepotDetailList.Where(p => p.SeriaNo.IsNotEmpty()).Select(p => p.SeriaNo).ToList();
                if (seriaNos.Count != seriaNos.Distinct().Count())
                    throw new ValidationException("存在相同的出库序列号".L10N());
                appDetailList.ForEach(p =>
                {
                    var outDtls = outDepot.PartOutDepotDetailList.Where(x => x.SparePartId == p.SparePartId).ToList();
                    var outCount = outDtls.Sum(x => x.OutDepotCount);
                    if (outCount > p.ApplyAmount)
                    {
                        throw new ValidationException("备件[{0}]出库数量不能大于申请数量".L10nFormat(p.SparePart.SparePartCode));
                    }
                });

                //更新库存数量
                foreach (var item in outDepot.PartOutDepotDetailList)
                {
                    if (item.SparePart.ControlMethod == SpareParts.Enums.ControlMethod.Sn)
                    {
                        UpdatePartDepotBySeriaBatch(item);
                    }
                    else
                    {
                        UpdatePartDepotByBatch(item);
                    }
                }
                //更新相关单据的审核状态
                if (outDepot.ReleDoc.IsNotEmpty())
                {
                    SparePartApp spApp = DB.Query<SparePartApp>().Where(p => p.No.Contains(outDepot.ReleDoc)).FirstOrDefault(new EagerLoadOptions().LoadWith(SparePartApp.ApplyDetailListProperty));
                    if (spApp == null)
                    {
                        throw new ValidationException("相关单据不存在，可能数据有问题".L10N());
                    }
                    //回写数据
                    foreach (var detail in spApp.ApplyDetailList)
                    {
                        var partOutDepotDetails = outDepot.PartOutDepotDetailList.Where(p => p.SparePartId == detail.SparePartId).ToList();
                        var partOutDepotSumCount = partOutDepotDetails.Sum(p => p.OutDepotCount);
                        DB.Update<ApplyDetail>().Where(p => p.Id == detail.Id).Set(p => p.OutDepotAmount, partOutDepotSumCount).Execute();
                    }

                    //修改状态
                    DB.Update<SparePartApp>().Where(P => P.Id == spApp.Id).Set(p => p.AuditState, Applys.Enums.AuditState.Butbounded).Execute();
                }

                //更新出库单
                outDepot.OutDepotState = OutDepotState.Ed;
                outDepot.OutDepotDate = DateTime.Now;
                RF.Save(outDepot);

                trans.Complete();
            }
        }
        #endregion

        #region 更新库存
        /// <summary>
        /// 序列号备件更新库存 根据批次号和序列号更新库存数量包含（备件库存，仓库库存，总库存）
        /// </summary>
        /// <param name="partOutDepotDetail">出库单明细</param>
        public virtual void UpdatePartDepotBySeriaBatch(PartOutDepotDetail partOutDepotDetail)
        {
            partOutDepotDetail.LoadProperty(PartOutDepotDetail.BatchNoRefProperty, partOutDepotDetail.BatchNoRef);
            //if (partOutDepotDetail.IsSerializeCtrView)
            //{
            //行锁
            DB.Update<StoreSummaryDetail>().Where(p => p.OrderNumberCode.Contains(partOutDepotDetail.SeriaNo)).Set(p => p.UpdateBy, RT.IdentityId).Execute();
            DB.Update<StoreSummaryLot>().Where(p => p.BatchNumber.Contains(partOutDepotDetail.BatchNoRef.BatchNumber)).Set(p => p.UpdateBy, RT.IdentityId).Execute();

            //获取仓库明细(行锁后优先查询，为了获取主数据ID)
            var storeSummaryDepot = DB.Query<StoreSummaryLot>()
                .Where(p => p.BatchNumber.Contains(partOutDepotDetail.BatchNoRef.BatchNumber)).FirstOrDefault();

            //行锁
            DB.Update<StoreSummary>().Where(p => p.Id == storeSummaryDepot.StoreSummaryId).Set(p => p.UpdateBy, RT.IdentityId).Execute();

            //获取库存明细和备件库存
            var storeSummaryDetail = DB.Query<StoreSummaryDetail>()
                .Where(p => p.OrderNumberCode.Contains(partOutDepotDetail.SeriaNo)).FirstOrDefault();
            var storeSummary = RF.GetById<StoreSummary>(storeSummaryDepot.StoreSummaryId);

            //校验
            if (storeSummaryDetail.StoreStatus != OrdNumStoreStatus.In)
                throw new ValidationException("该备件已经出库了".L10N());
            if (storeSummaryDepot.GoodNumber <= 0)
                throw new ValidationException("库存数不足，请选择其他的".L10N());
            if (storeSummaryDetail.Number < partOutDepotDetail.OutDepotCount)
                throw new ValidationException("序列号{0}的备件库存数量不够".L10nFormat(partOutDepotDetail.SeriaNo));
            else if (storeSummaryDepot.SumNumber < partOutDepotDetail.OutDepotCount)
                throw new ValidationException("备件明细总库存少于出库数量".L10N());
            else if (storeSummary.GoodNumber < partOutDepotDetail.OutDepotCount)
                throw new ValidationException("序列号{0}的备件库存数量不够,但是可能是数据错乱".L10nFormat(partOutDepotDetail.SeriaNo));
            else if (storeSummary.SumNumber < partOutDepotDetail.OutDepotCount)
                throw new ValidationException("备件库存总数量少于出库数量".L10N());


            DB.Update<StoreSummaryDetail>().Where(p => p.Id == storeSummaryDetail.Id).Set(p => p.StoreStatus, OrdNumStoreStatus.Out).Execute();
            DB.Update<StoreSummaryLot>().Where(p => p.Id == storeSummaryDepot.Id)
                .Set(p => p.GoodNumber, p => p.GoodNumber - partOutDepotDetail.OutDepotCount)
                .Set(p => p.SumNumber, p => p.SumNumber - partOutDepotDetail.OutDepotCount)
                .Execute();
            DB.Update<StoreSummary>().Where(p => p.Id == storeSummary.Id)
                .Set(p => p.GoodNumber, p => p.GoodNumber - partOutDepotDetail.OutDepotCount)
                .Set(p => p.SumNumber, p => p.SumNumber - partOutDepotDetail.OutDepotCount)
                .Execute();
            //}
        }

        /// <summary>
        /// 批次备件更新库存 根据批次号更新库存数量包含（备件库存，仓库库存，总库存）
        /// </summary>
        /// <param name="partOutDepotDetail">出库单明细</param>
        public virtual void UpdatePartDepotByBatch(PartOutDepotDetail partOutDepotDetail)
        {
            partOutDepotDetail.LoadProperty(PartOutDepotDetail.BatchNoRefProperty, partOutDepotDetail.BatchNoRef);

            //行锁
            DB.Update<StoreSummaryLot>().Where(p => p.BatchNumber.Contains(partOutDepotDetail.BatchNoRef.BatchNumber)).Set(p => p.UpdateBy, RT.IdentityId).Execute();
            //获取仓库明细(行锁后优先查询，为了获取主数据ID)
            var storeSummaryDepot = DB.Query<StoreSummaryLot>()
                .Where(p => p.BatchNumber.Contains(partOutDepotDetail.BatchNoRef.BatchNumber)).FirstOrDefault();

            //行锁
            DB.Update<StoreSummary>().Where(p => p.Id == storeSummaryDepot.StoreSummaryId).Set(p => p.UpdateBy, RT.IdentityId).Execute();
            //获取库存明细和备件库存
            var storeSummary = RF.GetById<StoreSummary>(storeSummaryDepot.StoreSummaryId);

            if (storeSummaryDepot.GoodNumber <= 0)
                throw new ValidationException("库存数为0，请选择其他批次".L10N());
            //检查库存数量
            if (storeSummaryDepot.GoodNumber < partOutDepotDetail.OutDepotCount)
                throw new ValidationException("序列号{0}的备件库存数量不够,但是可能是数据错乱".L10nFormat(partOutDepotDetail.SeriaNo));
            else if (storeSummaryDepot.SumNumber < partOutDepotDetail.OutDepotCount)
                throw new ValidationException("未知问题，可能是数据错乱".L10N());
            else if (storeSummary.GoodNumber < partOutDepotDetail.OutDepotCount)
                throw new ValidationException("序列号{0}的备件库存数量不够,但是可能是数据错乱".L10nFormat(partOutDepotDetail.SeriaNo));
            else if (storeSummary.SumNumber < partOutDepotDetail.OutDepotCount)
                throw new ValidationException("未知问题，可能是数据错乱".L10N());

            DB.Update<StoreSummaryLot>().Where(p => p.Id == storeSummaryDepot.Id)
                .Set(p => p.GoodNumber, p => p.GoodNumber - partOutDepotDetail.OutDepotCount)
                .Set(p => p.SumNumber, p => p.SumNumber - partOutDepotDetail.OutDepotCount)
                .Execute();
            DB.Update<StoreSummary>().Where(p => p.Id == storeSummary.Id)
                .Set(p => p.GoodNumber, p => p.GoodNumber - partOutDepotDetail.OutDepotCount)
                .Set(p => p.SumNumber, p => p.SumNumber - partOutDepotDetail.OutDepotCount)
                .Execute();
        }
        #endregion

        /// <summary>
        /// 是否生成交接单
        /// </summary>
        /// <returns>bool</returns>
        public virtual bool IsCreateHandoverBill()
        {
            var config = ConfigService.GetConfig(new IsCreateHandoverBillConfig());
            if (config == null)
                throw new ValidationException("未找到是否生成交接单的配置,请检查配置项".L10N());
            return config.IsCreateHandoverBill;
        }

        /// <summary>
        ///  获取备件申请明细
        /// </summary>
        ///  <param name="sortInfo">排序信息</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <param name="entity">出库单实体</param>
        /// <returns>备件申请明细列表</returns>
        public virtual IList<OutDepotDetail> GetOutDepotDetailList(IList<OrderInfo> sortInfo, PagingInfo pagingInfo, OutDepot entity)
        {
            var list = Query<OutDepotDetail>().Where(p => p.OutDepotId == entity.Id)
                .OrderBy(sortInfo).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());

            list.ForEach(applyDetail =>
            {
                var storeLocList = RT.Service.Resolve<StoreSummaryController>().GetStorageLocationForOutDepot((double)entity.WarehouseId, (QualityStatus)entity.QualityStatus, applyDetail.SparePartId, applyDetail.ControlMethod);
                applyDetail.AdviceStorageLocation = string.Join("，", storeLocList.Select(p => p.Name + "(" + p.RoutewayId + ")"));
            });
            return list;
        }

        /// <summary>
        ///  获取备件出库明细
        /// </summary>
        ///  <param name="sortInfo">排序信息</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <param name="entity">出库单实体</param>
        /// <returns>备件出库明细列表</returns>
        public virtual IList<PartOutDepotDetail> GetPartOutDepotDetailList(IList<OrderInfo> sortInfo, PagingInfo pagingInfo, OutDepot entity)
        {
            var list = Query<PartOutDepotDetail>().Where(p => p.OutDepotId == entity.Id)
                .OrderBy(sortInfo).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty().LoadWith(PartOutDepotDetail.SparePartProperty).LoadWith(PartOutDepotDetail.BatchNoRefProperty));

            list.ForEach(outDepotDetail =>
            {
                outDepotDetail.SparePartCodeView = outDepotDetail.SparePart.SparePartCode;
                outDepotDetail.SparePartNameView = outDepotDetail.SparePart.SparePartName;
                outDepotDetail.ControlMethodView = outDepotDetail.SparePart.ControlMethod;
                outDepotDetail.BatchNo = outDepotDetail.BatchNoRef?.BatchNumber;
                outDepotDetail.SiteCode = outDepotDetail.StorageLocationName;
                outDepotDetail.QualityStatus = (QualityStatus)entity.QualityStatus;
            });
            return list;
        }

        /// <summary>
        /// 根据备件出库单明细ID列表，获取备件出库明细
        /// </summary>
        /// <param name="ids">备件明细ID列表</param>
        /// <returns></returns>
        public virtual EntityList<PartOutDepotDetail> GetPartOutDepotDetailList(List<double> ids)
        {
            return Query<PartOutDepotDetail>().Where(p => ids.Contains(p.Id)).ToList();
        }

        /// <summary>
        ///  获取可现场退库的出库明细
        /// </summary>
        /// <param name="pagingInfo">分页信息</param>
        /// <param name="entity">出库单实体</param>
        ///  <param name="keyword">查询关键字</param>
        /// <returns>备件出库明细列表</returns>
        public virtual EntityList<PartOutDepotDetail> GetPartOutDepotDetailListForStore(PagingInfo pagingInfo, SparePartStore entity, string keyword)
        {
            var list = Query<PartOutDepotDetail>().Where(p => p.OldReturnQty + p.ReturnQty + p.UseCount < p.OutDepotCount)
                .WhereIf(entity.SparePartId != null, p => p.SparePartId == entity.SparePartId)
                .WhereIf(keyword.IsNotEmpty(), p => p.OutDepot.No.Contains(keyword))
                .ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());

            list.ForEach(p =>
            {
                p.CanReturnQty = p.OutDepotCount - (p.OldReturnQty + p.ReturnQty + p.UseCount);
            });
            return list;
        }

        /// <summary>
        ///  获取备件接收明细
        /// </summary>
        ///  <param name="sortInfo">排序信息</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <param name="entity">出库单实体</param>
        /// <returns>备件接收明细列表</returns>
        public virtual IList<OutDepotHandoverDetail> GetOutDepotHandoverDetailList(IList<OrderInfo> sortInfo, PagingInfo pagingInfo, OutDepot entity)
        {
            var list = Query<OutDepotHandoverDetail>().Where(p => p.OutDepotHandover.OutDepotId == entity.Id)
                .WhereIf(entity.HandoverDetailKeyWord.IsNotEmpty(), p => p.SparePart.SparePartCode.Contains(entity.HandoverDetailKeyWord)
                                    || p.SparePart.SparePartName.Contains(entity.HandoverDetailKeyWord)
                                    || p.SparePart.Specification.Contains(entity.HandoverDetailKeyWord)
                                    || p.BatchNo.Contains(entity.HandoverDetailKeyWord)
                                    || p.SeriaNo.Contains(entity.HandoverDetailKeyWord))
                 .OrderBy(sortInfo).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
            return list;
        }

        /// <summary>
        ///  获取备件接收明细
        /// </summary>
        ///  <param name="sortInfo">排序信息</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <param name="entity">交接单实体</param>
        /// <returns>备件接收明细列表</returns>
        public virtual IList<OutDepotHandoverDetail> GetOutDepotHandoverDetailList(IList<OrderInfo> sortInfo, PagingInfo pagingInfo, OutDepotHandover entity)
        {
            var list = Query<OutDepotHandoverDetail>().Where(p => p.OutDepotHandoverId == entity.Id)
                .WhereIf(entity.HandoverDetailKeyWord.IsNotEmpty(), p => p.SparePart.SparePartCode.Contains(entity.HandoverDetailKeyWord)
                                    || p.SparePart.SparePartName.Contains(entity.HandoverDetailKeyWord)
                                    || p.SparePart.Specification.Contains(entity.HandoverDetailKeyWord)
                                    || p.BatchNo.Contains(entity.HandoverDetailKeyWord)
                                    || p.SeriaNo.Contains(entity.HandoverDetailKeyWord))
                .OrderBy(sortInfo).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
            return list;
        }

        /// <summary>
        /// 获取出库单申请明细里的备件信息
        /// </summary>
        /// <param name="pagingInfo">分页信息</param>
        /// <param name="outDepot">出库单头</param>
        /// <param name="keyword">查询关键字</param>
        /// <returns>备件列表</returns>
        public virtual EntityList<SparePart> GetSparePartByOutDepot(PagingInfo pagingInfo, OutDepot outDepot, string keyword)
        {
            EntityList<SparePart> list = Query<SparePart>().Join<OutDepotDetail>((a, b) => a.Id == b.SparePartId)
                                                            .Where<OutDepotDetail>((a, b) => b.OutDepotId == outDepot.Id && b.RequireCount > b.PickedCount)
                                                            .WhereIf(keyword.IsNotEmpty(), p => p.SparePartCode.Contains(keyword) || p.SparePartName.Contains(keyword))
                                                            .ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
            return list;
        }

        /// <summary>
        /// 查询已出库可用序列号
        /// </summary>
        /// <param name="no">备件申请单号</param>
        /// <param name="pagingInfo"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual EntityList<SerializeNo> GetSerializeNos(string no, PagingInfo pagingInfo, string key)
        {
            var q = Query<SerializeNo>();
            q.Exists<PartOutDepotDetail>((x, y) => y.Where(p => p.SeriaNo == x.Code && p.OutDepot.ReleDoc == no && p.OutDepot.OutDepotState == OutDepotState.Ed));
            q.Where(p => p.State == State.Enable);
            if (key.IsNotEmpty())
            {
                q.Where(p => p.Code.Contains(key));
            }
            return q.ToList(pagingInfo);
        }

        /// <summary>
        /// 获取备件出库单明细
        /// </summary>
        /// <param name="sourceNo">来源单号</param>
        /// <param name="fromType">来源类型</param>
        public virtual EntityList<PartOutDepotDetail> GetPartOutDepotDetailDtl(string sourceNo, OutDepotType fromType)
        {
            var q = Query<PartOutDepotDetail>();
            if (sourceNo.IsNotEmpty())
            {
                q.Where(p => p.OutDepot.SourceNo == sourceNo);
            }
            q.Where(p => p.OutDepot.OutDepotType == fromType);
            q.Where(p => p.OutDepot.OutDepotState == OutDepotState.Ed);
            q.Where(p => p.OutDepotCount > p.UseCount);

            var elo = new EagerLoadOptions();
            elo.LoadWith(PartOutDepotDetail.SparePartProperty);
            elo.LoadWith(PartOutDepotDetail.OutDepotProperty);
            //elo.LoadWith(PartOutDepotDetail.OutDepotDepotProperty);
            elo.LoadWithViewProperty();
            return q.ToList(null, elo);
        }

        /// <summary>
        /// 获取备件出库单明细(层级逻辑)
        /// </summary>
        /// <param name="sparePartId"></param>
        /// <param name="equipAccountId"></param>
        /// <param name="modelId"></param>
        /// <param name="sourceNo"></param>
        /// <param name="pagingInfo"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual EntityList<PartOutDepotDetail> GetPartOutDepotDtls(double sparePartId, double equipAccountId, double modelId, string sourceNo, PagingInfo pagingInfo, string key)
        {
            var elo = new EagerLoadOptions();
            elo.LoadWith(PartOutDepotDetail.OutDepotProperty);
            elo.LoadWith(PartOutDepotDetail.SparePartProperty);
            elo.LoadWith(OutDepot.EquipAccountProperty);
            elo.LoadWithViewProperty();

            var sourceNoQueryer = this.GetPartOutDepotDtlQueryer(sparePartId, key);
            sourceNoQueryer.Where(p => p.OutDepot.SourceNo == sourceNo);
            var list = sourceNoQueryer.ToList(pagingInfo, elo);

            //当以来源单号为条件查不出数据时，查询设备台账数据
            if (list.Count <= 0)
            {
                var equipQueryer = this.GetPartOutDepotDtlQueryer(sparePartId, key);
                equipQueryer.Where(p => p.OutDepot.EquipAccountId == equipAccountId);
                list = equipQueryer.ToList(pagingInfo, elo);
            }

            //当以设备为条件查不出数据时，查询设备型号数据
            if (list.Count <= 0)
            {
                var noEquipQueryer = this.GetPartOutDepotDtlQueryer(sparePartId, key);
                noEquipQueryer.Where(p => p.OutDepot.EquipAccountId == null && p.OutDepot.EquipModelId == modelId);
                list = noEquipQueryer.ToList(pagingInfo, elo);
            }

            //当以设备/设备型号为条件查不出数据时，查询设备和设备型号为空的数据
            if (list.Count <= 0)
            {
                var noEquipAndModelQueryer = this.GetPartOutDepotDtlQueryer(sparePartId, key);
                noEquipAndModelQueryer.Where(p => p.OutDepot.EquipAccountId == null && p.OutDepot.EquipModelId == null);
                list = noEquipAndModelQueryer.ToList(pagingInfo, elo);
            }

            return list;
        }

        /// <summary>
        /// 获取备件出库单明细(层级逻辑)
        /// </summary>
        /// <param name="sparePartId"></param>
        /// <param name="equipAccountId"></param>
        /// <param name="modelId"></param>
        /// <param name="sourceNo"></param>
        /// <param name="pagingInfo"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual EntityList<PartOutDepotDetail> GetPartOutDepotDtls(double sparePartId, double? equipAccountId, double modelId, string sourceNo, PagingInfo pagingInfo, string key)
        {
            var elo = new EagerLoadOptions();
            elo.LoadWith(PartOutDepotDetail.OutDepotProperty);
            elo.LoadWith(PartOutDepotDetail.SparePartProperty);
            elo.LoadWith(OutDepot.EquipAccountProperty);
            elo.LoadWithViewProperty();

            var sourceNoQueryer = this.GetPartOutDepotDtlQueryer(sparePartId, key);
            sourceNoQueryer.Where(p => p.OutDepot.SourceNo == sourceNo);
            var list = sourceNoQueryer.ToList(pagingInfo, elo);

            //当以来源单号为条件查不出数据时，查询设备型号数据
            if (list.Count <= 0 && equipAccountId != null)
            {
                var equipQueryer = this.GetPartOutDepotDtlQueryer(sparePartId, key);
                equipQueryer.Where(p => p.OutDepot.EquipAccountId == equipAccountId);
                list = equipQueryer.ToList(pagingInfo, elo);
            }

            //当以设备为条件查不出数据时，查询设备型号数据
            if (list.Count <= 0)
            {
                var noEquipQueryer = this.GetPartOutDepotDtlQueryer(sparePartId, key);
                noEquipQueryer.Where(p => p.OutDepot.EquipAccountId == null && p.OutDepot.EquipModelId == modelId);
                list = noEquipQueryer.ToList(pagingInfo, elo);
            }

            //当以设备/设备型号为条件查不出数据时，查询设备和设备型号为空的数据
            if (list.Count <= 0)
            {
                var noEquipAndModelQueryer = this.GetPartOutDepotDtlQueryer(sparePartId, key);
                noEquipAndModelQueryer.Where(p => p.OutDepot.EquipAccountId == null && p.OutDepot.EquipModelId == null);
                list = noEquipAndModelQueryer.ToList(pagingInfo, elo);
            }

            return list;
        }

        /// <summary>
        /// 获取备件出库单明细 查询器
        /// </summary>
        /// <param name="sparePartId"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        private IEntityQueryer<PartOutDepotDetail> GetPartOutDepotDtlQueryer(double sparePartId, string key)
        {
            var q = Query<PartOutDepotDetail>();
            q.Where(p => p.SparePartId == sparePartId);
            q.Where(p => p.OutDepot.OutDepotType != OutDepotType.Pucharse && p.OutDepot.OutDepotType != OutDepotType.DgMaintain && p.OutDepot.OutDepotType != OutDepotType.Scrap);
            q.Where(p => p.OutboundStatus == OutboundStatus.Shipped);
            q.Where(p => p.UseCount < p.OutDepotCount);

            if (key.IsNotEmpty())
            {
                q.Where(p => p.OutDepot.No.Contains(key));
            }
            return q;
        }

        /// <summary>
        /// 根据备件ID列表，获取备件申请单明细
        /// </summary>
        /// <param name="sparePartIds">备件ID列表</param>
        /// <returns></returns>
        public virtual EntityList<PartOutDepotDetail> GetPartOutDepotDetailsBySparePartIds(IList<double> sparePartIds)
        {
            return Query<PartOutDepotDetail>().Where(p => sparePartIds.Contains(p.SparePartId)).ToList();
        }

        /// <summary>
        /// 获取备件出库单信息
        /// </summary>
        /// <param name="sparePartIds"></param>
        /// <returns></returns>
        public virtual Dictionary<double, SpareOutInfo> GetSpareOutDepotDetailInfo(IList<double> sparePartIds)
        {
            Dictionary<double, SpareOutInfo> outInfos = new Dictionary<double, SpareOutInfo>();
            sparePartIds.SplitDataExecute(tempIds =>
            {
                var list = Query<PartOutDepotDetail>()
                .Where(p => tempIds.Contains(p.SparePartId)).ToList<SpareOutInfo>();

                foreach(var i in list)
                {
                    if (outInfos.ContainsKey(i.Id))
                    {
                        outInfos[i.Id].UseCount += i.UseCount;
                        outInfos[i.Id].OutDepotCount += i.OutDepotCount;
                    }
                    else
                    {
                        outInfos.Add(i.Id, new SpareOutInfo { UseCount = i.UseCount, OutDepotCount = i.OutDepotCount });
                    }
                }
            });
            return outInfos;
        }

        /// <summary>
        ///  获取备件申请明细
        /// </summary>
        ///  <param name="detailIds">排序信息</param>        
        /// <returns>备件申请明细列表</returns>
        public virtual IList<PartOutDepotDetail> GetPartOutDepotDetails(List<double> detailIds)
        {
            return detailIds.SplitContains(tempIds =>
            {
                return Query<PartOutDepotDetail>().Where(x => tempIds.Contains(x.Id)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
        }


        /// <summary>
        /// 根据序列号ID获取备件出库明细
        /// </summary>
        /// <param name="SeriaNoRefId"></param>
        /// <returns></returns>
        public virtual PartOutDepotDetail GetPartOutDepotDetail(double SeriaNoRefId)
        {
            return Query<PartOutDepotDetail>().Where(p => p.SeriaNoRefId == SeriaNoRefId).FirstOrDefault();
        }

        /// <summary>
        /// 校验条码与出库单已选的备件是否匹配
        /// </summary>
        /// <param name="form">出库单</param>
        /// <returns></returns>
        public virtual OutDepotQueryInfo VerifyBarcodeIsSelectSparepart(OutDepot form)
        {
            OutDepotQueryInfo info = new OutDepotQueryInfo();

            if (form.ControlMethod == SpareParts.Enums.ControlMethod.Batch)
            {
                info.Success = false;
                info.Message = "未在此仓库找到该批次信息，请确认后重新扫描批次号！".L10N();
                return info;
            }

            if (form.ControlMethod == SpareParts.Enums.ControlMethod.Sn)
            {
                info.Success = false;
                info.Message = "未在此仓库找到该序列号信息，请确认后重新扫描序列号！".L10N();
                return info;
            }

            return info;
        }

        /// <summary>
        /// 获取出库明细
        /// </summary>
        /// <param name="outDepotId">出库单Id</param>
        /// <param name="keyword">查询关键字</param>
        /// <param name="orderInfoList">排序信息</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <returns>出库明细列表</returns>
        public virtual EntityList<PartOutDepotDetail> GetOutDepotDetails(double outDepotId, string keyword, IList<OrderInfo> orderInfoList, PagingInfo pagingInfo)
        {
            var q = Query<PartOutDepotDetail>()
                .Where(p => p.OutDepotId == outDepotId)
                .WhereIf(keyword.IsNotEmpty(), p => p.SparePart.SparePartCode.Contains(keyword)
                || p.SparePart.SparePartName.Contains(keyword) || p.SparePart.Specification.Contains(keyword)
                || p.BatchNoRef.BatchNumber.Contains(keyword) || p.SeriaNoRef.OrderNumberCode.Contains(keyword));

            OrderInfo orderInfo = new OrderInfo();
            orderInfo.Property = "LineNo";
            orderInfo.SortOrder = System.ComponentModel.ListSortDirection.Ascending;
            orderInfo.SortIndex = 1;
            orderInfoList.Add(orderInfo);

            var list = q.OrderBy(orderInfoList).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
            return list;
        }

        /// <summary>
        /// 获取待发货明细
        /// </summary>
        /// <param name="outDepotId">出库单Id</param>
        /// <param name="keyword">查询关键字</param>
        /// <param name="orderInfoList">排序信息</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <returns>待发货明细列表</returns>
        public virtual EntityList<PartOutDepotDetail> GetSendOutDepotDetails(double outDepotId, string keyword, IList<OrderInfo> orderInfoList, PagingInfo pagingInfo)
        {
            var q = Query<PartOutDepotDetail>()
                .Where(p => p.OutDepotId == outDepotId && p.OutboundStatus == OutboundStatus.Picked)
                .WhereIf(keyword.IsNotEmpty(), p => p.SparePart.SparePartCode.Contains(keyword)
                || p.SparePart.SparePartName.Contains(keyword) || p.SparePart.Specification.Contains(keyword)
                || p.BatchNoRef.BatchNumber.Contains(keyword) || p.SeriaNoRef.OrderNumberCode.Contains(keyword));

            OrderInfo orderInfo = new OrderInfo();
            orderInfo.Property = "LineNo";
            orderInfo.SortOrder = System.ComponentModel.ListSortDirection.Ascending;
            orderInfo.SortIndex = 1;
            orderInfoList.Add(orderInfo);
            return q.OrderBy(orderInfoList).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 备件条码出库查询
        /// </summary>
        /// <param name="barcode">备件条码</param>
        /// <param name="form">出库单头信息</param>
        /// <returns>出库条码扫描返回信息</returns>
        public virtual OutDepotQueryInfo OutDepotBarcodeQuery(string barcode, OutDepot form)
        {
            OutDepotQueryInfo info = new OutDepotQueryInfo();
            info.OutDepotInfo = new OutDepot();

            var storeSummary = StoreSummaryBarcodeQuery(barcode, form);

            if (storeSummary == null)
            {
                info.Success = false;
                info.Message = "所扫描内容有误，在所选仓库和质量状态中既不是序列号、批次号，也不是备件编码，或没有库位库存，请确认后重新扫描！".L10N();
            }
            else
            {
                info.Success = true;
                info.OutDepotInfo.SparePartId = storeSummary.SparePartId;
                info.OutDepotInfo.SparePartCode = storeSummary.SparePartCode;
                info.OutDepotInfo.SparePartName = storeSummary.SparePartName;
                info.OutDepotInfo.ControlMethod = storeSummary.ControlMethod;

                if (form.SparePartId != null && storeSummary.ControlMethod != form.ControlMethod)
                {
                    return VerifyBarcodeIsSelectSparepart(form);
                }

                if (!form.IsNew)
                {
                    var applySparePartIds = form.OutDepotDetailList.Where(p => p.RequireCount > p.PickedCount).Select(p => p.SparePartId).ToList();

                    if (!applySparePartIds.Contains(storeSummary.SparePartId))
                    {
                        info.Success = false;
                        info.Message = "所扫描备件信息与申请明细中存在备件不同，或该备件已拣货完毕，请确认后重新扫描！".L10N();
                        return info;
                    }
                }

                if (storeSummary.ControlMethod == SpareParts.Enums.ControlMethod.ItemCode)
                {
                    var storeLocatList = storeSummary.StoreSummaryLocationList.Where(p => p.WarehouseId == form.WarehouseId && (form.QualityStatus == QualityStatus.Good ? p.GoodNumber > 0 : p.RotNumber > 0));

                    if (storeLocatList.Count() == 1)
                    {
                        var storeLocat = storeLocatList.First();
                        info.OutDepotInfo.StorageLocationId = storeLocat.StorageLocationId;
                        info.OutDepotInfo.StorageLocationName = storeLocat.StorageLocation.Name;
                        info.OutDepotInfo.StorageLocationNum = form.QualityStatus == QualityStatus.Good ? storeLocat.GoodNumber : storeLocat.RotNumber;
                    }
                    info.OutDepotInfo.AdviceStorageLocation = string.Join("，", storeLocatList.Select(p => p.StorageLocation.Name + "(" + (form.QualityStatus == QualityStatus.Good ? p.GoodNumber : p.RotNumber) + ")"));
                    info.Message = "请输入备件的出库数量后回车！".L10N();
                }
                else if (storeSummary.ControlMethod == SpareParts.Enums.ControlMethod.Batch)
                {
                    var storeLotList = storeSummary.StoreSummaryDepotList.Where(p => p.WarehouseId == form.WarehouseId && (form.QualityStatus == QualityStatus.Good ? p.GoodNumber > 0 : p.RotNumber > 0) && p.BatchNumber == barcode);

                    if (storeLotList.Count() == 1)
                    {
                        var storeLot = storeLotList.First();
                        info.OutDepotInfo.StorageLocationId = storeLot.StorageLocationId;
                        info.OutDepotInfo.StorageLocationName = storeLot.StorageLocation.Name;
                        info.OutDepotInfo.StorageLocationNum = form.QualityStatus == QualityStatus.Good ? storeLot.GoodNumber : storeLot.RotNumber;
                    }
                    info.OutDepotInfo.AdviceStorageLocation = string.Join(",", storeLotList.Select(p => p.StorageLocation.Name + "(" + (form.QualityStatus == QualityStatus.Good ? p.GoodNumber : p.RotNumber) + ")"));
                    info.Message = "请输入该批次的出库数量后回车!".L10N();
                }
                else
                {
                    var storeDetail = storeSummary.StoreSummaryDetailList.First(p => p.OrderNumberCode == barcode);
                    if (storeDetail.StoreStatus == OrdNumStoreStatus.In)
                    {
                        form.StorageLocationId = storeDetail.StorageLocationId;
                        form.StorageLocationName = storeDetail.StorageLocation.Name;
                        form.ScanValue = barcode;
                        info = OutDepotQtyQuery(1, form, 0);
                    }
                    else
                    {
                        info.Success = false;
                        info.Message = "该序列号的入库状态为非入库，无法进行出库，请确认后重新扫描序列号！".L10N();
                    }
                }
            }
            return info;
        }

        /// <summary>
        /// 备件条码库存查询
        /// </summary>
        /// <param name="barcode">备件条码</param>
        /// <param name="form">出库单头信息</param>
        /// <returns>备件库存信息</returns>
        public virtual StoreSummary StoreSummaryBarcodeQuery(string barcode, OutDepot form)
        {
            DateTime? nullValue = null;

            var query = Query<StoreSummary>()
                .LeftJoin<StoreSummaryLocation>((a, b) => a.Id == b.StoreSummaryId && b.WarehouseId == form.WarehouseId && b.StoreSummary.SparePart.SparePartCode == barcode)
                .LeftJoin<StoreSummaryLot>((a, c) => a.Id == c.StoreSummaryId && c.WarehouseId == form.WarehouseId && c.BatchNumber == barcode)
                .LeftJoin<StoreSummaryDetail>((a, d) => a.Id == d.StoreSummaryId && d.WarehouseId == form.WarehouseId && d.OrderNumberCode == barcode)
                .WhereIf<StoreSummaryLocation, StoreSummaryLot, StoreSummaryDetail>(form.QualityStatus == QualityStatus.Good, (a, b, c, d) => b.GoodNumber > 0 || c.GoodNumber > 0 || d.GoodNumber > 0)
                .WhereIf<StoreSummaryLocation, StoreSummaryLot, StoreSummaryDetail>(form.QualityStatus == QualityStatus.Defective, (a, b, c, d) => b.RotNumber > 0 || c.RotNumber > 0 || d.RotNumber > 0)
                .WhereIf<StoreSummaryLocation, StoreSummaryLot, StoreSummaryDetail>(form.StorageLocationId != null, (a, b, c, d) => b.StorageLocationId == form.StorageLocationId || c.StorageLocationId == form.StorageLocationId || d.StorageLocationId == form.StorageLocationId)
                .Where<StoreSummaryLocation, StoreSummaryLot, StoreSummaryDetail>((a, b, c, d) => b.CreateDate != nullValue || c.CreateDate != nullValue || d.CreateDate != nullValue);

            return query.FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty()
                .LoadWith(StoreSummary.StoreSummaryLocationListProperty)
                .LoadWith(StoreSummary.StoreSummaryDepotListProperty)
                .LoadWith(StoreSummary.StoreSummaryDetailListProperty));
        }

        /// <summary>
        /// 备件数量出库查询
        /// </summary>
        /// <param name="qty">备件数量</param>
        /// <param name="form">出库单头信息</param>
        /// <param name="pickedQty">已拣数量</param>
        /// <returns>出库数量输入返回信息</returns>
        public virtual OutDepotQueryInfo OutDepotQtyQuery(int qty, OutDepot form, int pickedQty)
        {
            OutDepotQueryInfo info = new OutDepotQueryInfo();
            info.OutDepotInfo = new OutDepot();

            var storeSummary = StoreSummaryBarcodeQuery(form.ScanValue, form);

            if (storeSummary == null)
            {
                info.Success = false;
                info.Message = "所扫描内容有误，在所选仓库和质量状态中既不是序列号、批次号，也不是备件编码，或没有库位库存，请确认后重新扫描！".L10N();
            }
            else
            {
                info.Success = true;
                info.OutDepotInfo.SparePartId = storeSummary.SparePartId;
                info.OutDepotInfo.SparePartCode = storeSummary.SparePartCode;
                info.OutDepotInfo.SparePartName = storeSummary.SparePartName;
                info.OutDepotInfo.ControlMethod = storeSummary.ControlMethod;

                info.Message = "请扫描【序列号】/【批次号】/【备件编码】！".L10N();
                //if (storeSummary.ControlMethod == SpareParts.Enums.ControlMethod.ItemCode) 
                //{
                //    info.Message = "请输入备件的出库数量后回车！".L10N();
                //}
                //if (storeSummary.ControlMethod == SpareParts.Enums.ControlMethod.Batch)
                //{
                //    info.Message = "请扫描批次号！".L10N();
                //}
                //if (storeSummary.ControlMethod == SpareParts.Enums.ControlMethod.Sn)
                //{
                //    info.Message = "请扫描序列号！".L10N();
                //}

                OutDepotDetail applyDetail = new OutDepotDetail();
                applyDetail.SparePartId = storeSummary.SparePartId;
                applyDetail.SparePartCodeView = storeSummary.SparePartCode;
                applyDetail.SparePartNameView = storeSummary.SparePartName;
                applyDetail.ControlMethodView = storeSummary.ControlMethod;
                applyDetail.RequireCount = qty;
                applyDetail.PickedCount = qty;
                applyDetail.CreateDate = DateTime.Now;
                applyDetail.UpdateDate = DateTime.Now;
                info.OutDepotInfo.OutDepotDetailList.Add(applyDetail);

                PartOutDepotDetail outDepotDetail = new PartOutDepotDetail();
                outDepotDetail.SparePartId = storeSummary.SparePartId;
                outDepotDetail.SparePartCodeView = storeSummary.SparePartCode;
                outDepotDetail.SparePartNameView = storeSummary.SparePartName;
                outDepotDetail.ControlMethodView = storeSummary.ControlMethod;
                outDepotDetail.OutDepotCount = qty;
                outDepotDetail.QualityStatus = (QualityStatus)form.QualityStatus;
                outDepotDetail.CreateDate = DateTime.Now;
                outDepotDetail.UpdateDate = DateTime.Now;

                if (storeSummary.ControlMethod == SpareParts.Enums.ControlMethod.ItemCode)
                {
                    var storeLocat = storeSummary.StoreSummaryLocationList.First(p => p.StorageLocationId == form.StorageLocationId);

                    if (qty + pickedQty > (form.QualityStatus == QualityStatus.Good ? storeLocat.GoodNumber : storeLocat.RotNumber))
                    {
                        info.Success = false;
                        info.Message = "备件在此库位库存不足，请确认后重新输入出库数量后回车！".L10N();
                    }
                    else
                    {
                        outDepotDetail.StorageLocationId = storeLocat.StorageLocationId;
                        outDepotDetail.SiteCode = storeLocat.StorageLocation.Name;
                        info.OutDepotInfo.PartOutDepotDetailList.Add(outDepotDetail);
                    }
                }
                else if (storeSummary.ControlMethod == SpareParts.Enums.ControlMethod.Batch)
                {
                    var storeLot = storeSummary.StoreSummaryDepotList.First(p => p.StorageLocationId == form.StorageLocationId && p.BatchNumber == form.ScanValue);

                    if (qty + pickedQty > (form.QualityStatus == QualityStatus.Good ? storeLot.GoodNumber : storeLot.RotNumber))
                    {
                        info.Success = false;
                        info.Message = "备件在此库位库存不足，请确认后重新输入出库数量后回车！".L10N();
                    }
                    else
                    {
                        outDepotDetail.BatchNoRefId = storeLot.Id;
                        outDepotDetail.BatchNo = storeLot.BatchNumber;
                        outDepotDetail.StorageLocationId = storeLot.StorageLocationId;
                        outDepotDetail.SiteCode = storeLot.StorageLocation.Name;
                        info.OutDepotInfo.PartOutDepotDetailList.Add(outDepotDetail);
                    }
                }
                else
                {
                    var storeDetail = storeSummary.StoreSummaryDetailList.First(p => p.StorageLocationId == form.StorageLocationId && p.OrderNumberCode == form.ScanValue);

                    if (qty > (form.QualityStatus == QualityStatus.Good ? storeDetail.GoodNumber : storeDetail.RotNumber))
                    {
                        info.Success = false;
                        info.Message = "备件在此库位库存不足，请确认后重新扫描！".L10N();
                    }
                    else
                    {
                        outDepotDetail.SeriaNoRefId = storeDetail.Id;
                        outDepotDetail.SeriaNo = storeDetail.OrderNumberCode;
                        outDepotDetail.StorageLocationId = storeDetail.StorageLocationId;
                        outDepotDetail.SiteCode = storeDetail.StorageLocation.Name;
                        info.OutDepotInfo.PartOutDepotDetailList.Add(outDepotDetail);
                    }
                }
            }
            return info;
        }


        /// <summary>
        /// 保存出库单
        /// </summary>
        /// <param name="form">出库单头信息</param>
        /// <returns></returns>
        public virtual void SaveOutDepotBill(OutDepot form)
        {
            if (form == null)
            {
                throw new ValidationException("表头信息不能为空，请重新尝试".L10N());
            }

            //2022-06-15 张俊杰的需求 领用部门改成非必输，添加出库单那里。要加个检验部门不能为空
            if (!form.GetDepartmentId.HasValue)
            {
                throw new ValidationException("领用部门不能为空，请确认".L10N());
            }

            if (form.IsNew)
            {
                //构建申请单头信息
                SparePartApp apply = new SparePartApp();
                apply.GenerateId();
                apply.No = RT.Service.Resolve<SparePartAppController>().GetNo();
                apply.FromNo = form.No;
                apply.FromType = Applys.Enums.FromType.Hand;
                apply.DemandDate = DateTime.Now;
                apply.AuditState = Applys.Enums.AuditState.Butbound;
                apply.QualityStatus = (QualityStatus)form.QualityStatus;
                apply.GetDepartmentId = form.GetDepartmentId.Value;

                if (!form.OutDepotDetailList.Any())
                {
                    throw new ValidationException("申请明细没有可保存的数据，请确认！".L10N());
                }

                //构建申请明细
                foreach (var outApplyDetail in form.OutDepotDetailList)
                {
                    ApplyDetail applyDetail = new ApplyDetail();
                    applyDetail.SparePartAppId = apply.Id;
                    applyDetail.SparePartId = outApplyDetail.SparePartId;
                    applyDetail.WarehouseId = (double)form.WarehouseId;
                    applyDetail.ApplyAmount = outApplyDetail.RequireCount;
                    apply.ApplyDetailList.Add(applyDetail);
                }

                //修改出库单头信息
                form.ReleDoc = apply.No;
                form.OutState = form.OutDepotType == OutDepotType.DgMaintain ? OutDepotState.Ing : form.OutState;

                //修改出库明细
                int i = 1;
                foreach (var outDepotDetail in form.PartOutDepotDetailList)
                {
                    outDepotDetail.LineNo = i;
                    outDepotDetail.OutboundStatus = OutboundStatus.Picked;
                    i++;
                }

                using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
                {
                    RF.Save(apply);
                    RF.Save(form);
                    trans.Complete();
                }
            }
            else
            {
                //修改出库明细行号和状态
                var lineNos = Query<PartOutDepotDetail>().Where(p => p.OutDepotId == form.Id).Select(p => p.LineNo.MAX()).ToList<int>();
                int i = !lineNos.Any() ? 1 : lineNos.First() + 1;
                foreach (var outDepotDetail in form.PartOutDepotDetailList)
                {
                    outDepotDetail.LineNo = i;
                    outDepotDetail.OutboundStatus = OutboundStatus.Picked;
                    i++;
                }
                RF.Save(form);
            }
        }

        /// <summary>
        /// 按出库单明细发货
        /// </summary>
        /// <param name="partOutDepotDetailList">出库明细信息</param>
        /// <returns></returns>
        public virtual void SendPartOutDepotDetails(List<PartOutDepotDetail> partOutDepotDetailList)
        {
            EntityList<PartOutDepotDetail> outDepotDetailList = new EntityList<PartOutDepotDetail>();
            outDepotDetailList.AddRange(partOutDepotDetailList);

            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                OutDepot outDepot = outDepotDetailList[0].OutDepot;
                outDepot.PersistenceStatus = PersistenceStatus.Modified;

                var storeSummaryList = outDepotDetailList.Select(p => p.SparePartId).SplitContains(tempIds =>
                {
                    return Query<StoreSummary>().Where(p => tempIds.Contains(p.SparePartId))
                                                .ToList(null, new EagerLoadOptions()
                                                .LoadWithViewProperty()
                                                .LoadWith(StoreSummary.StoreSummaryLocationListProperty)
                                                .LoadWith(StoreSummary.StoreSummaryDepotListProperty)
                                                .LoadWith(StoreSummary.StoreSummaryDetailListProperty));
                });

                SparePartApp applyBill = outDepot.ReleDoc.IsNotEmpty() ? Query<SparePartApp>().Where(p => p.No == outDepot.ReleDoc).FirstOrDefault(new EagerLoadOptions().LoadWith(SparePartApp.ApplyDetailListProperty)) : new SparePartApp();

                foreach (var outDepotDetail in outDepotDetailList)
                {
                    var storeSummary = storeSummaryList.First(p => p.SparePartId == outDepotDetail.SparePartId);

                    //更新出库明细的状态为发货
                    outDepotDetail.OutboundStatus = OutboundStatus.Shipped;
                    outDepotDetail.PersistenceStatus = PersistenceStatus.Modified;
                    outDepotDetail.UnitPrice = Convert.ToDouble(storeSummary.AverageCost);

                    //更新出库单的申请明细的出库数量
                    DB.Update<OutDepotDetail>()
                        .Set(p => p.OutDepotCount, p => p.OutDepotCount + outDepotDetail.OutDepotCount)
                        .Where(p => p.OutDepotId == outDepotDetail.OutDepotId && p.SparePartId == outDepotDetail.SparePartId).Execute();

                    if (outDepot.ReleDoc.IsNotEmpty())
                    {
                        //更新申请单的申请明细的出库数量
                        var applyDetail = applyBill.ApplyDetailList.First(p => p.WarehouseId == outDepot.WarehouseId && p.SparePartId == outDepotDetail.SparePartId);
                        applyDetail.OutDepotAmount += outDepotDetail.OutDepotCount;

                        //更新申请单主表状态为已出库或部分出库
                        applyBill.AuditState = applyBill.ApplyDetailList.Sum(p => p.ApplyAmount) <= applyBill.ApplyDetailList.Sum(p => p.OutDepotAmount) ? Applys.Enums.AuditState.Butbounded : Applys.Enums.AuditState.PartButbound;

                        RF.Save(applyBill);
                    }

                    //更新备件库存
                    UpdateStoreSummary(outDepot, outDepotDetail, storeSummary);
                }

                int sumRequireCount = outDepot.OutDepotDetailList.Select(p => p.RequireCount).Sum();
                int sumOutDepotCount = outDepot.OutDepotDetailList.Select(p => p.OutDepotCount).Sum();

                //申请明细中，存在行申请数量＞出库数量＞0时，为部分出库状态;所有行申请数量 = 出库数量时，为已出库状态
                outDepot.OutDepotState = sumRequireCount > sumOutDepotCount ? OutDepotState.PartOut : OutDepotState.Ed;
                //主表状态为待出库时，出库状态为待出库; 主表状态为部分出库或已出库时，出库状态更新为已出库
                outDepot.OutState = OutDepotState.Ed;

                RF.Save(outDepot);

                //生成备件交接单
                bool isCreateHandoverBill = IsCreateHandoverBill();

                if (isCreateHandoverBill)
                {
                    var outDepotHandover = CreatHandoverBill(outDepotDetailList);
                    RF.Save(outDepotHandover);
                }

                RF.Save(outDepotDetailList);
                trans.Complete();
            }
        }

        /// <summary>
        /// 更新备件库存
        /// </summary>
        /// <param name="outDepot">出库单信息</param>
        /// <param name="outDepotDetail">出库明细信息</param>
        /// <param name="storeSummary">备件库存信息</param>
        /// <returns></returns>
        public virtual void UpdateStoreSummary(OutDepot outDepot, PartOutDepotDetail outDepotDetail, StoreSummary storeSummary)
        {
            storeSummary.SumNumber -= outDepotDetail.OutDepotCount;
            storeSummary.GoodNumber -= outDepot.QualityStatus == QualityStatus.Good ? outDepotDetail.OutDepotCount : 0;
            storeSummary.RotNumber -= outDepot.QualityStatus == QualityStatus.Defective ? outDepotDetail.OutDepotCount : 0;

            if (outDepotDetail.ControlMethod == SpareParts.Enums.ControlMethod.ItemCode)
            {
                var storeSummaryLocation = storeSummary.StoreSummaryLocationList.First(p => p.StoreSummaryId == storeSummary.Id && p.StorageLocationId == outDepotDetail.StorageLocationId);
                storeSummaryLocation.SumNumber -= outDepotDetail.OutDepotCount;
                storeSummaryLocation.GoodNumber -= outDepot.QualityStatus == QualityStatus.Good ? outDepotDetail.OutDepotCount : 0;
                storeSummaryLocation.RotNumber -= outDepot.QualityStatus == QualityStatus.Defective ? outDepotDetail.OutDepotCount : 0;

                if (storeSummaryLocation.GoodNumber < 0 || storeSummaryLocation.RotNumber < 0)
                {
                    throw new ValidationException("备件【{0}】的库存数量不足，发货失败".L10nFormat(storeSummary.SparePartCode));
                }
            }

            if (outDepotDetail.ControlMethod == SpareParts.Enums.ControlMethod.Batch)
            {
                var storeSummaryLot = storeSummary.StoreSummaryDepotList.First(p => p.Id == outDepotDetail.BatchNoRefId);
                storeSummaryLot.SumNumber -= outDepotDetail.OutDepotCount;
                storeSummaryLot.GoodNumber -= outDepot.QualityStatus == QualityStatus.Good ? outDepotDetail.OutDepotCount : 0;
                storeSummaryLot.RotNumber -= outDepot.QualityStatus == QualityStatus.Defective ? outDepotDetail.OutDepotCount : 0;

                if (storeSummaryLot.GoodNumber < 0 || storeSummaryLot.RotNumber < 0)
                {
                    throw new ValidationException("批次【{0}】的库存数量不足，发货失败".L10nFormat(outDepotDetail.BatchNo));
                }
            }

            if (outDepotDetail.ControlMethod == SpareParts.Enums.ControlMethod.Sn)
            {
                var storeSummaryDetail = storeSummary.StoreSummaryDetailList.First(p => p.Id == outDepotDetail.SeriaNoRefId);
                storeSummaryDetail.SumNumber -= outDepotDetail.OutDepotCount;
                storeSummaryDetail.GoodNumber -= outDepot.QualityStatus == QualityStatus.Good ? outDepotDetail.OutDepotCount : 0;
                storeSummaryDetail.RotNumber -= outDepot.QualityStatus == QualityStatus.Defective ? outDepotDetail.OutDepotCount : 0;
                storeSummaryDetail.StoreStatus = outDepot.OutDepotType == OutDepotType.DgMaintain ? OrdNumStoreStatus.Outsourced : OrdNumStoreStatus.Out;

                if (storeSummaryDetail.GoodNumber < 0 || storeSummaryDetail.RotNumber < 0)
                {
                    throw new ValidationException("序列号【{0}】的库存数量不足，发货失败".L10nFormat(outDepotDetail.SeriaNo));
                }
            }

            RF.Save(storeSummary);
        }

        /// <summary>
        /// 生成交接单
        /// </summary>
        /// <param name="outDepotDetailList">出库明细信息</param>
        /// <returns>备件交接单</returns>
        public virtual OutDepotHandover CreatHandoverBill(EntityList<PartOutDepotDetail> outDepotDetailList)
        {
            var handoverDetailList = outDepotDetailList.GroupBy(p => new { p.SparePartId, p.BatchNo, p.SeriaNo })
                                              .Select(p => new OutDepotHandoverDetail
                                              {
                                                  SparePartId = p.First().SparePartId,
                                                  BatchNo = p.First().BatchNo,
                                                  SeriaNo = p.First().SeriaNo,
                                                  Qty = p.Sum(s => s.OutDepotCount),
                                                  HandOverStatus = HandOverStatus.Pending
                                              }).ToList();

            handoverDetailList.ForEach(p =>
            {
                p.GenerateId();
            });

            OutDepotHandover outDepotHandover = new OutDepotHandover();
            outDepotHandover.HandoverNo = RT.Service.Resolve<OutDepotController>().GetOutDepotHandoverNo();
            outDepotHandover.OutDepotId = outDepotDetailList[0].OutDepotId;
            outDepotHandover.OutDepotDate = DateTime.Now;
            outDepotHandover.HandOverStatus = HandOverStatus.Pending;
            outDepotHandover.OutDepotHandoverDetailList.AddRange(handoverDetailList);

            //回写交接明细ID给出库明细
            outDepotDetailList.ForEach(p =>
            {
                p.OutDepotHandoverDetailId = handoverDetailList.First(f => f.SparePartId == p.SparePartId && f.BatchNo == p.BatchNo && f.SeriaNo == p.SeriaNo).Id;
            });

            return outDepotHandover;
        }

        /// <summary>
        /// 出库单关单操作
        /// </summary>
        /// <param name="outDepot">出库单信息</param>
        /// <returns></returns>
        public virtual void CloseOutDepotBill(OutDepot outDepot)
        {
            outDepot.OutDepotState = outDepot.OutDepotState == OutDepotState.Ing ? OutDepotState.Close : OutDepotState.Ed;

            foreach (var outDepotDetail in outDepot.PartOutDepotDetailList)
            {
                if (outDepotDetail.OutboundStatus == OutboundStatus.Picked)
                {
                    outDepotDetail.PersistenceStatus = PersistenceStatus.Deleted;

                    outDepot.OutDepotDetailList.First(p => p.SparePartId == outDepotDetail.SparePartId).PickedCount -= outDepotDetail.OutDepotCount;
                }
            }

            RF.Save(outDepot);
        }
    }
}
using SIE.Common.Configs;
using SIE.Common.Configs.CommonConfigs;
using SIE.Common.NumberRules;
using SIE.Core.Common;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EMS.Checks;
using SIE.EMS.Checks.Plans;
using SIE.EMS.Lubrications;
using SIE.EMS.Maintains.Controller;
using SIE.EMS.Maintains.Plans;
using SIE.EMS.SpareParts.Applys.Criterias;
using SIE.EMS.SpareParts.Applys.Details;
using SIE.EMS.SpareParts.Applys.Enums;
using SIE.EMS.SpareParts.OutDepots;
using SIE.EMS.SpareParts.OutDepots.Controllers;
using SIE.EMS.SpareParts.OutDepots.Details;
using SIE.EMS.SpareParts.OutDepots.Enums;
using SIE.Equipments.Common.Controller;
using SIE.Equipments.Configs;
using SIE.Equipments.EquipAccounts;
using SIE.Equipments.EquipModels;
using SIE.Resources.Enterprises;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.EMS.SpareParts.Applys.Controllers
{
    /// <summary>
    /// 备件控制器
    /// </summary>
    public class SparePartAppController : DomainController
    {

        #region Criteria查询
        /// <summary>
        /// 根据查询条件 查询备件申请单
        /// </summary>
        /// <param name="criteria">查询条件</param>
        /// <returns></returns>
        public virtual EntityList<SparePartApp> SelectByCriteria(SparePartAppCriteria criteria)
        {

            var query = DB.Query<SparePartApp>();

            if (!criteria.No.IsNullOrWhiteSpace())
            {
                query.Where(p => p.No.Contains(criteria.No));
            }
            if (!criteria.FromNo.IsNullOrWhiteSpace())
            {
                query.Where(p => p.FromNo.Contains(criteria.FromNo));
            }
            if (!criteria.EquipAccountCode.IsNullOrWhiteSpace())
            {
                query.Where(p => p.EquipAccount.Code.Contains(criteria.EquipAccountCode));
            }

            if (!criteria.EquipAccountCode.IsNullOrWhiteSpace())
            {
                query.Where(p => p.EquipAccount.Code.Contains(criteria.EquipAccountCode));
            }
            if (criteria.FromType.HasValue)
            {
                query.Where(p => p.FromType == criteria.FromType);
            }
            if (criteria.AuditState.HasValue)
            {
                query.Where(p => p.AuditState == criteria.AuditState);
            }

            if (criteria.EquipModel != null)
            {
                query.Where(p => p.EquipModelId == criteria.EquipModelId);
            }
            if (criteria.SparePart != null)
            {
                var queryDetail = DB.Query<ApplyDetail>();
                var ids =
                    queryDetail.Select(p => p.SparePartAppId).Where(p => p.SparePartId == criteria.SparePartId).ToList<double>();

                query.Where(p => ids.Contains(p.Id));
            }

            if (!criteria.SparePartName.IsNullOrWhiteSpace())
            {
                var queryDetail = DB.Query<ApplyDetail>();
                var ids = queryDetail.Select(p => p.SparePartAppId).Where(p => p.SparePart.SparePartName.Contains(criteria.SparePartName)).ToList<double>();
                query.Where(p => ids.Contains(p.Id));
            }

            if (criteria.CreateDate.BeginValue.HasValue)
            {
                query.Where(p => p.CreateDate > criteria.CreateDate.BeginValue);
            }
            if (criteria.CreateDate.EndValue.HasValue)
            {
                query.Where(p => p.CreateDate < criteria.CreateDate.EndValue);
            }
            query.OrderBy(criteria.OrderInfoList);

            return query.ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }
        #endregion

        #region 生成编码
        /// <summary>
        /// 获取自动编码No
        /// </summary>
        /// <returns></returns>
        public virtual string GetNo()
        {
            #region 注释
            //业务逻辑代码，此处生成自动编号
            var config = ConfigService.GetConfig<NoConfigValue>(new NoConfig(), typeof(SparePartApp));
            if (config == null || config.BacodeRule == null)
                throw new ValidationException("未找到备件申请编号生成规则,请检查规则配置".L10N());
            var code = RT.Service.Resolve<NumberRuleController>().GenerateSegment(config.BacodeRule.Id, 1).FirstOrDefault();
            #endregion
            return code;
        }
        #endregion

        #region 备件申请单-审核

        /// <summary>
        /// 备件申请单-审核
        /// </summary>
        /// <param name="bill">备件申请单</param>
        /// 
        public virtual SparePartApp AuditSparePartApp(SparePartApp bill)
        {
            SparePartApp app;
            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                app = AuditSparePartAppInner(bill);
                trans.Complete();
            }
            return app;
        }

        /// <summary>
        /// 备件申请单-审核
        /// </summary>
        /// <param name="bill">备件申请单</param>
        /// 
        public virtual SparePartApp AuditSparePartAppInner(SparePartApp bill)
        {
            if (bill == null)
            {
                throw new ValidationException("{0}为空，备件申请单审核操作失败。".L10nFormat(nameof(bill)));
            }

            var repository = bill.GetRepository() as EntityRepository;

            //1.0 创建出库单

            #region 创建出库单主表

            bool isWmsControl = RT.Service.Resolve<SparePartController>().IsWmsControl();
            EntityList<OutDepot> outDepotList = new EntityList<OutDepot>();

            if (isWmsControl)
            {
                throw new ValidationException("和WMS的对接尚未实现，暂无法执行审核操作".L10N());
            }
            else
            {
                var dicAppDtl = bill.ApplyDetailList.GroupBy(p => p.WarehouseId).ToDictionary(p => p.Key, p => p.ToList());
                foreach (var kv in dicAppDtl)
                {
                    OutDepot outDepot = new OutDepot();
                    //出库单类型依据申请单来源类型判断
                    switch (bill.FromType)
                    {
                        case FromType.Hand:
                            outDepot.OutDepotType = OutDepotType.Other;
                            break;
                        case FromType.SpotCheck:
                            outDepot.OutDepotType = OutDepotType.Check;
                            break;
                        case FromType.Upkeep:
                            outDepot.OutDepotType = OutDepotType.Repair;
                            break;
                        case FromType.Maintain:
                            outDepot.OutDepotType = OutDepotType.Maintain;
                            break;
                        case FromType.Setup:
                            outDepot.OutDepotType = OutDepotType.Setup;
                            break;
                        case FromType.Lubrication:
                            outDepot.OutDepotType = OutDepotType.Lubrication;
                            break;
                        default:
                            outDepot.OutDepotType = OutDepotType.Other;
                            break;
                    }
                    outDepot.GenerateId();
                    outDepot.No = RT.Service.Resolve<OutDepotController>().GetNo();
                    outDepot.OutDepotState = OutDepotState.Ing;
                    outDepot.SourceNo = bill.FromNo;
                    outDepot.ReleDoc = bill.No;
                    outDepot.QualityStatus = bill.QualityStatus;
                    outDepot.GetDepartmentId = bill.GetDepartmentId;
                    outDepot.OutDepotDate = null;
                    outDepot.EquipAccountId = bill.EquipAccountId;
                    outDepot.EquipModelId = bill.EquipModelId;
                    outDepot.IsAppComeHere = YesNo.No;
                    outDepot.WarehouseId = kv.Key;

                    foreach (var appDtl in kv.Value)
                    {
                        OutDepotDetail outDtl = new OutDepotDetail();
                        outDtl.GenerateId();
                        outDtl.SparePartId = appDtl.SparePartId;
                        outDtl.SparePartPart = appDtl.SparePartPart;
                        outDtl.EquipModelId = bill.EquipModelId;
                        outDtl.RequireCount = appDtl.ApplyAmount;
                        outDtl.OutDepotId = outDepot.Id;
                        outDepot.OutDepotDetailList.Add(outDtl);
                    }
                    outDepotList.Add(outDepot);
                    #endregion
                }
            }

            //2.0 申请单字段变更
            bill.AuditState = AuditState.Butbound;

            if (!bill.DemandDate.HasValue)
            {
                bill.DemandDate = repository.GetDbTime();
            }

            //3.0 保存

            RF.Save(outDepotList);
            RF.Save(bill);
            return bill;
        }

 


        #endregion

        #region 备件申请单-取消审核
        /// <summary>
        /// 备件申请单-取消审核
        /// </summary>
        /// <param name="bill"></param>
        /// <returns></returns>
        public virtual SparePartApp CancelAuditSparePartApp(SparePartApp bill)
        {
            //1.0 申请单字段变更
            bill.AuditState = AuditState.StandAudit;
            //2.0 保存
            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                RF.Save(bill);
                trans.Complete();
            }
            return bill;
        }
        #endregion

        #region 备件申请单-保存
        /// <summary>
        /// 备件申请单-保存
        /// </summary>
        /// <param name="bill"></param>
        /// <returns></returns>
        public virtual SparePartApp SparePartAppSave(SparePartApp bill)
        {
            //1.0 加载子表数据
            bill.GetAllChildData<SparePartApp, ApplyDetail>();

            //3.0 保存
            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                RF.Save(bill);
                trans.Complete();
            }
            return bill;
        }
        #endregion

        #region 获取申请单(根据单号)

        /// <summary>
        /// 获取申请单(根据单号)
        /// </summary>
        /// <param name="no"></param>
        /// <returns></returns>
        public virtual SparePartApp GetSparePartAppByNo(string no)
        {
            var query = Query<SparePartApp>();
            query.Where(p => p.No == no);
            return query.FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取申请单(根据Id)
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public virtual SparePartApp GetSparePartAppById(double Id)
        {
            var query = Query<SparePartApp>();
            query.Where(p => p.Id == Id);
            return query.FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
        }

        #endregion

        #region 审核操作一些列按钮
        /// <summary>
        /// 改变审核状态 如果是审核创建出库单 如果是取消审核 删除出库单
        /// </summary>
        /// <param name="spa"></param>
        /// <param name="auditState"></param>
        public virtual void ChageAuditSatate(SparePartApp spa, AuditState auditState)
        {
            //如果变更为待出库 需要创建出库单
            if (auditState == AuditState.Butbound)
            {
                CreateOutboundBill(spa);
            }
            else if (auditState == AuditState.StandAudit && spa.AuditState == AuditState.Butbound)
            {
                //取消审核 删除出库单
                var outDepot = DB.Query<OutDepot>().Where(p => p.ReleDoc.Contains(spa.No)).FirstOrDefault();

                if (outDepot != null)
                {
                    using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
                    {
                        try
                        {
                            DB.Delete<OutDepotDetail>().Where(p => p.OutDepotId == outDepot.Id).Execute();
                            DB.Delete<OutDepot>().Where(p => p.Id == outDepot.Id).Execute();
                        }
                        finally
                        {
                            //删除单据
                            trans.Complete();
                        }
                    }
                }
            }
            else if (auditState == AuditState.StandAudit && spa.ApplyDetailList.Count > 0)
            {
                //申请数量必须>0
                foreach (var item in spa.ApplyDetailList)
                {
                    if (item.ApplyAmount <= 0)
                    {
                        throw new ValidationException("申请数量请修改为大于0".L10N());
                    }
                }
            }
            else
            {
                //
            }
            //修改状态并保存
            spa.AuditState = auditState;
            var repository = spa.GetRepository() as EntityRepository;
            if (!spa.DemandDate.HasValue)
            {
                spa.DemandDate = repository.GetDbTime();
            }
            var config = RT.Service.Resolve<ApprovalController>().GetApprovalConfigValue(typeof(SparePartApp));
            using (var tran = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                RF.Save(spa);
                //自动审核
                if (!config.EnableAudit)
                {
                    //【是否启用审批】为否时,提交的同时进行审批
                    AuditSparePartAppInner(spa);
                }
                tran.Complete();
            }
        }

        /// <summary>
        /// 创建出库单
        /// </summary>
        /// <param name="spa"></param>
        private void CreateOutboundBill(SparePartApp spa)
        {
            var no = RT.Service.Resolve<OutDepotController>().GetNo();

            //创建出库单
            OutDepot outDepot = new OutDepot()
            {
                No = no,
                //OutDepotType = OutDepotType.Check,//暂时不弄这个
                OutDepotState = OutDepotState.Ing,
                SourceNo = spa.FromNo,
                ReleDoc = spa.No,
                GetDepartmentId = spa.GetDepartmentId,
                //OutDepotDate   = spa.,
                EquipAccountId = spa.EquipAccountId,
                EquipModelId = spa.EquipModelId,
                IsAppComeHere = YesNo.No,

            };

            switch (spa.FromType)
            {
                case FromType.Hand:
                    outDepot.OutDepotType = OutDepotType.Other;
                    break;
                case FromType.SpotCheck:
                    outDepot.OutDepotType = OutDepotType.Check;
                    break;
                case FromType.Upkeep:
                    outDepot.OutDepotType = OutDepotType.Repair;
                    break;
                case FromType.Maintain:
                    outDepot.OutDepotType = OutDepotType.Maintain;
                    break;
                case FromType.Setup:
                    outDepot.OutDepotType = OutDepotType.Setup;
                    break;
                default:
                    outDepot.OutDepotType = OutDepotType.Other;
                    break;
            }

            //创建明细
            foreach (var item in spa.ApplyDetailList)
            {
                var outde = new OutDepotDetail()
                {
                    OutDepotId = outDepot.Id,
                    SparePartId = item.SparePartId,
                    RequireCount = item.ApplyAmount,

                    DepotPartCount = item.DepotAmount,
                };
                
                //找到入库信息
                StoreSummary storeSummary = GetStoreSummary(item.SparePartId, item.WarehouseId);
                storeSummary.LoadProperty(StoreSummary.StoreSummaryDepotListProperty, storeSummary.StoreSummaryDepotList);

                var sorttedList = storeSummary.StoreSummaryDepotList.OrderBy(p => p.CreateDate);
                storeSummary.StoreSummaryDepotList.Clear();
                storeSummary.StoreSummaryDepotList.AddRange(sorttedList);

                //var detail = storeSummary.StoreSummaryDepotList.FirstOrDefault();

                outDepot.OutDepotDetailList.Add(outde);

            }
            RF.Save(outDepot);
        }

        /// <summary>
        /// 更新点检单备件申请备注
        /// </summary>
        /// <param name="spaNo">申请单号</param>
        /// <param name="checkNo">点检单号</param>
        /// <param name="remark">审核意见</param>
        private void UpdateCheckConfirmApply(string spaNo, string checkNo, string remark)
        {
            // 点检单Id
            double checkId = Query<CheckPlan>().Where(p => p.CheckPlanNo == checkNo).Select(p => new { p.Id }).FirstOrDefault<double>();
            // 备件申请明细Ids
            List<double> applyDtlIds = Query<ApplyDetail>()
                .LeftJoin<SparePartApp>((x, y) => x.SparePartAppId == y.Id)
                .Where<SparePartApp>((x, y) => y.No == spaNo).Select<SparePartApp>((x, y) => new {x.Id}).ToList<double>().ToList();

            // 点检备件申请
            var checkApplyList = Query<CheckPlanSparePartApl>().Where(p => p.CheckPlanId == checkId && p.ApplyDetailId != null && applyDtlIds.Contains((double)p.ApplyDetailId)).ToList();

            foreach (var checkApply in checkApplyList)
            {
                if (checkApply.Remark.IsNullOrEmpty())
                {
                    checkApply.Remark = remark;
                }
                else
                {
                    checkApply.Remark += ";" + remark;
                }
            }

            RF.Save(checkApplyList);
        }

        /// <summary>
        /// 更新保养单备件申请备注
        /// </summary>
        /// <param name="spaNo">申请单号</param>
        /// <param name="mainNo">保养单号</param>
        /// <param name="remark">审核意见</param>
        private void UpdateMaintainApply(string spaNo, string mainNo, string remark)
        {
            // 保养单Id
            double mainId = Query<MaintainPlan>().Where(p => p.MaintainNo == mainNo).Select(p => new {p.Id}).FirstOrDefault<double>();
            // 更新备注
            List<double> applyDtlIds = Query<ApplyDetail>()
                .LeftJoin<SparePartApp>((x, y) => x.SparePartAppId == y.Id)
                .Where<SparePartApp>((x, y) => y.No == spaNo).Select<SparePartApp>((x, y) => new { x.Id }).ToList<double>().ToList();

            // 保养备件申请
            var maintainApplyList = Query<MaintainPlanSparePartApl>().Where(p => p.MaintainPlanId == mainId && p.ApplyDetailId != null && applyDtlIds.Contains((double)p.ApplyDetailId)).ToList();

            foreach (var maintainApply in maintainApplyList)
            {
                if (maintainApply.Remark.IsNullOrEmpty())
                {
                    maintainApply.Remark = remark;
                }
                else
                {
                    maintainApply.Remark += ";" + remark;
                }
            }

            RF.Save(maintainApplyList);
        }

        /// <summary>
        /// 更新点检、保养的备件申请备注
        /// </summary>
        /// <param name="spa">申请单</param>
        /// <param name="remark">审核意见</param>
        public virtual void UpdateSparepartApply(SparePartApp spa, string remark)
        {
            if (spa.FromType == FromType.SpotCheck)
            {
                UpdateCheckConfirmApply(spa.No, spa.FromNo, remark);
            }
            else if (spa.FromType == FromType.Maintain)
            {
                UpdateMaintainApply(spa.No, spa.FromNo, remark);
            }
        }
        #endregion

        /// <summary>
        /// 获取库位
        /// </summary>
        /// <param name="partId"></param>
        /// <param name="depotId"></param>
        /// <returns></returns>
        public virtual StoreSummaryLot GetStoreDepotByDepotId(double partId, double depotId)
        {

            var storeSummaryDepot = DB.Query<StoreSummaryLot>()
               .Where(p => p.WarehouseId == depotId)
               .Join<StoreSummary>((d, s) => s.Id == d.StoreSummaryId && s.SparePartId == partId)
               .OrderBy(p => p.CreateDate)
               .FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());

            return storeSummaryDepot;
        }

        /// <summary>
        /// 得到仓库备件数量
        /// </summary>
        /// <param name="partId">备件id</param>
        /// <param name="depotId">仓库id</param>
        /// <returns></returns>
        public virtual int GetStoreDepotSumByDepot(double partId, double depotId)
        {

            var goodNumbers = DB.Query<StoreSummaryLot>()
               .Where(p => p.WarehouseId == depotId)
               .Join<StoreSummary>((d, s) => s.Id == d.StoreSummaryId && s.SparePartId == partId)
               .Select(p => p.GoodNumber)
               .ToList<double>();

            int sum = 0;
            foreach (var item in goodNumbers)
            {
                sum += (int)item;
            }
            return sum;
        }

        #region 删除按钮
        /// <summary>
        /// 删除选中的申请单并删除子表
        /// </summary>
        /// <param name="ids"></param>
        public virtual int DeleteApp(List<double> ids)
        {
            int count = 0;
            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {

                DB.Delete<ApplyDetail>().Where(s => ids.Contains(s.SparePartAppId)).Execute();
                count = DB.Delete<SparePartApp>().Where(s => ids.Contains(s.Id)).Execute();
                trans.Complete();
            }
            return count;
        }


        /// <summary>
        /// 删除明细
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public virtual int DeleteAppDetails(List<double> ids)
        {
            int count = 0;
            count = DB.Delete<ApplyDetail>().Where(s => ids.Contains(s.Id)).Execute();
            return count;
        }
        #endregion
                
        /// <summary>
        /// 根据台账id 查到设备型号
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public virtual EquipAccount GetEquipModelByAccountId(double accountId)
        {
            var account = RF.GetById<EquipAccount>(accountId, new EagerLoadOptions()
                .LoadWith(EquipAccount.EquipModelProperty)
                .LoadWith(EquipAccount.UseDepartmentProperty)
                .LoadWith(EquipModel.EquipTypeProperty)
                );


            return account;
        }

        /// <summary>
        /// 设备型号 因为子表要控制数据源所以每改变一次就要保存一下
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="equipModelId"></param>
        public virtual void SaveEquipModel(double appId, double equipModelId)
        {
            var app = RF.GetById<SparePartApp>(appId);
            app.EquipModelId = equipModelId;
            RF.Save(app);
        }

        /// <summary>
        /// 获取备件基础信息
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual EntityList<SparePart> GetPartByEquipModel(ApplyDetail entity, PagingInfo p)
        {
            //筛选出与原申请单同设备型号的备件信息 或者 设备型号为空的备件信息

            var query = DB.Query<SparePart>();
            //if (entity.SparePart.SparePartType.EquipModelId.HasValue)
            //{
            //    query.Where(c => c.SparePartType.EquipModelId == entity.SparePart.SparePartType.EquipModelId || c.SparePartType.EquipModelId == null);
            //}
            //else
            //{
            //    query.Where(c => c.SparePartType.EquipModelId == null);
            //}

            var result = query.ToList(p, new EagerLoadOptions().LoadWithViewProperty());
            return result;
        }

        /// <summary>
        /// 得到领用部门
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public virtual EntityList<Enterprise> GetEnterprise(PagingInfo p)
        {
            var enterpriseLevels = DB.Query<Enterprise>().Where(e => e.Level.Type == EnterpriseType.Department).ToList(p, new EagerLoadOptions().LoadWithViewProperty());


            //List<double> ids = new List<double>();
            //foreach (var item in enterpriseLevels)
            //{
            //    ids.Add(item.Id);
            //}
            //var result = DB.Query<Enterprise>()
            //    //.Where(a =>ids.Contains( a.LevelId))
            //    .ToList(p, null);

            return enterpriseLevels;
        }

        /// <summary>
        ///  找到备件库存
        /// </summary>
        /// <param name="appId">备件Id</param>
        /// <param name="depotId">仓库Id</param>
        /// <returns></returns>
        public virtual StoreSummary GetStoreSummary(double appId, double depotId)
        {
            StoreSummary storeSummary = DB.Query<StoreSummary>()
                .Where(p => p.SparePartId == appId)
                .LeftJoin<StoreSummaryDetail>((d, s) => d.Id == s.StoreSummaryId && s.WarehouseId == depotId)
                .FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());

            return storeSummary;
        }

        /// <summary>
        /// 获取已出库备件申请单明细(层级逻辑)
        /// </summary>
        /// <param name="sparePartId"></param>
        /// <param name="equipAccountId"></param>
        /// <param name="modelId"></param>
        /// <param name="sourceNo"></param>
        /// <param name="fromType"></param>
        /// <param name="pagingInfo"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual EntityList<ApplyDetail> GetSparePartAppDtls(double sparePartId, double equipAccountId, double modelId, string sourceNo, FromType fromType, PagingInfo pagingInfo, string key)
        {
            var elo = new EagerLoadOptions();
            elo.LoadWith(ApplyDetail.SparePartAppProperty);
            elo.LoadWith(ApplyDetail.SparePartProperty);
            elo.LoadWith(SparePartApp.EquipAccountProperty);
            elo.LoadWithViewProperty();

            var sourceNoQueryer = this.GetSparePartAppDtlQueryer(sparePartId, key, fromType);
            sourceNoQueryer.Where(p => p.SparePartApp.FromNo == sourceNo);
            var list = sourceNoQueryer.ToList(pagingInfo, elo);

            //当以来源单号为条件查不出数据时，查询设备型号数据
            if (list.Count <= 0)
            {
                var equipQueryer = this.GetSparePartAppDtlQueryer(sparePartId, key, fromType);
                equipQueryer.Where(p => p.SparePartApp.EquipAccountId == equipAccountId);
                list = equipQueryer.ToList(pagingInfo, elo);
            }

            //当以设备为条件查不出数据时，查询设备型号数据
            if (list.Count <= 0)
            {
                var noEquipQueryer = this.GetSparePartAppDtlQueryer(sparePartId, key, fromType);
                noEquipQueryer.Where(p => p.SparePartApp.EquipAccountId == null && p.SparePartApp.EquipModelId == modelId);
                list = noEquipQueryer.ToList(pagingInfo, elo);
            }

            //当以设备/设备型号为条件查不出数据时，查询设备和设备型号为空的数据
            if (list.Count <= 0)
            {
                var noEquipAndModelQueryer = this.GetSparePartAppDtlQueryer(sparePartId, key, fromType);
                noEquipAndModelQueryer.Where(p => p.SparePartApp.EquipAccountId == null && p.SparePartApp.EquipModelId == null);
                list = noEquipAndModelQueryer.ToList(pagingInfo, elo);
            }

            return list;
        }

        /// <summary>
        /// 获取已出库备件申请单明细 查询器
        /// </summary>
        /// <param name="sparePartId"></param>
        /// <param name="key"></param>
        /// <param name="fromType"></param>
        /// <returns></returns>
        private IEntityQueryer<ApplyDetail> GetSparePartAppDtlQueryer(double sparePartId, string key, FromType fromType)
        {
            var q = Query<ApplyDetail>();
            q.Where(p => p.SparePartId == sparePartId);
            q.Where(p => p.SparePartApp.FromType == fromType);
            q.Where(p => p.SparePartApp.AuditState == AuditState.Butbounded);
            q.Where(p => p.UseAmount < p.OutDepotAmount);

            if (key.IsNotEmpty())
                q.Where(p => p.SparePartApp.No.Contains(key));

            return q;
        }

        /// <summary>
        /// 设备点检执行UI生成备件申请单
        /// </summary>
        /// <param name="uiCheckPlanSpareParts">备件更换清单</param>
        public virtual void UIGenerateSparePartApp(List<CheckPlanSparePartApl> uiCheckPlanSpareParts)
        {
            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                //先执行保存更改数据
                uiCheckPlanSpareParts.ForEach(p => RF.Save(p));

                var checkPlan = uiCheckPlanSpareParts.FirstOrDefault().CheckPlan;
                GenerateCheckSparePartApp(checkPlan.Id);

                trans.Complete();
            }
        }

        /// <summary>
        /// 设备润滑执行UI生成备件申请单
        /// </summary>
        /// <param name="uiLubricationSpareParts">备件更换清单</param>
        public virtual void UIGenerateSparePartApp(List<LubricationSparePartApply> uiLubricationSpareParts)
        {
            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                //先执行保存更改数据
                uiLubricationSpareParts.ForEach(p => RF.Save(p));
                var lubrication = uiLubricationSpareParts.FirstOrDefault().Lubrication;
                GenerateLubricationSparePartApp(lubrication.Id);

                trans.Complete();
            }
        }

        /// <summary>
        /// 设备保养执行UI生成备件申请单
        /// </summary>
        /// <param name="uiMaintainPlanSpareParts">备件更换清单</param>
        public virtual void UIGenerateSparePartApp(List<MaintainPlanSparePartApl> uiMaintainPlanSpareParts)
        {
            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                //先执行保存更改数据
                uiMaintainPlanSpareParts.ForEach(p => RF.Save(p));

                var maintainPlan = uiMaintainPlanSpareParts.FirstOrDefault().MaintainPlan;
                GenerateMaintainSparePartApp(maintainPlan.Id, FromType.Maintain);

                trans.Complete();
            }
        }

        /// <summary>
        /// 执行设备点检执行生成备件申请单
        /// </summary>
        /// <param name="checkPlanId"></param>
        public virtual void GenerateCheckSparePartApp(double checkPlanId)
        {
            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                //从点检单ID查出点检备件申请数据
                var datas = RT.Service.Resolve<CheckController>().GetCheckPlanSparePartApls(checkPlanId, false);
                if (datas.Count <= 0) throw new ValidationException("没有备件申请数据".L10N());

                var checkPlan = datas.FirstOrDefault().CheckPlan;

                var equipAccount = checkPlan.EquipAccount;

                if (equipAccount == null)
                {
                    throw new ValidationException("点检计划【{0}】的设备资料为空。"
                        .L10nFormat(checkPlan.CheckPlanNo));
                }

                if (datas.Any(p => p.ApplyQty <= 0))
                    throw new ValidationException("存在备件申请项申请数量为0".L10nFormat());
                if (datas.Any(p => p.OutStockWarehouseId == null || p.OutStockWarehouseId == 0))
                    throw new ValidationException("存在备件申请项没有选择出库仓库".L10nFormat());

                //构建申请单实体
                SparePartApp sparePartApp = new SparePartApp();
                sparePartApp.No = this.GetNo();
                sparePartApp.FromType = FromType.SpotCheck;
                sparePartApp.FromNo = datas.FirstOrDefault().CheckPlan.CheckPlanNo;
                sparePartApp.DemandDate = DateTime.Now;
                sparePartApp.AuditState = AuditState.StandAudit;
                sparePartApp.EquipAccountId = equipAccount?.Id;
                sparePartApp.EquipModelId = equipAccount?.EquipModelId;

                if (!equipAccount.UseDepartmentId.HasValue)
                {
                    throw new ValidationException("设备【{0}】的使用部门为空，不能生成备件申请单。".L10nFormat(equipAccount.Code));
                }

                sparePartApp.GetDepartmentId = equipAccount.UseDepartmentId.Value;

                sparePartApp.QualityStatus = SIE.Equipments.Enums.QualityStatus.Good;
                RF.Save(sparePartApp);

                //构建申请单明细
                datas.ForEach(p =>
                {
                    var dtl = new ApplyDetail();
                    dtl.SparePartAppId = sparePartApp.Id;
                    dtl.SparePartId = p.SparePartId;                    
                    dtl.WarehouseId = p.OutStockWarehouseId.Value;
                    dtl.ApplyAmount = p.ApplyQty;
                    dtl.GenerateId();
                    RF.Save(dtl);

                    DB.Update<CheckPlanSparePartApl>().Where(x => x.Id == p.Id)
                        .Set(x => x.ApplyDetailId, dtl.Id)
                        .Set(x => x.IsApply, true).Execute();
                });

                trans.Complete();
            }
        }


        /// <summary>
        /// 执行润滑执行生成备件申请单
        /// </summary>
        /// <param name="lubricationId"></param>
        public virtual void GenerateLubricationSparePartApp(double lubricationId)
        {
            //从点检单ID查出点检备件申请数据
            var datas = RT.Service.Resolve<LubricationPlanController>().GetLubricationSparePartApls(lubricationId, false);
            if (datas.Count <= 0)
            {
                throw new ValidationException("没有备件申请数据".L10N());
            }
            var lubrication = datas.FirstOrDefault().Lubrication;
            var equipAccount = lubrication.EquipAccount;

            if (equipAccount == null)
            {
                throw new ValidationException("润滑计划【{0}】的设备资料为空。".L10nFormat(lubrication.LubricationNo));
            }

            if (datas.Any(p => p.ApplyQty <= 0))
            {
                throw new ValidationException("存在备件申请项申请数量为0".L10nFormat());
            }
            if (datas.Any(p => p.OutStockWarehouseId == null || p.OutStockWarehouseId == 0))
            {
                throw new ValidationException("存在备件申请项没有选择出库仓库".L10nFormat());
            }

            //构建申请单实体
            SparePartApp sparePartApp = new SparePartApp();
            sparePartApp.No = this.GetNo();
            sparePartApp.FromType = FromType.Lubrication;
            sparePartApp.FromNo = datas.FirstOrDefault().Lubrication.LubricationNo;
            sparePartApp.DemandDate = DateTime.Now;
            sparePartApp.AuditState = AuditState.StandAudit;
            sparePartApp.EquipAccountId = equipAccount?.Id;
            sparePartApp.EquipModelId = equipAccount?.EquipModelId;

            if (!equipAccount.UseDepartmentId.HasValue)
            {
                throw new ValidationException("设备【{0}】的使用部门为空，不能生成备件申请单。".L10nFormat(equipAccount.Code));
            }

            sparePartApp.GetDepartmentId = equipAccount.UseDepartmentId.Value;
            sparePartApp.QualityStatus = SIE.Equipments.Enums.QualityStatus.Good;

            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                RF.Save(sparePartApp);
                //构建申请单明细
                datas.ForEach(p =>
                {
                    var dtl = new ApplyDetail();
                    dtl.SparePartAppId = sparePartApp.Id;
                    dtl.SparePartId = p.SparePartId;
                    dtl.WarehouseId = p.OutStockWarehouseId.Value;
                    dtl.ApplyAmount = p.ApplyQty;
                    dtl.GenerateId();
                    RF.Save(dtl);

                    DB.Update<LubricationSparePartApply>().Where(x => x.Id == p.Id)
                        .Set(x => x.ApplyDetailId, dtl.Id)
                        .Set(x => x.IsApply, true).Execute();
                });

                trans.Complete();
            }
        }

        /// <summary>
        /// 执行设备保养执行生成备件申请单
        /// </summary>
        /// <param name="maintainPlanId">保养计划ID</param>
        /// <param name="fromType">来源类型</param>
        public virtual void GenerateMaintainSparePartApp(double maintainPlanId, FromType fromType = FromType.Upkeep)
        {
            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                //从点检单ID查出点检备件更换数据
                var datas = RT.Service.Resolve<MaintainController>().GetMaintainPlanSparePartApls(maintainPlanId, false);
                if (datas.Count <= 0) throw new ValidationException("没有备件申请数据".L10N());

                var maintainPlan = datas.FirstOrDefault().MaintainPlan;
                var equipAccount = maintainPlan.EquipAccount ?? throw new ValidationException("保养计划【{0}】的设备资料为空。"
                        .L10nFormat(maintainPlan.MaintainNo));
                if (datas.Any(p => p.ApplyQty <= 0))
                    throw new ValidationException("存在备件申请项申请数量为0".L10nFormat());
                if (datas.Any(p => p.OutStockWarehouseId == null || p.OutStockWarehouseId == 0))
                    throw new ValidationException("存在备件申请项没有选择出库仓库".L10nFormat());


                //构建申请单实体
                SparePartApp sparePartApp = new SparePartApp();
                sparePartApp.No = this.GetNo();
                sparePartApp.FromType = fromType;
                sparePartApp.FromNo = datas.FirstOrDefault().MaintainPlan.MaintainNo;
                sparePartApp.DemandDate = DateTime.Now;
                sparePartApp.AuditState = AuditState.StandAudit;
                sparePartApp.EquipAccountId = equipAccount?.Id;
                sparePartApp.EquipModelId = equipAccount?.EquipModelId;

                if (!equipAccount.UseDepartmentId.HasValue)
                {
                    throw new ValidationException("设备【{0}】的使用部门为空，不能生成备件申请单。".L10nFormat(equipAccount.Code));
                }

                sparePartApp.GetDepartmentId = equipAccount.UseDepartmentId.Value;
                sparePartApp.QualityStatus = SIE.Equipments.Enums.QualityStatus.Good;
                RF.Save(sparePartApp);

                //构建申请单明细
                datas.ForEach(p =>
                {
                    var dtl = new ApplyDetail();
                    dtl.SparePartAppId = sparePartApp.Id;
                    dtl.SparePartId = p.SparePartId;                    
                    dtl.WarehouseId = p.OutStockWarehouseId.Value;
                    dtl.ApplyAmount = p.ApplyQty;
                    dtl.GenerateId();
                    RF.Save(dtl);

                    DB.Update<MaintainPlanSparePartApl>().Where(x => x.Id == p.Id)
                        .Set(x => x.ApplyDetailId, dtl.Id)
                        .Set(x => x.IsApply, true).Execute();
                });

                trans.Complete();
            }
        }

        /// <summary>
        /// 获取备件申请单明细
        /// </summary>
        /// <param name="sourceNo"></param>
        /// <param name="fromType"></param>
        public virtual EntityList<ApplyDetail> GetSparePartAppDtl(string sourceNo, FromType fromType)
        {
            var q = Query<ApplyDetail>();
            q.Where(p => p.SparePartApp.FromNo == sourceNo);
            q.Where(p => p.SparePartApp.FromType == fromType);
            q.Where(p => p.SparePartApp.AuditState == AuditState.Butbounded);
            q.Where(p => p.OutDepotAmount > p.UseAmount);
            return q.ToList();
        }

        /// <summary>
        /// 是否启用审批流
        /// </summary>
        /// <returns>bool</returns>
        public virtual bool IsEnableAuditFlow()
        {
            var config = ConfigService.GetConfig(new ApprovalConfig(), typeof(SparePartApp));
            if (config == null)
                throw new ValidationException("未找到是否启用审批流的配置,请检查配置项".L10N());
            return config.EnableAudit;
        }
    }
}

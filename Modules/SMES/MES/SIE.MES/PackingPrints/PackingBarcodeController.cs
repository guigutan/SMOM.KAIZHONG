using SIE.Barcodes;
using SIE.Common.Domain;
using SIE.Common.InvOrg;
using SIE.Common.NumberRules;
using SIE.Common.Prints;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EventMessages.MES.Barcodes;
using SIE.MES.PackingPrints.ViewModels;
using SIE.MES.WorkOrders;
using SIE.Packages.Packages;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.MES.PackingPrints
{
    /// <summary>
    /// 条码控制器
    /// </summary>
    public partial class PackingBarcodeController : DomainController
    {
        /// <summary>
        /// 根据包装条码打印条件获取工单
        /// </summary>
        /// <param name="criteria">包装条码打印查询实体</param>
        /// <exception cref="ArgumentNullException">参数为空</exception>
        /// <returns>工单列表</returns>
        public virtual EntityList<PackingWorkOrder> GetWorkOrders(PackingWorkOrderCriteria criteria)
        {
            Check.NotNull(criteria, nameof(criteria));
            var query = Query<PackingWorkOrder>();
            if (criteria.No.IsNotEmpty())
                query.Where(p => p.No.Contains(criteria.No));
            if (criteria.PlanBeginDate.BeginValue.HasValue)
                query.Where(p => p.PlanBeginDate >= criteria.PlanBeginDate.BeginValue);
            if (criteria.PlanBeginDate.EndValue.HasValue)
                query.Where(p => p.PlanBeginDate <= criteria.PlanBeginDate.EndValue);
            return query.OrderBy(criteria.OrderInfoList).ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据工单号获取包装号列表
        /// </summary>
        /// <param name="workOrderId">工单Id</param>
        /// <param name="pagingInfo">分页参数</param>
        /// <param name="sortInfo">排序参数</param>
        /// <returns>包装号列表</returns>
        public virtual EntityList<PackingBarcode> GetPackingBarcodeListByWorkOrderId(double workOrderId, PagingInfo pagingInfo, IList<OrderInfo> sortInfo)
        {
            return Query<PackingBarcode>().Where(p => p.WorkOrderId == workOrderId).OrderBy(sortInfo).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据工单和规则明细获取包装号数
        /// </summary>
        /// <param name="workOrderId">工单Id</param>
        /// <param name="detailId">包装关系Id</param>
        /// <returns>包装号数</returns>
        public virtual int GetPackingBarcodeCount(double workOrderId, double detailId)
        {
            return Query<PackingBarcode>().Where(p => p.WorkOrderId == workOrderId && p.PackageRuleDetailId == detailId).Count();
        }

        /// <summary>
        /// 根据工单Id获取工单包装规则
        /// </summary>
        /// <param name="workOrderId">工单Id</param>
        /// <param name="keyword">模糊查询</param>
        /// <param name="info">排序</param>
        /// <returns>工单包装规则列表</returns>
        public virtual EntityList<PackageRuleDetailViewModel> GetWorkOrderPackageRuleDetailList(double workOrderId, string keyword = "", PagingInfo info = null)
        {
            var reList = new EntityList<PackageRuleDetailViewModel>();
            var query = Query<WorkOrderPackageRuleDetail>().Where(p => p.WorkOrderId == workOrderId && p.IsPrint);
            if (keyword.IsNotEmpty())
            {
                query.Join<PackingUnit>((x, y) => x.PackageUnitId == y.Id && !y.IsMasterUnit && (y.Code.Contains(keyword) || y.Name.Contains(keyword)));
            }
            else
            {
                query.Join<PackingUnit>((x, y) => x.PackageUnitId == y.Id && !y.IsMasterUnit);
            }
            var list = query.ToList(info, new EagerLoadOptions().LoadWithViewProperty());
            foreach (var item in list)
            {
                reList.Add(new PackageRuleDetailViewModel()
                {
                    Id = item.Id.ToString(),
                    PackageUnitName = item.PackageUnitName,
                    Qty = item.Qty,
                    LevelQty = item.Qty,
                    NumberRuleName = item.NumberRuleName,
                    IsPrint = item.IsPrint,
                    TemplateName = item.TemplateName,
                });
            }
            reList.SetTotalCount(list.TotalCount);
            return reList;
        }

        /// <summary>
        /// 根据包装规则Id获取工单包装规则
        /// </summary>
        /// <param name="detailId">包装规则Id</param>
        /// <returns>工单包装规则</returns>
        public virtual WorkOrderPackageRuleDetail GetWorkOrderPackageRuleDetail(double detailId)
        {
            return Query<WorkOrderPackageRuleDetail>().Where(p => p.Id == detailId).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 创建包装号
        /// </summary>
        /// <param name="info">打印信息</param>
        /// <returns>错误信息和包装号列表</returns>
        public virtual Tuple<string, EntityList<PackingBarcode>> PrintPackingBarcodes(PackingPrinterInfo info)
        {
            string errMsg = string.Empty;
            EntityList<PackingBarcode> packingBarcodes = new EntityList<PackingBarcode>();
            try
            {
                packingBarcodes.AddRange(Print(info));
            }
            catch (Exception exc)
            {
                errMsg = exc.Message;
            }
            return new Tuple<string, EntityList<PackingBarcode>>(errMsg, packingBarcodes);
        }

        /// <summary>
        /// 创建包装号
        /// </summary>
        /// <param name="info">打印信息</param>
        /// <returns>包装号列表</returns>
        public virtual EntityList<PackingBarcode> Print(PackingPrinterInfo info)
        {
            using (var tran = DB.TransactionScope(BarcodeEntityDataProvider.ConnectionStringName))
            {
                var packageRuleDetail = GetById<WorkOrderPackageRuleDetail>(info.PackageRuleDetailId);
                if (packageRuleDetail == null)
                    throw new EntityNotFoundException(typeof(WorkOrderPackageRuleDetail), info.PackageRuleDetailId);
                var numberRule = GetById<NumberRule>(info.NumberRuleId);
                if (numberRule == null)
                    throw new EntityNotFoundException(typeof(NumberRule), info.NumberRuleId);
                var template = GetById<PrintTemplate>(info.TemplateId);
                if (template == null)
                    throw new EntityNotFoundException(typeof(PrintTemplate), info.TemplateId);
                if (info.PrintQty <= 0)
                    throw new ValidationException("打印数量必须大于0".L10N());
                var packingBarcodes = RT.Service.Resolve<NumberRuleController>().GenerateSegment(numberRule.Id, info.PrintQty, packageRuleDetail);
                var packingBarcodeList = new EntityList<PackingBarcode>();
                var now = RF.Find<PackingBarcode>().GetDbTime();
                foreach (var sn in packingBarcodes)
                {
                    var packingBarcode = new PackingBarcode()
                    {
                        Code = sn,
                        PrintDate = now,
                        PrintTimes = info.PageCount,
                        IsUse = false,
                        WorkOrderId = info.WorkOrderId,
                        PackageRuleDetailId = Convert.ToDouble(info.PackageRuleDetailId),
                        PrintedState = BarcodeState.Printed,
                        PrintById = AppRuntime.IdentityId,
                    };
                    packingBarcode.CreateBy = RT.IdentityId;
                    packingBarcode.CreateDate = now;
                    packingBarcode.UpdateBy = RT.IdentityId;
                    packingBarcode.UpdateDate = now;
                    InvOrgIdExtension.SetInvOrgId(packingBarcode, RT.InvOrg);
                    PhantomEntityExtension.SetIsPhantom(packingBarcode, false);
                    packingBarcodeList.Add(packingBarcode);
                }
                var Logger = Logging.LogManager.GetLogger("startup_logger");
                using (Diagnostics.PerformenceWatcher.Start(Logger, "批量保存包装号列表"))
                {
                    var existSns = packingBarcodes.SplitContains(codes => Query<PackingBarcode>().Where(p => codes.Contains(p.Code)).ToList()).Select(p => p.Code).ToList();
                    if (existSns.Count > 0)
                        throw new ValidationException("已经存在包装号：{0}".L10nFormat(string.Join(";", existSns)));
                    BulkSaver.SetBatchEntityId(packingBarcodeList);
                    RF.BatchInsert(packingBarcodeList);
                }
                tran.Complete();

                //推送打印条码消息到边端
                var packingBarcodeInfo = new PackingBarcodeInfo();
                packingBarcodeInfo.MsgType = "5";
                packingBarcodeInfo.WorkOrderNo = packingBarcodeList[0].WorkOrder.No;

                foreach (var packingBarcode in packingBarcodeList)
                {
                    var packingCode = new PackingCode();
                    packingCode.Code = packingBarcode.Code;
                    packingCode.PackUnitName = packingBarcode.PackageRuleDetail.PackageUnit.Name;
                    packingCode.IsUse = packingBarcode.IsUse;
                    packingBarcodeInfo.PackingCodeList.Add(packingCode);
                }
                RT.EventBus.Publish<PackingBarcodeInfo>(packingBarcodeInfo);
                return packingBarcodeList;
            }
        }

        /// <summary>
        /// 补打
        /// </summary>
        /// <param name="packingBarcodeList">选中条码列表</param>
        /// <param name="reason">补打原因</param>
        /// <param name="times">补打次数</param> 
        /// <returns>补打结果</returns>
        public virtual void ReprintPackingBarcode(EntityList<PackingBarcode> packingBarcodeList, string reason, int times)
        {
            if (reason.IsNullOrWhiteSpace())
            {
                throw new ValidationException("补打原因不允许为空.".L10N());
            }
            if (packingBarcodeList.Count == 0)
                throw new ArgumentNullException(nameof(packingBarcodeList));
            if (times < 1)
                throw new ValidationException("打印次数：{0} 必须大于等于 1".L10nFormat(times));
            foreach (var packingBarcode in packingBarcodeList)
            {
                packingBarcode.PrintDate = RF.Find<PackingBarcode>().GetDbTime();
                packingBarcode.PrintById = AppRuntime.IdentityId;
                packingBarcode.PrintedState = BarcodeState.Printed;
                packingBarcode.PrintTimes += times;
                RF.Save(packingBarcode);
            }
        }

        /// <summary>
        /// 获取包装号Id所对应的所有包装号信息列表
        /// </summary>
        /// <param name="packingIds">包装号Ids列表</param>
        /// <returns>包装号信息列表</returns>
        public virtual EntityList<PackingBarcode> GetPackingBarcodesByIds(List<double> packingIds)
        {
            return Query<PackingBarcode>().Where(p => packingIds.Contains(p.Id)).ToList();
        }

        /// <summary>
        /// 获取包装号
        /// </summary>
        /// <param name="packageNo">包装号</param>
        public virtual PackingBarcode GetPackingBarcode(string packageNo)
        {
            return Query<PackingBarcode>().Where(p => p.Code == packageNo)
                .FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 验证包装号
        /// </summary>
        /// <param name="packageNo">包装号</param>
        /// <param name="packingUnit">包装单位</param>
        /// <param name="currentPackingRelation">包装关系</param>
        /// <exception cref="ValidationException">包装号不存在、包装号已使用</exception>
        public virtual void ValidatePackingBarcode(string packageNo, PackingUnit packingUnit,
            Packages.PackingRelation currentPackingRelation)
        {
            if (packingUnit == null)
            {
                throw new ArgumentNullException(nameof(packingUnit));
            }

            if (packageNo.IsNullOrEmpty())
            {
                throw new ValidationException("包装号不存在".L10N());
            }

            var packingBarcode = GetPackingBarcode(packageNo);

            if (packingBarcode == null)
            {
                throw new ValidationException("包装号[{0}]不存在".L10nFormat(packageNo));
            }

            if (packingBarcode.IsUse)
            {
                throw new ValidationException("包装号[{0}]已使用".L10nFormat(packageNo));
            }

            if (packingBarcode.PackageUnitId != packingUnit.Id)
            {
                throw new ValidationException("包装号【{0}】包装单位是【{1}】与要扫描的包装单位不相符，请扫描【{2}】的包装号"
                    .L10nFormat(packageNo, packingBarcode.PackageUnitName, packingUnit.Name));
            }

            if (packingBarcode.IsUse)
            {
                throw new ValidationException("包装号[{0}]已使用".L10nFormat(packageNo));
            }

            if (currentPackingRelation != null
                && packingBarcode.WorkOrderId != currentPackingRelation.WorkOrderId)
            {
                throw new ValidationException("包装号[{0}]的工单与当前正在包装的工单不相同".L10nFormat(packageNo));
            }
        }
    }
}
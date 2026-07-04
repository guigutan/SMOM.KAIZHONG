using SIE.Common.Catalogs;
using SIE.Common.Sort;
using SIE.Core.Enums;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EMS.AssetDisposals;
using SIE.EMS.AssetIssues;
using SIE.EMS.AssetScraps;
using SIE.EMS.AssetTransfers;
using SIE.EMS.Checks.Plans;
using SIE.EMS.DevicePurs;
using SIE.EMS.Equipments.Accounts;
using SIE.EMS.Equipments.Accounts.Criterias;
using SIE.EMS.Equipments.Accounts.ViewModels;
using SIE.EMS.Equipments.Models;
using SIE.EMS.FixedAssets.Accounts;
using SIE.EMS.MainenanceProjects;
using SIE.EMS.Maintains.Plans;
using SIE.Equipments.Configs;
using SIE.Equipments.DeviceIOTParas;
using SIE.Equipments.DeviceIOTParas.Details;
using SIE.Equipments.Enums;
using SIE.Equipments.EquipAccountLocations;
using SIE.Equipments.EquipAccounts;
using SIE.Equipments.EquipModels;
using SIE.Equipments.EquipTypes;
using SIE.EventMessages.EMS.EquipAccount;
using SIE.Rbac.Users;
using SIE.Resources.CalendarSchemes;
using SIE.Resources.WipResources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using SIE.Common.Configs;
using SIE.EMS.Equipments.ApiModels;
using SIE.Resources.Enterprises;
using SIE.Resources.Employees;

namespace SIE.EMS.Equipments
{
    /// <summary>
    /// 设备台账控制器
    /// </summary>
    public partial class EquipController : DomainController, IEquipAccount
    {
        /// <summary>
        /// 根据设备Id查询设备列表
        /// </summary>
        /// <param name="equipIds">设备Id集合</param>
        /// <returns>设备列表</returns>
        public virtual EntityList<EquipAccount> GetEquipAccountListByIds(List<double> equipIds)
        {
            return equipIds.SplitContains(tempIds =>
            {
                return Query<EquipAccount>()
                .Where(p => p.UseState == AccountUseState.Using)
                .Where(n => tempIds.Contains(n.Id))
                .ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
        }

        /// <summary>
        /// 获取设备台账列表
        /// Expression不支持序列号，前端不要调用
        /// </summary>
        /// <param name="exp">过滤条件</param>
        /// <param name="pagingInfo">分页条件</param>
        /// <returns>设备台账列表</returns>
        public virtual EntityList<EquipAccount> GetEquipAccountsByExpression(Expression<Func<EquipAccount, bool>> exp, PagingInfo pagingInfo)
        {
            var query = Query<EquipAccount>();
            if (exp != null)
            {
                query.Where(exp);
            }
            return query.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取设备台账
        /// Expression不支持序列号，前端不要调用
        /// </summary>
        /// <param name="exp">过滤条件</param>
        /// <returns>设备台账</returns>
        public virtual EquipAccount GetEquipAccount(Expression<Func<EquipAccount, bool>> exp)
        {
            var query = Query<EquipAccount>();
            if (exp != null)
            {
                query.Where(exp);
            }
            return query.FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取设备台账
        /// </summary>
        /// <param name="equipCode">设备编码</param>
        /// <returns></returns>
        public virtual EquipAccount GetEquipAccount(string equipCode)
        {
                var q = Query<EquipAccount>();
                q.Where(p => p.Code == equipCode);

                var account = q.FirstOrDefault();
                return account;
        }

        /// <summary>
        /// 获取设备台账
        /// </summary>
        /// <param name="equipCode">设备编码</param>
        /// <returns></returns>
        public virtual EquipAccount GetEquipAccountNoAuth(string equipCode)
        {
        using (SIE.DataAuth.DataAuths.LoadAll())
        {
            var q = Query<EquipAccount>();
            q.Where(p => p.Code == equipCode);

            var account = q.FirstOrDefault();
            return account;
        }
        }

        /// <summary>
        /// 获取设备编码
        /// </summary>
        /// <returns></returns>
        public virtual string GetEquipCode(double equipTypeId)
        {
            var equipType = RF.GetById<EquipType>(equipTypeId);
            var equipCount = Query<EquipAccount>().Exists<EquipModel>((a, b) => b.Where(f => f.Id == a.EquipModelId && f.EquipTypeId == equipTypeId)).Count();
            equipType.Num = equipCount + 1;
            return equipType.TypeCode + "-" + equipType.Num.ToString("0000");
        }

        /// <summary>
        /// 根据保养单号查询实体获取台账
        /// </summary>
        /// <returns></returns>
        public virtual EntityList<EquipAccount> GetEquipAccountsByMaintainPlanCriteria(MaintainPlanCriteria criteria)
        {
            var query = Query<EquipAccount>();
            if (criteria.EquipCode.IsNotEmpty())
            {
                query.Where(w => w.Code.Contains(criteria.EquipCode));
            }
            if (criteria.WorkshopId.HasValue)
            {
                query.Where(w => w.WorkShopId == criteria.WorkshopId);
            }
            if (criteria.MachineNo.IsNotEmpty())
            {
                query.Where(w => w.Name.Contains(criteria.MachineNo));
            }
            return query.ToList(new PagingInfo() { PageSize = 999999999 }, new EagerLoadOptions().LoadWithViewProperty().LoadWith(EquipAccount.WorkShopProperty).LoadWith(EquipAccount.ProcessProperty));
        }

        /// <summary>
        /// 判断是否要显示所有设备
        /// </summary>
        /// <returns>bool</returns>
        public virtual bool IsShowAllEquipAccount()
        {
            var deviceBillList = Query<DeviceBill>()
                                .LeftJoin<UserInUserGroup>((a, b) => a.DevicePur.UserGroupId == b.UserGroupId)
                                .Where<UserInUserGroup>((p, g) => p.DevicePur.UserId == RT.Identity.UserId || g.UserId == RT.Identity.UserId)
                                .ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            return !deviceBillList.Any();
        }

        /// <summary>
        /// 根据点检查询实体获取台账(新)
        /// </summary>
        /// <returns></returns>
        public virtual EntityList<EquipAccountSelect> CriteriaEquipForCheckPlans(CheckPlanCriteria criteria, bool isShowAllEquipAccount = false)
        {
            var query = Query<EquipAccountSelect>();
            if (criteria.EquipAccountId.HasValue && criteria.EquipAccountId != 0)
            {
                query.Where(w => w.Id == criteria.EquipAccountId);
            }
            if (criteria.EquipAccountName.IsNotEmpty())
            {
                query.Where(w => w.Name.Contains(criteria.EquipAccountName));
            }
            if (criteria.EquipModelId.HasValue && criteria.EquipModelId != 0)
            {
                query.Where(w => w.EquipModelId == criteria.EquipModelId);
            }
            if (criteria.TypeCategory.IsNotEmpty())
            {
                query.Where(w => w.EquipModel.TypeCategory == criteria.TypeCategory);
            }
            if (criteria.WipResourceId.HasValue && criteria.WipResourceId != 0)
            {
                query.Where(w => w.ResourceId == criteria.WipResourceId);
            }
            if (criteria.WorkshopId.HasValue && criteria.WorkshopId != 0)
            {
                query.Where(w => w.WorkShopId == criteria.WorkshopId);
            }
            if (criteria.State.HasValue)
            {
                query.Where(w => w.State == criteria.State);
            }
            if (criteria.UseState.HasValue)
            {
                query.Where(w => w.UseState == criteria.UseState);
            }
            if (criteria.ManageDepartmentId.HasValue)
            {
                query.Where(w => w.ManageDepartmentId == criteria.ManageDepartmentId);
            }

            var list = query.ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
            return list;
        }

        /// <summary>
        /// 根据保养查询实体获取台账(新)
        /// </summary>
        /// <returns></returns>
        public virtual EntityList<EquipAccountSelect> CriteriaEquipForMaintainPlans(MaintainPlanCriteria criteria, bool isShowAllEquipAccount = false)
        {
            var query = Query<EquipAccountSelect>();

            if (criteria.EquipAccountId.HasValue && criteria.EquipAccountId != 0)
            {
                query.Where(w => w.Id == criteria.EquipAccountId);
            }

            if (criteria.MachineNo.IsNotEmpty())
            {
                query.Where(w => w.Name.Contains(criteria.MachineNo));
            }

            if (criteria.EquipModelId.HasValue && criteria.EquipModelId != 0)
            {
                query.Where(w => w.EquipModelId == criteria.EquipModelId);
            }

            if (criteria.EquipTypeCategory.IsNotEmpty())
            {
                query.Where(w => w.EquipModel.TypeCategory == criteria.EquipTypeCategory);
            }

            if (criteria.WipResourceId.HasValue && criteria.WipResourceId != 0)
            {
                query.Where(w => w.ResourceId == criteria.WipResourceId);
            }

            if (criteria.WorkshopId.HasValue && criteria.WorkshopId != 0)
            {
                query.Where(w => w.WorkShopId == criteria.WorkshopId);
            }

            if (criteria.UseState.HasValue)
            {
                query.Where(p => p.UseState == criteria.UseState);
            }
            return query.ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据点检单号查询实体获取台账
        /// </summary>
        /// <returns></returns>
        public virtual EntityList<EquipAccount> GetEquipAccountsByCheckPlanCriteria(CheckPlanCriteria criteria)
        {
            var query = Query<EquipAccount>();
            if (criteria.EquipCode.IsNotEmpty())
            {
                query.Where(w => w.Code.Contains(criteria.EquipCode));
            }
            if (criteria.WorkshopId.HasValue)
            {
                query.Where(w => w.WorkShopId == criteria.WorkshopId);
            }
            if (criteria.ProcessId.HasValue)
            {
                query.Where(w => w.ProcessId == criteria.ProcessId);
            }
            if (criteria.MachineNo.IsNotEmpty())
            {
                query.Where(w => w.Name.Contains(criteria.MachineNo));
            }
            if (criteria.CheckCycleType.HasValue)
            {
                query.Exists<EquipAccountCheckProject>(
                    (x, y) => y.Join<ProjectDetail>((c, d) => c.ProjectDetailId == d.Id && d.CycleType == (criteria.CheckCycleType == CheckCycleType.Day ? CycleType.Day : CycleType.Class))
                        .Where(p => p.EquipAccountId == x.Id));
            }
            return query.ToList(new PagingInfo() { PageSize = 999999999 }, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 查询需校验的设备台账
        /// </summary>
        /// <returns></returns>
        public virtual EntityList<EquipAccount> GetCalibrationEquipAccounts(PagingInfo page, string keyword)
        {
            var query = Query<EquipAccount>();
            query.Join<EquipModel>((ac, m) => ac.EquipModelId == m.Id);
            query.Join<EquipModel, EquipType>((m, t) => m.EquipTypeId == t.Id);
            query.WhereIf(keyword.IsNotEmpty(), p => p.Code.Contains(keyword) || p.Name.Contains(keyword));
            return query.ToList(page, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 查询相关产线下的设备台账
        /// </summary>
        /// <param name="resourceId">产线Id</param>
        /// <param name="pagingInfo">分页</param>
        /// <returns>设备台账列表</returns>
        public virtual EntityList<EquipAccount> GetCheckPlanEquipAccountsByResourceId(double resourceId, PagingInfo pagingInfo)
        {
            var query = Query<EquipAccount>().Where(p => p.UseState == AccountUseState.Using && p.ResourceId == resourceId);
            var list = query.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
            list.ForEach(account => { account.TreePId = null; });
            return list;
        }

        /// <summary>
        /// 查询设备台账信息
        /// </summary>
        /// <param name="pagingInfo"></param>
        /// <param name="equipModelId"></param>
        /// <param name="equipTypeId"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public virtual EntityList<EquipAccount> GetEquipAccounts(PagingInfo pagingInfo, double? equipModelId, double? equipTypeId, string keyword)
        {
            bool hasFilter = false;

            var q = Query<EquipAccount>();

            if (equipModelId.HasValue)
            {
                q.Where(p => p.EquipModelId == equipModelId);

                hasFilter = true;
            }

            if (equipTypeId.HasValue)
            {
                q.Where(p => p.EquipModel.EquipTypeId == equipTypeId);
                hasFilter = true;
            }

            if (keyword.IsNotEmpty())
            {
                q.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword));
                hasFilter = true;
            }
            var equipAccounts = q.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());

            if (hasFilter)
            {
                equipAccounts.ForEach(x => x.TreePId = null);
            }

            return equipAccounts;
        }

        /// <summary>
        /// 查询设备台账信息
        /// </summary>
        /// <param name="pagingInfo">分页信息</param>
        /// <param name="equipWarehouseId">资产领用仓库Id</param>
        /// <param name="equipModelId">设备型号Id</param>
        /// <param name="equipTypeId">设备类型Id</param>
        /// <param name="factoryId">工厂Id</param>
        /// <param name="lendingDepartmentId">借出部门Id</param>
        /// <param name="keyword">查询关键字</param>
        /// <returns>设备台账列表</returns>
        public virtual EntityList<EquipAccountSelect> GetEquipAccounts(PagingInfo pagingInfo, double? equipWarehouseId, double? equipModelId, double? equipTypeId, double? factoryId, double? lendingDepartmentId, string keyword)
        {
            var q = Query<EquipAccountSelect>().Where(p => p.Frozen == YesNo.No && p.UseState != AccountUseState.Scrap && p.UseState != AccountUseState.ToAccepted && p.UseState != AccountUseState.Lease && p.UseState != AccountUseState.DisposedOf);
            if (equipModelId.HasValue)
            {
                q.Where(p => p.EquipModelId == equipModelId);
            }
            if (equipTypeId.HasValue)
            {
                q.Where(p => p.EquipModel.EquipTypeId == equipTypeId);
            }
            if (factoryId.HasValue)
            {
                q.Where(p => p.FactoryId == factoryId);
            }
            if (lendingDepartmentId.HasValue)
            {
                q.Where(p => p.ManageDepartmentId == lendingDepartmentId);
            }
            if (keyword.IsNotEmpty())
            {
                q.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword));
            }
            q.Where(p => p.WarehouseId == equipWarehouseId);
            var list = q.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());

            list.ForEach(p =>
            {
                p.OriginalValue = 1;//用来带出资产领用单设备清单中的申请数量
                p.TreePId = null;
            });

            return list;
        }

        /// <summary>
        /// 根据发放设备清单明细行获取设备台账信息
        /// </summary>
        /// <param name="pagingInfo">分页信息</param>
        /// <param name="issueEquip">发放设备清单明细</param>
        /// <param name="keyword">查询关键字</param>
        /// <returns>设备台账列表</returns>
        public virtual EntityList<EquipAccount> GetEquipAccounts(PagingInfo pagingInfo, AssetIssueEquipment issueEquip, string keyword)
        {
            var q = Query<EquipAccount>().Where(p => p.Frozen == YesNo.No && p.UseState != AccountUseState.Scrap && p.UseState != AccountUseState.ToAccepted && p.UseState != AccountUseState.Lease && p.UseState != AccountUseState.DisposedOf);

            if (issueEquip.AssetRequisitionEquipment.EquipModel != null)
            {
                q.Where(p => p.EquipModel.Code == issueEquip.AssetRequisitionEquipment.EquipModel.Code);
            }
            if (issueEquip.AssetRequisitionEquipment.EquipType != null)
            {
                q.Where(p => p.EquipModel.EquipType.TypeCode == issueEquip.AssetRequisitionEquipment.EquipType.TypeCode);
            }
            if (issueEquip.FactoryId.HasValue)
            {
                q.Where(p => p.FactoryId == issueEquip.FactoryId);
            }
            if (issueEquip.LendingDepartmentId.HasValue)
            {
                q.Where(p => p.ManageDepartmentId == issueEquip.LendingDepartmentId);
            }
            if (keyword.IsNotEmpty())
            {
                q.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword));
            }
            q.NotExists<AssetIssueEquipment>((x, y) => y.Where(p => p.AssetIssue.ApprovalStatus != ApprovalStatus.Audited && p.EquipAccountId == x.Id && p.AssetIssueId != issueEquip.AssetIssueId));
            q.NotExists<AssetTransferDetail>((x, z) => z.Where(p => p.AssetTransfer.ApprovalStatus != ApprovalStatus.Audited && p.EquipAccountId == x.Id));

            var list = q.ToList(pagingInfo);
            list.ForEach(p =>
            {
                p.TreePId = null;
            });
            return list;
        }

        /// <summary>
        /// 根据报废设备清单明细行获取设备台账信息
        /// </summary>
        /// <param name="pagingInfo">分页信息</param>
        /// <param name="scrapEquip">报废设备清单明细</param>
        /// <param name="keyword">查询关键字</param>
        /// <returns>设备台账列表</returns>
        public virtual EntityList<EquipAccountSelect> GetEquipAccounts(PagingInfo pagingInfo, AssetScrapEquipment scrapEquip, string keyword)
        {
            var q = Query<EquipAccountSelect>().Where(p => p.UseState == AccountUseState.Using || p.UseState == AccountUseState.InIdle ||
                                                     p.UseState == AccountUseState.Archive || p.UseState == AccountUseState.Repair || p.UseState == AccountUseState.OutsourcedRepair);

            if (scrapEquip.FactoryId.HasValue)
            {
                q.Where(p => p.FactoryId == scrapEquip.FactoryId);
            }
            if (scrapEquip.ManageDeptId.HasValue)
            {
                q.Where(p => p.ManageDepartmentId == scrapEquip.ManageDeptId);
            }
            if (scrapEquip.UseDeptId.HasValue)
            {
                q.Where(p => p.UseDepartmentId == scrapEquip.UseDeptId);
            }
            if (scrapEquip.WarehouseId.HasValue)
            {
                q.Where(p => p.WarehouseId == scrapEquip.WarehouseId);
            }
            if (scrapEquip.IsFixAsset)
            {
                q.Where(p => p.FixedAssetsAccountId != null);
            }
            if (keyword.IsNotEmpty())
            {
                q.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword));
            }

            var list = q.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());

            list.ForEach(p =>
            {
                p.TreePId = null;
            });
            return list;
        }

        /// <summary>
        /// 根据处置设备清单明细行获取设备台账信息
        /// </summary>
        /// <param name="pagingInfo">分页信息</param>
        /// <param name="disposalEquip">处置设备清单明细</param>
        /// <param name="keyword">查询关键字</param>
        /// <returns>设备台账列表</returns>
        public virtual EntityList<EquipAccount> GetEquipAccounts(PagingInfo pagingInfo, AssetDisposalEquipment disposalEquip, string keyword)
        {
            var q = Query<EquipAccount>().Where(p => p.UseState == AccountUseState.Scrap);

            if (disposalEquip.FactoryId.HasValue)
            {
                q.Where(p => p.FactoryId == disposalEquip.FactoryId);
            }
            if (disposalEquip.ManageDeptId.HasValue)
            {
                q.Where(p => p.ManageDepartmentId == disposalEquip.ManageDeptId);
            }
            if (disposalEquip.UseDeptId.HasValue)
            {
                q.Where(p => p.UseDepartmentId == disposalEquip.UseDeptId);
            }
            if (disposalEquip.WarehouseId.HasValue)
            {
                q.Where(p => p.WarehouseId == disposalEquip.WarehouseId);
            }
            if (disposalEquip.IsFixAsset)
            {
                q.Where(p => p.FixedAssetsAccountId != null);
            }
            if (keyword.IsNotEmpty())
            {
                q.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword));
            }

            var list = q.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());

            var assetScrapEquipList = list.Select(p => p.Id).SplitContains(tempIds =>
            {
                return Query<AssetScrapEquipment>().Where(p => tempIds.Contains(p.EquipAccountId)).ToList();
            });

            var fixedAssetEquipList = list.Where(p => p.FixedAssetsAccountId != null).Select(p => p.Id).SplitContains(tempIds =>
            {
                return Query<FixedAssetDeviceBill>().Where(p => tempIds.Contains(p.EquipAccountId) && p.IsMajor).ToList();
            });

            list.ForEach(account =>
            {
                var assetScrapEquip = assetScrapEquipList.FirstOrDefault(p => p.EquipAccountId == account.Id);
                var fixedAssetEquip = fixedAssetEquipList.FirstOrDefault(p => p.EquipAccountId == account.Id);

                if (assetScrapEquip != null)
                {
                    account.ScrapType = assetScrapEquip.ScrapType;
                    account.Reason = assetScrapEquip.Reason;
                }

                if (fixedAssetEquip == null)
                {
                    account.OriginalAssetsValue = 0;
                    account.NetAssetValue = 0;
                    account.DepreciationResidualValue = 0;
                }

                account.TreePId = null;
            });
            return list;
        }

        /// <summary>
        /// 查询设备列表
        /// </summary>
        /// <param name="pagingInfo">分页参数</param>
        /// <param name="keyword">关键字</param>
        /// <returns>设备列表</returns>
        public virtual EntityList<EquipAccount> GetEquipAccountsAuth(PagingInfo pagingInfo, string keyword)
        {
            using (SIE.DataAuth.DataAuths.LoadAll())
            {
                var query = Query<EquipAccount>();

                if (!keyword.IsNullOrEmpty())
                {
                    query.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword));
                }

                var equipAccounts = query.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());

                if (!keyword.IsNullOrEmpty())
                {
                    //有传过滤条件，则清空掉TreeId，断开树型结构，以免前端报错
                    equipAccounts.ForEach(p => p.TreePId = null);
                }

                return equipAccounts;
            }
        }

        /// <summary>
        /// 查询设备列表
        /// </summary>
        /// <param name="pagingInfo">分页参数</param>
        /// <param name="keyword">关键字</param>
        /// <returns>设备列表</returns>
        public virtual EntityList<EquipAccount> GetEquipAccounts(PagingInfo pagingInfo, string keyword)
        {
            var query = Query<EquipAccount>();

            if (!keyword.IsNullOrEmpty())
            {
                query.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword));
            }

            var equipAccounts = query.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());

            if (!keyword.IsNullOrEmpty())
            {
                //有传过滤条件，则清空掉TreeId，断开树型结构，以免前端报错
                equipAccounts.ForEach(p => p.TreePId = null);
            }

            return equipAccounts;
        }

        /// <summary>
        /// 根据保养单号查询实体获取台账
        /// </summary>
        /// <returns></returns>
        public virtual EntityList<EquipAccount> GetEquipAccountDatas(double? workshopId, double? processId, string lineIds, string keyword, PagingInfo pagingInfo)
        {
            var query = Query<EquipAccount>();
            if (workshopId.HasValue)
            {
                query.Where(w => w.WorkShopId == workshopId.Value);
            }
            if (processId.HasValue)
            {
                query.Where(w => w.ProcessId == processId.Value);
            }
            if (lineIds.IsNotEmpty())
            {
                var prodLineIdList = lineIds.Split(';').Select(p => Convert.ToDouble(p)).ToList();
                query.Where(p => p.ResourceId != null && prodLineIdList.Contains(p.ResourceId.Value));
            }
            if (keyword.IsNotEmpty())
            {
                query.Where(w => w.Code.Contains(keyword) || w.Name.Contains(keyword));
            }
            return query.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty().LoadWith(EquipAccount.WorkShopProperty).LoadWith(EquipAccount.ProcessProperty));
        }

        /// <summary>
        /// 根据编码获取设备台账
        /// </summary>
        /// <param name="code">设备台账编码</param>
        /// <returns></returns>
        public virtual EquipAccount GetEquipAccountsByCode(string code)
        {
            return Query<EquipAccount>().Where(w => w.Code == code).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据Ids获取设备台账
        /// </summary>
        /// <param name="ids">设备台账Id集合</param>
        /// <returns></returns>
        public virtual EntityList<EquipAccount> GetEquipAccountsByIds(List<double> ids)
        {
            return Query<EquipAccount>().Where(w => ids.Contains(w.Id)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }
        /// <summary>
        /// 根据设备型号Ids获取闲置的设备台账
        /// </summary>
        /// <param name="modelIds">设备型号Id集合</param>
        /// <returns></returns>
        public virtual EntityList<EquipAccount> GetEquipAccountsByModelIds(List<double> modelIds)
        {
            return Query<EquipAccount>().Where(w => modelIds.Contains(w.EquipModelId) && w.UseState == AccountUseState.InIdle).OrderBy(o => o.CreateDate).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 更新台帐状态
        /// </summary>
        /// <param name="state">状态</param>
        /// <param name="peopleId">更新人</param>
        /// <param name="ids">台帐Id集合</param>
        public virtual void UpdateEquipAccount(AccountUseState state, double peopleId, List<double> ids)
        {
            DB.Update<EquipAccount>()
                 .Set(f => f.UseState, state)
                 .Set(f => f.UpdateBy, peopleId)
                 .Set(f => f.UpdateDate, DateTime.Now)
                 .Where(w => ids.Contains(w.Id)).Execute();
        }

        /// <summary>
        /// 根据产线获取设备台账列表
        /// </summary>
        /// <param name="resourceId">产线ID</param>
        /// <param name="keyword">查询关键字</param>
        /// <param name="pagingInfo">分页</param>
        /// <returns>设备台账列表</returns>
        public virtual EntityList<EquipAccount> GetEquipAccountsByResourceId(double resourceId, string keyword, PagingInfo pagingInfo)
        {
            Expression<Func<EquipAccount, bool>> exp = p => p.ResourceId == resourceId;
            if (!string.IsNullOrEmpty(keyword))
            {
                exp = p => p.ResourceId == resourceId && (p.Code.Contains(keyword) || p.Name.Contains(keyword));
            }
            return GetEquipAccountsByExpression(exp, pagingInfo);
        }

        /// <summary>
        /// 获取设备列表
        /// </summary>
        /// <param name="resourceId">产线ID</param>
        /// <param name="equipIdList">排除的设备Id集合</param>
        /// <param name="key">查询字段</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <returns>设备列表</returns>
        public virtual EntityList<EquipAccount> GetLoadItemEquipAccountList(double resourceId, List<double> equipIdList, string key, PagingInfo pagingInfo)
        {
            Expression<Func<EquipAccount, bool>> exp = p => p.ResourceId == resourceId;
            if (equipIdList != null && equipIdList.Any())
            {
                exp = p => p.ResourceId == resourceId && equipIdList.Contains(p.Id);
                if (!string.IsNullOrEmpty(key))
                {
                    exp = p => p.ResourceId == resourceId && equipIdList.Contains(p.Id) && (p.Code.Contains(key) || p.Name.Contains(key));
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(key))
                {
                    exp = p => p.ResourceId == resourceId && (p.Code.Contains(key) || p.Name.Contains(key));
                }
            }
            return GetEquipAccountsByExpression(exp, pagingInfo);
        }

        /// <summary>
        /// 更新设备台账的仪器状态
        /// </summary>
        /// <param name="equipId"></param>
        /// <param name="state">仪器状态</param>
        /// <param name="accState">台账状态</param>
        /// <param name="useState">使用状态</param>
        public virtual void UpdateAccountQualityState(double equipId, QualityState state, AccountState accState, AccountUseState useState)
        {
            if (!CheckQualityState(equipId))
            {
                return;
            }
            DB.Update<EquipAccount>()
                .Set(p => p.QualityState, state)
                .Set(p => p.State, accState)
                .Set(p => p.UseState, useState)
                .Where(p => p.Id == equipId).Execute();
        }

        /// <summary>
        /// 更新设备状态
        /// </summary>
        /// <param name="equipId">设备台账ID</param>
        /// <param name="qualityState">仪器状态</param>
        /// <param name="accountState">设备状态</param>
        /// <param name="accountUseState">使用状态</param>
        public virtual void UpdateAccountState(double equipId, QualityState qualityState, AccountState accountState, AccountUseState accountUseState)
        {
            if (!CheckQualityState(equipId))
            {
                return;
            }

            DB.Update<EquipAccount>()
                .Set(p => p.QualityState, qualityState)
                .Set(p => p.State, accountState)
                .Set(p => p.UseState, accountUseState)
                .Set(f => f.UpdateBy, RT.IdentityId)
                .Set(f => f.UpdateDate, DateTime.Now)
                .Where(p => p.Id == equipId).Execute();
        }

        /// <summary>
        /// 校验台账状态
        /// </summary>
        /// <param name="equipId"></param>
        private bool CheckQualityState(double equipId)
        {
            var srcEquip = RF.GetById<EquipAccount>(equipId);
            Check.AssertNotNull(srcEquip, "设备台账不能为空".L10N());
            var srcState = srcEquip.QualityState;
            if (srcState == QualityState.Scrap)
            {
                return false;
            }

            return true;
        }

        #region 设备台账

        /// <summary>
        /// 通过设备台账Id列表获取点检项目列表
        /// </summary>
        /// <param name="accountIds">设备台账Id列表</param>
        /// <returns>点检项目列表</returns>
        public virtual EntityList<EquipAccountCheckProject> GetCheckProjectsOfAccounts(List<double> accountIds)
        {
            return accountIds.SplitContains(tempIds =>
             {
                 return Query<EquipAccountCheckProject>()
                     .Where(p => tempIds.Contains(p.EquipAccountId))
                     .ToList(null, new EagerLoadOptions().LoadWithViewProperty());
             });
        }

        /// <summary>
        /// 通过设备台账Id列表获取点检项目列表
        /// </summary>
        /// <param name="accountIds">设备台账Id列表</param>
        /// <param name="type">点检计划类型</param>
        /// <returns>点检项目列表</returns>
        public virtual EntityList<EquipAccountCheckProject> GetCheckProjectsOfAccounts(List<double> accountIds, CheckPlanType type)
        {
            var checkProjects = new EntityList<EquipAccountCheckProject>();
            accountIds.SplitDataExecute(tempIds =>
            {
                var query = Query<EquipAccountCheckProject>().Where(p => tempIds.Contains(p.EquipAccountId));
                if (type == CheckPlanType.Day)
                {
                    query.Where(p => p.CycleType == CycleType.Day);
                }
                else //频次获取班类型
                {
                    query.Where(p => p.CycleType == CycleType.Class);
                }
                checkProjects.AddRange(query.ToList());
            });
            return checkProjects;
        }

        /// <summary>
        /// 通过设备台账Id列表获取保养项目列表(贪婪加载点检保养项目)
        /// </summary>
        /// <param name="accountIds">设备台账Id列表</param>
        /// <returns>保养项目列表</returns>
        public virtual EntityList<EquipAccountMaintainProject> GetMaintainProjectsOfAccounts(List<double> accountIds)
        {
            return accountIds.SplitContains(tempIds =>
            {
                return Query<EquipAccountMaintainProject>().Where(p => tempIds.Contains(p.EquipAccountId))
                    .ToList(null, new EagerLoadOptions().LoadWithViewProperty().LoadWith(EquipAccountMaintainProject.ProjectDetailProperty));
            });
        }

        /// <summary>
        /// 获取润滑项目列表
        /// </summary>
        /// <returns></returns>
        public virtual EntityList<EquipAccountLubricationProject> GetLubricationProjectOfAccounts(List<double> accountIds)
        {
            return accountIds.SplitContains(tempIds =>
            {
                return Query<EquipAccountLubricationProject>()
                    .Where(p => tempIds.Contains(p.EquipAccountId))
                    .ToList(null, new EagerLoadOptions().LoadWithViewProperty().LoadWith(EquipAccountLubricationProject.ProjectDetailProperty));
            });
        }

        /// <summary>
        /// 根据设备台账Id列表更新点检保养项目、单元组成和单元组成物料清单
        /// </summary>
        /// <param name="accountIds">设备台账Id列表</param>
        public virtual void SynModelDatas(List<double> accountIds)
        {
            EntityList<EquipAccountCheckProject> checkProjectList = new EntityList<EquipAccountCheckProject>();

            EntityList<EquipAccountMaintainProject> maintainProjectList = new EntityList<EquipAccountMaintainProject>();

            EntityList<EquipAccountLubricationProject> lubricationProjectList = new EntityList<EquipAccountLubricationProject>();

            var equipCt = RT.Service.Resolve<EquipController>();

            #region 加载初始数据
            //获取设备台账列表
            var equipAccounts = equipCt.GetEquipAccountsByIds(accountIds);
            var modelIds = equipAccounts.Select(p => p.EquipModelId).Distinct().ToList();

            //获取设备型号对应的点检保养项目列表
            var checkProjectsOfModels = equipCt.GetCheckProjectsOfModels(modelIds);
            var dicCheckPrjsOfModels = checkProjectsOfModels.GroupBy(p => p.EquipModelId)
                .ToDictionary(p => p.Key, p => p.ToList());

            var maintainProjectsOfModels = equipCt.GetMaintainProjectsOfModels(modelIds);
            var dicMaintainPrjsOfModels = maintainProjectsOfModels.GroupBy(p => p.EquipModelId)
                .ToDictionary(p => p.Key, p => p.ToList());

            var lubricationProjectOfModels = equipCt.GetEquipModelLubricationProjects(modelIds);
            var dicLubricationPrjsOfModels = lubricationProjectOfModels.GroupBy(p => p.EquipModelId)
                .ToDictionary(p => p.Key, p => p.ToList());

            //获取设备台账对应的点检保养项目列表           
            var checkProjectsOfAccounts = equipCt.GetCheckProjectsOfAccounts(accountIds);
            var maintainProjectsOfAccounts = equipCt.GetMaintainProjectsOfAccounts(accountIds);
            var lubricationsOfAccounts = equipCt.GetLubricationProjectOfAccounts(accountIds);
            //获取设备台账对应的单元组成和单元组成物料列表                
            #endregion

            foreach (var equipAccount in equipAccounts)
            {
                //删除原有的点检项目                   
                checkProjectsOfAccounts.ForEach(project =>
                {
                    project.PersistenceStatus = PersistenceStatus.Deleted;
                });
                checkProjectList.AddRange(checkProjectsOfAccounts);
                // 更新设备型号的点检项目
                checkProjectList.AddRange(CreateCheckProjectList(dicCheckPrjsOfModels, equipAccount));

                //删除原有的保养项目
                maintainProjectsOfAccounts.ForEach(project =>
                {
                    project.PersistenceStatus = PersistenceStatus.Deleted;
                });
                maintainProjectList.AddRange(maintainProjectsOfAccounts);
                //更新设备型号的保养项目
                maintainProjectList.AddRange(CreateMaintainProjectList(dicMaintainPrjsOfModels, equipAccount));

                //删除原有的润滑项目
                lubricationsOfAccounts.ForEach(project =>
                {
                    project.PersistenceStatus = PersistenceStatus.Deleted;
                });
                lubricationProjectList.AddRange(lubricationsOfAccounts);
                //更新设备型号的润滑项目
                lubricationProjectList.AddRange(CreateLubricationProjectList(dicLubricationPrjsOfModels, equipAccount));

            }

            SaveEquipAccountRelateInfos(checkProjectList, maintainProjectList, lubricationProjectList);
        }

        /// <summary>
        /// 根据设备台账Id列表更新点检保养项目、单元组成和单元组成物料清单(实现接口)
        /// </summary>
        /// <param name="accountIds"></param>
        public virtual void SynModelData(List<double> accountIds)
        {
            SynModelDatas(accountIds);
        }

        /// <summary>
        /// 保存同步设备与人员权限
        /// </summary>
        /// <param name="accountIds"></param>
        public virtual void SynDevicePur(List<double> accountIds)
        {
            // 获取当前登陆人权限
            var devicePurIds = Query<DevicePur>().As("dp").LeftJoin<UserInUserGroup>("uig", (dp, uig) => dp.UserGroupId == uig.UserGroupId)
                .Where<UserInUserGroup>((dp, uig) => dp.UserId == RT.Identity.UserId || uig.UserId == RT.Identity.UserId)
                .Select(dp => new 
                {
                    dp.Id
                }).ToList<double>().ToList();
            if (!devicePurIds.Any())
            {
                return;
            }

            // 当前登录人已有的设备清单
            var deviceBillAccountIds = Query<DeviceBill>().Where(p => devicePurIds.Contains(p.DevicePurId)).Select(p => new { p.EquipAccountId }).ToList<double>().ToList();
            EntityList<DeviceBill> deviceBills = new EntityList<DeviceBill>();
            foreach (var devId in devicePurIds)
            {
                foreach (var accountId in accountIds)
                {
                    if (deviceBillAccountIds.Any(p => p == accountId)) // 已存在则不同步
                    {
                        continue;
                    }
                    DeviceBill bill = new DeviceBill
                    {
                        DevicePurId = devId,
                        EquipAccountId = accountId,
                    };
                    deviceBills.Add(bill);
                }

            }
            RF.BatchInsert(deviceBills);
        }

        /// <summary>
        /// 保存设备台账点检/保养/单元组成/单元物料信息
        /// </summary>
        /// <param name="checkProjectList">设备台账点检项目列表</param>
        /// <param name="maintainProjectList">设备台账保养项目列表</param>                
        /// <param name="lubricationProjectsList">设备台账润滑项目</param>
        public virtual void SaveEquipAccountRelateInfos(EntityList<EquipAccountCheckProject> checkProjectList,
            EntityList<EquipAccountMaintainProject> maintainProjectList,
            EntityList<EquipAccountLubricationProject> lubricationProjectsList)
        {
            using (var tran = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                RF.Save(checkProjectList);
                RF.Save(maintainProjectList);
                RF.Save(lubricationProjectsList);
                tran.Complete();
            }
        }

        /// <summary>
        /// 创建全新设备台账保养项目
        /// </summary>
        /// <param name="dicMaintainPrjsOfModels">设备型号保养项目字典</param>
        /// <param name="equipAccount">设备台账</param>
        /// <returns>新的设备台账保养项目列表</returns>
        public virtual EntityList<EquipAccountMaintainProject> CreateMaintainProjectList(Dictionary<double, List<EquipModelMaintainProject>> dicMaintainPrjsOfModels, EquipAccount equipAccount)
        {
            if (dicMaintainPrjsOfModels == null || equipAccount == null)
            {
                return new EntityList<EquipAccountMaintainProject>();
            }
            EntityList<EquipAccountMaintainProject> maintainProjectList = new EntityList<EquipAccountMaintainProject>();
            List<EquipModelMaintainProject> maintainPrjsOfModel = null;
            if (dicMaintainPrjsOfModels.TryGetValue(equipAccount.EquipModelId, out maintainPrjsOfModel))
            {
                foreach (var maintainPrjOfModel in maintainPrjsOfModel)
                {
                    var maintainProject = CreateEquipAccountMaintainProject(equipAccount, maintainPrjOfModel);
                    maintainProjectList.Add(maintainProject);
                }
            }

            return maintainProjectList;
        }

        /// <summary>
        /// 更新设备台账保养项目(原已存在设备台账保养项目)
        /// </summary>
        /// <param name="dicMaintainPrjsOfModels">设备型号保养项目字典</param>
        /// <param name="equipAccount">设备台账</param>
        /// <param name="maintainPrjsOfAccount">原有设备台账保养项目列表</param>
        /// <returns>新的设备台账保养项目列表</returns>
        public virtual EntityList<EquipAccountMaintainProject> UpdateMaintainProjectList(Dictionary<double, List<EquipModelMaintainProject>> dicMaintainPrjsOfModels, EquipAccount equipAccount, List<EquipAccountMaintainProject> maintainPrjsOfAccount)
        {
            if (dicMaintainPrjsOfModels == null || equipAccount == null)
            {
                return new EntityList<EquipAccountMaintainProject>();
            }
            EntityList<EquipAccountMaintainProject> maintainProjectList = new EntityList<EquipAccountMaintainProject>();
            List<EquipModelMaintainProject> maintainPrjsOfModel = null;
            if (dicMaintainPrjsOfModels.TryGetValue(equipAccount.EquipModelId, out maintainPrjsOfModel))
            {
                var dicMaintainPrjsOfAccount = maintainPrjsOfAccount.ToDictionary(p => p.ProjectDetailId);
                foreach (var maintainPrjOfModel in maintainPrjsOfModel)
                {
                    if (!dicMaintainPrjsOfAccount.ContainsKey(maintainPrjOfModel.ProjectDetailId))
                    {
                        var maintainProject = CreateEquipAccountMaintainProject(equipAccount, maintainPrjOfModel);
                        maintainProjectList.Add(maintainProject);
                    }
                }
            }

            return maintainProjectList;
        }

        /// <summary>
        /// 创建设备台账保养项目
        /// </summary>
        /// <param name="equipAccount">设备台账</param>
        /// <param name="maintainPrjOfModel">设备型号保养项目</param>
        /// <returns>设备台账保养项目</returns>
        public virtual EquipAccountMaintainProject CreateEquipAccountMaintainProject(EquipAccount equipAccount, EquipModelMaintainProject maintainPrjOfModel)
        {
            if (equipAccount == null || maintainPrjOfModel == null)
            {
                return new EquipAccountMaintainProject();
            }
            return new EquipAccountMaintainProject()
            {
                EquipAccountId = equipAccount.Id,
                ProjectDetailId = maintainPrjOfModel.ProjectDetailId,
                DepartmentId = maintainPrjOfModel.DepartmentId,
                PersistenceStatus = PersistenceStatus.New,
                ProjectName = maintainPrjOfModel.ProjectName,
                ProjectType = maintainPrjOfModel.ProjectType,
                CycleType = maintainPrjOfModel.CycleType,
                Part = maintainPrjOfModel.Part,
                Consumable = maintainPrjOfModel.Consumable,
                Method = maintainPrjOfModel.Method,
                Standard = maintainPrjOfModel.Standard,
                MinValue = maintainPrjOfModel.MinValue,
                MaxValue = maintainPrjOfModel.MaxValue,
                Unit = maintainPrjOfModel.Unit,
                UseTime = maintainPrjOfModel.UseTime,
                DepartmentNameView = maintainPrjOfModel.DepartmentName,
            };
        }

        /// <summary>
        /// 创建全新设备台账点检项目
        /// </summary>
        /// <param name="dicCheckPrjsOfModels">设备型号点检项目字典</param>
        /// <param name="equipAccount">设备台账</param>
        /// <returns>新的设备台账点检项目列表</returns>
        public virtual EntityList<EquipAccountCheckProject> CreateCheckProjectList(Dictionary<double, List<EquipModelCheckProject>> dicCheckPrjsOfModels, EquipAccount equipAccount)
        {
            if (dicCheckPrjsOfModels == null || equipAccount == null)
            {
                return new EntityList<EquipAccountCheckProject>();
            }
            EntityList<EquipAccountCheckProject> checkProjectList = new EntityList<EquipAccountCheckProject>();
            List<EquipModelCheckProject> checkPrjsOfModel = null;
            if (dicCheckPrjsOfModels.TryGetValue(equipAccount.EquipModelId, out checkPrjsOfModel))
            {
                foreach (var checkPrjOfModel in checkPrjsOfModel)
                {
                    var checkProject = CreateEquipAccountCheckProject(equipAccount, checkPrjOfModel);
                    checkProjectList.Add(checkProject);
                }
            }

            return checkProjectList;
        }

        /// <summary>
        /// 更新设备台账点检项目(原已存在设备台账点检项目)
        /// </summary>
        /// <param name="dicCheckPrjsOfModels">设备型号点检项目字典</param>
        /// <param name="equipAccount">设备台账</param>
        /// <param name="checkPrjsOfAccount">原有设备台账点检项目列表</param>
        /// <returns>新的设备台账点检项目列表</returns>
        public virtual EntityList<EquipAccountCheckProject> UpdateCheckProjectList(Dictionary<double, List<EquipModelCheckProject>> dicCheckPrjsOfModels, EquipAccount equipAccount, List<EquipAccountCheckProject> checkPrjsOfAccount)
        {
            if (dicCheckPrjsOfModels == null || checkPrjsOfAccount == null)
            {
                return new EntityList<EquipAccountCheckProject>();
            }
            EntityList<EquipAccountCheckProject> checkProjectList = new EntityList<EquipAccountCheckProject>();
            List<EquipModelCheckProject> checkPrjsOfModel = null;
            if (dicCheckPrjsOfModels.TryGetValue(equipAccount.EquipModelId, out checkPrjsOfModel))
            {
                var dicCheckPrjsOfAccount = checkPrjsOfAccount.ToDictionary(p => p.ProjectDetailId);
                foreach (var checkPrjOfModel in checkPrjsOfModel)
                {
                    if (!dicCheckPrjsOfAccount.ContainsKey(checkPrjOfModel.ProjectDetailId))
                    {
                        var checkProject = CreateEquipAccountCheckProject(equipAccount, checkPrjOfModel);
                        checkProjectList.Add(checkProject);
                    }
                }
            }

            return checkProjectList;
        }

        /// <summary>
        /// 创建设备台账点检项目
        /// </summary>
        /// <param name="equipAccount">设备台账</param>
        /// <param name="checkPrjOfModel">设备型号点检项目</param>
        /// <returns>设备台账点检项目</returns>
        public virtual EquipAccountCheckProject CreateEquipAccountCheckProject(EquipAccount equipAccount, EquipModelCheckProject checkPrjOfModel)
        {
            if (equipAccount == null || checkPrjOfModel == null)
            {
                return new EquipAccountCheckProject();
            }
            return new EquipAccountCheckProject()
            {
                EquipAccountId = equipAccount.Id,
                ProjectDetailId = checkPrjOfModel.ProjectDetailId,
                DepartmentId = checkPrjOfModel.DepartmentId,
                PersistenceStatus = PersistenceStatus.New,
                ProjectName = checkPrjOfModel.ProjectName,
                ProjectType = checkPrjOfModel.ProjectType,
                CycleType = checkPrjOfModel.CycleType,
                Part = checkPrjOfModel.Part,
                Consumable = checkPrjOfModel.Consumable,
                Method = checkPrjOfModel.Method,
                Standard = checkPrjOfModel.Standard,
                MinValue = checkPrjOfModel.MinValue,
                MaxValue = checkPrjOfModel.MaxValue,
                Unit = checkPrjOfModel.Unit,
                UseTime = checkPrjOfModel.UseTime,
                DepartmentNameView = checkPrjOfModel.DepartmentName,

            };
        }

        /// <summary>
        /// 更新设备台账润滑项目
        /// </summary>
        /// <param name="dicLubricationProjectOfModels">设备型号润滑项目字典</param>
        /// <param name="equipAccount">设备台账</param>
        /// <param name="lubricationProjectsOfAccount">设备台账已存在的润滑项目</param>
        /// <returns></returns>
        public virtual EntityList<EquipAccountLubricationProject> UpdateLubricationProjectList(Dictionary<double, List<EquipModelLubricationProject>> dicLubricationProjectOfModels,
            EquipAccount equipAccount, List<EquipAccountLubricationProject> lubricationProjectsOfAccount)
        {
            if (dicLubricationProjectOfModels == null || equipAccount == null)
            {
                return new EntityList<EquipAccountLubricationProject>();
            }
            EntityList<EquipAccountLubricationProject> lubricationProjectList = new EntityList<EquipAccountLubricationProject>();
            List<EquipModelLubricationProject> lubricationPrjsOfModel = null;
            if (dicLubricationProjectOfModels.TryGetValue(equipAccount.EquipModelId, out lubricationPrjsOfModel))
            {
                var dicLubricationPrjsOfAccount = lubricationProjectsOfAccount.ToDictionary(p => p.ProjectDetailId);
                EquipAccountLubricaSparePart sp = null;
                foreach (var lubricationPrjOfModel in lubricationPrjsOfModel)
                {
                    if (!dicLubricationPrjsOfAccount.ContainsKey(lubricationPrjOfModel.ProjectDetailId))
                    {
                        var lubricationProject = CreateEquipAccountLubricationProject(equipAccount, lubricationPrjOfModel);
                        foreach (var item in lubricationPrjOfModel.LubricaSparePartList)
                        {
                            sp = new EquipAccountLubricaSparePart();
                            sp.Qty = item.Qty;
                            sp.LubricationProjectId = lubricationProject.Id;
                            sp.SparePart = item.SparePart;
                            lubricationProject.LubricaSparePartList.Add(sp);
                        }
                        lubricationProjectList.Add(lubricationProject);
                    }
                }
            }

            return lubricationProjectList;
        }

        /// <summary>
        ///创建润滑项目
        /// </summary>
        /// <param name="equipAccount"></param>
        /// <param name="lubricationPrjOfModel"></param>
        /// <returns></returns>
        public virtual EquipAccountLubricationProject CreateEquipAccountLubricationProject(EquipAccount equipAccount, EquipModelLubricationProject lubricationPrjOfModel)
        {
            if (lubricationPrjOfModel == null || equipAccount == null)
            {
                return new EquipAccountLubricationProject();
            }
            return new EquipAccountLubricationProject()
            {
                EquipAccountId = equipAccount.Id,
                ProjectDetailId = lubricationPrjOfModel.ProjectDetailId,
                DepartmentId = lubricationPrjOfModel.DepartmentId,
                PersistenceStatus = PersistenceStatus.New,
                ProjectName = lubricationPrjOfModel.ProjectName,
                ProjectType = lubricationPrjOfModel.ProjectType,
                CycleType = lubricationPrjOfModel.CycleType,
                Part = lubricationPrjOfModel.Part,
                Consumable = lubricationPrjOfModel.Consumable,
                Method = lubricationPrjOfModel.Method,
                Standard = lubricationPrjOfModel.Standard,
                MinValue = lubricationPrjOfModel.MinValue,
                MaxValue = lubricationPrjOfModel.MaxValue,
                Unit = lubricationPrjOfModel.Unit,
                UseTime = lubricationPrjOfModel.UseTime,
                DepartmentNameView = lubricationPrjOfModel.Department?.Name,
                ProjectCycle = lubricationPrjOfModel.ProjectCycle,
                WarningPeriod = lubricationPrjOfModel.WarningPeriod,
                LubricatingType = lubricationPrjOfModel.LubricatingType,
            };
        }

        /// <summary>
        /// 创建设备台账润滑项目列表
        /// </summary>
        /// <param name="dicLubricationPrjsOfModels">设备型号润滑项目字典</param>
        /// <param name="equipAccount">设备台账</param>
        /// <returns></returns>
        public virtual EntityList<EquipAccountLubricationProject> CreateLubricationProjectList(Dictionary<double, List<EquipModelLubricationProject>> dicLubricationPrjsOfModels, EquipAccount equipAccount)
        {
            if (dicLubricationPrjsOfModels == null || equipAccount == null)
            {
                return new EntityList<EquipAccountLubricationProject>();
            }
            EntityList<EquipAccountLubricationProject> lubricationProjectList = new EntityList<EquipAccountLubricationProject>();
            List<EquipModelLubricationProject> lubricationProject = null;
            if (dicLubricationPrjsOfModels.TryGetValue(equipAccount.EquipModelId, out lubricationProject))
            {
                EquipAccountLubricaSparePart sp = null;
                foreach (var prjOfModel in lubricationProject)
                {
                    var lubricationPjt = CreateEquipAccountLubricationProject(equipAccount, prjOfModel);
                    foreach (var item in prjOfModel.LubricaSparePartList)
                    {
                        sp = new EquipAccountLubricaSparePart();
                        sp.Qty = item.Qty;
                        sp.LubricationProjectId = lubricationPjt.Id;
                        sp.SparePart = item.SparePart;
                        lubricationPjt.LubricaSparePartList.Add(sp);
                    }
                    lubricationProjectList.Add(lubricationPjt);
                }
            }
            return lubricationProjectList;
        }

        /// <summary>
        /// 根据设备台账的设备型号获取其相关信息(包括位置列表)
        /// </summary>
        /// <param name="account">设备台账</param>
        /// <returns>设备台账信息</returns>
        public virtual EquipAccountInfo GetEquipModelRelateInfos(EquipAccountBase account)
        {
            EquipAccountInfo accountInfo = new EquipAccountInfo();
            var equipCt = RT.Service.Resolve<EquipController>();
            DateTime now = RF.Find<EquipAccount>().GetDbTime();

            #region 加载初始数据
            //获取设备型号对应的点检保养项目列表
            var modelIds = new List<double>() { account.EquipModelId };
            var model = RT.Service.Resolve<EquipModelController>()
                .GetEquipModelsOfModel(account.EquipModelId);
            var checkProjectsOfModels = equipCt.GetCheckProjectsOfModels(modelIds);
            var maintainProjectsOfModels = equipCt.GetMaintainProjectsOfModels(modelIds);

            var locationsOfModels = RT.Service.Resolve<EquipModelController>()
                .GetLocationsOfModels(modelIds);

            var locations = new List<EquipAccountLocation>();
            locationsOfModels.ForEach(c =>
            {
                var location = CreateLocation(account, c);
                location.CreateDate = now;
                location.UpdateDate = now;
                locations.Add(location);
            });
            #endregion

            accountInfo.IndustryCategory = ((int)account.EquipModel.IndustryCategory).ToString();
            accountInfo.RailType = (int?)model.RailType;
            accountInfo.VirtualDevice = model.VirtualDevice != null ? (int)model.VirtualDevice : 0;
            accountInfo.FeederBinding = model.FeederBinding == null ? 0 : (int)model.FeederBinding;
            accountInfo.FeederLocFailSafe = model.FeederLocFailSafe == null ? 0 : (int)model.FeederLocFailSafe;
            accountInfo.FeederBarcodeFailSafe = model.FeederBarcodeFailSafe == null ? 0 : (int)model.FeederBarcodeFailSafe;
            accountInfo.IsDisabled = model.IsDisabled == null ? 0 : (int)model.IsDisabled;
            accountInfo.AgingType = model.AgingType == null ? 0 : (int?)model.AgingType;
            accountInfo.ProductionType = model.ProductionType == null ? 0 : (int?)model.ProductionType;

            accountInfo.CheckPrjList.AddRange(GetCheckProjectList(account, now, checkProjectsOfModels));
            accountInfo.MaintainPrjList.AddRange(GetMaintainProjectList(account, now, maintainProjectsOfModels));
            //添加新设备台账时不需要同步带出润滑项目 2022427
            accountInfo.LocationList.AddRange(locations);
            accountInfo.PhysicalUnionList = new EntityList<EquipAccountPhysicalUnion>();

            accountInfo.IsCalibration = IsCalibration(model);

            return accountInfo;
        }

        /// <summary>
        /// 查询是否是计量台账
        /// </summary>
        /// <param name="account">设备台账</param>
        /// <returns>设备台账信息</returns>
        public virtual bool GetIsCalibration(EquipAccountBase account)
        {
            var model = RT.Service.Resolve<EquipModelController>().GetEquipModelsOfModel(account.EquipModelId);

            if (model == null)
            {
                return false;
            }
            return IsCalibration(model);
        }

        /// <summary>
        /// 根据设备型号判断是否是计量设备(型号配置项中有配置)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private bool IsCalibration(EquipModel model)
        {
            List<Catalog> CatalogList = RT.Service.Resolve<CatalogController>().GetCatalogList(EquipType.EquipTypeCatalogType).ToList();

            //获取设备台账配置的计量设备查询信息
            var config = ConfigService.GetConfig(new EquipModelEquipmentCategoryConfig(), typeof(EquipModel));
            List<string> strarr = new List<string>();
            if (config != null && config.EquipmentMeteringIds.IsNotEmpty())
            {
                strarr = config.EquipmentMeteringIds.Split(',').ToList();
            }
            List<string> arraylist = CatalogList.Where(p => strarr.Contains(p.Id.ToString())).Select(p => p.Code).ToList();
            if (model.EquipTypeId == null)
            {
                return false;
            }
            if (arraylist.Contains(model.TypeCategory))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 根据设备台账的设备型号获取其相关信息(包括位置列表)
        /// </summary>
        /// <param name="lubricationProject">润滑项目</param>
        /// <returns>设备台账信息</returns>
        public virtual EquipAccountLubricationProjectInfo GetAccountSparePartItemInfos(EquipAccountLubricationProject lubricationProject)
        {
            EquipAccountLubricationProjectInfo LubricationProjectInfo = new EquipAccountLubricationProjectInfo();
            var equipCt = RT.Service.Resolve<ProjectDetailController>();
            DateTime now = RF.Find<EquipAccount>().GetDbTime();
            List<double> projectDetailIds = new List<double>() { lubricationProject.ProjectDetailId };
            #region 加载初始数据
            //获取设备型号对应的点检保养项目列表

            var sparePartItemModels = equipCt.GetSparePartItem(projectDetailIds);

            #endregion

            LubricationProjectInfo.EquipAccountLubricaSparePartList.AddRange(GetAccountSparePartItemList(lubricationProject.Id, now, sparePartItemModels));

            return LubricationProjectInfo;
        }


        /// <summary>
        /// 通过设备台账ID获取点检项目(包括位置列表)
        /// </summary>
        /// <param name="accountId">设备台账ID</param>
        /// <returns>设备台账信息</returns>
        public virtual EquipAccountInfo GetEquipCheckProjectInfos(double accountId)
        {
            EquipAccountInfo accountInfo = new EquipAccountInfo();
            var equipCt = RT.Service.Resolve<EquipController>();
            var accountIds = new List<double>() { accountId };
            var checkProjects = equipCt.GetCheckProjectsOfAccounts(accountIds);
            checkProjects.ForEach(p =>
            {
                p.GenerateId();
                p.PersistenceStatus = PersistenceStatus.New;
            });

            accountInfo.CheckPrjList.AddRange(checkProjects);

            return accountInfo;
        }

        /// <summary>
        /// 获取设备台账保养项目列表
        /// </summary>
        /// <param name="account">设备台账</param>
        /// <param name="now">当前时间</param>
        /// <param name="maintainProjectsOfModels">设备型号保养项目列表</param>
        /// <returns>设备台账保养项目列表</returns>
        public virtual List<EquipAccountMaintainProject> GetMaintainProjectList(EquipAccountBase account, DateTime now,
            EntityList<EquipModelMaintainProject> maintainProjectsOfModels)
        {
            List<EquipAccountMaintainProject> maintainProjects = new List<EquipAccountMaintainProject>();
            maintainProjectsOfModels.ForEach(m =>
            {
                var maintainProject = CreateMaintainProject(account, m, now);
                maintainProjects.Add(maintainProject);
            });

            return maintainProjects;
        }

        /// <summary>
        /// 获取设备台账点检保养项目列表
        /// </summary>
        /// <param name="account">设备台账</param>
        /// <param name="now">当前时间</param>
        /// <param name="checkProjectsOfModels">设备型号点检项目列表</param>
        /// <returns>设备台账点检保养项目列表</returns>
        public virtual List<EquipAccountCheckProject> GetCheckProjectList(EquipAccountBase account, DateTime now,
            EntityList<EquipModelCheckProject> checkProjectsOfModels)
        {
            List<EquipAccountCheckProject> checkProjects = new List<EquipAccountCheckProject>();
            checkProjectsOfModels.ForEach(c =>
            {
                var checkProject = CreateCheckProject(account, c, now);
                checkProjects.Add(checkProject);
            });

            return checkProjects;
        }

        /// <summary>
        /// 创建设备台账保养项目
        /// </summary>
        /// <param name="account">设备台账</param>
        /// <param name="m">设备型号保养项目</param>
        /// <param name="now">当前时间</param>
        /// <returns>设备台账保养项目</returns>
        public virtual EquipAccountMaintainProject CreateMaintainProject(EquipAccountBase account, EquipModelMaintainProject m, DateTime now)
        {
            return new EquipAccountMaintainProject()
            {
                EquipAccountId = account.Id,
                ProjectDetailId = m.ProjectDetailId,
                ProjectName = m.ProjectName,
                ProjectType = m.ProjectType,
                CycleType = m.CycleType,
                Part = m.Part,
                Consumable = m.Consumable,
                Method = m.Method,
                Standard = m.Standard,
                MinValue = m.MinValue,
                MaxValue = m.MaxValue,
                Unit = m.Unit,
                UseTime = m.UseTime,
                UpdateDate = now,
                CreateDate = now,
                DepartmentId = m.DepartmentId,
                DepartmentNameView = m.Department?.Name,
                PersistenceStatus = PersistenceStatus.New
            };
        }

        /// <summary>
        /// 创建设备台账点检项目
        /// </summary>
        /// <param name="account">设备台账</param>
        /// <param name="c">设备型号点检项目</param>
        /// <param name="now">当前时间</param>
        /// <returns>设备台账点检项目</returns>
        public virtual EquipAccountCheckProject CreateCheckProject(EquipAccountBase account, EquipModelCheckProject c, DateTime now)
        {
            return new EquipAccountCheckProject()
            {
                EquipAccountId = account.Id,
                ProjectDetailId = c.ProjectDetailId,
                ProjectName = c.ProjectName,
                ProjectType = c.ProjectType,
                //ProjectCategory = c.Category,
                CycleType = c.CycleType,
                Part = c.Part,
                Consumable = c.Consumable,
                Method = c.Method,
                Standard = c.Standard,
                MinValue = c.MinValue,
                MaxValue = c.MaxValue,
                Unit = c.Unit,
                UseTime = c.UseTime,
                UpdateDate = now,
                CreateDate = now,
                DepartmentId = c.DepartmentId,
                DepartmentNameView = c.Department?.Name,
                PersistenceStatus = PersistenceStatus.New
            };
        }

        /// <summary>
        /// 保存保养项目
        /// </summary>
        /// <param name="checkProjectInfos">保养项目信息</param>
        public virtual void SaveAccountMaintainCommand(List<Common.Entity.CheckProjectInfo> checkProjectInfos)
        {
            if (checkProjectInfos == null || checkProjectInfos.Count == 0)
            {
                throw new ValidationException("保养项目列表不能为空".L10N());
            }
            var savedData = new EntityList<EquipAccountMaintainProject>();
            var projectDetailIds = checkProjectInfos.Select(p => p.ProjectDetailId).Distinct().ToList();
            var details = RT.Service.Resolve<ProjectDetailController>().GetProjectDetails(projectDetailIds);
            if (details.Any(p => p.ProjectType != ProjectType.Maintain))
            {
                throw new ValidationException("保养项目不能添加点检项目类型！".L10N());
            }
            using (var tran = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                var equipAccountId = checkProjectInfos.FirstOrDefault().SourceId;
                //增加设备台账的行锁，防止并发和系统网络延迟时重复提交
                DB.Update<EquipAccount>().Set(p => p.Id, equipAccountId).Where(p => p.Id == equipAccountId).Execute();
                foreach (var item in checkProjectInfos)
                {
                    var detail = details.FirstOrDefault(m => m.Id == item.ProjectDetailId);

                    if (!detail.CycleType.HasValue)
                    {
                        throw new ValidationException("保养项目【{0}】的周期类型为空".L10nFormat(detail.Name));
                    }

                    var checkProject = new EquipAccountMaintainProject();
                    checkProject.EquipAccountId = item.SourceId;
                    checkProject.ProjectDetailId = item.ProjectDetailId;
                    checkProject.ProjectType = detail.ProjectType;
                    checkProject.Consumable = detail.Consumable;
                    checkProject.CycleType = detail.CycleType.Value;
                    checkProject.MaxValue = detail.MaxValue;
                    checkProject.Method = detail.Method;
                    checkProject.MinValue = detail.MinValue;
                    checkProject.Part = detail.Part;
                    checkProject.Standard = detail.Standard;
                    checkProject.Unit = detail.Unit;
                    checkProject.UseTime = detail.UseTime;

                    savedData.Add(checkProject);
                }
                RF.Save(savedData);
                tran.Complete();
            }
        }
        #endregion

        /// <summary>
        /// 获取使用中的设备
        /// </summary>
        /// <returns>设备集合</returns>
        public virtual EntityList<EquipAccount> GetEquipmentForUse()
        {
            return Query<EquipAccount>().Where(p => p.UseState == AccountUseState.Using).ToList();
        }

        /// <summary>
        /// 同步使用中的设备到资源
        /// </summary> 
        /// <returns>设备同步到资源的信息</returns>
        public virtual string SyncUseingEquipment()
        {
            StringBuilder sb = new StringBuilder();
            var itemList = GetEquipmentForUse();
            var srlist = RT.Service.Resolve<WipResourceController>().GetWipResourceBySrcType(new List<SyncSourceType>() { SyncSourceType.Equipment }, null, string.Empty);
            foreach (var item in itemList)
            {
                WipResource src = srlist.FirstOrDefault(p => p.Code == item.Code);
                try
                {
                    if (src == null)
                    {
                        src = new WipResource();
                        src.Code = item.Code;
                        src.Name = item.Name;
                        src.WorkShop = item.WorkShop;
                        src.FactoryId = item.FactoryId;
                        src.ResourceState = ResourceState.Actived;
                        src.SourceType = SyncSourceType.Equipment;
                        src.SourceId = item.Id;
                        src.Scheme = RT.Service.Resolve<CalendarSchemeController>().GetDefaultCalendar();
                        src.Qty = 1;
                        src.TaktTime = 1;
                    }
                    else
                    {
                        src.Code = item.Code;
                        src.Name = item.Name;
                        src.SourceId = item.Id;

                        if (src.WorkShopId != item.WorkShopId)
                        {
                            src.WorkShopId = item.WorkShopId;
                        }
                        if (src.FactoryId != item.FactoryId)
                        {
                            src.FactoryId = item.FactoryId;
                        }
                        if (src.ResourceState == ResourceState.Diseffect)
                        {
                            src.ResourceState = ResourceState.Actived;
                        }
                    }
                    if (src.PersistenceStatus != PersistenceStatus.Unchanged)
                    {
                        RF.Save(src);
                    }
                }
                catch (Exception exc)
                {
                    sb.AppendLine("同步设备台账{0}失败：{1}".L10nFormat(item.Code, exc.Message));
                }
            }

            srlist = RT.Service.Resolve<WipResourceController>().GetWipResourceBySrcType(new List<SyncSourceType>() { SyncSourceType.Equipment }, null, string.Empty);
            if (srlist.Count > 0)
            {
                var codes = srlist.Select(p => p.Code).Distinct().ToList();
                var itemListCodes = itemList.Select(p => p.Code).Distinct().ToList();

                codes = codes.Except(itemListCodes).ToList();
                //把已经不存在的设备台账变成失效状态
                DB.Update<WipResource>().Set(p => p.ResourceState, ResourceState.Diseffect).Where(p => codes.Contains(p.Code) && p.SourceType == SyncSourceType.Equipment).Execute();
            }

            return sb.ToString();
        }

        /// <summary>
        /// 获取降级父设备台账
        /// </summary>
        /// <param name="equipAccountId">选中的设备台账ID</param>
        /// <param name="keyword">下拉列表过滤条件</param>
        /// <param name="pagingInfo">分页</param>
        /// <returns>设备BOM明细列表</returns>
        public virtual EntityList<EquipAccount> GetDowngradeEquipAccounts(double equipAccountId, string keyword, PagingInfo pagingInfo)
        {
            //排除当天台账树树的子ID
            var ids = new List<double>();
            GetEquipAccountTreeUnderIds(equipAccountId, ids);

            var q = Query<EquipAccount>();
            q.Where(p => !ids.Contains(p.Id));
            if (keyword.IsNotEmpty())
            {
                q.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword));
            }
            return q.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 递归遍历设备台账树子ID
        /// </summary>
        /// <param name="equipAccountId"></param>
        /// <param name="ids"></param>
        /// <returns></returns>
        public virtual void GetEquipAccountTreeUnderIds(double equipAccountId, List<double> ids)
        {
            if (ids == null)
            {
                return;
            }
            ids.Add(equipAccountId);
            var list = Query<EquipAccount>().Where(p => p.TreePId == equipAccountId).ToList();
            foreach (var item in list)
            {
                GetEquipAccountTreeUnderIds(item.Id, ids);
            }
        }


        /// <summary>
        /// 获取设备台账点检项目
        /// </summary>
        /// <param name="equipmentId">设备台账ID</param>
        /// <param name="orderInfoList">排序信息</param>
        /// <param name="pagingInfo">关键字</param>
        /// <returns>备件更换记录列表</returns>
        public virtual EntityList<EquipAccountCheckProject> GetEquipAccountCheckProjects(double equipmentId, List<OrderInfo> orderInfoList, PagingInfo pagingInfo)
        {
            var query = Query<EquipAccountCheckProject>();
            if (equipmentId > 0)
            {
                query.Where(p => p.EquipAccountId == equipmentId);
            }
            return query.OrderBy(orderInfoList).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 通过设备台账Id获取点检项目列表
        /// </summary>
        /// <param name="equipAccountId">设备台账ID</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <param name="orderInfos">排序信息</param>
        /// <returns>仪器参数列表</returns>
        public virtual EntityList<EquipAccountCheckProject> GetEquipAccountCheckProjects(
            double equipAccountId, PagingInfo pagingInfo, IList<OrderInfo> orderInfos)
        {
            return Query<EquipAccountCheckProject>()
                .Where(p => p.EquipAccountId == equipAccountId)
                .OrderBy(orderInfos)
                .ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取设备台账物联参数
        /// </summary>
        /// <param name="equipmentId">设备台账ID</param>
        /// <param name="orderInfoList">排序信息</param>
        /// <param name="pagingInfo">关键字</param>
        /// <returns>备件更换记录列表</returns>
        public virtual EntityList<EquipAccountPhysicalUnion> GetEquipAccountPhysicalUnions(double equipmentId, List<OrderInfo> orderInfoList, PagingInfo pagingInfo)
        {
            var query = Query<EquipAccountPhysicalUnion>();
            if (equipmentId > 0)
            {
                query.Where(p => p.EquipAccountId == equipmentId);
            }
            return query.OrderBy(orderInfoList).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据设备台账Id列表更新点检保养项目、单元组成和单元组成物料清单（增加同步位置列表）
        /// </summary>
        /// <param name="accountIds">设备台账Id列表</param>
        public virtual void SynModelExtDatas(List<double> accountIds)
        {
            using (var tran = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                RT.Service.Resolve<EquipController>().SynModelDatas(accountIds);

                SynEquipAccountLocation(accountIds);
                tran.Complete();
            }
        }

        /// <summary>
        /// 同步位置列表信息
        /// </summary>
        /// <param name="accountIds">设备台帐Id列表</param>
        public virtual void SynEquipAccountLocation(List<double> accountIds)
        {
            var locationList = new EntityList<EquipAccountLocation>();
            //获取设备台账列表
            var equipAccounts = GetEquipAccountsByIds(accountIds);
            var modelIds = equipAccounts.Select(p => p.EquipModelId).Distinct().ToList();
            //获取设备型号对应的位置列表
            var locationsOfModels = RT.Service.Resolve<EquipModelController>().GetLocationsOfModels(modelIds);

            var dicLocationsOfModels = locationsOfModels
                .GroupBy(p => p.EquipModelId).ToDictionary(p => p.Key, p => p.ToList());

            //获取设备台账对应的位置列表          
            var locationsOfAccounts = GetLocationsOfAccounts(accountIds);
            var dicLocationsOfAccounts = locationsOfAccounts
                .GroupBy(p => p.EquipAccountId)
                .ToDictionary(p => p.Key, p => p.ToList());

            foreach (var equipAccount in equipAccounts)
            {
                //更新位置列表                   
                if (dicLocationsOfAccounts.TryGetValue(equipAccount.Id, out List<EquipAccountLocation> locationsOfAccount))
                {
                    locationList.AddRange(UpdateLocationList(dicLocationsOfModels, equipAccount, locationsOfAccount));
                }
                else
                {
                    locationList.AddRange(CreateLocationList(dicLocationsOfModels, equipAccount));
                }
            }

            RF.Save(locationList);
        }

        /// <summary>
        /// 通过设备台账Id列表获取位置列表
        /// </summary>
        /// <param name="accountIds">设备台账Id列表</param>
        /// <returns>位置列表</returns>
        public virtual EntityList<EquipAccountLocation> GetLocationsOfAccounts(List<double> accountIds)
        {
            return accountIds.SplitContains(tempIds =>
            {
                return Query<EquipAccountLocation>().Where(p => tempIds.Contains(p.EquipAccountId)).ToList();
            });
        }

        /// <summary>
        /// 更新设备台账位置列表(原已存在设备台账位置列表)
        /// </summary>
        /// <param name="dicLocationsOfModels">设备型号位置列表字典</param>
        /// <param name="equipAccount">设备台账</param>
        /// <param name="locationsOfAccount">原有设备台账位置列表列表</param>
        /// <returns>新的设备台账位置列表列表</returns>
        private EntityList<EquipAccountLocation> UpdateLocationList(Dictionary<double, List<EquipModelLocation>> dicLocationsOfModels, EquipAccount equipAccount, List<EquipAccountLocation> locationsOfAccount)
        {
            EntityList<EquipAccountLocation> locationList = new EntityList<EquipAccountLocation>();
            List<EquipModelLocation> locationsOfModel = null;
            if (dicLocationsOfModels.TryGetValue(equipAccount.EquipModelId, out locationsOfModel))
            {
                foreach (var locOfModel in locationsOfModel)
                {
                    if (locationsOfAccount.Any(p => p.Subarea == locOfModel.Subarea && p.BigStance == locOfModel.BigStance
                    && p.Stance == locOfModel.Stance && p.StanceType == locOfModel.StanceType))
                    {
                        continue;
                    }
                    var location = CreateLocation(equipAccount, locOfModel);
                    locationList.Add(location);
                }
            }
            return locationList;
        }

        /// <summary>
        /// 创建全新设备台账位置列表
        /// </summary>
        /// <param name="dicLocationsOfModels">设备型号位置列表字典</param>
        /// <param name="equipAccount">设备台账</param>
        /// <returns>新的设备台账位置列表</returns>
        private EntityList<EquipAccountLocation> CreateLocationList(Dictionary<double, List<EquipModelLocation>> dicLocationsOfModels, EquipAccount equipAccount)
        {
            EntityList<EquipAccountLocation> locationList = new EntityList<EquipAccountLocation>();
            List<EquipModelLocation> locationsOfModel = null;
            if (dicLocationsOfModels.TryGetValue(equipAccount.EquipModelId, out locationsOfModel))
            {
                foreach (var locationOfModel in locationsOfModel)
                {
                    var location = CreateLocation(equipAccount, locationOfModel);
                    locationList.Add(location);
                }
            }
            return locationList;
        }

        /// <summary>
        /// 创建设备台账位置列表
        /// </summary>
        /// <param name="equipAccount">设备台账</param>
        /// <param name="locOfModel">设备型号位置列表</param>
        /// <returns>设备台账位置列表</returns>
        private EquipAccountLocation CreateLocation(EquipAccountBase equipAccount, EquipModelLocation locOfModel)
        {
            var location = new EquipAccountLocation();
            location.Subarea = locOfModel.Subarea;
            location.BigStance = locOfModel.BigStance;
            location.Stance = locOfModel.Stance;
            location.StanceType = locOfModel.StanceType;
            location.EquipAccountId = equipAccount.Id;
            location.PersistenceStatus = PersistenceStatus.New;
            return location;
        }

        /// <summary>
        /// 获取设备台账ID
        /// </summary>
        /// <param name="equipCode"></param>
        /// <returns></returns>
        public virtual double GetEquipAccountId(string equipCode)
        {
            var q = Query<EquipAccount>();
            q.Where(p => p.Code == equipCode);
            q.Select(p => p.Id);

            var account = q.FirstOrDefault();
            if (account == null)
            {
                throw new ValidationException("设备[{0}]不存在。".L10nFormat(equipCode));
            }
            return account.Id;
        }

        /// <summary>
        /// 生成设备履历
        /// </summary>
        /// <param name="equipAccountId"></param>
        /// <param name="resumeType"></param>
        /// <param name="state"></param>
        /// <param name="sourceNo"></param>
        public virtual void GenerateEquipAccountResume(double equipAccountId, ResumeType resumeType, AccountState state, string sourceNo)
        {
            if (resumeType == ResumeType.CallRepair)//报修未成为既定事实 不生成报修履历
            {
                return;
            }
            var resume = new EquipAccountResume()
            {
                EquipAccountId = equipAccountId,
                ResumeType = resumeType,
                State = state,
                No = sourceNo
            };

            RF.Save(resume);
        }

        /// <summary>
        /// 通过设备台账Id获取保养项目列表(贪婪加载点检保养项目)
        /// </summary>
        /// <param name="equipAccountId">设备台账ID</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <param name="orderInfos">排序信息</param>
        /// <returns>保养项目列表</returns>
        public virtual EntityList<EquipAccountMaintainProject> GetEquipAccountMaintainProjects(
            double equipAccountId, PagingInfo pagingInfo, IList<OrderInfo> orderInfos)
        {
            return Query<EquipAccountMaintainProject>()
                .Where(p => p.EquipAccountId == equipAccountId)
                .OrderBy(orderInfos)
                .ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 通过设备台账获取润滑项目
        /// </summary>
        /// <param name="equipAccountId"></param>
        /// <param name="pagingInfo"></param>
        /// <param name="orderInfos"></param>
        /// <returns></returns>
        public virtual EntityList<EquipAccountLubricationProject> GetEquipAccountLubricationProject(
            double equipAccountId, PagingInfo pagingInfo, IList<OrderInfo> orderInfos)
        {
            return Query<EquipAccountLubricationProject>()
                .Where(p => p.EquipAccountId == equipAccountId)
                .OrderBy(orderInfos)
                .ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 通过设备台账Id获取仪器参数列表(贪婪加载点检保养项目)
        /// </summary>
        /// <param name="equipAccountId">设备台账ID</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <param name="orderInfos">排序信息</param>
        /// <returns>仪器参数列表</returns>
        public virtual EntityList<EquipParam> GetEquipParams(
            double equipAccountId, PagingInfo pagingInfo, IList<OrderInfo> orderInfos)
        {
            return Query<EquipParam>()
                .Where(p => p.EquipAccountId == equipAccountId)
                .OrderBy(orderInfos)
                .ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 是否存在RFID
        /// </summary>
        /// <param name="id"></param>
        /// <param name="RFID"></param>
        /// <returns></returns>
        public virtual bool IsExsitedRFID(double id, string RFID)
        {
            return Query<EquipAccount>().Where(m => m.RFID == RFID && m.Id != id).FirstOrDefault() != null;
        }

        /// <summary>
        /// 获取设备台账润滑记录备件清单
        /// </summary>
        /// <param name="lubricationProjectId"></param>
        /// <param name="now"></param>
        /// <param name="sparePartItemList"></param>
        /// <returns></returns>
        private List<EquipAccountLubricaSparePart> GetAccountSparePartItemList(double lubricationProjectId, DateTime now,
           EntityList<SparePartItem> sparePartItemList)
        {
            List<EquipAccountLubricaSparePart> lubricationProjects = new List<EquipAccountLubricaSparePart>();
            sparePartItemList.ForEach(m =>
            {
                var lubricationProject = CreateEquipAccountLubricaSparePart(lubricationProjectId, m, now);
                lubricationProjects.Add(lubricationProject);
            });

            return lubricationProjects;
        }


        /// <summary>
        /// 创建设备台账润滑项目的备件清单
        /// </summary>
        /// <param name="lubricationProjectId"></param>
        /// <param name="m"></param>
        /// <param name="now"></param>
        /// <returns></returns>
        private EquipAccountLubricaSparePart CreateEquipAccountLubricaSparePart(double lubricationProjectId,
            SparePartItem m, DateTime now)
        {
            return new EquipAccountLubricaSparePart()
            {
                LubricationProjectId = lubricationProjectId,
                SparePartId = m.SparePartId,
                SparePartCode = m.SparePart.SparePartCode,
                SparePartName = m.SparePart.SparePartName,
                Qty = m.Qty,
                UpdateDate = now,
                CreateDate = now,
                PersistenceStatus = PersistenceStatus.New,
            };
        }


        /// <summary>
        /// 根据设备台账获取技术参数
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="pagingInfo"></param>
        /// <param name="orderInfos"></param>
        /// <returns></returns>

        public virtual EntityList<EquipModelTechParameter> GetEquipModelTechParametersByEquipAccount(double accountId,
            PagingInfo pagingInfo, IList<OrderInfo> orderInfos)
        {
            var result = Query<EquipModelTechParameter>().Join<EquipAccount>((x, y) => x.EquipModelId == y.EquipModelId && y.Id == accountId)
                 .ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
            var res = new EntityList<EquipModelTechParameter>();
            res.SetTotalCount(result.Count);
            res.AddRange(result.OrderBy(f => SortExtension.GetIndex(f)));
            return res;
        }

        /// <summary>
        /// 保存编辑后的台账
        /// </summary>
        /// <param name="newEquipAccount"></param>
        /// <returns></returns>

        public virtual void SaveEditEquipAccount(EquipAccount newEquipAccount)
        {
            var oldEquipAccount = RF.GetById<EquipAccount>(newEquipAccount.Id, new EagerLoadOptions().LoadWithViewProperty());
            if (oldEquipAccount == null)
            {
                throw new ValidationException("无法找到原数据进行更新！".L10N());
            }

            if (oldEquipAccount.EquipModelId != newEquipAccount.EquipModelId)//设备型号变更则需要删除点检项目  保养项目 润滑项目 
            {
                using (var tran = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
                {
                    DB.Delete<EquipAccountCheckProject>().Where(m => m.EquipAccountId == newEquipAccount.Id).Execute();
                    DB.Delete<EquipAccountMaintainProject>().Where(m => m.EquipAccountId == newEquipAccount.Id).Execute();
                    DB.Delete<EquipAccountLubricationProject>().Where(m => m.EquipAccountId == newEquipAccount.Id).Execute();
                    RF.Save(newEquipAccount);
                    tran.Complete();
                }
            }
            else
            {
                RF.Save(newEquipAccount);
            }
        }

        /// <summary>
        /// 获取设备台账信息
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public virtual EntityList<EquipAccount> GetFilterFixCodeEquipAccountsByCriteria(EquipAccountCriteria criteria)
        {
            var query = Query<EquipAccount>();
            if (criteria.Code.IsNotEmpty())
            {
                query.Where(p => p.Code.Contains(criteria.Code));
            }
            if (criteria.Name.IsNotEmpty())
            {
                query.Where(p => p.Name.Contains(criteria.Name));
            }
            if (criteria.ModelCode.IsNotEmpty()
                || criteria.ModelName.IsNotEmpty()
                || criteria.TypeCategory.IsNotEmpty())
            {
                query.Exists<EquipModel>((x, y) => y.Join<EquipType>((c, d) => c.EquipTypeId == d.Id)
                .Where(p => p.Id == x.EquipModelId)
                .WhereIf(criteria.ModelCode.IsNotEmpty(), c => c.Code.Contains(criteria.ModelCode))
                .WhereIf(criteria.ModelName.IsNotEmpty(), c => c.Name.Contains(criteria.ModelName))
                .WhereIf(criteria.TypeCategory.IsNotEmpty(),
                    e => e.TypeCategory == criteria.TypeCategory));
            }

            if (criteria.State.HasValue)
            {
                query.Where(p => p.State == criteria.State);
            }
            if (criteria.AccountUseState.HasValue)
            {
                query.Where(p => p.UseState == criteria.AccountUseState);
            }
            if (criteria.WorkShopId.HasValue)
            {
                query.Where(p => p.WorkShopId == criteria.WorkShopId);
            }
            if (criteria.ResourceId.HasValue && criteria.ResourceId != 0)
            {
                query.Where(p => p.ResourceId == criteria.ResourceId);
            }
            if (criteria.ProcessId.HasValue)
            {
                query.Where(p => p.ProcessId == criteria.ProcessId.Value);
            }
            if (criteria.CreateDate.BeginValue.HasValue)
            {
                query.Where(p => p.CreateDate >= criteria.CreateDate.BeginValue);
            }
            if (criteria.CreateDate.EndValue.HasValue)
            {
                query.Where(p => p.CreateDate <= criteria.CreateDate.EndValue);
            }
            query.Where(p => p.AssetCode == "");

            return query.OrderBy(criteria.OrderInfoList).ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }


        /// <summary>
        /// 根据设备类型查询视图获取设备台账
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public virtual EntityList<EquipAccount> AccountByModleTypeCriteria(AccountByModleTypeCriteria criteria)
        {
            var deviceIOTPara = DB.Query<DeviceIOTPara>().Where(p => p.EquipModelId == criteria.EquipModelId).FirstOrDefault();
            var query = DB.Query<EquipAccount>().LeftJoin<FacilityDetail>((e, f) => e.Id == f.EquipAccountId);
            if (deviceIOTPara != null)
            {
                query.Where(p => p.EquipModelId == deviceIOTPara.EquipModelId);
            }
            query.OrderByDescending<FacilityDetail>((e, f) => f.Id);
            var result = query.ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty().LoadWith(EquipAccount.EquipModelProperty).LoadWith(EquipModel.EquipTypeProperty));

            return result;

        }

        /// <summary>
        /// 获取设备台账变更信息
        /// </summary>
        /// <param name="accountIds"></param>
        /// <returns></returns>
        public virtual List<EquipChangeInfo> GetEquipChangeInfos(List<double> accountIds)
        {
            List<EquipChangeInfo> equipChangeInfos = new List<EquipChangeInfo>();
            accountIds.SplitDataExecute(tempIds =>
            {
                var list = Query<EquipAccount>().As("ea")
                .LeftJoin<Enterprise>("ep", (ea, ep) => ea.UseDepartmentId == ep.Id)
                .LeftJoin<Employee>("ey", (ea, ey) => ea.UserId == ey.Id)
                .Where(ea => tempIds.Contains(ea.Id))
                .Select<Enterprise, Enterprise, Employee>((ea, ep, oep, ey) => new
                {
                    Id = ea.Id,
                    UseDepartmentId = ep.Id,
                    UseDepartmentName = ep.Name,
                    OldUseDepartmentId = oep.Id,
                    OldUseDepartmentName = oep.Name,
                    UserId = ey.Id,
                    UserName = ey.Name,
                    UseState = ea.UseState,
                    OldUseState = ea.OldUseState,
                }).ToList<EquipChangeInfo>().ToList();
                equipChangeInfos.AddRange(list);
            });
            return equipChangeInfos;
        }

        /// <summary>
        /// 获取设备投资基础信息
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual SIE.EMS.Equipments.ApiModels.EquipInfo GetEquipAccountInfo(string key)
        {
            if (key.IsNullOrEmpty())
            {
                return new SIE.EMS.Equipments.ApiModels.EquipInfo();
            }
            var states = new List<SIE.Core.Enums.AccountUseState> { SIE.Core.Enums.AccountUseState.Scrap, SIE.Core.Enums.AccountUseState.ToAccepted, SIE.Core.Enums.AccountUseState.DisposedOf };
            return Query<EquipAccount>()
                .LeftJoin<EquipModel>((ea, em) => ea.EquipModelId == em.Id)
                .Where(ea => (ea.Code.Contains(key) || ea.Name.Contains(key) || ea.RFID.Contains(key)) && !states.Contains(ea.UseState))
                .Select<EquipModel>((ea, em) => new
                {
                    Id = ea.Id,
                    Code = ea.Code,
                    Name = ea.Name,
                    EqiupModelId = em.Id,
                    EquipModelCode = em.Code,
                    EquipModelName = em.Name,
                }).FirstOrDefault<EquipInfo>();
        }
    }
}
using DocumentFormat.OpenXml.Wordprocessing;
using SIE.Common.Attachments;
using SIE.Common.Catalogs;
using SIE.Common.Configs;
using SIE.Common.NumberRules;
using SIE.Core.ApiModels;
using SIE.Core.Common;
using SIE.Core.Enums;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Equipments.Configs;
using SIE.Equipments.Enums;
using SIE.Equipments.EquipAccountLocations;
using SIE.Equipments.EquipModels;
using SIE.Equipments.EquipTypes;
using SIE.Equipments.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace SIE.Equipments.EquipAccounts
{
    /// <summary>
    /// 设备台账控制器
    /// </summary>
    public partial class EquipAccountController : DomainController
    {
        /// <summary>
        /// 根据设备台账ID获取缸槽列表
        /// </summary>
        /// <param name="equipAccountId">设备台账ID</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <param name="sortInfo">排序信息</param>
        /// <returns></returns>
        public virtual EntityList<EquipAccountSlot> GetPcbSlotList(double equipAccountId, PagingInfo pagingInfo = null, IList<OrderInfo> sortInfo = null)
        {
            var query = Query<EquipAccountSlot>().Where(p => p.EquipAccountId == equipAccountId);
            if (sortInfo != null)
                query.OrderBy(sortInfo);
            return query.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
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
                query.Where(exp);
            return query.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取设备台账列表
        /// Expression不支持序列号，前端不要调用
        /// </summary>
        /// <param name="pagingInfo">分页条件</param>
        /// <param name="keyword">关键字</param>
        /// <returns>设备台账列表</returns>
        public virtual EntityList<EquipAccount> GetEquipAccounts(PagingInfo pagingInfo, string keyword)
        {
            var query = Query<EquipAccount>().WhereIf(keyword.IsNotEmpty(), p => p.Code.Contains(keyword) || p.Name.Contains(keyword));
            return query.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取设备台账列表
        /// Expression不支持序列号，前端不要调用
        /// </summary>
        /// <param name="pagingInfo">分页条件</param>
        /// <param name="keyword">关键字</param>
        /// <returns>设备台账列表</returns>
        public virtual EntityList<EquipAccount> GetAllEquipAccounts(PagingInfo pagingInfo, string keyword)
        {
            using (SIE.DataAuth.DataAuths.LoadAll())
            {
                var query = Query<EquipAccount>().WhereIf(keyword.IsNotEmpty(), p => p.Code.Contains(keyword) || p.Name.Contains(keyword));
                var list = query.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
                foreach (var item in list)
                {
                    item.TreePId = null;
                }
                return list;
            }
        }

        /// <summary>
        /// 获取设备台账
        /// </summary>
        /// <param name="equipAccountId">设备台账ID</param>
        /// <returns></returns>
        public virtual EquipAccount GetEquipAccountById(double equipAccountId)
        {
            return RF.GetById<EquipAccount>(equipAccountId, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取设备台账基本信息
        /// </summary>
        /// <param name="equipAccountId">台账Id</param>
        /// <returns></returns>
        public virtual BaseDataInfo GetEquipAccountBaseInfo(double equipAccountId)
        {
            return Query<EquipAccount>().Where(p => p.Id == equipAccountId).Select(p => new
            {
                Id = p.Id,
                Code = p.Code,
                Name = p.Name,
            }).FirstOrDefault<BaseDataInfo>();
        }

        /// <summary>
        /// 获取设备台账基本信息(批量)
        /// </summary>
        /// <param name="equipAccountIds">台账Ids</param>
        /// <returns></returns>
        public virtual List<BaseDataInfo> GetEquipAccountBaseInfos(List<double> equipAccountIds)
        {
            var infos = new List<BaseDataInfo>();
            equipAccountIds.SplitDataExecute(tempIds =>
            {
                var list = Query<EquipAccount>().Where(p => tempIds.Contains(p.Id)).Select(p => new
                {
                    Id = p.Id,
                    Code = p.Code,
                    Name = p.Name,
                }).ToList<BaseDataInfo>();
                infos.AddRange(list);
            });
            return infos;
        }

        /// <summary>
        /// 获取设备台账
        /// </summary>
        /// <param name="equipAccountId">设备台账ID</param>
        /// <returns></returns>
        public virtual EquipAccount GetEquipAccountByIdNoDataAuth(double equipAccountId)
        {
            using (SIE.DataAuth.DataAuths.LoadAll())
            {
                return RF.GetById<EquipAccount>(equipAccountId, new EagerLoadOptions().LoadWithViewProperty());
            }
        }
     

        /// <summary>
        /// 根据Ids获取设备台账
        /// </summary>
        /// <param name="ids">设备台账Id集合</param>
        /// <returns></returns>
        public virtual EntityList<EquipAccount> GetEquipAccountsByIds(List<double> ids)
        {
            return ids.SplitContains(tempids =>
            {
                return Query<EquipAccount>().Where(m => tempids.Contains(m.Id)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
        }

        /// <summary>
        /// 根据Ids获取设备台账(提升性能只查询业务逻辑使用到的ID，Code)
        /// </summary>
        /// <param name="ids">设备台账Id集合</param>
        /// <returns></returns>
        public virtual List<EquipAccountData> GetEquipAccountsByIdsNoLoad(List<double> ids)
        {
            return Query<EquipAccount>().Select(p => new { p.Id, p.Code }).Where(w => ids.Contains(w.Id)).ToList<EquipAccountData>().ToList();
        }

        /// <summary>
        /// 根据编码获取设备台账
        /// </summary>
        /// <param name="codeList">设备台账编码集合</param>
        /// <returns></returns>
        public virtual EntityList<EquipAccount> GetEquipAccountsByCode(List<string> codeList)
        {
            return codeList.SplitContains(codes =>
            {
                return Query<EquipAccount>().Where(m => codes.Contains(m.Code)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
        }


        /// <summary>
        /// 根据编码获取设备台账
        /// </summary>
        /// <param name="code">设备台账编码</param>
        /// <returns></returns>
        public virtual EquipAccount GetEquipAccountsByCode(string code)
        {
            return Query<EquipAccount>().Where(w => w.Code == code)
                .FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
        }



        /// <summary>
        /// 根据编码获取设备台账-无权限控制
        /// </summary>
        /// <param name="code">设备台账编码</param>
        /// <returns></returns>
        public virtual EquipAccount GetEquipAccountsByCodeNoAuth(string code)
        {
            using (SIE.DataAuth.DataAuths.LoadAll())
            {
                return Query<EquipAccount>().Where(w => w.Code == code)
                .FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
            }
        }

        /// <summary>
        /// 获取设备 无权限限制
        /// </summary>
        /// <param name="codes"></param>
        /// <returns></returns>

        public virtual EntityList<EquipAccount> GetEquipAccountsNoLimit(List<string> codes)
        {
            using (SIE.DataAuth.DataAuths.LoadAll())
            {
               return GetEquipAccounts(codes);
            }
        }

        /// <summary>
        /// 根据编码获取设备台账集合
        /// </summary>
        /// <param name="codes">设备编码</param>
        /// <returns>设备台账集合</returns>
        public virtual EntityList<EquipAccount> GetEquipAccounts(List<string> codes)
        {
            var exp = codes.CreateContainsExpression<EquipAccount>("x", nameof(EquipAccount.Code));
            if (exp == null)
                return new EntityList<EquipAccount>();
            var query = Query<EquipAccount>().Where(exp);
            return query.ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取设备台账(按产线ID)
        /// </summary>
        /// <param name="resourceId">产线ID</param>
        /// <returns></returns>
        public virtual EntityList<EquipAccount> GetEquipAccountByResourceId(double resourceId)
        {
            return Query<EquipAccount>()
                .Where(x => x.ResourceId == resourceId)
                .ToList();
        }

        /// <summary>
        /// 查询设备型号
        /// </summary>
        /// <param name="value">设备型号编码</param>
        /// <returns></returns>
        public virtual EquipModel GetEquipModelByCode(string value)
        {
            return Query<EquipModel>()
                .Where(x => x.Code == value)
                .FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
        }

        #region 设备与工序关系
        /// <summary>
        /// 获取设备与工序关系
        /// </summary>
        /// <param name="equipId">设备ID</param>
        /// <param name="pagingInfo">分页条件</param>
        /// <param name="orderInfos">排序条件</param>
        /// <returns>设备与工序关系</returns>
        public virtual EntityList<EquipAccountProcess> GetEquipAccountProcesses(double equipId, PagingInfo pagingInfo, IList<OrderInfo> orderInfos)
        {
            return Query<EquipAccountProcess>().Where(p => p.EquipAccountId == equipId).OrderBy(orderInfos).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 保存设备与工序关系
        /// </summary>
        /// <param name="equipProcesss">设备与工序关系列表</param>  
        public virtual void GetEquipAccountProcesses(EntityList<EquipAccountProcess> equipProcesss)
        {
            RF.Save(equipProcesss);
        }

        /// <summary>
        /// 获取设备与工序关系
        /// </summary>
        /// <param name="equipId">设备ID</param> 
        /// <returns>设备与工序关系</returns>
        public virtual EntityList<EquipAccountProcess> GetEquipAccountProcesses(double equipId)
        {
            return Query<EquipAccountProcess>().Where(p => p.EquipAccountId == equipId).ToList();
        }
        #endregion

        /// <summary>
        /// 获取所有物流设备
        /// </summary>
        /// <returns>设备</returns>
        public virtual EntityList<EquipAccount> GetEquipAccountsForWcs()
        {
            return Query<EquipAccount>()
                .Where(p => p.EquipModel.IndustryCategory == Core.Enums.IndustryCategory.LogisticsEquipment)
                .ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 降级设备台账
        /// </summary>
        /// <param name="equipAccountId">设备台账ID</param>
        /// <param name="parentAccountId">目标父台账ID</param>
        public virtual void DowngradeAccountCommand(double equipAccountId, double parentAccountId)
        {
            if (equipAccountId == parentAccountId)
                throw new ValidationException("不能选择自己作为父台账".L10N());

            DB.Update<EquipAccount>().Set(p => p.TreePId, parentAccountId).Where(p => p.Id == equipAccountId).Execute();
        }

        /// <summary>
        /// 升级设备台账
        /// </summary>
        /// <param name="equipAccountId">设备台账ID</param>
        public virtual void UpgradeEquipAccount(double equipAccountId)
        {
            var equipAccount = RF.GetById<EquipAccount>(equipAccountId);
            if (equipAccount == null)
                throw new ValidationException("升级失败，该设备台账不存在。".L10N());
            if (equipAccount.TreePId == null)
                throw new ValidationException("升级失败，该设备台账已经是最高级。".L10N());

            var parentEquipAccount = RF.GetById<EquipAccount>(equipAccount.TreePId);
            if (parentEquipAccount == null)
                throw new ValidationException("升级失败，该设备台账的父级不存在，请检查。".L10N());

            //把台账的父ID赋值父实体的上级ID
            DB.Update<EquipAccount>().Set(p => p.TreePId, parentEquipAccount.TreePId).Where(p => p.Id == equipAccountId).Execute();
        }

        /// <summary>
        /// 查询设备台账列表
        /// </summary>
        /// <param name="criteria">设备台账查询对象</param>
        /// <returns>设备台账列表</returns>
        public virtual EntityList<EquipAccount> GetEquipAccountsByCriteria(EquipAccountCriteria criteria)
        {
            if (criteria.IsLoadAll)
            {
                //(不过滤设备权限）
                using (SIE.DataAuth.DataAuths.LoadAll())
                {
                    return GetEquipAccountsByCriteriaInner(criteria);
                }
            }
            else
            {
                return GetEquipAccountsByCriteriaInner(criteria);
            }
        }

        /// <summary>
        /// 查询设备台账列表
        /// </summary>
        /// <param name="criteria">设备台账查询对象</param>
        /// <returns>设备台账列表</returns>
        private EntityList<EquipAccount> GetEquipAccountsByCriteriaInner(EquipAccountCriteria criteria)
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

            if (criteria.ModelCode.IsNotEmpty() || criteria.ModelName.IsNotEmpty() || criteria.TypeCategory.IsNotEmpty() || (criteria.EquipTypeId != null && criteria.EquipTypeId != 0))
            {
                query.Join<EquipModel>((x, y) => x.EquipModelId == y.Id)
                .WhereIf<EquipModel>(criteria.ModelCode.IsNotEmpty(), (x, y) => y.Code.Contains(criteria.ModelCode))
                .WhereIf<EquipModel>(criteria.ModelName.IsNotEmpty(), (x, y) => y.Name.Contains(criteria.ModelName))
                .WhereIf<EquipModel>(criteria.EquipTypeId != null && criteria.EquipTypeId != 0, (x, y) => y.EquipTypeId == criteria.EquipTypeId)
                .WhereIf<EquipModel>(criteria.TypeCategory.IsNotEmpty(), (x, y) => y.TypeCategory == criteria.TypeCategory);
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
            //用于盘点计划选择设备台账
            if (criteria.FactoryId.HasValue)
            {
                query.Where(p => p.FactoryId == criteria.FactoryId.Value);
            }
            if (criteria.IsSelect)
            {
                query.Where(p => p.UseState != AccountUseState.ToAccepted && p.UseState != AccountUseState.Scrap && p.UseState != AccountUseState.DisposedOf);
            }
            if (criteria.EquipOther.IsNotEmpty())
            {
                query.Where(p => p.Alias.Contains(criteria.EquipOther));
            }
            if (criteria.UseDeptId != null && criteria.UseDeptId != 0)
            {
                query.Where(p => p.UseDepartmentId == criteria.UseDeptId);
            }
            if (criteria.ManageDeptId != null && criteria.ManageDeptId != 0)
            {
                query.Where(p => p.ManageDepartmentId == criteria.ManageDeptId);
            }
            return query.OrderByDescending(p => p.UpdateDate).OrderBy(criteria.OrderInfoList).ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取委外维修设备编码
        /// </summary>
        /// <param name="pagingInfo">分页</param>
        /// <param name="keyword">编码</param>
        /// <returns>设备编码</returns>
        public virtual EntityList<EquipAccount> GetRepairAccounts(PagingInfo pagingInfo, string keyword)
        {
            return Query<EquipAccount>().Where(p => p.UseState == AccountUseState.OutsourcedRepair).WhereIf(!keyword.IsNullOrWhiteSpace(), p => p.Code.Contains(keyword)).ToList(pagingInfo);
        }

        /// <summary>
        /// 获取设备台账编码
        /// </summary>
        /// <returns>设备台账单号</returns>
        public virtual string GetAccountNo()
        {
            var config = ConfigService.GetConfig(new AccountNoConfig(), typeof(EquipAccount));
            if (config == null || config.NumberRuleId == null)
            {
                throw new ValidationException("未找到编码生成规则,请检查规则配置".L10N());
            }
            return RT.Service.Resolve<NumberRuleController>().GenerateSegment(config.NumberRule.Id, 1).FirstOrDefault();
        }

        /// <summary>
        /// 根据设备台账的设备型号获取其相关信息(包括位置列表)
        /// </summary>
        /// <param name="account">设备台账</param>
        /// <returns>设备台账信息</returns>
        public virtual EquipAccountInfo GetEquipModelRelateInfos(EquipAccount account)
        {
            EquipAccountInfo accountInfo = new EquipAccountInfo();

            DateTime now = RF.Find<EquipAccount>().GetDbTime();

            #region 加载初始数据
            //获取设备型号对应的点检保养项目列表
            var modelIds = new List<double>() { account.EquipModelId };
            var model = RT.Service.Resolve<EquipModelController>()
                .GetEquipModelsOfModel(account.EquipModelId);

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
            accountInfo.VirtualDevice = (int)model.VirtualDevice;
            accountInfo.FeederBinding = (int)model.FeederBinding;
            accountInfo.FeederLocFailSafe = (int)model.FeederLocFailSafe;
            accountInfo.FeederBarcodeFailSafe = (int)model.FeederBarcodeFailSafe;
            accountInfo.IsDisabled = (int)model.IsDisabled;
            accountInfo.AgingType = (int?)model.AgingType;
            accountInfo.ProductionType = (int?)model.ProductionType;

            accountInfo.LocationList.AddRange(locations);
            accountInfo.PhysicalUnionList = new EntityList<EquipAccountPhysicalUnion>();

            return accountInfo;
        }

        /// <summary>
        /// 创建设备台账位置列表
        /// </summary>
        /// <param name="equipAccount">设备台账</param>
        /// <param name="locOfModel">设备型号位置列表</param>
        /// <returns>设备台账位置列表</returns>
        private EquipAccountLocation CreateLocation(EquipAccount equipAccount, EquipModelLocation locOfModel)
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
        /// 保存设备台账工序
        /// </summary>
        /// <param name="equipAccountProcesses"></param>
        public virtual void SaveEquipAccountProcessList(List<EquipAccountProcess> equipAccountProcesses)
        {
            var equipAccountProcess = equipAccountProcesses.FirstOrDefault();

            var processIds = equipAccountProcesses
                .Select(x => x.ProcessId).Distinct().ToList();

            var equipAccountId = equipAccountProcess.EquipAccountId;

            var equipAccountProcessesOfExists = Query<EquipAccountProcess>()
                .Where(x => x.EquipAccountId == equipAccountId)
                .Where(x => processIds.Contains(x.ProcessId))
                .ToList(null, new EagerLoadOptions().LoadWithViewProperty());

            if (equipAccountProcessesOfExists.Any())
            {
                throw new ValidationException("工序【{0}】已经添加，请不要重复添加。"
                    .L10nFormat(string.Join(",", equipAccountProcessesOfExists.Select(x => x.ProcessName).Distinct())));
            }

            EntityList<EquipAccountProcess> equipProcesss = new EntityList<EquipAccountProcess>();

            equipAccountProcesses.ForEach(p =>
            {
                p.PersistenceStatus = PersistenceStatus.New;
                equipProcesss.Add(p);
            });

            RF.Save(equipProcesss);
        }

        /// <summary>
        /// 保存设备台账产品
        /// </summary>
        /// <param name="equipAccountProducts"></param>
        public virtual void SaveEquipAccountProductList(List<EquipAccountProduct> equipAccountProducts)
        {
            var equipAccountProduct = equipAccountProducts.FirstOrDefault();

            var productIds = equipAccountProducts
                .Select(x => x.ProductId).Distinct().ToList();

            var equipAccountId = equipAccountProduct.EquipAccountId;

            var equipAccountProcductsOfExists = Query<EquipAccountProduct>()
                .Where(x => x.EquipAccountId == equipAccountId)
                .Where(x => productIds.Contains(x.ProductId))
                .ToList(null, new EagerLoadOptions().LoadWithViewProperty());

            if (equipAccountProcductsOfExists.Any())
            {
                throw new ValidationException("产品【{0}】已经添加，请不要重复添加。"
                    .L10nFormat(string.Join(",", equipAccountProcductsOfExists.Select(x => x.ProductName).Distinct())));
            }

            EntityList<EquipAccountProduct> equipProducts = new EntityList<EquipAccountProduct>();

            equipAccountProducts.ForEach(p =>
            {
                p.PersistenceStatus = PersistenceStatus.New;
                equipProducts.Add(p);
            });

            RF.Save(equipProducts);
        }
        /// <summary>
        /// 设置LOGO
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual void SetEquipLogo(double id)
        {
            using (var tran = DB.TransactionScope(EquipmentEntityDataProvider.ConnectionStringName))
            {
                DB.Update<EquipAccountAttachment>().Set(p => p.IsEquipLogo, true).Where(p => p.Id == id).Execute();
                DB.Update<EquipAccountAttachment>().Set(p => p.IsEquipLogo, false).Where(p => p.Id != id).Execute();
                tran.Complete();
            }
        }
        /// <summary>
        /// 删除附件
        /// </summary>
        /// <param name="ids"></param>
        public virtual void DelEquip(double ids)
        {
            var equipAccount = RF.GetById<EquipAccountAttachment>(ids);
            if (equipAccount != null)
            {
                RT.Service.Resolve<SIE.Common.Attachments.AttachmentController>().DeleteFile(equipAccount.FileName, equipAccount.FilePath);
            }
            DB.Delete<EquipAccountAttachment>().Where(p =>p.Id == ids).Execute();
        }

        /// <summary>
        /// 删除上传的文件(单个)
        /// </summary>
        /// <param name="attachment"></param>
        public virtual void DeleteAttachmentFile(EquipAccountAttachment attachment)
        {
            RT.Service.Resolve<AttachmentController>().DeleteFile(attachment.FileName, attachment.FilePath);

        }

        /// <summary>
        /// 打印条码
        /// </summary>
        /// <param name="equipAccounts">设备台账</param>
        /// <param name="reason">原因</param>
        /// <param name="times">打印次数</param>
        public virtual string PrintEquipAccounts(List<EquipAccount> equipAccounts, string reason, int times)
        {
            string errMsg = string.Empty;
            try
            {
                Print(equipAccounts, reason, times);
            }
            catch (Exception exc)
            {
                errMsg = exc.Message;
            }

            return errMsg;
        }

        /// <summary>
        /// 打印条码
        /// </summary>
        /// <param name="equipAccounts">设备台账</param>
        /// <param name="reason">原因</param>
        /// <param name="times">打印次数</param>
        public virtual void Print(List<EquipAccount> equipAccounts, string reason, int times)
        {

            if (equipAccounts == null || equipAccounts.Count == 0)
            {
                throw new ValidationException("请选择设备台账进行打印".L10N());
            }
            if (times < 1)
            {
                throw new ValidationException("打印次数：{0} 必须大于等于 1".L10nFormat(times));
            }
        }

        /// <summary>
        /// 根据设备台账获取设备履历
        /// </summary>
        /// <param name="id"></param>
        /// <param name="sortInfo"></param>
        /// <param name="pagingInfo"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public virtual EntityList<EquipAccountResume> GetEquipAccountResumes(double id, List<OrderInfo> sortInfo, PagingInfo pagingInfo, ResumeType? state)
        {
            var query = Query<EquipAccountResume>();
            if (id > 0)
                query.Where(p => p.EquipAccountId == id);
            if (state.HasValue)
                query.Where(p => p.ResumeType == state);

            return query.OrderBy(sortInfo).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据设备台账获取附件信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="sortInfo"></param>
        /// <param name="pagingInfo"></param>        
        /// <returns></returns>
        public virtual EntityList<EquipAccountAttachment> GetEquipAccountAttachment(double id, List<OrderInfo> sortInfo, PagingInfo pagingInfo)
        {
            var query = Query<EquipAccountAttachment>();
            if (id > 0)
            {
                query.Where(p => p.OwnerId == id);
            }
            return query.OrderBy(sortInfo).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 修改设备状态
        /// </summary>
        /// <param name="strDeviceCode">设备代码</param>
        /// <param name="eaState">设备状态</param>
        /// <param name="strWarehouseCode">仓库编码（对应设备状态）</param>
        /// <returns>修改所影响的行数</returns>
        public virtual int UpdateDeviceStatus(string strDeviceCode, AccountState eaState, string strWarehouseCode)
        {
            var count = DB.Update<EquipAccount>()
               .Set(p => p.State, eaState)
               .Where(p => p.Code == strDeviceCode && p.InstallationLocation == strWarehouseCode)
               .Execute();
            return count;
        }

        /// <summary>
        /// 获取所有的设备型号(无视权限)
        /// </summary>
        /// <param name="pagingInfo"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public virtual EntityList<EquipModel> GetEquipModelLoadAll(PagingInfo pagingInfo, string keyword)
        {
            using (SIE.DataAuth.DataAuths.LoadAll())
            {
                return Query<EquipModel>().Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword)).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
            }
        }

        /// <summary>
        /// 获取所有计量设备型号
        /// </summary>
        /// <param name="pagingInfo"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public virtual EntityList<EquipModel> GetMeteringEquipModelLoadAll(PagingInfo pagingInfo, string keyword)
        {
            //获取设备台账配置的计量设备查询信息
            List<string> meteringCodes = new List<string>();
            List<Catalog> catalogList = RT.Service.Resolve<CatalogController>().GetCatalogList(EquipType.EquipTypeCatalogType).ToList();
            var config = ConfigService.GetConfig(new EquipModelEquipmentCategoryConfig(), typeof(EquipModel));
            if (config != null && config.EquipmentMeteringIds.IsNotEmpty())
            {
                List<string> cataIds = config.EquipmentMeteringIds.Split(',').ToList();
                meteringCodes = catalogList.Where(p => cataIds.Contains(p.Id.ToString())).Select(p => p.Code).ToList();
            }
            if (meteringCodes.Count <= 0)
            {
                return new EntityList<EquipModel>();
            }
            using (SIE.DataAuth.DataAuths.LoadAll())
            {
                return Query<EquipModel>()
                    .Where(p => meteringCodes.Contains(p.TypeCategory) && (p.Code.Contains(keyword) || p.Name.Contains(keyword))).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
            }
        }
        /// <summary>
        /// 根据编码获取设备型号
        /// </summary>
        /// <param name="codes"></param>
        /// <returns></returns>
        public virtual EntityList<EquipModel> GetEquipModelsByCodes(List<string> codes)
        {
            var list = codes.SplitContains(c =>
            {
                return Query<EquipModel>().Where(p => c.Contains(p.Code)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
            return list;
        }


        /// <summary>
        /// 获取设备
        /// </summary>
        /// <param name="pagingInfo"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public virtual EntityList<EquipAccount> GetEquipAccountBykeyword(PagingInfo pagingInfo, string keyword)
        {
                return Query<EquipAccount>().Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword)).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据设备ID获取附件
        /// </summary>
        /// <param name="eqpIds">设备ID集合</param>
        /// <returns></returns>
        public virtual EntityList<EquipAccountAttachment> GetEquipAccountAttachmentsByIds(List<double?> eqpIds)
        {
            return eqpIds.SplitContains(ids =>
            {
                return Query<EquipAccountAttachment>().Where(p => ids.Contains(p.OwnerId)).ToList();
            });
        }

        /// <summary>
        /// 获取设备投资
        /// </summary>
        /// <param name="pageInfo">分页信息</param>
        /// <param name="key">关键字</param>
        /// <param name="incloudStates">包含的状态</param>
        /// <param name="exceptStates">排除的状态</param>
        /// <returns></returns>
        public virtual EntityList<EquipAccount> GetAllEquipAccounts(PagingInfo pageInfo, string key, List<SIE.Core.Enums.AccountUseState> incloudStates, List<SIE.Core.Enums.AccountUseState> exceptStates)
        {
            using (SIE.DataAuth.DataAuths.LoadAll())
            {
                return Query<EquipAccount>().Where(p => p.Code.Contains(key) || p.Name.Contains(key))
                    .WhereIf(incloudStates != null && incloudStates.Count > 0, p => incloudStates.Contains(p.UseState))
                    .WhereIf(exceptStates != null && exceptStates.Count > 0, p => !exceptStates.Contains(p.UseState))
                    .ToList(pageInfo, new EagerLoadOptions().LoadWithViewProperty());
            }
        }

        /// <summary>
        /// 根据设备台账ID获取产品列表
        /// </summary>
        /// <param name="equipId"></param>
        /// <param name="orderInfos"></param>
        /// <param name="pagingInfo"></param>
        /// <returns></returns>
        public virtual EntityList<EquipAccountProduct> GetEquipAccountProducts(double equipId, IList<OrderInfo> orderInfos, PagingInfo pagingInfo)
        {
            var query = Query<EquipAccountProduct>();            
            query.Where(p => p.EquipAccountId == equipId);
            if (orderInfos != null && orderInfos.Count > 0)
                query.OrderBy(orderInfos);

            return query.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }
    }
}

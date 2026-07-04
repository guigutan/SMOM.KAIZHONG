using NPOI.SS.Formula.Functions;
using SIE.Common.Configs;
using SIE.Common.NumberRules;
using SIE.Core.ApiModels;
using SIE.Core.Common.Controllers;
using SIE.Core.Enums;
using SIE.CSM.Suppliers;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EMS.EquipLends.ApiModels;
using SIE.EMS.EquipLends.Configs;
using SIE.EMS.EquipLends.Enums;
using SIE.EMS.Equipments;
using SIE.EMS.Equipments.ApiModels;
using SIE.Equipments.EquipAccounts;
using SIE.Resources.Employees;
using SIE.Resources.Enterprises;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.EMS.EquipLends
{
    /// <summary>
    /// 设备借还管理控制器
    /// </summary>
    public partial class EquipLendController : DomainController
    {
        #region 查询
        /// <summary>
        /// 查询实体查询逻辑
        /// </summary>
        /// <param name="criteria">设备借还查询实体</param>
        /// <returns></returns>
        public virtual EntityList<EquipLendManage> CriteriaQueryEntityList(EquipLendManageCriteria criteria)
        {
            if (criteria == null)
            {
                return new EntityList<EquipLendManage>();
            }
            var query = Query<EquipLendManage>();
            if (criteria.EquipCode.IsNotEmpty())
            {
                query.Where(p => p.EquipAccount.Code.Contains(criteria.EquipCode));
            }
            if (criteria.FixCode.IsNotEmpty())
            {
                query.Where(p => p.EquipAccount.FixedAssetsAccount.Code.Contains(criteria.FixCode));
            }
            if (criteria.RFID.IsNotEmpty())
            {
                query.Where(p => p.EquipAccount.RFID.Contains(criteria.RFID));
            }
            if (criteria.EquipModelId != null && criteria.EquipModelId != 0)
            {
                query.Where(p => p.EquipAccount.EquipModelId ==  criteria.EquipModelId);
            }
            if (criteria.State.HasValue)
            {
                query.Where(p => p.LendState == criteria.State.Value);
            }
            if (criteria.LendObject.HasValue)
            {
                query.Where(p => p.LendObject == criteria.LendObject.Value);
            }
            if (criteria.LendEnterpriseId != null && criteria.LendEnterpriseId != 0)
            {
                query.Where(p => p.LendEnterpriseId == criteria.LendEnterpriseId);
            }
            if (criteria.LendEmployeeId != null && criteria.LendEmployeeId != 0)
            {
                query.Where(p => p.LendEmployeeId == criteria.LendEmployeeId);
            }
            if (criteria.LendDate.BeginValue.HasValue)
            {
                query.Where(p => p.CreateDate >= criteria.LendDate.BeginValue.Value);
            }
            if (criteria.LendDate.EndValue.HasValue)
            {
                query.Where(p => p.CreateDate <=  criteria.LendDate.EndValue.Value);
            }
            return query.OrderBy(criteria.OrderInfoList).ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据Id获取设备借还数据
        /// </summary>
        /// <param name="lendIds">设备借还Ids</param>
        /// <returns></returns>
        public virtual List<EquipLendSubmitInfo> GetEquipLendManageByIds(List<double> lendIds)
        {
            List<EquipLendSubmitInfo> equipLendSubmitInfos = new List<EquipLendSubmitInfo>();
            lendIds.SplitDataExecute(tempIds =>
            {
                var list = Query<EquipLendManage>().Where(p => tempIds.Contains(p.Id))
                .Select(p => new
                {
                    Id = p.Id,
                    No = p.No,
                    LendState = p.LendState,
                    EquipAccountId = p.EquipAccountId,
                    LendEnterpriseId = p.LendEnterpriseId,
                    LendEmployeeId = p.LendEmployeeId,
                }).ToList<EquipLendSubmitInfo>();
                equipLendSubmitInfos.AddRange(list);
            });
            return equipLendSubmitInfos;
        }

        /// <summary>
        /// 查询设备是否有某些状态的设备借还数据
        /// </summary>
        /// <param name="id">排除自身Id</param>
        /// <param name="equipId">设备台账Id</param>
        /// <param name="states">设备借还状态</param>
        /// <returns></returns>
        public virtual LendState? GetStateEquipLendCount(double id, double equipId, List<LendState> states)
        {
            var state = Query<EquipLendManage>().Where(p => p.Id != id && p.EquipAccountId == equipId && states.Contains(p.LendState)).Select(p => p.LendState).FirstOrDefault();
            if (state != null)
            {
                return state.LendState;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 配置项是否启用审核
        /// </summary>
        /// <returns></returns>
        public virtual bool IsNeedExamine()
        {
            bool needExamine = false;
            var config = ConfigService.GetConfig(new EquipLendManageConfig(), typeof(EquipLendManage));
            if (config != null)
            {
                needExamine = config.LendExamine || config.ReturnExamine;
            }
            return needExamine;
        }

        /// <summary>
        /// 是否需要归还审核
        /// </summary>
        /// <returns></returns>
        public virtual bool IsNeedReutrnExamine()
        {
            bool returnExamine = false;
            var config = ConfigService.GetConfig(new EquipLendManageConfig(), typeof(EquipLendManage));
            if (config != null)
            {
                returnExamine = config.ReturnExamine;
            }
            return returnExamine;
        }

        /// <summary>
        /// 导入根据设备编码获取设备信息
        /// </summary>
        /// <param name="codes">设备编码</param>
        /// <returns></returns>
        public virtual List<BaseDataInfo> ImportGetEquipInfoByCodes(List<string> codes)
        {
            List<BaseDataInfo> baseDataInfos = new List<BaseDataInfo>();
            codes.SplitDataExecute(temps =>
            {
                var list = Query<EquipAccount>().Where(p => temps.Contains(p.Code))
                .Select(p => new
                {
                    Id = p.Id,
                    Code = p.Code,
                }).ToList<BaseDataInfo>();
                baseDataInfos.AddRange(list);
            });
            return baseDataInfos;
        }

        /// <summary>
        /// 导入根据借机部门名称获取
        /// </summary>
        /// <param name="names">部门名称</param>
        /// <returns></returns>
        public virtual List<BaseDataInfo> ImportGetEnterpriseByNames(List<string> names)
        {
            List<EnterpriseType?> types = new List<EnterpriseType?> { EnterpriseType.Plant, EnterpriseType.Department, EnterpriseType.Shop, EnterpriseType.Line };
            List<BaseDataInfo> baseDataInfos = new List<BaseDataInfo>();
            names.SplitDataExecute(temps =>
            {
                var list = Query<Enterprise>()
                .Join<EnterpriseLevel>((x, y) => x.LevelId == y.Id)
                .Where<EnterpriseLevel>((x, y) => temps.Contains(x.Name) && types.Contains(y.Type))
                .Select(x => new
                {
                    Id = x.Id,
                    Name = x.Name,
                }).ToList<BaseDataInfo>();
                baseDataInfos.AddRange(list);
            });
            return baseDataInfos;
        }

        /// <summary>
        /// 导入根据工号获取员工信息
        /// </summary>
        /// <param name="codes">工号</param>
        /// <returns></returns>
        public virtual List<BaseDataInfo> ImportGetEmployeeByCodes(List<string> codes)
        {
            List<BaseDataInfo> baseDataInfos = new List<BaseDataInfo>();
            codes.SplitDataExecute(temps =>
            {
                var list = Query<Employee>().Where(p => temps.Contains(p.Code)).Select(p => new
                {
                    Id = p.Id,
                    Code = p.Code,
                }).ToList<BaseDataInfo>();
                baseDataInfos.AddRange(list);
            });
            return baseDataInfos;
        }

        /// <summary>
        /// 导入根据编码获取供应商
        /// </summary>
        /// <param name="codes">供应商编码</param>
        /// <returns></returns>
        public virtual List<BaseDataInfo> ImportGetSupplierByCodes(List<string> codes)
        {
            List<BaseDataInfo> baseDataInfos = new List<BaseDataInfo>();
            codes.SplitDataExecute(temps =>
            {
                var list = Query<Supplier>().Where(p => temps.Contains(p.Code)).Select(p => new
                {
                    Id = p.Id,
                    Code = p.Code,
                }).ToList<BaseDataInfo>();
                baseDataInfos.AddRange(list);
            });
            return baseDataInfos;
        }

        /// <summary>
        /// 导入获取设备对应的借还单
        /// </summary>
        /// <param name="equipIds">设备Ids</param>
        /// <returns></returns>
        public virtual List<BaseDataInfo> ImportGetStateEquipLend(List<double> equipIds)
        {
            List<LendState> states = new List<LendState> { LendState.Saved, LendState.LendExamine, LendState.HasLended, LendState.ReturnExamine };
            List<BaseDataInfo> baseDataInfos = new List<BaseDataInfo>();
            equipIds.SplitDataExecute(tempIds =>
            {
                var list = Query<EquipLendManage>().Where(p => tempIds.Contains(p.EquipAccountId) && states.Contains(p.LendState))
                    .Select(p => new
                    {
                        Id = p.EquipAccountId,
                    }).ToList<BaseDataInfo>();
                baseDataInfos.AddRange(list);
            });
            return baseDataInfos;
        }
        #endregion

        #region 业务逻辑

        /// <summary>
        /// 生成设备借机单号
        /// </summary>
        /// <param name="count">生成数量</param>
        /// <returns></returns>
        public virtual List<string> GetLendNos(int count = 1)
        {
            var config = ConfigService.GetConfig(new EquipLendManageConfig(), typeof(EquipLendManage));
            if (config == null || config.NoRuleId == null)
            {
                throw new ValidationException("未找到借还单号编码生成规则,请检查规则配置".L10N());
            }
            return RT.Service.Resolve<NumberRuleController>().GenerateSegment(config.NoRuleId.Value, count).ToList();
        }

        /// <summary>
        /// 设备借还保存前校验
        /// </summary>
        /// <param name="equipLendManage"></param>
        public virtual void EquipLendOnSavingValidate(EquipLendManage equipLendManage)
        {
            if (equipLendManage == null)
            {
                throw new ValidationException("页面数据错误，请刷新！".L10N());
            }
            StringBuilder error = new StringBuilder();
            if (equipLendManage.LendObject == Enums.LendObject.Internal) // 内部
            {
                if (equipLendManage.LendEnterpriseId == null || equipLendManage.LendEnterpriseId == 0)
                {
                    error.AppendLine("借机部门必填;".L10N());
                }
                if (equipLendManage.LendEmployeeId == null || equipLendManage.LendEmployeeId == 0)
                {
                    error.AppendLine("借机人必填;".L10N());
                }
            }
            else // 外部
            {
                if (equipLendManage.SupplierId == null || equipLendManage.SupplierId == 0)
                {
                    error.AppendLine("供应商必填;".L10N());
                }
            }
            if (equipLendManage.Reason.Length <= 0)
            {
                error.AppendLine("借出原因必填;".L10N());
            }
            List<LendState> states = new List<LendState> { LendState.Saved, LendState.LendExamine, LendState.HasLended, LendState.ReturnExamine };
            var existsState = GetStateEquipLendCount(equipLendManage.Id, equipLendManage.EquipAccountId, states);
            if (existsState != null)
            {
                error.AppendLine("设备已存在状态为{0}的单据，不能借出;".L10nFormat(existsState.ToLabel()));
            }

            if (error.Length > 0)
            {
                throw new ValidationException(error.ToString());
            }
        }

        /// <summary>
        /// 设备借还提交前校验
        /// </summary>
        /// <param name="equipLendManages">设备借还数据</param>
        private void EquipLendSubmitValidate(List<EquipLendSubmitInfo> equipLendManages)
        {
            if (equipLendManages.Count <= 0)
            {
                throw new ValidationException("请选择设备借还数据进行提交".L10N());
            }
            if (equipLendManages.Any(p => p.LendState != (int)LendState.Saved))
            {
                throw new ValidationException("只有单据状态为【{0}】时才能提交".L10nFormat(LendState.Saved.ToLabel().L10N()));
            }
        }

        /// <summary>
        /// 设备借还归还前校验
        /// </summary>
        /// <param name="equipLendManages"></param>
        private void EquipLendReturnValidate(List<EquipLendSubmitInfo> equipLendManages)
        {
            if (equipLendManages.Count <= 0)
            {
                throw new ValidationException("请选择设备借还数据进行归还".L10N());
            }
            if (equipLendManages.Any(p => p.LendState != (int)LendState.HasLended))
            {
                throw new ValidationException("只有单据状态为【{0}】时才能归还".L10nFormat(LendState.HasLended.ToLabel().L10N()));
            }
            if (equipLendManages.Select(p => p.LendEmployeeId).Distinct().Count() > 1)
            {
                throw new ValidationException("借机人不一致，不允许归还".L10N());
            }
        }

        /// <summary>
        /// 归还创建设备履历
        /// </summary>
        /// <param name="lend">借还单信息</param>
        /// <param name="equipInfos">设备信息</param>
        /// <returns></returns>
        private EquipAccountResume CreateReturnEquipResume(EquipLendSubmitInfo lend, List<EquipChangeInfo> equipInfos)
        {
            var equipInfo = equipInfos.FirstOrDefault(p => p.Id == lend.EquipAccountId);
            string changed = "管理状态";
            string remark = "管理状态{0}变更为{1}".L10nFormat(AccountUseState.Lease.ToLabel(), equipInfo.OldUseState.ToLabel());
            EquipAccountResume resume = new EquipAccountResume
            {
                ResumeType = SIE.Equipments.Enums.ResumeType.LendReturn,
                No = lend.No,
                EquipAccountId = lend.EquipAccountId,
                Changed = changed,
                Remark = remark,
            };
            return resume;
        }

        /// <summary>
        /// 借出创建设备履历
        /// </summary>
        /// <param name="lend">借还单信息</param>
        /// <param name="equipInfos">设备信息</param>
        /// <returns></returns>
        private EquipAccountResume CreateLendEquipResume(EquipLendSubmitInfo lend,  List<EquipChangeInfo> equipInfos)
        {
            var equipInfo = equipInfos.FirstOrDefault(p => p.Id == lend.EquipAccountId);
            string changed = "管理状态";
            string remark = "管理状态{0}变更为{1}".L10nFormat(equipInfo.UseState.ToLabel(), AccountUseState.Lease.ToLabel());
            EquipAccountResume resume = new EquipAccountResume
            {
                ResumeType = SIE.Equipments.Enums.ResumeType.LendReturn,
                No = lend.No,
                EquipAccountId = lend.EquipAccountId,
                Changed = changed,
                Remark = remark,
            };
            return resume;
        }

        /// <summary>
        /// 设备借还提交业务
        /// </summary>
        /// <param name="equipLendManages">设备借还数据</param>
        /// <param name="lendExamine">是否需要借出审核</param>
        public virtual void EquipLendSubmitOrder(List<EquipLendSubmitInfo> equipLendManages, bool lendExamine)
        {
            // 设备信息
            var equipChangeInfos = RT.Service.Resolve<EquipController>().GetEquipChangeInfos(equipLendManages.Select(p => p.EquipAccountId).ToList());
            EntityList<EquipAccountResume> resumes = new EntityList<EquipAccountResume>();

            // 是否启用借出审核
            LendState state = lendExamine ? LendState.LendExamine : LendState.HasLended;
            foreach(var lend in equipLendManages)
            {
                // 更新状态
                DB.Update<EquipLendManage>().Set(p => p.LendState, state).Where(p => p.Id == lend.Id).Execute();
                if (!lendExamine)
                {
                    // 不启用审核提交更新设备台账状态、使用部门、使用责任人
                    DB.Update<EquipAccount>()
                        .Set(p => p.OldUseState, p => p.UseState)
                        .Set(p => p.UseState, Core.Enums.AccountUseState.Lease)
                        .Where(p => p.Id == lend.EquipAccountId).Execute();
                    EquipAccountResume resume = CreateLendEquipResume(lend, equipChangeInfos);
                    resumes.Add(resume);
                }
            }
            if (resumes.Count > 0)
            {
                RT.Service.Resolve<CommonController>().BatchInsertSave(resumes);
            }
        }

        /// <summary>
        /// 设备借还归还业务
        /// </summary>
        /// <param name="equipLendManages">设备借还数据</param>
        /// <param name="returnExamine">是否归还审核</param>
        /// <param name="returnRemark">归还说明</param>
        public virtual void EquipLendReturnOrder(List<EquipLendSubmitInfo> equipLendManages, bool returnExamine, string returnRemark)
        {
            // 设备信息
            var equipChangeInfos = RT.Service.Resolve<EquipController>().GetEquipChangeInfos(equipLendManages.Select(p => p.EquipAccountId).ToList());
            EntityList<EquipAccountResume> resumes = new EntityList<EquipAccountResume>();

            LendState state = returnExamine ? LendState.ReturnExamine : LendState.HasReturned;
            foreach(var lend in equipLendManages)
            {
                // 更新状态和归还说明
                DB.Update<EquipLendManage>().Set(p => p.LendState, state).Set(p => p.ReturnRemark, returnRemark).Where(p => p.Id == lend.Id).Execute();
                if (!returnExamine)
                {
                    DB.Update<EquipAccount>()
                        .Set(p => p.UseState, p => p.OldUseState)
                        .Set(p => p.OldUseState, p => null)
                        .Where(p => p.Id == lend.EquipAccountId).Execute();
                    EquipAccountResume resume = CreateReturnEquipResume(lend, equipChangeInfos);
                    resumes.Add(resume);
                }
            }
            if (resumes.Count > 0)
            {
                RT.Service.Resolve<CommonController>().BatchInsertSave(resumes);
            }
        }

        /// <summary>
        /// 设备借还保存提交
        /// </summary>
        /// <param name="equipLendManage">设备借还实体</param>
        public virtual void EquipLendSaveSubmit(EquipLendManage equipLendManage)
        {
            EquipLendSubmitInfo equipLendSubmitInfo = new EquipLendSubmitInfo
            {
                Id = equipLendManage.Id,
                LendState = (int)equipLendManage.LendState,
                EquipAccountId = equipLendManage.EquipAccountId,
                LendEnterpriseId = equipLendManage.LendEnterpriseId,
                LendEmployeeId = equipLendManage.LendEmployeeId,
            };
            EquipLendSubmitValidate(new List<EquipLendSubmitInfo> { equipLendSubmitInfo });
            bool lendExamine = false; // 是否启用借出审核
            var config = ConfigService.GetConfig(new EquipLendManageConfig(), typeof(EquipLendManage));
            if (config != null)
            {
                lendExamine = config.LendExamine;
            }
            using (var tran = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                // 保存
                RF.Save(equipLendManage);
                EquipLendSubmitOrder(new List<EquipLendSubmitInfo> { equipLendSubmitInfo }, lendExamine);
                tran.Complete();
            }
        }

        /// <summary>
        /// 设备借还提交命令
        /// </summary>
        /// <param name="lendIds">设备借还Ids</param>
        public virtual void EquipLendSubmit(List<double> lendIds)
        {
            List<EquipLendSubmitInfo> equipLendSubmitInfos = GetEquipLendManageByIds(lendIds);
            EquipLendSubmitValidate(equipLendSubmitInfos); // 验证
            bool lendExamine = false; // 是否启用借出审核
            var config = ConfigService.GetConfig(new EquipLendManageConfig(), typeof(EquipLendManage));
            if (config != null)
            {
                lendExamine = config.LendExamine;
            }
            using (var tran = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                EquipLendSubmitOrder(equipLendSubmitInfos, lendExamine);
                tran.Complete();
            }
        }

        /// <summary>
        /// 设备借机归还命令
        /// </summary>
        /// <param name="returnRemark">归还说明</param>
        /// <param name="equipLendIds">设备借机Ids</param>
        public virtual void EquipLendReturn(string returnRemark, List<double> equipLendIds)
        {
            List<EquipLendSubmitInfo> equipLendSubmitInfos = GetEquipLendManageByIds(equipLendIds);
            EquipLendReturnValidate(equipLendSubmitInfos);
            bool returnExamine = false; // 是否启用归还审核
            var config = ConfigService.GetConfig(new EquipLendManageConfig(), typeof(EquipLendManage));
            if (config != null)
            {
                returnExamine = config.ReturnExamine;
            }
            using (var tran = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                EquipLendReturnOrder(equipLendSubmitInfos, returnExamine, returnRemark);
                tran.Complete();
            }
        }

        /// <summary>
        /// 借出审核处理
        /// </summary>
        /// <param name="pass">结果</param>
        /// <param name="remark">审核备注</param>
        /// <param name="info">借还信息</param>
        /// <param name="now">时间</param>
        /// <param name="equipLendExamineRecords">审核记录保存列表</param>
        /// <param name="resumes">设备履历保存列表</param>
        /// <param name="equipChangeInfos">设备信息</param>
        private void LendExamine(bool pass, string remark, EquipLendSubmitInfo info, DateTime now,
            EntityList<EquipLendExamineRecord> equipLendExamineRecords, EntityList<EquipAccountResume> resumes, List<EquipChangeInfo> equipChangeInfos)
        {
            LendState lendState = pass ? LendState.HasLended : LendState.Saved;
            // 更新借还单状态
            DB.Update<EquipLendManage>().Set(p => p.LendState, lendState).Where(p => p.Id == info.Id).Execute();
            // 生成一条审核记录
            EquipLendExamineRecord equipLendExamineRecord = new EquipLendExamineRecord
            {
                EquipLendManageId = info.Id,
                ExamineType = ExamineType.Lend,
                ExamineResult = pass ? ExamineResult.Pass : ExamineResult.Reject,
                EmployeeId = RT.IdentityId,
                ExamineDate = now,
                Remark = remark,
            };
            equipLendExamineRecords.Add(equipLendExamineRecord);
            // 通过更新设备台账信息
            if (pass)
            {
                DB.Update<EquipAccount>()
                    .Set(p => p.OldUseState, p => p.UseState)
                    .Set(p => p.UseState, Core.Enums.AccountUseState.Lease)
                    .Where(p => p.Id == info.EquipAccountId).Execute();
                EquipAccountResume resume = CreateLendEquipResume(info, equipChangeInfos);
                resumes.Add(resume);
            }
        }

        /// <summary>
        /// 归还审核处理
        /// </summary>
        /// <param name="pass">结果</param>
        /// <param name="remark">审核备注</param>
        /// <param name="info">借还信息</param>
        /// <param name="now">时间</param>
        /// <param name="equipLendExamineRecords">审核记录保存列表</param>
        /// <param name="resumes">设备履历保存列表</param>
        /// <param name="equipChangeInfos">设备信息</param>
        private void ReturnExamine(bool pass, string remark, EquipLendSubmitInfo info, DateTime now,
            EntityList<EquipLendExamineRecord> equipLendExamineRecords, EntityList<EquipAccountResume> resumes, List<EquipChangeInfo> equipChangeInfos)
        {
            LendState lendState = pass ? LendState.HasReturned : LendState.HasLended;
            // 更新借还单状态
            DB.Update<EquipLendManage>().Set(p => p.LendState, lendState).Where(p => p.Id == info.Id).Execute();
            // 生成一条审核记录
            EquipLendExamineRecord equipLendExamineRecord = new EquipLendExamineRecord
            {
                EquipLendManageId = info.Id,
                ExamineType = ExamineType.Return,
                ExamineResult = pass ? ExamineResult.Pass : ExamineResult.Reject,
                EmployeeId = RT.IdentityId,
                ExamineDate = now,
                Remark = remark,
            };
            equipLendExamineRecords.Add(equipLendExamineRecord);
            // 通过更新设备台账信息
            if (pass)
            {
                DB.Update<EquipAccount>()
                    .Set(p => p.UseState, p => p.OldUseState)
                    .Set(p => p.OldUseState, p => null)
                    .Where(p => p.Id == info.EquipAccountId).Execute();
                EquipAccountResume resume = CreateReturnEquipResume(info, equipChangeInfos);
                resumes.Add(resume);
            }
        }

        /// <summary>
        /// 设备借还审核
        /// </summary>
        /// <param name="equipLendExamineInfo">审核信息</param>
        /// <param name="equipLendIds">设备借机Ids</param>
        public virtual void EquipLendExamine(EquipLendExamineInfo equipLendExamineInfo, List<double> equipLendIds)
        {
            if (equipLendExamineInfo.Result == 20 && equipLendExamineInfo.Remark.Length <= 0)
            {
                throw new ValidationException("审核结果为驳回时，审核意见必填".L10N());
            }
            var equipLendInfos = GetEquipLendManageByIds(equipLendIds);
            if (equipLendInfos == null || equipLendInfos.Count == 0)
            {
                throw new ValidationException("设备借还单不存在，请刷新".L10N());
            }
            if (equipLendInfos.Any(p => p.LendState != 1 && p.LendState != 3))
            {
                throw new ValidationException("存在设备借还单状态不为借出待审核或归还待审核，请刷新".L10N());
            }
            // 设备信息
            var equipChangeInfos = RT.Service.Resolve<EquipController>().GetEquipChangeInfos(equipLendInfos.Select(p => p.EquipAccountId).ToList());
            EntityList<EquipAccountResume> resumes = new EntityList<EquipAccountResume>();


            bool pass = equipLendExamineInfo.Result == 10; // 审核结果
            string remark = equipLendExamineInfo.Remark; // 审核意见
            DateTime now = RF.Find<EquipLendManage>().GetDbTime();
            EntityList<EquipLendExamineRecord> equipLendExamineRecords = new EntityList<EquipLendExamineRecord>();
            using(var tran = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                foreach (var info in equipLendInfos)
                {
                    if (info.LendState == 1) // 借出单
                    {
                        LendExamine(pass, remark, info, now, equipLendExamineRecords, resumes, equipChangeInfos);
                    }
                    if (info.LendState == 3) // 归还单
                    {
                        ReturnExamine(pass, remark, info, now, equipLendExamineRecords, resumes, equipChangeInfos);
                    }
                }
                RT.Service.Resolve<CommonController>().BatchInsertSave(equipLendExamineRecords);
                if (resumes.Count > 0)
                {
                    RT.Service.Resolve<CommonController>().BatchInsertSave(resumes);
                }
                tran.Complete();
            }
            
        }
        #endregion
    }
}

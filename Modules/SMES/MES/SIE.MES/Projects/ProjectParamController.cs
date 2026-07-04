using DocumentFormat.OpenXml;
using SIE.Core.Common.Controllers;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Items;
using SIE.MES.Projects.ApiModels;
using SIE.MES.Projects.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.Projects
{
    /// <summary>
    /// 项目参数控制器
    /// </summary>
    public class ProjectParamController : DomainController
    {
        #region 查询

        /// <summary>
        /// 界面查询
        /// </summary>
        /// <param name="criteria">查询实体</param>
        /// <returns></returns>
        public virtual EntityList<ProjectParam> QueryProjectParams(ProjectParamCriteria criteria)
        {
            if (criteria == null)
            {
                return new EntityList<ProjectParam>();
            }
            var q = Query<ProjectParam>();
            if (criteria.Code.IsNotEmpty())
            {
                q.Where(p => p.Code.Contains(criteria.Code));
            }
            if (criteria.Name.IsNotEmpty())
            {
                q.Where(p => p.Name.Contains(criteria.Name));
            }
            if (criteria.Type.IsNotEmpty())
            {
                q.Where(p => p.Type == criteria.Type);
            }
            return q.OrderBy(criteria.OrderInfoList).ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 工序标准参数查询
        /// </summary>
        /// <param name="criteria">查询实体</param>
        /// <returns></returns>
        public virtual EntityList<ProcessStandardParam> QueryProcessStandardParams(ProcessStandardCriteria criteria)
        {
            if (criteria == null)
            {
                return new EntityList<ProcessStandardParam>();
            }
            var q = Query<ProcessStandardParam>();
            if (criteria.Type.IsNotEmpty())
            {
                q.Where(p => p.Type == criteria.Type);
            }
            if (criteria.ProductId != null && criteria.ProductId != 0)
            {
                q.Where(p => p.ProductId == criteria.ProductId);
            }
            if (criteria.ProcessId != null && criteria.ProcessId != 0)
            {
                q.Where(p => p.ProcessId  == criteria.ProcessId);
            }
            if (criteria.ProcessStStatus.HasValue)
            {
                q.Where(p => p.ProcessStStatus == criteria.ProcessStStatus);
            }
            if (criteria.CreateDate.BeginValue.HasValue)
            {
                q.Where(p => p.CreateDate >= criteria.CreateDate.BeginValue);
            }
            if (criteria.CreateDate.EndValue.HasValue)
            {
                q.Where(p => p.CreateDate <= criteria.CreateDate.EndValue);
            }
            return q.OrderBy(criteria.OrderInfoList).ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取编码名称项目参数
        /// </summary>
        /// <param name="codes">编码</param>
        /// <param name="names">名称</param>
        /// <param name="editIds">编辑数据Ids</param>
        /// <returns></returns>
        public virtual List<ProjectParam> GetValidateUniqueProjectParams(List<string> codes, List<string> names, IEnumerable<double> editIds)
        {
            List<ProjectParam> projectParams = new List<ProjectParam>();
            var list = Query<ProjectParam>().Where(p => (codes.Contains(p.Code) || names.Contains(p.Name)) && !editIds.Contains(p.Id)).ToList();
            projectParams.AddRange(list);
            return projectParams;
        }

        /// <summary>
        /// 获取产品、工序 标准参数
        /// </summary>
        /// <param name="productIds">产品Id</param>
        /// <param name="processIds">工序Id</param>
        /// <param name="editIds">编辑数据Ids</param>
        /// <returns></returns>
        public virtual List<ProcessStandardParam> GetValidateUniqueProcessParams(IEnumerable<double> productIds, IEnumerable<double> processIds, IEnumerable<double> editIds)
        {
            List<ProcessStandardParam> processStandardParams = new List<ProcessStandardParam>();
            var list = Query<ProcessStandardParam>().Where(p => productIds.Contains(p.ProductId) && processIds.Contains(p.ProcessId) && !editIds.Contains(p.Id)).ToList();
            processStandardParams.AddRange(list);
            return processStandardParams;
        }

        /// <summary>
        /// 获取同标准参数下的工序需求明细
        /// </summary>
        /// <param name="projectIds">参数编码</param>
        /// <param name="editIds">编辑数据Ids</param>
        /// <param name="parentIds">主数据</param>
        /// <returns></returns>
        public virtual List<ProcessStandardParamDtl> GetValidateUniqueProcessParamDtls(IEnumerable<double> projectIds, IEnumerable<double> editIds, IEnumerable<double> parentIds)
        {
            List<ProcessStandardParamDtl> processStandardParamDtls = new List<ProcessStandardParamDtl>();
            var list = Query<ProcessStandardParamDtl>().Where(p => projectIds.Contains(p.ProjectParamId) && parentIds.Contains(p.ProcessStandardParamId) && !editIds.Contains(p.Id)).ToList();
            processStandardParamDtls.AddRange(list);
            return processStandardParamDtls;
        }

        /// <summary>
        /// 获取当前产品所有工序的标准工序参数明细
        /// </summary>
        /// <param name="productIds">产品Ids</param>
        /// <param name="processIds">工序Ids</param>
        /// <returns></returns>
        public virtual List<ProStdParamDtlInfo> GetProcessStandardParamDtlsByProcess(IEnumerable<double> productIds, IEnumerable<double> processIds)
        {
            List<ProStdParamDtlInfo> paramDtls = new List<ProStdParamDtlInfo>();
            productIds.SplitDataExecute(tempIds1 =>
            {
                processIds.SplitDataExecute(tempIds2 =>
                {
                    var list = Query<ProcessStandardParamDtl>()
                    .Join<ProcessStandardParam>((pspd, psp) => pspd.ProcessStandardParamId == psp.Id && tempIds1.Contains(psp.ProductId) && tempIds2.Contains(psp.ProcessId) && psp.ProcessStStatus == Enums.ProcessStStatus.Examined)
                    .Select<ProcessStandardParam>((pspd, psp) => new
                    {
                        ProjectParamId = pspd.ProjectParamId,
                        ProcessStDtlValueType = pspd.ProcessStDtlValueType,
                        Unit = pspd.Unit,
                        SingleValue = pspd.SingleValue,
                        RangeMaxValue = pspd.RangeMaxValue,
                        RangeMinValue = pspd.RangeMinValue,
                        ProcessStandardParamId = psp.Id,
                        ProductId = psp.ProductId,
                        ProcessId = psp.ProcessId,
                    })
                    .ToList<ProStdParamDtlInfo>();
                    paramDtls.AddRange(list);
                });
            });
            return paramDtls;
        }

        /// <summary>
        /// 获取同标准参数下的工序需求明细
        /// </summary>
        /// <param name="standardIds">主表Ids</param>
        /// <returns></returns>
        public virtual Dictionary<double, List<ProcessStandardParamDtl>> GetProcessParamDtlByIds(IEnumerable<double> standardIds)
        {
            List<ProcessStandardParamDtl> processStandardParamDtls = new List<ProcessStandardParamDtl>();
            var list = Query<ProcessStandardParamDtl>().Where(p => standardIds.Contains(p.ProcessStandardParamId)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            processStandardParamDtls.AddRange(list);
            return processStandardParamDtls.GroupBy(p => p.ProcessStandardParamId).ToDictionary(p => p.Key, p => p.ToList());
        }

        /// <summary>
        /// 下拉查询项目参数
        /// </summary>
        /// <param name="pagingInfo">分页信息</param>
        /// <param name="key">关键字</param>
        /// <returns></returns>
        public virtual EntityList<ProjectParam> GetProjectParams(PagingInfo pagingInfo, string key)
        {
            var q = Query<ProjectParam>().Where(p => p.Code.Contains(key) || p.Name.Contains(key)).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
            return q;
        }

        /// <summary>
        /// 获取非待审核条数
        /// </summary>
        /// <param name="ids">参数Ids</param>
        /// <returns></returns>
        public virtual int GetNotToExamineCount(IEnumerable<double> ids)
        {
            int count = 0;
            ids.SplitDataExecute(tempIds =>
            {
                var qCount = Query<ProcessStandardParam>().Where(p => tempIds.Contains(p.Id) && p.ProcessStStatus != Enums.ProcessStStatus.ToExamine).Count();
                count += qCount;
            });
            return count;
        }

        /// <summary>
        /// 获取非已审核条数
        /// </summary>
        /// <param name="ids">参数Ids</param>
        /// <returns></returns>
        public virtual int GetNotExaminedCount(IEnumerable<double> ids)
        {
            int count = 0;
            ids.SplitDataExecute(tempIds =>
            {
                var qCount = Query<ProcessStandardParam>().Where(p => tempIds.Contains(p.Id) && p.ProcessStStatus != Enums.ProcessStStatus.Examined).Count();
                count += qCount;
            });
            return count;
        }
        #endregion

        #region 业务逻辑

        /// <summary>
        /// 必填校验
        /// </summary>
        /// <param name="projectParams">保存数据</param>
        private void ValidateRequire(IEnumerable<ProjectParam> projectParams)
        {
            if (projectParams.Any(p => p.Code.IsNullOrEmpty()))
            {
                throw new ValidationException("编码不能为空".L10N());
            }
            if (projectParams.Any(p => p.Name.IsNullOrEmpty()))
            {
                throw new ValidationException("名称不能为空".L10N());
            }
            if (projectParams.Any(p => p.Type.IsNullOrEmpty()))
            {
                throw new ValidationException("类型不能为空".L10N());
            }
        }

        /// <summary>
        /// 工序标准参数必填校验
        /// </summary>
        /// <param name="processStandardParams"></param>
        private void ValidateRequire(IEnumerable<ProcessStandardParam> processStandardParams)
        {
            if (processStandardParams.Any(p => p.Type.IsNullOrEmpty()))
            {
                throw new ValidationException("类型不能为空".L10N());
            }
        }

        /// <summary>
        /// 重复校验
        /// </summary>
        /// <param name="newProjectParams">新增数据</param>
        /// <param name="editIds">排除的修改数据</param>
        private void ValidateUnique(IEnumerable<ProjectParam> newProjectParams, IEnumerable<double> editIds)
        {
            string _codeUnique = "编码重复不允许保存".L10N();
            string _nameUnique = "名称重复不允许保存".L10N();
            var codeHash = new HashSet<string>();
            if (newProjectParams.Any(p => !codeHash.Add(p.Code)))
            {
                throw new ValidationException(_codeUnique);
            }

            var nameHash = new HashSet<string>();
            if (newProjectParams.Any(p => !nameHash.Add(p.Name)))
            {
                throw new ValidationException(_nameUnique);
            }

            var dbList = GetValidateUniqueProjectParams(codeHash.ToList(), nameHash.ToList(), editIds);
            if (dbList.Any(p => !codeHash.Add(p.Code)))
            {
                throw new ValidationException(_codeUnique);
            }
            if (dbList.Any(p => !nameHash.Add(p.Name)))
            {
                throw new ValidationException(_nameUnique);
            }
            
        }

        /// <summary>
        /// 工序标准参数重复校验
        /// </summary>
        /// <param name="newProcessParams">新增数据</param>
        /// <param name="editIds">排除的修改数据</param>
        private void ValidateUnique(IEnumerable<ProcessStandardParam> newProcessParams, IEnumerable<double> editIds)
        {
            string repeatStr = "产品编码+工序唯一".L10N();
            var repeatHash = new HashSet<string>();
            if (newProcessParams.Any(p => !repeatHash.Add(p.ProductId + "@" + p.ProcessId)))
            {
                throw new ValidationException(repeatStr);
            }

            var dbList = GetValidateUniqueProcessParams(newProcessParams.Select(p => p.ProductId), newProcessParams.Select(p => p.ProcessId), editIds);
            if (dbList.Any(p => !repeatHash.Add(p.ProductId + "@" + p.ProcessId)))
            {
                throw new ValidationException(repeatStr);
            }
        }


        /// <summary>
        /// 工序标准参数明细重复校验
        /// </summary>
        /// <param name="newProcessParamDtls">新增数据</param>
        /// <param name="editIds">排除的修改数据</param>
        /// <param name="parentIds">主表数据</param>
        private void ValidateDetailUnique(IEnumerable<ProcessStandardParamDtl> newProcessParamDtls, IEnumerable<double> editIds, IEnumerable<double> parentIds)
        {
            //if (newProcessParamDtls.Any(p => p.RangeMaxValue < p.RangeMinValue))
            //{
            //    throw new ValidationException("上限值不能小于下限值".L10N());
            //}

            string repeatStr = "同一工序标准参数下工序需求属性组的参数编码唯一".L10N();
            var repeatHash = new HashSet<string>();
            if (newProcessParamDtls.Any(p => !repeatHash.Add(p.ProcessStandardParamId + "@" + p.ProjectParamId)))
            {
                throw new ValidationException(repeatStr);
            }

            var dbList = GetValidateUniqueProcessParamDtls(newProcessParamDtls.Select(p => p.ProjectParamId), editIds, parentIds);
            if (dbList.Any(p => !repeatHash.Add(p.ProcessStandardParamId + "@" + p.ProjectParamId)))
            {
                throw new ValidationException(repeatStr);
            }
        }

        /// <summary>
        /// 项目参数保存前校验
        /// </summary>
        /// <param name="projectParams">保存数据</param>
        public virtual void ParamBeforeSaving(EntityList<ProjectParam> projectParams)
        {
            if (!projectParams.Any())
            {
                return;
            }
            // 非空校验
            ValidateRequire(projectParams);
            // 唯一校验
            ValidateUnique(projectParams, projectParams.Where(p => p.PersistenceStatus == PersistenceStatus.Modified).Select(p => p.Id));
        }

        /// <summary>
        /// 工序标准参数保存前校验
        /// </summary>
        /// <param name="processStandardParams">保存数据</param>
        public virtual void ProcessStandardBeforeSaving(EntityList<ProcessStandardParam> processStandardParams)
        {
            if (!processStandardParams.Any())
            {
                return;
            }
            // 非空校验
            ValidateRequire(processStandardParams);
            // 主表唯一校验
            ValidateUnique(processStandardParams, processStandardParams.Where(p => p.PersistenceStatus == PersistenceStatus.Modified).Select(p => p.Id));
            // 明细表唯一校验
            ValidateDetailUnique(processStandardParams.SelectMany(p => p.ProcessDtlList), processStandardParams.SelectMany(p => p.ProcessDtlList).Where(p => p.PersistenceStatus != PersistenceStatus.New).Select(p => p.Id), processStandardParams.Select(p => p.Id));
        }

        /// <summary>
        /// 工序标准参数保存
        /// </summary>
        /// <param name="processStandardParams">保存数据</param>
        public virtual void ProcessStandardSave(EntityList<ProcessStandardParam> processStandardParams)
        {
            foreach (var p in processStandardParams)
            {
                p.ProcessStStatus = Enums.ProcessStStatus.ToExamine;
            }
            RF.Save(processStandardParams);
        }

        /// <summary>
        /// 审核工序标准参数
        /// </summary>
        /// <param name="ids">参数Ids</param>
        public virtual void ExamineProcessStandard(IEnumerable<double> ids)
        {
            if (GetNotToExamineCount(ids) > 0)
            {
                throw new ValidationException("存在工序标准参数状态不为待审核，请刷新界面".L10N());
            }
            using(var tran = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                ids.SplitDataExecute(tempIds =>
                {
                    DB.Update<ProcessStandardParam>().Where(p => tempIds.Contains(p.Id)).Set(p => p.ProcessStStatus, Enums.ProcessStStatus.Examined).Execute();
                });
                tran.Complete();
            }
        }

        /// <summary>
        /// 撤销审核工序标准参数
        /// </summary>
        /// <param name="ids">参数Ids</param>
        public virtual void RevokeExamineProcessStandard(IEnumerable<double> ids)
        {
            if (GetNotExaminedCount(ids) > 0)
            {
                throw new ValidationException("存在工序标准参数状态不为已审核，请刷新界面".L10N());
            }
            using (var tran = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                ids.SplitDataExecute(tempIds =>
                {
                    DB.Update<ProcessStandardParam>().Where(p => tempIds.Contains(p.Id)).Set(p => p.ProcessStStatus, Enums.ProcessStStatus.ToExamine).Execute();
                });
                tran.Complete();
            }
        }
        #endregion
    }
}

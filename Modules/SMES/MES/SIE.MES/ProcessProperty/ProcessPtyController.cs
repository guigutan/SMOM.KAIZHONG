using DocumentFormat.OpenXml.Office2021.DocumentTasks;
using SIE.Api;
using SIE.Common;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Items;
using SIE.Items.KzItemCategorys;
using SIE.MES.Fixture;
using SIE.MES.PrepareProducts;
using SIE.MES.PrepareProducts.Enums;
using SIE.MES.PrepareProducts.Services;
using SIE.MES.ProcessPrepareRecords;
using SIE.Tech.Processs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.ProcessProperty
{
    /// <summary>
    /// 工序属性控制器
    /// </summary>
    public class ProcessPtyController : DomainController
    {

        private const string msg = "第{0}项填写异常，该项没有填写结果，但已填写备注信息。请填写结果或清空备注信息";

        /// <summary>
        /// 根据工序Id获取工序属性
        /// </summary>
        /// <param name="processIds"></param>
        /// <returns></returns>
        public virtual EntityList<ProcessPtyDetail> GetProcessPtyDetailsByProcessId(List<double> processIds)
        {
            var list = Query<ProcessPtyDetail>().Where(p => processIds.Contains(p.ProcessPty.ProcessId)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            return list;
        }

        /// <summary>
        ///确认按钮
        /// </summary>
        /// <param name="prepareRecordDetails"></param>
        public virtual void ExecuteComfrim(EntityList<ProcessPrepareRecordDetail> prepareRecordDetails)
        {
            var newPrepareRecordDetailList = new EntityList<ProcessPrepareRecordDetail>();
            foreach (var projectInfo in prepareRecordDetails)
            {
                ProcessPrepareRecordDetail prepareRecordDetail = new ProcessPrepareRecordDetail();
                prepareRecordDetail.ProcessId = projectInfo.ProcessId;
                prepareRecordDetail.PrepareRecordId = projectInfo.PrepareRecordId;
                prepareRecordDetail.PrepareProjectId = projectInfo.PrepareProjectId;
                prepareRecordDetail.ProjectCode = projectInfo.ProjectCode;
                prepareRecordDetail.ProjectDesc = projectInfo.ProjectDesc;
                prepareRecordDetail.ProjectName = projectInfo.ProjectName;
                prepareRecordDetail.Remark = projectInfo.Remark;
                prepareRecordDetail.ConfirmerId = RT.IdentityId;
                prepareRecordDetail.Result = projectInfo.Result;
                prepareRecordDetail.ProjectType = projectInfo.ProjectType;
                newPrepareRecordDetailList.Add(prepareRecordDetail);
            }
            Comfrim(newPrepareRecordDetailList);
        }

        /// <summary>
        /// 确认
        /// </summary>
        /// <param name="prepareRecordDetails"></param>
        /// <exception cref="ValidationException"></exception>
        public virtual void Comfrim(EntityList<ProcessPrepareRecordDetail> prepareRecordDetails)
        {
            if (!prepareRecordDetails.Any())
            {
                throw new ValidationException("该工单产品或产品族未维护对应工序相关产品产前准备项目，请维护！".L10N());
            }
            var now = RF.Find<ProcessPrepareRecordDetail>().GetDbTime();
            var parentId = prepareRecordDetails.First().PrepareRecordId;
            var parent = RF.GetById<ProcessPrepareRecord>(parentId);
            if (parent != null && parent.PrepareState == PrepareRecordState.Confirm)
            {
                throw new ValidationException("该工单产前准备状态为已确认，操作失败!".L10N());
            }
            var coumnters = DB.Query<ProcessPrepareRecordDetail>().Where(m => m.PrepareRecordId == parentId).ToList();

            for (int i = 0; i < prepareRecordDetails.Count; i++)
            {
                var item = prepareRecordDetails[i];
                if (!item.Remark.IsNullOrEmpty() && !item.Result.HasValue)
                {
                    throw new ValidationException(msg.L10nFormat(i + 1));
                }
                if (item.Result.HasValue && item.Result == PrepareRecordDetailResult.Fail && item.Remark.IsNullOrEmpty())
                {
                    throw new ValidationException("结果为不通过时，备注必填".L10N());

                }
                item.PersistenceStatus = item.Counter > 0 ? PersistenceStatus.Modified : PersistenceStatus.New;

                item.Counter = coumnters.Any() ? coumnters.Max(m => m.Counter) + 1 : 1;
                item.ConfirmerId = RT.IdentityId;

                item.ConfirmTime = now;
            }
            var toSaveList = prepareRecordDetails.Where(m => m.Result == PrepareRecordDetailResult.Pass
            || m.Result == PrepareRecordDetailResult.Fail).AsEntityList();
            //校验是否所有项目都通过 是则更新主表数据为已确认

            using (var tran = DB.AutonomousTransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                if (toSaveList.Count(m => m.Result == PrepareRecordDetailResult.Pass) == prepareRecordDetails.Count)
                {
                    parent.PrepareState = PrepareRecordState.Confirm;
                    RF.Save(parent);
                }
                RF.Save(toSaveList);
                tran.Complete();
            }
        }

        /// <summary>
        /// 根据 工序名称获取工序属性
        /// </summary>
        /// <param name="processIds"></param>
        /// <returns></returns>
        public virtual EntityList<ProcessPty> GetProcessPtysByProcessNames(List<string> processNames)
        {
            var list = processNames.SplitContains(names =>
            {
                return Query<ProcessPty>().Where(p => names.Contains(p.Process.Name)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });

            return list;
        }

        /// <summary>
        /// 根据工序获取工序属性维护
        /// </summary>
        /// <param name="processIds"></param>
        /// <returns></returns>
        public virtual EntityList<ProcessPty> GetProcessPtysByProcessIds(List<double> processIds)
        {
            var list = processIds.SplitContains(ids =>
            {
                return Query<ProcessPty>().Where(p => ids.Contains(p.ProcessId)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
            return list;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="processIds"></param>
        /// <returns></returns>
        [ApiService]
        public virtual EntityList<ProcessPty> GetProcessPtysByProcessIds(List<double> processIds, double itemId)
        {
            //         return DB.Query<ProcessPty>("A").Join<KzItemCategory>("C", (x, y) => (x.KzItemCategoryId != null && y.Id != null && x.KzItemCategoryId == y.Id) || (x.KzItemCategoryId == null && y.Id == null))
            //         .Where(p => p.SQL<bool>(@" (CASE 
            // -- 优先：A.A=C.A1（非空）且存在B1=target_B1 → 精准匹配
            // WHEN A.Kz_Item_Category_Id IS NOT NULL 
            //      AND C.ID IS NOT NULL 
            //      AND A.Kz_Item_Category_Id = C.id
            //      AND EXISTS (SELECT 1 FROM KZ_ITEM_CATEGORY WHERE C.ID = A.Kz_Item_Category_Id AND C.ITEM_ID = " + itemId + @")
            // THEN (C.ITEM_ID = " + itemId + @" AND A.PROCESS_ID = " + processId + @")
            // -- 兜底：无优先匹配 → A.A为空且A.B=target_B
            // ELSE (A.Kz_Item_Category_Id IS NULL AND A.PROCESS_ID = " + processId + @")
            //END) = 1")).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            var list = processIds.SplitContains(ids =>
            {
                return Query<ProcessPty>().Where(p => ids.Contains(p.ProcessId)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
            return list;
        }

        /// <summary>
        /// 根据工序ID获取工序属性
        /// </summary>
        /// <param name="processId"></param>
        /// <returns></returns>
        public virtual ProcessPty GetProcessPtysByProcessId(double processId,double itemId)
        {
            var processPtys = GetProcessPtysByProcessIds(new List<double>() { processId }, itemId);
            var kzItemCategory = RT.Service.Resolve<KzItemCategorysController>().GetKzItemCategorieByItemId(itemId);
            var pps = new List<ProcessPty>();
            if (kzItemCategory != null)
            {
                pps = processPtys.Where(p => p.KzCategoryId == kzItemCategory.KzCategoryId).ToList();
            }
            ////当找得到分类得时候，优先找到分类的，然后再找工序的
            if (pps.Count == 0)
                pps = processPtys.Where(p => p.KzCategoryId == null).ToList();

            return pps.FirstOrDefault();
            //Query<ProcessPty>().Where(p => p.ProcessId == processId).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 查询工序属性维护
        /// </summary>
        /// <param name="criterial"></param>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        public virtual EntityList<ProcessPty> CriterialProcessPty(ProcessPtyCriterial criterial)
        {
            if (criterial == null)
            {
                throw new ValidationException("工序属性查询实体异常！".L10N());
            }
            var q = Query<ProcessPty>();
            if (criterial.ProcessId.HasValue)
            {
                q.Where(p => p.ProcessId == criterial.ProcessId);
            }
            if (criterial.Scheduling.HasValue)
            {
                if (criterial.Scheduling == ProcessState.s)
                {
                    q.Where(p => p.Scheduling == true);
                }
                else if (criterial.Scheduling == ProcessState.f)
                {
                    q.Where(p => p.Scheduling == false);
                }
            }

            if (criterial.IsPrepare.HasValue)
            {
                if (criterial.IsPrepare == ProcessState.s)
                {
                    q.Where(p => p.IsPrepare == true);
                }
                else if (criterial.IsPrepare == ProcessState.f)
                {
                    q.Where(p => p.IsPrepare == false);
                }
            }

            if (criterial.DispatchWork.HasValue)
            {
                if (criterial.DispatchWork == ProcessState.s)
                {
                    q.Where(p => p.DispatchWork == true);
                }
                else if (criterial.DispatchWork == ProcessState.f)
                {
                    q.Where(p => p.DispatchWork == false);
                }
            }

            if (criterial.IsTransfer.HasValue)
            {
                if (criterial.IsTransfer == ProcessState.s)
                {
                    q.Where(p => p.IsTransfer == true);
                }
                else if (criterial.IsTransfer == ProcessState.f)
                {
                    q.Where(p => p.IsTransfer == false);
                }
            }
            
            if (!criterial.ProductLine.IsNullOrEmpty())
            {
                q.Where(m => m.ProductLine.Contains("%" + criterial.ProductLine + "%"));
            }

            if (!criterial.ProductType.IsNullOrEmpty())
            {
                q.Where(m => m.ProductType.Contains("%" + criterial.ProductType + "%"));
            }
            if (!criterial.ProcessCode.IsNullOrEmpty())
            {
                q.Where(m => m.Process.Code.Contains("%" + criterial.ProcessCode + "%"));
            }
            if (criterial.Type.HasValue)
                q.Where(p => p.Type == criterial.Type);

            return q.OrderBy(criterial.OrderInfoList).ToList(criterial.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 是否有相同的数据
        /// </summary>
        /// <param name="productLine"></param>
        /// <param name="itemType"></param>
        /// <param name="productType"></param>
        /// <param name="process"></param>
        /// <returns></returns>
        public virtual bool GetProcessPtyBool(string productLine, ItemType itemType,string productType,double process)
        {
            var q=Query<ProcessPty>().Where(p=>p.ProductLine== productLine&&p.ProductType== productType&&p.ProcessId== process&&p.Type== itemType).ToList();
            if (q.Count > 0)
                return true;
            else
                return false;
        }

        public virtual double GetProcessId(string code)
        {
            var q = Query<Process>().Where(p => p.Code==code).ToList();
            if (q.Count > 0)
            {
                return q.FirstOrDefault().Id;
            }
            return 0;
        }


        /// <summary>
        /// 是否转入的工序
        /// </summary>
        /// <param name="processId"></param>

        /// <returns></returns>
        public virtual bool GetIsTransferProcessPty(double processId,double itemId)
        {
            var processPtys = GetProcessPtysByProcessIds(new List<double>() { processId }, itemId);
            var kzItemCategory = RT.Service.Resolve<KzItemCategorysController>().GetKzItemCategorieByItemId(itemId);
            var pps = new List<ProcessPty>();
            if (kzItemCategory != null)
            {
                pps = processPtys.Where(p => p.KzCategoryId == kzItemCategory.KzCategoryId).ToList();
            }
            ////当找得到分类得时候，优先找到分类的，然后再找工序的
            if (pps.Count == 0)
                pps = processPtys.Where(p => p.KzCategoryId == null).ToList();

            var processPty = pps.FirstOrDefault();
            //var processPty = Query<ProcessPty>().Where(p => p.ProcessId == processId && p.IsTransfer == true).FirstOrDefault();
            if (processPty?.IsTransfer == true)
                return true;
            return false;
        }

        /// <summary>
        /// 获取所有工序
        /// </summary>
        /// <returns></returns>
        public virtual EntityList<Process> GetProcess()
        {
            return Query<Process>().ToList();
        }


        /// <summary>
        /// 获取排程点工序
        /// </summary>
        /// <returns></returns>
        public virtual EntityList<Process> GetProcess(string keynowe, PagingInfo pagingInfo)
        {
            return Query<Process>().LeftJoin<ProcessPty>((x,y)=>x.Id == y.ProcessId)
                .Where<ProcessPty>((x, y)=>y.Scheduling)
                .WhereIf(!keynowe.IsNullOrEmpty(),p=>p.Code.Contains(keynowe)||p.Name.Contains(keynowe))
                .ToList(pagingInfo);
        }

        /// <summary>
        /// 获取排程点工序
        /// </summary>
        /// <returns></returns>
        public virtual ProcessPty GetProcessPtyByProcessId(double processId)
        {
            using (SIE.DataAuth.DataAuths.LoadAll())
            {
                return Query<ProcessPty>().Where(p => p.ProcessId == processId && p.Scheduling == true).FirstOrDefault();
            }
        }

        public virtual List<double> GetProcessIds()
        {
            using (SIE.DataAuth.DataAuths.LoadAll())
            {
                return Query<ProcessPty>().Where(p => p.Scheduling == true).Select(p=>p.ProcessId).ToList<double>().ToList();
            }
        }
    }
}

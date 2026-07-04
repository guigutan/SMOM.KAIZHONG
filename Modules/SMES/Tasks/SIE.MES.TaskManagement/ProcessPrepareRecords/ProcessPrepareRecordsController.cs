using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.Scripting.Utils;
using SIE.Common;
using SIE.Common.Configs;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Items.KzItemCategorys;
using SIE.MES.PrepareProducts;
using SIE.MES.PrepareProducts.Enums;
using SIE.MES.ProcessProperty;
using SIE.MES.TaskManagement.Configs;
using SIE.MES.TaskManagement.Dispatchs;
using SIE.MES.TaskManagement.ProcessPrepareRecords.Datas;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.MES.TaskManagement.ProcessPrepareRecords
{
    public partial class ProcessPrepareRecordsController : DomainController
    {
        /// <summary>
        /// 根据派工任务单删除对应的工序产前准备记录
        /// </summary>
        /// <param name="dispatchTaskIds"></param>
        public virtual void DeleteProcessPrepareRecordByDispatchTaskIds(List<double> dispatchTaskIds)
        {
            var processPrepareRecords = GetProcessPrepareRecordsByDispatchTaskIds(dispatchTaskIds);

            foreach (var processPrepareRecord in processPrepareRecords)
            {
                //改为删除状态
                processPrepareRecord.PersistenceStatus = PersistenceStatus.Deleted;
                //将明细都改为删除状态
                processPrepareRecord.PrepareRecordDetail.ForEach(p => p.PersistenceStatus = PersistenceStatus.Deleted);
            }

            if (processPrepareRecords.Count > 0)
                RF.Save(processPrepareRecords);
        }

        /// <summary>
        /// 根据派工任务单创建工序产前准备记录
        /// </summary>
        public virtual void CreateProcessPrepareRecord(List<double> dispatchTaskIds)
        {
            var dispatchTasks = RT.Service.Resolve<DispatchController>().GetDispatchTasks(dispatchTaskIds);

            EntityList<ProcessPrepareRecord> processPrepareRecords = new EntityList<ProcessPrepareRecord>();

            var processIds = dispatchTasks.Where(p => p.ProcessId != null).Select(p => p.ProcessId.Value).Distinct().ToList();

            foreach (var dispatchTask in dispatchTasks)
            {
                var processPtys = RT.Service.Resolve<ProcessPtyController>().GetProcessPtysByProcessIds(new List<double>() { dispatchTask.ProcessId.Value }, dispatchTask.ProductId);
                var kzItemCategory = RT.Service.Resolve<KzItemCategorysController>().GetKzItemCategorieByItemId(dispatchTask.ProductId);
                var pps = new List<ProcessPty>();
                if (kzItemCategory != null)
                {
                    pps = processPtys.Where(p => p.KzCategoryId == kzItemCategory.KzCategoryId).ToList();
                }
                ////当找得到分类得时候，优先找到分类的，然后再找工序的
                if (pps.Count == 0)
                    pps = processPtys.Where(p => p.KzCategoryId == null).ToList();

                //当没有在维护工序属性里面，或者没有勾上产前准备的，就不给它生成工序产前准备记录
                if (pps.All(p => p.ProcessId != dispatchTask.ProcessId) || pps.Any(p => p.ProcessId == dispatchTask.ProcessId && (p.IsPrepare == null || p.IsPrepare == false)))
                    continue;
                //创建工序产前准备记录
                ProcessPrepareRecord processPrepareRecord = new ProcessPrepareRecord();
                processPrepareRecord.DispatchTaskId = dispatchTask.Id;
                processPrepareRecord.DispatchTask = dispatchTask;
                processPrepareRecord.PrepareState = PrepareRecordState.ToConfirm;
                processPrepareRecord.PersistenceStatus = PersistenceStatus.New;

                processPrepareRecords.Add(processPrepareRecord);
            }

            if (processPrepareRecords.Count > 0)
                RF.Save(processPrepareRecords);
        }

        /// <summary>
        /// 根据派工任务单，获取工序产前准备记录
        /// </summary>
        /// <param name="dispatchTaskIds"></param>
        /// <returns></returns>
        public virtual EntityList<ProcessPrepareRecord> GetProcessPrepareRecordsByDispatchTaskIds(List<double> dispatchTaskIds)
        {
            var list = Query<ProcessPrepareRecord>().Where(p => dispatchTaskIds.Contains((double)p.DispatchTaskId)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            return list;
        }

        /// <summary>
        /// 工序产前准备记录执行确认
        /// </summary>
        public virtual void PPRExecuteComfrim(List<ProcessPrepareRecordDetail> details)
        {
            if (details.Any(p => p.Result == null))
                throw new ValidationException("存在数据未填写的结果".L10N());

            List<SubmitPprListDetailInfo> infos = new List<SubmitPprListDetailInfo>();

            foreach (var detail in details)
            {
                SubmitPprListDetailInfo info = new SubmitPprListDetailInfo();

                info.Id = 0;
                info.ProId = detail.PrepareProjectId ?? 0;
                info.Result = detail.Result == PrepareRecordDetailResult.Pass ? true : false;
                info.Remark = detail.Remark;
                infos.Add(info);
            }
            //提交
            SubmitPprListDetailInfos(details.FirstOrDefault().PrepareRecordId, RT.IdentityId, infos);
        }

        /// <summary>
        /// 创建产品准备记录
        /// </summary>
        /// <param name="prepareRecordId"></param>
        /// <returns></returns>
        public virtual EntityList<ProcessPrepareRecordDetail> CreateProcessPrepareRecordDetail(double prepareRecordId)
        {
            var prepareRecord = RF.GetById<ProcessPrepareRecord>(prepareRecordId, new EagerLoadOptions().LoadWithViewProperty());
            if (prepareRecord == null)
            {
                throw new ValidationException("系统找不到相关产前准备信息".L10N());
            }
            if (prepareRecord.PrepareState != PrepareRecordState.ToConfirm)
            {
                throw new ValidationException("产前准备状态不为待确认，执行失败".L10N());
            }
            //1.获取已有的记录数据
            EntityList<ProcessPrepareRecordDetail> prepareRecordDetails = Query<ProcessPrepareRecordDetail>().Where(p => p.PrepareRecordId == prepareRecord.Id).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            var displayPrepareRecordDetails = new EntityList<ProcessPrepareRecordDetail>();
            displayPrepareRecordDetails.AddRange(prepareRecordDetails);

            var infos = GetPprListDetailInfos(prepareRecordId);

            //获取项目
            var proIds = infos.Select(p => p.ProId).Distinct().ToList();
            var pros = Query<PrepareProject>().Where(p => proIds.Contains(p.Id)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());

            foreach (var info in infos)
            {
                var item = pros.FirstOrDefault(p => p.Id == info.ProId);
                if (prepareRecordDetails.Any(p => p.PrepareProjectId == item.Id))
                    continue;

                var prepareRecordDetailItem = new ProcessPrepareRecordDetail()
                {
                    ProcessId = prepareRecord.DispatchTask.ProcessId,
                    ProcessName = prepareRecord.DispatchTask.Process.Name,
                    PrepareProjectId = item.Id,
                    ProjectType = item.ProType,
                    ProjectCode = item.ProCode,
                    ProjectName = item.ProName,
                    ProjectDesc = item.ProDesc,
                    //Result = info.Result == true ? PrepareProducts.Enums.PrepareRecordDetailResult.Pass : PrepareProducts.Enums.PrepareRecordDetailResult.Fail,
                    //Remark = info.Remark,
                    PrepareRecordId = prepareRecord.Id,
                    PersistenceStatus = PersistenceStatus.New,
                    ConfirmTime = DateTime.Now,
                    //ConfirmerId = empId
                };
                prepareRecordDetailItem.ExtValues["ProcessId_Display"] = prepareRecord.DispatchTask?.Process?.Name;
                //details.Add(prepareRecordDetailItem);
                displayPrepareRecordDetails.Add(prepareRecordDetailItem);
                prepareRecordDetails.Add(prepareRecordDetailItem);
            }

            #region 旧逻辑

            //EntityList<PrepareProductDetail> prepareProductDetails = GetProductOrFamilyDetails(prepareRecord);
            //1.获取已有的记录数据
            //EntityList<ProcessPrepareRecordDetail> prepareRecordDetails = GetHadRecord(prepareRecord);
            //取工单的工序集合
            //var processList = prepareRecord.DispatchTask.WorkOrder.RoutingProcessList;

            //foreach (var item in prepareProductDetails)
            //{
            //    var objs = prepareRecordDetails.Where(p => p.PrepareProjectId == item.PrepareProjectId);
            //    if (objs.Any() && objs.Any(obj => obj.Result == PrepareRecordDetailResult.Pass))//已经生成过
            //    {
            //        continue;
            //    }

            //    //工序有值，且工单工序中不包含此工序则不生成
            //    if (item.ProcessId.HasValue && !processList.Any(m => m.ProcessId == item.ProcessId.Value))
            //    {
            //        continue;
            //    }
            //    //已生成过相同工序的不再生成且是通过的
            //    if (prepareRecordDetails.Any(m => m.ProcessId == item.ProcessId && item.ProcessId.HasValue &&
            //    m.PrepareProjectId == item.PrepareProjectId && m.Result == PrepareRecordDetailResult.Pass))
            //    {
            //        continue;
            //    }
            //    else
            //    {//失败的仍需生产

            //        var prepareRecordDetailItem = new ProcessPrepareRecordDetail()
            //        {
            //            ProcessId = item.ProcessId,
            //            ProcessName = item.ProcessName,
            //            PrepareProjectId = item.PrepareProjectId,
            //            ProjectType = item.PrepareProjectType,
            //            ProjectCode = item.PrepareProjectCode,
            //            ProjectName = item.PrepareProjectName,
            //            ProjectDesc = item.PrepareProjectDesc,
            //            PrepareRecordId = prepareRecord.Id
            //        };
            //        prepareRecordDetailItem.ExtValues["ProcessId_Display"] = item.ProcessName;
            //        if (displayPrepareRecordDetails.FindIndex(m => m.ProcessId == prepareRecordDetailItem.ProcessId &&
            //        m.PrepareProjectId == prepareRecordDetailItem.PrepareProjectId
            //        ) >= 0)//存在相同的不再加载
            //        {
            //            continue;
            //        }
            //        displayPrepareRecordDetails.Add(prepareRecordDetailItem);
            //        prepareRecordDetails.Add(prepareRecordDetailItem);

            //    }

            //}

            #endregion

            return displayPrepareRecordDetails.AsEntityList();

        }

        /// <summary>
        /// 获取产品或产品族的已设置的项目明细
        /// </summary>
        /// <param name="prepareRecord"></param>
        /// <returns></returns>
        public virtual EntityList<PrepareProductDetail> GetProductOrFamilyDetails(ProcessPrepareRecord prepareRecord)
        {
            var resultList = DB.Query<PrepareProductDetail>().Join<PrepareProduct>((x, y) => x.PrepareProductId == y.Id).
                    Where<PrepareProduct>((p, q) => (q.ProductId != null && q.ProductId == prepareRecord.DispatchTask.WorkOrder.ProductId))
                    .OrderBy(m => m.PrepareProjectType).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            if (resultList.Any())
            {
                return resultList;
            }
            else
            {
                return DB.Query<PrepareProductDetail>().Join<PrepareProduct>((x, y) => x.PrepareProductId == y.Id).
                Where<PrepareProduct>((p, q) => (q.ProductFamilyId != null && q.ProductFamilyId == prepareRecord.DispatchTask.WorkOrder.ProductFamilyId))
                .OrderBy(m => m.PrepareProjectType).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            }
        }

        private EntityList<ProcessPrepareRecordDetail> GetHadRecord(ProcessPrepareRecord prepareRecord)
        {
            var detail = GetHadRecord(prepareRecord);
            EntityList<ProcessPrepareRecordDetail> recordDetail = new EntityList<ProcessPrepareRecordDetail>();
            var groupDetail = detail.GroupBy(p => new { p.ProcessId, p.PrepareProjectId }).ToList();
            foreach (var item in groupDetail)
            {
                var maxCounter = item.OrderByDescending(p => p.Counter).FirstOrDefault();
                recordDetail.Add(maxCounter);
            }
            return recordDetail;
        }

        /// <summary>
        /// 根据主表id获取明细子表
        /// </summary>
        /// <param name="preRecordId"></param>
        /// <returns></returns>
        public virtual EntityList<ProcessPrepareRecordDetail> GetPrepareRecordDetailList(double preRecordId)
        {
            return DB.Query<ProcessPrepareRecordDetail>().Where(p => p.PrepareRecordId == preRecordId).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 查询产前准备记录
        /// </summary>
        /// <param name="prepareRecordCriteria"></param>
        /// <returns></returns>
        public virtual EntityList<ProcessPrepareRecord> QueryPrepareRecordList(ProcessPrepareRecordCriteria prepareRecordCriteria)
        {
            var query = Query<ProcessPrepareRecord>();
            if (prepareRecordCriteria == null)
            {
                throw new ValidationException("产前准备记录查询实体异常！".L10N());
            }
            if (prepareRecordCriteria.No.IsNotEmpty())
            {
                query.Where(p => p.DispatchTask.WorkOrder.No.Contains(prepareRecordCriteria.No));
            }
            if (prepareRecordCriteria.FactoryId != 0 && prepareRecordCriteria.FactoryId != null)
            {
                query.Where(p => p.DispatchTask.WorkOrder.FactoryId == prepareRecordCriteria.FactoryId);
            }
            if (prepareRecordCriteria.WorkShopId != 0 && prepareRecordCriteria.WorkShopId != null)
            {
                query.Where(p => p.DispatchTask.WorkOrder.WorkShopId == prepareRecordCriteria.WorkShopId);
            }
            if (prepareRecordCriteria.ResourceId != 0 && prepareRecordCriteria.ResourceId != null)
            {
                query.Where(p => p.DispatchTask.WorkOrder.ResourceId == prepareRecordCriteria.ResourceId);
            }
            if (prepareRecordCriteria.ProductName.IsNotEmpty())
            {
                query.Where(p => p.DispatchTask.WorkOrder.Product.Name.Contains(prepareRecordCriteria.ProductName));
            }
            if (prepareRecordCriteria.State.IsNotEmpty())
            {
                var criteriaState = new List<int>();
                prepareRecordCriteria.State.Split(',').ForEach(state =>
                {
                    criteriaState.Add(int.Parse(state));
                });
                query.Where(p => criteriaState.Contains((int)p.DispatchTask.WorkOrder.State));
            }
            if (prepareRecordCriteria.PreState.HasValue)
            {
                query.Where(p => p.PrepareState == prepareRecordCriteria.PreState);
            }
            if (prepareRecordCriteria.PlanBeginTime.BeginValue.HasValue)
            {
                query.Where(p => p.DispatchTask.WorkOrder.PlanBeginDate >= prepareRecordCriteria.PlanBeginTime.BeginValue);
            }
            if (prepareRecordCriteria.PlanBeginTime.EndValue.HasValue)
            {
                query.Where(p => p.DispatchTask.WorkOrder.PlanBeginDate <= prepareRecordCriteria.PlanBeginTime.EndValue);
            }
            if (prepareRecordCriteria.ConfirmTime.BeginValue.HasValue || prepareRecordCriteria.ConfirmTime.EndValue.HasValue)
            {
                query.Exists<ProcessPrepareRecordDetail>((x, y) => y.WhereIf(prepareRecordCriteria.ConfirmTime.BeginValue.HasValue, p => p.ConfirmTime >= prepareRecordCriteria.ConfirmTime.BeginValue)
                .WhereIf(prepareRecordCriteria.ConfirmTime.EndValue.HasValue, p => p.ConfirmTime <= prepareRecordCriteria.ConfirmTime.EndValue)
                .Where(p => p.PrepareRecordId == x.Id));
            }

            if (prepareRecordCriteria.DispatchTaskNo.IsNotEmpty())
            {
                query.Where(p => p.DispatchTask.No.Contains(prepareRecordCriteria.DispatchTaskNo));
            }

            if (prepareRecordCriteria.TaskStatus.IsNotEmpty())
            {
                var criteriaState = new List<int>();
                prepareRecordCriteria.TaskStatus.Split(',').ForEach(state =>
                {
                    criteriaState.Add(int.Parse(state));
                });
                query.Where(p => criteriaState.Contains((int)p.DispatchTask.TaskStatus));
            }
            if (prepareRecordCriteria.ProcessName.IsNotEmpty())
            {
                query.Where(p => p.DispatchTask.Process.Name.Contains(prepareRecordCriteria.ProcessName));
            }
            if (!prepareRecordCriteria.TaskPerformer.IsNullOrEmpty())
            {
                query.Where(p => p.DispatchTask.TaskPerformer.Contains(prepareRecordCriteria.TaskPerformer));
            }
            return query.OrderBy(prepareRecordCriteria.OrderInfoList).ToList(prepareRecordCriteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        public virtual ProcessPrepareRecord GetProcessPrepareRecord(double id)
        {
            return Query<ProcessPrepareRecord>().Where(p => p.Id == id).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 校验任务单工序产前准备
        /// </summary>
        /// <param name="task"></param>
        public virtual void ValidateProcessPrepare(DispatchTask task)
        {
            if (task == null) return;
            var config = ConfigService.GetConfig(new ProcessPrepareRecordConfig(), typeof(ProcessPrepareRecord));
            if (config.IsValidateProcessPrepare != true)
                return;
            var record = Query<ProcessPrepareRecord>().Where(p => p.DispatchTaskId == task.Id).FirstOrDefault();
            if (record == null)
                return;
            if (record.PrepareState == PrepareRecordState.ToConfirm)
                throw new ValidationException("任务单[{0}]工序产前准备未完成,请检查".L10nFormat(task?.No));
        }
    }
}

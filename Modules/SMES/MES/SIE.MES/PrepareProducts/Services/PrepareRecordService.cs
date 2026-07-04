using Microsoft.Scripting.Utils;
using NPOI.OpenXmlFormats.Spreadsheet;
using SIE.Api;
using SIE.Common;
using SIE.Core.Common.Service;
using SIE.Core.WorkOrders;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MES.PrepareProducts.ApiModels;
using SIE.MES.PrepareProducts.Daos;
using SIE.MES.PrepareProducts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.MES.PrepareProducts.Services
{
    /// <summary>
    /// 产前准备记录服务
    /// </summary>
    public class PrepareRecordService : DomainService
    {
        private readonly PrepareRecordDao _prepareRecordDao;
        private const string msg = "第{0}项填写异常，该项没有填写结果，但已填写备注信息。请填写结果或清空备注信息";
        /// <summary>
        /// 产前准备记录
        /// </summary>
        /// <param name="prepareRecordDao"></param>
        public PrepareRecordService(PrepareRecordDao prepareRecordDao)
        {
            _prepareRecordDao = prepareRecordDao;
        }

        /// <summary>
        /// 查询产前准备记录
        /// </summary>
        /// <param name="prepareRecordCriteria"></param>
        /// <returns></returns>
        public virtual EntityList<PrepareRecord> QueryPrepareRecordList(PrepareRecordCriteria prepareRecordCriteria)
        {
            return _prepareRecordDao.QueryPrepareRecordList(prepareRecordCriteria);
        }

        /// <summary>
        /// 根据主表id获取明细子表
        /// </summary>
        /// <param name="preRecordId"></param>
        /// <returns></returns>
        public virtual EntityList<PrepareRecordDetail> GetPrepareRecordDetailList(double preRecordId)
        {
            return _prepareRecordDao.GetPrepareRecordDetailList(preRecordId);
        }

        /// <summary>
        /// 创建产品准备记录
        /// </summary>
        /// <param name="prepareRecordId"></param>
        /// <returns></returns>
        public virtual EntityList<PrepareRecordDetail> CreatePrepareRecordDetail(double prepareRecordId)
        {
            var prepareRecord = RF.GetById<PrepareRecord>(prepareRecordId, new EagerLoadOptions().LoadWithViewProperty());
            if (prepareRecord == null)
            {
                throw new ValidationException("系统找不到相关产前准备信息".L10N());
            }
            if (prepareRecord.PrepareState != Enums.PrepareRecordState.ToConfirm)
            {
                throw new ValidationException("产前准备状态不为待确认，执行失败".L10N());
            }
            //1.获取已有的记录数据
            EntityList<PrepareRecordDetail> prepareRecordDetails = GetHadRecord(prepareRecord);
            EntityList<PrepareProductDetail> prepareProductDetails = _prepareRecordDao.GetProductOrFamilyDetails(prepareRecord);

            //取工单的工序集合
            var processList = prepareRecord.RoutingProcessList;
            var displayPrepareRecordDetails =new EntityList<PrepareRecordDetail>();
            displayPrepareRecordDetails.AddRange(prepareRecordDetails.Where(p => p.Result == PrepareRecordDetailResult.Fail).ToList());
            foreach (var item in prepareProductDetails)
            {
                var objs = prepareRecordDetails.Where(p => p.PrepareProjectId == item.PrepareProjectId);
                if (objs.Any()&& objs.Any(obj => obj.Result == PrepareRecordDetailResult.Pass))//已经生成过
                {
                    continue;
                }

                //工序有值，且工单工序中不包含此工序则不生成
                if (item.ProcessId.HasValue && !processList.Any(m => m.ProcessId == item.ProcessId.Value))
                {
                    continue;
                }
                //已生成过相同工序的不再生成且是通过的
                if (prepareRecordDetails.Any(m => m.ProcessId == item.ProcessId && item.ProcessId.HasValue &&
                m.PrepareProjectId==item.PrepareProjectId&&m.Result == PrepareRecordDetailResult.Pass))
                {
                    continue;
                }
                else
                {//失败的仍需生产

                    var prepareRecordDetailItem = new PrepareRecordDetail()
                    {
                        ProcessId = item.ProcessId,
                        ProcessName= item.ProcessName,
                        PrepareProjectId = item.PrepareProjectId,
                        ProjectType = item.PrepareProjectType,
                        ProjectCode = item.PrepareProjectCode,
                        ProjectName = item.PrepareProjectName,
                        ProjectDesc = item.PrepareProjectDesc,
                        PrepareRecordId = prepareRecord.Id
                    };
                    prepareRecordDetailItem.ExtValues["ProcessId_Display"] = item.ProcessName;
                    if (displayPrepareRecordDetails.FindIndex(m => m.ProcessId == prepareRecordDetailItem.ProcessId &&
                    m.PrepareProjectId == prepareRecordDetailItem.PrepareProjectId
                    ) >= 0)//存在相同的不再加载
                    {
                        continue;
                    }
                    displayPrepareRecordDetails.Add(prepareRecordDetailItem);
                    prepareRecordDetails.Add(prepareRecordDetailItem);
                    
                }

            }
            return displayPrepareRecordDetails.AsEntityList();
        }


        private EntityList<PrepareRecordDetail> GetHadRecord(PrepareRecord prepareRecord)
        {
            var detail = _prepareRecordDao.GetHadRecord(prepareRecord);
            EntityList< PrepareRecordDetail> recordDetail = new EntityList< PrepareRecordDetail>();
            var groupDetail = detail.GroupBy(p => new { p.ProcessId, p.PrepareProjectId}).ToList();
            foreach ( var item in groupDetail )
            {
                var maxCounter = item.OrderByDescending(p => p.Counter).FirstOrDefault();
                recordDetail.Add(maxCounter);
            }
            return recordDetail;
        }

        /// <summary>
        /// 确认
        /// </summary>
        /// <param name="prepareRecordDetails"></param>
        /// <exception cref="ValidationException"></exception>
        public virtual void Comfrim(EntityList<PrepareRecordDetail> prepareRecordDetails)
        {
            if (!prepareRecordDetails.Any())
            {
                throw new ValidationException("该工单产品或产品族未维护对应工序相关产品产前准备项目，请维护！".L10N());
            }
            var now = RF.Find<PrepareRecordDetail>().GetDbTime();
            var parentId = prepareRecordDetails.First().PrepareRecordId;
            var parent = RF.GetById<PrepareRecord>(parentId);
            if(parent != null&& parent.PrepareState== PrepareRecordState.Confirm) {
                throw new ValidationException("该工单产前准备状态为已确认，操作失败!".L10N());
            }
            var coumnters = DB.Query<PrepareRecordDetail>().Where(m => m.PrepareRecordId == parentId).ToList();

            for (int i = 0; i < prepareRecordDetails.Count; i++)
            {
                var item = prepareRecordDetails[i];
                if (!item.Remark.IsNullOrEmpty() && !item.Result.HasValue)
                {
                    throw new ValidationException(msg.L10nFormat(i + 1));
                }
                if (item.Result.HasValue && item.Result == PrepareRecordDetailResult.Fail&&item.Remark.IsNullOrEmpty())
                {
                    throw new ValidationException("结果为不通过时，备注必填".L10N());

                }
                item.PersistenceStatus = item.Counter > 0 ? PersistenceStatus.Modified : PersistenceStatus.New;

                item.Counter= coumnters.Any()? coumnters.Max(m=>m.Counter)+1:1;
                item.ConfirmerId = RT.IdentityId;

                item.ConfirmTime = now;
            }
            var toSaveList = prepareRecordDetails.Where(m => m.Result == Enums.PrepareRecordDetailResult.Pass
            || m.Result == Enums.PrepareRecordDetailResult.Fail).AsEntityList();
            //校验是否所有项目都通过 是则更新主表数据为已确认

            using (var tran = DB.AutonomousTransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                if (toSaveList.Count(m => m.Result == PrepareRecordDetailResult.Pass) == prepareRecordDetails.Count)
                {
                    parent.PrepareState = Enums.PrepareRecordState.Confirm;
                    RF.Save(parent);
                }
                RF.Save(toSaveList);
                tran.Complete();
            }
        }


        /// <summary>
        /// 获取单据
        /// </summary>
        /// <param name="prepareProductsFilter"></param>
        /// <returns></returns>
        public virtual List<PrepareProductsBill> GetBills(PrepareProductsFilter prepareProductsFilter)
        {
            List<PrepareProductsBill> prepareProductsBills = new List<PrepareProductsBill>();
            if (prepareProductsFilter == null)
            {
                var bills = DB.Query<PrepareRecord>().Where(m => m.PrepareState == Enums.PrepareRecordState.ToConfirm
                && m.PlanBeginDate == DateTime.Now.Date
                ).OrderBy(m =>new { m.PrepareState,m.PlanBeginDate}).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
                GetItemToBills(prepareProductsBills, bills);
                return prepareProductsBills;
            }
            else
            {
                var query = DB.Query<PrepareRecord>();
                if (prepareProductsFilter.State.HasValue)
                {
                    if ((PrepareRecordState)prepareProductsFilter.State.Value != PrepareRecordState.All)//加载全部则不加该条件
                    {
                        query.Where(m => m.PrepareState == (PrepareRecordState)prepareProductsFilter.State.Value);
                    }
                }
                if (prepareProductsFilter.ResourceId.HasValue)
                {
                    query.Where(m => m.ResourceId == prepareProductsFilter.ResourceId);
                }
                if (prepareProductsFilter.FactoryId.HasValue)
                {
                    query.Where(m => m.FactoryId == prepareProductsFilter.FactoryId);
                }
                if (!prepareProductsFilter.WoNo.IsNullOrEmpty())
                {
                    query.Where(m => m.No.Contains(prepareProductsFilter.WoNo));
                }
                if (prepareProductsFilter.WorkShopId.HasValue)
                {
                    query.Where(m => m.WorkShopId == prepareProductsFilter.WorkShopId);
                }
                if (prepareProductsFilter.WoState.HasValue && prepareProductsFilter.WoState != 2)//2为全部 不加条件则为全部
                {
                    query.Where(m => m.State == (WorkOrderState)prepareProductsFilter.WoState);
                }

                if (prepareProductsFilter.PlanBeginTime.HasValue)
                {
                    DateTime endDate = DateTime.Now;
                    var dateBegin = GetDate(prepareProductsFilter.PlanBeginTime.Value, out endDate);
                    query.Where(m => m.PlanBeginDate >= dateBegin && m.PlanBeginDate <= endDate);
                }
                var bills = query.OrderBy(m=> new { m.PrepareState, m.PlanBeginDate }).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
                GetItemToBills(prepareProductsBills, bills);
                return prepareProductsBills;
            }

        }

        /// <summary>
        /// 获取时间
        /// </summary>
        /// <param name="date"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        private DateTime GetDate(int date, out DateTime endDate)
        {
            var datatimeNow = RF.Find<PrepareRecord>().GetDbTime();
            endDate = datatimeNow.Date;
            var beginTime = datatimeNow.Date;
            switch (date)
            {
                case 0://今天
                    break;
                case 1://未来3天
                    beginTime = datatimeNow.Date;
                    endDate = beginTime.AddDays(3);
                    break;
                case 2://最近一周
                    beginTime = datatimeNow.Date.AddDays(-7);
                    endDate = datatimeNow.Date.AddHours(-12);
                    break;
                case 3://本月
                    beginTime = new DateTime(datatimeNow.Date.Year, datatimeNow.Date.Month, 1, 0, 0, 0);
                    endDate = beginTime.AddMonths(1).AddDays(-1);
                    break;
                default: break;
            }
            return beginTime;

        }

        /// <summary>
        /// 转换为API对象
        /// </summary>
        /// <param name="prepareProductsBills"></param>
        /// <param name="bills"></param>
        private void GetItemToBills(List<PrepareProductsBill> prepareProductsBills, EntityList<PrepareRecord> bills)
        {
            foreach (var item in bills)
            {
                PrepareProductsBill prepareProductsBill = new PrepareProductsBill();
                prepareProductsBill.WONo = item.No;
                prepareProductsBill.BillId = item.Id;
                prepareProductsBill.ProduceId = item.ProductId;
                prepareProductsBill.ProduceName = item.ProductName;
                prepareProductsBill.ResourceName = item.ResourceName;
                prepareProductsBill.FactoryName = item.FactoryName;
                prepareProductsBill.State = (int)item.PrepareState;
                prepareProductsBill.StateDisplay = item.PrepareState.ToLabel().L10N();
                prepareProductsBill.Factory = item.FactoryId.HasValue ? item.FactoryId.Value : 0;
                prepareProductsBill.PlanBeginTime = item.PlanBeginDate.ToString("yyyy-MM-dd HH:mm:ss");
                prepareProductsBills.Add(prepareProductsBill);
            }
        }

        /// <summary>
        /// 获取产前准备明细
        /// </summary>
        /// <param name="billId"></param>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        public virtual ProjectCheckInfo GetProjectCheckInfo(double billId)
        {
            ProjectCheckInfo projectCheckInfo = new ProjectCheckInfo();
            var bill = RF.GetById<PrepareRecord>(billId, new EagerLoadOptions().LoadWithViewProperty());
            if (bill == null)
                throw new ValidationException("系统不存在单据！".L10N());
            var billDetail = CreatePrepareRecordDetail(billId);
            List<PrepareProductsBill> prepareProductsBills = new List<PrepareProductsBill>();
            GetItemToBills(prepareProductsBills, new EntityList<PrepareRecord> { bill });
            projectCheckInfo.PrepareProductsBill = prepareProductsBills.First();
            foreach (var item in billDetail)
            {
                ProjectInfo projectInfo = new ProjectInfo();
                projectInfo.ProjectId = item.PrepareProjectId;
                projectInfo.RecordDetailId = item.Id;
                projectInfo.Remark = item.Remark;
                projectInfo.Result = item.Result == null ? -1 : (int)item.Result;
                projectInfo.Desc = item.ProjectDesc;
                projectInfo.ProcessId = item.ProcessId;
                projectInfo.ProcessName = item.ProcessName;
                projectInfo.ProjectName = item.ProjectName;
                projectInfo.ProjectCode = item.ProjectCode;
                projectInfo.PrepareRecordId = item.PrepareRecordId;
                projectInfo.ProjectType = item.ProjectType.HasValue ? (int)item.ProjectType : -1;
                projectCheckInfo.ProjectInfos.Add(projectInfo);

            }
            return projectCheckInfo;
        }

        /// <summary>
        /// 提交数据
        /// </summary>
        /// <param name="projectInfos"></param>
        public virtual void SubmitData(List<ProjectInfo> projectInfos)
        {
            var saveList = projectInfos.Where(m => m.Result == 0|| m.Result==1);
            EntityList<PrepareRecordDetail> saveEntityList = new EntityList<PrepareRecordDetail>();
            foreach (var projectInfo in saveList)
            {
                PrepareRecordDetail prepareRecordDetail = new PrepareRecordDetail();
                if (projectInfo.RecordDetailId != 0)
                {
                    prepareRecordDetail.PersistenceStatus = PersistenceStatus.Modified;
                }
                else
                {
                    prepareRecordDetail.PersistenceStatus = PersistenceStatus.New;
                }
                prepareRecordDetail.ProcessId = projectInfo.ProcessId;
                prepareRecordDetail.PrepareRecordId = projectInfo.PrepareRecordId;
                prepareRecordDetail.PrepareProjectId = projectInfo.ProjectId;
                prepareRecordDetail.ProjectCode = projectInfo.ProjectCode;
                prepareRecordDetail.ProjectDesc = projectInfo.Desc;
                prepareRecordDetail.ProjectName = projectInfo.ProjectName;
                prepareRecordDetail.Remark = projectInfo.Remark;
                prepareRecordDetail.ConfirmerId = RT.IdentityId;
                prepareRecordDetail.PrepareProjectId = projectInfo.ProjectId;
                prepareRecordDetail.Result = (PrepareRecordDetailResult)projectInfo.Result;
                if (projectInfo.ProjectType != -1)
                {
                    prepareRecordDetail.ProjectType = (PrepareProjectType)projectInfo.ProjectType;
                }

                saveEntityList.Add(prepareRecordDetail);
            }
            Comfrim(saveEntityList);
        }
    }
}


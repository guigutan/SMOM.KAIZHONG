using DocumentFormat.OpenXml.Office2021.DocumentTasks;
using SIE.Andon.Andons;
using SIE.Andon.Andons.Enum;
using SIE.Andon.Andons.ForWinform.ApiModels;
using SIE.Api;
using SIE.Core.ApiLogs;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Items.KzItemCategorys;
using SIE.MES.Andon;
using SIE.MES.LineAndon;
using SIE.MES.PrepareProducts;
using SIE.MES.ProcessPrepareRecords;
using SIE.MES.ProcessProperty;
using SIE.MES.TaskManagement.Dispatchs;
using SIE.MES.TaskManagement.ProcessPrepareRecords.Datas;
using SIE.Resources.Employees;
using SIE.Resources.WipResources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.TaskManagement.ProcessPrepareRecords
{
    public partial class ProcessPrepareRecordsController : DomainController
    {
        /// <summary>
        /// 提交产前准备任务项目
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="infos"></param>
        [ApiService("提交产前准备任务项目")]
        [ApiLog]
        public virtual void SubmitPprListDetailInfos(double Id, double empId, List<SubmitPprListDetailInfo> infos)
        {
            var prepareRecord = RT.Service.Resolve<ProcessPrepareRecordsController>().GetProcessPrepareRecord(Id);//RF.GetById<ProcessPrepareRecord>(Id, new EagerLoadOptions().LoadWithViewProperty());
            var proIds = infos.Select(p => p.ProId).Distinct().ToList();
            //获取项目
            var pros = Query<PrepareProject>().Where(p => proIds.Contains(p.Id)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());

            var details = Query<ProcessPrepareRecordDetail>().Where(p => p.PrepareRecordId == prepareRecord.Id).ToList(null, new EagerLoadOptions().LoadWithViewProperty());

            //EntityList<ProcessPrepareRecordDetail> details = new EntityList<ProcessPrepareRecordDetail>();
            foreach (var info in infos)
            {
                var item = pros.FirstOrDefault(p => p.Id == info.ProId);

                ProcessPrepareRecordDetail prepareRecordDetailItem = null;//details.FirstOrDefault(p => p.PrepareProjectId == info.ProId);
                if (prepareRecordDetailItem == null)
                {
                    prepareRecordDetailItem = new ProcessPrepareRecordDetail();
                    prepareRecordDetailItem.PersistenceStatus = PersistenceStatus.New;
                    details.Add(prepareRecordDetailItem);
                }

                prepareRecordDetailItem.ProcessId = prepareRecord.DispatchTask.ProcessId;
                prepareRecordDetailItem.ProcessName = prepareRecord.DispatchTask.Process.Name;
                prepareRecordDetailItem.PrepareProjectId = item.Id;
                prepareRecordDetailItem.ProjectType = item.ProType;
                prepareRecordDetailItem.ProjectCode = item.ProCode;
                prepareRecordDetailItem.ProjectName = item.ProName;
                prepareRecordDetailItem.ProjectDesc = item.ProDesc;
                prepareRecordDetailItem.Result = info.Result == true ? PrepareProducts.Enums.PrepareRecordDetailResult.Pass : PrepareProducts.Enums.PrepareRecordDetailResult.Fail;
                prepareRecordDetailItem.Remark = info.Remark;
                prepareRecordDetailItem.PrepareRecordId = prepareRecord.Id;
                prepareRecordDetailItem.ConfirmTime = DateTime.Now;
                prepareRecordDetailItem.ConfirmerId = empId;

                //var prepareRecordDetailItem = new ProcessPrepareRecordDetail()
                //{
                //    ProcessId = prepareRecord.DispatchTask.ProcessId,
                //    ProcessName = prepareRecord.DispatchTask.Process.Name,
                //    PrepareProjectId = item.Id,
                //    ProjectType = item.ProType,
                //    ProjectCode = item.ProCode,
                //    ProjectName = item.ProName,
                //    ProjectDesc = item.ProDesc,
                //    Result = info.Result == true ? PrepareProducts.Enums.PrepareRecordDetailResult.Pass : PrepareProducts.Enums.PrepareRecordDetailResult.Fail,
                //    Remark = info.Remark,
                //    PrepareRecordId = prepareRecord.Id,
                //    PersistenceStatus = PersistenceStatus.New,
                //    ConfirmTime= DateTime.Now,
                //    ConfirmerId= empId
                //};
                
            }
            //只要前端的传过来都是通过的，就直接变为通过
            if (infos.All(p => p.Result == true))
                prepareRecord.PrepareState = PrepareProducts.Enums.PrepareRecordState.Confirm;
            using (var tran = DB.TransactionScope(TaskManagementEntityDataProvider.ConnectionStringName))
            {
                RF.Save(details);
                RF.Save(prepareRecord);

                var notProIds = infos.Where(p => p.Result == false).Select(p => p.ProId).Distinct().ToList();
                if (notProIds.Count > 0)
                {
                    //根据不通过的项目获取对应的安灯维护
                    var andons = Query<SIE.Andon.Andons.Andon>().Exists<AndonPrepareProjectDetail>((x, y) => y.Where(p => p.AndonId == x.Id && notProIds.Contains(p.PrepareProjectId))).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
                    if (andons.Count > 0)
                    {
                        var andonUpholdData = Query<SIE.MES.Andon.AndonUphold>().Where(p => p.Id == prepareRecord.DispatchTask.Resource.AndonUpholdId).ToList().FirstOrDefault();
                        if (andonUpholdData == null)
                        {
                            throw new ValidationException("该产线对应的安灯区域没有维护!".L10N());
                        }
                        if (!andonUpholdData.AndonEntity.IsNotEmpty())
                        {
                            throw new ValidationException("安灯区域IOT实体没有维护!".L10N());
                        }
                        if (!andonUpholdData.AndonOrder.IsNotEmpty())
                        {
                            throw new ValidationException("安灯区域IOT指令没有维护!".L10N());
                        }
                        foreach (var andon in andons)
                        {
                            CreateAndonManage(andon, prepareRecord.DispatchTask);

                            //var andonUpholdData = Query<SIE.MES.Andon.AndonUphold>().Where(p => p.Id == prepareRecord.DispatchTask.Resource.AndonUpholdId).ToList().FirstOrDefault();
                            //if (andonUpholdData == null)
                            //{
                            //    throw new ValidationException("该产线对应的安灯区域没有维护!".L10N());
                            //}
                            //if (!andonUpholdData.AndonEntity.IsNotEmpty())
                            //{
                            //    throw new ValidationException("安灯区域IOT实体没有维护!".L10N());
                            //}
                            //if (!andonUpholdData.AndonOrder.IsNotEmpty())
                            //{
                            //    throw new ValidationException("安灯区域IOT指令没有维护!".L10N());
                            //}
                            //触发iot接口 打开
                            var strToKen = RT.Service.Resolve<AndonManageController>().IotGetToken();

                            var iotMessage = RT.Service.Resolve<AndonManageController>().IotGetWrite(strToKen, andonUpholdData.AndonEntity, andonUpholdData.AndonOrder, 1, 1);

                            if (iotMessage.Contains("失败"))
                            {
                                throw new ValidationException("安灯跟IOT接口失败,请到安全区域,检查实体和指令!".L10N());
                            }
                        }
                    }
                }
                tran.Complete();
            }


        }


        public virtual void CreateAndonManage(SIE.Andon.Andons.Andon andon, DispatchTask dispatchTask)
        {
            //获取当前产线下的安灯区域
            var resData = Query<WipResource>().Where(p => p.Id == dispatchTask.ResourceId).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
            if (resData.AndonUpholdId == null)
            {
                throw new ValidationException("产线下没有维护安灯区域描述!".L10N());
            }
            if (resData.AndonUpholdId < 0)
            {
                throw new ValidationException("产线下没有维护安灯区域描述!".L10N());
            }
            var andonLine = Query<AndonLine>().Where(p => p.MachineCode == resData.Code ).ToList().FirstOrDefault();//&& p.AndonUpholdId == resData.AndonUpholdId
            if (andonLine == null)
            {
                throw new ValidationException("资源和区域描述,没有在产线与安灯区域找到!".L10N());
            }
            var equipAccount = andonLine.Equipment;
            if (equipAccount == null && andon.AndonClass == AndonTypeClass.Machine)
            {
                throw new ValidationException("产线与安灯区域,没有维护主设备号!".L10N());
            }
            //根据安灯明细获取A1的人员
            //var andonDesc = Query<AndonSesp>().Where(p => p.AndonId == andon.Id && p.AndonUpholdId == resData.AndonUpholdId).OrderBy(p => p.AndonLevel).FirstOrDefault();
            //if (andonDesc == null)
            //{
            //    throw new ValidationException("安灯维护下的安灯清单,跟当前安灯信息维护不一致!".L10N());
            //}

            var responseDtl = andon.AndonResponseDetailList.Where(p => p.AndonUpholdId == resData.AndonUpholdId).OrderBy(p => p.AndonseepLevel).FirstOrDefault();
            if (responseDtl == null)
            {
                throw new ValidationException("请先维护安灯维护下面的安灯责任组，触发失败！".L10N());
            }
            //任意一个在职的
            var agD = responseDtl.AndonGroup.AndonGroupDetailList.Where(p => p.User.State == State.Enable && p.User.Employee.EmployeeStatus == Resources.EmployeeStatus.Job).FirstOrDefault();
            if (agD == null)
            {
                throw new ValidationException("安灯责任组维护基础表未维护用户，触发失败！".L10N());
            }

            AndonManage andonManage = new AndonManage();

            var dbDateTime = RF.Find<AndonManage>().GetDbTime();
            andonManage.AndonManageCode = RT.Service.Resolve<AndonManageController>().GetAndonManageCode();
            andonManage.State = SIE.Andon.Andons.Enum.AndonManageState.Standby;
            andonManage.TriggerId = RT.IdentityId;
            andonManage.TriggerTime = dbDateTime;
            andonManage.AndonManageClass = andon.AndonClass;
            if (andon.AndonTypeId.HasValue)
            {
                andonManage.AndonTypeId = andon.AndonTypeId.Value;
            }
            andonManage.AndonId = andon.Id;
            andonManage.Solution = andon.Solution;
            andonManage.FaultTime = dbDateTime;
            andonManage.LineStopFlag = andon.LineStop;
            andonManage.AskMaterialFlag = andon.AskMaterial;
            andonManage.AskMaterial = andon.AskMaterial == SIE.Andon.Andons.Enum.AndonYesOrNo.Yes;
            andonManage.LineStop = andon.LineStop == SIE.Andon.Andons.Enum.AndonYesOrNo.Yes;
            andonManage.WipResourceId = (double)dispatchTask.ResourceId;
            andonManage.ProcessId = dispatchTask.ProcessId;
            andonManage.WorkShopId = dispatchTask.WorkShopId.Value;
            andonManage.FactoryId = dispatchTask.FactoryId.Value;
            andonManage.WorkOrderId = dispatchTask.WorkOrderId;
            //andonManage.RespPersonId = (double)andonDesc.EmployeeId;
            andonManage.RespPersonId = agD.User.EmployeeId;
            andonManage.PersistenceStatus = PersistenceStatus.New;
            andonManage.EquipAccount = equipAccount;
            andonManage.ProblemDesc = "产前准备";
            //andonManage.StationId= dispatchTask
            RF.Save(andonManage);
            //触发安灯
            RT.Service.Resolve<AndonManageController>().SendMarkdownMessage(andonManage);
            //创建触发操作记录
            var andonManageOperateLog = new AndonManageOperateLog
            {
                AndonManageId = andonManage.Id,
                OperateTime = dbDateTime,
                OperateType = AndonManageOperateType.Add,
                OperaterId = RT.IdentityId,
                LastOperate = 0,
                PersistenceStatus = PersistenceStatus.New
            };
            RF.Save(andonManageOperateLog);
        }

        /// <summary>
        /// 获取产前准备任务项目
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [ApiService("获取产前准备任务项目")]
        public virtual List<PprListDetailInfo> GetPprListDetailInfos(double id)
        {
            List<PprListDetailInfo> infos = new List<PprListDetailInfo>();

            var record = Query<ProcessPrepareRecord>().Where(p => p.Id == id).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());


            //当找得到物料得时候，优先找到物料的,找不到就直接查全部，然后再找工序的
            //var processPtys = Query<ProcessPty>().Where(p => p.ProcessId == record.DispatchTask.ProcessId).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            //var pps = processPtys.Where(f => f.CategoryItemCode == record.DispatchTask.Product.Code).ToList();
            //if (pps.Count == 0)
            //    pps = processPtys.ToList();
            var processPtys = RT.Service.Resolve<ProcessPtyController>().GetProcessPtysByProcessIds(new List<double>() { record.DispatchTask.ProcessId.Value }, record.DispatchTask.ProductId);
            var kzItemCategory = RT.Service.Resolve<KzItemCategorysController>().GetKzItemCategorieByItemId(record.DispatchTask.ProductId);
            var pps = new List<ProcessPty>();
            if (kzItemCategory != null)
            {
                pps = processPtys.Where(p => p.KzCategoryId == kzItemCategory.KzCategoryId).ToList();
            }
            ////当找得到分类得时候，优先找到分类的，然后再找工序的
            if (pps.Count == 0)
                pps = processPtys.Where(p => p.KzCategoryId == null).ToList();

            var processPtyIds = pps.Select(p => p.Id).Distinct().ToList();
            // 工序找维护工序属性里面子表明细项目

            var processPtyDetails = processPtyIds.SplitContains(ids =>
             {
                 return Query<ProcessPtyDetail>().Where(p => ids.Contains(p.ProcessPtyId)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
             });

            //var processPtyDetails = Query<ProcessPtyDetail>().Join<ProcessPty>((x, y) => x.ProcessPtyId == y.Id && y.ProcessId == record.DispatchTask.ProcessId).ToList(null, new EagerLoadOptions().LoadWithViewProperty());

            foreach (var processPtyDetail in processPtyDetails)
            {
                PprListDetailInfo info = new PprListDetailInfo();
                info.Id = processPtyDetail.Id;
                info.ProId = processPtyDetail.PrepareProjectId;
                info.ProType = processPtyDetail.ProType?.ToLabel();
                info.ProDesc = processPtyDetail.ProDesc;
                infos.Add(info);
            }

            return infos;
        }

        /// <summary>
        /// 获取产前准备任务
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [ApiService("获取产前准备任务")]
        public virtual List<PprListInfo> GetPprListInfos(string key, int pageSize, int pageNumber)
        {
            List<PprListInfo> pprListInfos = new List<PprListInfo>();

            //必须是该员工下的所属的资源,且是待确认状态的数据
            var q = Query<ProcessPrepareRecord>().Join<EmployeeResource>((x, y) => x.DispatchTask.ResourceId == y.ResourceId && y.EmployeeId == RT.IdentityId).Where(p => p.PrepareState == PrepareProducts.Enums.PrepareRecordState.ToConfirm);
            if (!key.IsNullOrEmpty())
            {
                //找出对应的产线
                var andonLines = Query<AndonLine>().Where(p => p.MachineCode.Contains(key) || p.MachineName.Contains(key)).ToList();
                //将找出来的产线的工作中心也带出来
                var workOrderCodes = andonLines.GroupBy(p => p.WorkCenterId).Select(p => p.FirstOrDefault().WorkCenter.Code).Distinct().ToList();

                if (workOrderCodes.Count > 0)
                    q.Where(p => p.DispatchTask.WorkOrder.No.Contains(key) || p.DispatchTask.No.Contains(key) || p.DispatchTask.Resource.Code.Contains(key) || p.DispatchTask.Resource.Name.Contains(key) || p.DispatchTask.Product.Code.Contains(key) || workOrderCodes.Contains(p.DispatchTask.Resource.Code));
                else
                    q.Where(p => p.DispatchTask.WorkOrder.No.Contains(key) || p.DispatchTask.No.Contains(key) || p.DispatchTask.Resource.Code.Contains(key) || p.DispatchTask.Resource.Name.Contains(key) || p.DispatchTask.Product.Code.Contains(key));

            }
            var list = q.OrderBy(p => p.CreateDate).ToList(new PagingInfo(pageNumber, pageSize), new EagerLoadOptions().LoadWithViewProperty());

            foreach (var l in list)
            {
                PprListInfo pprListInfo = new PprListInfo();

                pprListInfo.Id = l.Id;
                pprListInfo.WorkOrderNo = l.WorkOrderNo;
                pprListInfo.TaskNo = l.DispatchTaskNo;
                pprListInfo.Process = l.ProcessName;
                pprListInfo.WipResource = l.Resource;
                pprListInfo.WipResourceId = l.ResourceId;
                pprListInfo.ProductCode = l.ProductCode;
                pprListInfo.ProductName = l.ProductName;
                pprListInfo.Qty = l.PlanQty;
                pprListInfo.State = l.PrepareState.ToLabel();
                pprListInfo.PlanBeginTime = l.PlanBeginDate?.ToString("yyyy-MM-dd HH:mm:ss");

                pprListInfos.Add(pprListInfo);
            }

            return pprListInfos;

        }
    }
}

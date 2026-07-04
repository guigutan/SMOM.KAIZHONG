using SIE.Andon.Andons.APIModel;
using SIE.Andon.Andons.Enum;
using SIE.Api;
using SIE.Common.Attachments;
using SIE.Core.ApiModels;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Equipments.Abnormal;
using SIE.Equipments.EquipAccounts;
using SIE.EventMessages.MES.WorkOrders.Models;
using SIE.Items;
using SIE.LES.LinesideWarehouses;
using SIE.MES.PrepareProducts;
using SIE.MES.WIP.ApiModels;
using SIE.MES.WorkOrders;
using SIE.Resources.Employees;
using SIE.Resources.Enterprises;
using SIE.Resources.WipResources;
using SIE.Tech.Processs;
using SIE.Tech.Stations;
using SIE.Utils;
using SIE.Warehouses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace SIE.Andon.Andons
{
    public partial class AndonManageController : DomainController
    {

        /// <summary>
        /// 获取安灯
        /// </summary>
        /// <param name="keyword">搜索关键字</param>
        /// <returns></returns>
        [ApiService("获取安灯明细")]
        [return: ApiReturn("返回安灯明细：List<Andon>")]
        public virtual List<Andon> GetAndonManageInfo([ApiParameter] string keyword)
        {
            var result = new List<Andon>();
            var types = GetCurrentUserAndonTypes();
            if (types.Any())
            {
                var andonTypeIds = types.Select(m => (double?)m.Id).ToList();
                var res = Query<Andon>().Where(y => y.State == Domain.State.Enable && andonTypeIds.Contains(y.AndonTypeId))
                    .WhereIf(!keyword.IsNullOrEmpty(), y => y.AndonCode == keyword || y.AndonName == keyword)
                    .ToList(null, new EagerLoadOptions().LoadWithViewProperty());
                return res.OrderBy(m => m.AndonClass).ThenBy(m => m.OrderNo).ToList();
            }
            return result;
        }

        /// <summary>
        /// 获取相同类型的安灯明细
        /// </summary>
        /// <param name="andonTypeId"></param>
        /// <returns></returns>
        [ApiService("获取相同类型的安灯明细")]
        [return: ApiReturn("返回相同类型的安灯维护：List<Andon>")]
        public virtual List<Andon> GetSameTypeAndonInfo([ApiParameter] double andonTypeId)
        {
            var res = Query<Andon>().Where(y => y.State == Domain.State.Enable && andonTypeId == y.AndonTypeId)
                        .ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            var resultList = res.OrderBy(m => m.AndonClass).ThenBy(m => m.OrderNo).ToList();
            return resultList;
        }



        /// <summary>
        /// 创建安灯触发
        /// </summary>
        /// <param name="andonId"></param>
        /// <param name="stationId"></param>
        /// <returns></returns>
        [ApiService("创建安灯触发")]
        [return: ApiReturn("返回安灯触发：List<Andon>")]
        public virtual AndonManageInfo GetNewAndonManage([ApiParameter(description: "安灯Id")] double andonId, [ApiParameter(description: "资源Id")] double resourceId)
        {
            var andon = Query<Andon>().Where(m => m.Id == andonId).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
            if (andon != null)
            {
                var identity = RF.GetById<Employee>(RT.IdentityId);
                var team = Query<WorkGroup>().Where(p => p.Id == identity.WorkGroupId).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
                var andonManage = new AndonManageInfo
                {
                    AndonManageCode = RT.Service.Resolve<AndonManageController>().GetAndonManageCode(),
                    State = SIE.Andon.Andons.Enum.AndonManageState.Standby,
                    TriggerId = RT.IdentityId,
                    TriggerTime = RF.Find<AndonManage>().GetDbTime(),
                    FaultTime = RF.Find<AndonManage>().GetDbTime().ToString(),
                    WorkGroupId = identity.WorkGroupId.HasValue ? identity.WorkGroupId : null,
                    WorkGroup = team != null ? team.Name : "",
                    AndonManageClass = andon.AndonClass.ToLabel(),
                    AndonId = andon.Id,
                    Solution = andon.Solution,
                    Department = andon.DepartmentName,
                    DepartmentId = (double)andon.DepartmentId,
                    AndonName = andon.AndonName,
                    AndonTypeName = andon.AndonTypeName
                };

                if (andon.AndonTypeId.HasValue)
                {
                    andonManage.AndonTypeId = andon.AndonTypeId.Value;
                }
                andonManage.LineStopFlag = andon.LineStop;
                andonManage.AskMaterialFlag = andon.AskMaterial;
                andonManage.AskMaterial = (andon.AskMaterial != Enum.AndonYesOrNo.Artificial && andon.AskMaterial == Enum.AndonYesOrNo.Yes);
                andonManage.LineStop = (andon.LineStop != Enum.AndonYesOrNo.Artificial && andon.LineStop == Enum.AndonYesOrNo.Yes);
                var wos = GetResourceWorkOrder(resourceId);
                if (wos != null && wos.Any())
                {
                    var wo = wos.FirstOrDefault();
                    if (wo != null)
                    {
                        andonManage.woId = wo.Id;
                        andonManage.WoNo = wo.No;
                    }
                }
                //根据选择的工位带出在制工单 可选生产资源的工单

                return andonManage;
            }
            return null;
        }

        /// <summary>
        /// 提交安灯触发
        /// </summary>
        /// <param name="andonManageInfo"></param>
        /// <param name="submitCallMaterialInfo"></param>
        /// <returns></returns>
        [ApiService("提交安灯触发")]
        [return: ApiReturn("返回成功失败")]
        public virtual bool SubmitAndonManage([ApiParameter(description: "提交的安灯数据")] AndonManageInfo andonManageInfo, [ApiParameter("叫料数据")] SubmitCallMaterialInfo submitCallMaterialInfo)
        {
            var andon = Query<Andon>().Where(m => m.Id == andonManageInfo.AndonId).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
            if (andon == null)
            {
                throw new ValidationException("安灯数据异常，触发失败！".L10N());
            }
            if (andon.AndonClass == Enum.AndonTypeClass.Machine && !andonManageInfo.EquipAccountId.HasValue)
            {
                throw new ValidationException("安灯大类为【机】时校验设备台账必输".L10N());
            }

            var andonManage = new AndonManage();
            CreateAndonManage(andonManage, andon, andonManageInfo);

            if (andon.RepeatTrigger && AndonManageRepeatTrigger(andonManage))
            {
                throw new ValidationException("该安灯{0}存在未关闭的事件，请确认是否重复触发".L10nFormat(andon.AndonCode));
            }
            using (var tran = DB.AutonomousTransactionScope(AndonEntityDataProvider.ConnectionStringName))
            {
                //生成停线
                if (andonManage.LineStop)
                {
                    var abnormalCause = new AbnormalCause
                    {
                        Code = RT.Service.Resolve<AbnormalCauseController>().GetNewAbnormalCode(),
                        SourceType = ExceptionStopSourceType.AlertLight,
                        EquipAccountId = andonManage.EquipAccountId,
                        ResourceId = andonManage.WipResourceId,
                        ExceptionStopType = ExceptionStopType.StopLine,
                        AbnormalType = andonManage.Andon.DefaultType,
                        AbnormalReason = andonManage.ProblemDesc,
                        ShopId = andonManage.WorkShopId,
                        WorkOrderId = andonManage.WorkOrderId
                    };
                    RF.Save(abnormalCause);
                    andonManage.AbnormalCauseId = abnormalCause.Id;
                }
                RF.Save(andonManage);
                if (andonManage.AskMaterial)
                {
                    var callMaterialInfo = CreateAndonCallMaterial(submitCallMaterialInfo, andonManageInfo, andonManage.Id);
                    SaveItemDetail(new EntityList<AndonManageCallMaterial> { callMaterialInfo });
                }
                //创建触发操作记录
                var operateTime = RF.Find<AndonManageOperateLog>().GetDbTime();
                if (operateTime < andonManage.FaultTime)
                {
                    throw new ValidationException("安灯故障发生时间不能晚于当前时间！".L10N());
                }
                var andonManageOperateLog = new AndonManageOperateLog
                {
                    AndonManageId = andonManage.Id,
                    OperateTime = operateTime,
                    OperateType = AndonManageOperateType.Add,
                    OperaterId = RT.IdentityId,
                    LastOperate = 0
                };

                RF.Save(andonManageOperateLog);
                var resource = RF.GetById<WipResource>(andonManageInfo.WipResourceId);
                var andonUpholdData = Query<SIE.MES.Andon.AndonUphold>().Where(p => p.Id == resource.AndonUpholdId).ToList().FirstOrDefault();
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
                //触发安灯
                RT.Service.Resolve<AndonManageController>().SendMarkdownMessage(andonManage);
                //触发iot接口 打开
                var strToKen = RT.Service.Resolve<AndonManageController>().IotGetToken();

                var iotMessage = RT.Service.Resolve<AndonManageController>().IotGetWrite(strToKen, andonUpholdData.AndonEntity, andonUpholdData.AndonOrder, 1, 1);

                if (iotMessage.Contains("失败"))
                {
                    throw new ValidationException("安灯跟IOT接口失败,请到安全区域,检查实体和指令!".L10N());
                }
                tran.Complete();
            }
            return true;
        }

        private AndonManageCallMaterial CreateAndonCallMaterial(SubmitCallMaterialInfo submitCallMaterialInfo, AndonManageInfo andonManageInfo, double andonManageId)
        {
            if (submitCallMaterialInfo == null)
            {
                throw new ValidationException("物料清单信息有误！".L10N());
            }
            if (submitCallMaterialInfo.ItemId == null || submitCallMaterialInfo.ItemId == 0)
            {
                throw new ValidationException("生成备料单物料不能为空！".L10N());
            }
            var item = RF.GetById<Item>(submitCallMaterialInfo.ItemId);
            var resource = RF.GetById<WipResource>(andonManageInfo.WipResourceId);
            if (item == null)
            {
                throw new ValidationException("物料不存在！".L10N());
            }
            if (resource == null)
            {
                throw new ValidationException("资源信息不存在！".L10N());
            }
            AndonManageCallMaterial andonManageCallMaterial = new AndonManageCallMaterial
            {
                AndonManageId = andonManageId,
                ItemId = item.Id,
                ConsumeType = item.ConsumeMode,
                Qty = submitCallMaterialInfo.Qty,
                TimeNeed = DateTime.Parse(submitCallMaterialInfo.NeedTime),
                WareHouseId = submitCallMaterialInfo.LineWareId,
                StorageLocationId = submitCallMaterialInfo.LineStorageId,
                WorkOrderId = andonManageInfo.woId,
                FactoryId = (double)resource.FactoryId,
                WorkShopId = (double)resource.WorkShopId,
                WipId = resource.Id,
            };
            return andonManageCallMaterial;
        }

        private void CreateAndonManage(AndonManage andonManage, Andon andon, AndonManageInfo andonManageInfo)
        {
            andonManage.GenerateId();

            //to do  赋值
            if (andonManageInfo.WipResourceId <= 0)
            {
                throw new ValidationException("请选择产线".L10N());
            }
            var resource = RF.GetById<WipResource>(andonManageInfo.WipResourceId);
            if (resource == null)
            {
                throw new ValidationException("系统不存在该产线".L10N());
            }
            if (!resource.FactoryId.HasValue)
            {
                throw new ValidationException("当前所选产线未维护所属工厂，请维护".L10N());
            }
            if (!resource.WorkShopId.HasValue)
            {
                throw new ValidationException("当前所选产线未维护所属工厂，请维护".L10N());
            }
            //获取当前产线下的安灯区域
            var resData = Query<WipResource>().Where(p => p.Id == andonManageInfo.WipResourceId).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
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
            andonManage.RespPersonId = agD.User.EmployeeId;

            andonManage.AndonId = andon.Id;
            //andonManage.RespPersonId = (double)andonDesc.EmployeeId;
            andonManage.FactoryId = resource.FactoryId.Value;
            andonManage.WorkShopId = resource.WorkShopId.Value;
            andonManage.WipResourceId = resource.Id;
            andonManage.AndonManageCode = andonManageInfo.AndonManageCode;
            andonManage.State = andonManageInfo.State;
            andonManage.TriggerId = RT.IdentityId;
            andonManage.TriggerTime = andonManageInfo.TriggerTime;
            andonManage.FaultTime = DateTime.Parse(andonManageInfo.FaultTime);
            andonManage.WorkGroup = andonManageInfo.WorkGroup;
            andonManage.AndonManageClass = andon.AndonClass;
            andonManage.AndonId = andon.Id;
            andonManage.Solution = andon.Solution;
            andonManage.Department = andon.DepartmentName;
            andonManage.AndonName = andon.AndonName;
            andonManage.AndonTypeName = andon.AndonTypeName;
            andonManage.ProblemDesc = andonManageInfo.ProblemDesc;
            if (!andonManageInfo.Defect.IsNullOrEmpty())
            {
                andonManage.Defect = andonManageInfo.Defect.TrimEnd(',');
            }
            if (!andonManageInfo.DefectIds.IsNullOrEmpty())
            {
                andonManage.DefectIds = andonManageInfo.DefectIds.TrimEnd(',');
            }

            //andonManage.StationId = andonManageInfo.StationId;
            //andonManage.ProcessId = andonManageInfo.ProcessId;
            if (andon.AndonTypeId.HasValue)
            {
                andonManage.AndonTypeId = andon.AndonTypeId.Value;
            }
            andonManage.AskMaterial = andonManageInfo.AskMaterial;
            if (andonManageInfo.woId != -1)
            {
                andonManage.WorkOrderId = andonManageInfo.woId;
            }
            else
            {
                andonManage.WorkOrderId = null;
            }
            andonManage.LineStop = andonManageInfo.LineStop;
            andonManage.BarCode = andonManageInfo.BarCode;
            if (andonManageInfo.EquipAccountId != -1)
            {
                andonManage.EquipAccountId = andonManageInfo.EquipAccountId;
            }
            else
            {
                andonManage.EquipAccountId = null;
            }
            var uploadFilePath = UploadPhoto(andonManageInfo.AttachmentInfos.FirstOrDefault());
            if (!uploadFilePath.IsNullOrEmpty())
            {
                andonManage.PhotoFile = uploadFilePath;
            }
        }

        /// <summary>
        /// 根据产线获取设备台账列表
        /// </summary>
        /// <param name="queryInfo">设备查询信息</param>
        /// <returns>分页设备信息</returns>
        [ApiService("根据产线获取设备台账列表")]
        [return: ApiReturn("分页设备信息 PagingBaseDataInfo")]
        public virtual PagingBaseDataInfo GetEquipAccountInfos([ApiParameter("设备查询信息")] EquipQueryInfo queryInfo)
        {
            var pageNumber = queryInfo.PageNumber;
            var pageSize = queryInfo.PageSize;
            var pagingInfo = new PagingInfo()
            {
                PageNumber = pageNumber.HasValue ? pageNumber.Value : 1,
                PageSize = pageSize.HasValue ? pageSize.Value : int.MaxValue - 1,
                IsNeedCount = true
            };


            var equips = GetEquipAccountsByResourceId(queryInfo.ResourceId, queryInfo.Keyword, pagingInfo);
            if (equips.Count <= 0)
                equips = GetEquipAccounts(pagingInfo, queryInfo.Keyword);

            var infos = new List<BaseDataInfo>();
            equips.ForEach(workshop =>
            {
                infos.Add(new BaseDataInfo()
                {
                    Id = workshop.Id,
                    Code = workshop.Code,
                    Name = workshop.Name
                });
            });
            PagingBaseDataInfo result = new PagingBaseDataInfo()
            {
                PageNumber = pagingInfo.PageNumber,
                PageSize = pagingInfo.PageSize,
                TotalCount = equips.TotalCount
            };
            result.DataInfos.AddRange(infos);
            return result;
        }

        /// <summary>
        /// 根据产线获取工单
        /// </summary>
        /// <param name="queryInfo"></param>
        /// <returns></returns>
        [ApiService("根据产线获取工单列表")]
        [return: ApiReturn("产线选工单 PagingBaseDataInfo")]
        public virtual PagingBaseDataInfo GetWorkOrders([ApiParameter("工单查询信息")] EquipQueryInfo queryInfo)
        {
            if (queryInfo.ResourceId <= 0)
            {
                throw new ValidationException("未选择产线，请先选择产线才能选工单".L10N());
            }

            var pageNumber = queryInfo.PageNumber;
            var pageSize = queryInfo.PageSize;
            var pagingInfo = new PagingInfo()
            {
                PageNumber = pageNumber.HasValue ? pageNumber.Value : 1,
                PageSize = pageSize.HasValue ? pageSize.Value : int.MaxValue - 1,
                IsNeedCount = true
            };

            var wos = Query<WorkOrder>()
                    .Where(p => p.ResourceId == queryInfo.ResourceId)
                    .Where(p => p.State != Core.WorkOrders.WorkOrderState.Finish && p.State != Core.WorkOrders.WorkOrderState.Close)
                    .WhereIf(!queryInfo.Keyword.IsNullOrEmpty(), p => p.No.Contains(queryInfo.Keyword))
                    .ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());


            var infos = new List<BaseDataInfo>();
            wos.ForEach(wo =>
            {
                infos.Add(new BaseDataInfo()
                {
                    Id = wo.Id,
                    Code = wo.No,
                    Name = ""
                });
            });
            PagingBaseDataInfo result = new PagingBaseDataInfo()
            {
                PageNumber = pagingInfo.PageNumber,
                PageSize = pagingInfo.PageSize,
                TotalCount = wos.TotalCount
            };
            result.DataInfos.AddRange(infos);
            return result;
        }

        /// <summary>
        /// 获取安灯管理信息
        /// </summary>
        /// <param name="andonManageId"></param>
        /// <returns></returns>
        [ApiService("获取安灯管理信息")]
        [return: ApiReturn("安灯管理信息 AndonManageInfo")]
        public virtual AndonManageInfo GetAndonManageDetailInfo([ApiParameter("安灯管理数据Id")] double andonManageId)
        {

            var andonManage = RF.GetById<AndonManage>(andonManageId, new EagerLoadOptions().LoadWithViewProperty());
            if (andonManage == null)
            {
                throw new ValidationException("数据不存在".L10N());
            }
            var result = new AndonManageInfo
            {
                Id = andonManage.Id,
                LineStop = andonManage.LineStop,
                ProblemDesc = andonManage.ProblemDesc,
                ProcessId = andonManage.ProcessId,
                Process = andonManage.ProcessName,
                Solution = andonManage.Solution,
                State = andonManage.State,
                AndonTypeName = andonManage.AndonTypeName,
                AndonTypeId = andonManage.AndonTypeId,
                AndonManageClass = andonManage.AndonManageClass.ToLabel(),
                AndonManageCode = andonManage.AndonManageCode,
                AndonName = andonManage.AndonName,
                AskMaterial = andonManage.AskMaterial,
                BarCode = andonManage.BarCode,
                WorkGroup = andonManage.WorkGroup,
                WoNo = andonManage.WoNo,
                Defect = andonManage.Defect,
                Department = andonManage.Department,
                Station = andonManage.StationName,
                WorkShop = andonManage.WorkShopName,
                WipResource = andonManage.WipResourceName,
                EquipAccountCode = andonManage.EquipAccountCode,
                EquipAccountName = andonManage.EquipAccountName,
                HandleId = andonManage.HandlerId,
                TriggerId = andonManage.TriggerId,
                AndonId = andonManage.AndonId,
            };
            if (!andonManage.PhotoFile.IsNullOrEmpty())
            {
                var attachmentInfo = new APIModel.AttachmentInfo()
                {
                    Content = GetAttachmentUrl(andonManage.PhotoFile, andonManage.PhotoFile).Replace('\\', '/'),
                    FileName = andonManage.PhotoFile

                };
                result.AttachmentInfos.Add(attachmentInfo);
            }

            return result;
        }

        /// <summary>
        /// 获取状态为待响应且有响应权限的安灯（暂无权限控制逻辑）
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [ApiService("获取待响应安灯管理信息")]
        [return: ApiReturn("待响应安灯管理信息 StandbyAndonManageInfo")]
        public virtual List<AndonManageInfo> GetStandbyAndonManageDetailInfo([ApiParameter("分页页数")] int pageNumber, [ApiParameter("分页大小")] int pageSize)
        {
            PagingInfo pagingInfo = new PagingInfo();
            pagingInfo.PageNumber = pageNumber;
            pagingInfo.PageSize = pageSize;
            var andonManages = Query<AndonManage>()
                .Exists<EmployeeEnterprise>((x, y) => y.Where(p => p.EmployeeId == RT.IdentityId && p.EnterpriseId == x.FactoryId))
                .Where(p => p.State == AndonManageState.Standby).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
            var andonManageInfoList = new List<AndonManageInfo>();
            if (andonManages.Count > 0)
            {
                andonManages.ForEach(andonManage =>
                {
                    var andonManageInfo = new AndonManageInfo
                    {
                        Id = andonManage.Id,
                        LineStop = andonManage.LineStop,
                        ProblemDesc = andonManage.ProblemDesc,
                        ProcessId = andonManage.ProcessId,
                        Process = andonManage.ProcessName,
                        Solution = andonManage.Solution,
                        State = andonManage.State,
                        AndonTypeName = andonManage.AndonTypeName,
                        AndonTypeId = andonManage.AndonTypeId,
                        AndonManageClass = andonManage.AndonManageClass.ToLabel(),
                        AndonManageCode = andonManage.AndonManageCode,
                        AndonName = andonManage.AndonName,
                        AskMaterial = andonManage.AskMaterial,
                        BarCode = andonManage.BarCode,
                        WorkGroup = andonManage.WorkGroup,
                        WoNo = andonManage.WoNo,
                        Defect = andonManage.Defect,
                        Department = andonManage.Department,
                        Station = andonManage.StationName,
                        WorkShop = andonManage.WorkShopName,
                        WipResource = andonManage.WipResourceName,
                        EquipAccountCode = andonManage.EquipAccountCode,
                        EquipAccountName = andonManage.EquipAccountName,
                        HandleId = andonManage.HandlerId,
                        TriggerId = andonManage.TriggerId
                    };
                    if (!andonManage.PhotoFile.IsNullOrEmpty())
                    {
                        var attachmentInfo = new APIModel.AttachmentInfo()
                        {
                            Content = GetAttachmentUrl(andonManage.PhotoFile, andonManage.PhotoFile),
                            FileName = andonManage.PhotoFile
                        };
                        andonManageInfo.AttachmentInfos.Add(attachmentInfo);
                    }
                    andonManageInfoList.Add(andonManageInfo);
                });
            }
            return andonManageInfoList;
        }

        /// <summary>
        /// 获取单据列表信息
        /// </summary>
        /// <param name="queryInfo"></param>
        /// <returns></returns>
        [ApiService("获取安灯管理单据列表")]
        [return: ApiReturn("分页安灯管理信息 AndonManagerResultInfos")]
        public virtual AndonManagerResultInfos GetBillInfos([ApiParameter("查询信息")] PagingKeywordQueryInfo queryInfo)
        {
            var andonManagerResultInfos = new AndonManagerResultInfos();
            var pageNumber = queryInfo.PageNumber;
            var pageSize = queryInfo.PageSize;
            var pagingInfo = new PagingInfo()
            {
                PageNumber = pageNumber.HasValue ? pageNumber.Value : 1,
                PageSize = pageSize.HasValue ? pageSize.Value : int.MaxValue - 1,
                IsNeedCount = true
            };
            //触发人或处理人为当前用户且状态为待响应、处理中、待验收的安灯
            var res = Query<AndonManage>().Where(m => (m.State == AndonManageState.Processing || m.State == AndonManageState.Standby || m.State == AndonManageState.ToAccepted) &&
              (m.HandlerId == RT.IdentityId || m.TriggerId == RT.IdentityId)).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
            res.ForEach(item =>
            {

                var andonManagerResultInfo = new AndonManagerResultInfo();
                andonManagerResultInfo.AndonName = item.AndonName;
                andonManagerResultInfo.AndonId = item.AndonId;
                andonManagerResultInfo.Id = item.Id;
                andonManagerResultInfo.Code = item.AndonManageCode;
                andonManagerResultInfo.AndonType = item.AndonTypeName;
                andonManagerResultInfo.EquipmentName = item.EquipAccountName;
                andonManagerResultInfo.ExecptionDesc = item.ProblemDesc;
                andonManagerResultInfo.Line = item.WipResourceName;
                andonManagerResultInfo.WorkShop = item.WorkShopName;
                andonManagerResultInfo.State = (int)item.State;

                andonManagerResultInfos.AndonManagerResults.Add(andonManagerResultInfo);
            });
            andonManagerResultInfos.TotalCount = res.TotalCount;
            andonManagerResultInfos.PageNumber = pageNumber;
            andonManagerResultInfos.PageSize = pageSize;

            return andonManagerResultInfos;
        }

        /// <summary>
        /// 获取安灯经验单据列表
        /// </summary>
        /// <param name="queryInfo">分页信息</param>
        /// <param name="andonId">安灯编码</param>
        /// <returns></returns>

        [ApiService("获取安灯经验单据列表")]
        [return: ApiReturn("分页安灯经验信息 AndonManagerResultInfos")]
        public virtual AndonExperienceInfos GetExperienceInfos([ApiParameter("查询信息")] PagingKeywordQueryInfo queryInfo, [ApiParameter("安灯编码")] double andonId)
        {
            var andonExperienceInfos = new AndonExperienceInfos();
            var pageNumber = queryInfo.PageNumber;
            var pageSize = queryInfo.PageSize;
            var pagingInfo = new PagingInfo()
            {
                PageNumber = pageNumber.HasValue ? pageNumber.Value : 1,
                PageSize = pageSize.HasValue ? pageSize.Value : int.MaxValue - 1,
                IsNeedCount = true
            };

            var res = Query<AndonExperience>().Where(m => m.AndonId == andonId && m.ExperienceFlag).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
            res.ForEach(item =>
            {

                var andonExperienceInfo = new AndonExperienceInfo();
                andonExperienceInfo.EventReason = item.Reason;
                andonExperienceInfo.HandleMethod = item.HandleMethod;
                andonExperienceInfo.Id = item.Id;
                andonExperienceInfo.Code = item.AndonManageCode;
                andonExperienceInfo.Measures = item.Measures;
                andonExperienceInfos.AndonExperienceResults.Add(andonExperienceInfo);
            });
            andonExperienceInfos.TotalCount = res.TotalCount;
            andonExperienceInfos.PageNumber = pageNumber;
            andonExperienceInfos.PageSize = pageSize;
            return andonExperienceInfos;
        }


        /// <summary>
        ///取消安灯
        /// </summary>
        /// <param name="andonManageId">安灯管理Id</param>
        /// <param name="reason">取消原因</param>
        /// <returns></returns>
        [ApiService("取消安灯")]
        [return: ApiReturn("")]
        public virtual void CancelAndonManage([ApiParameter("安灯管理Id")] double andonManageId, [ApiParameter("取消原因")] string reason)
        {
            this.AndonManageCancel(andonManageId, AndonManageOperateType.Cancel, reason);
        }

        /// <summary>
        /// 响应安灯
        /// </summary>
        /// <param name="andonManageId">安灯管理Id</param>
        /// <param name="reason">备注</param>
        /// <returns></returns>
        [ApiService("响应安灯")]
        [return: ApiReturn("")]
        public virtual void ResponseAndonManage([ApiParameter("安灯管理Id")] double andonManageId, [ApiParameter("备注")] string reason)
        {
            this.AndonManageResponse(andonManageId, AndonManageOperateType.Response, reason);
        }

        /// <summary>
        /// 安灯转派
        /// </summary>
        /// <param name="andonManageId">安灯管理Id</param>
        /// <param name="reason">备注</param>
        /// <param name="reassignEmployeeId">转派员工</param>
        /// <param name="ressignAndonId">转派安灯Id</param>
        [ApiService("安灯转派")]
        [return: ApiReturn("")]
        public virtual void ReassignmenAndonManaget([ApiParameter("安灯管理Id")] double andonManageId, [ApiParameter("备注")] string reason,
            [ApiParameter("转派员工")] double? reassignEmployeeId, [ApiParameter("转派安灯Id")] double ressignAndonId)
        {
            this.AndonManageReassignment(andonManageId, AndonManageOperateType.Reassignment, reason, reassignEmployeeId, ressignAndonId);
        }

        /// <summary>
        /// 处理完成
        /// </summary>
        /// <param name="andonManageId"></param>
        /// <param name="reason"></param>
        /// <param name="enventReason">事件原因</param>
        /// <param name="handleWay">处理方式</param>
        /// <param name="measure">预防措施</param>
        [ApiService("处理完成")]
        [return: ApiReturn("")]
        public virtual void HandleAndonManage([ApiParameter("安灯管理Id")] double andonManageId, [ApiParameter("备注")] string reason,
           [ApiParameter("安灯管理Id")] string enventReason, [ApiParameter("安灯管理Id")] string handleWay, [ApiParameter("安灯管理Id")] string measure)

        {
            this.AndonManageHandleAsync(andonManageId, AndonManageOperateType.Handle, reason, enventReason, handleWay, measure);
        }


        /// <summary>
        /// 安灯管理验收命令
        /// </summary>
        /// <param name="andonManageId">安灯管理Id</param>
        /// <param name="reason">备注</param>
        /// <param name="actualTime">实际影响时间</param>
        [ApiService("安灯管理验收命令")]
        [return: ApiReturn("")]
        public virtual void CheckAndonManage([ApiParameter("安灯管理Id")] double andonManageId, [ApiParameter("备注")] string reason, [ApiParameter("实际影响时间")] double? actualTime)
        {
            this.AndonManageCheck(andonManageId, AndonManageOperateType.Check, reason, actualTime);
        }


        /// <summary>
        /// 驳回
        /// </summary>
        /// <param name="andonManageId"></param>
        /// <param name="reason"></param>
        [ApiService("安灯管理不通过命令")]
        [return: ApiReturn("")]
        public virtual void RejectAndonManage([ApiParameter("安灯管理Id")] double andonManageId, [ApiParameter("备注")] string reason)
        {
            this.AndonManageReject(andonManageId, AndonManageOperateType.Reject, reason);
        }


        /// <summary>
        /// 获取工序信息列表
        /// </summary>
        /// <param name="queryInfo">工序查询信息</param>
        /// <returns>工序信息列表</returns>
        [ApiService("获取工序信息列表")]
        [return: ApiReturn("获取工序信息列表 GetAllProcessDataInfos")]
        public virtual ProcessDataInfo GetAllProcessDataInfos([ApiParameter("工序查询信息")] ProcessQueryInfo queryInfo)
        {
            if (queryInfo.EmployeeId <= 0)
                throw new ValidationException("员工ID不存在!".L10N());
            var employee = RF.GetById<Employee>(queryInfo.EmployeeId);
            if (employee == null)
                throw new ValidationException("员工信息不存在!".L10N());
            var pageNumber = queryInfo.PageNumber;
            var pageSize = queryInfo.PageSize;
            var pagingInfo = new PagingInfo()
            {
                PageNumber = pageNumber.HasValue ? pageNumber.Value : 1,
                PageSize = pageSize.HasValue ? pageSize.Value : int.MaxValue - 1,
                IsNeedCount = true
            };

            var query = Query<Process>().Exists<ProcessEmployee>((a, b) => b.Where(f => f.ProcessId == a.Id && f.EmployeeId == queryInfo.EmployeeId));
            if (queryInfo.Keyword.IsNotEmpty())
                query.Where(p => p.Name.Contains(queryInfo.Keyword));
            var processList = query.ToList();
            var result = CreateProcessDataInfos(pagingInfo, processList);
            return result;
        }

        /// <summary>
        /// 获取工位信息列表
        /// </summary>
        /// <param name="queryInfo">工序查询信息</param>
        /// <returns>工序信息列表</returns>
        [ApiService("获取工位信息列表")]
        [return: ApiReturn("获取工序信息列表 GetAllStationDataInfos")]
        public virtual StationDataInfo GetAllStationDataInfos([ApiParameter("工序查询信息")] StationQueryInfo queryInfo)
        {
            if (queryInfo.EmployeeId <= 0)
                throw new ValidationException("员工Id不存在！".L10N());
            var employee = RF.GetById<Employee>(queryInfo.EmployeeId);
            if (employee == null)
                throw new ValidationException("员工不存在".L10N());
            if (queryInfo.ResourceId <= 0)
                throw new ValidationException("资源Id不存在！".L10N());
            var resource = RF.GetById<WipResource>(queryInfo.ResourceId);
            if (resource == null)
                throw new ValidationException("资源不存在！".L10N());
            if (!RT.Service.Resolve<WipResourceController>().IsExistEmployeeResource(queryInfo.EmployeeId, queryInfo.ResourceId))
                throw new ValidationException("员工[{0}]与资源[{1}]不存在关系！".L10nFormat(employee.Name, resource.Name));
            var processTypes = System.Enum.GetValues(typeof(ProcessType)).Cast<ProcessType>().Select(e => (int)e).ToList();
            if (!processTypes.Contains(queryInfo.ProcessType))
                throw new ValidationException("工序类型不存在！".L10N());
            if (queryInfo.ProcessId <= 0)
                throw new ValidationException("工序Id不存在！".L10N());
            var process = RF.GetById<Process>(queryInfo.ProcessId);
            if (process == null)
                throw new ValidationException("工序不存在！".L10N());
            var pageNumber = queryInfo.PageNumber;
            var pageSize = queryInfo.PageSize;
            var pagingInfo = new PagingInfo()
            {
                PageNumber = pageNumber.HasValue ? pageNumber.Value : 1,
                PageSize = pageSize.HasValue ? pageSize.Value : int.MaxValue - 1,
                IsNeedCount = true
            };

            if (!RT.Service.Resolve<ProcessController>().IsEmpHasProcessSkill(queryInfo.ProcessId, queryInfo.EmployeeId))
                throw new ValidationException("员工[{0}]不具有工序[{1}]所要求的技能！".L10nFormat(employee.Name, process.Name));
            var stations = RT.Service.Resolve<StationController>().GetStationsByResourceId(queryInfo.ResourceId, queryInfo.ProcessId, pagingInfo);
            var result = CreateStationDataInfos(pagingInfo, stations);
            return result;
        }

        /// <summary>
        /// 创建分页工序信息
        /// </summary>
        /// <param name="pagingInfo">分页信息</param>
        /// <param name="processList">工序列表</param>
        /// <returns>分页工序信息</returns>
        private ProcessDataInfo CreateProcessDataInfos(PagingInfo pagingInfo, EntityList<Process> processList)
        {
            ProcessDataInfo result = new ProcessDataInfo()
            {
                PageNumber = pagingInfo.PageNumber,
                PageSize = pagingInfo.PageSize,
                TotalCount = processList.TotalCount
            };
            processList.ForEach(process =>
            {
                var processInfo = new ProcessInfo()
                {
                    Id = process.Id,
                    Name = process.Name,
                    Type = EnumViewModel.EnumToLabel(process.Type).L10N(),
                    EumType = (int)process.Type
                };

                result.ProcessInfos.Add(processInfo);
            });
            return result;
        }

        /// <summary>
        /// 创建分页工位信息
        /// </summary>
        /// <param name="pagingInfo">分页信息</param>
        /// <param name="stations">工位列表</param>
        /// <returns>分页工位信息</returns>
        private StationDataInfo CreateStationDataInfos(PagingInfo pagingInfo, EntityList<Station> stations)
        {
            StationDataInfo result = new StationDataInfo()
            {
                PageNumber = pagingInfo.PageNumber,
                PageSize = pagingInfo.PageSize,
                TotalCount = stations.TotalCount
            };

            stations.ForEach(station =>
            {
                var stationInfo = new StationInfo()
                {
                    Id = station.Id,
                    Code = station.Code,
                    Name = station.Name,
                };

                result.StationInfos.Add(stationInfo);
            });
            return result;
        }

        /// <summary>
        /// 根据产线获取设备台账列表
        /// </summary>
        /// <param name="resourceId">产线ID</param>
        /// <param name="keyword">查询关键字</param>
        /// <param name="pagingInfo">分页</param>
        /// <returns>设备台账列表</returns>
        protected virtual EntityList<EquipAccount> GetEquipAccountsByResourceId(double resourceId, string keyword, PagingInfo pagingInfo)
        {
            Expression<Func<EquipAccount, bool>> exp = p => p.ResourceId == resourceId;
            if (!string.IsNullOrEmpty(keyword))
            {
                exp = p => p.ResourceId == resourceId && (p.Code.Contains(keyword) || p.Name.Contains(keyword));
            }
            var query = Query<EquipAccount>();
            if (exp != null)
            {
                query.Where(exp);
            }
            return query.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 查询设备列表
        /// </summary>
        /// <param name="pagingInfo">分页参数</param>
        /// <param name="keyword">关键字</param>
        /// <returns>设备列表</returns>
        protected virtual EntityList<EquipAccount> GetEquipAccounts(PagingInfo pagingInfo, string keyword)
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
        /// 上传图片
        /// </summary>
        /// <param name="file"></param>
        protected virtual string UploadPhoto(APIModel.AttachmentInfo file)
        {
            if (file == null)
                return "";
            if (file.Content.Split(',').Length > 1 && file.FileName.Split('.').Length > 1)
            {
                var exts = new List<string> { "png", "jpg", "jpeg", "bmp", "gif", "webp", "psd", "svg", "tiff", "jfif" };
                var fileAss = file.FileName.Substring(file.FileName.LastIndexOf(".") + 1);
                if (!exts.Contains(fileAss))
                {
                    throw new ValidationException("只能上传图片格式的文件".L10N());
                }
                var bytes = Convert.FromBase64String(file.Content.Split(',')[1]);
                const string prePath = "AndonManageImage";
                var path = $"{prePath}/{Guid.NewGuid()}";
                RT.Service.Resolve<SIE.Common.Attachments.AttachmentController>().FileStorage(file.FileName, bytes, path);
                file.FliePath = $"{path}/{file.FileName}";
                return file.FliePath;
            }
            return "";
        }

        /// <summary>
        ///获取文件内容
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>

        private string GetAttachmentUrl(string filePath, string fileName)
        {
            string data = string.Empty;
            //文件名或路径为空，退出取值(考虑是否抛异常)
            if (filePath.IsNullOrEmpty() || fileName.IsNullOrEmpty())
            {
                return data;
            }
            try
            {
                var downLoadFileUrl = RT.Service.Resolve<AttachmentController>().GetDownloadPath();
                data = downLoadFileUrl + "/" + filePath;
            }
            catch (Exception)//吃掉下载不成功的异常
            {
                throw new ValidationException("执行失败！".L10N());
            }

            return data;
        }

        /// <summary>
        /// 安灯触发叫料选择仓库
        /// </summary>
        /// <returns></returns>
        [ApiService("安灯触发叫料选择仓库")]
        [return: ApiReturn("仓库信息列表 List<WareInfo>")]
        public virtual List<CallMaterialInfo> AndonTriggerWareInfo()
        {
            var query = Query<Warehouse>().ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            List<CallMaterialInfo> wareInfos = new List<CallMaterialInfo>();
            query.ForEach(w =>
            {
                CallMaterialInfo wareInfo = new CallMaterialInfo
                {
                    Id = w.Id,
                    Code = w.Code,
                    Name = w.Name,
                };
                wareInfos.Add(wareInfo);
            });
            return wareInfos;
        }

        /// <summary>
        /// 安灯触发叫料选择库位
        /// </summary>
        /// <returns></returns>
        [ApiService("安灯触发叫料选择库位")]
        [return: ApiReturn("库位信息列表 List<WareInfo>")]
        public virtual List<CallMaterialInfo> AndonTriggerStorageInfo([ApiParameter("仓库Id")] double? wareId)
        {
            if (wareId == null || wareId == 0)
            {
                throw new ValidationException("请先维护仓库，再选择库位！".L10N());
            }
            var query = Query<StorageLocation>().Where(p => p.WarehouseId == wareId).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            List<CallMaterialInfo> wareInfos = new List<CallMaterialInfo>();
            query.ForEach(w =>
            {
                CallMaterialInfo wareInfo = new CallMaterialInfo
                {
                    Id = w.Id,
                    Code = w.Code,
                    Name = w.Name,
                };
                wareInfos.Add(wareInfo);
            });
            return wareInfos;
        }

        /// <summary>
        /// 感觉产线获取默认线边仓
        /// </summary>
        /// <param name="wipId"></param>
        /// <returns></returns>
        [ApiService("安灯触发叫料初始化线边仓")]
        [return: ApiReturn("线边仓仓库和库位 List<CallMaterialInfo>")]
        public virtual List<CallMaterialInfo> InitDefaultLinWare([ApiParameter("资源Id")] double? wipId)
        {
            if (wipId == null || wipId == 0)
            {
                throw new ValidationException("资源信息错误！".L10N());
            }
            List<CallMaterialInfo> callMaterialInfos = new List<CallMaterialInfo>();
            var lineSideWareHouse = Query<LinesideWarehouse>().Where(p => p.WipResouceId == wipId).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
            if (lineSideWareHouse != null)
            {
                var ware = RF.GetById<Warehouse>(lineSideWareHouse.WarehouseId);
                var storage = RF.GetById<StorageLocation>(lineSideWareHouse.StorageLocationId);
                CallMaterialInfo wareInfo = new CallMaterialInfo
                {
                    Id = ware.Id,
                    Code = ware.Code,
                    Name = ware.Name,
                };
                callMaterialInfos.Add(wareInfo);
                CallMaterialInfo storageInfo = new CallMaterialInfo
                {
                    Id = storage.Id,
                    Code = storage.Code,
                    Name = storage.Name,
                };
                callMaterialInfos.Add(storageInfo);
            }
            return callMaterialInfos;
        }

        /// <summary>
        /// 叫料选择物料
        /// </summary>
        /// <param name="woId"></param>
        /// <param name="wipId"></param>
        /// <returns></returns>
        [ApiService("安灯触发叫料选择物料")]
        [return: ApiReturn("物料 List<CallMaterialInfo>")]
        public virtual List<CallMaterialInfo> ChoseItems([ApiParameter("工单Id")] double? woId, [ApiParameter("资源Id")] double? wipId)
        {
            AndonManageCallMaterial andonManageCallMaterial = new AndonManageCallMaterial
            {
                WorkOrderId = woId,
                ProcessId = wipId,
            };
            var itemEntityList = ChoseItems(andonManageCallMaterial, null, string.Empty);
            List<CallMaterialInfo> items = new List<CallMaterialInfo>();
            itemEntityList.ForEach(item =>
            {
                CallMaterialInfo itemInfo = new CallMaterialInfo
                {
                    Id = item.Id,
                    Code = item.Code,
                    Name = item.Name,
                };
                items.Add(itemInfo);
            });
            return items;
        }
    }
}

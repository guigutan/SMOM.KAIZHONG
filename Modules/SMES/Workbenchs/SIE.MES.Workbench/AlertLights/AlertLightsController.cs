using SIE.Common.Alert;
using SIE.Common.Catalogs;
using SIE.Common.Messages;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Equipments.Abnormal;
using SIE.MES.Workbench.AlertLights;
using SIE.MES.Workbench.AndonAbnormals;
using SIE.MES.WorkBench.AlertLights;
using SIE.Resources.Employees;
using SIE.Resources.Enterprises;
using SIE.Tech.Stations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.MES.Workbench
{
    /// <summary>
    /// 安灯呼叫控制器
    /// </summary>
    public partial class AlertLightsController : DomainController
    {
        /// <summary>
        /// 获取安灯异常实体
        /// </summary>
        /// <param name="employeeId">安灯员工Id</param>
        /// <param name="stationId">安灯工位Id</param>
        /// <param name="processStatusList">处理状态集合</param>
        /// <returns>安灯异常实体</returns>
        public virtual EntityList<AlertLight> GetAlertLights(double? employeeId, double? stationId, List<ProcessStatusType> processStatusList = null)
        {
            if (employeeId <= 0 || employeeId == null)
                throw new ValidationException("员工信息不能为空!".L10N());
            if (stationId <= 0 || stationId == null)
                throw new ValidationException("工位信息不能为空!".L10N());
            var auerys = Query<AlertLight>().Where(p => p.CallEmployeeId == employeeId && p.StationId == stationId);
            if (processStatusList != null && processStatusList.Count > 0)
            {
                List<int> psvalueList = processStatusList.Select(x => (int)x).ToList();
                auerys.Where(p => psvalueList.Contains((int)p.ProcessStatus)); ////&& p.Status == true
            }

            var alertLights = auerys.OrderByDescending(x => x.Id).ToList();
            return alertLights;
        }

        /// <summary>
        /// 获取安灯异常实体
        /// </summary>
        /// <param name="employeeId">安灯员工Id</param>
        /// <param name="stationId">安灯工位Id</param>
        /// <param name="alertType">安灯呼叫类型</param>
        /// <param name="processStatusList">处理状态集合</param>
        /// <returns>安灯异常实体</returns>
        public virtual EntityList<AlertLight> GetAlertLights(double? employeeId, double? stationId, AlertCallType alertType, List<ProcessStatusType> processStatusList = null)
        {
            if (employeeId <= 0 || employeeId == null)
                throw new ValidationException("员工信息不能为空!".L10N());
            if (stationId <= 0 || stationId == null)
                throw new ValidationException("工位信息不能为空!".L10N());
            var querys = Query<AlertLight>().Where(p => p.CallEmployeeId == employeeId && p.StationId == stationId && p.AlertType == alertType);
            if (processStatusList != null && processStatusList.Count > 0)
            {
                List<int> psvalueList = processStatusList.Select(x => (int)x).ToList();
                querys.Where(p => psvalueList.Contains((int)p.ProcessStatus)); ////&& p.Status == true
            }

            var alertLights = querys.OrderByDescending(x => x.Id).ToList();
            return alertLights;
        }

        /// <summary>
        /// 获取安灯呼叫的处理状态
        /// </summary>
        /// <param name="employeeId">安灯员工Id</param>
        /// <param name="stationId">安灯工位Id</param>
        /// <param name="alertType">呼叫类型</param>
        /// <returns>安灯呼叫的处理状态</returns>
        public virtual ProcessStatusType GetAlertLightStatusType(double? employeeId, double? stationId, AlertCallType alertType)
        {
            var qry = GetAlertLights(employeeId, stationId, alertType).FirstOrDefault();
            if (qry == null)
                throw new ValidationException("安灯异常处理状态未找到".L10N());
            else
                return (ProcessStatusType)qry.ProcessStatus;
        }

        /// <summary>
        /// 安灯异常呼叫保存
        /// </summary>
        /// <param name="alertType">安灯呼叫类型</param>
        /// <param name="excepTypeId">安灯异常类型</param>
        /// <param name="employeeId">安灯员工Id</param>
        /// <param name="stationId">安灯工位Id</param>
        /// <param name="workOrderId">工单Id</param>
        /// <param name="productId">产品Id</param>
        public virtual void AlertLightsCallSave(AlertCallType alertType, double excepTypeId, double? employeeId, double? stationId, double? workOrderId, double? productId)
        {
            var curExcepType = RF.GetById<Catalog>(excepTypeId);
            if (curExcepType == null)
                throw new EntityNotFoundException(typeof(Catalog), excepTypeId);
            var employee = RF.GetById<Employee>(employeeId);
            if (employee == null)
                throw new ValidationException("员工Id: [{0}]的信息为空".L10nFormat(employeeId));
            var station = ValidationStation(stationId.Value);
            var entityRepository = station.GetRepository() as EntityRepository;
            var dbNowTime = entityRepository.GetDbTime();
            ////创建安灯呼叫
            var alertLight = CreateAlertLight(alertType, curExcepType, employee, dbNowTime, station);
            ////创建系统消息
            string title = station.Resource.Name + " 发生Andon ".L10N() + alertType.ToLabel() + dbNowTime;
            string content = station.Name + " 发生Andon ".L10N() + curExcepType.Name + dbNowTime;
            var sysMsg = CreateSystemMessage(title, content, employeeId.Value, dbNowTime, AlertLevel.Gently);
            ////创建异常停线
            AbnormalCause abnormalCause = null;
            if (alertType == AlertCallType.Stop)
            {
                abnormalCause = CreateAbnormalCause(dbNowTime, station, workOrderId.Value, productId.Value, curExcepType);
            }

            using (var tran = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                RF.Save(alertLight);
                RF.Save(sysMsg);
                if (abnormalCause != null)
                    RF.Save(abnormalCause);
                tran.Complete();
            }
        }

        /// <summary>
        /// 安灯异常恢复
        /// </summary>
        /// <param name="employeeId">安灯员工Id</param>
        /// <param name="stationId">安灯工位Id</param>
        /// <param name="alertType">呼叫类型</param>
        /// <param name="workOrderId">工单Id</param>
        /// <param name="productId">产品Id(物料Id)</param>
        public virtual void AlertLightsResume(double? employeeId, double? stationId, AlertCallType alertType, double? workOrderId, double? productId)
        {
            var station = ValidationStation(stationId.Value);
            var prcStatusTypes = new List<ProcessStatusType>() { ProcessStatusType.Waitting, ProcessStatusType.Processing };
            var alertLight = GetAlertLights(employeeId, stationId, alertType, prcStatusTypes).FirstOrDefault();
            if (alertLight != null)
            {
                string sysMsgContent = string.Empty;
                var entityRepository = alertLight.GetRepository() as EntityRepository;
                var dbNowTime = entityRepository.GetDbTime();
                ////更新安灯呼叫
                alertLight.RestoreTime = dbNowTime;
                if (alertLight.ProcessStatus == ProcessStatusType.Processing)
                {
                    alertLight.ProcessStatus = ProcessStatusType.Closed;
                    sysMsgContent = "关闭Andon ".L10N();
                }
                else if (alertLight.ProcessStatus == ProcessStatusType.Waitting)
                {
                    alertLight.ProcessStatus = ProcessStatusType.Cancel;
                    sysMsgContent = "取消Andon ".L10N();
                }

                ////创建系统消息
                string title = station.Resource.Name + sysMsgContent + alertType.ToLabel() + dbNowTime;
                string content = station.Name + sysMsgContent + alertLight.ExceptionType.Name + dbNowTime;
                var sysMsg = CreateSystemMessage(title, content, employeeId.Value, dbNowTime, AlertLevel.Gently);
                ////更新异常停线
                AbnormalCause abnormalCause = null; //更新异常停线的结束时间
                if (alertType == AlertCallType.Stop)
                {
                    var abnormalCauseList = RT.Service.Resolve<AbnormalCauseController>().GetAbnormalCauses(ExceptionStopType.StopLine, station.ResourceId, productId, workOrderId);
                    abnormalCause = abnormalCauseList?.FirstOrDefault(x => x.BeginDate == alertLight.TriggerTime && x.EndDate == null);
                    if (abnormalCause != null)
                    {
                        if (alertLight.ProcessStatus == ProcessStatusType.Cancel)
                            abnormalCause.PersistenceStatus = PersistenceStatus.Deleted;
                        else if (alertLight.ProcessStatus == ProcessStatusType.Closed)
                            abnormalCause.EndDate = dbNowTime;
                    }
                }

                using (var tran = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
                {
                    RF.Save(alertLight);
                    RF.Save(sysMsg);
                    if (abnormalCause != null)
                        RF.Save(abnormalCause);
                    tran.Complete();
                }
            }
        }

        /// <summary>
        /// 创建系统消息
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="content">内容</param>
        /// <param name="receiveById">接收人Id</param>
        /// <param name="receiveDate">接收时间</param>
        /// <param name="alertLevel">预警等级</param>
        /// <returns>系统消息</returns>
        private Message CreateSystemMessage(string title, string content, double receiveById, DateTime receiveDate, AlertLevel alertLevel)
        {
            Message sysMsg = new Message();
            sysMsg.Title = title;
            sysMsg.Content = content;
            sysMsg.ReceiveById = receiveById;
            sysMsg.ReceiveDate = receiveDate;
            sysMsg.AlertLevel = alertLevel;
            sysMsg.IsRead = false;
            sysMsg.IsTop = false;

            return sysMsg;
        }

        /// <summary>
        /// 创建安灯或安灯异常实体
        /// </summary>
        /// <param name="alertType">呼叫类型</param>
        /// <param name="excepType">异常类型</param>
        /// <param name="employee">员工</param>
        /// <param name="dbNowTime">数据库NowTime</param>
        /// <param name="station">工位</param>
        /// <returns>安灯或安灯异常实体</returns>
        private AlertLight CreateAlertLight(AlertCallType alertType, Catalog excepType, Employee employee, DateTime dbNowTime, Station station)
        {
            var alertLightHanders = CreateAlertLightHanders(alertType, excepType, employee);
            AlertLight alertLight = new AlertLight();
            alertLight.TriggerTime = dbNowTime;
            alertLight.AlertType = alertType;
            alertLight.ExceptionType = excepType;
            alertLight.ExpTypeName = excepType.Name;
            alertLight.CallEmployeeId = employee.Id;
            alertLight.StationId = station.Id;
            alertLight.AlertLightHanders.AddRange(alertLightHanders);
            alertLight.ProcessStatus = ProcessStatusType.Waitting;
            alertLight.ProductLineId = station.ResourceId;
            return alertLight;
        }

        /// <summary>
        /// 创建异常停线实体
        /// </summary>
        /// <param name="dbNowTime">数据库NowTime</param>
        /// <param name="station">工位</param>
        /// <param name="workOrderId">工位Id</param>
        /// <param name="productId">产品Id</param>
        /// <param name="excepType">异常类型</param>
        /// <returns>异常停线实体</returns>
        private AbnormalCause CreateAbnormalCause(DateTime dbNowTime, Station station, double workOrderId, double productId, Catalog excepType)
        {
            AbnormalCause abnormalCause = new AbnormalCause();
            abnormalCause.ExceptionStopType = ExceptionStopType.StopLine;
            abnormalCause.BeginDate = dbNowTime;
            abnormalCause.ResourceId = station.ResourceId;
            abnormalCause.ShopId = station.Resource.WorkShopId.Value;
            abnormalCause.WorkOrderId = workOrderId;
            abnormalCause.ProductId = productId;
            abnormalCause.AbnormalType = excepType.Code;
            abnormalCause.SourceType = ExceptionStopSourceType.AlertLight;

            return abnormalCause;
        }

        /// <summary>
        /// 检查工位信息是否符合要求
        /// </summary>
        /// <param name="stationId">工位信息</param>
        /// <returns>工位实体</returns>
        private Station ValidationStation(double stationId)
        {
            var station = RF.GetById<Station>(stationId);
            if (station == null)
                throw new EntityNotFoundException(typeof(Station), stationId);
            if (station.Resource == null)
                throw new ValidationException("工位[{0}]的资源不能为空".L10nFormat(station.Code));
            return station;
        }

        /// <summary>
        /// 获取员工呼叫设置信息
        /// </summary>
        /// <param name="alertType">呼叫类型</param>
        /// <param name="exceptionType">异常类型</param>
        /// <param name="workGroupId">班组Id</param>
        /// <returns>员工呼叫设置实体</returns>
        public virtual EmpCallSetting GetEmpCallSet(AlertCallType alertType, Catalog exceptionType, double workGroupId)
        {
            var empCall = Query<EmpCallSetting>().Where(p => p.AlertType == alertType && p.ExceptionTypeId == exceptionType.Id && p.WorkGroupId == workGroupId).FirstOrDefault();
            return empCall;
        }

        /// <summary>
        /// 获取默认的员工呼叫通知
        /// </summary>
        /// <param name="alertType">呼叫类型</param>
        /// <param name="excepType">异常类型</param>
        /// <param name="employee">员工信息</param>
        /// <returns>员工呼叫通知实体</returns>
        public virtual EntityList<EmpCallInform> GetEmpCallInforms(AlertCallType alertType, Catalog excepType, Employee employee)
        {
            if (employee.WorkGroupId == null)
                throw new ValidationException("员工[{0}]的班组信息为空".L10nFormat(employee.Name));
            var empCall = GetEmpCallSet(alertType, excepType, employee.WorkGroupId.Value);
            if (empCall == null || empCall.InformList == null || empCall.InformList.Count == 0)
                throw new ValidationException("班组[{0}]的员工呼叫[{1}]设置信息为空".L10nFormat(employee.WorkGroup.Code, excepType.Name));
            var empCallInforms = empCall.InformList;
            return empCallInforms;
        }

        /// <summary>
        /// 获取安灯处理人员列表
        /// </summary>
        /// <param name="alertType">呼叫类型</param>
        /// <param name="excepType">异常类型</param>
        /// <param name="employee">员工信息</param>
        /// <returns>安灯处理人员列表</returns>
        private EntityList<AlertLightHandler> CreateAlertLightHanders(AlertCallType alertType, Catalog excepType, Employee employee)
        {
            var empCallInforms = GetEmpCallInforms(alertType, excepType, employee);
            var alertLightHanders = new EntityList<AlertLightHandler>();

            foreach (var empCallItem in empCallInforms)
            {
                var alertHanderItem = new AlertLightHandler();
                alertHanderItem.Handler = empCallItem.Employee;
                alertHanderItem.HandlerName = empCallItem.Employee.Name;
                alertLightHanders.Add(alertHanderItem);
            }

            return alertLightHanders;
        }

        /// <summary>
        /// 获取员工呼叫设置实体
        /// </summary>
        /// <param name="wkgpId">班组Id</param>
        /// <param name="pagingInfo">界面分页信息</param>
        /// <returns>员工呼叫设置集合</returns>
        public virtual EntityList<EmpCallSetting> GetEmpCallSettings(double wkgpId, PagingInfo pagingInfo)
        {
            if (wkgpId <= 0)
                throw new ValidationException("班组信息为空".L10N());
            var empCallSettings = Query<EmpCallSetting>().Where(p => p.WorkGroupId == wkgpId).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
            return empCallSettings;
        }

        /// <summary>
        /// 获取安灯异常信息
        /// </summary>
        /// <param name="criteria">安灯异常查询实体</param>
        /// <returns>安灯异常信息</returns>
        public virtual EntityList<AndonAbnormal> GetAndonAbnormals(AndonAbnormalCriteria criteria)
        {
            var querys = Query<AndonAbnormal>();
            if (criteria.ProductLine != null)
                querys.Where(x => x.ProductLineId == criteria.ProductLineId);
            if (criteria.AlertType != null)
                querys.Where(x => x.AlertType == criteria.AlertType);
            if (criteria.ExceptionType != null)
                querys.Where(x => x.ExceptionType == criteria.ExceptionType);
            if (criteria.ProcessStatus != null)
                querys.Where(x => x.ProcessStatus == criteria.ProcessStatus);
            if (criteria.LoginEmpId != null)
                querys.Join<AlertLightHandler>((x, r) => r.AlertLightId == x.Id && r.HandlerId == criteria.LoginEmpId);

            var result = querys.ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
            return result;
        }

        /// <summary>
        /// 安灯异常
        /// </summary>
        /// <param name="curDate">指定日期</param>
        /// <param name="resourceId">指定资源Id</param>
        /// <returns>指定日期、指定资源父组织下的所有产线，安灯异常信息</returns>
        public virtual EntityList<AndonAbnormal> GetAndonAbnormals(DateTime curDate, double resourceId)
        {
            //需要根据产线Id，获取当前车间的所有产线Id(prdLineIds)
            List<double> prdLineIds = GetPrdLineIds(resourceId);
            var andonCtls = Query<AndonAbnormal>().Where(p => p.TriggerTime >= curDate && prdLineIds.Contains(p.ProductLineId)).ToList();
            return andonCtls;
        }

        /// <summary>
        /// 安灯异常信息
        /// </summary>
        /// <param name="curDate">指定日期</param>
        /// <param name="processStatusList">处理状态枚举集合</param>
        /// <param name="workShopId">车间Id</param>
        /// <returns>指定日期、指定车间，安灯异常信息</returns>
        public virtual EntityList<AndonAbnormal> GetAndonAbnormals(DateTime curDate, List<ProcessStatusType> processStatusList = null, double? workShopId = null)
        {
            var querys = Query<AndonAbnormal>().Where(p => p.TriggerTime >= curDate);
            if (processStatusList != null && processStatusList.Count > 0)
            {
                List<int> psvalueList = processStatusList.Select(x => (int)x).ToList();
                querys.Where(p => psvalueList.Contains((int)p.ProcessStatus));
            }

            if (workShopId.HasValue)
                querys.Where(p => p.ProductLine.WorkShopId == workShopId);

            var andonCtls = querys.OrderByDescending(x => x.TriggerTime).ToList();
            return andonCtls;
        }

        /// <summary>
        /// 安灯异常个数
        /// </summary>
        /// <param name="curDate">指定日期</param>
        /// <param name="processStatusList">处理状态枚举集合</param>
        /// <param name="workShopId">车间Id</param>
        /// <returns>指定日期、指定车间，安灯异常个数</returns>
        public virtual int GetAndonAbnormalCount(DateTime curDate, List<ProcessStatusType> processStatusList = null, double? workShopId = null)
        {
            var andonCount = GetAndonAbnormals(curDate, processStatusList, workShopId).Count();
            return andonCount;
        }

        /// <summary>
        /// 待实现方法--根据产线Id，获父组织的所有产线资源
        /// </summary>
        /// <param name="resourceId">产线Id</param>
        /// <returns>所有产线Id</returns>
        private List<double> GetPrdLineIds(double resourceId)
        {
            List<double> prdLineIds = null;
            var resources = RT.Service.Resolve<EnterpriseController>().GetLineResources(resourceId);
            prdLineIds = resources.Select(p => p.Id).ToList();
            return prdLineIds;
        }

        /// <summary>
        /// 安灯异常签到命令处理
        /// </summary>
        /// <param name="entityId">安灯异常实体Id</param>
        /// <param name="empId">用户Id</param>
        public virtual void SignAndonAbnormal(double entityId, double empId)
        {
            var curEmp = RF.GetById<Employee>(empId);
            if (curEmp == null)
                throw new EntityNotFoundException(typeof(Employee), empId);
            var andonAbnl = RF.GetById<AndonAbnormal>(entityId);
            if (andonAbnl == null)
                throw new EntityNotFoundException(typeof(AndonAbnormal), entityId);
            var entityRepository = andonAbnl.GetRepository() as EntityRepository;
            var dbNowTime = entityRepository.GetDbTime();
            ////更新安灯异常
            andonAbnl.ProcessStatus = ProcessStatusType.Processing;
            andonAbnl.SignTime = dbNowTime;
            andonAbnl.ProcessEmployee = curEmp;
            andonAbnl.ProcessEmployeeId = empId;

            ////创建系统消息
            string title = andonAbnl.Station.Resource.Name + andonAbnl.AlertType.ToLabel() + " Andon处理人员签到 ".L10N() + dbNowTime;
            string content = andonAbnl.Station.Name + andonAbnl.ExceptionType.Name + " Andon处理人员 " + andonAbnl.ProcessEmployee.Name + "签到".L10N() + dbNowTime;
            var sysMsg = CreateSystemMessage(title, content, empId, dbNowTime, AlertLevel.Gently);

            using (var tran = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                RF.Save(andonAbnl);
                RF.Save(sysMsg);
                tran.Complete();
            }
        }
    }
}

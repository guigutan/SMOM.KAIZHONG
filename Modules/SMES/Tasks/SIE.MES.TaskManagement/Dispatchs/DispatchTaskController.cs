using DocumentFormat.OpenXml.Drawing;
using DotLiquid.Util;
using IronPython.Runtime.Operations;
using SIE.Core.ApiModels;
using SIE.Core.Enums;
using SIE.Domain;
using SIE.EventMessages;
using SIE.Items;
using SIE.KZ.Base.Interfaces;
using SIE.MES.TaskManagement.Reports;
using SIE.Rbac.InvOrgs;
using SIE.Resources.Enterprises;
using SIE.Tech.Processs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.TaskManagement.Dispatchs
{
    /// <summary>
    /// 
    /// </summary>
    public partial class DispatchTaskController : DomainController
    {

        /// <summary>
        /// 获取派工任务产线
        /// </summary>
        /// <param name="wipResourceIds"></param>
        /// <returns></returns>
        public virtual List<double> GetSchedulingWipResourceIds(List<double> wipResourceIds)
        {
            var ids = wipResourceIds.ConvertAll(d => (double?)d);
            var curTime = RF.Find<DispatchTask>().GetDbTime();
            //待派工/派工中/已派工/执行中/暂停/已完成的任务单
            var taskResourceId = Query<DispatchTask>()
                .Where(p => ids.Contains(p.ResourceId) && p.PlanBeginTime < curTime && p.PlanEndTime > curTime && (p.TaskStatus == DispatchTaskStatus.ToDispatch || p.TaskStatus == DispatchTaskStatus.Dispatching || p.TaskStatus == DispatchTaskStatus.Dispatched || p.TaskStatus == DispatchTaskStatus.Executing || p.TaskStatus == DispatchTaskStatus.Pause || p.TaskStatus == DispatchTaskStatus.Finished))
                .Select(p => p.ResourceId).ToList<double>();

            return taskResourceId.ToList();
        }

        /// <summary>
        /// 获取工序为“料加工”的派工单状态为待派工、执行中、暂停、派工中的任务
        /// </summary>
        /// <param name="productIds"></param>
        /// <returns></returns>
        public virtual EntityList<DispatchTask> GetDispatchTasks(List<double> productIds)
        {
            var query = Query<DispatchTask>().Where(p => productIds.Contains(p.ProductId)
                && (p.TaskStatus == DispatchTaskStatus.ToDispatch || p.TaskStatus == DispatchTaskStatus.Dispatching ||
                p.TaskStatus == DispatchTaskStatus.Executing || p.TaskStatus == DispatchTaskStatus.Pause)
                && p.PlanBeginTime > DateTime.Now && p.PlanBeginTime < DateTime.Now.AddDays(3));
            query.Exists<Process>((d, p) => p.Where(w => w.Id == d.ProcessId && w.Name == "精加工"));
            var tasks = query.ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            return tasks;
        }

        /// <summary>
        /// 获取派工任务列表
        /// </summary>
        /// <param name="mrps"></param>
        /// <param name="processIds"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public virtual Dictionary<string, EntityList<DispatchTask>> GetDispatchTaskList(List<DictionaryData> mrps, List<DictionaryData> dicProcessCodes, DateTime? startTime, DateTime? endTime)
        {
            Dictionary<string, EntityList<DispatchTask>> dispatchTaskList = new Dictionary<string, EntityList<DispatchTask>>();
            var invCurr = RT.InvOrg;
            foreach (var item in mrps)
            {
                var invOrg = Query<InvOrg>().Where(p => p.ExternalId == item.DicKey).FirstOrDefault();
                if (invOrg == null)
                    continue;
                RT.InvOrg = invOrg.Code;
                var processList = RT.Service.Resolve<ProcessController>().GetProcessesList(dicProcessCodes.Where(p=>p.DicKey == item.DicKey).FirstOrDefault()?.DicValue);
                var processIds = processList.Select(p => p.Id).ToList();
                var ids = processIds.ConvertAll(d => (double?)d);

                var query = Query<DispatchTask>().LeftJoin<Enterprise>((d, i) => d.WorkShopId == i.Id)
                    .Where<Enterprise>((d, i) => item.DicValue.Contains(i.Code) && ids.Contains(d.ProcessId));
                if (startTime != null)
                    query.Where(p => p.PlanBeginTime >= startTime);
                if (endTime != null)
                    query.Where(p => p.PlanEndTime <= endTime);
                var entityList = query.ToList(null, new EagerLoadOptions().LoadWithViewProperty());
                dispatchTaskList.Add(item.DicKey, entityList);
            }
            RT.InvOrg = invCurr;
            return dispatchTaskList;
        }

        /// <summary>
        /// 获取派工任务列表
        /// </summary>
        /// <param name="dateType"></param>
        /// <param name="dicprocessId"></param>
        /// <returns></returns>
        public virtual Dictionary<string, EntityList<DispatchTask>> GetDispatchTaskList(DateTime? startTime, DateTime? endTime, List<DictionaryData> dicpProcessCodes)
        {
            Dictionary<string, EntityList<DispatchTask>> dispatchTaskList = new Dictionary<string, EntityList<DispatchTask>>();
            var invCurr = RT.InvOrg;
            foreach (var item in dicpProcessCodes)
            {
                var invOrg = Query<InvOrg>().Where(p => p.ExternalId == item.DicKey).FirstOrDefault();
                if (invOrg == null)
                    continue;
                RT.InvOrg = invOrg.Code;

                var query = Query<DispatchTask>()
                    .LeftJoin<Process>((d, p) =>d.ProcessId==p.Id)
                    .Where<Process>((d, p) => item.DicValue.Contains(p.Code));
                query.Where(p => p.PlanBeginTime >= startTime);
                query.Where(p => p.PlanBeginTime <= endTime);
                var entityList = query.ToList(null, new EagerLoadOptions().LoadWithViewProperty());
                dispatchTaskList.Add(item.DicKey, entityList);
            }
            RT.InvOrg = invCurr;
            return dispatchTaskList;
        }
    }
}

using SIE.Andon.Andons;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MES.TaskManagement.Dispatchs;
using SIE.Resources.WipResources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.TaskManagement.IOT.Data
{
    public class AxisChangeRecordController : DomainController
    {
        /// <summary>
        /// IOT押出换轴记录查询
        /// </summary>
        /// <param name="criterial"></param>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        public virtual EntityList<AxisChangeRecord> GetAxisChangeRecords(AxisChangeRecordCriterial criterial)
        {
            if (criterial == null)
            {
                throw new ValidationException("IOT押出换轴记录查询实体异常！".L10N());
            }
            //var queryDatas = Query<AxisChangeRecord>().Where(p => p.DispatchTaskId == null).ToList();
            //var list = queryDatas.Select(p => p.TaskNo).ToList();
            //if (queryDatas.Count > 0)
            //{
            //    EntityList<AxisChangeRecord> axisList = new EntityList<AxisChangeRecord>();
            //    var dispatchTask = Query<DispatchTask>().Where(p => list.Contains(p.No)).ToList();
            //    foreach (var item in queryDatas)
            //    {
            //        item.DispatchTaskId = dispatchTask.FirstOrDefault(p => p.No == item.TaskNo)?.Id;
            //        axisList.Add(item);
            //    }
            //    RF.Save(axisList);
            //}

            var query = Query<AxisChangeRecord>();
            if (!criterial.IotEntity.IsNullOrEmpty())
            {
                query.Where(p => p.IotEntity.Contains(criterial.IotEntity));
            }
            if (criterial.ChangeFlag.HasValue)
            {
                query.Where(p => p.ChangeFlag == criterial.ChangeFlag);
            }
            if (!criterial.TaskNo.IsNullOrEmpty())
            {
                query.Where(p => p.TaskNo.Contains(criterial.TaskNo));
            }
            if (criterial.IsReport.HasValue)
            {
                query.Where(p => p.IsReport == criterial.IsReport);
            }
            //if (!criterial.ResourceCode.IsNullOrEmpty())
            //{
            //    query.Where(p => p.DispatchTask.Resource.Code.Contains(criterial.ResourceCode));
            //}
            //if (!criterial.ResourceName.IsNullOrEmpty())
            //{
            //    query.Where(p => p.DispatchTask.Resource.Name.Contains(criterial.ResourceName));
            //}

            if (criterial.CollectionTime.BeginValue.HasValue)
            {
                query.Where(p => p.CollectionTime >= criterial.CollectionTime.BeginValue.Value);
            }
            if (criterial.CollectionTime.EndValue.HasValue)
            {
                query.Where(p => p.CollectionTime <= criterial.CollectionTime.EndValue.Value);
            }

            var taskSql = string.Empty;

            if (!criterial.ResourceCode.IsNullOrEmpty())
            {
                var symbol = "=";
                if (criterial.ResourceCode.Contains("%"))
                    symbol = "like";

                if (taskSql.IsNullOrEmpty())
                {
                    taskSql = " Wip.Code {0} '{1}'".L10nFormat(symbol, criterial.ResourceCode);
                }
                else
                {
                    taskSql += " and Wip.Code {0} '{1}'".L10nFormat(symbol, criterial.ResourceCode);
                }
            }
            if (!criterial.ResourceName.IsNullOrEmpty())
            {
                var symbol = "=";
                if (criterial.ResourceName.Contains("%"))
                    symbol = "like";

                if (taskSql.IsNullOrEmpty())
                {
                    taskSql = " Wip.Name {0} '{1}'".L10nFormat(symbol, criterial.ResourceName);
                }
                else
                {
                    taskSql += " and Wip.Name {0} '{1}'".L10nFormat(symbol, criterial.ResourceName);
                }
            }
            if (!taskSql.IsNullOrEmpty())
            {
                query.Join<DispatchTask>((x, y) => x.TaskNo == y.No).Join<DispatchTask, WipResource>(("Wip"),(x, y) => x.ResourceId == y.Id && y.SQL<bool>(taskSql));
            }

            var list = query.OrderBy(criterial.OrderInfoList).ToList(criterial.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());

            var taskNos = list.Select(p => p.TaskNo).Distinct().ToList();
            var tasks = taskNos.SplitContains(nos =>
             {
                 return Query<DispatchTask>().Where(p => nos.Contains(p.No)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
             });
            foreach (var l in list)
            {
                var task = tasks.FirstOrDefault(p => p.No == l.TaskNo);
                if (task != null)
                {
                    l.ResourceCode = task.ResourceCode;
                    l.ResourceName = task.ResourceName;
                }
            }

            return list;
        }
    }
}

using Castle.Core.Internal;
using DocumentFormat.OpenXml.Office2010.Ink;
using SIE.Andon.AndonAverageTime.Datas;
using SIE.Andon.Andons;
using SIE.Andon.Andons.Enum;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SIE.Andon.AndonAverageTime
{
    /// <summary>
    /// 安灯平均时长 控制器
    /// </summary>
    public class AndonAverageTimeController : DomainController
    {
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public virtual EntityList<AndonAverageTimeViewModel> GetAndonAverageTimes(AndonAverageTimeViewModelCriteria criteria)
        {
            EntityList<AndonAverageTimeViewModel> result = new EntityList<AndonAverageTimeViewModel>();

            //查询数据  MES_ANDONMANAGE    
            var queryAndonManage = Query<AndonManage>();

            if (criteria.CreateTime.BeginValue.HasValue) { queryAndonManage = queryAndonManage.Where(p => p.CreateDate >= criteria.CreateTime.BeginValue); }
            if (criteria.CreateTime.EndValue.HasValue) { queryAndonManage = queryAndonManage.Where(p => p.CreateDate <= criteria.CreateTime.EndValue); }
            var whereByUserName = string.Empty;
            if (criteria.ByUserId != null) { whereByUserName = Query<Employee>().Where(p => p.Id == criteria.ByUserId).FirstOrDefault().Name; }


            queryAndonManage.Exists<Resources.Employees.EmployeeEnterprise>((x, y) => y.Where(p => p.EmployeeId == RT.IdentityId && p.EnterpriseId == x.FactoryId));
            var sourceList = queryAndonManage.ToList();


            var ids = sourceList.Select(p => p.Id).Distinct().ToList();
            var logs = ids.SplitContains(temp =>
            {
                return Query<AndonManageOperateLog>().Where(p => temp.Contains(p.AndonManageId)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });


            foreach (var item in sourceList)
            {
                //响应
                item.Responser = logs.Where(p => p.OperateType == AndonManageOperateType.Response && p.AndonManageId == item.Id).OrderByDescending(p => p.CreateDate).FirstOrDefault()?.OperaterName;
                item.ResponseTime = logs.Where(p => p.OperateType == AndonManageOperateType.Response && p.AndonManageId == item.Id).OrderByDescending(p => p.CreateDate).FirstOrDefault()?.OperateTime;
                item.ResponseHour = item.ResponseTime == null ? 0 : Math.Round((decimal)(item.ResponseTime.Value - item.TriggerTime).TotalMinutes, 2);
                //处理
                item.Handler2 = logs.Where(p => p.OperateType == AndonManageOperateType.Handle && p.AndonManageId == item.Id).OrderByDescending(p => p.CreateDate).FirstOrDefault()?.OperaterName;
                item.HandleTime = logs.Where(p => p.OperateType == AndonManageOperateType.Handle && p.AndonManageId == item.Id).OrderByDescending(p => p.CreateDate).FirstOrDefault()?.OperateTime;
                item.HandleHour = item.HandleTime == null || item.ResponseTime == null ? 0 : Math.Round((decimal)(item.HandleTime.Value - item.ResponseTime.Value).TotalMinutes, 2);
                //验收
                item.Checker = logs.Where(p => p.OperateType == AndonManageOperateType.Check && p.AndonManageId == item.Id).OrderByDescending(p => p.CreateDate).FirstOrDefault()?.OperaterName;
                item.CheckTime = logs.Where(p => p.OperateType == AndonManageOperateType.Check && p.AndonManageId == item.Id).OrderByDescending(p => p.CreateDate).FirstOrDefault()?.OperateTime;
                item.CheckHour = item.CheckTime == null || item.HandleTime == null ? 0 : Math.Round((decimal)(item.CheckTime.Value - item.HandleTime.Value).TotalMinutes, 2);
                //持续时长
                item.Duration = (item.HandleHour ?? 0) + (item.ResponseHour ?? 0) + (item.CheckHour ?? 0);
            }

            // 提取每个安灯单的参与人及有效时长（分钟）
            var averageDataList = sourceList.Select(item => new
            {
                Responser = item.Responser ?? string.Empty,
                ResponseDuration = item.ResponseHour ?? 0m,

                Handler2 = item.Handler2 ?? string.Empty,
                HandleDuration = item.HandleHour ?? 0m,

                Checker = item.Checker ?? string.Empty,
                CheckDuration = item.CheckHour ?? 0m

            }).ToList();

            // 提取所有参与人并去重
            var allUsers = averageDataList
                .SelectMany(d => new[] { d.Responser, d.Handler2, d.Checker })
                .Where(u => !string.IsNullOrWhiteSpace(u))
                .Distinct()
                .ToList();

            //查询条件
            if (!string.IsNullOrEmpty(whereByUserName)) { allUsers = new List<string> { whereByUserName }; }

            //按人统计平均时长
            foreach (var user in allUsers)
            {
                // 响应平均时长
                var responseMinutes = averageDataList.Where(x => x.Responser == user).Select(x => x.ResponseDuration);
                decimal avgResponseMinutes = responseMinutes.Any() ? responseMinutes.Average() : 0m;

                // 处理平均时长
                var handleMinutes = averageDataList.Where(x => x.Handler2 == user).Select(x => x.HandleDuration);
                decimal avgHandleMinutes = handleMinutes.Any() ? handleMinutes.Average() : 0m;

                // 验收平均时长
                var checkMinutes = averageDataList.Where(x => x.Checker == user).Select(x => x.CheckDuration);
                decimal avgCheckMinutes = checkMinutes.Any() ? checkMinutes.Average() : 0m;

                var vm = new AndonAverageTimeViewModel
                {
                    ByUserName = user,
                    ResponseDurationAverage = avgResponseMinutes,
                    HandleDurationAverage = avgHandleMinutes,
                    CheckDurationAverage = avgCheckMinutes
                };
                result.Add(vm);
            }

            return result;
        }
    }
}
using SIE.Api;
using SIE.Common.InvOrg;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MES.OnOffDuty;
using SIE.MES.WIP;
using SIE.Resources.Employees;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.MES.ForWinform
{

    /// <summary>
    /// 
    /// </summary>
    public class WinFormOnOffDutyController : OnOffDutyController
    {

        /// <summary>
        /// 获取记录明细模型
        /// </summary>
        /// <param name="onOffDutyRecrods"></param>
        /// <returns></returns>
        private OnOffDutyCollectDetailViewModel GetDetailModel(OnOffDutyRecrods onOffDutyRecrods)
        {
            return new OnOffDutyCollectDetailViewModel
            {
                OnOffDutyType = onOffDutyRecrods.OnOffDutyType,
                CollectUseName = RT.Identity.Name,
                InputDate = DateTime.Now,
                CollectDate = DateTime.Now,
                ProcessName = onOffDutyRecrods.Process.Name,
                StationName = onOffDutyRecrods.Station.Name,
                StaffNO = onOffDutyRecrods.Employee.Code,
                StaffName = onOffDutyRecrods.Employee.Name,
                ResourceName = onOffDutyRecrods.Resource.Name

            };
        }


        /// <summary>
        /// 上下岗执行
        /// </summary>
        /// <param name="barcode"></param>
        /// <param name="isOnDuty"></param>
        /// <param name="workcell"></param>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        [ApiService("上下岗执行")]
        public virtual OnOffDutyCollectDetailViewModel OnOffDuty([ApiParameter("用户扫描的条码")] string barcode, [ApiParameter("是否是上岗")] bool isOnDuty,
            [ApiParameter("工作单元")] Workcell workcell)
        {
            var staff = RT.Service.Resolve<EmployeeController>().GetEmployeeByCode(barcode);
            if (staff == null)
            {
                throw new ValidationException("系统不存在当前扫描员工".L10N());

            }
            var onoffDutyRecord = new OnOffDutyRecrods();
            onoffDutyRecord.ProcessId = workcell.ProcessId;
            onoffDutyRecord.EmployeeId = staff.Id;
            onoffDutyRecord.StationId = workcell.StationId;
            onoffDutyRecord.ResourceId = workcell.ResourceId;
            onoffDutyRecord.OnOffDutyType = isOnDuty ? OnOffDutyType.OnDuty : OnOffDutyType.OffDuty;
            this.OnOffDuty(onoffDutyRecord, workcell, isOnDuty);
            return this.GetDetailModel(onoffDutyRecord);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="keyWord"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageNum"></param>
        /// <returns></returns>
        [ApiService("查询上岗、下岗补录数据")]

        public virtual Tuple<int, List<OnOffDutyRecrods>> FetchOnOffDutyRecrodsForinput([ApiParameter("搜索关键字")] string keyWord, [ApiParameter("页行数")] int pageSize = 20, [ApiParameter("页数")] int pageNum = 1)
        {
            var q = Query<Employee>().LeftJoin<EmployeeGroup>((x, y) => x.EmployeeGroupId == y.Id);
            //员工
            if (keyWord.IsNotEmpty())
            {
                q.Where(p => p.Code.Contains(keyWord) || p.Name.Contains(keyWord));
            }
            var pagingInfo = new PagingInfo(pageNum, pageSize, true);
            EntityList<Employee> employeeReuslt;
            using (InvOrgs.WithAll())
            {
                employeeReuslt = q.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
            }
            List<OnOffDutyRecrods> onOffDutyRecrodsList = new List<OnOffDutyRecrods>();
            foreach (var item in employeeReuslt)
            {
                onOffDutyRecrodsList.Add(new OnOffDutyRecrods
                {
                    EmployeeId = item.Id,
                    EmployeeCode = item.Code,
                    EmployeeName = item.Name,
                    EmployeeGroupName = item.EmployeeGroupName,
                    UserCode = item.UserCode,
                });
            }
            return new Tuple<int, List<OnOffDutyRecrods>>(employeeReuslt.TotalCount, onOffDutyRecrodsList);
        }

        /// <summary>
        /// 补录上下岗信息
        /// </summary>
        /// <param name="onOffDutyRecrods"></param>
        /// <exception cref="ValidationException"></exception>
        [ApiService("补录上下岗信息")]
        public virtual void SetOnOffDutyRecrods([ApiParameter("用户所选数据")]  List<OnOffDutyRecrods> onOffDutyRecrods)
        {

            EntityList<OnOffDutyRecrods> selectedList = new EntityList<OnOffDutyRecrods>();
            foreach (var item in onOffDutyRecrods)
            {
                var selected = item as OnOffDutyRecrods;
                if (selected != null)
                {
                    if (!selected.OnDutyTime.HasValue)
                    {
                        throw new ValidationException("员工号【{0}】上岗补录时间必输！".L10nFormat(selected.EmployeeName));
                    }
                    if (!selected.OffDutyTime.HasValue)
                    {

                        throw new ValidationException("员工号【{0}】下岗补录时间必输！".L10nFormat(selected.EmployeeName));
                    }
                    if (selected.OffDutyTime <= selected.OnDutyTime)
                    {
                        throw new ValidationException("员工号【{0}】上岗补录时间不能早于下岗补录时间！".L10nFormat(selected.EmployeeName));
                    }
                    if (DateTime.Now <= selected.OffDutyTime)
                    {
                        throw new ValidationException("员工号【{0}】下岗补录时间不能早于当前时间!".L10nFormat(selected.EmployeeName));
                    }
                    selected.OnDutyDuration = (selected.OffDutyTime - selected.OnDutyTime).Value.TotalMinutes;
                    selected.OnOffDutyType = OnOffDutyType.OffDuty;
                    selected.IsAdditionalRecording = true;
                    selected.PersistenceStatus = PersistenceStatus.New;
                    selectedList.Add(selected);
                }
            }
            if (selectedList.Any())
            {
                RT.Service.Resolve<OnOffDutyController>().SetOnOffDuty(selectedList);
            }
            else
            {
                throw new ValidationException("请选择需要补录的员工!".L10N());
            }
        }
    }
}

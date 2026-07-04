using SIE.Domain;
using SIE.MES.DashBoard.DashBoards.WorkShop;
using SIE.MES.WIP.Products;
using SIE.MES.WorkOrders;
using SIE.Rbac.InvOrgs;
using SIE.Resources.WipResources;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.MES.DashBoard.DashBoards.ProductionLine
{
    /// <summary>
    /// 车间看板控制器
    /// </summary>
    public partial class WorkShopController : DomainController
    {
        /// <summary>
        /// 根据工厂获取安全生产天数
        /// </summary>
        /// <param name="factory"></param>
        /// <returns></returns>
        public virtual WorkSafety GetWorkSafetyByFactory(string factory)
        {
            WorkSafety workSafety = Query<WorkSafety>().Where(p => p.Factory.ExternalId == factory).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
            return workSafety;
        }

        /// <summary>
        /// 获取工作小时数
        /// </summary>
        /// <param name="resouceId">产线Id</param>
        /// <returns>工作小时数</returns>
        public virtual double GetworkHours(double resouceId)
        {
            double workHours = 0; //工作小时数
            DateTime currentDay = DateTime.Now.Date;
            DateTime currentTime = Convert.ToDateTime(DateTime.Now.ToLongTimeString());
            var shiftType = RT.Service.Resolve<WipResourceController>().GetWipResourceShiftType(resouceId, currentDay);
            if (shiftType != null)
            {
                double shiftHour = 0; //班制总时间
                double shiftRestHour = 0; //班制休息总时间
                foreach (var shift in shiftType.ShiftList)
                {
                    if (currentTime >= Convert.ToDateTime(shift.BeginTime.ToLongTimeString()) && currentTime < Convert.ToDateTime(shift.EndTime.ToLongTimeString()))
                    {
                        #region 班制时间,目前不考虑跨日
                        TimeSpan ts = currentTime.Subtract(Convert.ToDateTime(shift.BeginTime.ToLongTimeString()));
                        shiftHour += ts.TotalHours;
                        #endregion

                        #region 休息时间
                        foreach (var shiftRest in shift.ShiftRestList)
                        {
                            TimeSpan rests = new TimeSpan();
                            if (currentTime >= Convert.ToDateTime(shiftRest.EndTime.ToLongTimeString()))
                            {
                                rests = Convert.ToDateTime(shiftRest.EndTime.ToLongTimeString()).Subtract(Convert.ToDateTime(shiftRest.BeginTime.ToLongTimeString()));
                            }
                            else if (currentTime >= Convert.ToDateTime(shiftRest.BeginTime.ToLongTimeString()) && currentTime < Convert.ToDateTime(shiftRest.EndTime.ToLongTimeString()))
                            {
                                rests = currentTime.Subtract(Convert.ToDateTime(shiftRest.BeginTime.ToLongTimeString()));
                            }
                            else
                            {
                                //
                            }

                            shiftRestHour += rests.TotalHours;
                        }
                        #endregion
                    }
                }

                workHours = shiftHour - shiftRestHour;
            }

            return workHours;
        }

        /// <summary>
        /// 获取不良数
        /// </summary>
        /// <param name="workorder">工单实体</param>
        /// <returns>不良数</returns>
        public virtual decimal GetNgQty(List<WorkOrder> workorder)
        {
            decimal ngQty = 0;
            var woIdList = workorder.Select(p => p.Id);
            var wipProductVersions = Query<WipProductVersion>().Where(p => woIdList.Contains(p.WorkOrderId)).ToList();
            foreach (var wipProductVersion in wipProductVersions)
            {
                if (wipProductVersion.DefectList.Count > 0)
                {
                    ngQty += 1;
                }
            }

            return ngQty;
        }

        /// <summary>
        /// 获取库存组织
        /// </summary>
        /// <returns></returns>
        public virtual EntityList<InvOrg> GetInvOrgs()
        {
           return Query<Rbac.InvOrgs.InvOrg>().ToList();
        }

        /// <summary>
        /// 按照生产设备获取设备台账维护
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public virtual EntityList<InvOrg> GetInvOrgs(PagingInfo info, string code)
        {
            var equipType = Query<InvOrg>().Where(p => p.Code == RT.InvOrg);
           
            return equipType.ToList(info, new EagerLoadOptions().LoadWithViewProperty());
        }
    }
}

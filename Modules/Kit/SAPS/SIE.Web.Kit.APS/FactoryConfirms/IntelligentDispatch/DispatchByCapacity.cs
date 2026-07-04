using SIE.Domain;
using SIE.Kit.APS.FactoryConfirms;
using SIE.Kit.APS.TargetCapacitys;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Web.Kit.APS.FactoryConfirms
{
    /// <summary>
    /// 智能分厂 根据产能面积
    /// </summary>
    public class DispatchByCapacity : DispatchBase
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="lstData">销售单明细数据</param>
        public DispatchByCapacity(List<FactoryConfirmsViewModel> lstData) : base(lstData) { }

        /// <summary>
        /// 目标产能配置表
        /// </summary>
        private List<TargetCapacity> LstTC { get; set; }

        /// <summary>
        /// 历史产能
        /// </summary>
        private Dictionary<string, List<FactoryConfirmsViewModel2>> DicHistoryCapacity { get; set; }

        /// <summary>
        /// 数据初始化
        /// </summary>
        public override void DataInit()
        {
            //年份
            List<string> lstYear = new List<string>();
            foreach (var data in this.LstData)
            {
                if (data.PromiseDelivery != null)////承诺交期
                    lstYear.Add(data.PromiseDelivery.Value.Year.ToString());
                else//k客户交期
                    lstYear.Add(data.RequireDelivery.Year.ToString());
            }
            lstYear = lstYear.Distinct().ToList();
            var ctrl = RT.Service.Resolve<TargetCapacityController>();
            this.LstTC = ctrl.GetYearTargetCapacity(lstYear).ToList();

            var ctrl2 = RT.Service.Resolve<FactoryConfirmsController>();
            var lst = ctrl2.GetConfirmsViewModel2(lstYear).ToList();
            var toDoIds = this.LstData.Select(x => x.Id).ToList();
            lst.RemoveAll(x => toDoIds.Contains(x.Id));//删除已经确认要重新分派的单，目的是要清掉准备重新分派的单所占用的产能
            //根据日期分组
            var gs = lst.GroupBy(x => GroupByDate(x.PromiseDelivery, x.RequireDelivery).ToString("yyyy-MM"));
            this.DicHistoryCapacity = new Dictionary<string, List<FactoryConfirmsViewModel2>>();
            foreach (var g in gs)
            {
                this.DicHistoryCapacity[g.Key] = g.ToList();
            }
        }

        /// <summary>
        /// 分派完后刷新基础数据（重新占用产能）
        /// </summary>
        /// <param name="data">已分派的数据</param>
        public override void RefreshData(FactoryConfirmsViewModel data)
        {
            DateTime dt = GroupByDate(data.PromiseDelivery, data.RequireDelivery);// data.PromiseDelivery == null ? data.RequireDelivery : data.PromiseDelivery.Value;
            string dateStr = dt.ToString("yyyy-MM");
            List<FactoryConfirmsViewModel2> lst = this.DicHistoryCapacity.ContainsKey(dateStr) ? this.DicHistoryCapacity[dateStr] : new List<FactoryConfirmsViewModel2>();
            var newData = new FactoryConfirmsViewModel2() { Id = data.Id, Area = data.Area, ItemId = data.ItemId, LineNo = data.LineNo, EnterpriseId = data.EnterpriseId, LineState = data.LineState, PromiseDelivery = data.PromiseDelivery, RequireDelivery = data.RequireDelivery };
            if (this.DicHistoryCapacity.ContainsKey(dateStr))
                this.DicHistoryCapacity[dateStr].Add(newData);
            else
                this.DicHistoryCapacity[dateStr] = new List<FactoryConfirmsViewModel2>() { newData };
        }

        /// <summary>
        /// 工厂分配
        /// </summary>
        /// <param name="data">销售单行</param>
        /// <param name="lstFo">可分配的工厂（库存组织）</param>
        /// <returns></returns>
        public override List<double> Dispatch(FactoryConfirmsViewModel data, List<double> lstFo)
        {
            DateTime dt = GroupByDate(data.PromiseDelivery, data.RequireDelivery);// data.PromiseDelivery == null ? data.RequireDelivery : data.PromiseDelivery.Value;
            string dateStr = dt.ToString("yyyy-MM");
            List<FactoryConfirmsViewModel2> lst = this.DicHistoryCapacity.ContainsKey(dateStr) ? this.DicHistoryCapacity[dateStr] : new List<FactoryConfirmsViewModel2>();
            lst.RemoveAll(x => x.Id == data.Id);//清掉当前的数据

            List<FactoryLoading> lstFoLoading = new List<FactoryLoading>();
            foreach (var fo in lstFo)
            {
                var foDetail = lst.Where(x => x.EnterpriseId == fo).ToList();

                lstFoLoading.Add(new FactoryLoading()
                {
                    FoID = fo,
                    Loading = data.Area,
                    HistoryLoading = foDetail.Sum(x => x.Area),
                    TargetCapacity = this.GetTargetCapacity(fo, dt)
                });
            }
            List<FactoryLoading> result = lstFoLoading.Where(x => x.TargetCapacity - x.HistoryLoading - x.Loading > 0).ToList();
            if (result.Count > 0)//有满足产能的工厂时，按占用剩余比例越小的优先
                return result.OrderBy(x => x.Loading / (x.TargetCapacity - x.HistoryLoading)).Select(x => x.FoID).ToList();

            //没有满足产能面积的话，按剩余最多的排序，全部返回
            return result.OrderByDescending(x => (x.TargetCapacity - x.HistoryLoading)).Select(x => x.FoID).ToList();
        }

        /// <summary>
        /// 根据日期月份 获取目标产能
        /// </summary>
        /// <param name="foId">工厂ID</param>
        /// <param name="date">日期</param>
        /// <returns></returns>
        public decimal GetTargetCapacity(double foId, DateTime date)
        {
            var tc = this.LstTC.FirstOrDefault(x => x.EnterpriseId == foId && x.Year == date.Year.ToString());
            if (tc == null)
                return 0;
            decimal result = 0;
            switch (date.Month)
            {
                case 1:
                    result = tc.M1;
                    break;
                case 2:
                    result = tc.M2;
                    break;
                case 3:
                    result = tc.M3;
                    break;
                case 4:
                    result = tc.M4;
                    break;
                case 5:
                    result = tc.M5;
                    break;
                case 6:
                    result = tc.M6;
                    break;
                case 7:
                    result = tc.M7;
                    break;
                case 8:
                    result = tc.M8;
                    break;
                case 9:
                    result = tc.M9;
                    break;
                case 10:
                    result = tc.M10;
                    break;
                case 11:
                    result = tc.M11;
                    break;
                case 12:
                    result = tc.M12;
                    break;
            }
            return result;
        }

        /// <summary>
        /// 工厂的实际产能 用于分配计算
        /// </summary>
        public class FactoryLoading
        {

            /// <summary>
            /// 工厂ID
            /// </summary>
            public double FoID { get; set; }

            /// <summary>
            /// 本次负载
            /// </summary>
            public decimal Loading { get; set; }

            /// <summary>
            /// 历史占用负载
            /// </summary>
            public decimal HistoryLoading { get; set; }

            /// <summary>
            /// 目标产能
            /// </summary>
            public decimal TargetCapacity { get; set; }
        }
    }
}


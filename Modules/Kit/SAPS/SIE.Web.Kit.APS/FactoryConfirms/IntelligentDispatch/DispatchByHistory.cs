using SIE.Domain;
using SIE.Kit.APS.FactoryConfirms;
using System.Collections.Generic;
using System.Linq;
namespace SIE.Web.Kit.APS.FactoryConfirms
{
    /// <summary>
    /// 智能分厂 根据历史
    /// </summary>
    public class DispatchByHistory : DispatchBase
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="lstData">销售单明细数据</param>
        public DispatchByHistory(List<FactoryConfirmsViewModel> lstData) : base(lstData) { }

        /// <summary>
        /// 历史数据 Key:物料ID Value:库存组织
        /// </summary>
        private Dictionary<double, double> DicHistory { get; set; }

        /// <summary>
        /// 数据初始化
        /// </summary>
        public override void DataInit()
        {
            var ctrl2 = RT.Service.Resolve<FactoryConfirmsController>();
            var items = this.LstData.Select(x => x.ItemId).ToList();
            var lst = ctrl2.GetHistory().ToList();
            lst = lst.OrderByDescending(x => GroupByDate(x.PromiseDelivery, x.RequireDelivery)).ToList();
            var gs = lst.GroupBy(x => x.ItemId);
            this.DicHistory = new Dictionary<double, double>();
            foreach (var g in gs)
            {
                this.DicHistory[g.Key] = g.First().EnterpriseId;
            }
        }

        /// <summary>
        /// 工厂分配
        /// </summary>
        /// <param name="data">销售单行</param>
        /// <param name="lstFo">可分配的工厂（库存组织）</param>
        /// <returns></returns>
        public override List<double> Dispatch(FactoryConfirmsViewModel data, List<double> lstFo)
        {
            if (this.DicHistory != null && this.DicHistory.ContainsKey(data.ItemId))
            {
                if (lstFo.Contains(this.DicHistory[data.ItemId]))
                    return new List<double>() { this.DicHistory[data.ItemId] };
            }
            return lstFo;
        }
    }
}
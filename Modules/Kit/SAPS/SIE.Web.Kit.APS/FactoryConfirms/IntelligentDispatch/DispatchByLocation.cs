using SIE.Domain;
using SIE.Kit.APS.FactoryConfirms;
using SIE.Kit.APS.ProductLocations;
using SIE.SO.SaleOrders;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Web.Kit.APS.FactoryConfirms
{
    /// <summary>
    /// 智能分厂 根据产品定位
    /// </summary>
    public class DispatchByLocation : DispatchBase
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="lstData">销售单明细数据</param>
        public DispatchByLocation(List<FactoryConfirmsViewModel> lstData) : base(lstData)
        {
            this.LstPL = new List<ProductLocation>();
            this.DicDetailProcess = new Dictionary<double, List<SpecialProcess>>();
        }

        /// <summary>
        /// 产品定位配置集合
        /// </summary>
        private List<ProductLocation> LstPL { get; set; }

        /// <summary>
        /// 销售单明细的 特殊工艺值. Key:销售单明细ID
        /// </summary>
        private Dictionary<double, List<SpecialProcess>> DicDetailProcess { get; set; }

        /// <summary>
        /// 数据初始化
        /// </summary>
        public override void DataInit()
        {
            if (this.LstData == null || this.LstData.Count == 0)
                return;
            var ctrl = RT.Service.Resolve<ProductLocationController>();
            this.LstPL = ctrl.GetAllProductLocation().ToList();

            var ctrl2 = RT.Service.Resolve<SpecialProcessController>();
            var lst = ctrl2.GetDataByOrderDetail(this.LstData.Select(x => x.Id).ToList());
            var gs = lst.GroupBy(x => x.SaleOrderDetailId);
            this.DicDetailProcess = new Dictionary<double, List<SpecialProcess>>();
            foreach (var g in gs)
            {
                this.DicDetailProcess[g.Key] = g.ToList();
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
            List<double> lstResult = new List<double>();
            if (this.LstPL == null || this.LstPL.Count == 0)
                return lstFo;
            var gs = LstPL.GroupBy(x => x.Classification);
            foreach (var g in gs)
            {
                List<double> lstG = new List<double>();
                foreach (var pl in g)
                {
                    if (!lstFo.Contains(pl.EnterpriseId) || lstG.Contains(pl.EnterpriseId))
                        continue;
                    switch (pl.Classification)
                    {
                        case Classification.Product://产品分类 要匹配销售单明细中产品类型的值
                            if (data.ProductType == pl.TypeValue)
                                lstG.Add(pl.EnterpriseId);
                            break;
                        //case SIE.Kit.APS.ProductLocations.Classification.SpecialProcess://特殊工艺 要匹配销售单明细中特殊工艺表中的值对应值的范围（最小值~最大值）
                        //    if (!this.DicDetailProcess.ContainsKey(data.Id))
                        //        break;
                        //    var ps = this.DicDetailProcess[data.Id].FirstOrDefault(x => x.Process.ToString() == pl.TypeValue);
                        //    if (ps != null && (ps.Value >= pl.MinValue && ps.Value <= pl.MaxValue))
                        //        lstG.Add(pl.EnterpriseId.Value);
                        //    break;
                        case Classification.Industry://行业类型 要匹配销售单明细中 行业类型中的值
                            if (data.IndustryType == pl.TypeValue)
                                lstG.Add(pl.EnterpriseId);
                            break;
                    }
                }
                if (lstG.Count == 1)//如果某种条件只有一个库存组织的选择的话， 就直接返回
                    return lstG;
                lstResult.AddRange(lstG.ToArray());
            }
            return lstResult.Distinct().ToList();
        }
    }
}

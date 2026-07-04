using SIE.Domain;
using SIE.Kit.APS.FactoryConfirms;
using SIE.Resources.Enterprises;
using System.Collections.Generic;
using System.Linq;
namespace SIE.Web.Kit.APS.FactoryConfirms
{
    /// <summary>
    /// 智能分工厂 的算法
    /// </summary>
    public class IntelligentDispatch
    {

        /// <summary>
        /// 智能分工厂
        /// </summary>
        /// <param name="allFo">所有工厂（来源于本库存组织的工厂列表）</param>
        /// <param name="lstData">等待分厂确认的列表</param>
        /// <param name="lstDispathchRule">分厂的规则明细</param>
        public IntelligentDispatch(List<Enterprise> allFo, List<FactoryConfirmsViewModel> lstData, List<BranchFactoryProgrammeDetail> lstDispathchRule)
        {
            this.AllFo = allFo;
            this.LstData = lstData;
            this.LstDispathchRule = lstDispathchRule;
        }

        /// <summary>
        /// 所有工厂（来源于本库存组织的工厂列表）
        /// </summary>
        public List<Enterprise> AllFo { get; set; }

        /// <summary>
        /// 等待分厂确认的列表
        /// </summary>
        public List<FactoryConfirmsViewModel> LstData { get; set; }

        /// <summary>
        /// 分厂的规则明细
        /// </summary>
        public List<BranchFactoryProgrammeDetail> LstDispathchRule { get; set; }

        /// <summary>
        /// 智能分工厂逻辑
        /// </summary>
        public void Dispathch()
        {
            //工厂模式，先根据配置创建分配对像集合
            List<DispatchBase> lstDispatch = new List<DispatchBase>();
            foreach (var obj in this.LstDispathchRule)
            {
                DispatchBase dp = null;
                if (obj.ProgrammeRule == ProgrammeRule.ProductLocation)
                    dp = new DispatchByLocation(this.LstData);
                else if (obj.ProgrammeRule == ProgrammeRule.TargetCapacity)
                    dp = new DispatchByCapacity(this.LstData);
                else if (obj.ProgrammeRule == ProgrammeRule.HistoryLately)
                    dp = new DispatchByHistory(this.LstData);
                dp.DataInit();
                lstDispatch.Add(dp);
            }

            //分配工厂（库存组织）
            foreach (var data in this.LstData)
            {
                List<double> lstFo = this.AllFo.Select(x => x.Id).ToList();
                foreach (var dp in lstDispatch)
                {
                    lstFo = dp.Dispatch(data, lstFo);
                    if (lstFo.Count == 1)
                        break;
                }
                if (lstFo.Count > 0)
                    data.EnterpriseId = lstFo[0];//把指派的库存组织 赋值到数据中
                else
                {
                    var first = this.AllFo.FirstOrDefault();
                    if (first != null)
                        data.EnterpriseId = first.Id;//指派给随意的库存组织
                }
                //分派完成，刷新缓存数据
                foreach (var dp in lstDispatch)
                    dp.RefreshData(data);
            }
        }
    }
}


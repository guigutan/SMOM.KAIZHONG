using SIE.Domain;
using SIE.Kit.APS.EngineerPlans.HelpClass;
using SIE.SO.SaleOrders;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Kit.APS.EngineerPlans.Handle
{

    /// <summary>
    /// 同步工程计划
    /// </summary>
    [Services.Service(FallbackType = typeof(GenerateEngineerPlanHandle), ServiceLifeStyle = Services.ServiceLifeStyle.Transient)]
    public class GenerateEngineerPlanHandle
    {
        #region 属性

        /// <summary>
        /// 可同步的销售订单集合
        /// </summary>
        List<SaleOrderDetail> SaleOrderDetailList;

        /// <summary>
        /// 工程计划集合
        /// </summary>
        EntityList<EngineerPlan> EngineerPlanList;

        /// <summary>
        /// ALL工程计划
        /// </summary>
        EntityList<EngineerPlan> EngineerPlanListAll;

        List<EngineerPlanInfo> EngineerPlanSolutionList;

        Dictionary<double, EngineerPlan> PlanDic;

        #endregion

        /// <summary>
        /// 构造函数
        /// </summary>
        public GenerateEngineerPlanHandle()
        {
            SaleOrderDetailList = new List<SaleOrderDetail>();
            EngineerPlanList = new EntityList<EngineerPlan>();
            EngineerPlanListAll = new EntityList<EngineerPlan>();
            EngineerPlanSolutionList = new List<EngineerPlanInfo>();
            PlanDic = new Dictionary<double, EngineerPlan>();
        }

        /// <summary>
        ///  同步工程计划方法
        /// </summary>
        public virtual void GenerateEngineerPlan()
        {
            LoadBase();

            InitDate();

            //有需同步信息数据则保存

            SaveDate();
        }

        /// <summary>
        /// 加载数据
        /// </summary>
        public void LoadBase()
        {
            //为新单且已分配工厂的销售订单行(外部ecn等已判断是否新单)
            SaleOrderDetailList = RT.Service.Resolve<EngineerPlanController>().GetSaleOrderDetailList().ToList();

            //不包含完成和已删除的数据
            List<SOMI_PlanState> statelist = new List<SOMI_PlanState>() { SOMI_PlanState.Finish, SOMI_PlanState.Deleted };
            //先查询所有的符合更新条件的数据(非贪婪加载)
            PlanDic = RT.Service.Resolve<EngineerPlanController>().GetEngineerPlanAll(statelist).GroupBy(p=>p.Id).ToDictionary(p=>p.Key,p=>p.FirstOrDefault());

            //包含已同步在工程计划中的未计划与已计划的工程计划集合
            EngineerPlanSolutionList = RT.Service.Resolve<EngineerPlanController>().GetEngineerPlanAllToInfo();
        }

        private EngineerPlan GetEngineerPlan(double id)
        {
            if (PlanDic.ContainsKey(id))
            {
                return PlanDic[id];
            }
            return null;
        }

        /// <summary>
        /// 同步信息
        /// </summary>
        public void InitDate()
        {
            //先同步已是工程计划的数据
            EngineerPlanSolutionList.ForEach(o => syncSoInfo2(o));
    
            SaleOrderDetailList.ForEach(SoLine =>
            {
                EngineerPlan Plan = new EngineerPlan();
                //初始未计划
                Plan.PlanState = SOMI_PlanState.WaitToPlan;
                syncSoInfo(Plan, SoLine);
                //添加对象集合
                EngineerPlanList.Add(Plan);
            });
        }


        /// <summary>
        /// 同步销售订单行冗余信息
        /// </summary>
        private void syncSoInfo2(EngineerPlanInfo planso)
        {
            EngineerPlan Plan = GetEngineerPlan(planso.Id);
            if (Plan == null)
            {
                return;
            }
            if (planso.OrderDetailId == 0) {
                Plan.PlanState = SOMI_PlanState.Deleted;
                EngineerPlanListAll.Add(Plan);
                return;
            }
            Plan.ProductType = planso.ProductType;
            Plan.LineNo = planso.LineNo;
            Plan.ItemRevision = planso.ItemRevision;
            Plan.ItemExtPropName = planso.ItemExtPropName;
            Plan.ItemId = planso.ItemId;
            Plan.CustomerId = planso.CustomerId;
            Plan.IsNew = planso.IsNew;
            Plan.ExternalEcn = planso.ExternalEcn;
            Plan.Qty = planso.Qty;
            Plan.UnitId = planso.UnitId;
            Plan.Area = planso.Area;
            Plan.CustomerPoDate = planso.CustomerPoDate;
            Plan.RegisterDateTime = planso.RegisterDateTime;
            Plan.RequireDelivery = planso.RequireDelivery;
            Plan.OrderType = planso.OrderType;
            Plan.AllegroType = planso.AllegroType;
            Plan.AppArea = planso.AppArea;
            Plan.FactoryId = (double)planso.FactoryId;
            EngineerPlanListAll.Add(Plan);
        }

        /// <summary>
        /// 同步销售订单行冗余信息
        /// </summary>
        /// <param name="Plan"></param>
        /// <param name="SoLine"></param>
        private void syncSoInfo(EngineerPlan Plan, SaleOrderDetail SoLine)
        {
            Plan.SaleOrderDetail = SoLine;
            if (SoLine == null)
            {
                Plan.PlanState = SOMI_PlanState.Deleted;
                return;
            }
            Plan.ProductType = SoLine.ProductType;
            Plan.SaleOrderNo = SoLine.SaleOrder.Code;
            Plan.LineNo = SoLine.LineNo;
            Plan.ItemRevision = SoLine.ItemRevision;
            Plan.ItemExtPropName = SoLine.ItemExtPropName;
            Plan.ItemId = SoLine.ItemId;
            Plan.CustomerId = SoLine.SaleOrder.CustomerId;
            Plan.IsNew = SoLine.IsNew;
            Plan.ExternalEcn = SoLine.ExternalEcn;
            Plan.Qty = SoLine.Qty;
            Plan.UnitId = SoLine.UnitId;
            Plan.Area = SoLine.Area;
            Plan.CustomerPoDate = SoLine.SaleOrder.CustomerPoDate;
            Plan.RegisterDateTime = SoLine.SaleOrder.RegisterDateTime;
            Plan.RequireDelivery = SoLine.RequireDelivery;
            Plan.OrderType = SoLine.OrderType;
            Plan.AllegroType = SoLine.AllegroType;
            Plan.AppArea = SoLine.AppArea;
            Plan.FactoryId = (double)SoLine.EnterpriseId;
        }

        /// <summary>
        /// 保存信息
        /// </summary>
        public void SaveDate()
        {
            using (var tran = DB.TransactionScope(KitAPSEntityDataProvider.ConnectionStringName))
            {
                //原有工程计划更新
                if (EngineerPlanListAll.Any()) RF.Save(EngineerPlanListAll);
                //新加的工程计划
                if (EngineerPlanList.Any()) RF.Save(EngineerPlanList);
                tran.Complete();
            }
        }
    }
}

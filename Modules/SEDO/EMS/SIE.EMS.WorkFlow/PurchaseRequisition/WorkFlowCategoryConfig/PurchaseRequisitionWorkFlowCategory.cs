using SIE.WorkFlow.Base.FlowDefinitions.Categorys;
using System;

namespace SIE.EMS.WorkFlow.PurchaseRequisition.WorkFlowCategoryConfig
{
    /// <summary>
    /// 工作流分类-资产采购申请
    /// </summary>
    [Serializable]
    public class PurchaseRequisitionWorkFlowCategory : WorkFlowCategoryBase
    {
        /// <summary>
        /// 名称
        /// </summary>
        public static readonly string CategoryName = "资产采购申请审核流程";

        /// <summary>
        /// 构造函数
        /// </summary>
        public PurchaseRequisitionWorkFlowCategory()
        {
            Name = CategoryName;
            Config = new PurchaseRequisitionWorkFlowCategoryConfig();
        }
    }
}

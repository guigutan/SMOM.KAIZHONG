using SIE.Rbac.InvOrgs;
using SIE.Warehouses;
using System;

namespace SIE.ERPInterface.Common.Datas.EbsData
{
    /// <summary>
    /// 仓库
    /// </summary>
    [Serializable]
    public class ErpWarehouseData : EbsDataBase
    {
         
        /// <summary>
        /// 二级库存名称(Code)
        /// </summary>
        public string Secondary_Inventory_Name { get; set; }

        /// <summary>
        /// 描述(Name)
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// ERP组织ID
        /// </summary>
        public int Organization_Id { get; set; }

        /// <summary>
        /// SMOM库存组织
        /// </summary>
        public InvOrg? InvOrg { get; set; }

        /// <summary>
        /// 组织代码
        /// </summary>
        public string Organization_Code { get; set; }
       
        /// <summary>
        /// 启用标志
        /// </summary>
        public string Enable_Flag { get; set; }

        /// <summary>
        /// ERP子库
        /// </summary>
        public ErpWarehouse? ErpWarehouse { get; set; }

    }

}

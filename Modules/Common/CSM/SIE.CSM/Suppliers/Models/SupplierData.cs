using SIE.EventMessages;

namespace SIE.CSM.Suppliers
{
    /// <summary>
    /// 供应商信息
    /// </summary>
    public class SupplierData : ErpInfoData
    {
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 简称
        /// </summary>
        public string ShortName { get; set; }

        /// <summary>
        /// 英文名称
        /// </summary>
        public string EnglishName { get; set; }

        /// <summary>
        /// 所在区域
        /// </summary>
        public string SalesArea { get; set; }

        /// <summary>
        /// 税号  
        /// </summary>
        public string DutyParagraph { get; set; }

        /// <summary>
        /// 联系人  
        /// </summary>
        public string Contacts { get; set; }

        /// <summary>
        /// 联系电话  
        /// </summary>
        public string ContactNumber { get; set; }

        /// <summary>
        /// 联系地址  
        /// </summary>
        public string ContactAddress { get; set; }

        /// <summary>
        /// 电子邮箱  
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 状态  
        /// </summary>
        public int State { get; set; }

        /// <summary>
        /// 邮编  
        /// </summary>
        public string PostalCode { get; set; }

        /// <summary>
        /// 备注  
        /// </summary>
        public string Remark { get; set; }
    }
}

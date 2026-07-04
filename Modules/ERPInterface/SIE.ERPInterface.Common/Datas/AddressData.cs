using SIE.EventMessages;

namespace SIE.ERPInterface.Common.Datas
{
    /// <summary>
    /// 地址信息
    /// </summary>
    public class AddressData : ErpInfoData
    {
        /// <summary>
        /// 国家
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// 省
        /// </summary>
        public string Province { get; set; }

        /// <summary>
        /// 市
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// 区
        /// </summary>
        public string Area { get; set; }

        /// <summary>
        /// 详细地址  
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 联系人  
        /// </summary>
        public string Contacts { get; set; }

        /// <summary>
        /// 联系电话      
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 传真  
        /// </summary>
        public string Fax { get; set; }

        /// <summary>
        /// 电子邮箱  
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 邮编  
        /// </summary>
        public string ZipCode { get; set; }

        /// <summary>
        /// 状态  
        /// </summary>
        public int State { get; set; }

        /// <summary>
        /// 备注  
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 地址类型  
        /// </summary>
        public string AddressType { get; set; }
    }
}

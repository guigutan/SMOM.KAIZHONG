using System;

namespace SIE.ERPInterface.Common.Datas
{
    /// <summary>
    /// 供应商数据
    /// </summary>
    [Serializable]
    public class SupplierDataEbs : EbsDataBase
    {
        /// <summary>
        /// 供应商ID
        /// </summary>
        public int Vendor_Id { get; set; }

        /// <summary>
        /// 供应商编号
        /// </summary>
        public string Vendor_Num { get; set; }

        /// <summary>
        /// 供应商名称
        /// </summary>
        public string Vendor_Name { get; set; }

        /// <summary>
        /// 供应商备用名称
        /// </summary>
        public string Vendor_Name_Alt { get; set; }

        /// <summary>
        /// 供应商类型代码
        /// </summary>
        public string Vendor_Type_Lookup_Code { get; set; }

        /// <summary>
        /// 供应商类型名称
        /// </summary>
        public string Vendor_Type_Lookup_Name { get; set; }

        /// <summary>
        /// 启用标志
        /// </summary>
        public string Enable_Flag { get; set; }

        /// <summary>
        /// 供应商
        /// </summary>
        public SIE.CSM.Suppliers.Supplier? Supplier { get; set; }
    }

}

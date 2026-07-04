using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.ERPInterface.Common.Datas
{
    /// <summary>
    /// 客户数据
    /// </summary>
    [Serializable]
    public class CustomerDataEbs : EbsDataBase
    {         
        /// <summary>
        /// 编号
        /// </summary>
        public string Party_Number { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Party_Name { get; set; }

        /// <summary>
        /// 简称
        /// </summary>
        public string Known_As { get; set; }

        /// <summary>
        /// 所在区域
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public string Party_Type { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// 供应商编码
        /// </summary>
        public string Vendor_Code { get; set; }

        /// <summary>
        /// 税号
        /// </summary>
        public string Num_1099 { get; set; }

        /// <summary>
        /// 联系人
        /// </summary>
        public string Contact_Person { get; set; }

        /// <summary>
        /// 联系电话
        /// </summary>
        public string Primary_Phone_Number { get; set; }

        /// <summary>
        /// 联系地址
        /// </summary>
        public string Address1 { get; set; }

        /// <summary>
        /// 电子邮件
        /// </summary>
        public string Email_Address { get; set; }

        /// <summary>
        /// 邮政编码
        /// </summary>
        public string Postal_Code { get; set; }
       
        /// <summary>
        /// 启用标志
        /// </summary>
        public string Enable_Flag { get; set; }

        /// <summary>
        /// 库存组织Id
        /// </summary>
        public double Organization_Id { get; set; }

        /// <summary>
        /// 客户
        /// </summary>
        public SIE.CSM.Customers.Customer? Customer { get; set; }
    }

}

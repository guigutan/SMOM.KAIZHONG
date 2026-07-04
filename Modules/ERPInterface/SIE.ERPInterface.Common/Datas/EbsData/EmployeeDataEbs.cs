using SIE.Domain;
using System;

namespace SIE.ERPInterface.Common.Datas
{
    /// <summary>
    /// 员工数据
    /// </summary>
    [Serializable]
    public class EmployeeDataEbs : EbsDataBase
    {         
        /// <summary>
        /// 员工编号
        /// </summary>
        public string Employee_Number { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string Full_Name { get; set; }

        /// <summary>
        /// 性别 0男 1女
        /// </summary>
        public int Sex { get; set; }

        /// <summary>
        /// 原始入职日期
        /// </summary>
        public DateTime? Original_Date_Of_Hire { get; set; }

        /// <summary>
        /// 邮箱地址
        /// </summary>
        public string Email_Address { get; set; }
       
        /// <summary>
        /// 启用标志
        /// </summary>
        public string Enable_Flag { get; set; }        

        /// <summary>
        /// 员工
        /// </summary>
        public SIE.Resources.Employees.Employee? Employee { get; set; } 
    }

}

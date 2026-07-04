using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.DashBoard.KzReport.Datas
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class RequestProductionAchievementRateData
    {
        /// <summary>
        /// 分组
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        /// 产品线
        /// </summary>
        public string ProductLine { get; set; }
        /// <summary>
        /// 厂部名称
        /// </summary>
        public string PlantName { get; set; }
        /// <summary>
        /// 时间范围
        /// </summary>
        public DateRange DateRange { get; set; }

        /// <summary>
        /// 查询工序ID
        /// </summary>
        public List<string> ProcessCodes { get; set; }


        /// <summary>
        /// 查询物料编码
        /// </summary>
        public List<string> ItemCodes { get; set; }
    }
  
    /// <summary>
    /// 查询维度
    /// </summary>
    public enum GroupNameBy
    {
        /// <summary>
        /// 查询维度-产品线
        /// </summary>
        [Description("产品线")]
        ByProductLine,
        /// <summary>
        /// 查询维度-产部
        /// </summary>
        [Description("产部")]
        ByPlant,
        /// <summary>
        /// 查询维度-工序
        /// </summary>
        [Description("工序")]
        ByProcess,
        /// <summary>
        /// 查询维度-产品
        /// </summary>
        [Description("产品")]
        ByProduct
    }

    /// <summary>
    /// 获取枚举的 Description 描述
    /// </summary>
    public static class EnumExtension
    {
        /// <summary>
        /// 获取枚举的 Description 描述
        /// </summary>
        public static string GetDescription(this Enum value)
        {
            if (value == null) return "";

            FieldInfo field = value.GetType().GetField(value.ToString());
            if (field == null) return value.ToString();

            DescriptionAttribute attribute = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;
            return attribute == null ? value.ToString() : attribute.Description;
        }
    }

    /// <summary>
    /// 返回下拉的物料
    /// </summary>
    public class Products
    {
        /// <summary>
        /// 产品编码
        /// </summary>
        public string ProductCode { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName { get; set; }
    }
  



}

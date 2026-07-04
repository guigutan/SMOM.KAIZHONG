using SIE.Defects.InspectionItems;
using SIE.Defects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIE.Items;
using SIE.Tech.Processs;

namespace SIE.MES.ForWinform.ApiModels
{
    [Serializable]
    public class XPModelInspectionItem
    {

        public double Id
        {
            get; set;
        }

        /// <summary>
        /// 机型Id
        /// </summary>
        public double? ModelId
        {
            get; set;
        }

        /// <summary>
        /// 产品Id
        /// </summary>
        public double? ProductItemId
        {
            get; set;
        }

        /// <summary>
        /// 产品
        /// </summary>
        public Item ProductItem
        {
            get; set;
        }

        /// <summary>
        /// 工序Id
        /// </summary>
        public double ProcessId
        {
            get; set;
        }

        /// <summary>
        /// 工序
        /// </summary>
        public Process Process
        {
            get; set;
        }

        /// <summary>
        /// 排序
        /// </summary>
        public int? OrderNum
        {
            get; set;
        }

        /// <summary>
        /// 工序名称
        /// </summary>
        public string ProcessName
        {
            get; set;
        }

        /// <summary>
        /// 机型名称
        /// </summary>
        public string ModelName
        {
            get; set;
        }

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductItemName
        {
            get; set;
        }

        /// <summary>
        /// 单位名称
        /// </summary>
        public string UnitName
        {
            get; set;
        }

        /// <summary>
        /// 检验项目
        /// </summary>
        public string Name
        {
            get; set;
        }

        /// <summary>
        /// 检验类别
        /// </summary>
        public string Category
        {
            get; set;
        }

        /// <summary>
        /// 检验工具
        /// </summary>
        public string TestTool
        {
            get; set;
        }

        /// <summary>
        /// 检验依据
        /// </summary>
        public string InspectionBasis
        {
            get; set;
        }

        /// <summary>
        /// 规格下限
        /// </summary>
        public decimal? LimitLow
        {
            get; set;
        }

        /// <summary>
        /// 规格上限
        /// </summary>
        public decimal? LimitMax
        {
            get; set;
        }

        /// <summary>
        /// 技术要求
        /// </summary>
        public string TechnicalRequirements
        {
            get; set;
        }

        /// <summary>
        /// 检验方式Id
        /// </summary>
        public double InspectionModeId
        {
            get; set;
        }

        /// <summary>
        /// 检验方式
        /// </summary>
        public InspectionMode InspectionMode
        {
            get; set;
        }

        /// <summary>
        /// 是否必检
        /// </summary>
        public bool IsSuitable
        {
            get; set;
        }

        /// <summary>
        /// 生效日期
        /// </summary>
        public DateTime EffectiveStartTime
        {
            get; set;
        }

        /// <summary>
        /// 失效日期
        /// </summary>
        public DateTime? EffectiveEndTime
        {
            get; set;
        }

        /// <summary>
        /// 缺陷等级Id
        /// </summary>
        public double? DefectGradeId
        {
            get; set;
        }
        /// <summary>
        /// 缺陷等级
        /// </summary>
        public DefectGrade DefectGrade
        {
            get; set;
        }

        /// <summary>
        /// 检验标识
        /// </summary>
        public CheckTag CheckTag
        {
            get; set;
        }
        /// <summary>
        /// 规格下限判断符号
        /// </summary>
        public CompareType? LimitLowCompare
        {
            get; set;
        }

        /// <summary>
        /// 规格上限判断符号
        /// </summary>
        public CompareType? LimitMaxCompare
        {
            get; set;
        }
    }
}

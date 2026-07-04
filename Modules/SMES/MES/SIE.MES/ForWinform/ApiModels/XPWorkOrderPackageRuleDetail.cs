using SIE.Common.Sort;
using SIE.MES.WorkOrders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.ForWinform.ApiModels
{
    /// <summary>
    /// 工单包装规则
    /// </summary>
    [Serializable]
    public class XPWorkOrderPackageRuleDetail
    {
        /// <summary>
        /// 
        /// </summary>
        public double Id { get; set; }

        /// <summary>
        /// 长
        /// </summary>
        public decimal Length { get; set; }

        /// <summary>
        /// 宽
        /// </summary>
        public decimal Width { get; set; }

        /// <summary>
        /// 产品数
        /// </summary>
        public decimal Qty { get; set; }

        /// <summary>
        /// 包装数
        /// </summary>
        public decimal LevelQty { get; set; }

        /// <summary>
        /// 是否装箱
        /// </summary>
        public bool IsPackage { get; set; }

        /// <summary>
        /// 是否出库标签
        /// </summary>
        public bool IsOutStockLabel { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 重量
        /// </summary>
        public decimal Weight { get; set; }

        /// <summary>
        /// 是否入库标签
        /// </summary>
        public bool IsInStockLabel { get; set; }

        /// <summary>
        /// 高度
        /// </summary>
        public decimal Height { get; set; }

        /// <summary>
        /// 体积
        /// </summary>
        public decimal Volume { get; set; }

        /// <summary>
        /// 包装单位ID
        /// </summary>
        public double PackageUnitId { get; set; }

        /// <summary>
        /// 编码规则ID
        /// </summary>
        public double? NumberRuleId { get; set; }

        /// <summary>
        /// 工单ID
        /// </summary>
        public double WorkOrderId { get; set; }

        /// <summary>
        /// 打印模板ID
        /// </summary>
        public double? PrintTemplateId { get; set; }

        /// <summary>
        /// 是否打印
        /// </summary>
        public bool IsPrint { get; set; }

        /// <summary>
        /// 物料包装规则明细Id
        /// </summary>
        public double DetailId { get; set; }

        /// <summary>
        /// 包装单位名称
        /// </summary>
        public string PackageUnitName { get; set; }

        /// <summary>
        /// 规则名称
        /// </summary>
        public string NumberRuleName { get; set; }

        /// <summary>
        /// 模板名称
        /// </summary>
        public string TemplateName { get; set; }

        /// <summary>
        /// 是否主单位
        /// </summary>
        public bool IsMasterUnit { get; set; }


        /// <summary>
        /// 包装关系层级
        /// </summary>
        public double INDEX_ { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rule"></param>
        public XPWorkOrderPackageRuleDetail(WorkOrderPackageRuleDetail rule)
        {
            if (rule == null)
                return;
            this.Id = rule.Id;
            this.Length = rule.Length;
            this.Width = rule.Width;
            this.Qty = rule.Qty;
            this.LevelQty = rule.LevelQty;
            this.IsPackage = rule.IsPackage;
            this.IsOutStockLabel = rule.IsOutStockLabel;
            this.Description = rule.Description;
            this.Weight = rule.Weight;
            this.IsInStockLabel = rule.IsInStockLabel;
            this.Height = rule.Height;
            this.Volume = rule.Volume;
            this.PackageUnitId = rule.PackageUnitId;
            this.NumberRuleId = rule.NumberRuleId;
            this.WorkOrderId = rule.WorkOrderId;
            this.PrintTemplateId = rule.PrintTemplateId;
            this.IsPrint = rule.IsPrint;
            this.DetailId = rule.DetailId;
            this.PackageUnitName = rule.PackageUnitName;
            this.NumberRuleName = rule.NumberRuleName;
            this.TemplateName = rule.TemplateName;
            this.IsMasterUnit = rule.IsMasterUnit;
            this.INDEX_ = rule.GetIndex();
        }
    }
}

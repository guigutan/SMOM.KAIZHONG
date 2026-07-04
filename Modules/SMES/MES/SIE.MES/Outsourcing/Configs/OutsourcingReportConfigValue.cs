using SIE.Common.Configs;
using SIE.Domain;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.Outsourcing.Configs
{
    /// <summary>
    /// 工单号配置值
    /// </summary>
    [RootEntity, Serializable]
    [Label("委外报工配置值")]
    public class OutsourcingReportConfigValue: ConfigValue
    {
        #region 是否收货自动报工 IsInAutoReport
        /// <summary>
        /// 是否收货自动报工
        /// </summary>
        [Label("是否收货自动报工")]
        public static readonly Property<bool?> IsInAutoReportProperty = P<OutsourcingReportConfigValue>.Register(e => e.IsInAutoReport);

        /// <summary>
        /// 是否收货自动报工
        /// </summary>
        public bool? IsInAutoReport
        {
            get { return this.GetProperty(IsInAutoReportProperty); }
            set { this.SetProperty(IsInAutoReportProperty, value); }
        }
        #endregion

        #region 是否校验委外收货记录 IsOutsourcingInsVaildReportLog
        /// <summary>
        /// 是否校验委外收货记录
        /// </summary>
        [Label("是否校验委外收货记录")]
        public static readonly Property<bool?> IsOutsourcingInsVaildReportLogProperty = P<OutsourcingReportConfigValue>.Register(e => e.IsOutsourcingInsVaildReportLog);

        /// <summary>
        /// 是否校验委外收货记录
        /// </summary>
        public bool? IsOutsourcingInsVaildReportLog
        {
            get { return this.GetProperty(IsOutsourcingInsVaildReportLogProperty); }
            set { this.SetProperty(IsOutsourcingInsVaildReportLogProperty, value); }
        }
        #endregion

        #region 是否校验委外报工记录 IsReportValidOutsourcingReportLog
        /// <summary>
        /// 是否校验委外报工记录
        /// </summary>
        [Label("是否校验委外报工记录")]
        public static readonly Property<bool?> IsValidOutsourcingReportLogProperty = P<OutsourcingReportConfigValue>.Register(e => e.IsValidOutsourcingReportLog);

        /// <summary>
        /// 是否校验委外报工记录
        /// </summary>
        public bool? IsValidOutsourcingReportLog
        {
            get { return this.GetProperty(IsValidOutsourcingReportLogProperty); }
            set { this.SetProperty(IsValidOutsourcingReportLogProperty, value); }
        }
        #endregion

        #region 是否首工序校验(多个工序用英文逗号隔开) ValidStartProcess
        /// <summary>
        /// 是否首工序校验(多个工序用英文逗号隔开)
        /// </summary>
        [Label("是否首工序校验(多个工序用英文逗号隔开)")]
        public static readonly Property<string> ValidStartProcessProperty = P<OutsourcingReportConfigValue>.Register(e => e.ValidStartProcess);

        /// <summary>
        /// 是否首工序校验(多个工序用英文逗号隔开)
        /// </summary>
        public string ValidStartProcess
        {
            get { return this.GetProperty(ValidStartProcessProperty); }
            set { this.SetProperty(ValidStartProcessProperty, value); }
        }
        #endregion

        /// <summary>
        /// 显示 
        /// </summary>
        /// <returns>编码规则名称</returns>
        public override string Display()
        {
            if (IsInAutoReport == null)
                return "NIL";
            return IsInAutoReport == false ? "否" : "是";
        }
    }
}

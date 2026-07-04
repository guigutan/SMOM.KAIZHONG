using SIE.Common.Configs;
using SIE.Common.Prints;
using SIE.Domain;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EMS.SpareParts.Configs
{
    /// <summary>
    /// 批次号和序列号打印模板配置规则
    /// </summary>
    [System.ComponentModel.DisplayName("批次号和序列号打印模板配置规则")]
    [System.ComponentModel.Description("用于配置备件入库明细中批次号和序列号打印模板的具体规则")]
    public class StoreDetailLabelPrintConfig : ModuleConfig<StoreDetailLabelPrintConfigValue>
    {
        /// <summary>
        /// 默认值
        /// </summary>
        readonly StoreDetailLabelPrintConfigValue defaultValue = new StoreDetailLabelPrintConfigValue { LotPrintTemplate = null, SnPrintTempldate = null };

        /// <summary>
        /// 默认值
        /// </summary>
        public override StoreDetailLabelPrintConfigValue DefaultValue
        {
            get
            {
                return defaultValue;
            }
        }
    }

    /// <summary>
    /// 批次号配置值
    /// </summary>
    [RootEntity, Serializable]
    [Label("批次号")]
    public class StoreDetailLabelPrintConfigValue : ConfigValue
    {
        #region 批次标签打印模板 LotPrintTemplate
        /// <summary>
        /// 批次标签打印模板Id
        /// </summary>
        [Label("批次标签打印模板")]
        public static readonly IRefIdProperty LotPrintTemplateIdProperty =
            P<StoreDetailLabelPrintConfigValue>.RegisterRefId(e => e.LotPrintTemplateId, ReferenceType.Normal);

        /// <summary>
        /// 批次标签打印模板Id
        /// </summary>
        public double? LotPrintTemplateId
        {
            get { return (double?)this.GetRefNullableId(LotPrintTemplateIdProperty); }
            set { this.SetRefNullableId(LotPrintTemplateIdProperty, value); }
        }

        /// <summary>
        /// 批次标签打印模板
        /// </summary>
        public static readonly RefEntityProperty<PrintTemplate> LotPrintTemplateProperty =
            P<StoreDetailLabelPrintConfigValue>.RegisterRef(e => e.LotPrintTemplate, LotPrintTemplateIdProperty);

        /// <summary>
        /// 批次标签打印模板
        /// </summary>
        public PrintTemplate LotPrintTemplate
        {
            get { return this.GetRefEntity(LotPrintTemplateProperty); }
            set { this.SetRefEntity(LotPrintTemplateProperty, value); }
        }
        #endregion

        #region 序列号标签打印模板 SnPrintTempldate
        /// <summary>
        /// 序列号标签打印模板Id
        /// </summary>
        [Label("序列号标签打印模板")]
        public static readonly IRefIdProperty SnPrintTempldateIdProperty =
            P<StoreDetailLabelPrintConfigValue>.RegisterRefId(e => e.SnPrintTempldateId, ReferenceType.Normal);

        /// <summary>
        /// 序列号标签打印模板Id
        /// </summary>
        public double? SnPrintTempldateId
        {
            get { return (double?)this.GetRefNullableId(SnPrintTempldateIdProperty); }
            set { this.SetRefNullableId(SnPrintTempldateIdProperty, value); }
        }

        /// <summary>
        /// 序列号标签打印模板
        /// </summary>
        public static readonly RefEntityProperty<PrintTemplate> SnPrintTempldateProperty =
            P<StoreDetailLabelPrintConfigValue>.RegisterRef(e => e.SnPrintTempldate, SnPrintTempldateIdProperty);

        /// <summary>
        /// 序列号标签打印模板
        /// </summary>
        public PrintTemplate SnPrintTempldate
        {
            get { return this.GetRefEntity(SnPrintTempldateProperty); }
            set { this.SetRefEntity(SnPrintTempldateProperty, value); }
        }
        #endregion

        /// <summary>
        /// 显示
        /// </summary>
        /// <returns>编码规则名称</returns>
        public override string Display()
        {
            if (LotPrintTemplate == null || SnPrintTempldate == null)
                return "NIL";
            return LotPrintTemplate.FileName + ";" + SnPrintTempldate.FileName;
        }
    }
}

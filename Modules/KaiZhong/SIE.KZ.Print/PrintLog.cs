using SIE.Common.Prints;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.KZ.Print
{
    /// <summary>
    /// 打印日志,不分组织
    /// </summary>
    [RootEntity, Serializable]
    [CriteriaQuery]
    [Label("打印日志")]
    [DisplayMember(nameof(DeviceCode))]
    public class PrintLog : DataEntity
    {
        #region 设备编码 DeviceCode
        /// <summary>
        /// 设备编码
        /// </summary>
        [Label("设备编码")]
        public static readonly Property<string> DeviceCodeProperty = P<PrintLog>.Register(e => e.DeviceCode);

        /// <summary>
        /// 设备编码
        /// </summary>
        public string DeviceCode
        {
            get { return this.GetProperty(DeviceCodeProperty); }
            set { this.SetProperty(DeviceCodeProperty, value); }
        }
        #endregion

        #region 设备名称 DeviceName
        /// <summary>
        /// 设备名称
        /// </summary>
        [Label("设备名称")]
        public static readonly Property<string> DeviceNameProperty = P<PrintLog>.Register(e => e.DeviceName);

        /// <summary>
        /// 设备名称
        /// </summary>
        public string DeviceName
        {
            get { return this.GetProperty(DeviceNameProperty); }
            set { this.SetProperty(DeviceNameProperty, value); }
        }
        #endregion

        #region 打印机名称 PrinterName
        /// <summary>
        /// 打印机名称
        /// </summary>
        [Label("打印机名称")]
        public static readonly Property<string> PrinterNameProperty = P<PrintLog>.Register(e => e.PrinterName);

        /// <summary>
        /// 打印机名称
        /// </summary>
        public string PrinterName
        {
            get { return this.GetProperty(PrinterNameProperty); }
            set { this.SetProperty(PrinterNameProperty, value); }
        }
        #endregion

        #region 打印模板 PrintTemplate
        /// <summary>
        /// 打印模板Id
        /// </summary>
        [Label("打印模板")]
        public static readonly IRefIdProperty PrintTemplateIdProperty =
            P<PrintLog>.RegisterRefId(e => e.PrintTemplateId, ReferenceType.Normal);

        /// <summary>
        /// 打印模板Id
        /// </summary>
        public double? PrintTemplateId
        {
            get { return (double?)this.GetRefNullableId(PrintTemplateIdProperty); }
            set { this.SetRefNullableId(PrintTemplateIdProperty, value); }
        }

        /// <summary>
        /// 打印模板
        /// </summary>
        public static readonly RefEntityProperty<PrintTemplate> PrintTemplateProperty =
            P<PrintLog>.RegisterRef(e => e.PrintTemplate, PrintTemplateIdProperty);

        /// <summary>
        /// 打印模板
        /// </summary>
        public PrintTemplate PrintTemplate
        {
            get { return this.GetRefEntity(PrintTemplateProperty); }
            set { this.SetRefEntity(PrintTemplateProperty, value); }
        }
        #endregion

        #region 关键内容 DataKey
        /// <summary>
        /// 关键内容
        /// </summary>
        [Label("关键内容")]
        public static readonly Property<string> DataKeyProperty = P<PrintLog>.Register(e => e.DataKey);

        /// <summary>
        /// 关键内容
        /// </summary>
        public string DataKey
        {
            get { return this.GetProperty(DataKeyProperty); }
            set { this.SetProperty(DataKeyProperty, value); }
        }
        #endregion

        #region 打印数据 PrintData
        /// <summary>
        /// 打印数据
        /// </summary>
        [Label("打印数据")]
        public static readonly Property<string> PrintDataProperty = P<PrintLog>.Register(e => e.PrintData);

        /// <summary>
        /// 打印数据
        /// </summary>
        public string PrintData
        {
            get { return this.GetProperty(PrintDataProperty); }
            set { this.SetProperty(PrintDataProperty, value); }
        }
        #endregion

        #region 打印类型 PrintType
        /// <summary>
        /// 打印类型
        /// </summary>
        [Label("打印类型")]
        public static readonly Property<string> PrintTypeProperty = P<PrintLog>.Register(e => e.PrintType);

        /// <summary>
        /// 打印类型
        /// </summary>
        public string PrintType
        {
            get { return this.GetProperty(PrintTypeProperty); }
            set { this.SetProperty(PrintTypeProperty, value); }
        }
        #endregion

        #region 打印状态 PrintState
        /// <summary>
        /// 打印状态
        /// </summary>
        [Label("打印状态")]
        public static readonly Property<string> PrintStateProperty = P<PrintLog>.Register(e => e.PrintState);

        /// <summary>
        /// 打印状态
        /// </summary>
        public string PrintState
        {
            get { return this.GetProperty(PrintStateProperty); }
            set { this.SetProperty(PrintStateProperty, value); }
        }
        #endregion

        #region 库存组织 InvOrgId
        /// <summary>
        /// 库存组织
        /// </summary>
        [Label("库存组织")]
        public static readonly Property<int?> InvOrgIdProperty = P<PrintLog>.Register(e => e.InvOrgId);

        /// <summary>
        /// 库存组织
        /// </summary>
        public int? InvOrgId
        {
            get { return this.GetProperty(InvOrgIdProperty); }
            set { this.SetProperty(InvOrgIdProperty, value); }
        }
        #endregion

        #region 备注 Remark
        /// <summary>
        /// 备注
        /// </summary>
        [Label("备注")]
        public static readonly Property<string> RemarkProperty = P<PrintLog>.Register(e => e.Remark);

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            get { return this.GetProperty(RemarkProperty); }
            set { this.SetProperty(RemarkProperty, value); }
        }
        #endregion


    }
    /// <summary>
    /// 打印日志 实体配置
    /// </summary>
    internal class PrintLogConfig : EntityConfig<PrintLog>
    {
        /// <summary>
        /// Meta属性配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("PRINT_LOG").MapAllProperties();
            Meta.Property(PrintLog.DataKeyProperty).ColumnMeta.HasLength("MAX");
            Meta.Property(PrintLog.PrintDataProperty).ColumnMeta.HasLength("MAX");
            Meta.Property(PrintLog.RemarkProperty).ColumnMeta.HasLength(4000);
            Meta.DisableInvOrg();
            //Meta.EnablePhantoms();
        }
    }
}

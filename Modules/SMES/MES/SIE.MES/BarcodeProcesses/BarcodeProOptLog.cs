using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.BarcodeProcesses
{
    /// <summary>
    /// 条码工序指派操作日志
    /// </summary>
    [RootEntity, Serializable]
    [Label("条码工序指派操作日志")]
    public class BarcodeProOptLog : DataEntity
    {
        #region 条码 BarcodeProcess
        /// <summary>
        /// 条码Id
        /// </summary>
        [Label("条码")]
        public static readonly IRefIdProperty BarcodeProcessIdProperty =
            P<BarcodeProOptLog>.RegisterRefId(e => e.BarcodeProcessId, ReferenceType.Normal);

        /// <summary>
        /// 条码Id
        /// </summary>
        public double BarcodeProcessId
        {
            get { return (double)this.GetRefId(BarcodeProcessIdProperty); }
            set { this.SetRefId(BarcodeProcessIdProperty, value); }
        }

        /// <summary>
        /// 条码
        /// </summary>
        public static readonly RefEntityProperty<BarcodeProcess> BarcodeProcessProperty =
            P<BarcodeProOptLog>.RegisterRef(e => e.BarcodeProcess, BarcodeProcessIdProperty);

        /// <summary>
        /// 条码
        /// </summary>
        public BarcodeProcess BarcodeProcess
        {
            get { return this.GetRefEntity(BarcodeProcessProperty); }
            set { this.SetRefEntity(BarcodeProcessProperty, value); }
        }
        #endregion

        #region 操作时间 OptTime
        /// <summary>
        /// 操作时间
        /// </summary>
        [Label("操作时间")]
        public static readonly Property<DateTime> OptTimeProperty = P<BarcodeProOptLog>.Register(e => e.OptTime);

        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime OptTime
        {
            get { return this.GetProperty(OptTimeProperty); }
            set { this.SetProperty(OptTimeProperty, value); }
        }
        #endregion

        #region 操作人 Opter
        /// <summary>
        /// 操作人Id
        /// </summary>
        [Label("操作人")]
        public static readonly IRefIdProperty OpterIdProperty =
            P<BarcodeProOptLog>.RegisterRefId(e => e.OpterId, ReferenceType.Normal);

        /// <summary>
        /// 操作人Id
        /// </summary>
        public double OpterId
        {
            get { return (double)this.GetRefId(OpterIdProperty); }
            set { this.SetRefId(OpterIdProperty, value); }
        }

        /// <summary>
        /// 操作人
        /// </summary>
        public static readonly RefEntityProperty<Employee> OpterProperty =
            P<BarcodeProOptLog>.RegisterRef(e => e.Opter, OpterIdProperty);

        /// <summary>
        /// 操作人
        /// </summary>
        public Employee Opter
        {
            get { return this.GetRefEntity(OpterProperty); }
            set { this.SetRefEntity(OpterProperty, value); }
        }
        #endregion

        #region 操作内容 Content
        /// <summary>
        /// 操作内容
        /// </summary>
        [Label("操作内容")]
        [MaxLength(2000)]
        public static readonly Property<string> ContentProperty = P<BarcodeProOptLog>.Register(e => e.Content);

        /// <summary>
        /// 操作内容
        /// </summary>
        public string Content
        {
            get { return this.GetProperty(ContentProperty); }
            set { this.SetProperty(ContentProperty, value); }
        }
        #endregion

    }

    /// <summary>
    /// 数据源配置
    /// </summary>
    public class BarcodeProOptLogConfig : EntityConfig<BarcodeProOptLog>
    {
        /// <summary>
        /// 数据库配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("BC_BARCODE_POLOG").MapAllProperties();
            Meta.Property(BarcodeProOptLog.ContentProperty).ColumnMeta.HasLength(4000);
            Meta.EnablePhantoms();
        }
    }
}

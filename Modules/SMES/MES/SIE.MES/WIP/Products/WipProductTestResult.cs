using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.MES.WIP.Products
{
    /// <summary>
    /// 产品测试结果
    /// </summary>
    [ChildEntity, Serializable]
    [Label("产品测试结果")]
    public partial class WipProductTestResult : DataEntity
    {
        #region 测试项目 Item
        /// <summary>
        /// 测试项目
        /// </summary>
        [Required]
        [MaxLength(40)]
        [Label("测试项目")]
        public static readonly Property<string> ItemProperty = P<WipProductTestResult>.Register(e => e.Item);

        /// <summary>
        /// 测试项目
        /// </summary>
        public string Item
        {
            get { return GetProperty(ItemProperty); }
            set { SetProperty(ItemProperty, value); }
        }
        #endregion

        #region 测试结果 Result
        /// <summary>
        /// 测试结果
        /// </summary>
        [Label("测试结果")]
        public static readonly Property<string> ResultProperty = P<WipProductTestResult>.Register(e => e.Result);

        /// <summary>
        /// 测试结果
        /// </summary>
        public string Result
        {
            get { return GetProperty(ResultProperty); }
            set { SetProperty(ResultProperty, value); }
        }
        #endregion

        #region 采集记录 Process
        /// <summary>
        /// 采集记录Id
        /// </summary>
        public static readonly IRefIdProperty ProcessIdProperty = P<WipProductTestResult>.RegisterRefId(e => e.ProcessId, ReferenceType.Parent);

        /// <summary>
        /// 采集记录Id
        /// </summary>
        public double ProcessId
        {
            get { return (double)GetRefId(ProcessIdProperty); }
            set { SetRefId(ProcessIdProperty, value); }
        }

        /// <summary>
        /// 采集记录
        /// </summary>
        public static readonly RefEntityProperty<WipProductProcess> ProcessProperty = P<WipProductTestResult>.RegisterRef(e => e.Process, ProcessIdProperty);

        /// <summary>
        /// 采集记录
        /// </summary>
        public WipProductProcess Process
        {
            get { return GetRefEntity(ProcessProperty); }
            set { SetRefEntity(ProcessProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 产品测试结果 实体配置
    /// </summary>
    internal class WipProductTestResultConfig : EntityConfig<WipProductTestResult>
    {
        /// <summary>
        /// 配置数据库映射
        /// </summary>
		protected override void ConfigMeta()
        {
            Meta.MapTable("WIP_PROD_TST_RESULT").MapAllProperties();
            Meta.Property(WipProductTestResult.ProcessIdProperty).ColumnMeta.HasIndex();
            Meta.EnablePhantoms();
        }
    }
}
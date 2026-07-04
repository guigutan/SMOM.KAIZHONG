using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.MES.WIP.Pressure
{
    /// <summary>
    /// 耐压测试SN测试值
    /// </summary>
    [RootEntity, Serializable]
    [Label("耐压测试SN测试值")]
    [DisplayMember(nameof(Id))]
    public partial class WipPressureTestValue : DataEntity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public WipPressureTestValue()
        {
        }

        #region 耐压测试SN WipPressureSn
        /// <summary>
        /// 耐压测试SN Id
        /// </summary>
        [Label("耐压测试SN")]
        public static readonly IRefIdProperty WipPressureSnIdProperty =
            P<WipPressureTestValue>.RegisterRefId(e => e.WipPressureSnId, ReferenceType.Parent);

        /// <summary>
        /// 耐压测试SN Id
        /// </summary>
        public double WipPressureSnId
        {
            get { return (double)this.GetRefId(WipPressureSnIdProperty); }
            set { this.SetRefId(WipPressureSnIdProperty, value); }
        }

        /// <summary>
        /// 耐压测试SN
        /// </summary>
        public static readonly RefEntityProperty<WipPressureSn> WipPressureSnProperty =
            P<WipPressureTestValue>.RegisterRef(e => e.WipPressureSn, WipPressureSnIdProperty);

        /// <summary>
        /// 耐压测试SN
        /// </summary>
        public WipPressureSn WipPressureSn
        {
            get { return this.GetRefEntity(WipPressureSnProperty); }
            set { this.SetRefEntity(WipPressureSnProperty, value); }
        }
        #endregion

        #region 测试项目 TestItem
        /// <summary>
        /// 测试项目
        /// </summary>
        [Label("测试项目")]
        public static readonly Property<string> TestItemProperty = P<WipPressureTestValue>.Register(e => e.TestItem);

        /// <summary>
        /// 测试项目
        /// </summary>
        public string TestItem
        {
            get { return this.GetProperty(TestItemProperty); }
            set { this.SetProperty(TestItemProperty, value); }
        }
        #endregion

        #region 测试值 Value
        /// <summary>
        /// 测试值
        /// </summary>
        [Label("测试值")]
        public static readonly Property<string> ValueProperty = P<WipPressureTestValue>.Register(e => e.Value);

        /// <summary>
        /// 测试值
        /// </summary>
        public string Value
        {
            get { return this.GetProperty(ValueProperty); }
            set { this.SetProperty(ValueProperty, value); }
        }
        #endregion

        #region 测试结果 TestResult
        /// <summary>
        /// 测试结果
        /// </summary>
        [Label("测试结果")]
        public static readonly Property<TestResult?> TestResultProperty = P<WipPressureTestValue>.Register(e => e.TestResult);

        /// <summary>
        /// 测试结果
        /// </summary>
        public TestResult? TestResult
        {
            get { return this.GetProperty(TestResultProperty); }
            set { this.SetProperty(TestResultProperty, value); }
        }
        #endregion

        #region 视图属性

        #endregion
    }

    /// <summary>
    /// 实体配置
    /// </summary>
    internal class WipPressureTestValueEntityConfig : EntityConfig<WipPressureTestValue>
    {
        /// <summary>
        /// 对Meta属性的配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("WIP_PRESSURE_TEST_VAL").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}
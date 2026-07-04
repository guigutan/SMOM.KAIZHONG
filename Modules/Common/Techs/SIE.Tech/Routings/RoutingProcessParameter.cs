using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Tech.Processs;
using System;

namespace SIE.Tech.Routings
{
    /// <summary>
    /// 工序清单参数
    /// </summary>
    [ChildEntity, Serializable]
    [Label("工序清单参数")]
    public partial class RoutingProcessParameter : DataEntity
    {
        #region 规则Id RuleId
        /// <summary>
        /// 规则Id
        /// </summary>
        [MaxLength(80)]
        public static readonly Property<string> RuleIdProperty = P<RoutingProcessParameter>.Register(e => e.RuleId);

        /// <summary>
        /// 规则Id
        /// </summary>
        public string RuleId
        {
            get { return this.GetProperty(RuleIdProperty); }
            set { this.SetProperty(RuleIdProperty, value); }
        }
        #endregion

        #region 描述 Description
        /// <summary>
        /// 描述
        /// </summary>
        public static readonly Property<string> DescriptionProperty = P<RoutingProcessParameter>.Register(e => e.Description);

        /// <summary>
        /// 描述
        /// </summary>
        public string Description
        {
            get { return GetProperty(DescriptionProperty); }
            set { SetProperty(DescriptionProperty, value); }
        }
        #endregion

        #region 下一个工序 NextProcess
        /// <summary>
        /// 下一个工序Id
        /// </summary>
        public static readonly IRefIdProperty NextProcessIdProperty = P<RoutingProcessParameter>.RegisterRefId(e => e.NextProcessId, ReferenceType.Normal);

        /// <summary>
        /// 下一个工序Id
        /// </summary>
        public double? NextProcessId
        {
            get { return (double?)GetRefNullableId(NextProcessIdProperty); }
            set { SetRefNullableId(NextProcessIdProperty, value); }
        }

        /// <summary>
        /// 下一个工序
        /// </summary>
        public static readonly RefEntityProperty<RoutingProcess> NextProcessProperty = P<RoutingProcessParameter>.RegisterRef(e => e.NextProcess, NextProcessIdProperty);

        /// <summary>
        /// 下一个工序
        /// </summary>
        public RoutingProcess NextProcess
        {
            get { return GetRefEntity(NextProcessProperty); }
            set { SetRefEntity(NextProcessProperty, value); }
        }
        #endregion

        #region 参数 Type
        /// <summary>
        /// 参数
        /// </summary>
        public static readonly Property<ResultTypeForDesign> TypeProperty = P<RoutingProcessParameter>.Register(e => e.Type);

        /// <summary>
        /// 参数
        /// </summary>
        public ResultTypeForDesign Type
        {
            get { return GetProperty(TypeProperty); }
            set { SetProperty(TypeProperty, value); }
        }
        #endregion

        #region 脚本 Expression
        /// <summary>
        /// 脚本
        /// </summary>
        [Label("脚本")]
        [MaxLength(4000)]
        public static readonly Property<string> ExpressionProperty = P<RoutingProcessParameter>.Register(e => e.Expression);

        /// <summary>
        /// 脚本
        /// </summary>
        public string Expression
        {
            get { return this.GetProperty(ExpressionProperty); }
            set { this.SetProperty(ExpressionProperty, value); }
        }
        #endregion

        #region 跳转条件 Condition
        /// <summary>
        /// 跳转条件
        /// </summary>
        [Label("跳转条件")]
        public static readonly Property<string> ConditionProperty = P<RoutingProcessParameter>.Register(e => e.Condition);

        /// <summary>
        /// 跳转条件
        /// </summary>
        public string Condition
        {
            get { return this.GetProperty(ConditionProperty); }
            set { this.SetProperty(ConditionProperty, value); }
        }
        #endregion 

        #region 工序清单 RoutingProcess
        /// <summary>
        /// 工序清单Id
        /// </summary>
        public static readonly IRefIdProperty RoutingProcessIdProperty = P<RoutingProcessParameter>.RegisterRefId(e => e.RoutingProcessId, ReferenceType.Parent);

        /// <summary>
        /// 工序清单Id
        /// </summary>
        public double RoutingProcessId
        {
            get { return (double)GetRefId(RoutingProcessIdProperty); }
            set { SetRefId(RoutingProcessIdProperty, value); }
        }

        /// <summary>
        /// 工序清单
        /// </summary>
        public static readonly RefEntityProperty<RoutingProcess> RoutingProcessProperty = P<RoutingProcessParameter>.RegisterRef(e => e.RoutingProcess, RoutingProcessIdProperty);

        /// <summary>
        /// 工序清单
        /// </summary>
        public RoutingProcess RoutingProcess
        {
            get { return GetRefEntity(RoutingProcessProperty); }
            set { SetRefEntity(RoutingProcessProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 工序清单参数 实体配置
    /// </summary>
    internal class RoutingProcessParameterConfig : EntityConfig<RoutingProcessParameter>
    {
        /// <summary>
        /// 元数据配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("TECH_RT_PROC_PARAM").MapAllProperties();
            Meta.Property(RoutingProcessParameter.ExpressionProperty).ColumnMeta.HasLength(4000);
            Meta.Property(RoutingProcessParameter.ConditionProperty).ColumnMeta.HasLength(4000);
            Meta.Property(RoutingProcessParameter.RoutingProcessIdProperty).ColumnMeta.HasIndex();
            Meta.Property(RoutingProcessParameter.RuleIdProperty).ColumnMeta.HasLength(320);

            Meta.EnablePhantoms();
        }
    }
}
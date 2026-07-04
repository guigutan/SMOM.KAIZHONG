using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Tech.Processs.Scripts
{
    /// <summary>
    /// 脚本条件
    /// </summary>
    [RootEntity, Serializable]
    //[CriteriaQuery]
    [Label("脚本条件")]
    public partial class ScriptCondition : DataEntity
    {
        #region 条件组 Group
        /// <summary>
        /// 条件组
        /// </summary>
        [Label("条件组")]
        public static readonly Property<string> GroupProperty = P<ScriptCondition>.Register(e => e.Group);

        /// <summary>
        /// 条件组
        /// </summary>
        public string Group
        {
            get { return GetProperty(GroupProperty); }
            set { SetProperty(GroupProperty, value); }
        }
        #endregion

        #region 条件 Condition
        /// <summary>
        /// 条件
        /// </summary>
        [Required]
        [Label("条件")]
        public static readonly Property<string> ConditionProperty = P<ScriptCondition>.Register(e => e.Condition);

        /// <summary>
        /// 条件
        /// </summary>
        public string Condition
        {
            get { return GetProperty(ConditionProperty); }
            set { SetProperty(ConditionProperty, value); }
        }
        #endregion

        #region 值 Value
        /// <summary>
        /// 值
        /// </summary>
        [Required]
        [Label("值")]
        public static readonly Property<string> ValueProperty = P<ScriptCondition>.Register(e => e.Value);

        /// <summary>
        /// 值
        /// </summary>
        public string Value
        {
            get { return GetProperty(ValueProperty); }
            set { SetProperty(ValueProperty, value); }
        }
        #endregion

        #region 路径 Path
        /// <summary>
        /// 路径
        /// </summary>
        [Required]
        [Label("路径")]
        public static readonly Property<string> PathProperty = P<ScriptCondition>.Register(e => e.Path);

        /// <summary>
        /// 路径
        /// </summary>
        public string Path
        {
            get { return GetProperty(PathProperty); }
            set { SetProperty(PathProperty, value); }
        }
        #endregion

        #region 顺序 Index
        /// <summary>
        /// 顺序
        /// </summary>
        [Label("顺序")]
        public static readonly Property<int> IndexProperty = P<ScriptCondition>.Register(e => e.Index);

        /// <summary>
        /// 顺序
        /// </summary>
        public int Index
        {
            get { return GetProperty(IndexProperty); }
            set { SetProperty(IndexProperty, value); }
        }
        #endregion

        #region 参数ID ParameterId
        /// <summary>
        /// 参数ID
        /// </summary>
        [Label("参数ID")]
        public static readonly Property<double> ParameterIdProperty = P<ScriptCondition>.Register(e => e.ParameterId);

        /// <summary>
        /// 参数ID
        /// </summary>
        public double ParameterId
        {
            get { return GetProperty(ParameterIdProperty); }
            set { SetProperty(ParameterIdProperty, value); }
        }
        #endregion

        #region 关系 Relation
        /// <summary>
        /// 关系
        /// </summary>
        [Label("关系")]
        public static readonly Property<OperatorRelation?> RelationProperty = P<ScriptCondition>.Register(e => e.Relation);

        /// <summary>
        /// 关系
        /// </summary>
        public OperatorRelation? Relation
        {
            get { return GetProperty(RelationProperty); }
            set { SetProperty(RelationProperty, value); }
        }
        #endregion

        #region 目标类型 TargetType
        /// <summary>
        /// 目标类型
        /// </summary>
        [Label("目标类型")]
        public static readonly Property<TargetType> TargetTypeProperty = P<ScriptCondition>.Register(e => e.TargetType);

        /// <summary>
        /// 目标类型
        /// </summary>
        public TargetType TargetType
        {
            get { return GetProperty(TargetTypeProperty); }
            set { SetProperty(TargetTypeProperty, value); }
        }
        #endregion

        #region 比较符 Operators
        /// <summary>
        /// 比较符
        /// </summary>
        [Label("比较符")]
        public static readonly Property<Operators> OperatorsProperty = P<ScriptCondition>.Register(e => e.Operators);

        /// <summary>
        /// 比较符
        /// </summary>
        public Operators Operators
        {
            get { return GetProperty(OperatorsProperty); }
            set { SetProperty(OperatorsProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 脚本条件 实体配置
    /// </summary>
    internal class ScriptConditionConfig : EntityConfig<ScriptCondition>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("TECH_SCRIPT_CND").MapAllProperties();
            Meta.EnablePhantoms();
            Meta.SupportTree();
        }
    }
}
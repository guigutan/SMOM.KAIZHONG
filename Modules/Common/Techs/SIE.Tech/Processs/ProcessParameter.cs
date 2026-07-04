using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Tech.Processs
{
    /// <summary>
    /// 工序参数
    /// </summary>
    [ChildEntity, Serializable]
    [Label("工序参数")]
    [DisplayMember(nameof(Type))]
    public partial class ProcessParameter : DataEntity
    {
        #region 描述 Description
        /// <summary>
        /// 描述
        /// </summary>
        [Required]
        [Label("结果描述")]
        public static readonly Property<string> DescriptionProperty = P<ProcessParameter>.Register(e => e.Description);

        /// <summary>
        /// 描述
        /// </summary>
        public string Description
        {
            get { return GetProperty(DescriptionProperty); }
            set { SetProperty(DescriptionProperty, value); }
        }
        #endregion

        #region 采集结果 Type
        /// <summary>
        /// 采集结果
        /// </summary>
        [Label("结果")]
        public static readonly Property<ResultTypeForDesign> TypeProperty = P<ProcessParameter>.Register(e => e.Type);

        /// <summary>
        /// 采集结果
        /// </summary>
        public ResultTypeForDesign Type
        {
            get { return GetProperty(TypeProperty); }
            set { SetProperty(TypeProperty, value); }
        }
        #endregion

        #region 工序 Process
        /// <summary>
        /// 工序Id
        /// </summary>
        [Label("工序")]
        public static readonly IRefIdProperty ProcessIdProperty = P<ProcessParameter>.RegisterRefId(e => e.ProcessId, ReferenceType.Parent);

        /// <summary>
        /// 工序Id
        /// </summary>
        public double ProcessId
        {
            get { return (double)GetRefId(ProcessIdProperty); }
            set { SetRefId(ProcessIdProperty, value); }
        }

        /// <summary>
        /// 工序
        /// </summary>
        public static readonly RefEntityProperty<Process> ProcessProperty = P<ProcessParameter>.RegisterRef(e => e.Process, ProcessIdProperty);

        /// <summary>
        /// 工序
        /// </summary>
        public Process Process
        {
            get { return GetRefEntity(ProcessProperty); }
            set { SetRefEntity(ProcessProperty, value); }
        }
        #endregion

        #region 脚本 Script
        /// <summary>
        /// 脚本
        /// </summary>
        [Label("脚本")]
        [MaxLength(4000)]
        public static readonly Property<string> ScriptProperty = P<ProcessParameter>.Register(e => e.Script);

        /// <summary>
        /// 脚本
        /// </summary>
        public string Script
        {
            get { return this.GetProperty(ScriptProperty); }
            set { this.SetProperty(ScriptProperty, value); }
        }
        #endregion

        #region 跳转条件 Condition
        /// <summary>
        /// 跳转条件
        /// </summary>
        [Label("跳转条件")]
        public static readonly Property<string> ConditionProperty = P<ProcessParameter>.Register(e => e.Condition);

        /// <summary>
        /// 跳转条件
        /// </summary>
        public string Condition
        {
            get { return this.GetProperty(ConditionProperty); }
            set { this.SetProperty(ConditionProperty, value); }
        }
        #endregion 

        /// <summary>
        /// 属性变更
        /// </summary>
        /// <param name="propertyName">变更属性名称</param>
        protected override void OnPropertyChanged(string propertyName)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == nameof(Type))
            {
                if (Type != ResultTypeForDesign.Custom)
                {
                    Script = string.Empty;
                    Description = Type.ToLabel();
                }
            }
        }
    }

    /// <summary>
    /// 工序参数 实体配置
    /// </summary>
    internal class ProcessParameterConfig : EntityConfig<ProcessParameter>
    {
        /// <summary>
        /// 配置数据库的映射
        /// </summary>
		protected override void ConfigMeta()
        {
            Meta.MapTable("TECH_PROC_PARAM").MapAllProperties();
            Meta.Property(ProcessParameter.ScriptProperty).ColumnMeta.HasLength(4000);
            Meta.Property(ProcessParameter.ConditionProperty).ColumnMeta.HasLength(4000);
            Meta.Property(ProcessParameter.ProcessIdProperty).ColumnMeta.HasIndex();
            Meta.EnablePhantoms();
        }
    }
}
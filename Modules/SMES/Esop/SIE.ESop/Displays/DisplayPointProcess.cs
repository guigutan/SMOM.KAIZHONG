using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Tech.Processs;
using System;

namespace SIE.ESop.Displays
{
    /// <summary>
    /// 工位显示点关系
    /// </summary>
    [ChildEntity, Serializable]
    [Label("工位显示点关系")]
    [DisplayMember(nameof(DisplayPointProcess.ProcessId))]
    public partial class DisplayPointProcess : DataEntity
    {
        #region 工序 Process
        /// <summary>
        /// 工序ID
        /// </summary>
        [Required]
        [Label("工序")]
        public static readonly IRefIdProperty ProcessIdProperty = P<DisplayPointProcess>.RegisterRefId(e => e.ProcessId, ReferenceType.Normal);

        /// <summary>
        /// 工序ID
        /// </summary>
        public double ProcessId
        {
            get { return (double)this.GetRefId(ProcessIdProperty); }
            set { this.SetRefId(ProcessIdProperty, value); }
        }

        /// <summary>
        /// 工序
        /// </summary>
        public static readonly RefEntityProperty<Process> ProcessProperty = P<DisplayPointProcess>.RegisterRef(e => e.Process, ProcessIdProperty);

        /// <summary>
        /// 工序
        /// </summary>
        public Process Process
        {
            get { return this.GetRefEntity(ProcessProperty); }
            set { this.SetRefEntity(ProcessProperty, value); }
        }
        #endregion

        #region 显示点 DisplayPoint
        /// <summary>
        /// 显示点Id
        /// </summary>
        [Required]
        public static readonly IRefIdProperty DisplayPointIdProperty = P<DisplayPointProcess>.RegisterRefId(e => e.DisplayPointId, ReferenceType.Parent);

        /// <summary>
        /// 显示点Id
        /// </summary>
        public double DisplayPointId
        {
            get { return (double)GetRefId(DisplayPointIdProperty); }
            set { SetRefId(DisplayPointIdProperty, value); }
        }

        /// <summary>
        /// 显示点
        /// </summary> 
        public static readonly RefEntityProperty<DisplayPoint> DisplayPointProperty = P<DisplayPointProcess>.RegisterRef(e => e.DisplayPoint, DisplayPointIdProperty);

        /// <summary>
        /// 显示点
        /// </summary>
        public DisplayPoint DisplayPoint
        {
            get { return GetRefEntity(DisplayPointProperty); }
            set { SetRefEntity(DisplayPointProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 工位显示点关系 实体配置
    /// </summary>
    internal class DisplayPointStationConfig : EntityConfig<DisplayPointProcess>
    {
        /// <summary>
        /// 数据库配置映射
        /// </summary>
		protected override void ConfigMeta()
        {
            Meta.MapTable("ESOP_POINT_PROCESS").MapAllProperties();
            Meta.Property(DisplayPointProcess.DisplayPointIdProperty).ColumnMeta.HasIndex();
            Meta.EnablePhantoms();
        }
    }
}
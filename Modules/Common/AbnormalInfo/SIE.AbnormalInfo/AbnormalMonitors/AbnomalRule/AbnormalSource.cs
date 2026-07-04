using SIE;
using SIE.Domain;
using SIE.Mda.Metadatas.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.AbnormalInfo.AbnormalMonitors
{
	/// <summary>
	/// 异常来源
	/// </summary>
	[RootEntity, Serializable]
	[CriteriaQuery]
	[Label("异常来源")]
	[DisplayMember(nameof(MonitorName))]

    public partial class AbnormalSource : DataEntity
	{

		#region 监控名称 MonitorName
		/// <summary>
		/// 监控名称
		/// </summary>
		[Label("监控名称")]
		[Required]
		public static readonly Property<string> MonitorNameProperty = P<AbnormalSource>.Register(e => e.MonitorName);

		/// <summary>
		/// 监控名称
		/// </summary>
		public string MonitorName
		{
			get { return GetProperty(MonitorNameProperty); }
			set { SetProperty(MonitorNameProperty, value); }
		}
		#endregion

		#region 监控实体源 AbnormalEntityMetadata
		/// <summary>
		/// 监控实体源
		/// </summary>
		public static readonly IRefIdProperty AbnormalEntityMetadataIdProperty = P<AbnormalSource>.RegisterRefId(e => e.AbnormalEntityMetadataId, ReferenceType.Normal);

		/// <summary>
		/// 监控实体源
		/// </summary>
		public double AbnormalEntityMetadataId
		{
			get { return (double)GetRefId(AbnormalEntityMetadataIdProperty); }
			set { SetRefId(AbnormalEntityMetadataIdProperty, value); }
		}

		/// <summary>
		/// 监控实体源
		/// </summary>
		public static readonly RefEntityProperty<AbnormalEntityMetadata> AbnormalEntityMetadataProperty = P<AbnormalSource>.RegisterRef(e => e.AbnormalEntityMetadata, AbnormalEntityMetadataIdProperty);

		/// <summary>
		/// 异常判定规则
		/// </summary>
		public AbnormalEntityMetadata AbnormalEntityMetadata
		{
			get { return GetRefEntity(AbnormalEntityMetadataProperty); }
			set { SetRefEntity(AbnormalEntityMetadataProperty, value); }
		}
        #endregion

        #region 视图属性

        #region 功能模块 MetadataName
        /// <summary>
        /// 功能模块
        /// </summary>
        [Label("功能模块")]
        public static readonly Property<string> MetadataNameProperty = P<AbnormalSource>.RegisterView(e => e.MetadataName, p => p.AbnormalEntityMetadata.Name);

        /// <summary>
        /// 功能模块
        /// </summary>
        public string MetadataName
        {
            get { return this.GetProperty(MetadataNameProperty); }
        }
        #endregion

        #region 数据表名 TabName
        /// <summary>
        /// 数据表名
        /// </summary>
        [Label("数据表名")]
		public static readonly Property<string> TabNameProperty = P<AbnormalSource>.RegisterView(e => e.TabName, p => p.AbnormalEntityMetadata.TableName);

		/// <summary>
		/// 数据表名
		/// </summary>
		public string TabName
		{
			get { return this.GetProperty(TabNameProperty); }
		}
		#endregion

		#region 监控类型 MonitorType
		/// <summary>
		/// 监控类型
		/// </summary>
		[Label("监控类型")]
		public static readonly Property<string> MonitorTypeProperty = P<AbnormalSource>.RegisterView(e => e.MonitorType, p => p.AbnormalEntityMetadata.Type);

		/// <summary>
		/// 监控类型
		/// </summary>
		public string MonitorType
		{
			get { return this.GetProperty(MonitorTypeProperty); }
		}
		#endregion
		#endregion
	}

	/// <summary>
	/// 异常来源 实体配置
	/// </summary>
	internal class AbnormalSourceConfig : EntityConfig<AbnormalSource>
	{
		/// <summary>
      	  	/// 配置元数据
    	    	/// </summary>
		protected override void ConfigMeta()
		{
			Meta.MapTable("ABNOMAL_SOURCE").MapAllProperties();
			Meta.EnablePhantoms();
		}
	}
}
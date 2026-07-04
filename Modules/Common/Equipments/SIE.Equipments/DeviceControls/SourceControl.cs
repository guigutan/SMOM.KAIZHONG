using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Equipments.DeviceControls
{
    /// <summary>
    /// 设备控制来源清单
    /// </summary>
    [RootEntity, Serializable]
    [CriteriaQuery]
    [Label("设备控制来源清单")]
    public partial class SourceControl : DataEntity
    {
        #region 来源 Source
        /// <summary>
        /// 来源
        /// </summary>
        [Label("来源")]
        [Required]
        [NotDuplicate]
        public static readonly Property<string> SourceProperty = P<SourceControl>.Register(e => e.Source);

        /// <summary>
        /// 来源
        /// </summary>
        public string Source
        {
            get { return GetProperty(SourceProperty); }
            set { SetProperty(SourceProperty, value); }
        }
        #endregion

        #region 描述 Description
        /// <summary>
        /// 描述
        /// </summary>
        [Label("描述")]
        public static readonly Property<string> DescriptionProperty = P<SourceControl>.Register(e => e.Description);

        /// <summary>
        /// 描述
        /// </summary>
        public string Description
        {
            get { return GetProperty(DescriptionProperty); }
            set { SetProperty(DescriptionProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 设备控制来源清单 实体配置
    /// </summary>
    internal class SourceControlConfig : EntityConfig<SourceControl>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_CONTROL_SOURCE").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}

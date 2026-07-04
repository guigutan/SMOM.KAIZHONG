using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.Checks.Projects
{
    /// <summary>
	/// 项目图片
	/// </summary>
	[RootEntity, Serializable]
    [CriteriaQuery]
    [Label("项目图片")]
    public partial class ProjectPhoto : DataEntity
    {
        #region 图片 Photo
        /// <summary>
        /// 图片
        /// </summary>
        [Label("图片")]
        public static readonly Property<byte[]> PhotoProperty = P<ProjectPhoto>.Register(e => e.Photo);

        /// <summary>
        /// 图片
        /// </summary>
        public byte[] Photo
        {
            get { return this.GetProperty(PhotoProperty); }
            set { this.SetProperty(PhotoProperty, value); }
        }
        #endregion
    }
    /// <summary>
    /// 项目图片 实体配置
    /// </summary>
    internal class ProjectPhotoConfig : EntityConfig<ProjectPhoto>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_PROJECT_PHOTO").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}

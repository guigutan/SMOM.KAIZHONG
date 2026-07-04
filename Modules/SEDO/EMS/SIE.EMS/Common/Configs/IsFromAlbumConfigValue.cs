using SIE.Common.Configs;
using SIE.Domain;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.EMS.Common.Configs
{
    /// <summary>
    /// 配置实体
    /// </summary>
    [RootEntity, Serializable]
    [Label("是否允许从相册选择图片")]
    public class IsFromAlbumConfigValue : ConfigValue
    {
        #region 是否允许从相册选择图片 IsFromAlbum
        /// <summary>
        /// 是否允许从相册选择图片
        /// </summary>
        [Label("是否允许从相册选择图片")]
        public static readonly Property<bool> IsFromAlbumProperty = P<IsFromAlbumConfigValue>.Register(e => e.IsFromAlbum);

        /// <summary>
        /// 是否允许从相册选择图片
        /// </summary>
        public bool IsFromAlbum
        {
            get { return this.GetProperty(IsFromAlbumProperty); }
            set { this.SetProperty(IsFromAlbumProperty, value); }
        }
        #endregion


        /// <summary>
        /// 把值显示出来
        /// </summary>
        /// <returns>string</returns>
        public override string Display()
        {
            string display = IsFromAlbum ? "是".L10N() : "否".L10N();
            return display;
        }
    }
}

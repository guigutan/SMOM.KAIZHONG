using SIE.Common.Configs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.EMS.Common.Configs
{
    /// <summary>
    /// 配置
    /// </summary>
    [System.ComponentModel.DisplayName("是否允许从相册选择图片")]
    [System.ComponentModel.Description("是否允许从相册选择图片")]
    public class IsFromAlbumConfig : GlobalConfig<IsFromAlbumConfigValue>
    {
        /// <summary>
        /// 默认值
        /// </summary>
        readonly IsFromAlbumConfigValue defaultValue = new IsFromAlbumConfigValue { IsFromAlbum = false };

        /// <summary>
        /// 默认值
        /// </summary>
        public override IsFromAlbumConfigValue DefaultValue
        {
            get { return defaultValue; }
        }
    }
}

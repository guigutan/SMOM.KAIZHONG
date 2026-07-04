using SIE.EMS.Common.Configs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.EMS.Common
{
    /// <summary>
    /// 是否从相册选择图片配置项视图配置
    /// </summary>
    public class IsFromAlbumConfigValueViewConfig : WebViewConfig<IsFromAlbumConfigValue>
    {
        /// <summary>
        /// 明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.Property(p => p.IsFromAlbum).Show(ShowInWhere.All);
        }
    }
}

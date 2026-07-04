using SIE.Api;
using SIE.Common.Configs;
using SIE.Domain.Validation;
using SIE.EMS.Common.Configs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.EMS.Common.Controller
{
    /// <summary>
    /// Ems基础控制器
    /// </summary>
    public class EmsCommonController :DomainController
    {
        /// <summary>
        /// EDO图片控件是否允许从相册选择图片
        /// </summary>
        /// <returns></returns>
        [ApiService("是否允许从照片选择")]
        public virtual bool EmsIsFromAlbum()
        {
            var config = ConfigService.GetConfig(new IsFromAlbumConfig());
            if (config == null)
            {
                throw new ValidationException("未找到EDO是否允许从相册选择图片配置项，请检查配置".L10N());
            }
            return config.IsFromAlbum;
        }
    }
}

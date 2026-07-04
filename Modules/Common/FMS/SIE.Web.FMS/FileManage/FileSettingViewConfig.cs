using SIE.FMS;
using SIE.FMS.FileManages;

namespace SIE.Web.FMS
{
    /// <summary>
    /// 文件管理 视图配置
    /// </summary>
    internal class FileSettingViewConfig : WebViewConfig<FileSetting>
    {
        /// <summary>
        /// 配置默认视图
        /// </summary>
        protected override void ConfigView()
        {
            //默认视图
        }

        protected override void ConfigDetailsView()
        {            
            View.Property(p => p.IsOA);
            View.Property(p => p.PusherId).UseDataSource((e,p,k)=> {
                return RT.Service.Resolve<FileManageController>().GetPusher(p,k);
            });
            View.Property(p => p.AuditMans).Readonly();
            View.Property(p => p.VersionHead);
        }
    }
}

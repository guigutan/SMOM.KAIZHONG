using SIE.ERPInterface.Common.InfDataEntitys.Download;

namespace SIE.Web.ERPInterface.Common
{
    /// <summary>
    /// 下载接口中间表基类视图配置
    /// </summary>
    internal class DownloadBaseEntityViewConfig : WebViewConfig<DownloadBaseEntity>
    {
        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.State).HasOrderNo(1);
                View.Property(p => p.ProcessDate).Readonly().HasOrderNo(2);
                View.Property(p => p.ErpKey).Readonly().HasOrderNo(3);
                View.Property(p => p.RetryCount).HasOrderNo(4);
                View.Property(p => p.MaxRetryCount).Readonly().HasOrderNo(5);
                View.Property(p => p.LastUpdateDate).Readonly().HasOrderNo(6);
                View.Property(p => p.IsDelete).HasOrderNo(7);
                View.Property(p => p.IsManual).HasOrderNo(8);
            }
        }

        ///<summary>
        /// 配置查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.State);
                View.Property(p => p.ProcessDate);
                View.Property(p => p.ErpKey);
                View.Property(p => p.LastUpdateDate);
                View.Property(p => p.IsDelete);
            }
        }
    }
}
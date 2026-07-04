using SIE.Tech.Stations.Configs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.Tech.Stations.Configs
{
    /// <summary>
    /// 是否称重配置值视图配置
    /// </summary>
    public class DirectWeightConfigValueViewConfig : WebViewConfig<DirectWeightConfigValue>
    {
        /// <summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.Property(p => p.IsWeight);
        }
    }

    /// <summary>
    /// 包装采集配置值视图配置
    /// </summary>
    public class DirectWipPackingConfigValueViewConfig : WebViewConfig<DirectWipPackingConfigValue>
    {
        /// <summary>
        /// 通用视图配置
        /// </summary>
        protected override void ConfigView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.AutoDoPackingMode).Show(ShowInWhere.All);
                View.Property(p => p.IsAutoPrintPackageLabel).Show(ShowInWhere.All);
                View.Property(p => p.IsContinuityControl).Show(ShowInWhere.All);
                View.Property(p => p.PackingRuleValidMode).Show(ShowInWhere.All).HasLabel("验证方式");
            }
        }
    }
    /// <summary>
    /// 新包装条码打印方式配置值视图配置
    /// </summary>
    public class NewPackingPrintModeConfigValueViewConfig : WebViewConfig<NewPackingPrintModeConfigValue>
    {
        /// <summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.Property(p => p.PrintMode);
        }
    }

    /// <summary>
    /// 包装条码打印方式配置值视图配置
    /// </summary>
    public class DirectPackingPrintModeConfigValueViewConfig : WebViewConfig<DirectPackingPrintModeConfigValue>
    {
        /// <summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.Property(p => p.PrintMode);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class DirectWipPackingBillConfigValueViewConfig : WebViewConfig<DirectWipPackingBillConfigValue>
    {
        /// <summary>
        /// 通用视图配置
        /// </summary>
        protected override void ConfigView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.Warehouse).Show(ShowInWhere.All);
            }
        }
    }
}

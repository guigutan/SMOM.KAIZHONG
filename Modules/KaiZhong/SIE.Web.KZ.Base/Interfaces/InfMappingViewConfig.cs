using SIE.KZ.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.KZ.Base.Interfaces
{
    /// <summary>
    /// 基础数据接口信息 实体视图配置
    /// </summary>
    public class InfMappingViewConfig : WebViewConfig<InfMapping>
    {
        /// <summary>
        /// 通用视图配置
        /// </summary>
        protected override void ConfigView()
        {

        }

        /// <summary>
        ///  默认列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseDefaultCommands();
            using (View.OrderProperties())
            {
                View.Property(p => p.InfType).HasLabel("接口名称").Show(ShowInWhere.All);
                View.Property(p => p.InfCode).HasLabel("接口代码").Show(ShowInWhere.All);
                View.Property(p => p.ApiType).Show(ShowInWhere.All);
                View.Property(p => p.Method).Show(ShowInWhere.All);
                View.Property(p => p.Remark).HasLabel("备注").Show(ShowInWhere.All);
            }
        }

        /// <summary>
        /// 默认下拉选择视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.InfType).HasLabel("接口名称").Show(ShowInWhere.All);
            View.Property(p => p.InfCode).HasLabel("接口代码").Show(ShowInWhere.All);
            View.Property(p => p.Remark).HasLabel("备注").Show(ShowInWhere.All);
        }

        /// <summary>
        /// 默认查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.InfType).HasLabel("接口名称").Show(ShowInWhere.All);
                View.Property(p => p.InfCode).HasLabel("接口代码").Show(ShowInWhere.All);
                View.Property(p => p.Remark).HasLabel("备注").Show(ShowInWhere.All);
            }
        }
    }
}

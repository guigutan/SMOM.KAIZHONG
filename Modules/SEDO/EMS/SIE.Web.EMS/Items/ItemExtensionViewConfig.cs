using SIE.EMS.Items;
using SIE.Items;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.EMS.Items
{
    /// <summary>
    /// 视图配置
    /// </summary>
    public class ItemExtensionViewConfig : WebViewConfig<Item>
    {
        /// <summary>
        /// 基本资料
        /// </summary>
        public const string BaseDataViewGroup = "BaseDataViewGroup";

        /// <summary>
        /// 默认视图
        /// </summary>
        protected override void ConfigView()
        {
            View.HasDelegate(Item.CodeProperty);
            View.DeclareExtendViewGroup(new string[] { BaseDataViewGroup});

            if (ViewGroup == BaseDataViewGroup)
            {
                View.DomainName("物料基本资料");
                BaseDataView();
            }
        }

        /// <summary>
        /// 资本资料
        /// </summary>
        protected void BaseDataView()
        {
            View.Property(ItemExtension.SpartTypeProperty).HasLabel("备件类型").Show(ShowInWhere.All).HasOrderNo(100);
        }
    }
}

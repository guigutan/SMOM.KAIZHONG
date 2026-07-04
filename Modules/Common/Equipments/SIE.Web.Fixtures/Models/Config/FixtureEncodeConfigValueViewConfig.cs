using SIE.Common.Catalogs;
using SIE.Fixtures.Models.Config;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.Fixtures.Models.Config
{
    /// <summary>
    /// 工治具编码配置项视图配置
    /// </summary>
    public class FixtureEncodeConfigValueViewConfig : WebViewConfig<FixtureEncodeConfigValue>
    {
        /// <summary>
        /// 配置明细列表
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.Property(p => p.WareHouseTypeName).Show(ShowInWhere.List).
            UsePagingLookUpGridPopupEditor(p =>
            {
                p.Model = typeof(Catalog).FullName;
                p.XType = "mutilcatalogeditor_encodewarehouse";
                p.DisplayField = "Name";
                Dictionary<string, string> dic = new Dictionary<string, string>();
                dic.Add(FixtureEncodeConfigValue.WareHouseTypeCodeProperty.Name, Catalog.CodeProperty.Name);
                dic.Add(FixtureEncodeConfigValue.WareHouseTypeIdsProperty.Name, Catalog.IdProperty.Name);
                p.MutiLinkField = dic.ToJsonString();
            }).DisableSort().HasLabel("工治具仓库类别");
            View.Property(c => c.WareHouseTypeIds).Show(ShowInWhere.Hide);
        }
    }
}

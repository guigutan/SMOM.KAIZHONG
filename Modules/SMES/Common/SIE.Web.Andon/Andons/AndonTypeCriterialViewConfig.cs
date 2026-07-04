using SIE.Andon.Andons;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.Andon.Andons
{
    /// <summary>
    /// 安灯类型维护查询视图配置
    /// </summary>
    public class AndonTypeCriterialViewConfig : WebViewConfig<AndonTypeCriterial>
    {
        /// <summary>
        /// 查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.UseDefaultCommands();
            View.Property(p => p.AndonTypeCode).Show(ShowInWhere.All);
            View.Property(p => p.AndonTypeName).Show(ShowInWhere.All);
            View.Property(p => p.AndonTypeClass).Show(ShowInWhere.All);
            View.Property(p => p.State).Show(ShowInWhere.All);
            View.Property(p => p.CreateTime).UseDateRangeEditor(p => p.DateRangeType = ObjectModel.DateRangeType.All).Show(ShowInWhere.All);

        }
    }
}

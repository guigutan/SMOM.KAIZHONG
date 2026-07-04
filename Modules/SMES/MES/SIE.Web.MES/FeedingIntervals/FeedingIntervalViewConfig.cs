using SIE.MES.FeedingIntervals;
using SIE.MetaModel.View;
using SIE.Web.MES.BlueLable.Commands;
using System;
using System.Collections.Generic;


namespace SIE.Web.MES.FeedingIntervals
{
    /// <summary>
    /// 上料间隔时间 WEB显示界面 配置
    /// </summary>
    public class FeedingIntervalViewConfig : WebViewConfig<FeedingInterval>
    {
        /// <summary>
        /// 
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseDefaultCommands();
            View.UseCommands(typeof(BlueLableLevelImportCommand).FullName, typeof(BlueLableLevelDLTemplateCommand).FullName);
            using (View.OrderProperties())
            {
                View.Property(p => p.Item).UsePagingLookUpEditor((m, e) =>
                {
                    Dictionary<string, string> keyValues = new Dictionary<string, string>();
                    keyValues.Add(nameof(e.ItemName), nameof(e.Item.Name));
                    m.DicLinkField = keyValues;
                }).ShowInList(width: 200);
               
                View.Property(p => p.IntervalSecond).ShowInList(width: 150);              

            }
        }
    }
}

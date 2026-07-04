using SIE.MES.Capacitys;
using SIE.MetaModel.View;
using SIE.Web.MES.Capacitys.Commands;
using System.Collections.Generic;

namespace SIE.Web.MES.Capacitys
{
    /// <summary>
    /// 标准产能维护 视图配置
    /// </summary>
    public class StandardCapacityViewConfig : WebViewConfig<StandardCapacity>
    {
        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseDefaultCommands();
            View.UseCommands(WebCommandNames.ExportXls, WebCommandNames.ExportXlsAll);
            View.UseCommands(typeof(StandardCapacityImportCommand).FullName, typeof(StandardCapacityDLTemplateCommand).FullName);
            using (View.OrderProperties())
            {
                
                View.Property(p => p.Item).UsePagingLookUpEditor((m, e) =>
                {
                    Dictionary<string, string> keyValues = new Dictionary<string, string>();
                    keyValues.Add(nameof(e.ItemName), nameof(e.Item.Name));
                    keyValues.Add(nameof(e.ShortDescription), nameof(e.Item.ShortDescription));
                    keyValues.Add(nameof(e.MrpController), nameof(e.Item.MrpController));
                    m.DicLinkField = keyValues;
                }).ShowInList(width: 150);
                View.Property(p => p.ItemName).ShowInList(width: 150).Readonly();
                View.Property(p => p.ShortDescription).ShowInList(width: 150).Readonly();
                View.Property(p => p.MrpController).ShowInList().Readonly();

                View.Property(p => p.Process).UsePagingLookUpEditor((m, e) =>
                {
                    Dictionary<string, string> keyValues = new Dictionary<string, string>();
                    keyValues.Add(nameof(e.ProcessCode), nameof(e.Process.Code));
                    m.DicLinkField = keyValues;
                }).ShowInList(width: 150);
                View.Property(p => p.ProcessCode).Readonly().ShowInList(width: 150);
                View.Property(p => p.Resource).UsePagingLookUpEditor((m, e) =>
                {
                    Dictionary<string, string> keyValues = new Dictionary<string, string>();
                    keyValues.Add(nameof(e.ResourceName), nameof(e.Resource.Name));
                    m.DicLinkField = keyValues;
                }).ShowInList(width: 150);
                View.Property(p => p.ResourceName).ShowInList(width: 150).Readonly();
                View.Property(p => p.Capacity).ShowInList().UseSpinEditor(p => { p.MinValue = 0; }).HasLabel("标准产能(H)");
                View.Property(p => p.ShiftCapacity).ShowInList().UseSpinEditor(p => { p.MinValue = 0; });
            }
        }

        /// <summary>
        /// 查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.ItemCode).Show().Readonly(false);
            View.Property(p => p.ShortDescription).Show().Readonly(false);
            View.Property(p => p.Process).Show();
            View.Property(p => p.Resource).Show();
            View.Property(p => p.MrpController).Show().Readonly(false);
        }

        /// <summary>
        /// 配置导入视图
        /// </summary>
        protected override void ConfigImportView()
        {
            View.Property(p => p.ItemCode).Show();
            View.Property(p => p.ProcessCode).Show();
            View.Property(p => p.ResourceCode).Show();
            View.Property(p => p.Capacity).Show();
            View.Property(p => p.ShiftCapacity).Show();
        }
    }
}

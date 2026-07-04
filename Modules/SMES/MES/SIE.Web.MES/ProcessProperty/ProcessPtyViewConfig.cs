using DevExpress.CodeParser;
using DevExpress.XtraRichEdit.Layout;
using SIE.Equipments.Configs;
using SIE.Items.KzItemCategorys;
using SIE.MES.Fixture;
using SIE.MES.ProcessProperty;
using SIE.MetaModel.View;
using SIE.Web.Common;
using SIE.Web.MES.Fixture.Commands;
using SIE.Web.MES.LineAndon.Commands;
using SIE.Web.MES.ProcessProperty.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.ProcessProperty
{
    public class ProcessPtyViewConfig : WebViewConfig<ProcessPty>
    {
        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseCommands(WebCommandNames.Add, WebCommandNames.Edit, WebCommandNames.Delete, typeof(ProcessPtySaveCommands).FullName);
            View.UseCommands(typeof(ProcessPtyImportCommand).FullName, typeof(ProcessPtyDLTemplateCommand).FullName, WebCommandNames.ExportXls, WebCommandNames.ExportXlsAll);
            using (View.OrderProperties())
            {
                View.Property(p => p.Process).UsePagingLookUpEditor((m, e) =>
                {
                    Dictionary<string, string> keyValues = new Dictionary<string, string>();
                    keyValues.Add(nameof(e.ProcessCode), nameof(e.Process.Code));
                    m.DicLinkField = keyValues;
                }).ShowInList(width: 200);
                View.Property(p => p.ProcessCode).ShowInList(width: 150);
                View.Property(p => p.KzCategoryId).ShowInList(width: 150).UsePagingLookUpEditor((m, e) =>
                {
                    Dictionary<string, string> keyValues = new Dictionary<string, string>();
                    keyValues.Add(nameof(e.KzCategoryName), nameof(e.KzCategory.Name));
                    m.DicLinkField = keyValues;
                }).HasLabel("工艺属性分类编码");
                View.Property(p => p.KzCategoryName).ShowInList(width: 150).Readonly();
                View.Property(p => p.Scheduling).ShowInList(width: 150);
                View.Property(p => p.DispatchWork).ShowInList(width: 150);
                View.Property(p => p.IsPrepare).Show();
                View.Property(p => p.IsTransfer).Show();
                View.Property(p => p.IsPbcd).ShowInList(width: 150);
                View.Property(p => p.IsAutoDispatch).ShowInList(width: 150);
                View.Property(p => p.IsReportValid).Show();
                View.Property(p => p.IsUnFirstGenerateTask).Show();
                View.ChildrenProperty(p => p.ProcessPtyDetailList).Show(ChildShowInWhere.All);
            }
        }

        protected override void ConfigImportView()
        {
            View.Property(p => p.ProcessCode).ShowInList(width: 150).HasLabel("标准文本码");
            View.PropertyRef(p => p.KzCategory.Code).ShowInList(width: 150).HasLabel("工艺属性分类编码");
            View.Property(p => p.Scheduling).ShowInList(width: 150);
            View.Property(p => p.DispatchWork).ShowInList(width: 150);
            View.Property(p => p.IsPrepare).ShowInList(width: 150);
            View.Property(p => p.IsTransfer).ShowInList(width: 150);
            View.Property(p => p.IsReportValid).Show();
            View.Property(p => p.IsUnFirstGenerateTask).Show();
        }
    }
}

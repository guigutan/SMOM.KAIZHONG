using SIE.CSM.Suppliers;
using SIE.Domain;
using SIE.MetaModel.View;
using SIE.Kit.MES.SingleLabels;
using SIE.Web.Kit.Mes.SingleLabels.Commands;

namespace SIE.Web.Kit.Mes
{
    /// <summary>
    /// 单体条码视图配置类
    public class SingleLabelViewConfig : WebViewConfig<SingleLabel>
    {
        /// <summary>
        /// 通用视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.FormEdit();
            View.UseDefaultCommands();
        }

        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.AddBehavior("SIE.Web.Kit.Mes.SingleLabels.SingleLabelBehavior");
            ////View.ReplaceCommands(WebCommandNames.Add, typeof(AddSingleLabelCommand).FullName);
            View.ReplaceCommands(WebCommandNames.Copy, "SIE.Web.Kit.Mes.SingleLabels.Commands.CopySingleLabelCommand");
            View.UseCommands(typeof(SingleLabelImportCommand).FullName);
            View.Property(p => p.Sn).ShowInList(150);
            View.Property(p => p.BatchCode);
            View.Property(p => p.PrintDate).ShowInList(150);
            View.Property(p => p.SourceId);
            View.Property(p => p.SourceNo);
            View.Property(p => p.Employee);
            View.Property(p => p.SourceType);
            View.Property(p => p.Item);
            View.Property(p => p.ItemName);
            View.Property(p => p.LabelState);
            View.Property(p => p.Supplier).HasLabel("供应商编码");
            View.Property(p => p.SupplierName).HasLabel("供应商名称");
            View.Property(p => p.CreateByName);
            View.Property(p => p.CreateDate).ShowInList(150);
            View.Property(p => p.UpdateByName);
            View.Property(p => p.UpdateDate).ShowInList(150);
        }

        /// <summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.ClearCommands();
            View.UseCommands(typeof(SaveAndAddSingleLabelCommand).FullName);
            ////View.UseCommands(WebCommandNames.FormSave);
            View.Property(p => p.Sn).Readonly(p => p.PersistenceStatus != PersistenceStatus.New)
                .UseListSetting(e => { e.HelpInfo = "新增状态可编辑"; });
            View.Property(p => p.BatchCode);
            View.Property(p => p.PrintDate);
            View.Property(p => p.SourceId);
            View.Property(p => p.SourceNo);
            View.Property(p => p.Employee);
            View.Property(p => p.SourceType);
            View.Property(p => p.Item);
            View.Property(p => p.LabelState).Readonly(p => p.PersistenceStatus != PersistenceStatus.New)
                .UseListSetting(e => { e.HelpInfo = "新增状态可编辑"; });
            View.Property(p => p.Supplier).UsePagingLookUpEditor(p =>
            {
                p.DisplayField = Supplier.NameProperty.Name;
                p.SearchFieldList = new System.Collections.Generic.List<string>()
                {
                    Supplier.CodeProperty.Name,
                    Supplier.NameProperty.Name,
                    Supplier.ShortNameProperty.Name
                };
            }).ShowInDetail();
        }

        /// <summary>
        /// 配置查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.Sn);
                View.Property(p => p.SourceType);
                View.Property(p => p.LabelState);
            }
        }

        /// <summary>
        /// 配置下拉视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.Sn);
                View.Property(p => p.BatchCode);
                View.Property(p => p.SourceId);
            }
        }
    }
}

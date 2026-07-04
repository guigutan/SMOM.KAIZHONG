using SIE.MES.ItemEquipAccount;
using SIE.MES.LineAndon;
using SIE.MetaModel.View;
using SIE.Web.Common.Commands;
using SIE.Web.MES.ItemEquipAccount.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.ItemEquipAccount
{
    /// <summary>
    /// 模具与产品的关系视图配置
    /// </summary>
    public class EquipAccountItemViewConfig : WebViewConfig<EquipAccountItem>
    {
        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseCommands(WebCommandNames.Add, typeof(EquipAccountItemEditCommand).FullName, WebCommandNames.Delete, typeof(EquipAccountItemSaveCommand).FullName);
            //View.ReplaceCommands(EnableCommand.CommandName, typeof(EquipAccountItemEnableCommand).FullName);
            //View.ReplaceCommands(DisableCommand.CommandName, typeof(EquipAccountItemDisableCommand).FullName);
            View.UseCommands(typeof(EquipAccountItemImportCommand).FullName, typeof(EquipAccountItemDLTemplateCommand).FullName, typeof(EquipAccountItemImportEquipCommand).FullName, typeof(EquipAccountItemImportItemCommand).FullName, typeof(EquipAccountItemImportProcessCommand).FullName, typeof(EquipAccountItemImportUniqueCodeCommand).FullName, WebCommandNames.ExportXls, WebCommandNames.ExportXlsAll);
            View.UseCommands(typeof(EquipAccountItemLabelPrintCommand).FullName);
            using (View.OrderProperties())
            {
                View.Property(p => p.EquipAccount).UseDataSource((o, e, r) =>
                {
                    return RT.Service.Resolve<EquipAccountItemController>().GetEquipAccounts(e, r);
                }).UsePagingLookUpEditor((m, e) =>
                {
                    Dictionary<string, string> keyValues = new Dictionary<string, string>();
                    keyValues.Add(nameof(e.EquipAccountName), nameof(e.EquipAccount.Name));
                    m.DicLinkField = keyValues;
                }).ShowInList(width: 200);
                View.Property(p => p.EquipAccountName).Readonly().ShowInList(width: 300);
                View.Property(p => p.Drawn).Readonly().ShowInList(width: 300);
                View.Property(p => p.Item).UsePagingLookUpEditor((m, e) =>
                {
                    Dictionary<string, string> keyValues = new Dictionary<string, string>();
                    keyValues.Add(nameof(e.ItemName), nameof(e.Item.Name));
                    m.DicLinkField = keyValues;
                }).ShowInList(width: 200);
                View.Property(p => p.ItemName).Readonly().ShowInList(width: 300);
                View.Property(p => p.OldItem).Readonly().ShowInList(width: 300);
                View.Property(p => p.ItemMtart).Readonly().ShowInList(width: 300);
                View.Property(p => p.Process).UsePagingLookUpEditor((m, e) =>
                {
                    Dictionary<string, string> keyValues = new Dictionary<string, string>();
                    keyValues.Add(nameof(e.ProcessCode), nameof(e.Process.Code));
                    m.DicLinkField = keyValues;
                }).ShowInList(width: 200);
                View.Property(p => p.ProcessCode).Readonly().ShowInList(width: 150);
                View.Property(p => p.UniqueCode).ShowInList(width: 150).HasLabel("模具组");
            }
        }

        /// <summary>
        /// 配置导入视图
        /// </summary>
        protected override void ConfigImportView()
        {
            View.PropertyRef(p => p.EquipAccountCode).HasLabel("模具编码");
            View.PropertyRef(p => p.Item.Code).HasLabel("产品编码");
            View.PropertyRef(p => p.Process.Code).HasLabel("标准文本码");
            View.Property(p => p.UniqueCode).HasLabel("模具组");
        }
    }
}

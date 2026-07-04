using SIE.MES.ItemLine;
using SIE.MES.ItemProcess;
using SIE.MetaModel.View;
using SIE.Web.Common.Commands;
using SIE.Web.MES.ItemLine.Commands;
using SIE.Web.MES.ItemProcess.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.ItemProcess
{
    /// <summary>
    /// 物料与工序的关系视图配置
    /// </summary>
    public class ProcessItemViewConfig : WebViewConfig<ProcessItem>
    {
        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseCommands(WebCommandNames.Add, typeof(ProcessItemEditCommand).FullName, typeof(ProcessItemDeleteCommand).FullName, typeof(ProcessItemSaveCommand).FullName);
            View.ReplaceCommands(EnableCommand.CommandName, typeof(ProcessItemEnableCommand).FullName);
            View.ReplaceCommands(DisableCommand.CommandName, typeof(ProcessItemDisableCommand).FullName);
            View.UseCommands(typeof(ProcessItemImportCommand).FullName, typeof(ProcessItemDLTemplateCommand).FullName);
            using (View.OrderProperties())
            {
                View.Property(p => p.Item).UsePagingLookUpEditor((m, e) =>
                {
                    Dictionary<string, string> keyValues = new Dictionary<string, string>();
                    keyValues.Add(nameof(e.ItemName), nameof(e.Item.Name));
                    m.DicLinkField = keyValues;
                }).ShowInList(width: 200);
                View.Property(p => p.ItemName).Readonly().ShowInList(width: 300);
                View.Property(p => p.Process).ShowInList(width: 150);
                View.Property(p => p.State).Readonly().ShowInList(width: 150);
            }
        }

        /// <summary>
        /// 配置导入视图
        /// </summary>
        protected override void ConfigImportView()
        {
            View.PropertyRef(p => p.Item.Code).HasLabel("物料编码");            
            View.PropertyRef(p => p.Process.Code).HasLabel("工序编码");
        }
    }
}

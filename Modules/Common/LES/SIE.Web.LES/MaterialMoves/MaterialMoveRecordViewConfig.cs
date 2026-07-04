using SIE.LES.MaterialMoves;
using SIE.MetaModel.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.LES.MaterialMoves
{
    /// <summary>
    /// 挪料记录视图配置
    /// </summary>
    public class MaterialMoveRecordViewConfig : WebViewConfig<MaterialMoveRecord>
    {
        /// <summary>
        /// 列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.ClearCommands();
            View.UseCommands(WebCommandNames.ExportXls, WebCommandNames.ExportXlsAll);
            View.DisableEditing();
            using (View.OrderProperties())
            {
                View.Property(p => p.MoveType).HasLabel("挪料类型".L10N() + "*").ShowInList();
                View.Property(p => p.SourceWo).ShowInList();
                View.Property(p => p.TargetWo).ShowInList();
                View.Property(p => p.Item).HasLabel("物料编码".L10N()).ShowInList();
                View.Property(p => p.ItemName).HasLabel("物料名称".L10N() + "*").ShowInList();
                View.Property(p => p.ItemExtPropName).ShowInList();
                View.Property(p => p.UnitName).HasLabel("单位".L10N() + "*").ShowInList();
                View.Property(p => p.Reason).HasLabel("挪料原因".L10N() + "*").ShowInList();
                View.Property(p => p.MoveQty).HasLabel("挪料数量".L10N() + "*").ShowInList();
                View.Property(p => p.Warehouse).HasLabel("挪料仓库".L10N() + "*").ShowInList();
                View.Property(p => p.MoveSourceType).ShowInList();
            }
        }
    }
}

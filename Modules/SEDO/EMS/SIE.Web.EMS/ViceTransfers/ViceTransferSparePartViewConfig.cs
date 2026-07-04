using SIE.EMS.SpareParts;
using SIE.EMS.ViceTransfers;
using SIE.MetaModel.View;
using System.Collections.Generic;

namespace SIE.Web.EMS.ViceTransfers
{
    /// <summary>
    /// 备件需求清单视图
    /// </summary>
    public class ViceTransferSparePartViewConfig : WebViewConfig<ViceTransferSparePart>
    {


        /// <summary>
        /// 编辑视图
        /// </summary>
        public const string EditView = "EditView";

        /// <summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(EditView);
            if (ViewGroup == EditView)
            {
                DetailListView();
            }
        }

        /// <summary>
        /// 明细编写页面
        /// </summary>
        protected void DetailListView()
        {
            View.UseCommands("SIE.Web.EMS.ViceTransfers.Commands.AddSparePartDemandCommand", WebCommandNames.Delete);
            View.AddBehavior("SIE.Web.EMS.ViceTransfers.Scripts.ViceTransferSparePartBehavior");
            using (View.OrderProperties())
            {
                View.Property(p => p.LineNo).ShowInList(60).Readonly();
                View.Property(p => p.SparePartId).UsePagingLookUpEditor((m, e) =>
                {
                    Dictionary<string, string> keyValues = new Dictionary<string, string>();
                    keyValues.Add(nameof(e.SparePartCode), nameof(e.SparePart.SparePartCode));
                    keyValues.Add(nameof(e.SparePartName), nameof(e.SparePart.SparePartName));
                    keyValues.Add(nameof(e.SparePartType), nameof(e.SparePart.SpartType));
                    keyValues.Add(nameof(e.ControlMethod), nameof(e.SparePart.ControlMethod));
                    keyValues.Add(nameof(e.UnitName), nameof(e.SparePart.UnitName));
                    m.DicLinkField = keyValues;
                }).UseDataSource((source, pagingInfo, keyword) =>
                {
                    return RT.Service.Resolve<SparePartController>().GetSpareParts(pagingInfo, keyword);
                }).ShowInList();
                View.Property(p => p.SparePartName).ShowInList(80).Readonly();
                View.Property(p => p.SparePartType).ShowInList(80).Readonly();
                View.Property(p => p.ControlMethod).ShowInList(80).Readonly();

                View.Property(p => p.Qty).UseSpinEditor(m => { m.MinValue = 1; m.AllowDecimals = false; }).DefaultValue(1).ShowInList(80);
                View.Property(p => p.QualityStatus).ShowInList(80);
                View.Property(p => p.WhInventory).ShowInList(80).Readonly();
                View.Property(p => p.UnitName).ShowInList(80).Readonly();
                View.Property(p => p.UpdateBy).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);

                View.Property(p => p.CreateBy).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
            }
        }

        /// <summary>
        /// 配置列表
        /// </summary>
        protected override void ConfigListView()
        {
            View.DisableEditing();
            View.Property(p => p.LineNo).ShowInList(80);
            View.Property(p => p.SparePartId).ShowInList(120);
            View.Property(p => p.SparePartName).ShowInList(120);
            View.Property(p => p.SparePartType).ShowInList(120);
            View.Property(p => p.ControlMethod).ShowInList(120);

            View.Property(p => p.Qty).ShowInList(100);
            View.Property(p => p.TransferQty).ShowInList(100);
            View.Property(p => p.QualityStatus).ShowInList(120);
            View.Property(p => p.UnitName).ShowInList(60);
        }
    }
}

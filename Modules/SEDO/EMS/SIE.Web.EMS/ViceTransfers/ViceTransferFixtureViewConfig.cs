using SIE.EMS.ViceTransfers;
using SIE.Fixtures;
using SIE.Fixtures.Models;
using SIE.MetaModel.View;
using System.Collections.Generic;

namespace SIE.Web.EMS.ViceTransfers
{
    /// <summary>
    /// 工治具需求清单
    /// </summary>
    public class ViceTransferFixtureViewConfig : WebViewConfig<ViceTransferFixture>
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
        /// 明细编辑视图
        /// </summary>
        protected void DetailListView()
        {
            View.UseCommands("SIE.Web.EMS.ViceTransfers.Commands.AddFixtureDemandCommand",WebCommandNames.Delete);
            View.AddBehavior("SIE.Web.EMS.ViceTransfers.Scripts.FixtureDemandListBehavior");
            using (View.OrderProperties())
            {
                View.Property(p => p.LineNo).ShowInList().Readonly();
                View.Property(p => p.FixtureEncodeId).UsePagingLookUpEditor((m, e) =>
                {
                    Dictionary<string, string> keyValues = new Dictionary<string, string>();
                    keyValues.Add(nameof(e.ModelCode), nameof(e.FixtureEncode.ModelCode));
                    keyValues.Add(nameof(e.ModelName), nameof(e.FixtureEncode.ModelName));
                    keyValues.Add(nameof(e.FixtureTypeCode), nameof(e.FixtureEncode.FixtureType));
                    keyValues.Add(nameof(e.ManageMode), nameof(e.FixtureEncode.ManageMode));
                    keyValues.Add(nameof(e.UintName), nameof(e.FixtureEncode.UnitName));
                    m.DicLinkField = keyValues;
                }).
                    UseDataSource((e, c, r) =>
                    {
                        return RT.Service.Resolve<CoreFixtureController>().GetFixtureEncodes(r, c);
                    }).ShowInList();
                View.Property(p => p.ModelCode).ShowInList(120).Readonly();
                View.Property(p => p.FixtureTypeCode).ShowInList(120).Readonly();
                View.Property(p => p.ManageMode).ShowInList(120).Readonly();
                View.Property(p => p.Qty).UseSpinEditor(m=> { m.MinValue = 1;m.AllowDecimals = false; }).DefaultValue(1).ShowInList();
                View.Property(p => p.UintName).ShowInList().Readonly();
                View.Property(p => p.FixtureQualityState).ShowInList();
                View.Property(p => p.InStorageQty).ShowInList().Readonly();
                View.Property(p => p.OnlineQty).ShowInList().Readonly();
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
            View.Property(p => p.LineNo).ShowInList();
            View.Property(p => p.FixtureEncodeId).ShowInList();
            View.Property(p => p.ModelCode).ShowInList();
            View.Property(p => p.FixtureTypeCode).ShowInList();
            View.Property(p => p.ManageMode).ShowInList();
            View.Property(p => p.Qty).ShowInList();
            View.Property(p => p.UintName).ShowInList();
            View.Property(p => p.TransferQty).ShowInList();
            View.Property(p => p.FixtureQualityState).ShowInList();
        }
    }
}

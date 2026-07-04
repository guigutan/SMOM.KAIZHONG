using NPOI.SS.Formula.Functions;
using SIE.MES.WorkOrders;
using SIE.MetaModel.View;
using SIE.Web.Items._Extentions_;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.MES.WorkOrders
{
    public class WorkOrderOutputProductViewConfig : WebViewConfig<WorkOrderOutputProduct>
    {/// <summary>
     /// 联副产品列表编辑视图
     /// </summary>
        public const string EditView = "EditView";

        /// <summary>
        /// 查看联副产品列表视图
        /// </summary>
        public const string ReadonlyView = "ReadonlyView";
        protected override void ConfigView()
        {

            View.AssignAuthorize(typeof(WorkOrder));
            View.DeclareExtendViewGroup(new string[] { EditView, ReadonlyView });
            switch (ViewGroup)
            {
                case EditView:
                    EditConfigView();
                    break;
                case ReadonlyView:
                    ConfigReadonlyView();
                    break;
                default:
                    break;
            }
            
        }
        private void ConfigReadonlyView()
        {
            using (View.OrderProperties())
            {
                View.DisableEditing();
                View.Property(p => p.RowNumber).Show(ShowInWhere.All).Readonly(p => p.PersistenceStatus != Domain.PersistenceStatus.New);
                View.Property(p => p.Item).ShowInList(width: 160).HasLabel("物料编码").UsePagingLookUpEditor((m, e) =>
                {
                    Dictionary<string, string> dic = new Dictionary<string, string>();
                    dic.Add(nameof(e.ItemName), nameof(e.Item.Name));
                    dic.Add(nameof(e.Unit), nameof(e.Item.UnitName));
                    dic.Add(nameof(e.EnableExtPro), nameof(e.Item.EnableExtendProperty));
                    m.DicLinkField = dic;
                }).UseListSetting();
                View.Property(p => p.ItemName).ShowInList(width: 120).Readonly();
                View.Property(p => p.ItemExtPropName).UseItemExtPropRecordsFieldEditor(p =>
                {
                    p.IsAllRequired = true;
                    p.SourceEntityType = "WorkOrderOutputList";
                    p.ItemIdField = "ItemId";
                    p.DbField = "ItemExtProp";
                }).Show(ShowInWhere.All).Readonly(p => !p.EnableExtPro);
                View.Property(p => p.OutputListType).Show(ShowInWhere.All);
                View.Property(p => p.Qty).UseItemUnitEditor().Show(ShowInWhere.All);
                View.Property(p => p.SubmitQty).UseItemUnitEditor().Show(ShowInWhere.All);
                View.Property(p => p.Unit).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.CreateBy).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateBy).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            }

        }

        /// <summary>
        /// 
        /// </summary>
        protected override void ConfigSelectionView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.RowNumber).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.Item).Show(ShowInWhere.All);
                View.Property(p => p.ItemName).Readonly().Readonly();
                View.Property(p => p.ItemExtPropName).Readonly().Show(ShowInWhere.All);
                View.Property(p => p.OutputListType).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.Qty).UseItemUnitEditor().Show(ShowInWhere.All).Readonly();
                View.Property(p => p.Unit).Show(ShowInWhere.All).Readonly();
            }
        }

        /// <summary>
        /// 列表数据
        /// </summary>
        private  void EditConfigView()
        {
            View.UseCommands(WebCommandNames.Add, WebCommandNames.Edit, WebCommandNames.Delete);
            using (View.OrderProperties())
            {
                View.Property(p => p.RowNumber).Show(ShowInWhere.All).Readonly(p => p.PersistenceStatus != Domain.PersistenceStatus.New);
                View.Property(p => p.OutputListType).Show(ShowInWhere.All);
                View.Property(p => p.Item).ShowInList(width: 160).HasLabel("物料编码").UsePagingLookUpEditor((m, e) =>
                {
                    Dictionary<string, string> dic = new Dictionary<string, string>();
                    dic.Add(nameof(e.Unit), nameof(e.Item.UnitName));
                    dic.Add(nameof(e.ItemName), nameof(e.Item.Name));
                    dic.Add(nameof(e.EnableExtPro), nameof(e.Item.EnableExtendProperty));
                    m.DicLinkField = dic;
                }).UseListSetting().Cascade(p => p.ItemExtPropName, null).Cascade(p => p.ItemExtProp, null);
                View.Property(p => p.ItemName).ShowInList(width: 120).Readonly();
                View.Property(p => p.ItemExtPropName).UseItemExtPropRecordsFieldEditor(p =>
                {
                    p.IsAllRequired = true;
                    p.SourceEntityType = "WorkOrderOutputList";
                    p.ItemIdField = "ItemId";
                    p.DbField = "ItemExtProp";
                }).Show(ShowInWhere.All).Readonly(p => !p.EnableExtPro);
                View.Property(p => p.Qty).UseItemUnitEditor().Show(ShowInWhere.All);
                View.Property(p => p.Unit).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.CreateBy).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateBy).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            }
        }

    }
}

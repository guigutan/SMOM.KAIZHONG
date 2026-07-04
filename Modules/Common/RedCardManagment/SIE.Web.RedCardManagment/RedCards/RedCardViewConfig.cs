using SIE.Domain;
using SIE.MetaModel.View;
using SIE.RedCardManagment.RedCards;
using SIE.Web.RedCardManagment.RedCards.Commands;
using System;
using System.Collections.Generic;

namespace SIE.Web.RedCardManagment.RedCards
{
	/// <summary>
	/// 红牌管理视图配置
	/// </summary>
	internal class RedCardViewConfig : WebViewConfig<RedCard>
    {

        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.FormEdit();
            View.UseClientOrder();
            View.UseCommands(
                "SIE.Web.RedCardManagment.RedCards.Commands.CheckLogCommand",
                "SIE.Web.RedCardManagment.RedCards.Commands.EnableRedCardCommand",
                "SIE.Web.RedCardManagment.RedCards.Commands.DisableRedCardCommand",
                typeof(SyncRedCardReelsCommand).FullName,
                WebCommandNames.Save,
                WebCommandNames.ExportXls,
                WebCommandNames.ExportXlsAll
            );
            View.Property(p => p.No).Readonly().ShowInList(200);
            View.Property(p => p.ItemId).HasLabel("物料编码");
            View.Property(p => p.ItemName).Readonly();
            View.Property(p => p.SupplierId).HasLabel("供应商"); 
            View.Property(p => p.SupplierName).Readonly();
            View.Property(p => p.ItemBatch);
            View.Property(p => p.ProductDateStart);
            View.Property(p => p.ProductDateEnd);
            View.Property(p => p.ItemSN);
            View.Property(p => p.Status).DefaultValue((int)RedCardState.Disable).Readonly();
            View.Property(p => p.ApplyBillNo).Readonly();
            View.Property(p => p.AbnormalTaskNo).Readonly();
            View.Property(p => p.ApplicantName).Readonly();
            View.Property(p => p.ApplyTime).HasLabel("执行时间").Readonly().ShowInList(180);
            View.Property(p => p.CreateByName).Readonly();
            View.Property(p => p.CreateDate).Readonly();
            View.AttachChildrenProperty(typeof(ItemSnRetroactive), (w) =>
            {
                var args = w as ChildPagingDataArgs;
                var parent = args.Parent.CastTo<RedCard>();
                return RT.Service.Resolve<RedCardService>().GetItemSnRetroactives(parent.Id,args.PagingInfo);
            }).Show(ChildShowInWhere.All);
            View.AttachChildrenProperty(typeof(BatchRetroactive), (w) =>
            {
                var args = w as ChildPagingDataArgs;
                var parent = args.Parent.CastTo<RedCard>();
                return RT.Service.Resolve<RedCardService>().GetBatchRetroactives(parent.Id, args.PagingInfo);
            }).Show(ChildShowInWhere.All);
        }
    }
}
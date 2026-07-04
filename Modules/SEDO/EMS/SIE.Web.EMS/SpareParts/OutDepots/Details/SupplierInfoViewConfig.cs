using SIE.EMS.SpareParts.OutDepots.Details;
using SIE.EMS.SpareParts.OutDepots.Enums;
using SIE.MetaModel.View;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.EMS.SpareParts.OutDepots.Details
{
    /// <summary>
    /// 供应商信息视图配置
    /// </summary>
    public class SupplierInfoViewConfig : WebViewConfig<SupplierInfo>
    {
        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseCommands(
                  "SIE.Web.EMS.SpareParts.OutDepots.Commands.AddSuppCommand",//添加
                  "SIE.Web.EMS.SpareParts.OutDepots.Commands.EditSuppCommand",//修改
                  "SIE.Web.EMS.SpareParts.OutDepots.Commands.DelSuppCommand"//删除
                  );
            View.Property(p => p.Supplier).UsePagingLookUpEditor((m, e) =>
              {
                  Dictionary<string, string> keyValues = new Dictionary<string, string>();
                  keyValues.Add(nameof(e.SupplierName), nameof(e.Supplier.Name));
                  keyValues.Add(nameof(e.Contact), nameof(e.Supplier.Contacts));
                  keyValues.Add(nameof(e.ContactPhone), nameof(e.Supplier.ContactNumber));
                  keyValues.Add(nameof(e.ContactAddress), nameof(e.Supplier.ContactAddress));
                  m.DicLinkField = keyValues;
              })
                .Readonly(p => p.OutState == OutDepotState.Ed);
            View.Property(p => p.SupplierName).Readonly();
            View.Property(p => p.Contact).Readonly(p => p.OutState == OutDepotState.Ed);
            View.Property(p => p.ContactPhone).Readonly(p => p.OutState == OutDepotState.Ed);
            View.Property(p => p.ContactAddress).Readonly(p => p.OutState == OutDepotState.Ed);
            View.Property(p => p.DeliveryWay).Readonly(p => p.OutState == OutDepotState.Ed);
            View.Property(p => p.DepotRetDate).UseDateEditor(p => p.Format = "Y/m/d").Readonly(p => p.OutState == OutDepotState.Ed);
            View.Property(p => p.OutState).Readonly(p => p.OutState == OutDepotState.Ed);
        }
    }
}

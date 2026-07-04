using SIE.Items;
using SIE.MES.Validitys;
using SIE.MetaModel.View;
using SIE.Web.MES.Validitys.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.Validitys
{
    /// <summary>
    /// 有效期标准维护视图配置
    /// </summary>
    public class ValidityStandardViewConfig : WebViewConfig<ValidityStandard>
    {
        /// <summary>
        /// 列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.ClearCommands();
            View.UseCommands(typeof(ValidityAddCommand).FullName, WebCommandNames.Copy, WebCommandNames.Edit, WebCommandNames.Delete, typeof(ValiditySaveCommand).FullName,
                 typeof(ValidityStandardImportCommand).FullName
                );
            using (View.OrderProperties())
            {
                View.Property(p => p.Item).UseDataSource((s, p, k) =>
                {
                    return RT.Service.Resolve<ItemController>().GetItemDatas(p, k);
                }).UsePagingLookUpEditor((m, e) =>
                {
                    Dictionary<string, string> keyValues = new Dictionary<string, string>();
                    keyValues.Add(nameof(e.ItemName), nameof(e.Item.Name));
                    keyValues.Add(nameof(e.ItemType), nameof(e.Item.Type));
                    keyValues.Add(nameof(e.EnableExtProp), nameof(e.Item.EnableExtendProperty));
                    m.DicLinkField = keyValues;
                }).Cascade(p => p.ItemExtProp, null).Cascade(p => p.ItemExtPropName, null).HasLabel("物料编码").ShowInList(width: 120);
                View.Property(p => p.ItemName).Readonly().ShowInList(width: 120);
                View.Property(p => p.ItemExtPropName).UseItemExtPropRecordsFieldEditor(p =>
                {
                    p.IsAllRequired = true;
                    p.SourceEntityType = "ValidityStandard";
                    p.ItemIdField = "ItemId";
                    p.DbField = "ItemExtProp";
                }).Readonly(p => !p.EnableExtProp).ShowInList(width: 120);
                View.Property(p => p.ItemType).DefaultValue(null).Readonly().ShowInList(width: 120);
                View.Property(p => p.LongLived).HasLabel("可用时长寿命(H)").UseSpinEditor(p => { p.AllowBlank = true; p.MinValue = 0.001; p.DecimalPrecision = 3; }).ShowInList(width: 120);
                View.Property(p => p.Effective).UseDateEditor(p => p.Format = "Y-m-d").ShowInList(width: 120);
                View.Property(p => p.Expiration).UseDateEditor(p => p.AllowBlank = true).ShowInList(width: 120);
            }
        }
    }
}

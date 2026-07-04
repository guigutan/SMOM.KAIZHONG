using SIE.Common.Prints;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Packages.ItemLabels
{
    /// <summary>
    /// 物料标签编号打印
    /// </summary>
    [Serializable]
    [System.ComponentModel.DisplayName("物料标签编号打印")]
    public class ItemLabelPrintable : LabelPrintable<ItemLabel>
    {
        public override IEnumerable<String> GetPropertys(Type type = null)
        {
            var propertys = base.GetPropertys(type).ToList();
            propertys.Add("ItemCode");
            propertys.Add("ItemName");
            //propertys.Add("Lot");
            //propertys.Add("Label");
            //propertys.Add("Qty");
            //propertys.Add("Id");
            //propertys.Add("ItemId");
            propertys.Add("FactoryCode");
            return propertys;
        }
        public override string ConverterData(object data)
        {
            var content = base.ConverterData(data);
            var itemLabel = data as ItemLabel;
            if (itemLabel != null)
            {
                //var catalog = RT.Service.Resolve<CatalogController>().GetCatalog();
                content += itemLabel.Item?.Code + Separator
                    + itemLabel.Item?.Name + Separator
                    //+ itemLabel.Lot + Separator
                    //+ itemLabel.Label + Separator
                    //+ itemLabel.Qty + Separator
                    //+ itemLabel.Id + Separator
                    //+ itemLabel.ItemId + Separator
                    + itemLabel.Factory?.Code + Separator
                    ;
                //content = itemLabel.Label + itemLabel.Lot + itemLabel.Qty;
            }
            return content;
        }
    }
}

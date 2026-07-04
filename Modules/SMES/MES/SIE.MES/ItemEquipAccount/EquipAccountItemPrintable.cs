using SIE.Common.Prints;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.ItemEquipAccount
{
    /// <summary>
    /// 模具条码化打印
    /// </summary>
    [Serializable]
    //[System.ComponentModel.DisplayName("模具条码化打印")]
    [DisplayName("模具条码化打印")]
    public class EquipAccountItemPrintable : LabelPrintable<EquipAccountItem>
    {
        public override IEnumerable<String> GetPropertys(Type type = null)
        {
            var propertys = new List<String>();
            propertys.Add("模具编码");
            propertys.Add("模具图号");
            propertys.Add("旧料号");
            return propertys;
        }
        public override string ConverterData(object data)
        {
            var content = string.Empty;
            var equipAccountItem = data as EquipAccountItem;
            if (equipAccountItem != null)
            {
                //var catalog = RT.Service.Resolve<CatalogController>().GetCatalog();
                content += equipAccountItem.EquipAccountCode + Separator
                    + equipAccountItem.Drawn + Separator
                    + equipAccountItem.OldItem + Separator
                    ;
                //content = itemLabel.Label + itemLabel.Lot + itemLabel.Qty;
            }
            return content;
        }
    }
}

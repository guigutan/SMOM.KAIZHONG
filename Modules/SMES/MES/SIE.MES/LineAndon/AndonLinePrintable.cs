using SIE.Common.Prints;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.LineAndon
{
    /// <summary>
    /// 产线条码化打印
    /// </summary>
    [Serializable]
    //[System.ComponentModel.DisplayName("产线条码化打印")]
    [DisplayName("产线条码化打印")]
    public class AndonLinePrintable : LabelPrintable<AndonLine>
    {
        public override IEnumerable<String> GetPropertys(Type type = null)
        {
            var propertys = new List<String>();
            propertys.Add("产线代码");
            propertys.Add("产线描述");
            propertys.Add("设备编码");
            propertys.Add("入厂日期");
            return propertys;
        }
        public override string ConverterData(object data)
        {
            var content = string.Empty;
            var andonLine = data as AndonLine;
            if (andonLine != null)
            {
                //var catalog = RT.Service.Resolve<CatalogController>().GetCatalog();
                content += andonLine.MachineCode + Separator
                    + andonLine.MachineName + Separator
                    + andonLine.EquipmentNo + Separator
                    + andonLine.EquipmentDate + Separator
                    ;
                //content = itemLabel.Label + itemLabel.Lot + itemLabel.Qty;
            }
            return content;
        }
    }
}

using SIE.Common.Prints;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.TaskManagement.FeedingRecords
{
    /// <summary>
    /// 供料区条码化打印
    /// </summary>
    [Serializable]
    [DisplayName("供料区条码化打印")]
    public class FeedingAreaPrintable : LabelPrintable<FeedingArea>
    {
        public override IEnumerable<String> GetPropertys(Type type = null)
        {
            var propertys = new List<String>();
            propertys.Add("供料区编码");
            propertys.Add("供料区名称");
            return propertys;
        }
        public override string ConverterData(object data)
        {
            var content = string.Empty;
            var feedingArea = data as FeedingArea;
            if (feedingArea != null)
            {
                content += feedingArea.Code + Separator
                    + feedingArea.Name + Separator
                    ;
            }
            return content;
        }
    }
}

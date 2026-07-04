using SIE.Common.Prints;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace SIE.Inventory.Piles
{
    /// <summary>
    /// 垛条码
    /// </summary>
    [Serializable]
    [DisplayName("垛条码")]
    public class PilePrintable : LabelPrintable<Pile>
    {
        /// <summary>
        /// 获取属性列表
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns>属性列表</returns>
        public override IEnumerable<string> GetPropertys(Type type = null)
        {
            var propertys = base.GetPropertys(type).ToList();
            propertys.Add("PileStateStr");
            propertys.Add("BusinessTypeStr");
            propertys.Add("ItemStateStr");
            propertys.Add("CreateName");

            return propertys;
        }

        /// <summary>
        /// 转换数据
        /// </summary>
        /// <param name="data">对象</param>
        /// <returns>字符串</returns>
        public override string ConverterData(object data)
        {
            var content = base.ConverterData(data);
            Pile labeldata = data as Pile;

            content += labeldata?.PileState.ToLabel() + Separator +
                       labeldata?.BusinessType?.ToLabel() + Separator +
                       labeldata?.ItemState?.ToLabel() + Separator +
                       labeldata?.CreateByName + Separator;

            return content;
        }
    }
}

using NPOI.SS.Formula.Functions;
using SIE.Common.Prints;
using SIE.Core.Algorithms.KZ;
using SIE.Domain;
using SIE.Inventory.Piles;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace SIE.MES.WIP.Pressure
{
    /// <summary>
    /// 耐压测试SN
    /// </summary>
    [Serializable]
    [DisplayName("耐压测试SN")]
    public class WipPressureSnPrintable : LabelPrintable<WipPressureSn>
    {
        private Dictionary<double, WipPressureSn> dicWipPressureSns = new Dictionary<double, WipPressureSn>();
        private Dictionary<double, ItemCusotmerData> dicItemCusotmerDatas = new Dictionary<double, ItemCusotmerData>();
        /// <summary>
        /// 获取属性列表
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns>属性列表</returns>
        public override IEnumerable<string> GetPropertys(Type type = null)
        {
            var propertys = base.GetPropertys(type).ToList();
            propertys.Add("BatchNo");
            propertys.Add("WorkOrderNo");
            propertys.Add("ResourceCode");
            propertys.Add("ResourceName");
            propertys.Add("ProductCode");
            propertys.Add("ProductName");

            propertys.Add("Customer");
            propertys.Add("CodeAlias");
            propertys.Add("SupplierCode");
            propertys.Add("VersionNo");
            propertys.Add("ProjectName");
            propertys.Add("Drawing");

            propertys.Add("Attribute1");
            propertys.Add("Attribute2");
            propertys.Add("Attribute3");

            propertys.Add("Attribute4");
            propertys.Add("Attribute5");
            propertys.Add("Attribute6");

            propertys.Add("Attribute7");
            propertys.Add("Attribute8");
            propertys.Add("Attribute9");

            propertys.Add("Attribute10");

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
            var sn = data as WipPressureSn;
            if (!dicWipPressureSns.ContainsKey(sn?.Id ?? 0))
            {
                dicWipPressureSns.Add(sn.Id, RF.GetById<WipPressureSn>(sn.Id, new EagerLoadOptions().LoadWithViewProperty()));
                dicItemCusotmerDatas.Add(sn.Id, RT.Service.Resolve<ItemCusotmerDataController>().GetItemCusotmerData(sn.WipPressure?.ProductId ?? 0, batchNo: sn.BatchNo));
            }
            sn = dicWipPressureSns[sn?.Id ?? 0];
            var itemCustomer = dicItemCusotmerDatas[sn?.Id ?? 0];

            content += sn?.BatchNo + Separator +
                sn?.WorkOrderNo + Separator +
                sn?.ResourceCode + Separator +
                sn?.ResourceName + Separator +
                sn?.ProductCode + Separator +
                sn?.ProductName + Separator +

                itemCustomer?.Customer + Separator +
                itemCustomer?.CodeAlias + Separator +
                itemCustomer?.SupplierCode + Separator +
                itemCustomer?.VersionNo + Separator +
                itemCustomer?.ProjectName + Separator +
                itemCustomer?.Drawing + Separator +

                itemCustomer?.Attribute1 + Separator +
                itemCustomer?.Attribute2 + Separator +
                itemCustomer?.Attribute3 + Separator +
                itemCustomer?.Attribute4 + Separator +
                itemCustomer?.Attribute5 + Separator +
                itemCustomer?.Attribute6 + Separator +
                itemCustomer?.Attribute7 + Separator +
                itemCustomer?.Attribute8 + Separator +
                itemCustomer?.Attribute9 + Separator +
                itemCustomer?.Attribute10 + Separator

                ;

            return content;
        }
    }
}

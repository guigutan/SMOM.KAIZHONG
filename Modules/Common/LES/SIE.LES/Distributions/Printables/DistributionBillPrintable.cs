using SIE.Common.Prints;
using SIE.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace SIE.LES.Distributions.Printables
{
    /// <summary>
    /// 配送单管理
    /// </summary>
    [Serializable]
    [DisplayName("配送单管理")]
    public class DistributionBillPrintable : BillPrintable<Distribution>
    {
        /// <summary>
        /// 获取属性列表
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns>属性列表</returns>
        public override IEnumerable<string> GetPropertys(Type type = null)
        {
            var propertys = base.GetPropertys(type).ToList();
            if (type == typeof(Distribution))
            {
                propertys.Add("D_TargetLineName");
                propertys.Add("D_ShippingWareHouseCode");
                propertys.Add("D_DeliveryManName");
                propertys.Add("D_ReceiverName");
                propertys.Add("D_TableName");
                propertys.Add("D_CreateBy");
                propertys.Add("D_UpdateBy");
            }
             
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
            Distribution bill = data as Distribution;
            if (bill != null)
            {
                content += bill.ProductLine?.Name + Separator +
                            bill.Warehouse?.Code + Separator +
                            bill.Deliveryman?.Name + Separator +
                            bill.Receiver?.Name + Separator;
                            
                var meta = RF.Find<Distribution>().EntityMeta;
                var tableName = meta.TableMeta.TableName;
                content += tableName + Separator
                    + bill.CreateBy + Separator
                    + bill.UpdateBy + Separator;
            }
         
            return content;
        }
    }
}

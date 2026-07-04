using SIE.Common.Prints;
using SIE.Dock.DockAppoints;
using SIE.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace SIE.Dock.Printables
{
    /// <summary>
    /// 月台预约标签
    /// </summary>
    [Serializable]
    [DisplayName("月台预约标签")]
    public class DockAppointPrintable : BillPrintable<DockAppoint>
    {
        /// <summary>
        /// 获取属性列表
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns>属性列表</returns>
        public override IEnumerable<string> GetPropertys(Type type = null)
        {
            var propertys = base.GetPropertys(type).ToList();
            if (type == typeof(DockAppoint))
            {
                propertys.Add("R_DockCode");
                propertys.Add("R_DockName");
                propertys.Add("R_YardZoneCode");
                propertys.Add("R_YardZoneName");
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
            DockAppoint bill = data as DockAppoint;
            if (bill != null)
            {
                content += bill.DockMaintain?.Code + Separator +
                            bill.DockMaintain?.Name + Separator +
                            bill.YardZone?.Code + Separator +
                            bill.YardZone?.Name + Separator;
            }

            return content;
        }
    }
}
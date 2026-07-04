using SIE.Common.Prints;
using SIE.Dock.DockAppoints;
using SIE.Dock.DockQueues;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace SIE.Dock.Printables
{
    /// <summary>
    /// 月台排队标签
    /// </summary>
    [Serializable]
    [DisplayName("月台排队标签")]
    public class DockQueuePrintable : BillPrintable<DockQueue>
    {
        /// <summary>
        /// 获取属性列表
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns>属性列表</returns>
        public override IEnumerable<string> GetPropertys(Type type = null)
        {
            var propertys = base.GetPropertys(type).ToList();
            if (type == typeof(DockQueue))
            {
                propertys.Add("R_AssignDockCode");
                propertys.Add("R_AssignDockName");
                propertys.Add("R_YardZoneCode");
                propertys.Add("R_YardZoneName");
                propertys.Add("R_AppointDockCode");
                propertys.Add("R_AppointDockName");
                propertys.Add("R_DockAppointNo");
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
            DockQueue bill = data as DockQueue;
            if (bill != null)
            {
                content += bill.AssignDock?.Code + Separator +
                            bill.AssignDock?.Name + Separator +
                            bill.YardZone?.Code + Separator +
                            bill.YardZone?.Name + Separator +
                            bill.DockAppoint?.DockMaintain?.Code + Separator +
                            bill.DockAppoint?.DockMaintain?.Name + Separator +
                            bill.DockAppoint?.No + Separator;
            }

            return content;
        }
    }
}

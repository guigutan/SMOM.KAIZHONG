using SIE.Domain;
using SIE.Resources.CalendarSchemes;
using SIE.Resources.WipResources;
using SIE.Resources.WorkCenters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Resources.LineAndons
{
    public class LineAndonController : DomainController
    {
        /// <summary>
        /// 同步产线与安灯区域到资源
        /// </summary>
        /// <returns></returns>
        public virtual string SyncLineAndon()
        {
            StringBuilder sb = new StringBuilder();
            var lineAndonList = RF.GetAll<AndonLine>(null, new EagerLoadOptions().LoadWithViewProperty());
            var srlist = RT.Service.Resolve<WipResourceController>().GetWipResourceBySrcType(new List<SyncSourceType>() { SyncSourceType.LineAndon }, null, string.Empty);
            foreach (var item in lineAndonList)
            {
                WipResource src = srlist.FirstOrDefault(p => p.Code == item.MachineCode);
                try {
                    if (src == null)
                    {
                        src = new WipResource();
                        src.Code = item.MachineCode;
                        src.Name = item.MachineName;
                        src.SourceId = item.Id;
                        src.Scheme = RT.Service.Resolve<CalendarSchemeController>().GetDefaultCalendar();
                        src.Qty = 1;
                        src.TaktTime = 1;
                        RT.Service.Resolve<WipResourceController>().EnableWipResource(new List<WipResource>() { src });
                        src.ResourceState = ResourceState.Actived;
                        src.PersistenceStatus = PersistenceStatus.Unchanged;
                        src.AndonUpholdId = item.AndonUpholdId;
                        src.AndonCode = item.AndonCode;
                        src.WorkShopId = item.WorkShopId;
                        src.FactoryId = item.FactoryId;
                        src.SourceType = SyncSourceType.LineAndon;

                    }
                    else
                    {
                        src.Code = item.MachineCode;
                        src.Name = item.MachineName;
                        src.SourceId = item.Id;

                        src.AndonUpholdId = item.AndonUpholdId;
                        src.AndonCode = item.AndonCode;
                        src.WorkShopId = item.WorkShopId;
                        src.FactoryId = item.FactoryId;
                        if (src.ResourceState == ResourceState.Diseffect)
                        {
                            src.ResourceState = ResourceState.Actived;
                        }
                    }
                    if (src.PersistenceStatus != PersistenceStatus.Unchanged)
                    {
                        RF.Save(src);
                    }
                }
                catch (Exception ex)
                {
                    sb.AppendLine("同步产线与安灯区域{0}失败：{1}".L10nFormat(item.MachineCode, ex.Message));
                }
            }

            srlist = RT.Service.Resolve<WipResourceController>().GetWipResourceBySrcType(new List<SyncSourceType>() { SyncSourceType.LineAndon }, null, string.Empty);
            if (srlist.Count > 0)
            {
                var codes = srlist.Select(p => p.Code).Distinct().ToList();
                var lineAndonListcodes = lineAndonList.Select(p => p.MachineCode).Distinct().ToList();

                codes = codes.Except(lineAndonListcodes).ToList();
                //把已经不存在的产线变成失效状态
                DB.Update<WipResource>().Set(p => p.ResourceState, ResourceState.Diseffect).Where(p => codes.Contains(p.Code) && p.SourceType == SyncSourceType.LineAndon).Execute();
            }


            return sb.ToString();
        }
    }
}

using Irony;
using MimeKit.Cryptography;
using SIE.Domain;
using SIE.Resources.CalendarSchemes;
using SIE.Resources.Enterprises;
using SIE.Resources.WipResources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Resources.WorkCenters
{
    public class WorkCenterController : DomainController
    {
        
       /// <summary>
       /// 同步工作中心到资源
       /// </summary>
       /// <returns></returns>
        public virtual string SyncWorkCenter()
        {
            StringBuilder sb = new StringBuilder();
            var workCenterList = RF.GetAll<WorkCenter>(null, new EagerLoadOptions().LoadWithViewProperty());
            var srlist = RT.Service.Resolve<WipResourceController>().GetWipResourceBySrcType(new List<SyncSourceType>() { SyncSourceType.WorkCenter }, null, string.Empty);

            foreach (var workCenter in workCenterList)
            {
                WipResource src = srlist.FirstOrDefault(p => p.Code == workCenter.Code);
                try {
                    if (src == null)
                    {
                        src = new WipResource();
                        src.Code = workCenter.Code;
                        src.Name = workCenter.Name;
                        //src.WorkShop = workCenter.WorkShop;
                        //src.FactoryId = workCenter.FactoryId;
                        src.SourceType = SyncSourceType.WorkCenter;
                        src.SourceId = workCenter.Id;
                        src.Scheme = RT.Service.Resolve<CalendarSchemeController>().GetDefaultCalendar();
                        src.Qty = 1;
                        src.TaktTime = 1;
                        RT.Service.Resolve<WipResourceController>().EnableWipResource(new List<WipResource>() { src });
                        src.ResourceState = ResourceState.Actived;
                        src.PersistenceStatus = PersistenceStatus.Unchanged;
                    }
                    else
                    {
                        src.Code = workCenter.Code;
                        src.Name = workCenter.Name;
                        src.SourceId = workCenter.Id;

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
                    sb.AppendLine("同步企业模型{0}失败：{1}".L10nFormat(workCenter.Code, ex.Message));
                }
            }


            srlist = RT.Service.Resolve<WipResourceController>().GetWipResourceBySrcType(new List<SyncSourceType>() { SyncSourceType.WorkCenter }, null, string.Empty);
            if (srlist.Count > 0)
            {
                var codes = srlist.Select(p => p.Code).Distinct().ToList();
                var workCenterListCodes = workCenterList.Select(p => p.Code).Distinct().ToList();

                codes = codes.Except(workCenterListCodes).ToList();
                //把已经不存在的产线变成失效状态
                DB.Update<WipResource>().Set(p => p.ResourceState, ResourceState.Diseffect).Where(p => codes.Contains(p.Code) && p.SourceType == SyncSourceType.WorkCenter).Execute();
            }

            return sb.ToString();
        }

        /// <summary>
        /// 根据编码获取工作中心
        /// </summary>
        /// <param name="codes"></param>
        /// <returns></returns>
        public virtual EntityList<WorkCenter> GetWorkCentersByCode(List<string> codes)
        {
            var list = codes.SplitContains(c =>
            {
                return Query<WorkCenter>().Where(p => c.Contains(p.Code)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
            return list;
        }
    }
}

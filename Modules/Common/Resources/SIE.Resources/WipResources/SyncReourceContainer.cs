using SIE.Domain;
using SIE.EventMessages.Tech.Stations;
using SIE.Resources.Enterprises;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.Resources.WipResources
{
    /// <summary>
    /// 同步资源容器
    /// </summary>
    public class SyncReourceContainer
    {
        /// <summary>
        /// 
        /// </summary>
        protected SyncReourceContainer() { }

        ///// <summary>
        ///// 待同步资源集合
        ///// </summary>
        internal static List<ISyncRsource> SyncRsources { get; private set; }

        /// <summary>
        /// 同步资源
        /// </summary>
        public static string SyncResource()
        {
            var settings = RF.GetAll<SynWipResSetting>();
            if (SyncRsources == null)
                InitSyncRsources(settings);

            StringBuilder msg = new StringBuilder();
            foreach (var resource in SyncRsources)
            {
                if (settings.Any(p => p.Type == resource.GetType().FullName && p.IsSyn))
                    msg.Append(resource.SyncResource());
            }

            try
            {
                //删除用户组中失效的资源
                RT.Service.Resolve<WipResourceController>().DiseffectUpdateUserGroupResource();
            }
            catch (Exception ex)
            {
                msg.Append(ex.GetBaseException().Message);
            }
            try
            {
                //修改生产资源的时候，由生产资源产生的工位也有更新
                RT.Service.Resolve<IStation>().UpdateStationByWipResource();
            }
            catch (Exception ex)
            {
                msg.Append(ex.GetBaseException().Message);
            }

            return msg.ToString();
        }

        /// <summary>
        /// 初始化带同步资源
        /// </summary>
        static void InitSyncRsources(EntityList<SynWipResSetting> settings)
        {
            SyncRsources = new List<ISyncRsource>();
            foreach (var set in settings)
            {
                var type = Type.GetType(set.Type + "," + set.AssenblyName);
                if (type != null)
                {
                    SyncRsources.Add(Activator.CreateInstance(type) as ISyncRsource);
                }
            }
        }
    }

    /// <summary>
    /// 同步资源接口
    /// </summary>
    public interface ISyncRsource
    {
        /// <summary>
        /// 同步资源
        /// </summary>
        string SyncResource();
    }
}
using SIE.EventMessages.Tech.Processs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.EventMessages.Tech.Stations
{
    [Services.Service(FallbackType = typeof(DefaultIStation))]
    public interface IStation
    {
        /// <summary>
        /// 根据生产资源更新工位
        /// </summary>
        /// <param name="wipResourceIds"></param>
        void UpdateStationByWipResource();
    }

    public class DefaultIStation: IStation
    {
        /// <summary>
        /// 根据生产资源更新工位
        /// </summary>
        /// <param name="wipResourceIds"></param>
        public void UpdateStationByWipResource()
        { 
            
        }

    }
}

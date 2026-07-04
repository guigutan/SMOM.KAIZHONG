using SIE.Domain;
using SIE.Domain.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.Tech.Stations
{
    /// <summary>
    /// 工位清单保存前事件
    /// </summary>
    [System.ComponentModel.DisplayName("工位清单保存前事件")]
    [System.ComponentModel.Description("工位清单保存前校验是否存在关联工序")]
    public class StationSubmitting : OnSubmitting<Station>
    {
        /// <summary>
        /// 工位清单保存前事件
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="e"></param>
        protected override void Invoke(Station entity, EntitySubmittingEventArgs e)
        {
            if (e == null || entity == null)
            {
                return;
            }

            if (entity.IsImportData != true) 
            {
                //取工位下的工序列表
                var stationProcesses = RT.Service.Resolve<StationController>().GetStationProcess(entity.Id);

                //删除的
                List<double> deleteIds = new List<double>();
                if (entity.StationProcessList.DeletedList != null &&
                    entity.StationProcessList.DeletedList.Any())
                {
                    deleteIds = entity.StationProcessList.DeletedList
                        .Select(x => (double)x.GetId()).ToList();
                }

                if (!entity.StationProcessList.Any()
                    && !stationProcesses.Any(x => !deleteIds.Contains(x.Id)))
                {
                    throw new ValidationException("工位【{0}】的关联工序不能为空".L10nFormat(entity.Code));
                }
            }
        }
    }
}

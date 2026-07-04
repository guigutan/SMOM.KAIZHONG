using SIE.Common.Import;
using SIE.Domain.Validation;
using SIE.Warehouses.Stations;
using SIE.Web.Common.Import.Commands;
using System;
using System.Collections.Generic;

namespace SIE.Web.Warehouses.Stations.Commands
{
    /// <summary>
    /// 站台导入
    /// </summary>
    public class StationImportCommand : ImportExcelCommand
    {
        /// <summary>
        /// 是否有错误
        /// </summary>
        protected bool IsHasError;

        /// <summary>
        /// 行数据读取, 默认按视图配置把数据读取后，还可以进行自定义处理
        /// </summary>
        /// <param name="data"></param>
        /// <param name="cache"></param>
        /// <exception cref="ValidationException"></exception>
        protected override void OnRowDataRead(RowData data, CacheData cache)
        {
            var station = data.Entity as Station;

            if (!station.Code.StartsWith(station.StationType.ToString()))
            {
                IsHasError = true;
                throw new ValidationException("[{0}]类型为[{1}]，前缀需为[{2}]".L10nFormat(station.Code, station.StationType.ToLabel(), station.StationType.ToString()));
            }

            if (station.Code.IndexOf(":") < 0)
            {
                IsHasError = true;
                throw new ValidationException("[{0}]站台格式不正确：前缀  +  :  + 后续内容".L10nFormat(station.Code));
            }
        }

        /// <summary>
        /// 保存方法
        /// </summary>
        /// <param name="batch">数据</param>
        protected override void OnSave(IList<RowData> batch)
        {
            if (!IsHasError)
            {                             
                importResult.MessageList.AddRange(AppRuntime.Service.Resolve<StationController>().ImportSave(batch));              
            }
        }
    }
}

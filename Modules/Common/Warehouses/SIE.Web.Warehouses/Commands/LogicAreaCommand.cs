using SIE.Common.ImportHelper;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Warehouses;
using SIE.Warehouses.ImportHandles;
using SIE.Web.Command;
using SIE.Web.Common.Import.Commands;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace SIE.Web.Warehouses.Commands
{
    /// <summary>
    /// 导入命令
    /// </summary>
    public class LogicAreaImportCommand : ImportCommandBase
    {
        /// <summary>
        /// 导入命令
        /// </summary>
        /// <returns></returns>
        protected override ImportCompleted GetImportCompleted()
        {
            return (DataRow[] drSuccess, DataRow[] drFailed) =>
            {
            };
        }

        /// <summary>
        /// 获取导入处理类型
        /// </summary>
        /// <returns></returns>
        protected override Type GetImportHandleType()
        {
            return typeof(LogicAreaImportHandle);
        }

    }

    /// <summary>
    /// 添加命令
    /// </summary>
    public class LogicAreaAddCommand : ViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>bool</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var data = args.Data.ToJsonObject<LogicArea>();
            var code = RT.Service.Resolve<WarehouseController>().GetLogicAreaCode();
            data.Code = code;
            return data;
        }
    }

    /// <summary>
    /// 删除命令
    /// </summary>
    public class LogicAreaDeleteCommand : ViewCommand<IList<Entity>>
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>object</returns>
        protected override object Excute(IList<Entity> args, string scope)
        {
            if (args.ToList().OfType<LogicArea>().Count(p => p.PersistenceStatus == PersistenceStatus.New || p.State == State.Disable) != args.ToList().Count)
            {
                return false;
            }
            return true;
        }
    }

    /// <summary>
    /// 选择库位命令
    /// </summary>
    public class LogicAreaSelLocationCommand : ViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>object</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var locationList = args.Data.ToJsonObject<List<LogicAreaLocation>>();
            Check.NotNullOrEmpty(locationList, nameof(locationList));
            if (null == locationList || locationList.Count == 0)
            {
                throw new ArgumentNullException("{0}数据参数不能为空".L10nFormat(nameof(locationList)));
            }
            var locIds = locationList.Select(f => f.StorageLocationId).ToList();
            var locations = RT.Service.Resolve<WarehouseController>().GetStorageLocations(locIds, new EagerLoadOptions().LoadWithViewProperty());
            if (locations.Select(f => f.IsAutomatedStorage).Distinct().Count() > 1)
            {
                throw new ValidationException("选择的库位不能同时包含立库和平库".L10N());
            }
            var isauto = locations.FirstOrDefault().IsAutomatedStorage;
            RT.Service.Resolve<WarehouseController>().SaveLoc(locationList, isauto);

            return isauto;
        }
    }

    /// <summary>
    /// 库位关系删除命令
    /// </summary>
    public class LogicAreaLocDeleteCommand : ViewCommand<double[]>
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>object</returns>
        protected override object Excute(double[] args, string scope)
        {
            List<double> locIdList = args.ToList();
            EntityList<LogicAreaLocation> locations = RT.Service.Resolve<WarehouseController>().GetLogicAreaLocationList(locIdList);
            locations.ForEach(p => p.PersistenceStatus = PersistenceStatus.Deleted);

            RF.Save(locations);
            return true;
        }
    }
}

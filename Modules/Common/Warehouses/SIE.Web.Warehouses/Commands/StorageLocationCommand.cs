using SIE.Common.ImportHelper;
using SIE.Common.Prints;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Warehouses;
using SIE.Warehouses.Events;
using SIE.Warehouses.ImportHandles;
using SIE.Warehouses.Printables;
using SIE.Web.Command;
using SIE.Web.Common.Import.Commands;
using SIE.Web.Common.Prints;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace SIE.Web.Warehouses.Commands
{
    #region StorageLocationAddCommand 库位添加命令

    /// <summary>
    /// 库位添加命令
    /// </summary>
    public class StorageLocationAddCommand : ViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>object</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var storagelocation = args.Data.ToJsonObject<StorageLocation>();
            storagelocation.Code = RT.Service.Resolve<WarehouseController>().GetStorageLocationCode();

            return storagelocation;
        }
    }
    #endregion

    #region StorageLocationCopyCommand 复制新增命令
    /// <summary>
    /// 库位复制新增命令
    /// </summary>
    public class StorageLocationCopyCommand : ViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>object</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            return true;
        }
    }
    #endregion

    #region StorageLocationDeleteCommand 删除命令
    /// <summary>
    /// 库位删除命令
    /// </summary>
    public class StorageLocationDeleteCommand : ViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>object</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            return true;
        }
    }

    #endregion

    #region StorageLocationEnableCommand 启用命令
    /// <summary>
    /// 启用命令
    /// </summary>
    public class StorageLocationEnableCommand : ViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>object</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            List<double> storageLocationIdList = args.SelectedIds.ToList();
            if (storageLocationIdList.Count == 0)
            {
                StorageLocation storageLocation = args.Data.ToJsonObject<StorageLocation>();
                storageLocationIdList.Add(storageLocation.Id);
            }

            string errorMsg = RT.Service.Resolve<WarehouseController>().EnableStorageLocation(storageLocationIdList);
            if (!string.IsNullOrEmpty(errorMsg))
            {
                throw new ValidationException(errorMsg.L10N());
            }
            else
            {
                foreach (var id in storageLocationIdList)
                {
                    var storageLocation = RF.GetById<StorageLocation>(id);
                    storageLocation.State = State.Enable;
                    storageLocation.MarkSaved();
                }
            }
            return true;
        }
    }
    #endregion

    #region StorageLocationDisableCommand 禁用命令
    /// <summary>
    /// 启用命令
    /// </summary>
    public class StorageLocationDisableCommand : ViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>object</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var location = args.Data.ToJsonObject<StorageLocation>();

            RT.EventBus.Publish(new HasLocationStockEvent() { LocId = location.Id });

            List<double> storageLocationIdList = args.SelectedIds.ToList();
            if (storageLocationIdList.Count == 0)
            {
                StorageLocation storageLocation = args.Data.ToJsonObject<StorageLocation>();
                storageLocationIdList.Add(storageLocation.Id);
            }

            string errorMsg = RT.Service.Resolve<WarehouseController>().DisableStorageLocation(storageLocationIdList);
            if (!string.IsNullOrEmpty(errorMsg))
            {
                throw new ValidationException(errorMsg.L10N());
            }
            else
            {
                foreach (var id in storageLocationIdList)
                {
                    var storageLocation = RF.GetById<StorageLocation>(id);
                    storageLocation.State = State.Disable;
                    storageLocation.MarkSaved();
                }
            }
            return true;
        }
    }
    #endregion

    #region StorageLocationImportCommand 库位导入命令
    /// <summary>
    /// 库位导入命令
    /// </summary>
    public class StorageLocationImportCommand : ImportCommandBase
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
            return typeof(StorageLocationImportHandle);
        }

    }
    #endregion

    #region StorageLocationRouteImportCommand 库位巷道导入命令
    /// <summary>
    /// 库位导入命令
    /// </summary>
    public class StorageLocationRouteImportCommand : ImportCommandBase
    {
        /// <summary>
        /// 导入数据
        /// </summary>
        /// <param name="importViewArgs">导入视图参数</param>
        /// <param name="scope">使用范围</param>
        /// <returns>执行结果</returns>
        protected override object Excute(ImportViewArgs importViewArgs, string scope)
        {
            if (importViewArgs.BehaviorName.Equals("Download"))
            {
                return DownloadTemplate();
            }
            else
            {
                return ImportData(importViewArgs);
            }
        }

        /// <summary>
        /// 直接从服务器上下载模板
        /// </summary>
        /// <returns></returns>
        public virtual object DownloadTemplate()
        {
            const string templateFileName = "库位巷道关系导入模板.xlsx";
            const string fileRelativePath = "Templates/" + templateFileName;
            return new
            {
                FilePath = fileRelativePath,
                FileName = templateFileName
            };
        }

        /// <summary>
        /// 导入完成
        /// </summary>
        /// <returns>ImportCompleted</returns>
        protected override ImportCompleted GetImportCompleted()
        {
            return (DataRow[] drSuccess, DataRow[] drFailed) =>
            {
            };
        }

        /// <summary>
        /// 获取导入处理类型
        /// </summary>
        /// <returns>Type</returns>
        protected override Type GetImportHandleType()
        {
            return typeof(StorageLocationRouteImportHandle);
        }

    }
    #endregion

    #region StorageLocationFrozenCommand 冻结解冻命令
    /// <summary>
    /// 冻结解冻命令
    /// </summary>
    public class StorageLocationFrozenCommand : ViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>object</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            StorageLocation storageLocation = args.Data.ToJsonObject<StorageLocation>();

            List<double> storageLocationIdList = args.SelectedIds.ToList();
            if (storageLocationIdList.Count == 0)
            {
                storageLocationIdList.Add(storageLocation.Id);
            }

            string errorMsg = RT.Service.Resolve<WarehouseController>().FrozenOrThawLocation(storageLocationIdList);
            if (!string.IsNullOrEmpty(errorMsg))
            {
                throw new ValidationException(errorMsg.L10N());
            }
            else
            {
                foreach (var id in storageLocationIdList)
                {
                    var sl = RF.GetById<StorageLocation>(id);
                    sl.IsFrozen = !sl.IsFrozen;
                    sl.MarkSaved();
                }
            }
            return true;
        }
    }
    #endregion

    #region StorageLocationItemLookUpCommand 选择物料
    /// <summary>
    /// 选择物料
    /// </summary>
    public class StorageLocationItemLookUpCommand : ViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>object</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var meta = ClientEntities.Find(args.Type);
            var savedData = RF.Find(meta.EntityType).NewList();
            var storageLocationItemList = args.Data.ToJsonObject<List<StorageLocationItemList>>();
            Check.NotNullOrEmpty(storageLocationItemList, nameof(storageLocationItemList));
            if (storageLocationItemList == null || storageLocationItemList.Count == 0)
            {
                throw new ArgumentNullException("{0}数据参数不能为空".L10nFormat(nameof(storageLocationItemList)));
            }
            foreach (var item in storageLocationItemList)
            {
                var storageLocationItem = new StorageLocationItemList();
                storageLocationItem.ItemId = item.ItemId;
                storageLocationItem.StorageLocationId = item.StorageLocationId;
                savedData.Add(storageLocationItem);
            }
            RF.Save(savedData);
            return true;
        }
    }

    #endregion

    #region PrintStorageLocationCommand 打印标签
    /// <summary>
    /// 打印标签
    /// </summary>
    public class PrintStorageLocationCommand : ViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>object</returns>
        protected override object Excute(ViewArgs args, string scope)
        {            
            var ids = args.SelectedIds.ToList();


            var entitys = RT.Service.Resolve<WarehouseController>().GetStorageLocations(ids, null);
            
            if (!entitys.Any())
            {
                throw new ValidationException("请选择需要打印的库位".L10N());
            }
            PrintTemplate template = RT.Service.Resolve<WarehouseController>().GetStorageLocationPrintTemplate();
            if (template == null)
            {
                throw new ValidationException("请配置库位的打印规则".L10N());
            }
            var report = ReportFactory.Current.GetReportByExtension(template.Type);

            var printableType = Type.GetType(template.EntityType);
            if (printableType == null)
                throw new ValidationException("不存在实体类型[{0}]".L10nFormat(template.EntityType));

            var printable = new StorageLocationPrintable();
            var printDataCommom = new PrintDataCommon();
            printDataCommom.Url = report.PrintProcess(printable, template.Id, template.Content, () =>
              {
                  List<StorageLocation> printData = new List<StorageLocation>();
                  printData.AddRange(entitys);
                  return printData;
              });
            printDataCommom.Type = template.Type;
            return printDataCommom;
        }
    }

    #endregion

    #region StorageLocationItemDeleteCommand 删除专储物料清单
    /// <summary>
    /// 删除
    /// </summary>
    public class StorageLocationItemDeleteCommand : ViewCommand<double[]>
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>object</returns>
        protected override object Excute(double[] args, string scope)
        {
            RT.Service.Resolve<WarehouseController>().StorageLocationItemListDeleteData(args.ToList());
            return true;
        }
    }
    #endregion
}

using SIE.Common.ImportHelper;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.LES;
using SIE.LES.Commons;
using SIE.LES.PrepareItems;
using SIE.Warehouses;
using SIE.Web.Command;
using SIE.Web.Common.Import.Commands;
using System;
using System.Data;
using System.Linq;

namespace SIE.Web.LES.PrepareItems.Commands
{
    /// <summary>
    /// 备料模式-拉式保存命令
    /// </summary>
    public class SavePrepareItemPullCommand : SaveCommand
    {
        /// <summary>
        /// 保存前方法
        /// </summary>
        /// <param name="data">数据集合</param>
        protected override void OnSaving(EntityList data)
        {
            var itemPulls = data as EntityList<PrepareItemPull>;

            var warehousesCodes = itemPulls.Select(m => m.WarehouseCode).ToList();
            var wareHouseList = RT.Service.Resolve<WarehouseController>().GetWarehouseList(warehousesCodes);
            if (wareHouseList.Any(m => !m.IsLineWarehouse))
            {
                throw new ValidationException("仓库必须在功能【仓库】中是否线边仓=是，才能保存".L10N());
            }
            //固定量不能大于最高存量
            if (itemPulls.Any(m => m.MaxStock.HasValue && m.FixedQuantity.HasValue && m.FixedQuantity > m.MaxStock))
            {
                throw new ValidationException("固定量不能大于最高存量".L10N());
            }

            var itemEditIds = itemPulls.Where(p => p.PersistenceStatus == PersistenceStatus.Modified && p.PrepareItemType == PrepareItemType.Pull).Select(p => p.Id).ToList();
            EntityList<PrepareItemPull> notDuplicateItemPullList = new EntityList<PrepareItemPull>();
            notDuplicateItemPullList.AddRange(itemPulls);
            var ts = RT.Service.Resolve<PrepareItemController>().GetPrepareItemPulls(itemEditIds);
            notDuplicateItemPullList.AddRange(ts);
            var sameDtlCount = notDuplicateItemPullList.GroupBy(p => new { p.WarehouseId, p.ItemCategoryId, p.ItemId,p.TriggerType,p.ItemExtProp },
                (m, f) => new { Dtl = m, Count = f.Count() });
            if (sameDtlCount.Any(p => p.Count > 1))
            {
                throw new ValidationException("已存在相同的（仓库+物料类型+物料编码+触发方式+物料扩展属性）的记录！".L10N());
            }

            //itemPulls.Where(p => p.PersistenceStatus == PersistenceStatus.New).ForEach(p => { p.TriggerType = TriggerMode.BelowSafe; p.PrepareItemType = PrepareItemType.Pull; });

            base.OnSaving(itemPulls);
        }
    }

    /// <summary>
    /// 备料模式-拉式添加命令
    /// </summary>
    public class AddPrepareItemPullCommand : ViewCommand
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

    /// <summary>
    /// 备料模式-推式添加命令
    /// </summary>
    public class AddPrepareItemPushCommand : ViewCommand
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

    /// <summary>
    /// 备料模式-推式修改命令
    /// </summary>
    public class EditPrepareItemPushCommand : ViewCommand
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

    /// <summary>
    /// 备料模式-推式保存命令
    /// </summary>
    public class SavePrepareItemPushCommand : SaveCommand
    {
        /// <summary>
        /// 保存前方法
        /// </summary>
        /// <param name="data">数据集合</param>
        protected override void OnSaving(EntityList data)
        {
            var itemPushs = data as EntityList<PrepareItemPush>;

            itemPushs.Where(p => p.PersistenceStatus == PersistenceStatus.New).ForEach(p => p.PrepareItemType = PrepareItemType.Push);
            foreach (var item in itemPushs)
            {
                if (!item.WipResourceId.HasValue)
                {
                    throw new ValidationException("生产资源必填".L10N());
                }
                if (!item.TriggerType.HasValue)
                {
                    throw new ValidationException("触发方式必填".L10N());
                }
                if (item.TriggerType!= TriggerMode.ManualModel&&(  !item.AdvanceHours.HasValue|| item.AdvanceHours.Value<0))
                {
                    throw new ValidationException("提前小时必填，且必须大于或等于0".L10N());
                }
                if (!item.DemandType.HasValue)
                {
                    throw new ValidationException("需求计算方式必填".L10N());
                }
            }


            var itemEditIds = itemPushs.Where(p => p.PersistenceStatus == PersistenceStatus.Modified && p.PrepareItemType == PrepareItemType.Push).Select(p => p.Id).ToList();
            EntityList<PrepareItemPush> notDuplicateItemPushList = new EntityList<PrepareItemPush>();
            notDuplicateItemPushList.AddRange(itemPushs);
            var ts = RT.Service.Resolve<PrepareItemController>().GetPrepareItemPushs(itemEditIds);
            notDuplicateItemPushList.AddRange(ts);
            var sameDtlCount = notDuplicateItemPushList.GroupBy(p => new { p.WipResourceId, p.ItemCategoryId, p.ItemId, p.TriggerType, p.PrepareItemType,p.ItemExtProp },
                (m, f) => new { Dtl = m, Count = f.Count() });
            if (sameDtlCount.Any(p => p.Count > 1))
            {
                throw new ValidationException("已存在相同的(生产资源+物料类型+物料编码+触发方式+物料扩展属性)的记录".L10N());
            }
            base.OnSaving(itemPushs);
        }
    }

    /// <summary>
    /// 导入备料模式维护-推式命令
    /// </summary>
    public class ImportPrepareItemPushCommand : ImportCommandBase
    {
        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="importViewArgs"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
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
            const string templateFileName = "备料模式维护推式_导入模板.xlsx";
            return new
            {
                FilePath = "Templates/" + templateFileName,
                FileName = templateFileName
            };
        }

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
            return typeof(PrepareItemPushImportHandle);
        }
    }

    /// <summary>
    /// 导入备料模式维护-拉式命令
    /// </summary>
    public class ImportPrepareItemPullCommand : ImportCommandBase
    {
        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="importViewArgs"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
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
            const string templateFileName = "备料模式维护拉式_导入模板.xlsx";
            return new
            {
                FilePath = "Templates/" + templateFileName,
                FileName = templateFileName
            };
        }

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
            return typeof(PrepareItemPullImportHandle);
        }
    }
}

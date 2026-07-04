using SIE.Common.ImportHelper;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Items;
using SIE.Items.Units;
using SIE.Web.Command;
using SIE.Web.Common.Import.Commands;
using System;
using System.Data;
using System.Linq;

namespace SIE.Web.Items.Items.Commands
{
    /// <summary>
    /// 删除命令
    /// </summary>
    public class DeleteUnitConvertCommand : DeleteCommand
    {
    }

    /// <summary>
    /// 转换单位添加命令
    /// </summary>
    public class AddItemUnitCommand : ViewCommand
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
    /// 保存
    /// </summary>
    public class SaveUnitConvertCommand : SaveCommand
    {
        /// <summary>
        /// 保存前方法
        /// </summary>
        /// <param name="data">数据集合</param>
        protected override void OnSaving(EntityList data)
        {
            var unitConverts = data as EntityList<UnitConvert>;            
            if (!RT.Service.Resolve<UnitsController>().CheckInitUnitList())
            {
                throw new ValidationException("请单位初始化后，再添加！".L10N());
            }
            foreach (var itemUnit in unitConverts)
            {
                if (itemUnit.Item != null && itemUnit.MainUnitId == itemUnit.UnitId)
                {
                    throw new ValidationException("主单位与辅助单位不能相同".L10N());
                }
                var isData = RT.Service.Resolve<ItemUnitController>().GetItemUnits(itemUnit);
                if (isData)
                {
                    throw new ValidationException("存在相同的主单位和辅助单位数据".L10N());
                }              
            }
            foreach(var delUnitConvert in unitConverts.DeletedList)
            {
                var delUnitConvertTem = delUnitConvert as UnitConvert;
                if (delUnitConvertTem.IsDefault)
                {
                    RT.Service.Resolve<ItemUnitController>().UpdateSecondUnitId(delUnitConvertTem.ItemId);
                }
            }
            base.OnSaving(unitConverts);
        }
    }

    /// <summary>
    /// 导入备料模式维护-推式命令
    /// </summary>
    public class ImportUnitConvertCommand : ImportCommandBase
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
            const string templateFileName = "单位转换_导入模板.xlsx";
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
            return typeof(UnitConvertImportHandle);
        }
    }

    #region 设置默认命令
    /// <summary>
    /// 设置默认命令
    /// </summary>
    public class SetDefaultUnitConvertCommand : ViewCommand<double[]>
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>bool</returns>
        protected override object Excute(double[] args, string scope)
        {
            RT.Service.Resolve<ItemUnitController>().UpdateDefaultUnit(args.FirstOrDefault());
            return true;
        }
    }
    #endregion
}

using SIE.Domain;
using SIE.Tech.Processs;
using SIE.Web.Command;
using System;
using System.Collections.Generic;

namespace SIE.Web.Tech.Processs.Commands
{
    /// <summary>
    /// 工序包装单位命令
    /// </summary>
    [JsCommand("SIE.Web.Tech.Processs.Commands.ProcessPackingUnitCommand")]
    class ProcessPackingUnitCommand : ViewCommand
    {
        /// <summary>
        /// 工序包装单位命令执行方法
        /// </summary>
        /// <param name="args">视图参数</param>
        /// <param name="scope">作用域</param>
        /// <returns>执行结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var meta = ClientEntities.Find(args.Type);
            var savedData = RF.Find(meta.EntityType).NewList();
            var defectList = args.Data.ToJsonObject<List<ProcessPackingUnit>>();
            Check.NotNullOrEmpty(defectList, nameof(defectList));
            if (defectList == null || defectList.Count == 0)
            {
                throw new ArgumentNullException("{0}数据参数不能为空".L10nFormat(nameof(defectList)));
            }

            foreach (var item in defectList)
            {
                var processPackingUnit = new ProcessPackingUnit();
                processPackingUnit.PackageUnitId = item.PackageUnitId;
                processPackingUnit.ProcessId = item.ProcessId;
                savedData.Add(processPackingUnit);
            }

            RF.Save(savedData);
            return true;
        }
    }
}
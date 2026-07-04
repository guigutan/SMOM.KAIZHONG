using SIE.Common.ImportHelper;
using SIE.Domain;
using SIE.LES.LesStockCounts;
using SIE.LES.LesStockCounts.ImportHandles;
using SIE.Web.Command;
using SIE.Web.Common.Import.Commands;
using System;
using System.Data;
using System.Linq;

namespace SIE.Web.LES.LesStockCounts.Commands
{
    /// <summary>
    /// 修改
    /// </summary>
    public class EditSCDetailCommand : ViewCommand
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
    /// 删除
    /// </summary>
    public class DeleteSCDetailCommand : DeleteCommand
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
    /// 关闭
    /// </summary>
    public class CloseSCDetailCommand : SaveCommand
    {
        /// <summary>
        /// 关闭
        /// </summary>
        /// <param name="data">data</param>
        protected override void DoSave(EntityList data)
        {
            var dtlList = data as EntityList<LesStockCountDetail>;
            var dtlIdList = dtlList.Select(p => p.Id).ToList();
            var billId = dtlList.First().LesStockCountId;
            RT.Service.Resolve<LesStockCountController>().CloseLesStockCountDtl(billId,dtlIdList);
        }

        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>object</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var list = GetDeserializeData(args, scope);
            OnSaving(list);
            DoSave(list);
            OnSaved(list);
            var dtlList = list as EntityList<LesStockCountDetail>;
            return dtlList[0]?.LesStockCount;
        }
    }

    /// <summary>
    /// 默认实盘
    /// </summary>
    public class DefaultSCDetailCommand : ViewCommand
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
    /// 输入实盘数
    /// </summary>
    public class InputSCDetailQtyCommand : ViewCommand
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
    /// 新增盘盈
    /// </summary>
    public class AddSCDetailCommand : ViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>object</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var scDetail = args.Data.ToJsonObject<LesStockCountDetail>();
            scDetail.CountDimension = scDetail.LesStockCount.LesStockCountRangeList.FirstOrDefault().CountDimension;
            scDetail.State = LesCountState.Audit;
            scDetail.Qty = 0;
            scDetail.DiffCountQty = 0;
            scDetail.IsNewInventory = true;
            return scDetail;
        }
    }

    /// <summary>
    /// 导入命令
    /// </summary>
    public class SCDetailImportCommand : ImportCommandBase
    {
        /// <summary>
        /// 导入命令
        /// </summary>
        /// <returns>DataRow[]</returns>
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
            return typeof(ImportSCDetailHandle);
        }
    }

    /// <summary>
    /// 明细批量分析命令
    /// </summary>
    public class BatchAnalysisSCDetailCommand : ViewCommand
    {
        /// <summary>
        /// 明细批量分析命令
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>true</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            return true;
        }
    }
}

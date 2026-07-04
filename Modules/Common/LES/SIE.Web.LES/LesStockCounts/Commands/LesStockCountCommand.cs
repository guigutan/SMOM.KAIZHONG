using SIE.Common.Prints;
using SIE.Core.Enums;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Inventory.Task;
using SIE.Items;
using SIE.LES.LesStockCounts;
using SIE.Warehouses;
using SIE.Web.Command;
using SIE.Web.Common.Prints;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Web.LES.LesStockCounts.Commands
{
    /// <summary>
    /// 添加
    /// </summary>
    public class AddLesStockCountCommand : ViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>object</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var bill = args.Data.ToJsonObject<LesStockCount>();
            bill.OrderType = OrderType.StandardCount;
            bill.No = RT.Service.Resolve<LesStockCountController>().GetCountNo();
            bill.State = LesCountState.Create;
            bill.SourceType = SourceType.PC;
            bill.TaskLevel = TaskLevel.Middle;
            return bill;
        }
    }

    /// <summary>
    /// 编辑
    /// </summary>
    public class EditLesStockCountCommand : ViewCommand
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
    public class DeleteLesStockCountCommand : DeleteCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>object</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            return base.Excute(args, scope);
        }
    }

    /// <summary>
    /// 关闭命令
    /// </summary>
    public class CloseLesStockCountCommand : ViewCommand<double[]>
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>object</returns>
        protected override object Excute(double[] args, string scope)
        {
            List<double> idlist = args.ToList();
            RT.Service.Resolve<LesStockCountController>().CloseLesStockCount(idlist.FirstOrDefault());
            return true;
        }
    }

    /// <summary>
    /// 关闭
    /// </summary>
    public class CloseLesCountCommand : FormSaveCommand
    {
        /// <summary>
        /// 关闭
        /// </summary>
        /// <param name="entity">entity</param>
        protected override void DoSave(Entity entity)
        {
            var bill = entity as LesStockCount;
            RT.Service.Resolve<LesStockCountController>().CloseLesStockCount(bill.Id);
        }
    }

    #region 打印命令
    /// <summary>
    /// 打印单据
    /// </summary>
    public class PrintLesStockCountCommand : ViewCommand<PrintDatas>
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">打印数据</param>
        /// <param name="scope">scope</param>
        /// <returns>object</returns>
        protected override object Excute(PrintDatas args, string scope)
        {
            var billTemplateId = args.BillTemplateId;
            List<double> stockCountsIdList = args.BillIdList.ToList();
            var stockCounts = RT.Service.Resolve<LesStockCountController>().GetLesStockCounts(stockCountsIdList);
            if (stockCounts.Count <= 0)
            {
                throw new ValidationException("未选择打印数据".L10N());
            }

            // 1.获取打印模板
            PrintTemplate template = RF.GetById<PrintTemplate>(billTemplateId) ?? throw new ValidationException("打印模板为空或已禁用".L10N());

            //2.根据类型获取报表处理对像
            var report = ReportFactory.Current.GetReportByExtension(template.Type);

            //4.创建实体打印对像 如果清楚实体打印对像自己NEW 一个出来也行
            var printable = new LesStockCountPrintable();
            var printData = new PrintDataCommon();
            //5.调用打印处理函数返回打印模板BASE64字符串到前台，用于传输到打印预览页面
            printData.Url = report.PrintProcess(printable, template.Id, template.Content, () =>
            {
                List<LesStockCount> printData = new List<LesStockCount>();
                printData.AddRange(stockCounts);

                return printData;
            });
            printData.Type = template.Type;
            return printData;
        }
    }
    #endregion

    #region 表单保存
    /// <summary>
    /// 保存
    /// </summary>
    public class SaveLesCountFromCommand : FormSaveCommand
    {
        /// <summary>
        /// 保存前
        /// </summary>
        /// <param name="entity"></param>
        protected override void OnSaving(Entity entity)
        {
            var bill = entity as LesStockCount;
            var ctl = RT.Service.Resolve<LesStockCountController>();
            if (bill.PersistenceStatus != PersistenceStatus.New)
            {
                var newState = ctl.GetLesCountState(bill.Id);
                if (newState != bill.State)
                    throw new ValidationException("单据状态已变更为[{0}],无法保存".L10nFormat(newState.ToLabel()));
            }

            if (bill.LesStockCountRangeList.Count == 0)
            {
                var oldStockCountRange = ctl.GetLesStockCountRange(bill.Id);
                if (oldStockCountRange != null)
                {
                    bill.LesStockCountRangeList.Add(oldStockCountRange);
                }
            }
            base.OnSaving(entity);
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="entity">entity</param>
        protected override void DoSave(Entity entity)
        {
            var ctl = RT.Service.Resolve<LesStockCountController>();
            var bill = entity as LesStockCount;
            string lineno = string.Empty;
            if (bill.LesStockCountDetailList.Count > 0)
            {
                var items = RT.Service.Resolve<ItemController>().GetItemList(bill.LesStockCountDetailList.Select(p => p.ItemId).ToList());
                bill.LesStockCountDetailList.Where(f => f.ItemExtProp.IsNullOrEmpty() && !f.LotId.HasValue).ForEach(f =>
                {
                    if (items.FirstOrDefault(p => p.Id == f.ItemId && p.EnableExtendProperty) != null)
                        lineno += f.LineNo + "、";
                });
                if (lineno.IsNotEmpty())
                    throw new ValidationException("[行号{0}]物料扩展属性值需要维护完整".L10nFormat(lineno.Remove(lineno.Length - 1, 1)));
            }
            if (bill.PersistenceStatus != PersistenceStatus.New)
            {
                if (bill.LesStockCountRangeList.Count == 0)
                {
                    var oldStockCountRangeList = ctl.GetLesStockCountRange(bill.Id);
                    bill.LesStockCountRangeList.Add(oldStockCountRangeList);
                }
                var delDtlIdList = bill.LesStockCountDetailList.DeletedList.Select(p => p.GetId()).ToList();
                var commitDtlIdList = bill.LesStockCountDetailList.Select(p => p.Id).ToList();
                var oldDtlList = ctl.GetStockCountDetail(bill.Id).Where(p => !commitDtlIdList.Contains(p.Id) && !delDtlIdList.Contains(p.Id)).ToList();
                bill.LesStockCountDetailList.AddRange(oldDtlList);
                bill.LesStockCountDetailList.Where(p => !commitDtlIdList.Contains(p.Id)).ForEach(p => p.PersistenceStatus = PersistenceStatus.Unchanged);
            }

            var billNew = RF.GetById<LesStockCount>(bill.Id);
            if (billNew != null && billNew.State != bill.State)
                throw new ValidationException("单据状态已经变更,不能进行保存!".L10N());

            var result = ctl.SaveLesStockCount(bill);
            entity.Clone(result, new CloneOptions(CloneActions.ChildrenRecur));
        }
    }
    #endregion

    #region 生成明细
    /// <summary>
    /// 生成明细
    /// </summary>
    public class AduitLesCountFromCommand : FormSaveCommand
    {
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="entity">entity</param>
        protected override void DoSave(Entity entity)
        {
            RT.Service.Resolve<LesStockCountController>().AuditLesStockCount((double)entity.GetId());
        }
    }
    #endregion

    #region 完工
    /// <summary>
    /// 完工（无差异）
    /// </summary>
    public class FinishCountCommand : FormSaveCommand
    {
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="entity">entity</param>
        protected override void DoSave(Entity entity)
        {
            var bill = entity as LesStockCount;
            RT.Service.Resolve<LesStockCountController>().FinishedStockCount(bill.Id);
        }
    }

    #region 完工命令
    /// <summary>
    /// 完工命令
    /// </summary>
    public class FinishStockCountsCommand : ViewCommand<double[]>
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>object</returns>
        protected override object Excute(double[] args, string scope)
        {
            List<double> idlist = args.ToList();
            RT.Service.Resolve<LesStockCountController>().FinishedStockCount(idlist.FirstOrDefault());
            return true;
        }
    }
    #endregion
    #endregion  
}

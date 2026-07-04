using SIE.Domain;
using SIE.Inventory.Onhands;
using SIE.WMS.Statistics.MonthlyBalances;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Inventory.MonthlyBalances
{
    /// <summary>
    /// 月结库存报表控制器
    /// </summary>
    public partial class MonthlyBalanceController : DomainController
    {
        /// <summary>
        /// 获取月结库存记录
        /// </summary>
        /// <param name="queryAction">查询委托</param>
        /// <param name="elo">贪懒加载</param>
        /// <returns>月结库存记录</returns>
        public virtual EntityList<MonthlyBalance> GetMonthlyBalanceList(Action<IEntityQueryer<MonthlyBalance>> queryAction, EagerLoadOptions elo)
        {
            var query = Query<MonthlyBalance>();
            queryAction?.Invoke(query);
            return query.ToList(null, elo);
        }

        /// <summary>
        /// 根据账期获取月结行数
        /// </summary>
        /// <param name="month">账期</param>
        /// <returns>月结数据</returns>
        public virtual int GetInvMonthlyBalanceCount(string month)
        {
            var query = Query<MonthlyBalance>();
            if (!month.IsNullOrEmpty())
            {
                query.Where(p => p.AccountPeriod == month);
            }
            return query.Count();
        }

        /// <summary>
        /// 根据账期获取月结数据
        /// </summary>
        /// <param name="warehouseId"></param>
        /// <param name="itemId"></param>
        /// <param name="accountPeriod">账期</param>
        /// <param name="storerCode"></param>
        /// <returns>月结数据</returns>
        public virtual MonthlyBalance GetInvMonthlyBalance(string accountPeriod, string storerCode, double warehouseId, double itemId)
        {
            var query = Query<MonthlyBalance>();
            query.Where(p => p.WarehouseId == warehouseId && p.ItemId == itemId && p.AccountPeriod == accountPeriod && p.StorerCode == storerCode);
            return query.FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据账期删除月结数据
        /// </summary>
        /// <param name="month"></param>
        public virtual void DeleteMonthlyBalances(string month)
        {
            DB.Delete<MonthlyBalance>().Where(p => p.AccountPeriod == month).Execute();
        }

        /// <summary>
        /// 查询库存月结汇总数据
        /// </summary>
        /// <param name="month">账期</param>
        /// <returns>库存月结汇总数据</returns>
        public virtual EntityList<MonthlySummary> GetMonthlySummaries(string month)
        {
            var query = Query<MonthlySummary>()/*.Where(p => p.ItemId == 5 && p.WarehouseId == 7)*/;
            if (!month.IsNullOrEmpty())
            {
                query.Where(p => p.AccountPeriod == month);
            }
            query.Where(p => p.Qty != 0);
            ////排除当前月数据
            query.Where(p => p.AccountPeriod != DateTime.Now.ToString("yyyyMM"));
            return query.OrderBy(p => p.AccountPeriod).ToList();
        }

        /// <summary>
        /// 生成库存月结数据
        /// </summary>
        /// <param name="isClearLastMonth">是否清除上个月月结，重新生成</param>
        public virtual void GenerateMonthlyBalance(bool isClearLastMonth = false)
        {
            var last1Month = DateTime.Now.AddMonths(-1).ToString("yyyyMM");
            var last2Month = DateTime.Now.AddMonths(-2).ToString("yyyyMM");
            var ctl = RT.Service.Resolve<MonthlyBalanceController>();

            var count = ctl.GetInvMonthlyBalanceCount(last1Month);
            if (count <= 0)
            {
                count = ctl.GetInvMonthlyBalanceCount(last2Month);
                if (count <= 0)
                {
                    last1Month = string.Empty;
                }
            }

            using (var tran = DB.TransactionScope(InveEntityDataProvider.ConnectionStringName))
            {
                if (isClearLastMonth || (!isClearLastMonth && last1Month.IsNullOrEmpty()))
                {
                    ////先清除上个月数据
                    ctl.DeleteMonthlyBalances(last1Month);
                    var last1MonthResults = GetMonthlySummaries(last1Month);
                    var monthGroups = last1MonthResults.GroupBy(p => new { p.AccountPeriod, p.StorerCode, p.WarehouseId, p.ItemId })
                        .Select(p => new { p.Key.AccountPeriod, p.Key.StorerCode, p.Key.WarehouseId, p.Key.ItemId, Qty = p.Sum(t => t.Qty) }).OrderBy(p => p.AccountPeriod);

                    foreach (var group in monthGroups)
                    {
                        DateTime lastMonthTemp = DateTime.ParseExact(group.AccountPeriod, "yyyyMM", System.Globalization.CultureInfo.CurrentCulture);
                        var last2MonthResult = GetInvMonthlyBalance(lastMonthTemp.AddMonths(-1).ToString("yyyyMM"), group.StorerCode, group.WarehouseId, group.ItemId);

                        var dtlList = last1MonthResults.Where(p => p.AccountPeriod == group.AccountPeriod && p.StorerCode == group.StorerCode && p.WarehouseId == group.WarehouseId &&
                            p.ItemId == group.ItemId).ToList();
                        var generationDate = DateTime.Now;

                        foreach (var result in dtlList)
                        {
                            var invMonthlyBalance = new MonthlyBalance();
                            invMonthlyBalance.AccountPeriod = result.AccountPeriod;
                            invMonthlyBalance.StorerCode = result.StorerCode;
                            invMonthlyBalance.WarehouseId = result.WarehouseId;
                            invMonthlyBalance.ItemId = result.ItemId;
                            invMonthlyBalance.OrderType = result.OrderType;
                            invMonthlyBalance.Qty = result.Qty;
                            invMonthlyBalance.OpeningInvQty = last2MonthResult == null ? 0 : last2MonthResult.EndingInvQty;
                            invMonthlyBalance.EndingInvQty = group.Qty + invMonthlyBalance.OpeningInvQty;
                            invMonthlyBalance.GenerationDate = generationDate;
                            RF.Save(invMonthlyBalance);
                        }
                    }
                }

                tran.Complete();
            }
        }

        /// <summary>
        /// 生成历史时节库存记录
        /// </summary>
        public virtual void GenerateHisLotLpnOnhands(DateTime curDate)
        {
            using (var tran = DB.TransactionScope(InveEntityDataProvider.ConnectionStringName))
            {
                DB.Update<LotLpnOnhand>().Execute();
                var results = RT.Service.Resolve<InvOnhandController>().GetLotLpnOnhands();
                var hisOnhands = new EntityList<HisLotLpnOnhand>();
                foreach (var result in results)
                {
                    var hisOnhand = new HisLotLpnOnhand();
                    hisOnhand.GenerationDate = curDate;
                    hisOnhand.ItemId = result.ItemId;
                    hisOnhand.WarehouseId = result.WarehouseId;
                    hisOnhand.StorageAreaId = result.StorageAreaId;
                    hisOnhand.StorageLocationId = result.StorageLocationId;
                    hisOnhand.Qty = result.Qty;
                    hisOnhand.AvailableQty = result.AvailableQty;
                    hisOnhand.FreezingQty = result.FreezingQty;
                    hisOnhand.AllottedQty = result.AllottedQty;
                    hisOnhand.StorerCode = result.StorerCode;
                    hisOnhand.LotCode = result.LotCode;
                    hisOnhand.LotId = result.LotId;
                    hisOnhand.Lpn = result.Lpn;
                    hisOnhand.ProjectNo = result.ProjectNo;
                    hisOnhand.TaskNo = result.TaskNo;
                    hisOnhand.State = result.State;
                    hisOnhand.ItemExtProp = result.ItemExtProp;
                    hisOnhand.ItemExtPropName = result.ItemExtPropName;
                    //批量插入，保存是不走验证规则，需自己验证，目前只适用oracle，项目上可按情况是否启用
                    ////hisOnhand.CreateBy = RT.IdentityId;
                    ////hisOnhand.CreateDate = DateTime.Now;
                    ////hisOnhand.UpdateBy = RT.IdentityId;
                    ////hisOnhand.UpdateDate = DateTime.Now;
                    ////InvOrgIdExtension.SetInvOrgId(hisOnhand, RT.InvOrg);
                    hisOnhands.Add(hisOnhand);
                }

                RF.Save(hisOnhands);

                //批量插入，保存是不走验证规则，需自己验证，目前只适用oracle，项目上可按情况是否启用
                ////BulkSaver.SetBatchEntityId(hisOnhands);
                ////RF.BatchInsert(hisOnhands);
                tran.Complete();
            }
        }
    }
}

using Newtonsoft.Json;
using SIE.Barcodes.Barcodes;
using SIE.Barcodes.Barcodes.ViewModels;
using SIE.Barcodes.WipBatchs;
using SIE.Common.Domain;
using SIE.Common.InvOrg;
using SIE.Common.NumberRules;
using SIE.Common.Prints;
using SIE.Core.Items;
using SIE.Core.Logs;
using SIE.Core.WorkOrders;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EventMessages.MES.Barcodes;
using SIE.EventMessages.MES.WIP;
using SIE.Rbac.InvOrgs;
using SIE.Security;
using SIE.Security.Authentications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace SIE.Barcodes
{
    /// <summary>
    /// 条码控制器
    /// </summary>
    public partial class BarcodeController : DomainController, IBarcode
    {
        /// <summary>
        /// 获取条码
        /// </summary>
        /// <param name="sn">SN号</param>
        /// <returns>条码</returns>
        public virtual Barcode GetBarcode(string sn)
        {
            Check.NotNullOrEmpty(sn, nameof(sn));
            return Query<Barcode>()
                .Where(p => p.Sn == sn)
                .FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据条码明细id获取条码明细(贪婪加载工单)
        /// </summary>
        /// <param name="id">条码明细id</param>
        /// <returns>条码明细</returns>
        public virtual Barcode GetBarcode(double id)
        {
            return Query<Barcode>()
                .Where(p => p.Id == id)
                .FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取条码
        /// </summary>
        /// <param name="sn">SN号</param>
        /// <returns>条码</returns>
        public virtual double GetBarcodeWorkOrderId(string sn)
        {
            Check.NotNullOrEmpty(sn, nameof(sn));
            return Query<Barcode>()
                .Where(p => p.Sn == sn)
                .Select(x => x.WorkOrderId)
                .FirstOrDefault<double>();
        }

        /// <summary>
        /// 查询条码是否属于有效条码
        /// </summary>
        /// <param name="sn">SN号</param>
        /// <returns>条码</returns>
        public virtual bool IsBarcodeEnabled(string sn)
        {
            Check.NotNullOrEmpty(sn, nameof(sn));
            return Query<Barcode>()
                .Where(p => p.Sn == sn && !p.IsScraped && !p.IsPending)
                .Count() >= 1;
        }

        /// <summary>
        /// 查询条码所属工单Id
        /// </summary>
        /// <param name="sn">SN号</param>
        /// <returns>工单Id</returns>
        public virtual double? GetBarcodeOrderId(string sn)
        {
            Check.NotNullOrEmpty(sn, nameof(sn));
            return Query<Barcode>()
                .Where(p => p.Sn == sn)
                .Select(p => p.WorkOrderId).FirstOrDefault()?.WorkOrderId;
        }

        /// <summary>
        /// 获取条码
        /// </summary>
        /// <param name="sn">SN号</param>
        /// <returns>条码</returns>
        public virtual Barcode GetBarcodeLoadWithProduct(string sn)
        {
            Check.NotNullOrEmpty(sn, nameof(sn));
            return Query<Barcode>().Where(p => p.Sn == sn).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取条码数量
        /// </summary>
        /// <param name="sn">条码号</param>
        /// <returns>条码数量</returns>
        public virtual decimal GetBarcodeQty(string sn)
        {
            Check.NotNullOrEmpty(sn, nameof(sn));
            return Query<Barcode>().Where(p => p.Sn == sn).Select(p => p.Qty).FirstOrDefault<decimal>();
        }

        /// <summary>
        /// 查询条码
        /// </summary>
        /// <param name="criteria">条码查询实体</param>
        /// <exception cref="ArgumentNullException">参数为空</exception>
        /// <returns>条码列表</returns>
        public virtual EntityList<Barcode> GetBarcodes(BarcodeCriteria criteria)
        {
            Check.NotNull(criteria, nameof(criteria));
            using (Diagnostics.DebugTrace.Start("条码查询：".L10N()))
            {
                var query = Query<Barcode>();
                if (!criteria.Sn.IsNullOrWhiteSpace())
                    query.Where(p => p.Sn.Contains(criteria.Sn));
                if (!criteria.WorkOrderNo.IsNullOrWhiteSpace())
                    query.Where(p => p.WorkOrder.No.Contains(criteria.WorkOrderNo));
                if (criteria.PrinterId.HasValue)
                    query.Where(p => p.PrintById == criteria.PrinterId);
                if (criteria.PrintDate.BeginValue.HasValue)
                    query.Where(p => p.PrintDate >= criteria.PrintDate.BeginValue);
                if (criteria.PrintDate.EndValue.HasValue)
                    query.Where(p => p.PrintDate <= criteria.PrintDate.EndValue);
                if (criteria.State.HasValue)
                    query.Where(p => p.PrintedState == criteria.State);
                if (criteria.IsScraped.HasValue)
                    query.Where(p => p.IsScraped == criteria.IsScraped);
                if (criteria.IsPending.HasValue)
                    query.Where(p => p.IsPending == criteria.IsPending);
                return query.OrderBy(criteria.OrderInfoList).ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
            }
        }

        /// <summary>
        /// 获取条码列表
        /// </summary>
        /// <param name="workOrderId">工单ID</param>
        /// <param name="isScraped">是否报废</param>
        /// <returns>条码列表</returns>
        public virtual EntityList<Barcode> GetBarcodes(double workOrderId, bool? isScraped)
        {
            var query = Query<Barcode>().Where(p => p.WorkOrderId == workOrderId);
            if (isScraped.HasValue)
            {
                query.Where(p => p.IsScraped == isScraped);
            }
            return query.ToList();
        }

        /// <summary>
        /// 获取生产工单Id所对应的所有条码信息列表
        /// </summary>
        /// <param name="workOrderIds">生产工单Ids列表</param>
        /// <returns>条码信息列表</returns>
        public virtual EntityList<Barcode> GetBarcodes(List<double> workOrderIds)
        {
            if (!workOrderIds.Any())
            {
                return new EntityList<Barcode>();
            }
            var orderIds = workOrderIds.Cast<double?>().ToList();
            return Query<Barcode>().Where(p => orderIds.Contains(p.WorkOrderId)).ToList();
        }

        /// <summary>
        /// 根据工单ID获取条码消息
        /// </summary>
        /// <param name="workorderId">工单ID</param> 
        /// <param name="sortInfo">排序</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <returns>条码信息</returns>
        public virtual EntityList<Barcode> GetBarcodes(double workorderId, List<OrderInfo> sortInfo = null, PagingInfo pagingInfo = null)
        {
            var query = Query<Barcode>().Where(p => p.WorkOrderId == workorderId);
            return query.OrderBy(sortInfo).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 条码领用
        /// </summary>
        /// <param name="rangeId">条码范围ID</param>
        /// <param name="userName">用户名</param>
        /// <param name="pwd">密码</param>
        /// <exception cref="ValidationException">用户名为空、条码已领用</exception>
        /// <exception cref="EntityNotFoundException">条码不存在</exception>
        public virtual void BarcodeReceive(double rangeId, string userName, string pwd)
        {
            if (userName.IsNullOrEmpty())
            {
                throw new ValidationException("用户名不能为空".L10N());
            }
            var range = RF.GetById<BarcodeRange>(rangeId);
            if (range == null)
            {
                throw new EntityNotFoundException(typeof(BarcodeRange), rangeId);
            }
            if (range.State == ReceiveState.Received)
            {
                throw new ValidationException("领用失败，条码状态为[{0}]".L10nFormat(ReceiveState.Received.ToLabel()));
            }
            LoginUser user = AuthenticateManager.Current.Authenticate(userName, pwd);
            range.ReceiveById = user.EmployeeId;
            range.ReceiveDate = RF.Find<BarcodeRange>().GetDbTime();
            range.State = ReceiveState.Received;
            RF.Save(range);
        }

        /// <summary>
        /// 条码是否存在
        /// </summary>
        /// <param name="sn">SN号</param>
        /// <returns>True:条码存在 False:条码不存在</returns>
        public virtual bool ExistsSn(string sn)
        {
            return Query<Barcode>().Where(p => p.Sn == sn).Count() > 0;
        }

        /// <summary>
        /// 是否跨库存组织存在
        /// </summary>
        /// <param name="sn"></param>
        /// <returns></returns>
        public virtual bool UinqueExists(string sn)
        {
            return Query<UniqueBarcode>().Where(p => p.Sn == sn).Count() > 0;
        }

        /// <summary>
        /// 条码是否存在
        /// </summary>
        /// <param name="sn">条码</param>
        /// <param name="type">工单类型</param>
        /// <returns>true:条码存在 ; false:条码不存在</returns>
        public virtual bool Exists(string sn, WorkOrderType type)
        {
            var result = Query<Barcode>().Where(x => x.Sn == sn && x.WorkOrder.Type == type).Count() > 0;
            return result;
        }

        /// <summary>
        /// 条码是否已经报废
        /// </summary>
        /// <param name="sn">条码</param>
        /// <param name="isScraped">是否报废</param>
        /// <returns>true: 已经报废; false:未报废</returns>
        public virtual bool Exists(string sn, bool isScraped)
        {
            var result = Query<Barcode>().Where(x => x.Sn == sn && x.IsScraped == isScraped).Count() > 0;
            return result;
        }

        /// <summary>
        /// 检验是否存在条码和工单
        /// </summary>
        /// <param name="sn">SN号</param>
        /// <param name="workOrderId">工单Id</param>
        /// <returns>True：存在条码  False:不存在条码</returns>
        public virtual bool Exists(string sn, double workOrderId)
        {
            return Query<Barcode>().Where(p => p.WorkOrderId == workOrderId && p.Sn == sn).Count() > 0;
        }

        /// <summary>
        /// 获取条码Id所对应的所有条码信息列表
        /// </summary>
        /// <param name="barcodeIds">条码Ids列表</param>
        /// <returns>条码信息列表</returns>
        public virtual EntityList<Barcode> GetBarcodesByIds(List<double> barcodeIds)
        {
            if (!barcodeIds.Any())
            {
                return new EntityList<Barcode>();
            }
            EntityList<Barcode> barcodeList = new EntityList<Barcode>();
            for (int i = 0; i < Math.Ceiling((double)barcodeIds.Count / 1000); i++)
            {
                var query = Query<Barcode>()
                    .Where(p => barcodeIds.Skip(i * 1000).Take(1000).Contains(p.Id));
                barcodeList.AddRange(query.ToList());
            }

            return barcodeList;
        }

        /// <summary>
        /// 获取条码号列表所对应的所有条码信息列表
        /// </summary>
        /// <param name="sns">条码号列表</param>
        /// <returns>条码信息列表</returns>
        public virtual EntityList<Barcode> GetBarcodesBySns(List<string> sns)
        {
            if (!sns.Any())
            {
                return new EntityList<Barcode>();
            }
            var elo = new EagerLoadOptions().LoadWithViewProperty().LoadWith(Barcode.WorkOrderProperty);
            EntityList<Barcode> barcodeList = new EntityList<Barcode>();
            for (int i = 0; i < Math.Ceiling((double)sns.Count / 1000); i++)
            {
                var query = Query<Barcode>()
                    .Where(p => sns.Skip(i * 1000).Take(1000).Contains(p.Sn));
                barcodeList.AddRange(query.ToList(null, elo));
            }

            return barcodeList;
        }

        /// <summary>
        /// 获取工单条码数量
        /// </summary>
        /// <param name="workOrderId">工单Id</param> 
        /// <returns>条码数量</returns>
        public virtual int GetBarcodeCount(double workOrderId)
        {
            var barcodes = GetBarcodes(workOrderId);
            return (int)barcodes.Sum(p => p.Qty);
        }

        /// <summary>
        /// 获取工单报废条码数量
        /// </summary>
        /// <param name="workOrderId">工单Id</param>
        /// <returns>报废条码数量</returns>
        public virtual int GetScrapBarcodeCount(double workOrderId)
        {
            var barcodes = GetBarcodes(workOrderId, true);
            return (int)barcodes.Sum(p => p.Qty);
        }

        /// <summary>
        /// 条码导入
        /// </summary>
        /// <param name="workOrderId">工单Id</param>
        /// <param name="sns">条码</param>
        public virtual void ImportBarcode(double workOrderId, List<string> sns)
        {
            sns.Sort();
            var result = sns.Distinct();
            int printQty = result.Count();
            PrintWorkOrder workOrder = ValidatePrintWorkOrder(workOrderId, printQty, result);
            string beginSn = result.FirstOrDefault();
            string endSn = result.LastOrDefault();
            using (var tran = DB.TransactionScope(BarcodeEntityDataProvider.ConnectionStringName))
            {
                var range = new BarcodeRange()
                {
                    PrintQty = printQty,
                    StartSn = beginSn,
                    EndSn = endSn,
                    Rule = null,
                    State = ReceiveState.NoReceive,
                    WorkOrderId = workOrderId,
                    Template = null
                };
                RF.Save(range);
                var barcodeList = new EntityList<Barcode>();
                foreach (var sn in result)
                {
                    //if (Exists(sn))
                    //{
                    //    throw new ValidationException("不允许导入,条码：{0} 已存在条码表".L10nFormat(sn));
                    //}
                    if (UinqueExists(sn))//校验所有库存组织均不重复
                    {
                        throw new ValidationException("不允许导入,条码：{0} 已存在系统条码表中".L10nFormat(sn));
                    }
                    var barcode = new Barcode()
                    {
                        Sn = sn,
                        IsScraped = false,
                        IsPending = false,
                        Qty = 1,
                        BoxesQty = 1,
                        IsMantissa = false,
                        WorkOrderId = workOrderId,
                        PrintedState = BarcodeState.Notprint,
                        Range = range
                    };
                    barcodeList.Add(barcode);
                }

                RF.Save(barcodeList);
                DB.Update<PrintWorkOrder>().Where(p => p.Id == workOrderId)
                  .Set(p => p.PrintedQty, p => p.PrintedQty + printQty).Execute();
                tran.Complete();
            }
        }

        /// <summary>
        /// 创建工单条码
        /// </summary>
        /// <param name="info">打印信息</param>
        /// <returns>错误信息和条码列表</returns>
        public virtual Tuple<string, EntityList<Barcode>> PrintBarcodes(PrinterInfo info)
        {
            string errMsg = string.Empty;
            EntityList<Barcode> barcodes = new EntityList<Barcode>();

            try
            {
                barcodes.AddRange(Print(info));
            }
            catch (Exception exc)
            {
                errMsg = exc.Message;
            }

            return new Tuple<string, EntityList<Barcode>>(errMsg, barcodes);
        }

        /// <summary>
        /// 创建工单条码
        /// </summary>
        /// <param name="info">打印信息</param>
        /// <returns>条码列表</returns>
        public virtual EntityList<Barcode> Print(PrinterInfo info)
        {
            using (var tran = DB.TransactionScope(BarcodeEntityDataProvider.ConnectionStringName))
            {
                if (info == null)
                {
                    throw new ValidationException("打印信息不能为空".L10N());
                }
                var workOrder = GetById<PrintWorkOrder>(info.WorkOrderId);
                if (workOrder == null)
                {
                    throw new ValidationException("工单不能为空".L10N());
                }
                var numberRule = GetById<NumberRule>(info.NumberRuleId);
                if (numberRule == null)
                {
                    throw new ValidationException("条码编码规则不能为空".L10N());
                }
                var template = GetById<PrintTemplate>(info.PrintTemplateId);
                if (template == null)
                {
                    throw new ValidationException("打印模板不能为空".L10N());
                }
                ValidateBarcodePrint(info, workOrder);
                int fullBoxCount = info.PrintQty / info.SingleQty;
                var printQty = info.PrintQty % info.SingleQty == 0 ? fullBoxCount : fullBoxCount + 1;
                var barcodes = RT.Service.Resolve<NumberRuleController>().GenerateSegment(numberRule.Id, printQty, workOrder);
                var range = new BarcodeRange()
                {
                    PrintQty = printQty,
                    StartSn = barcodes.FirstOrDefault(),
                    EndSn = barcodes.LastOrDefault(),
                    Rule = numberRule,
                    State = ReceiveState.NoReceive,
                    WorkOrderId = workOrder.Id,
                    Template = template,
                };
                RF.Save(range);
                DB.Update<PrintWorkOrder>().Where(p => p.Id == info.WorkOrderId)
                  .Set(p => p.PrintedQty, p => p.PrintedQty + info.PrintQty).Execute();

                var barcodeList = new EntityList<Barcode>();
                var now = RF.Find<Barcode>().GetDbTime();
                int printedCount = info.PrintQty;

                //todo 生成唯一条码表数据 
                var res = barcodes.SplitContains(codes => { return Query<UniqueBarcode>().Where(m => codes.Contains(m.Sn)).ToList(); });
                if (res.Any())
                {
                    var org = RT.Service.Resolve<InvOrgController>().GetByCode((int)res.First().InvOrgId);
                    throw new ValidationException("打印失败，【{1}】库存组织存在相同的【{0}】等条码".L10nFormat(res.First().Sn, org.Name));
                }

                foreach (var sn in barcodes)
                {
                    bool isMantissa = false;
                    int qty = info.SingleQty;
                    if (printedCount < info.SingleQty)
                    {
                        qty = printedCount;
                        isMantissa = true;
                    }

                    var barcode = new Barcode()
                    {
                        Sn = sn,
                        IsScraped = false,
                        IsPending = false,
                        Qty = qty,
                        BoxesQty = info.SingleQty,
                        IsMantissa = isMantissa,
                        WorkOrder = workOrder,
                        PrintDate = now,
                        PrintTimes = 1,
                        PrintedState = BarcodeState.Printed,
                        Range = range,
                        PrintById = AppRuntime.IdentityId,
                    };

                    barcode.CreateBy = RT.IdentityId;
                    barcode.CreateDate = now;
                    barcode.UpdateBy = RT.IdentityId;
                    barcode.UpdateDate = now;
                    InvOrgIdExtension.SetInvOrgId(barcode, RT.InvOrg);
                    PhantomEntityExtension.SetIsPhantom(barcode, false);
                    barcodeList.Add(barcode);

                    printedCount -= info.SingleQty;
                }
                ValidateSameBarcode(barcodes);
                var Logger = Logging.LogManager.GetLogger("startup_logger");
                using (Diagnostics.PerformenceWatcher.Start(Logger, "批量保存条码列表"))
                {
                    BulkSaver.SetBatchEntityId(barcodeList);
                    RF.BatchInsert(barcodeList);
                }
                tran.Complete();

                //推送打印条码消息到边端
                var printBarcodeInfo = new PrintBarcodeInfo();
                printBarcodeInfo.MsgType = "3";
                printBarcodeInfo.WorkOrderNo = barcodeList[0].WorkOrder.No;
                printBarcodeInfo.BarcodeList.AddRange(barcodeList);
                RT.EventBus.Publish<PrintBarcodeInfo>(printBarcodeInfo);
                return barcodeList;
            }
        }

        /// <summary>
        /// 验证条码重复
        /// </summary>
        /// <param name="barcodes">条码</param>
        private void ValidateSameBarcode(IEnumerable<string> barcodes)
        {
            var map = barcodes.AsParallel().ToLookup(p => p, k => 1);
            var reduce = from IGrouping<string, int> barcode in map.AsParallel()
                         where barcode.Count() > 1
                         select new { Word = barcode.Key, Count = barcode.Count() };
            var repeatBarcodes = reduce.Where(p => p.Count > 1);
            if (repeatBarcodes.Any())
            {
                throw new ValidationException("存在相同条码，请检查条码生成编码规则：{0}".L10nFormat(string.Join(";", repeatBarcodes.Select(p => p.Word))));
            }
            List<string> existSns = new List<string>();
            using (SIE.Diagnostics.DebugTrace.Start("Parallel.ForEach:".L10N()))
            {
                var splitBarcodes = SplitBarcodes(barcodes);
                System.Threading.Tasks.Parallel.ForEach(splitBarcodes, new System.Threading.Tasks.ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount / 2 }, (barcode) =>
                {
                    System.Diagnostics.Debug.WriteLine($"线程ID：{System.Threading.Thread.CurrentThread.ManagedThreadId}");
                    var res = Query<Barcode>().Where(p => barcode.Contains(p.Sn)).ToList().Select(p => p.Sn);
                    if (res.Any())
                    {
                        existSns.AddRange(res);
                    }
                });
            }
            if (existSns.Any())
            {
                throw new ValidationException("已经存在条码：{0}".L10nFormat(string.Join(";", existSns)));
            }
        }

        /// <summary>
        /// 拆分条码
        /// </summary>
        /// <param name="barcodes">条码集合</param>
        /// <returns>拆分后条码</returns>
        private List<IEnumerable<string>> SplitBarcodes(IEnumerable<string> barcodes)
        {
            List<IEnumerable<string>> splitRes = new List<IEnumerable<string>>();
            IEnumerable<string> remainBarcodes = barcodes;
            while (true)
            {
                if (remainBarcodes.Count() <= 1000)
                {
                    splitRes.Add(remainBarcodes);
                    break;
                }
                splitRes.Add(remainBarcodes.Take(1000));
                remainBarcodes = remainBarcodes.Skip(1000);
            }
            return splitRes;
        }

        /// <summary>
        /// 条码报废
        /// </summary>
        /// <param name="barcodeList">待报废条码列表</param>
        /// <param name="reason">报废原因</param>
        /// <exception cref="ValidationException">报废原因为空、报废条码为空、多次报废</exception>
        public virtual void BarcodeScrap(EntityList<Barcode> barcodeList, string reason)
        {
            ValidateBarcodeScrap(barcodeList, reason);
            using (var tran = DB.TransactionScope(BarcodeEntityDataProvider.ConnectionStringName))
            {
                foreach (var barcode in barcodeList)
                {
                    barcode.IsScraped = true;
                    RF.Save(barcode);
                    var workorder = RF.GetById<PrintWorkOrder>(barcode.WorkOrderId);
                    if (workorder == null)
                    {
                        throw new EntityNotFoundException(typeof(PrintWorkOrder), barcode.WorkOrderId);
                    }
                    DB.Update<PrintWorkOrder>().Where(p => p.Id == barcode.WorkOrderId)
                      .Set(p => p.PrintedQty, p => p.PrintedQty - (int)barcode.Qty).Execute();

                    var barcodeLog = new BarcodeLog()
                    {
                        Qty = (int)barcode.Qty,
                        Reason = reason,
                        BarcodeId = barcode.Id,
                        OperatDate = RF.Find<BarcodeLog>().GetDbTime(),
                        OperatorId = RT.IdentityId,
                        WorkOrderId = barcode.WorkOrderId.Value,
                        Type = BarcodeLogType.Scraped,
                    };
                    RF.Save(barcodeLog);
                }

                var rangeIds = barcodeList.Where(p => p.RangeId > 0).Select(p => p.RangeId.Value).Distinct();
                foreach (var rangeId in rangeIds)
                {
                    var rang = RF.GetById<BarcodeRange>(rangeId);
                    if (rang == null)
                    {
                        throw new EntityNotFoundException(typeof(BarcodeRange), rangeId);
                    }
                    rang.ScrapedQty += barcodeList.Count(p => p.RangeId == rangeId);
                    RF.Save(rang);
                }
                //发送EventBus删除拼板码与条码关系信息，修改工单未绑定SN数量
                var scrapBarcodes = barcodeList.Select(p => new BarcodeScrapInfo { Sn = p.Sn, workOrderId = p.WorkOrderId }).ToList();
                RT.EventBus.Publish(new BarcodeScrapEvent(scrapBarcodes));
                tran.Complete();
            }
        }

        /// <summary>
        /// 条码报废
        /// </summary>
        /// <param name="barcodes">待报废条码列表</param>
        /// <param name="reason">报废原因</param>
        /// <exception cref="ValidationException">报废原因为空、报废条码为空、多次报废</exception>
        public virtual void BarcodeScrap(List<string> barcodes, string reason)
        {
            var barcodeList = GetBarcodesBySns(barcodes);
            BarcodeScrap(barcodeList, reason);
        }

        /// <summary>
        /// 验证证条码报废
        /// </summary>
        /// <param name="barcodeList">待报废条码列表</param>
        /// <param name="reason">报废原因</param>
        /// <exception cref="ValidationException">报废原因为空、报废条码为空、多次报废</exception>
        private void ValidateBarcodeScrap(EntityList<Barcode> barcodeList, string reason)
        {
            if (reason.IsNullOrEmpty())
            {
                throw new ValidationException("报废原因不允许为空.".L10N());
            }
            if (barcodeList == null || barcodeList.Count == 0)
            {
                throw new ValidationException("报废条码不能为空".L10N());
            }
            var scrapList = barcodeList.Where(p => p.IsScraped);
            if (scrapList.Any())
            {
                throw new ValidationException("条码：{0} 是报废条码不允许多次报废".L10nFormat(string.Join(",", scrapList.Select(p => p.Sn).ToArray())));
            }
        }

        /// <summary>
        /// 条码补打
        /// </summary>
        /// <param name="barcodeList">选中条码列表</param>
        /// <param name="type">打印类型</param>
        /// <param name="reason">补打原因</param>
        /// <param name="times">补打次数</param>
        /// <returns>补打结果</returns>
        public virtual string ReprintBarcode(EntityList<Barcode> barcodeList, BarcodeLogType type, string reason, int times)
        {
            string errMsg = string.Empty;
            try
            {
                Reprint(barcodeList, type, reason, times);
            }
            catch (Exception exc)
            {
                errMsg = exc.Message;
            }

            return errMsg;
        }

        /// <summary>
        /// 补打
        /// </summary>
        /// <param name="barcodeList">条码列表</param>
        /// <param name="type">打印类型</param>
        /// <param name="reason">原因</param>
        /// <param name="times">打印次数</param>
        public virtual void Reprint(EntityList<Barcode> barcodeList, BarcodeLogType type, string reason, int times)
        {
            if (reason.IsNullOrWhiteSpace())
            {
                throw new ValidationException("补打原因不允许为空.".L10N());
            }
            if (barcodeList == null || barcodeList.Count == 0)
            {
                throw new ArgumentNullException(nameof(barcodeList));
            }
            if (times < 1)
            {
                throw new ValidationException("打印次数：{0} 必须大于等于 1".L10nFormat(times));
            }
            using (var tran = DB.TransactionScope(BarcodeEntityDataProvider.ConnectionStringName))
            {
                var now = DateTime.Now;
                foreach (var barcode in barcodeList)
                {
                    barcode.PrintTimes += times;
                    barcode.PrintDate = now;
                    barcode.PrintById = AppRuntime.IdentityId;
                    barcode.PrintedState = BarcodeState.Printed;
                    RF.Save(barcode);
                    var barcodeLog = new BarcodeLog()
                    {
                        Qty = times,
                        Reason = reason,
                        BarcodeId = barcode.Id,
                        OperatDate = now,
                        OperatorId = AppRuntime.IdentityId,
                        WorkOrderId = barcode.WorkOrderId.Value,
                        Type = type,
                    };
                    RF.Save(barcodeLog);
                }

                tran.Complete();
            }
        }

        /// <summary>
        /// 条码挂起
        /// </summary>
        /// <param name="barcodeList">待挂起条码列表</param>
        /// <param name="reason">挂起原因</param>
        /// <exception cref="ValidationException">挂起原因为空</exception>
        /// <exception cref="ValidationException">待挂起条码列表不允许为空</exception>
        public virtual void BarcodePending(EntityList<Barcode> barcodeList, string reason)
        {
            if (barcodeList == null || barcodeList.Count == 0)
            {
                throw new ValidationException("待挂起条码列表不允许为空.".L10N());
            }
            if (reason.IsNullOrEmpty())
            {
                throw new ValidationException("挂起原因不允许为空.".L10N());
            }
            using (var tran = DB.TransactionScope(BarcodeEntityDataProvider.ConnectionStringName))
            {
                foreach (var barcode in barcodeList)
                {
                    barcode.IsPending = true;
                    RF.Save(barcode);
                    var barcodeLog = new BarcodeLog()
                    {
                        Qty = (int)barcode.Qty,
                        Reason = reason,
                        BarcodeId = barcode.Id,
                        OperatDate = RF.Find<BarcodeLog>().GetDbTime(),
                        OperatorId = RT.IdentityId,
                        WorkOrderId = barcode.WorkOrderId.Value,
                        Type = BarcodeLogType.Pending,
                    };
                    RF.Save(barcodeLog);
                }

                tran.Complete();
            }
        }

        /// <summary>
        /// 条码恢复
        /// </summary>
        /// <param name="ids">待恢复条码列表</param>
        /// <exception cref="ValidationException">挂起原因为空</exception>
        public virtual void BarcodeResume(double[] ids)
        {
            if (ids == null || !ids.Any())
            {
                return;
            }
            using (var tran = DB.TransactionScope(BarcodeEntityDataProvider.ConnectionStringName))
            {
                foreach (var id in ids)
                {
                    var barcode = RF.GetById<Barcode>(id);
                    barcode.IsPending = false;
                    RF.Save(barcode);
                    var barcodeLog = new BarcodeLog()
                    {
                        Qty = (int)barcode.Qty,
                        BarcodeId = barcode.Id,
                        OperatDate = RF.Find<BarcodeLog>().GetDbTime(),
                        OperatorId = RT.IdentityId,
                        WorkOrderId = barcode.WorkOrderId.Value,
                        Type = BarcodeLogType.Resume,
                    };
                    RF.Save(barcodeLog);
                }

                tran.Complete();
            }
        }

        /// <summary>
        /// 计算工单已生成条码数
        /// </summary>
        /// <param name="workorderId">工单ID</param>
        /// <returns>条码信息</returns>
        public virtual decimal CountGenerateBarcode(double workorderId)
        {
            return GetBarcodes(workorderId).Sum(p => p.Qty);
        }

        /// <summary>
        /// 获取工单的标签打印模板
        /// </summary>
        /// <param name="workOrderId">工单ID</param>
        /// <returns>标签打印模板</returns>
        public virtual PrintTemplate GetPrintTemplateByWo(double workOrderId)
        {
            return RF.GetById<WorkOrder>(workOrderId)?.Template?.LabelTemplate;
        }

        /// <summary>
        /// 根据规则Id查询打印模板
        /// </summary>
        /// <param name="ruleId">规则Id</param>
        /// <param name="entityType">打印条码类型</param>
        /// <param name="info">分页信息</param>
        /// <param name="keyword">关键词</param>
        /// <returns>打印模板列表</returns>
        public virtual EntityList<PrintTemplate> GetPrintTemplatesByRuleId(double ruleId, string entityType, PagingInfo info = null, string keyword = "")
        {
            var query = Query<PrintTemplate>().Where(p => p.State == State.Enable).Where(p => p.EntityType.Contains(entityType));
            if (keyword.IsNotEmpty())
            {
                query.Where(p => p.FileName.Contains(keyword));
            }
            query.Join<NumberRuleInTemplate>((x, y) => x.Id == y.TemplateId && y.RuleId == ruleId);
            return query.ToList(info);
        }

        /// <summary>
        /// 根据打印条码类型获取打印模板
        /// </summary>
        /// <param name="entityType">打印条码类型</param>
        /// <param name="info">分页信息</param>
        /// <param name="keyword">关键词</param>
        /// <returns>打印模板列表</returns>
        public virtual EntityList<PrintTemplate> GetPrintTemplatesByType(string entityType, PagingInfo info = null, string keyword = "")
        {
            var query = Query<PrintTemplate>().Where(p => p.State == State.Enable).Where(p => p.EntityType.Contains(entityType));
            if (keyword.IsNotEmpty())
            {
                query.Where(p => p.FileName.Contains(keyword));
            }
            return query.ToList(info);
        }

        /// <summary>
        /// 查询条码范围
        /// </summary>
        /// <param name="workOrderId">工单Id</param>
        /// <returns>条码范围</returns>
        public virtual EntityList<BarcodeRange> GetBarcodeRanges(double workOrderId)
        {
            return Query<BarcodeRange>().Where(p => p.WorkOrderId == workOrderId).ToList();
        }

        /// <summary>
        /// 根据生产订单Id列表获取条码范围列表
        /// </summary>
        /// <param name="workOrderIds">生产订单Id列表</param>
        /// <returns>条码范围列表</returns>
        public virtual EntityList<BarcodeRange> GetBarcodeRanges(List<double> workOrderIds)
        {
            if (workOrderIds.Count == 0)
            {
                return new EntityList<BarcodeRange>();
            }
            return Query<BarcodeRange>().Where(p => workOrderIds.Contains(p.WorkOrderId)).ToList();
        }

        /// <summary>
        /// 默认查询条码领用方法
        /// </summary>
        /// <param name="criteria">查询条件</param>
        /// <exception cref="ArgumentNullException">参数为空</exception>
        /// <returns>BarcodeRange列表</returns>
        public virtual EntityList<BarcodeRange> GetBarcodeRanges(BarcodeRangeCriteria criteria)
        {
            Check.NotNull(criteria, nameof(criteria));
            var q = Query<BarcodeRange>();
            if (!criteria.WorkOrderNo.IsNullOrWhiteSpace())
            {
                q.Where(p => p.WorkOrder.No.Contains(criteria.WorkOrderNo));
            }
            if (criteria.ReceiveById.HasValue)
            {
                q.Where(p => p.ReceiveById == criteria.ReceiveById);
            }
            if (criteria.ReceiveDate.BeginValue.HasValue)
            {
                q.Where(p => p.ReceiveDate >= criteria.ReceiveDate.BeginValue);
            }
            if (criteria.ReceiveDate.EndValue.HasValue)
            {
                q.Where(p => p.ReceiveDate <= criteria.ReceiveDate.EndValue);
            }
            return q.OrderBy(criteria.OrderInfoList).ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 默认查询条码打印日志方法
        /// </summary>
        /// <param name="criteria">查询条件</param>
        /// <exception cref="ArgumentNullException">参数为空</exception>
        /// <returns>BarcodeLog列表</returns>
        public virtual EntityList<BarcodeLog> GetBarcodeLogs(BarcodeLogCriteria criteria)
        {
            Check.NotNull(criteria, nameof(criteria));
            var query = Query<BarcodeLog>();
            if (criteria.OperatDate.BeginValue.HasValue)
            {
                query.Where(p => p.OperatDate >= criteria.OperatDate.BeginValue);
            }
            if (criteria.OperatDate.EndValue.HasValue)
            {
                query.Where(p => p.OperatDate <= criteria.OperatDate.EndValue);
            }
            if (criteria.Operator != null)
            {
                query.Where(p => p.OperatorId == criteria.OperatorId.Value);
            }
            if (criteria.Type.HasValue)
            {
                query.Where(p => p.Type == criteria.Type);
            }
            if (!criteria.WorkOrderNo.IsNullOrWhiteSpace())
            {
                query.Where(p => p.WorkOrder.No.Contains(criteria.WorkOrderNo));
            }
            if (!criteria.Barcode.IsNullOrWhiteSpace())
            {
                query.Where(p => p.Barcode.Sn.Contains(criteria.Barcode));
            }
            return query.OrderBy(criteria.OrderInfoList).ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据条码打印条件获取工单
        /// </summary>
        /// <param name="criteria">条码打印查询实体</param>
        /// <exception cref="ArgumentNullException">参数为空</exception>
        /// <returns>工单列表</returns>
        public virtual EntityList<PrintWorkOrder> GetWorkOrders(PrintWorkOrderCriteria criteria)
        {
            Check.NotNull(criteria, nameof(criteria));
            var query = Query<PrintWorkOrder>().Where(p => p.State != WorkOrderState.CancelRelease);
            if (!criteria.No.IsNullOrWhiteSpace())
            {
                query.Where(p => p.No.Contains(criteria.No));
            }
            if (criteria.PlanBeginDate.BeginValue.HasValue)
            {
                query.Where(p => p.PlanBeginDate >= criteria.PlanBeginDate.BeginValue);
            }
            if (criteria.PlanBeginDate.EndValue.HasValue)
            {
                query.Where(p => p.PlanBeginDate <= criteria.PlanBeginDate.EndValue);
            }
            if (criteria.CreateDate.BeginValue.HasValue)
            {
                query.Where(p => p.CreateDate >= criteria.CreateDate.BeginValue);
            }
            if (criteria.CreateDate.EndValue.HasValue)
            {
                query.Where(p => p.CreateDate <= criteria.CreateDate.EndValue);
            }
            if (criteria.CreateBy != null)
            {
                query.Where(p => p.CreateBy == criteria.CreateById);
            }
            query.Join<ItemBatchRule>((x, y) => x.ProductId == y.ItemId && y.RetrospectType == RetrospectType.Single);
            query.Where(p => !p.IsPanelWorkOrder);
            return query.OrderBy(criteria.OrderInfoList).Distinct().ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 验证条码打印
        /// </summary>
        /// <param name="info">打印信息</param>
        /// <param name="workOrder">打印工单</param>
        private void ValidateBarcodePrint(PrinterInfo info, PrintWorkOrder workOrder)
        {
            if (info.PrintQty == 0)
            {
                throw new ValidationException("打印条码数必须大于0".L10N());
            }
            if (info.SingleQty <= 0)
            {
                throw new ValidationException("单张数量必须大于0".L10N());
            }
            int residualQty = (int)workOrder.PlanQty - info.PrintedQty;
            if (info.PrintQty > residualQty)
            {
                throw new ValidationException("打印数量：{0} 不能大于 剩余数量：{1}".L10nFormat(info.PrintQty, residualQty));
            }
        }

        /// <summary>
        /// 验证导入条码工单
        /// </summary>
        /// <param name="workOrderId">工单id</param>
        /// <param name="count">条码数量</param>
        /// <param name="sns">条码列表</param> 
        /// <returns>打印工单</returns>
        private PrintWorkOrder ValidatePrintWorkOrder(double workOrderId, int count, IEnumerable<string> sns)
        {
            Regex regex = new Regex(@"[\u4e00-\u9fa5]");
            if (count == 0)
            {
                throw new ValidationException("导入条码数量不能为 0".L10N());
            }
            ////中文字符验证
            sns.ForEach(sn =>
            {
                if (regex.IsMatch(sn))
                {
                    throw new ValidationException("不允许导入,条码：{0} 带有中文字符。".L10nFormat(sn));
                }
            });
            var workOrder = GetById<PrintWorkOrder>(workOrderId);
            if (workOrder == null)
            {
                throw new EntityNotFoundException(typeof(WorkOrder), workOrderId);
            }
            if (workOrder.PlanQty < workOrder.PrintedQty + count)
            {
                throw new ValidationException("已超过工单计划数量[{0}],请确认!".L10nFormat(workOrder.PlanQty));
            }
            return workOrder;
        }

        /// <summary>
        /// 通过工单Id获取条码打印
        /// </summary>
        /// <param name="workOrderId">工单Id</param>
        /// <returns>条码打印</returns>
        public virtual PrintWorkOrder GetPrintWorkOrder(double workOrderId)
        {
            return Query<PrintWorkOrder>().Where(p => p.Id == workOrderId).FirstOrDefault(new EagerLoadOptions().LoadWith(PrintWorkOrder.TemplateProperty));
        }

        /// <summary>
        /// 入库获取条码数据
        /// </summary>
        /// <param name="sns">条码</param>
        /// <returns>条码数据</returns>
        /// <remarks>WMS创建ASN调用</remarks>
        public virtual List<BarCodeInfoWithQty> GetBarcodeInfo(List<string> sns)
        {
            SaveGetBarcodeInfoLog(sns);

            List<BarCodeInfoWithQty> rst = new List<BarCodeInfoWithQty>();
            var barcodes = GetBarcodesBySns(sns);
            var batchSns = sns.Where(p => !barcodes.Contains(p)).ToList();
            var batchcodes = RT.Service.Resolve<WipBatchController>().GetWipBatches(batchSns);
            barcodes.ForEach(p =>
            {
                BarCodeInfoWithQty item = new BarCodeInfoWithQty
                {
                    Qty = p.Qty,
                    Sn = p.Sn
                };
                rst.Add(item);
            });
            batchcodes.ForEach(p =>
            {
                BarCodeInfoWithQty item = new BarCodeInfoWithQty
                {
                    Qty = p.Qty,
                    Sn = p.BatchNo
                };
                rst.Add(item);
            });

            return rst;
        }

        /// <summary>
        /// 保存入库获取条码数据日志
        /// </summary>
        /// <param name="sns">条码</param>
        private void SaveGetBarcodeInfoLog(List<string> sns)
        {
            using (var tran = DB.AutonomousTransactionScope(BarcodeEntityDataProvider.ConnectionStringName))
            {
                var strSns = JsonConvert.SerializeObject(sns);
                var inputValue = "条码:{0}".L10nFormat(strSns);
                var log = new InterfaceLog()
                {
                    Name = "IBarcode",
                    Method = "GetBarcodeInfo",
                    ControllerName = "BarcodeController",
                    InputValue = inputValue,
                };

                RF.Save(log);
                tran.Complete();
            }
        }

        /// <summary>
        /// 条码归属
        /// </summary>
        /// <param name="barBelongVM">条码归属ViewModel</param>
        /// <returns>错误信息</returns>
        public virtual string BarcodeBelong(BarcodeBelongViewModel barBelongVM)
        {
            string errMsg = string.Empty;
            try
            {

                if (barBelongVM == null || !barBelongVM.WorkOrderId.HasValue)
                {
                    throw new ValidationException("归属工单必填，条码归属失败!".L10N());
                }

                var barcode = GetBarcode(barBelongVM.BarcodeId);
                var belongWo = RF.GetById<WorkOrder>(barBelongVM.WorkOrderId);

                if (belongWo == null)
                {
                    throw new ValidationException("归属工单不存在，条码归属失败!".L10N());
                }

                //SN归属工单运行时检查
                RT.Service.Resolve<IWipController>().BarcodeBelongWorkOrderCheck(barcode.Sn);

                using (var tran = DB.TransactionScope(BarcodeEntityDataProvider.ConnectionStringName))
                {
                    BarcodeBelong(barcode, belongWo);

                    tran.Complete();
                }
            }
            catch (Exception exc)
            {
                errMsg = exc.Message;
            }

            return errMsg;
        }

        /// <summary>
        /// 条码归属
        /// </summary>
        /// <param name="barcode">条码</param> 
        /// <param name="belongWorkOrder">归属工单</param>
        public virtual void BarcodeBelong(Barcode barcode, WorkOrder belongWorkOrder)
        {
            Check.NotNull(barcode, nameof(barcode));
            Check.NotNull(belongWorkOrder, nameof(belongWorkOrder));

            if (!barcode.WorkOrderId.HasValue)
            {
                throw new ValidationException("原工单不存在，条码归属失败!".L10N());
            }

            var orgWo = RF.GetById<WorkOrder>(barcode.WorkOrderId);
            if (orgWo == null)
            {
                throw new ValidationException("原工单不存在，条码归属失败!".L10N());
            }

            if (barcode.IsScraped)
            {
                throw new ValidationException("工单[{0}]条码[{1}]已报废，条码归属失败!".L10nFormat(barcode.WONo, barcode.Sn));
            }
            if (barcode.IsPending)
            {
                throw new ValidationException("工单[{0}]条码[{1}]已挂起，条码归属失败!".L10nFormat(barcode.WONo, barcode.Sn));
            }
            var printingQty = belongWorkOrder.PlanQty - belongWorkOrder.PrintedQty;

            if (printingQty <= 0)
            {
                throw new ValidationException("归属工单[{0}]的剩余数量为0，条码归属失败!".L10nFormat(belongWorkOrder.No));
            }
            barcode.WorkOrderId = belongWorkOrder.Id;

            RF.Save(barcode);

            DB.Update<WorkOrder>()
                .Set(x => x.PrintedQty, x => x.PrintedQty + barcode.Qty)
                .Where(x => x.Id == belongWorkOrder.Id)
                .Execute();

            ////belongWorkOrder.PrintedQty += 1;

            ////RF.Save(belongWorkOrder);

            CreateBarcodeBelongLog(barcode, orgWo.Id, belongWorkOrder.Id);
        }

        /// <summary>
        /// 创建保存条码归属日志
        /// </summary>
        /// <param name="barcode">条码</param>
        /// <param name="orgWoId">原工单Id</param>
        /// <param name="belongWoId">归属工单Id</param>
        private void CreateBarcodeBelongLog(Barcode barcode, double orgWoId, double belongWoId)
        {
            var log = new BarcodeBelongLog()
            {
                OrgWorkOrderId = orgWoId,
                WorkOrderId = belongWoId,
                Sn = barcode.Sn,
                OperatorId = RT.IdentityId,
                OperatDate = RF.Find<BarcodeBelongLog>().GetDbTime(),
            };
            RF.Save(log);
        }
    }
}
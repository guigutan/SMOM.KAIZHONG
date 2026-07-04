using SIE.Common;
using SIE.Core.Enums;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.ManagedProperty;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Inventory.Transactions
{
    /// <summary>
    /// 功能/事务控制器
    /// </summary>
    public partial class TransactionController : DomainController
    {

        /// <summary>
        /// 获取单据大类
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public virtual EntityList<Function> GetFunctions(FunctionCriteria criteria)
        {
            var query = Query<Function>();
            if (criteria.Code.IsNotEmpty())
                query.Where(p => p.Code.Contains(criteria.Code));
            if (criteria.Name.IsNotEmpty())
                query.Where(p => p.Name.Contains(criteria.Name));
            if (criteria.Description.IsNotEmpty())
                query.Where(p => p.Description.Contains(criteria.Description));
            if (criteria.State.HasValue)
                query.Where(p => p.State == criteria.State);
            if (criteria.SourceType.HasValue)
                query.Where(p => p.SourceType == criteria.SourceType);
            if (criteria.FunctionType.HasValue)
                query.Where(p => p.FunctionType == criteria.FunctionType);
            if (criteria.OrderInfoList.Any())
                query.OrderBy(criteria.OrderInfoList);
            else
                query.OrderBy(p => p.FunctionType).OrderByDescending(p => p.UpdateDate);
            return query.ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());

        }

        /// <summary>
        /// 获取功能
        /// </summary>
        /// <param name="source">数据来源类型</param>
        /// <returns>返回功能列表</returns>
        public virtual EntityList<Function> GetFunctions(SourceType source)
        {
            return Query<Function>().Where(p => p.SourceType == source).ToList();
        }

        /// <summary>
        /// 获取单据大类数据
        /// </summary>
        /// <param name="ids">Id集合</param>
        /// <returns>单据大类数据</returns>
        public virtual EntityList<Function> GetFunctions(List<double> ids)
        {
            if (ids == null || ids.Count == 0)
            {
                return new EntityList<Function>();
            }
            return ids.SplitContains(sons =>
            {
                return Query<Function>().Where(p => sons.Contains(p.Id)).ToList();
            });
        }

        /// <summary>
        /// 获取单据大类数据
        /// </summary>
        /// <param name="codes">编码集合</param>
        /// <returns>单据大类数据</returns>
        public virtual EntityList<Function> GetFunctions(List<string> codes)
        {
            return codes.SplitContains(pCodes =>
            {
                return Query<Function>().Where(p => pCodes.Contains(p.Code)).ToList();
            });
        }

        /// <summary>
        /// 获取单据大类
        /// </summary>
        /// <param name="codes">编码</param>
        /// <returns>单据大类数据</returns>
        public virtual EntityList<Function> GetEnabelFuncions(List<string> codes)
        {
            return codes.SplitContains(pCodes =>
            {
                var query = DB.Query<Function>("A0").Where(p => p.State == State.Enable);
                NotExistFunctionEmplyee(query, Function.IdProperty);
                return query.Where(p => pCodes.Contains(p.Code)).ToList();
            });
        }

        /// <summary>
        /// 根据类型获取单据大类
        /// </summary>
        /// <param name="functionType">订单类型</param>
        /// <returns>返回功能列表</returns>
        public virtual Function GetFunctionByType(OrderType functionType, bool isNeedCheck = false)
        {
            var q = DB.Query<Function>("A0");
            if (isNeedCheck)
                NotExistFunctionEmplyee(q, Function.IdProperty);
            q.Where(x => x.Code == functionType.ToString());
            return q.FirstOrDefault();
        }

        /// <summary>
        /// 员工有大类权限
        /// </summary>
        /// <param name="functionType">大类</param>
        /// <returns></returns>
        public virtual bool EmployeeHasFunction(OrderType functionType)
        {
            var q = DB.Query<Function>("A0");
            NotExistFunctionEmplyee(q, Function.IdProperty);
            q.Where(x => x.Code == functionType.ToString());
            return q.Count() > 0;
        }

        /// <summary>
        /// 获取是否IQC报检By单据大类
        /// </summary>
        /// <param name="functionType">类型</param>
        /// <returns>bool</returns>
        public virtual bool GetIsIqcByType(OrderType functionType)
        {
            return Query<Function>().Where(x => x.Code == functionType.ToString() && x.IsQc).Count() > 0;
        }

        /// <summary>
        /// 根据类型获取单据大类
        /// </summary>
        /// <param name="codes">订单类型</param>
        /// <returns>返回功能列表</returns>
        public virtual EntityList<Function> GetFunctionByTypes(List<string> codes)
        {
            var q = Query<Function>();
            q.Where(x => codes.Contains(x.Code));
            return q.ToList();
        }

        /// <summary>
        /// 获取单据是否需要进行IQC报检标识
        /// </summary>
        /// <param name="orderType">单据类型</param>
        /// <returns>是否需要IQC报检</returns>
        public virtual bool GetIsQc(OrderType orderType)
        {
            bool flag = false;
            switch (orderType)
            {
                case OrderType.PurchaseIn:
                    flag = true;
                    break;
                case OrderType.CustomerIn:
                    flag = true;
                    break;
                case OrderType.VMIIN:
                    flag = true;
                    break;
                default:
                    flag = false;
                    break;
            }

            return flag;
        }

        /// <summary>
        /// 初始化功能
        /// </summary>
        public virtual void InitFunction(List<string> selectCodes)
        {
            using (var tran = DB.TransactionScope(InveEntityDataProvider.ConnectionStringName))
            {
                Function function = null;
                var alltrans = RF.GetAll<Transaction>();

                var receiptType = new List<OrderType>() { OrderType.PurchaseIn, OrderType.PartedIn, OrderType.CustomerIn, OrderType.VMIIN, OrderType.OtherIn,
                     OrderType.Finished, OrderType.MaterialReturn, OrderType.OutMaterialReturn, OrderType.SaleReturn, OrderType.CrossOrgTransferIn ,OrderType.WhTransferIn};
                var shipType = new List<OrderType>() { OrderType.OtherOut, OrderType.OutWorkFeed, OrderType.OutWorkFeedUse, OrderType.OutAllotReturn, OrderType.SaleOut, OrderType.WorkFeed, OrderType.WoFinishReturn, OrderType.SupplierReturn, OrderType.WhTransferOut };
                var invType = new List<OrderType>() { OrderType.DirectMove, OrderType.DirectAllocate, OrderType.TwoAllocate, OrderType.AllocateIn, OrderType.InventoryAdjust,
                 OrderType.StandardCount, OrderType.AccountCount, OrderType.RandomCount, OrderType.DifferenceCount, OrderType.CycleCount, OrderType.Frozen, OrderType.UnFrozen, OrderType.LotAdjust,
                  OrderType.StorerAdjust, OrderType.OnhandStateAdjust, OrderType.ProjectAdjust
                };
                var productType = new List<OrderType>() { OrderType.MaterialUp, OrderType.MaterialRecive, OrderType.MaterialDown, OrderType.WoFinish, OrderType.AutoJoinLineWarehouse, OrderType.MaterialReduce, };
                var otherType = new List<OrderType>() {   OrderType.InvProcess, OrderType.WaveAssign, OrderType.WavePick, OrderType.WaveReplenish, OrderType.DirectReplenish, OrderType.TwoReplenish,
                 OrderType.ReplenishIn, OrderType.Inspection, OrderType.CombineMerge, OrderType.BorrowLend, OrderType.WhReturn
                };

                foreach (OrderType item in Enum.GetValues(typeof(OrderType)))
                {
                    if (!selectCodes.Contains(item.ToString()))
                    {
                        continue;
                    }
                    function = GetFunctionByType(item);
                    if (function == null)
                    {
                        function = new Function();
                        function.Code = item.ToString();
                        function.Name = item.ToLabel();
                        function.Description = item.ToLabel();
                        function.State = State.Enable;
                        function.SourceType = SourceType.Internal;
                        function.IsQc = GetIsQc(item);
                        function.OrderType = item;
                        function.ConfigCollectType = Interfaces.CollectAutoGenerateType.NotGenerate;
                        if (item == OrderType.AllocateIn || item == OrderType.DifferenceCount || item == OrderType.CycleCount)
                        {
                            function.State = State.Disable;
                        }
                        if (item == OrderType.PurchaseIn)
                        {
                            function.IsCollectByDelivery = true;
                        }
                        if (receiptType.Contains(item))
                            function.FunctionType = FunctionType.Receipt;
                        else if (shipType.Contains(item))
                        {
                            function.FunctionType = FunctionType.Shipment;
                            function.IsAllowOut = true;
                        }
                        else if (invType.Contains(item))
                            function.FunctionType = FunctionType.Inventory;
                        else if (productType.Contains(item))
                            function.FunctionType = FunctionType.Product;
                        else
                            function.FunctionType = FunctionType.Other;

                        RF.Save(function);
                    }
                    else
                    {
                        if (receiptType.Contains(item))
                            function.FunctionType = FunctionType.Receipt;
                        else if (shipType.Contains(item))
                        {
                            function.FunctionType = FunctionType.Shipment;
                            function.IsAllowOut = true;
                        }
                        else if (invType.Contains(item))
                            function.FunctionType = FunctionType.Inventory;
                        else if (productType.Contains(item))
                            function.FunctionType = FunctionType.Product;
                        else
                            function.FunctionType = FunctionType.Other;
                        RF.Save(function);
                    }
                    if (item != OrderType.SupplierReturn && item != OrderType.BorrowLend)
                        InitTransaction(function);
                    InitExtTransaction(function, alltrans);
                }

                tran.Complete();
            }
        }

        /// <summary>
        /// 初始化基准小类
        /// </summary>
        /// <param name="function">单据大类</param>
        /// <returns>单据小类</returns>
        public virtual Transaction InitTransaction(Function function)
        {
            var relation = Query<FunctionTransaction>().Where(p => p.FunctionId == function.Id).FirstOrDefault();
            if (relation == null)
            {
                var transaction = Query<Transaction>().Where(p => p.Code == function.Code).FirstOrDefault();
                if (transaction == null)
                {
                    transaction = new Transaction()
                    {
                        Code = function.Code,
                        Name = function.Name,
                        IsEdit = true,
                    };
                    RF.Save(transaction);
                }
                else
                {//已经存在单据小类，删除原来单据小类的关系，绑定到初始化的大类中
                    DB.Delete<FunctionTransaction>().Where(p => p.TransactionId == transaction.Id).Execute();
                }


                FunctionTransaction functionTransaction = new FunctionTransaction()
                {
                    TransactionId = transaction.Id,
                    FunctionId = function.Id
                };
                RF.Save(functionTransaction);
                return transaction;
            }
            else
                return relation.Transaction;
        }

        /// <summary>
        /// 初始化特殊小类
        /// </summary>
        /// <param name="function"></param>
        public virtual void InitExtTransaction(Function function, EntityList<Transaction> alltrans)
        {
            EntityList<Transaction> transactions = new EntityList<Transaction>();
            if (function.Code == OrderType.SupplierReturn.ToString())
            {
                transactions.Add(new Transaction()
                {
                    Code = "ReturnFail",
                    Name = "来料不良退货".L10N(),
                    IsEdit = true,
                });

                transactions.Add(new Transaction()
                {
                    Code = "ReturnPass",
                    Name = "在库物料退货".L10N(),
                    IsEdit = true,
                });             
            }

            if (function.Code == OrderType.BorrowLend.ToString())
            {
                transactions.Add(new Transaction()
                {
                    Code = "LendOut",
                    Name = "借出".L10N(),
                    IsEdit = true,
                });

                transactions.Add(new Transaction()
                {
                    Code = "LendBack",
                    Name = "返还".L10N(),
                    IsEdit = true,
                });
            }

            if (function.Code == OrderType.OtherIn.ToString())
            {

                transactions.Add(new Transaction()
                {
                    Code = "OnhandInit",
                    Name = "库存初始化".L10N(),
                    IsEdit = true,
                });
            }
            if (function.Code == OrderType.WorkFeed.ToString())
            {
                transactions.Add(new Transaction()
                {
                    Code = "OverShip",
                    Name = "工单超发".L10N(),
                    IsEdit = true,
                });
            }
            transactions.ForEach(f =>
            {
                if (!alltrans.Any(a => a.Code == f.Code))
                    alltrans.Add(f);
            });



            EntityList<FunctionTransaction> funcTransactions = new EntityList<FunctionTransaction>();
            var newTransactions = alltrans.Where(f => f.PersistenceStatus == PersistenceStatus.New).AsEntityList();
            RF.Save(newTransactions);
            var curFunCodes = transactions.Select(a => a.Code).ToList();//本次添加的小类
            //查询大类小类关系
            var funTrans = Query<FunctionTransaction>().Where(f => curFunCodes.Contains(f.Transaction.Code)).ToList(null, new EagerLoadOptions().LoadWith(FunctionTransaction.TransactionProperty));
            alltrans.Where(p => curFunCodes.Contains(p.Code)).ForEach(p =>
             {
                 if (funTrans.Any(f => f.Transaction.Code == p.Code && f.FunctionId == function.Id))
                 {//已经存在相同关系的，忽略
                     return;
                 }
                 if (funTrans.Any(f => f.Transaction.Code == p.Code && f.FunctionId != function.Id))
                 {//存在不相同关系的，删除
                     DB.Delete<FunctionTransaction>().Where(f => f.TransactionId == p.Id).Execute();
                 }
                 funcTransactions.Add(new FunctionTransaction()
                 {//重新添加关系
                     TransactionId = p.Id,
                     FunctionId = function.Id
                 });
             });
            RF.Save(funcTransactions);
        }


        /// <summary>
        /// 禁用IQC报检
        /// </summary>
        /// <param name="funIdList">Id列表</param>
        public virtual void DisableFunctionIsQc(List<double> funIdList)
        {
            var funs = GetFunctions(funIdList);
            if (funs.Any(p => !p.IsQc))
                throw new ValidationException("单据大类已禁用IQC报检,不能再禁用".L10N());

            funs.ForEach(p => p.IsQc = false);
            RF.Save(funs);
        }

        /// <summary>
        /// 启用IQC报检
        /// </summary>
        /// <param name="funIdList">Id列表</param>
        public virtual void EnableFunctionIsQc(List<double> funIdList)
        {
            var funs = GetFunctions(funIdList);
            if (funs.Any(p => p.IsQc))
                throw new ValidationException("单据大类已启用IQC报检,不能再启用".L10N());

            funs.ForEach(p => p.IsQc = true);
            RF.Save(funs);
        }

        /// <summary>
        /// 禁用OQC报检
        /// </summary>
        /// <param name="funIdList">Id列表</param>
        public virtual void DisableFunctionOqc(List<double> funIdList)
        {
            var funs = GetFunctions(funIdList);
            if (funs.Any(p => !p.IsOqc))
                throw new ValidationException("单据大类已禁用OQC报检,不能再禁用".L10N());

            funs.ForEach(p => p.IsOqc = false);
            RF.Save(funs);
        }

        /// <summary>
        /// 启用OQC报检
        /// </summary>
        /// <param name="funIdList">Id列表</param>
        public virtual void EnableFunctionOqc(List<double> funIdList)
        {
            var funs = GetFunctions(funIdList);
            if (funs.Any(p => p.IsOqc))
                throw new ValidationException("单据大类已启用OQC报检,不能再启用".L10N());

            funs.ForEach(p => p.IsOqc = true);
            RF.Save(funs);
        }

        /// <summary>
        /// 禁用允许超发
        /// </summary>
        /// <param name="funIdList">Id列表</param>
        public virtual void DisableFunctionAllowOut(List<double> funIdList)
        {
            var funs = GetFunctions(funIdList);
            if (funs.Any(p => !p.IsAllowOut))
                throw new ValidationException("单据大类已禁用允许超发,不能再禁用".L10N());

            funs.ForEach(p => p.IsAllowOut = false);
            RF.Save(funs);
        }

        /// <summary>
        /// 启用允许超发
        /// </summary>
        /// <param name="funIdList">Id列表</param>
        public virtual void EnableFunctionAllowOut(List<double> funIdList)
        {
            var funs = GetFunctions(funIdList);
            if (funs.Any(p => p.IsAllowOut))
                throw new ValidationException("单据大类已启用允许超发,不能再启用".L10N());

            funs.ForEach(p => p.IsAllowOut = true);
            RF.Save(funs);
        }

        /// <summary>
        /// 禁用拣货后即发货
        /// </summary>
        /// <param name="funIdList">Id列表</param>
        public virtual void DisableFunctionIsAutoDelivery(List<double> funIdList)
        {
            var funs = GetFunctions(funIdList);
            if (funs.Any(p => !p.IsAutoDelivery))
                throw new ValidationException("单据大类已禁用拣货后即发货,不能再禁用".L10N());

            funs.ForEach(p => p.IsAutoDelivery = false);
            RF.Save(funs);
        }

        /// <summary>
        /// 启用拣货后即发货
        /// </summary>
        /// <param name="funIdList">Id列表</param>
        public virtual void EnableFunctionIsAutoDelivery(List<double> funIdList)
        {
            var funs = GetFunctions(funIdList);
            if (funs.Any(p => p.IsAutoDelivery))
                throw new ValidationException("单据大类已启用拣货后即发货,不能再启用".L10N());

            funs.ForEach(p => p.IsAutoDelivery = true);
            RF.Save(funs);
        }

        /// <summary>
        /// 禁用越库后自动拣货
        /// </summary>
        /// <param name="funIdList">Id列表</param>
        public virtual void DisableFunctionIsCrossPick(List<double> funIdList)
        {
            var funs = GetFunctions(funIdList);
            if (funs.Any(p => !p.IsCrossPick))
                throw new ValidationException("单据大类已禁用越库后自动拣货,不能再禁用".L10N());

            funs.ForEach(p => p.IsCrossPick = false);
            RF.Save(funs);
        }

        /// <summary>
        /// 启用越库后自动拣货
        /// </summary>
        /// <param name="funIdList">Id列表</param>
        public virtual void EnableFunctionIsCrossPick(List<double> funIdList)
        {
            var funs = GetFunctions(funIdList);
            if (funs.Any(p => p.IsCrossPick))
                throw new ValidationException("单据大类已启用越库后自动拣货,不能再启用".L10N());

            funs.ForEach(p => p.IsCrossPick = true);
            RF.Save(funs);
        }

        /// <summary>
        /// 启用按包装分配
        /// </summary>
        /// <param name="funIdList">Id列表</param>
        public virtual void EnableIsPackage(List<double> funIdList)
        {
            var funs = GetFunctions(funIdList);
            if (funs.Any(p => p.IsPickByPackage))
                throw new ValidationException("单据大类已启用按包装分配,不能再启用".L10N());

            funs.ForEach(p => p.IsPickByPackage = true);
            RF.Save(funs);
        }

        /// <summary>
        /// 禁用按包装分配
        /// </summary>
        /// <param name="funIdList">Id列表</param>
        public virtual void DisableIsPackage(List<double> funIdList)
        {
            var funs = GetFunctions(funIdList);
            if (funs.Any(p => !p.IsPickByPackage))
                throw new ValidationException("单据大类已禁用按包装分配,不能再禁用".L10N());

            funs.ForEach(p => p.IsPickByPackage = false);
            RF.Save(funs);
        }

        /// <summary>
        /// 启用收货到不合格
        /// </summary>
        /// <param name="funIdList">Id列表</param>
        public virtual void EnableIsReceiveNg(List<double> funIdList)
        {
            var funs = GetFunctions(funIdList);
            if (funs.Any(p => p.IsReceiveNg))
                throw new ValidationException("单据大类已启用收货到不合格,不能再启用".L10N());

            funs.ForEach(p => p.IsReceiveNg = true);
            RF.Save(funs);
        }

        /// <summary>
        /// 禁用收货到不合格
        /// </summary>
        /// <param name="funIdList">Id列表</param>
        public virtual void DisableIsReceiveNg(List<double> funIdList)
        {
            var funs = GetFunctions(funIdList);
            if (funs.Any(p => !p.IsReceiveNg))
                throw new ValidationException("单据大类已禁用收货到不合格,不能再禁用".L10N());

            funs.ForEach(p => p.IsReceiveNg = false);
            RF.Save(funs);
        }

        /// <summary>
        /// 启用部分发货
        /// </summary>
        /// <param name="funIdList">Id列表</param>
        public virtual void EnableIsPartDelivery(List<double> funIdList)
        {
            var funs = GetFunctions(funIdList);
            if (funs.Any(p => p.IsPartDelivery))
                throw new ValidationException("单据大类已启用部分发货,不能再启用".L10N());

            funs.ForEach(p => p.IsPartDelivery = true);
            RF.Save(funs);
        }

        /// <summary>
        /// 禁用部分发货
        /// </summary>
        /// <param name="funIdList">Id列表</param>
        public virtual void DisableIsPartDelivery(List<double> funIdList)
        {
            var funs = GetFunctions(funIdList);
            if (funs.Any(p => !p.IsPartDelivery))
                throw new ValidationException("单据大类已禁用部分发货,不能再禁用".L10N());

            funs.ForEach(p => p.IsPartDelivery = false);
            RF.Save(funs);
        }


        /// <summary>
        /// 获取单据大类数据
        /// </summary>
        /// <param name="isOqc">是否OQC</param>
        /// <returns>单据大类数据</returns>
        public virtual List<OrderType> GetFunctionsByIsOqc(bool isOqc)
        {
            List<OrderType> orderTypes = new List<OrderType>();
            var funs = Query<Function>().Where(p => p.IsOqc).ToList();
            funs.ForEach(p =>
            {
                if (System.Enum.TryParse(p.Code, out OrderType type))
                {
                    orderTypes.Add(type);
                }
            });

            return orderTypes;
        }

        /// <summary>
        /// 禁用按送货明细收货
        /// </summary>
        /// <param name="funIdList">Id列表</param>
        public virtual void DisableCollectByDelivery(List<double> funIdList)
        {
            var funs = GetFunctions(funIdList);
            if (funs.Any(p => !p.IsCollectByDelivery))
                throw new ValidationException("单据大类已禁用IQC报检,不能再禁用".L10N());

            funs.ForEach(p => p.IsCollectByDelivery = false);
            RF.Save(funs);
        }

        /// <summary>
        /// 启用按送货明细收货
        /// </summary>
        /// <param name="funIdList">Id列表</param>
        public virtual void EnableCollectByDelivery(List<double> funIdList)
        {
            var funs = GetFunctions(funIdList);
            if (funs.Any(p => p.IsCollectByDelivery))
                throw new ValidationException("单据大类已启用IQC报检,不能再启用".L10N());

            funs.ForEach(p => p.IsCollectByDelivery = true);
            RF.Save(funs);
        }


        /// <summary>
        /// 获取单据小类
        /// </summary>
        /// <param name="orderType">订单类型</param>
        /// <returns>单据小类信息</returns>
        public virtual EntityList<Transaction> GetTransactions(OrderType orderType)
        {
            var query = Query<Transaction>().Where(p => p.State == State.Enable);
            query.Join<FunctionTransaction>((x, y) => x.Id == y.TransactionId)
                .Join<FunctionTransaction, Function>((x, y) => x.FunctionId == y.Id && y.Code == orderType.ToString());
            return query.ToList();
        }

        /// <summary>
        /// 获取单据小类
        /// </summary>
        /// <param name="pagingInfo">分页信息</param>
        /// <param name="keyword">关键字</param>
        /// <param name="orderType">订单类型</param>
        /// <returns>单据小类信息</returns>
        public virtual EntityList<Transaction> GetTransactions(PagingInfo pagingInfo, string keyword, OrderType orderType)
        {
            var query = Query<Transaction>().Where(p => p.State == State.Enable);
            query.Join<FunctionTransaction>((x, y) => x.Id == y.TransactionId)
                .Join<FunctionTransaction, Function>((x, y) => x.FunctionId == y.Id && y.Code == orderType.ToString());
            if (orderType == OrderType.SupplierReturn)
                query.Where(a => a.Code != "ReceiveReturn");
            if (!keyword.IsNullOrEmpty())
            {
                query.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword));
            }
            return query.ToList();
        }

        /// <summary>
        /// 获取单据小类
        /// </summary>
        /// <param name="pagingInfo">分页信息</param>
        /// <param name="keyword">关键字</param>
        /// <param name="orderTypes">订单类型</param>
        /// <returns>单据小类信息</returns>
        public virtual EntityList<Transaction> GetTransactions(PagingInfo pagingInfo, string keyword, List<OrderType> orderTypes)
        {
            List<string> orderTypeCodes = orderTypes.Select(a => a.ToString()).ToList();
            var query = Query<Transaction>().Where(p => p.State == State.Enable);
            query.Join<FunctionTransaction>((x, y) => x.Id == y.TransactionId)
                .Join<FunctionTransaction, Function>((x, y) => x.FunctionId == y.Id && orderTypeCodes.Contains(y.Code));

            if (!keyword.IsNullOrEmpty())
            {
                query.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword));
            }
            return query.ToList();
        }

        /// <summary>
        /// 获取单据小类
        /// </summary>
        /// <param name="codes">编码集合</param>
        /// <returns>单据小类数据</returns>
        public virtual EntityList<Transaction> GetTransactionByCodes(List<string> codes)
        {
            return codes.SplitContains(pCodes =>
            {
                return Query<Transaction>().Where(p => pCodes.Contains(p.Code)).ToList();
            });
        }

        /// <summary>
        /// 获取单据小类
        /// </summary>
        /// <param name="idList">ID列表</param>
        /// <returns>单据小类信息</returns>
        public virtual EntityList<FunctionTransaction> GetTransactionDatas(List<double> idList)
        {
            var query = Query<FunctionTransaction>().Where(p => idList.Contains(p.Id));
            return query.ToList();
        }

        /// <summary>
        /// 根据订单类型获取单据小类
        /// </summary>
        /// <param name="orderType"></param>
        /// <returns></returns>
        public virtual Transaction GetDefaultTransactions(OrderType orderType)
        {
            var query = Query<Transaction>().Where(p => p.State == State.Enable).Where(p => p.Code != "ReceiveReturn");
            query.Join<FunctionTransaction>((x, y) => x.Id == y.TransactionId)
                .Join<FunctionTransaction, Function>((x, y) => x.FunctionId == y.Id)
                .Where<Function>((a, b) => b.Code == orderType.ToString()).OrderBy(p => p.Id);

            return query.FirstOrDefault();
        }

        /// <summary>
        /// 获取单据大类下的单据小类
        /// </summary>
        /// <param name="orderType">单据大类</param>
        /// <param name="transactions">单据小类</param>
        /// <returns></returns>
        public virtual EntityList<Transaction> GetOrderTypeTransactions(OrderType orderType, List<string> transactions)
        {
            var query = Query<Transaction>().Where(p => p.State == State.Enable).Where(p => p.Code != "ReceiveReturn");
            query.Join<FunctionTransaction>((x, y) => x.Id == y.TransactionId)
                .Join<FunctionTransaction, Function>((x, y) => x.FunctionId == y.Id)
                .Where<Function>((a, b) => b.Code == orderType.ToString());
            if (transactions != null && transactions.Count > 0)
            {
                query.Where(p => transactions.Contains(p.Name));
            }
            return query.ToList();
        }
        /// <summary>
        /// 获取大类与小类关系
        /// </summary>
        /// <param name="functionId"></param>
        /// <returns></returns>
        public virtual EntityList<FunctionTransaction> GetFunctionTransactions(double functionId, PagingInfo pagingInfo, IList<OrderInfo> sortInfo)
        {
            var query = Query<FunctionTransaction>().Where(p => p.FunctionId == functionId);
            pagingInfo.IsNeedCount = true;
            EagerLoadOptions elo = new EagerLoadOptions();
            elo.LoadWithViewProperty();
            query.OrderBy(sortInfo);
            return query.ToList(pagingInfo, elo);
        }

        /// <summary>
        /// 验证登录员工是否禁用单据大类
        /// </summary>
        /// <typeparam name="T">主体单据实体泛型</typeparam>
        /// <param name="query">查询</param>
        /// <param name="funIdProperty">主体单据大类托管属性</param>         
        public virtual void NotExistFunctionEmplyee<T>(IEntityQueryer<T> query, IManagedProperty funIdProperty) where T : DataEntity
        {
            query.NotExists<FunctionEmployee>((p, e) => e.Where(t => t.FunctionId == (double)p.GetProperty(funIdProperty) || p.GetProperty(funIdProperty) == null)
                 .Where(t => t.EmployeeId == RT.IdentityId));
        }
    }
}

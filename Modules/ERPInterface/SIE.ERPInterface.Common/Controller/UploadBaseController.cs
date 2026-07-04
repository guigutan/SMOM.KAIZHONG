using Org.BouncyCastle.Asn1.Ocsp;
using Org.BouncyCastle.Ocsp;
using SIE.Barcodes.WipBatchs;
using SIE.Common;
using SIE.Common.Configs;
using SIE.Core.Common;
using SIE.Core.Enums;
using SIE.Core.WorkOrders;
using SIE.CSM.Suppliers;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.ERPInterface.Common.Datas;
using SIE.ERPInterface.Common.Enums;
using SIE.ERPInterface.Common.Logs;
using SIE.ERPInterface.Common.UploadTransactionRules;
using SIE.EventMessages.ErpCommon;
using SIE.EventMessages.ErpCommon.Datas;
using SIE.Inventory.TransactionProcessing;
using SIE.Inventory.Transactions;
using SIE.Items;
using SIE.KZ.Base.SmomControl;
using SIE.MES.BatchWIP;
using SIE.MES.DispoLookups;
using SIE.MES.Outsourcing;
using SIE.MES.ReworkLayoutVersions;
using SIE.MES.TaskManagement.Configs;
using SIE.MES.TaskManagement.Dispatchs;
using SIE.MES.TaskManagement.FeedingRecords;
using SIE.MES.TaskManagement.Reports;
using SIE.MES.TaskManagement.SuspectProductLabels;
using SIE.MES.WorkOrders;
using SIE.Packages.ItemLabels;
using SIE.ProductIntfc.OutputProducts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Xml;

namespace SIE.ERPInterface.Common.Controller
{
    /// <summary>
    /// 上传基础控制器
    /// </summary>
    public class UploadBaseController : DomainController
    {
        #region 声明变量

        /// <summary>
        /// 最大批处理数量(根据项目实际需要，可以做成配置项或配置文件)
        /// </summary>
        public const int MAX_BATCH_QUANTITY = 1000;

        #endregion

        #region 通用

        /// <summary>
        /// 获取库存事务表未上传数据
        /// 根据变量MAX_BATCH_QUANTITY确定获取数量，默认值1000
        /// 贪懒加载
        /// </summary>
        /// <param name="tuples"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        protected virtual EntityList<InvTransaction> GetUnUploadInvTransactions(List<Tuple<OrderType, TransactionType, double?, string>> tuples, Expression<Func<InvTransaction, bool>> expression = null)
        {
            var q = Query<InvTransaction>();
            q.Where(p => p.UploadFlag == false).OrderBy(p => p.TransactionDate);

            //事务大小类查询条件
            if (tuples != null)
            {
                Expression<Func<InvTransaction, bool>> exp = null;
                for (int i = 0; i < tuples.Count; i++)
                {
                    var tuple = tuples[i];
                    Expression<Func<InvTransaction, bool>> where;
                    if (tuple.Item4.IsNotEmpty())
                    {
                        where = (f => f.OrderType == tuple.Item1 && f.TransactionType == tuple.Item2 && f.TransactionId == tuple.Item3 && f.BillNo == tuple.Item4);
                    }
                    else
                    {
                        if (tuple.Item3.HasValue && tuple.Item3 > 0)
                        {
                            where = (f => f.OrderType == tuple.Item1 && f.TransactionType == tuple.Item2 && f.TransactionId == tuple.Item3);
                        }
                        else
                        {
                            where = (f => f.OrderType == tuple.Item1 && f.TransactionType == tuple.Item2);
                        }
                    }

                    exp = exp == null ? Expression.Lambda<Func<InvTransaction, bool>>(where.Body, where.Parameters) : exp.Or(Expression.Lambda<Func<InvTransaction, bool>>(where.Body, where.Parameters));
                }
                if (exp != null) q.Where(exp);
            }

            if (expression != null) q.Where(expression);//扩展条件

            //贪婪加载
            EagerLoadOptions elo = new EagerLoadOptions();
            elo.LoadWith(InvTransaction.ItemProperty);
            elo.LoadWith(InvTransaction.TransactionProperty);
            elo.LoadWith(InvTransaction.FromWarehouseProperty);
            elo.LoadWith(InvTransaction.ToWarehouseProperty);
            elo.LoadWith(InvTransaction.FromLocationProperty);
            elo.LoadWith(InvTransaction.ToLocationProperty);
            elo.LoadWith(InvTransaction.CustomerProperty);
            elo.LoadWith(InvTransaction.SupplierProperty);
            elo.LoadWith(InvTransaction.UnitProperty);
            elo.LoadWith(InvTransaction.LotProperty);
            elo.LoadWithViewProperty();

            return q.ToList(new PagingInfo(1, MAX_BATCH_QUANTITY), elo);
        }

        /// <summary>
        /// 根据ID集合，查询实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ids"></param>
        /// <returns></returns>
        public virtual Dictionary<double, T> GetDataEntityDict<T>(List<double> ids) where T : DataEntity
        {
            var exp = ids.CreateContainsExpression<T>("x", "Id");
            var entityDict = Query<T>().Where(exp).ToList().ToDictionary(p => p.Id, p => p);
            return entityDict;
        }

        /// <summary>
        /// 获取中间表未处理和重试数据
        /// 根据变量MAX_BATCH_QUANTITY确定获取数量，默认值1000
        /// </summary>
        /// <param name="tuples"></param>
        /// <returns></returns>
        public virtual EntityList<UploadTransaction> GetUploadTransactions(List<Tuple<OrderType, TransactionType, double?, string>> tuples, Expression<Func<UploadTransaction, bool>> exp = null)
        {
            var q = Query<UploadTransaction>();
            q.Where(p => p.State == ProcessState.Unprocessed || p.State == ProcessState.Retry).OrderBy(p => p.TransactionDate);

            //事务大小类查询条件
            if (tuples != null)
            {
                //Expression<Func<UploadTransaction, bool>> exp = null;
                for (int i = 0; i < tuples.Count; i++)
                {
                    var tuple = tuples[i];
                    Expression<Func<UploadTransaction, bool>> where = null;
                    if (tuple.Item4.IsNotEmpty() && tuple.Item3.HasValue && tuple.Item3 > 0)//通过指定单据上传ERP的，需要排除上传超时的项
                    {
                        where = (f => f.OrderType == tuple.Item1 && f.TransactionType == tuple.Item2 && f.TransactionId == tuple.Item3.Value && f.BillNo == tuple.Item4 && f.DataKey.IsNullOrEmpty());
                    }
                    else if (tuple.Item4.IsNotEmpty())
                    {
                        where = (f => f.OrderType == tuple.Item1 && f.TransactionType == tuple.Item2 && f.BillNo == tuple.Item4 && f.DataKey.IsNullOrEmpty());
                    }
                    else if (tuple.Item3.HasValue && tuple.Item3 > 0)
                    {
                        where = (f => f.OrderType == tuple.Item1 && f.TransactionType == tuple.Item2 && f.TransactionId == tuple.Item3.Value);
                    }
                    else
                    {
                        where = (f => f.OrderType == tuple.Item1 && f.TransactionType == tuple.Item2);
                    }
                    
                    exp = exp == null ? Expression.Lambda<Func<UploadTransaction, bool>>(where.Body, where.Parameters) : exp.Or(Expression.Lambda<Func<UploadTransaction, bool>>(where.Body, where.Parameters));
                }

                if (exp != null) q.Where(exp);
            }

            //贪婪加载
            EagerLoadOptions elo = new EagerLoadOptions();
            elo.LoadWith(UploadTransaction.TransactionProperty);
            elo.LoadWithViewProperty();

            return q.ToList(new PagingInfo(1, MAX_BATCH_QUANTITY), elo);
        }

        #endregion

        #region 事务上传数据

        /// <summary>
        /// 从库存事务表表上传数据到事务上传表（上传中间表）
        /// </summary>
        /// <param name="tuples"></param>
        /// <param name="exclWhIds">目标仓排除清单</param>
        public virtual void UploadInvTransactionsToInf(List<Tuple<OrderType, TransactionType, double?, string>> tuples, List<double> exclWhIds = null)
        {

            //目标仓库排除
            Expression<Func<InvTransaction, bool>> expression = null;
            if (exclWhIds != null && exclWhIds.Count > 0) expression = (p => !exclWhIds.Contains(p.ToWarehouseId.Value));

            //捉取统计表数据
            var unUploadInvTransactions = this.GetUnUploadInvTransactions(tuples, expression);

            #region 数据查询

            //获取供应商地址集合
            var supplierAddrIds = unUploadInvTransactions.Where(p => p.SupplierAddress.HasValue).Select(p => p.SupplierAddress.Value).Distinct().ToList();
            var supplierAddrDict = new Dictionary<double, SupplierAddress>();
            if (supplierAddrIds.Any())
                supplierAddrDict = this.GetDataEntityDict<SupplierAddress>(supplierAddrIds);

            //采购订单
            var poIds = unUploadInvTransactions.Where(p => p.PoId.HasValue).Select(p => p.PoId.Value).Distinct().ToList();
            //var poDict = new Dictionary<double, PO.PurchaseOrders.PurchaseOrder>();
            //var poLineDict = new Dictionary<double, PO.PurchaseOrders.PurchaseOrderDetail>();
            //if (poIds.Any())
            //{
            //    poDict = this.GetDataEntityDict<PO.PurchaseOrders.PurchaseOrder>(poIds);
            //    var poLineIds = unUploadInvTransactions.Where(p => p.PoLineId.HasValue).Select(p => p.PoLineId.Value).Distinct().ToList();
            //    if (poLineIds.Any())
            //        poLineDict = this.GetDataEntityDict<PO.PurchaseOrders.PurchaseOrderDetail>(poLineIds);
            //}

            #endregion
            List<double> invIds = new List<double>();
            using (var trans = DB.TransactionScope(InterfaceEntityDataProvider.ConnectionStringName))
            {
                foreach (var invTransaction in unUploadInvTransactions)
                {
                    #region 取值

                    //供应商地址
                    SupplierAddress supplierAddress = null;
                    if (invTransaction.SupplierAddress.HasValue && supplierAddrDict.Any())
                        supplierAddrDict.TryGetValue(invTransaction.SupplierAddress.Value, out supplierAddress);

                    //采购订单
                    //PO.PurchaseOrders.PurchaseOrder po = null;
                    //if (invTransaction.PoId.HasValue)
                    //    poDict.TryGetValue(invTransaction.PoId.Value, out po);
                    //PO.PurchaseOrders.PurchaseOrderDetail poDtl = null;
                    //if (invTransaction.PoLineId.HasValue)
                    //    poLineDict.TryGetValue(invTransaction.PoLineId.Value, out poDtl);

                    #endregion

                    //this.SaveUploadTransaction(invTransaction, supplierAddress?.SourceKey, po?.SourceKey, poDtl?.SourceKey, poDtl?.IsReturnErp);      //保存事务上传表
                    invIds.Add(invTransaction.Id);
                }
                invIds.SplitDataExecute(sons =>
                {
                    DB.Update<InvTransaction>().Set(p => p.UploadFlag, true).Where(p => sons.Contains(p.Id)).Execute();   //更新库存事务表上传标记
                });

                trans.Complete();
            }
        }

        /// <summary>
        /// 从库存事务表表上传数据到事务上传表（上传中间表）
        /// </summary>
        /// <param name="transactionRule"></param>
        public virtual List<Tuple<OrderType, TransactionType, double?, string>> UploadInvTransactionsToInf(UploadTransactionRule transactionRule)
        {
            var transExclWhList = transactionRule?.UploadTransctionExclWhList;
            var transRuleDtlList = transactionRule?.UploadTransactionRuleDtlList;
            List<double> exclWhIds = null;

            if (transactionRule == null)
                throw new ValidationException("交易上传规则不能为空。".L10N());
            if (transRuleDtlList.Count <= 0)
                throw new ValidationException("交易上传规则明细，事务定义不能为空。");
            if (transExclWhList.Count > 0)
                exclWhIds = transExclWhList.Select(p => p.WarehouseId).Distinct().ToList();     //排除列表，目标仓ID

            var tuples = new List<Tuple<OrderType, TransactionType, double?, string>>();
            transRuleDtlList.ForEach(p =>
            {
                tuples.Add(new Tuple<OrderType, TransactionType, double?, string>(p.OrderType, p.TransactionType, p.TransactionId, ""));
            });

            //执行
            UploadInvTransactionsToInf(tuples, exclWhIds);

            return tuples;
        }

        /// <summary>
        /// 保存事务上传表
        /// </summary>
        /// <param name="invTransaction"></param>
        /// <param name="supplierAddrKey"></param>
        /// <param name="poKey"></param>
        /// <param name="poLineKey"></param>
        /// <param name="poIsReturnErp"></param>
        private void SaveUploadTransaction(InvTransaction invTransaction, string supplierAddrKey, string poKey, string poLineKey, bool? poIsReturnErp)
        {
            var uploadTransaction = new UploadTransaction();
            uploadTransaction.OrderType = invTransaction.OrderType;
            uploadTransaction.TransactionId = invTransaction.TransactionId.Value;
            uploadTransaction.TransactionType = invTransaction.TransactionType;
            uploadTransaction.State = ProcessState.Unprocessed;
            uploadTransaction.Quantity = invTransaction.Qty;
            uploadTransaction.TransactionDate = invTransaction.TransactionDate;
            uploadTransaction.InvTransactionId = invTransaction.Id;
            uploadTransaction.Remark = invTransaction.Remark;
            uploadTransaction.FromOnhandState = invTransaction.FromOnhandSate;
            //原因
            ////uploadTransaction.ReasonId;
            ////uploadTransaction.ReasonName;
            ////uploadTransaction.ReasonErpKey;
            //供应商
            uploadTransaction.SupplierId = invTransaction.SupplierId;
            uploadTransaction.SupplierErpKey = invTransaction.Supplier?.SourceKey;
            uploadTransaction.SupplierAddressId = invTransaction.SupplierAddress;
            uploadTransaction.SupplierAddrErpKey = supplierAddrKey;
            //物料
            uploadTransaction.LotCode = invTransaction.LotCode;
            uploadTransaction.ProductBatch = invTransaction.Lot?.LotAtt04;
            uploadTransaction.UnitCode = invTransaction.Unit?.Code;
            uploadTransaction.UnitName = invTransaction.Unit?.Name;
            uploadTransaction.ItemId = invTransaction.ItemId;
            uploadTransaction.ItemCode = invTransaction.Item?.Code;
            uploadTransaction.ItemName = invTransaction.Item?.Name;
            uploadTransaction.ItemErpKey = invTransaction.Item?.SourceKey;
            //单据
            uploadTransaction.BillNo = invTransaction.BillNo;
            uploadTransaction.BillId = invTransaction.BillId;
            uploadTransaction.BillErpKey = invTransaction.SourceBillId;
            uploadTransaction.BillLineId = invTransaction.BillDtlId;
            uploadTransaction.BillLineNo = invTransaction.BillDtlNo;
            uploadTransaction.BillLineErpKey = invTransaction.SourceBillDtlId;
            uploadTransaction.AsnNo = invTransaction.AsnNo;
            uploadTransaction.AsnLineNo = invTransaction.AsnLineNo;
            //跨组织调拨需记录发运单号
            uploadTransaction.SoNo = invTransaction.SourceBillNo;
            //采购订单
            uploadTransaction.PoId = invTransaction.PoId;
            uploadTransaction.PoNo = invTransaction.PoNo;
            uploadTransaction.PoLineId = invTransaction.PoLineId;
            uploadTransaction.PoErpKey = poKey;
            uploadTransaction.PoLineNo = invTransaction.PoLineNo;
            uploadTransaction.PoLineErpKey = poLineKey;
            uploadTransaction.PoIsReturnErp = poIsReturnErp;
            //工单
            if (invTransaction.OrderType == OrderType.MaterialReturn || invTransaction.OrderType == OrderType.WorkFeed || invTransaction.OrderType == OrderType.OutWorkFeed || invTransaction.OrderType == OrderType.PartedIn || invTransaction.OrderType == OrderType.Finished)
                uploadTransaction.WoNo = invTransaction.OrderNo;

            //来源仓库
            uploadTransaction.FromWarehouseId = invTransaction.FromWarehouseId;
            uploadTransaction.FromWarehouseCode = invTransaction.FromWarehouse?.Code;
            uploadTransaction.FromWarehouseName = invTransaction.FromWarehouse?.Name;
            uploadTransaction.FromWarehouseErpKey = invTransaction.FromWarehouse?.SourceKey;
            //来源库位
            uploadTransaction.FromLocationId = invTransaction.FromLocationId;
            uploadTransaction.FromLocationCode = invTransaction.FromLocation?.Code;
            uploadTransaction.FromLocationName = invTransaction.FromLocation?.Name;
            uploadTransaction.FromLocationErpKey = invTransaction.FromLocation?.SourceKey;
            //目标仓库
            uploadTransaction.ToWarehouseId = invTransaction.ToWarehouseId;
            uploadTransaction.ToWarehouseCode = invTransaction.ToWarehouse?.Code;
            uploadTransaction.ToWarehouseName = invTransaction.ToWarehouse?.Name;
            uploadTransaction.ToWarehouseErpKey = invTransaction.ToWarehouse?.SourceKey;
            //目标库位
            uploadTransaction.ToLocationId = invTransaction.ToLocationId;
            uploadTransaction.ToLocationCode = invTransaction.ToLocation?.Code;
            uploadTransaction.ToLocationName = invTransaction.ToLocation?.Name;
            uploadTransaction.ToLocationErpKey = invTransaction.ToLocation?.SourceKey;
            //跨组织调拨
            uploadTransaction.AllotModel = invTransaction.AllotModel;
            uploadTransaction.TargetErpOrganizationName = invTransaction.TargetErpOrganizationName;
            //ERP相关
            uploadTransaction.ErpOrganizationName = invTransaction.ErpOrganizationName;
            uploadTransaction.ErpOrgName = invTransaction.ErpOrgName;
            uploadTransaction.ErpWarehouseCode = invTransaction.ErpWarehouseCode;
            uploadTransaction.TargetErpWarehouseCode = invTransaction.TargetErpWarehouseCode;
            uploadTransaction.ErpAccount = invTransaction.ErpAccount;

            RF.Save(uploadTransaction);//保存事务上传表
        }

        /// <summary>
        /// 保存事务上传记录
        /// </summary>
        public virtual ProcessResult SaveTransactionData(SoapResult soapResult, EntityList<UploadTransaction> uploadTransactions,
           UploadTransactionLog uploadTransactionLog, Dictionary<string, SoapUploadParameterDtl> paraDtlDic)
        {
            //事务上传记录
            uploadTransactionLog.ErpBatchId = uploadTransactionLog.Id;
            uploadTransactionLog.ResponseCode = soapResult.X_RESPONSE_CODE;
            uploadTransactionLog.ResponseMessage = soapResult.X_RESPONSE_MESSAGE;
            uploadTransactionLog.RequestStr = soapResult.RequestStr;
            uploadTransactionLog.ResponseStr = soapResult.ResponseStr;
            uploadTransactionLog.RequestDate = soapResult.RequestDate;
            uploadTransactionLog.ResponseDate = soapResult.ResponseDate;
            RF.Save(uploadTransactionLog);

            var result = new ProcessResult();
            XmlNodeList xmlLineList = null;

            var xdoc = new XmlDocument();
            xdoc.LoadXml(soapResult.X_RESPONSE_DATA);
            xmlLineList = xdoc.GetElementsByTagName("LINE");

            if (xmlLineList == null || xmlLineList.Count <= 0) return result;//没有明细数据，退出

            //保存中间表数据
            for (int i = 0; i < xmlLineList.Count; i++)
            {
                try
                {
                    using (var trans = DB.TransactionScope(InterfaceEntityDataProvider.ConnectionStringName))
                    {
                        var dataNode = xmlLineList[i];
                        var logDtl = UpdateUploadTransactionState(dataNode, paraDtlDic);       //验证节点内容，匹配事务上传ID
                        logDtl.UploadTransactionLogId = uploadTransactionLog.Id;
                        RF.Save(logDtl);
                        trans.Complete();

                        if (logDtl.ProcessState != ProcessState.Processed)
                            throw new ValidationException(logDtl.ValidateMessage);
                    }

                    result.AddSuccessMsg();
                }
                catch (Exception ex)
                {
                    //失败记录
                    result.AddFailMsg(ex);
                }
            }

            return result;
        }

        /// <summary>
        /// 更新事务上传状态
        /// </summary>
        /// <param name="dataNode"></param>
        /// <param name="paraDtlDic"></param>
        private UploadTransactionLogDtl UpdateUploadTransactionState(XmlNode dataNode, Dictionary<string, SoapUploadParameterDtl> paraDtlDic)
        {
            XmlNode xmlLineId = dataNode.SelectSingleNode("LINE_SEQUENCE");
            XmlNode xmlValidateState = dataNode.SelectSingleNode("VALIDATE_STATUS");
            XmlNode xmlValidateMsg = dataNode.SelectSingleNode("VALIDATE_MESSAGE");
            XmlNode xmlProcessState = dataNode.SelectSingleNode("PROCESS_STATUS");
            XmlNode xmlProcessMsg = dataNode.SelectSingleNode("PROCESS_MESSAGE");
            ////XmlNode xmlWoNo = dataNode.SelectSingleNode("WIP_ENTITY_NAME");
            ////XmlNode xmlQty = dataNode.SelectSingleNode("MOVE_QUANTITY");
            ////XmlNode xmlType = dataNode.SelectSingleNode("MOVE_TYPE_CODE");

            SoapUploadParameterDtl para;
            if (!paraDtlDic.TryGetValue(xmlLineId.InnerText, out para))
                throw new ValidationException("ERP传出的行序列与MES传入的行序列不匹配".L10N());

            var state = xmlValidateState?.InnerText == "S" && xmlProcessState?.InnerText == "S" ? ProcessState.Processed : ProcessState.Failed;

            DB.Update<UploadTransaction>()
                .Set(p => p.State, state)
                .Set(p => p.ValidateMessage, xmlValidateMsg?.InnerText)
                .Set(p => p.ProcessMessage, xmlProcessMsg?.InnerText)
                .Where(p => p.Id == para.UploadTransactionId)
                .Execute();

            var logDtl = new UploadTransactionLogDtl();
            logDtl.UploadTransactionId = para.UploadTransactionId;
            logDtl.ProcessMessage = xmlProcessMsg?.InnerText;
            logDtl.ValidateMessage = xmlValidateMsg?.InnerText;
            logDtl.ProcessState = state;

            return logDtl;
        }



        #endregion

        #region 事务上传控制

        /// <summary>
        /// 调整失败事务
        /// </summary>
        /// <param name="transIds"></param>
        /// <param name="state"></param>
        public virtual void AdjustTransationFromCommand(List<double> transIds, ProcessState state)
        {
            var exp = transIds.CreateContainsExpression<UploadTransaction>("x", UploadTransaction.IdProperty.Name);
            DB.Update<UploadTransaction>().Set(p => p.State, state).Where(p => p.State == ProcessState.Failed || p.State == ProcessState.Abandon || p.State == ProcessState.Retry).Where(exp).Execute();
        }


        /// <summary>
        /// 调整失败事务
        /// </summary>
        /// <param name="transIds"></param>
        /// <param name="state"></param>
        public virtual void AdjustTransation(List<double> transIds, ProcessState state)
        {
            var exp = transIds.CreateContainsExpression<UploadTransaction>("x", UploadTransaction.IdProperty.Name);
            DB.Update<UploadTransaction>().Set(p => p.State, state).Where(p => p.State == ProcessState.Failed).Where(exp).Execute();
        }

        /// <summary>
        /// 调整失败事务
        /// </summary>
        /// <param name="transIds"></param>
        /// <param name="dateTime"></param>
        public virtual void AdjustTransation(List<double> transIds, DateTime dateTime)
        {
            var exp = transIds.CreateContainsExpression<UploadTransaction>("x", UploadTransaction.IdProperty.Name);
            DB.Update<UploadTransaction>().Set(p => p.TransactionDate, dateTime).Where(p => p.State == ProcessState.Failed).Where(exp).Execute();
        }

        #endregion

        #region 交易上传规则

        /// <summary>
        /// 初始化交易上传规则
        /// </summary>
        public virtual void InitTransRule()
        {
            var uploadTransTuples = this.GetUploadTransactionTuples();                                  //获取定义上传事务组合
            var uploadTransRuleDtls = this.GetUploadTransactionRuleDtls();                              //获取交易上传规则明细
            var uploadTransRules = uploadTransRuleDtls.Select(p => p.UploadTransactionRule).ToList();   //获取交易上传规则

            using (var trans = DB.TransactionScope(InterfaceEntityDataProvider.ConnectionStringName))
            {
                foreach (var tuple in uploadTransTuples)
                {
                    //数据量少，使用linq查询比字典效率更高
                    var rule = uploadTransRules.FirstOrDefault(p => p.Name == tuple.Item1);    //获取交易上传规则     
                    var ruleDtl = uploadTransRuleDtls.FirstOrDefault(p => p.OrderType == tuple.Item2 && p.TransactionType == tuple.Item3);     //根据单据大类和交易类型，获取交易上传规则明细

                    if (rule == null)
                    {
                        rule = new UploadTransactionRule();
                        rule.Name = tuple.Item1;
                        RF.Save(rule);
                        uploadTransRules.Add(rule);
                    }

                    if (ruleDtl == null)
                    {
                        ruleDtl = new UploadTransactionRuleDtl() { UploadTransactionRuleId = rule.Id, OrderType = tuple.Item2, TransactionType = tuple.Item3 };
                        RF.Save(ruleDtl);
                    }
                }
                trans.Complete();
            }
        }

        /// <summary>
        /// 获取定义上传事务组合(项目按照需要修改配置)
        /// </summary>
        /// <returns></returns>
        public virtual List<Tuple<string, OrderType, TransactionType>> GetUploadTransactionTuples()
        {
            var tuples = new List<Tuple<string, OrderType, TransactionType>>();
            tuples.Add(new Tuple<string, OrderType, TransactionType>("完工入库".L10N(), OrderType.Finished, TransactionType.InStorage));
            tuples.Add(new Tuple<string, OrderType, TransactionType>("生产退料".L10N(), OrderType.MaterialReturn, TransactionType.InStorage));
            tuples.Add(new Tuple<string, OrderType, TransactionType>("来料入库".L10N(), OrderType.PurchaseIn, TransactionType.InStorage));
            tuples.Add(new Tuple<string, OrderType, TransactionType>("来料接收".L10N(), OrderType.PurchaseIn, TransactionType.Receive));
            tuples.Add(new Tuple<string, OrderType, TransactionType>("销售出库".L10N(), OrderType.SaleOut, TransactionType.OutStorage));
            tuples.Add(new Tuple<string, OrderType, TransactionType>("销售退货".L10N(), OrderType.SaleReturn, TransactionType.InStorage));
            tuples.Add(new Tuple<string, OrderType, TransactionType>("供应商退货".L10N(), OrderType.SupplierReturn, TransactionType.OutStorage));
            tuples.Add(new Tuple<string, OrderType, TransactionType>("工单发料".L10N(), OrderType.WorkFeed, TransactionType.OutStorage));

            tuples.Add(new Tuple<string, OrderType, TransactionType>("来料检验".L10N(), OrderType.PurchaseIn, TransactionType.IqcQualified));
            tuples.Add(new Tuple<string, OrderType, TransactionType>("来料检验".L10N(), OrderType.PurchaseIn, TransactionType.IqcUnQualified));

            tuples.Add(new Tuple<string, OrderType, TransactionType>("其他出入库".L10N(), OrderType.OtherIn, TransactionType.InStorage));
            tuples.Add(new Tuple<string, OrderType, TransactionType>("其他出入库".L10N(), OrderType.OtherOut, TransactionType.OutStorage));

            return tuples;
        }

        /// <summary>
        /// 获取交易上传规则
        /// 数据量不会多，不分页
        /// </summary>
        /// <returns></returns>
        public virtual EntityList<UploadTransactionRule> GetUploadTransactionRules()
        {
            return Query<UploadTransactionRule>().ToList();
        }

        /// <summary>
        /// 获取交易上传规则明细
        /// 数据量不会多，不分页
        /// </summary>
        /// <returns></returns>
        public virtual EntityList<UploadTransactionRuleDtl> GetUploadTransactionRuleDtls()
        {
            var elo = new EagerLoadOptions().LoadWith(UploadTransactionRuleDtl.UploadTransactionRuleProperty);
            return Query<UploadTransactionRuleDtl>().ToList(null, elo);
        }

        /// <summary>
        /// 获取交易上传规则明细
        /// </summary>
        /// <param name="orderType">单据大类</param>
        /// <param name="transactionType">交易类型</param>
        /// <returns></returns>
        public virtual UploadTransactionRuleDtl GetUploadTransactionRuleDtl(OrderType orderType, TransactionType transactionType)
        {
            return Query<UploadTransactionRuleDtl>().Where(p => p.OrderType == orderType && p.TransactionType == transactionType).FirstOrDefault();
        }

        #endregion

        #region 工单报工事务上传

        /// <summary>
        /// 获取完工工单未上传数据
        /// 根据变量MAX_BATCH_QUANTITY确定获取数量，默认值1000
        /// 贪懒加载
        /// </summary>
        /// <returns></returns>
        protected virtual EntityList<SIE.MES.WorkOrders.WorkOrder> GetUnUploadFinishWorkOrders()
        {
            var q = Query<SIE.MES.WorkOrders.WorkOrder>();
            q.Where(p => p.State == WorkOrderState.Finish && p.UploadFlag == false).OrderBy(p => p.ActuFinishDate);

            //贪婪加载
            EagerLoadOptions elo = new EagerLoadOptions();
            elo.LoadWithViewProperty();

            return q.ToList(new PagingInfo(1, MAX_BATCH_QUANTITY), elo);
        }

        /// <summary>
        /// 获取未上传报工数据
        /// 根据变量MAX_BATCH_QUANTITY确定获取数量，默认值1000
        /// 贪懒加载
        /// </summary>
        /// <returns></returns>
        protected virtual EntityList<ReportRecord> GetUnUploadReportRecords()
        {
            //获取SAP上传配置
            var config = ConfigService.GetConfig(new ReportRecordUploadConfig(), typeof(ReportRecordExamine));
            if (config?.EnableUpload != true)
                return new EntityList<ReportRecord>();
            var uploadSteusList = config.UploadSteus.Split(",").ToList();

            var q = Query<ReportRecord>();
            q.Where(p => p.ExamineState == ReportRecordExamineState.Confirmed && (p.UploadFlag == false || p.UploadFlag == null)).OrderBy(p => p.CreateBy);
            //根据上传配置值过滤
            q.Where(p => uploadSteusList.Contains(p.Steus));

            //贪婪加载
            EagerLoadOptions elo = new EagerLoadOptions();
            elo.LoadWithViewProperty();

            return q.ToList(new PagingInfo(1, MAX_BATCH_QUANTITY), elo);
        }


        /// <summary>
        /// 从报工记录上传数据到事务上传表（上传中间表）
        /// </summary>
        public virtual void UploadTaskReportRecordToInf()
        {
            //捉取统计表数据
            var unUploadReportRecords = GetUnUploadReportRecords(); //this.GetUnUploadFinishWorkOrders();
            if (unUploadReportRecords.Count == 0)
                return;
            var woIds = unUploadReportRecords.Where(p => p.WorkOrderId.HasValue).Select(p => p.WorkOrderId.Value).Distinct().ToList();
            var layoutInfos = RT.Service.Resolve<SIE.MES.WorkOrders.WorkOrderController>().GetLayoutInfosByWorkOrderId(woIds);
            using (var trans = DB.TransactionScope(InterfaceEntityDataProvider.ConnectionStringName))
            {
                var result = new EntityList<UploadTransaction>();
                foreach (var unUploadReportRecord in unUploadReportRecords)
                {
                    result.Add(CreateUploadTransactionNew(unUploadReportRecord, layoutInfos));
                }
                List<double> ids = unUploadReportRecords.Select(c => c.Id).ToList();
                DB.Update<ReportRecord>().Set(p => p.UploadFlag, true).Where(p => ids.Contains(p.Id)).Execute();   //更新上传标记
                RF.Save(result);

                trans.Complete();
            }
        }

        /// <summary>
        /// 创建事务上传实体
        /// </summary>
        /// <param name="workOrder">工单</param>
        /// <returns></returns>
        private UploadTransaction CreateUploadTransaction(SIE.MES.WorkOrders.WorkOrder workOrder)
        {
            var uploadTransaction = new UploadTransaction();
            uploadTransaction.OrderType = OrderType.WoFinish;
            uploadTransaction.TransactionId = null;
            uploadTransaction.TransactionType = TransactionType.WoFinish;
            uploadTransaction.State = ProcessState.Unprocessed;

            uploadTransaction.TransactionDate = workOrder.ActuFinishDate.HasValue ? workOrder.ActuFinishDate.Value : DateTime.Now;
            uploadTransaction.WoId = workOrder.Id;
            uploadTransaction.Remark = string.Empty;
            uploadTransaction.FromOnhandState = null;
            //原因
            uploadTransaction.ReasonName = string.Empty;


            uploadTransaction.ItemId = workOrder.ProductId;
            uploadTransaction.ItemCode = workOrder.ProductCode;
            uploadTransaction.ItemName = workOrder.ProductName;

            uploadTransaction.OrdKey = (workOrder.ActuFinishDate.Value.ToString("yyyyMMdd") + "◇" + workOrder.No + "◇" + workOrder.Id).Replace('-', '^');

            //工单
            uploadTransaction.WoNo = workOrder.No;
            //uploadTransaction.ActuStartDate = workOrder.ActuStartDate;
            //uploadTransaction.ActuFinishDate = workOrder.ActuFinishDate;
            //uploadTransaction.ReportOkQty = workOrder.FinishQty;
            //uploadTransaction.PutIntoQty = workOrder.OnlineQty;
            //uploadTransaction.ProcessTechCode = workOrder.ProcessTechCode;
            //uploadTransaction.ProductionOrderCode = workOrder.ProductionOrderCode;

            ////人员
            //uploadTransaction.EmployeeId = workOrder.CreateBy;
            uploadTransaction.UploadCount = 0;

            return uploadTransaction;
        }

        /// <summary>
        /// 创建事务上传实体
        /// </summary>
        /// <param name="reportRecord">报工记录</param>
        /// <param name="layoutInfos">工艺路线</param>
        /// <returns></returns>
        private UploadTransaction CreateUploadTransactionNew(ReportRecord reportRecord, EntityList<SIE.MES.WorkOrders.LayoutInfo> layoutInfos)
        {
            var uploadTransaction = new UploadTransaction();
            uploadTransaction.OrderType = OrderType.WoFinish;
            uploadTransaction.TransactionId = null;
            uploadTransaction.TransactionType = TransactionType.WoFinish;
            uploadTransaction.State = ProcessState.Unprocessed;

            uploadTransaction.TransactionDate = reportRecord.ReportTime.HasValue ? reportRecord.ReportTime.Value : reportRecord.CreateDate;
            uploadTransaction.WoId = reportRecord.WorkOrderId;
            uploadTransaction.Remark = string.Empty;
            uploadTransaction.FromOnhandState = null;
            //原因
            uploadTransaction.ReasonName = string.Empty;


            uploadTransaction.ItemId = reportRecord.ProductId;
            uploadTransaction.ItemCode = reportRecord.ProductCode;
            uploadTransaction.ItemName = reportRecord.ProductName;

            uploadTransaction.OrdKey = "ReportRecord_" + reportRecord.Id.ToString();// (uploadTransaction.TransactionDate.ToString("yyyyMMdd") + "◇" + reportRecord.WorkOrderNo + "◇" + reportRecord.Id + "◇" + RT.InvOrg).Replace('-', '^');

            //工单
            uploadTransaction.WoNo = reportRecord.WorkOrderNo;
            uploadTransaction.Quantity = reportRecord.ReportQty;
            uploadTransaction.WorkCenter = "";
            var layoutInfo = layoutInfos.Where(p => p.WorkOrderId == reportRecord.WorkOrderId && p.ProcessCode == reportRecord.ProcessCode).FirstOrDefault();
            if (layoutInfo != null)
            {
                uploadTransaction.WorkCenter = layoutInfo.WorkCenterCode;
                uploadTransaction.WERKS = layoutInfo.Factory;
                uploadTransaction.Vornr = layoutInfo.Vornr;
            }

            uploadTransaction.ProcessCode = reportRecord.ProcessCode;
            uploadTransaction.ReportRecordId = reportRecord.Id;
            uploadTransaction.NgQty = reportRecord.NgQty;
            uploadTransaction.OkQty = reportRecord.OkQty;
            uploadTransaction.ReworkQty = reportRecord.ReworkQty;
            uploadTransaction.SuspectQty = reportRecord.SuspectQty;
            //uploadTransaction.ActuStartDate = workOrder.ActuStartDate;
            //uploadTransaction.ActuFinishDate = workOrder.ActuFinishDate;
            //uploadTransaction.repo = workOrder.FinishQty;
            //uploadTransaction.PutIntoQty = workOrder.OnlineQty;
            //uploadTransaction.ProcessTechCode = workOrder.ProcessTechCode;
            //uploadTransaction.ProductionOrderCode = workOrder.ProductionOrderCode;

            ////人员
            //uploadTransaction.EmployeeId = workOrder.CreateBy;
            uploadTransaction.UploadCount = 0;

            return uploadTransaction;
        }
        #endregion

        #region 扣料记录上传事务

        /// <summary>
        /// 扣料记录到事务上传
        /// </summary>
        public virtual void UploadDeductionToInf()
        {
            var deductionRecords = GetDeductionRecords();
            if (deductionRecords.Count < 1)
                return;

            using (var trans = DB.TransactionScope(InterfaceEntityDataProvider.ConnectionStringName))
            {
                EntityList<UploadTransaction> uploadTransactions = new EntityList<UploadTransaction>();
                var woIds = deductionRecords.Select(p => p.WorkOrderId).Distinct().ToList();
                var layoutInfos = RT.Service.Resolve<SIE.MES.WorkOrders.WorkOrderController>().GetLayoutInfosByWorkOrderId(woIds);
                var dateTime = RF.Find<UploadTransaction>().GetDbTime();
                foreach (var g in deductionRecords.GroupBy(p => new { p.WorkOrderId, p.ItemId, p.ProcessCode, p.ItemLabelLot, p.Vornr, p.Lgort }))
                {
                    uploadTransactions.Add(CreateUploadTransaction(g.ToList(), layoutInfos, dateTime));

                    foreach (var deductionRecord in g)
                    {
                        deductionRecord.UploadFlag = true;
                        deductionRecord.PersistenceStatus = PersistenceStatus.Modified;
                        RF.Save(deductionRecord);
                    }

                }
                RF.Save(uploadTransactions);
                //RF.Save(deductionRecords);
                trans.Complete();
            }
        }

        public virtual EntityList<DeductionRecord> GetDeductionRecords()
        {
            //扣料上传的物料只根据材料MRB控制者和物料类型字段进行判断,且物料标签来源类型不为批次生产采集，满足的物料才进行数据上传
            var list = Query<DeductionRecord>().Join<ItemLabel>((x, y) => x.ItemLabelId == y.Id && y.SourceType != LabelSource.BatchWip).Join<ItemLabel, Item>((x, y) => x.ItemId == y.Id).Join<Item, DispoLookup>((x, y) => x.MrpController == y.MaterialDispo && x.Mtart == y.Mtart)
              .Where(p => (p.UploadFlag == null || p.UploadFlag == false)).OrderByDescending(p => p.UpdateDate).ToList(new PagingInfo(1, MAX_BATCH_QUANTITY), new EagerLoadOptions().LoadWithViewProperty());

            //扣料上传的物料标签来源类型为批次生产采集
            var list1 = Query<DeductionRecord>().Join<ItemLabel>((x, y) => x.ItemLabelId == y.Id && y.SourceType == LabelSource.BatchWip)
                .Where(p => (p.UploadFlag == null || p.UploadFlag == false)).OrderByDescending(p => p.UpdateDate).ToList(new PagingInfo(1, MAX_BATCH_QUANTITY), new EagerLoadOptions().LoadWithViewProperty());

            list.AddRange(list1);

            //var list = Query<DeductionRecord>()
            //    .Join<ReportRecord>("record", (x, y) => x.ReportRecordId == y.Id)
            //    .Join<ReportRecord, WorkOrder>("wo", (x, y) => x.WorkOrderId == y.Id)
            //    .Join<ItemLabel>("label", (x, y) => x.ItemLabelId == y.Id)
            //    .Join<DispoLookup>("up", (x, y) => x.ReportRecord.WorkOrder.Fevor == y.Fevor
            //    //当用到两个Item表的时候，产品没法识别，只能用sql的方式实现
            //    && y.SQL<bool>("exists(select 1 from Item where item.is_phantom = 0 and item.id = wo.product_id and item.Mrp_Controller = up.Dispo)")
            //    && y.SQL<bool>("exists(select 1 from item where item.is_phantom = 0 and item.id = label.Item_Id and item.Mrp_Controller = up.Material_Dispo)")
            //    /*&& x.ReportRecord.WorkOrder.Product.MrpController == y.Dispo && x.ItemLabel.Item.MrpController == y.MaterialDispo*/)
            //    .Where(p => (p.UploadFlag == null || p.UploadFlag == false)).ToList(new PagingInfo(1, MAX_BATCH_QUANTITY), new EagerLoadOptions().LoadWithViewProperty());
            return list;
        }

        /// <summary>
        /// 创建事务上传
        /// </summary>
        /// <returns></returns>
        public virtual UploadTransaction CreateUploadTransaction(List<DeductionRecord> deductionRecords, EntityList<LayoutInfo> layoutInfos, DateTime dateTime)
        {
            var deductionRecord = deductionRecords.FirstOrDefault();

            UploadTransaction uploadTransaction = new UploadTransaction();

            uploadTransaction.OrderType = OrderType.Deduction;
            uploadTransaction.TransactionId = null;
            uploadTransaction.TransactionType = TransactionType.Deduction;
            uploadTransaction.State = ProcessState.Unprocessed;
            //uploadTransaction.BillId = deductionRecord.Id;
            uploadTransaction.SourceId = string.Join(';', deductionRecords.Select(p => p.Id));

            uploadTransaction.TransactionDate = dateTime;//deductionRecord.CreateDate;
            uploadTransaction.WoId = deductionRecord.ReportRecord.WorkOrderId;
            uploadTransaction.Remark = string.Empty;
            uploadTransaction.FromOnhandState = null;
            //原因
            uploadTransaction.ReasonName = string.Empty;


            uploadTransaction.ItemId = deductionRecord.ItemId;
            uploadTransaction.ItemCode = deductionRecord.ItemCode;
            uploadTransaction.ItemName = deductionRecord.ItemName;

            uploadTransaction.OrdKey = "DeductionRecord_" + deductionRecord.Id.ToString();/*(uploadTransaction.TransactionDate.ToString("yyyyMMdd") + "◇" + reportRecord.WorkOrderNo + "◇" + reportRecord.Id + "◇" + RT.InvOrg).Replace('-', '^');*/

            //工单
            uploadTransaction.WoNo = deductionRecord.WorkOrderNo;
            uploadTransaction.Quantity = deductionRecords.Sum(p => p.DeductedQty.Value); //deductionRecord.DeductedQty.Value;
            uploadTransaction.WorkCenter = "";
            var layoutInfo = layoutInfos.Where(p => p.WorkOrderId == deductionRecord.WorkOrderId && p.ProcessCode == deductionRecord.ProcessCode).FirstOrDefault();
            if (layoutInfo != null)
            {
                uploadTransaction.WorkCenter = layoutInfo.WorkCenterCode;
                uploadTransaction.WERKS = layoutInfo.Factory;
            }
            uploadTransaction.LotCode = deductionRecord.ItemLabel.Lot;
            uploadTransaction.Vornr = deductionRecord.ReportRecord.Vornr;
            uploadTransaction.ProcessCode = deductionRecord.ProcessCode;
            //uploadTransaction.ReportRecordId = deductionRecord.ReportRecordId;
            var bom = deductionRecord.ReportRecord.WorkOrder.BomList.FirstOrDefault(p => p.ItemId == deductionRecord.ItemLabel.ItemId);
            uploadTransaction.ToLocationCode = bom == null || bom.Lgort.Trim().IsNullOrEmpty() ? deductionRecord.ItemLabel.Lgort : bom.Lgort;

            //uploadTransaction.NgQty = reportRecord.NgQty;
            //uploadTransaction.OkQty = reportRecord.OkQty;
            //uploadTransaction.ReworkQty = reportRecord.ReworkQty;
            //uploadTransaction.SuspectQty = reportRecord.SuspectQty;
            uploadTransaction.UploadCount = 0;

            uploadTransaction.PersistenceStatus = PersistenceStatus.New;
            return uploadTransaction;
        }

        #endregion

        #region 余料称重记录上传事务

        /// <summary>
        /// 创建事务上传
        /// </summary>
        public virtual void UploadScrapWeighingRecordToInf()
        {
            var records = Query<ScrapWeighingRecord>().Where(p => p.DeductedQty != null && p.DeductedQty > 0 && (p.UploadFlag == null || p.UploadFlag == false)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            if (records.Count < 1)
                return;

            using (var trans = DB.TransactionScope(InterfaceEntityDataProvider.ConnectionStringName))
            {
                EntityList<UploadTransaction> uploadTransactions = new EntityList<UploadTransaction>();
                var woIds = records.Where(p => p.WorkOrderId != null).Select(p => p.WorkOrderId.Value).Distinct().ToList();
                var layoutInfos = RT.Service.Resolve<SIE.MES.WorkOrders.WorkOrderController>().GetLayoutInfosByWorkOrderId(woIds);
                var dateTime = RF.Find<UploadTransaction>().GetDbTime();
                foreach (var g in records)
                {
                    uploadTransactions.Add(CreateUploadTransaction(g, layoutInfos, dateTime));
                    g.UploadFlag = true;
                    g.PersistenceStatus = PersistenceStatus.Modified;
                    RF.Save(g);
                }
                RF.Save(uploadTransactions);
                trans.Complete();
            }
        }

        public virtual UploadTransaction CreateUploadTransaction(ScrapWeighingRecord record,EntityList<LayoutInfo> layoutInfos, DateTime dateTime)
        {

            UploadTransaction uploadTransaction = new UploadTransaction();

            uploadTransaction.OrderType = OrderType.Deduction;//OrderType.ScrapWeighing;
            uploadTransaction.TransactionId = null;
            uploadTransaction.TransactionType = TransactionType.ScrapWeighing;
            uploadTransaction.State = ProcessState.Unprocessed;
            //uploadTransaction.BillId = deductionRecord.Id;
            uploadTransaction.SourceId = record.Id.ToString();

            uploadTransaction.TransactionDate = dateTime;//deductionRecord.CreateDate;
            uploadTransaction.WoId = record.WorkOrderId;
            uploadTransaction.Remark = string.Empty;
            uploadTransaction.FromOnhandState = null;
            uploadTransaction.BillId = record.Id;
            //原因
            uploadTransaction.ReasonName = string.Empty;


            uploadTransaction.ItemId = record.ItemId;
            uploadTransaction.ItemCode = record.ItemCode;
            uploadTransaction.ItemName = record.ItemName;

            uploadTransaction.OrdKey = "ScrapWeighingRecord_" + record.Id.ToString();/*(uploadTransaction.TransactionDate.ToString("yyyyMMdd") + "◇" + reportRecord.WorkOrderNo + "◇" + reportRecord.Id + "◇" + RT.InvOrg).Replace('-', '^');*/

            //工单
            uploadTransaction.WoNo = record.DispatchTask?.WorkOrder?.No;
            uploadTransaction.Quantity = record.DeductedQty ?? 0; //deductionRecord.DeductedQty.Value;
            uploadTransaction.WorkCenter = "";
            var layoutInfo = layoutInfos.Where(p => p.WorkOrderId == record.DispatchTask?.WorkOrderId && p.ProcessCode == record.ProcessCode).FirstOrDefault();
            if (layoutInfo != null)
            {
                uploadTransaction.WorkCenter = layoutInfo.WorkCenterCode;
                uploadTransaction.WERKS = layoutInfo.Factory;
                uploadTransaction.Vornr = layoutInfo.Vornr;
            }
            uploadTransaction.LotCode = record.Lot;
            uploadTransaction.ProcessCode = record.ProcessCode;
            var bom = record.DispatchTask?.WorkOrder.BomList.FirstOrDefault(p => p.ItemId == record.ItemId);
            uploadTransaction.ToLocationCode = bom == null || bom.Lgort.Trim().IsNullOrEmpty() ? record.ItemLabel.Lgort : bom.Lgort;

            uploadTransaction.UploadCount = 0;

            uploadTransaction.PersistenceStatus = PersistenceStatus.New;
            return uploadTransaction;
        }

        #endregion

        #region 委外出库同步工厂数据上传事务

        public virtual void OutsourcingoutsSync()
        {
            //var q = Query<UploadTransaction>();
            //q.Where(p => p.OrderType == OrderType.Outsourcingouts && p.TransactionType == TransactionType.Outsourcingouts);
            //q.Where(p => p.BillLineId != null);
            //q.Where(p => p.State == ProcessState.Unprocessed || p.State == ProcessState.Retry).OrderByDescending(p => p.CreateDate);
            ////贪婪加载
            //EagerLoadOptions elo = new EagerLoadOptions();
            //elo.LoadWith(UploadTransaction.TransactionProperty);
            //elo.LoadWithViewProperty();

            //var list = q.ToList(new PagingInfo(1, MAX_BATCH_QUANTITY), elo);

            //var BillLineIds = list.Select(p => p.BillLineId.Value).Distinct().ToList();
            //EntityList<ProcessingOutbound> outbounds = RT.Service.Resolve<OutsourcingController>().GetProcessingOutboundsByIds(BillLineIds);
            EntityList<ProcessingOutbound> outbounds = Query<ProcessingOutbound>().Where(p => (p.IsUpload == null || p.IsUpload == false) && (p.ReLoadCount == null || p.ReLoadCount <= 5)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            foreach (var g in outbounds.GroupBy(p => p.OutsourcingRequestId))
            {
                var request = RF.GetById<OutsourcingRequest>(g.FirstOrDefault().OutsourcingRequestId, new EagerLoadOptions().LoadWithViewProperty());
                //获取要同步到的工厂(同一委外单，只有去同一个工厂)
                //var tran = list.FirstOrDefault(p => p.BillLineId == g.FirstOrDefault().Id);

                request.ProcessingOutsourcingOutboundList.Clear();
                request.ProcessingOutsourcingOutboundList.AddRange(g.AsEntityList());
                //要清除否则到时候数据也会被更新
                request.ProcessingOutsourcingInStockList.Clear();
                request.OutsourcingReportLogList.Clear();
                //调用接口重传
                var response = RT.Service.Resolve<OutsourcingApiController>().SyncOtherFactory(request, 1, request.OutFactory);
            }
        }

        #endregion

        #region 委外入库同步工厂数据事务上传

        public virtual void OutsourcingInsSync()
        {
            //var q = Query<UploadTransaction>();
            //q.Where(p => p.OrderType == OrderType.OutsourcingIns && p.TransactionType == TransactionType.OutsourcingIns);
            //q.Where(p => p.BillLineId != null);
            //q.Where(p => p.State == ProcessState.Unprocessed || p.State == ProcessState.Retry).OrderByDescending(p => p.CreateDate);
            ////贪婪加载
            //EagerLoadOptions elo = new EagerLoadOptions();
            //elo.LoadWith(UploadTransaction.TransactionProperty);
            //elo.LoadWithViewProperty();
            //var list = q.ToList(new PagingInfo(1, MAX_BATCH_QUANTITY), elo);

            ////收货
            //var BillLineIds = list.Select(p => p.BillLineId.Value).Distinct().ToList();
            //EntityList<ProcessingInStock> inStocks = RT.Service.Resolve<OutsourcingController>().GetProcessingInStocksById(BillLineIds);
            EntityList<ProcessingInStock> inStocks = Query<ProcessingInStock>().Where(p => (p.IsUpload == null || p.IsUpload == false) && (p.ReLoadCount == null || p.ReLoadCount <= 5)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            foreach (var g in inStocks.GroupBy(p => p.OutsourcingRequestId))
            {
                var request = RF.GetById<OutsourcingRequest>(g.FirstOrDefault().OutsourcingRequestId, new EagerLoadOptions().LoadWithViewProperty());
                request.ProcessingOutsourcingInStockList.Clear();
                request.ProcessingOutsourcingInStockList.AddRange(g.AsEntityList());
                //要清除否则到时候数据也会被更新
                request.OutsourcingReportLogList.Clear();
                request.ProcessingOutsourcingOutboundList.Clear();
                //获取要同步到的工厂(同一委外单，只有去同一个工厂)
                //var tran = list.FirstOrDefault(p => p.BillLineId == g.FirstOrDefault().Id);
                //调用接口重传
                var response = RT.Service.Resolve<OutsourcingApiController>().SyncOtherFactory(request, 2, request.OutFactory);
            }
        }

        #endregion

        #region 委外报工同步工厂数据事务上传

        public virtual void OutsourcingReportSync()
        {
            //var q = Query<UploadTransaction>();
            //q.Where(p => p.OrderType == OrderType.OutsourcingReport && p.TransactionType == TransactionType.OutsourcingReport);
            //q.Where(p => p.BillLineId != null);
            //q.Where(p => p.State == ProcessState.Unprocessed || p.State == ProcessState.Retry).OrderByDescending(p => p.CreateDate);
            ////贪婪加载
            //EagerLoadOptions elo = new EagerLoadOptions();
            //elo.LoadWith(UploadTransaction.TransactionProperty);
            //elo.LoadWithViewProperty();
            //var list = q.ToList(new PagingInfo(1, MAX_BATCH_QUANTITY), elo);

            //var BillLineIds = list.Select(p => p.BillLineId.Value).Distinct().ToList();
            //EntityList<OutsourcingReportLog> inStocks = RT.Service.Resolve<OutsourcingController>().GetOutsourcingReportLogsByIds(BillLineIds);
            EntityList<OutsourcingReportLog> inStocks = Query<OutsourcingReportLog>().Where(p => (p.IsUpload == null || p.IsUpload == false) && (p.ReLoadCount == null || p.ReLoadCount <= 5)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            foreach (var g in inStocks.GroupBy(p => p.OutsourcingRequestId))
            {
                var request = RF.GetById<OutsourcingRequest>(g.FirstOrDefault().OutsourcingRequestId, new EagerLoadOptions().LoadWithViewProperty());
                request.OutsourcingReportLogList.Clear();
                request.OutsourcingReportLogList.AddRange(g.AsEntityList());
                //要清除否则到时候数据也会被更新
                request.ProcessingOutsourcingOutboundList.Clear();
                request.ProcessingOutsourcingInStockList.Clear();
                //获取要同步到的工厂(同一委外单，只有去同一个工厂)
                //var tran = list.FirstOrDefault(p => p.BillLineId == g.FirstOrDefault().Id);
                //调用接口重传
                var response = RT.Service.Resolve<OutsourcingApiController>().SyncOtherFactory(request, 3, request.InitiatorFactory);
            }
        }

        #endregion

        #region 委外可疑品标签同步

        public virtual void SyncSupWipBatch()
        {
            //var q = Query<UploadTransaction>();
            //q.Where(p => p.OrderType == OrderType.OutsourcingSupWipBatch && p.TransactionType == TransactionType.OutsourcingSupWipBatch);
            //q.Where(p => p.State == ProcessState.Unprocessed || p.State == ProcessState.Retry).OrderByDescending(p => p.CreateDate);
            ////贪婪加载
            //EagerLoadOptions elo = new EagerLoadOptions();
            //elo.LoadWith(UploadTransaction.TransactionProperty);
            //elo.LoadWithViewProperty();
            //var list = q.ToList(new PagingInfo(1, MAX_BATCH_QUANTITY), elo);

            //var ids = list.Select(p => p.BillId.Value).Distinct().ToList();
            //var wipBatchs = RT.Service.Resolve<WipBatchController>().GetWipBatches(ids);
            var wipBatchs = Query<WipBatch>().Where(p => (p.IsUpload == null || p.IsUpload == false) && (p.ReLoadCount == null || p.ReLoadCount <= 5)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            var dispatchTaskIds = wipBatchs.Where(p => p.DispatchTaskId != null).Select(p => p.DispatchTaskId.Value).ToList(); 

            var tasks = RT.Service.Resolve<DispatchController>().GetDispatchTasks(dispatchTaskIds);
            foreach (var task in tasks)
            {
                var wips = wipBatchs.Where(p => p.DispatchTaskId == task.Id).ToList();
                OutsourcingRequest request = Query<OutsourcingRequest>().Where(p => p.WorkOrderId == task.WorkOrderId && p.BeginProcess.Process.Code == task.Process.Code).FirstOrDefault();
                //调用接口开始上传
                if (request != null)
                {
                    //可疑品接口
                    RT.Service.Resolve<SuspectProductLabelController>().SyncOtherFactoryInterface(wips, task, request);

                    ////调用接口传报工记录
                    //EntityList<OutsourcingReportLog> logs = new EntityList<OutsourcingReportLog>();
                    //foreach (var newWipBatch in wips)
                    //{
                    //    var processingType = RT.Service.Resolve<OutsourcingApiController>().GetProcessingTypeByWipBatch(newWipBatch);
                    //    OutsourcingReportLog log = request.OutsourcingReportLogList.FirstOrDefault(p => p.SN == newWipBatch.BatchNo);
                    //    logs.Add(log);
                    //}
                    //var req = new OutsourcingRequest();
                    //req.Clone(request, new CloneOptions(CloneActions.NormalProperties));
                    //req.OutsourcingReportLogList.Clear();
                    //req.ProcessingOutsourcingInStockList.Clear();
                    //req.ProcessingOutsourcingOutboundList.Clear();
                    //req.OutsourcingReportLogList.AddRange(logs);
                    ////调用接口，同步给其他工厂、并且更新事务上传
                    //var response = RT.Service.Resolve<OutsourcingApiController>().SyncOtherFactory(req, 3);
                }
            }
        }

        #endregion

        #region 发货确认事务上传

        public virtual void OutboundConfirmTransaction()
        {
            var list = Query<OutboundConfirmDetail>().Where(p => p.IsUpload == false || p.IsUpload == null).ToList(new PagingInfo(1, 2000), new EagerLoadOptions().LoadWithViewProperty());

            foreach(var detail in list)
            {
                //创建事务上传
                OutboundConfirmTransactionData transactionData = new OutboundConfirmTransactionData();
                transactionData.BillId = detail.Id;
                transactionData.Quantity = detail.Qty;
                transactionData.TransactionDate = RF.Find<OutboundConfirmDetail>().GetDbTime();
                transactionData.Zuid = detail.Zuid;
                RT.Service.Resolve<UploadLogControllercs>().OutboundConfirmTransaction(new List<OutboundConfirmTransactionData>() { transactionData });
                //更新状态
                detail.IsUpload = true;
                RF.Save(detail);
            }
        }

        #endregion

        #region 副记录上传事务

        /// <summary>
        /// 扣料记录到事务上传
        /// </summary>
        public virtual void UploadOutputProductToInf()
        {
            var records = GetOutputProductRecords();
            if (records.Count < 1)
                return;

            var invOrg = DB.Query<Rbac.InvOrgs.InvOrg>().Where(a => a.Code == RT.InvOrg).FirstOrDefault();
            using (var trans = DB.TransactionScope(InterfaceEntityDataProvider.ConnectionStringName))
            {                
                EntityList<UploadTransaction> uploadTransactions = new EntityList<UploadTransaction>();
                var woIds = records.Select(p => p.StorageWorkOrderId).Distinct().ToList();
                var bomList = RT.Service.Resolve<SIE.MES.WorkOrders.WorkOrderController>().GetWorkOrderBomList(woIds);
                foreach (var deductionRecord in records)
                {
                    uploadTransactions.Add(CreateUploadTransaction(deductionRecord, bomList, invOrg));

                    deductionRecord.UploadFlag = true;
                    deductionRecord.PersistenceStatus = PersistenceStatus.Modified;
                }
                RF.Save(uploadTransactions);
                RF.Save(records);
                trans.Complete();
            }
        }

        public virtual EntityList<OutputProductRecord> GetOutputProductRecords()
        {
            var list = Query<OutputProductRecord>()
              .Where(p => (p.UploadFlag == null || p.UploadFlag == false)).OrderBy(p => p.CreateDate).ToList(new PagingInfo(1, MAX_BATCH_QUANTITY), new EagerLoadOptions().LoadWithViewProperty());

            return list;
        }

        /// <summary>
        /// 创建事务上传
        /// </summary>
        /// <returns></returns>
        public virtual UploadTransaction CreateUploadTransaction(OutputProductRecord record, EntityList<MES.WorkOrders.WorkOrderBom> bomList, Rbac.InvOrgs.InvOrg invOrg)
        {
            UploadTransaction uploadTransaction = new UploadTransaction();

            uploadTransaction.OrderType = OrderType.OutputProduct;
            uploadTransaction.TransactionId = null;
            uploadTransaction.TransactionType = TransactionType.OutputProduct;
            uploadTransaction.State = ProcessState.Unprocessed;
            uploadTransaction.BillId = record.Id;

            uploadTransaction.TransactionDate = record.CreateDate;
            uploadTransaction.WoId = record.StorageWorkOrderId;
            uploadTransaction.Remark = string.Empty;
            uploadTransaction.FromOnhandState = null;
            //原因
            uploadTransaction.ReasonName = string.Empty;


            uploadTransaction.ItemId = record.ItemId;
            uploadTransaction.ItemCode = record.ItemCode;
            uploadTransaction.ItemName = record.ItemName;

            uploadTransaction.OrdKey = "OutputProductRecord_" + record.Id.ToString();/*(uploadTransaction.TransactionDate.ToString("yyyyMMdd") + "◇" + reportRecord.WorkOrderNo + "◇" + reportRecord.Id + "◇" + RT.InvOrg).Replace('-', '^');*/

            //工单
            uploadTransaction.WoNo = record.WorkOrderNo;
            uploadTransaction.Quantity = record.Qty;
            //uploadTransaction.WorkCenter = "";
            uploadTransaction.WERKS = invOrg.ExternalId;

            //uploadTransaction.ReportRecordId = record.Id;
            var bom = bomList.FirstOrDefault(p => p.WorkOrderId == record.StorageWorkOrderId && p.ItemId == record.ItemId);
            uploadTransaction.ToLocationCode = bom?.Lgort;

            uploadTransaction.UploadCount = 0;

            uploadTransaction.PersistenceStatus = PersistenceStatus.New;
            return uploadTransaction;
        }

        #endregion

        #region GUID传输给集团

        /// <summary>
        /// 
        /// </summary>
        /// <param name="relastions"></param>
        public virtual void GuidToGroup(List<GuidFactoryRelastion> relastions)
        {
            var Url = RT.Config.Get("Group.URL");
            if (Url.IsNullOrEmpty())
                return;

                try
                {
                    var smomParam = new List<SmomParam>()
                    {
                    new SmomParam { Value =relastions }
                                 }.ToArray();
                var response = SmomControlHepler.SmomPost<object>("SmomBaseController", "GuidFactory", Url, smomParam);
                }
                catch (Exception ex)
                {
                    
                }

        }

        #endregion

        #region 返工信息上传事务

        /// <summary>
        /// 返工信息上传事务
        /// </summary>
        public virtual void UploadReworkInfoRecordToInf()
        {
            //获取返工信息数据
            var records = GetReworkInfoRecords();

            if (records.Count < 1)
                return;

            using (var trans = DB.TransactionScope(InterfaceEntityDataProvider.ConnectionStringName))
            {
                EntityList<UploadTransaction> uploadTransactions = new EntityList<UploadTransaction>();
                var dateTime = RF.Find<UploadTransaction>().GetDbTime();
                foreach (var record in records)
                {
                    uploadTransactions.Add(CreateReworkInfoRecordTransaction(record, dateTime));

                    record.IsUpload = true;
                    record.PersistenceStatus = PersistenceStatus.Modified;
                    RF.Save(record);
                }
                RF.Save(uploadTransactions);
                trans.Complete();
            }

        }

        /// <summary>
        /// 获取返工信息
        /// </summary>
        private EntityList<ReworkInfoRecord> GetReworkInfoRecords()
        {
            var list = Query<ReworkInfoRecord>().Where(p => p.IsUpload == null || p.IsUpload == false).ToList(new PagingInfo(1, MAX_BATCH_QUANTITY), new EagerLoadOptions().LoadWithViewProperty());
            return list;
        }

        /// <summary>
        /// 创建事务上传
        /// </summary>
        /// <returns></returns>
        public virtual UploadTransaction CreateReworkInfoRecordTransaction(ReworkInfoRecord record, DateTime dateTime)
        {
            UploadTransaction uploadTransaction = new UploadTransaction();

            uploadTransaction.OrderType = OrderType.ReworkInfoRecord;
            uploadTransaction.TransactionId = null;
            uploadTransaction.TransactionType = TransactionType.ReworkInfoRecord;
            uploadTransaction.State = ProcessState.Unprocessed;
            uploadTransaction.BillId = record.Id;
            uploadTransaction.SourceId = record.Id.ToString();

            uploadTransaction.TransactionDate = dateTime;
            uploadTransaction.Remark = string.Empty;
            uploadTransaction.FromOnhandState = null;
            uploadTransaction.ReasonName = string.Empty;


            uploadTransaction.ItemId = record.ItemId;
            uploadTransaction.ItemCode = record.ItemCode;
            uploadTransaction.ItemName = record.ItemName;

            uploadTransaction.OrdKey = "ReworkInfoRecord_" + record.Id.ToString();
            uploadTransaction.Quantity = record.Qty;
            uploadTransaction.WERKS = record.Factory;
            uploadTransaction.Zuid = record.UniqueCode;
            uploadTransaction.Department = record.Department;
            uploadTransaction.Version = record.Version;
            uploadTransaction.UploadCount = 0;

            uploadTransaction.PersistenceStatus = PersistenceStatus.New;
            return uploadTransaction;
        }

        #endregion
    }
}

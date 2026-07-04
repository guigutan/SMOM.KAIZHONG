using SIE.Barcodes;
using SIE.Common.ImportHelper;
using SIE.Core.Items;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EventMessages.MES.Barcodes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace SIE.MES.WorkOrders.ImportWorkOrders
{
    /// <summary>
    /// 导入工单 处理逻辑类
    /// </summary>
    [Services.Service(FallbackType = typeof(ImportBarcodeHandle), ServiceLifeStyle = Services.ServiceLifeStyle.Transient)]
    public class ImportBarcodeHandle : IDisposable, IBusinessImport
    {
        /// <summary> 
        /// 导入列名列表
        /// </summary>
        private List<string> columnNameList = new List<string> { "工单编号", "条码" };

        #region 私有属性
        /// <summary>
        /// 工单字典
        /// </summary>
        private Dictionary<string, WorkOrder> workOrderNoDic = new Dictionary<string, WorkOrder>();

        /// <summary>
        /// 物料字典
        /// </summary>
        private Dictionary<double, string> itemDic = new Dictionary<double, string>();

        /// <summary>
        /// 工单物料追溯方式字典
        /// </summary>
        private Dictionary<double, RetrospectType?> retrospectTypeDic = new Dictionary<double, RetrospectType?>();
        #endregion

        /// <summary>
        /// 导入模板的列头名
        /// </summary>
        public List<string> ColumnNameList
        {
            get
            {
                return columnNameList;
            }

            set
            {
                columnNameList = value;
            }
        }

        /// <summary>
        /// 列的标准验证 (列名 列对应验证 )
        /// </summary>
        public Dictionary<string, ValidColumn> ColumnValidList { get; set; }

        /// <summary>
        /// 创建列
        /// </summary>
        /// <returns>IBusinessImport</returns>
        public IBusinessImport CreaetColumnValid()
        {
            this.ColumnValidList = new Dictionary<string, ValidColumn>
            {
                { "工单编号", new ValidColumn(ImportDataType._String, true, ValidWorkOrder) },
                { "条码", new ValidColumn(ImportDataType._Custom, true, ValidBarcode) },
            };

            return this;
        }

        #region 基础验证
        /// <summary>
        /// 验证物料
        /// </summary>
        /// <param name="obj">物料编码</param>
        /// <param name="messageTip">错误信息</param>
        /// <param name="dr">验证数据的当前行对象</param>
        /// <returns>返回是否验证通过</returns>
        private bool ValidWorkOrder(object obj, out string messageTip, DataRow dr)
        {
            return ValidBarcodeDataHelper.ValidWorkOrder(ref workOrderNoDic, ref retrospectTypeDic, ref itemDic, obj.ToString(), out messageTip);
        }

        /// <summary>
        /// 验证客户名称
        /// </summary>
        /// <param name="obj">客户名称</param>
        /// <param name="messageTip">错误信息</param>
        /// <param name="dr">验证数据的当前行对象</param>
        /// <returns>返回是否验证通过</returns>
        private bool ValidBarcode(object obj, out string messageTip, DataRow dr)
        {
            return ValidBarcodeDataHelper.ValidBarcode(obj.ToString(), out messageTip);
        }
        #endregion

        /// <summary>
        /// 释放数据
        /// </summary>
        public void Dispose()
        {
            workOrderNoDic.Clear();
            retrospectTypeDic.Clear();
        }

        /// <summary>
        /// 处理业务数据
        /// </summary>
        /// <param name="drs">需要处理的数据集</param>
        public void ProcessBusinessDataHandle(DataRow[] drs)
        {
            Dictionary<string, List<string>> importDatas = new Dictionary<string, List<string>>();
            Dictionary<string, int> printQtys = new Dictionary<string, int>();
            ValidateData(drs, importDatas, printQtys);

            using (var tran = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                EntityList<BarcodeRange> rangeList = new EntityList<BarcodeRange>();
                EntityList<Barcode> barcodeList = new EntityList<Barcode>();
                EntityList<WorkOrder> workOrderList = new EntityList<WorkOrder>();
                var printBarcodeInfoList = new List<PrintBarcodeInfo>();

                foreach (var importData in importDatas)
                {
                    WorkOrder workOrder = null;
                    workOrderNoDic.TryGetValue(importData.Key, out workOrder);
                    var barcodeSns = importData.Value;
                    if (barcodeSns.Count <= 0)
                        continue;
                    barcodeSns.Sort();
                    var barcodeSnList = barcodeSns.Distinct();
                    int printSumQty = barcodeSnList.Count();
                    string beginSn = barcodeSnList.FirstOrDefault();
                    string endSn = barcodeSnList.LastOrDefault();

                    var range = new BarcodeRange()
                    {
                        PrintQty = printSumQty,
                        StartSn = beginSn,
                        EndSn = endSn,
                        Rule = null,
                        State = ReceiveState.NoReceive,
                        WorkOrder = workOrder,
                        Template = null
                    };
                    rangeList.Add(range);

                    foreach (var sn in barcodeSnList)
                    {
                        var barcode = new Barcode()
                        {
                            Sn = sn,
                            IsScraped = false,
                            IsPending = false,
                            Qty = 1,
                            BoxesQty = 1,
                            IsMantissa = false,
                            WorkOrderId = workOrder.Id,
                            PrintedState = BarcodeState.Notprint,
                            Range = range
                        };
                        barcodeList.Add(barcode);
                    }

                    workOrder.PrintedQty += printSumQty;
                    workOrderList.Add(workOrder);

                    var printBarcodeInfo = new PrintBarcodeInfo();
                    printBarcodeInfo.MsgType = "3";
                    printBarcodeInfo.WorkOrderNo = importData.Key;
                    printBarcodeInfo.BarcodeList.AddRange(barcodeList);
                    printBarcodeInfoList.Add(printBarcodeInfo);
                }

                RF.Save(rangeList);
                RF.Save(barcodeList);
                RF.Save(workOrderList);

                //推送导入条码消息到边端
                printBarcodeInfoList.ForEach(p =>
                {
                    RT.EventBus.Publish<PrintBarcodeInfo>(p);
                });
                tran.Complete();
            }
        }

        private void ValidateData(DataRow[] drs, Dictionary<string, List<string>> importDatas, Dictionary<string, int> printQtys)
        {
            //循环检验每一行数据
            foreach (var mainDataItem in drs)
            {
                try
                {
                    var workOrderNo = mainDataItem.Field<string>(ColIndex("工单编号"));
                    var barcodeSn = mainDataItem.Field<string>(ColIndex("条码"));

                    WorkOrder wo = null;
                    if (workOrderNoDic.TryGetValue(workOrderNo, out wo))
                    {
                        if (!printQtys.ContainsKey(workOrderNo))
                        {
                            printQtys.Add(workOrderNo, 0);
                        }

                        if (!importDatas.ContainsKey(workOrderNo))
                        {
                            importDatas.Add(workOrderNo, new List<string>());
                        }

                        RetrospectType? retrospectType = null;
                        if (retrospectTypeDic.TryGetValue(wo.ProductId, out retrospectType))
                        {
                            string itemCode = string.Empty;
                            itemDic.TryGetValue(wo.ProductId, out itemCode);
                            if (retrospectType == RetrospectType.Batch)
                            {
                                throw new ValidationException("工单[{0}]的产品[{1}]是批次类型，不能导入条码！".L10nFormat(workOrderNo, itemCode));
                            }
                        }

                        if (importDatas.ContainsKey(workOrderNo))
                        {
                            var barcodeSns = importDatas[workOrderNo];
                            if (barcodeSns.Contains(barcodeSn))
                            {
                                throw new ValidationException("导入数据中已存在条码编号{0}！".L10nFormat(barcodeSn));
                            }
                        }

                        if (printQtys.ContainsKey(workOrderNo))
                        {
                            if (printQtys[workOrderNo] + 1 + wo.PrintedQty > wo.PlanQty)
                            {
                                throw new ValidationException("已超过工单[{0}]计划数量[{1}],请确认!".L10nFormat(wo.No, wo.PlanQty));
                            }

                            printQtys[workOrderNo] = printQtys[workOrderNo] + 1;
                        }

                        importDatas[workOrderNo].Add(barcodeSn);
                    }
                }
                catch (Exception exc)
                {
                    string strMsg = AppRuntime.Location.ConnectDataDirectly ? exc.Message : exc.InnerException.Message;
                    mainDataItem[ImportDataHandle.MessageColumnName] = mainDataItem[ImportDataHandle.MessageColumnName] + strMsg;
                }
            }
        }

        /// <summary>
        /// 获取指定列名的索引
        /// </summary>
        /// <param name="columnName">指定列名</param>
        /// <returns>返回对应索引</returns>
        private int ColIndex(string columnName)
        {
            return columnNameList.IndexOf(columnName);
        }
    }
}
using SIE.Common.ImportHelper;
using SIE.Domain;
using SIE.Warehouses;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace SIE.LES.LesStockCounts.ImportHandles
{
    /// <summary>
    /// 导入实盘数据
    /// </summary>
    [Services.Service(FallbackType = typeof(ImportSCDetailHandle), ServiceLifeStyle = Services.ServiceLifeStyle.Transient)]
    public class ImportSCDetailHandle : IDisposable, IBusinessImport
    {
        /// <summary>
        /// 列名
        /// </summary>
        public List<string> ColumnNameList { get; set; } = new List<string>
        {
            "行号", "数量"
        };

        /// <summary>
        /// 盘点单
        /// </summary>
        private LesStockCount bill;

        /// <summary>
        /// 行号-单据明细
        /// </summary>
        private Dictionary<string, LesStockCountDetail> dicBillDtl;

        /// <summary>
        /// 列的标准验证 (列名 列对应验证 )
        /// </summary>
        public Dictionary<string, ValidColumn> ColumnValidList { get; set; }

        /// <summary>
        /// 创建列的验证
        /// </summary>
        /// <returns>返回当前对象</returns>
        public IBusinessImport CreaetColumnValid()
        {
            this.ColumnValidList = new Dictionary<string, ValidColumn>();
            this.ColumnValidList.Add(ColumnNameList[0], new ValidColumn(ImportDataType._Custom, true, VaildLineNo));    // 行号
            this.ColumnValidList.Add(ColumnNameList[1], new ValidColumn(ImportDataType._Custom, true, VaildQty));            // 数量

            return this;
        }

        /// <summary>
        /// 释放数据
        /// </summary>
        public void Dispose()
        {
            if (dicBillDtl != null)
            {
                dicBillDtl.Clear();
                dicBillDtl = null;
            }
        }

        /// <summary>
        /// 业务数据处理
        /// </summary>
        /// <param name="drs">数据集合</param>
        public void ProcessBusinessDataHandle(DataRow[] drs)
        {
            if (drs.Any(p => !string.IsNullOrEmpty(p[ImportDataHandle.MessageColumnName].ToString()))) return;
            var mainDataList = from g in drs
                               select new
                               {
                                   LineNo = g.Field<string>(ColIndex("行号")),
                                   Qty = g.Field<string>(ColIndex("数量")),
                                   DetailInfo = g
                               };
            foreach (var mainDataItem in mainDataList)
            {
                LesStockCountDetail stockCountDetail = bill.LesStockCountDetailList.FirstOrDefault(p => p.LineNo == mainDataItem.LineNo);
                stockCountDetail.ActualCountQty = Convert.ToDecimal(mainDataItem.Qty);
                stockCountDetail.DiffCountQty = stockCountDetail.ActualCountQty - stockCountDetail.Qty;
                if(stockCountDetail.DiffCountQty==0)
                {
                    stockCountDetail.LesStockCountDetailResult = LesStockCountDetailResult.Normal;
                }else
                {
                    stockCountDetail.LesStockCountDetailResult = LesStockCountDetailResult.Abnormal;
                }
                stockCountDetail.CountById = RT.IdentityId;
                stockCountDetail.CountDate = DateTime.Now;
            }
            RT.Service.Resolve<LesStockCountController>().SaveLesStockCount(bill);
        }

        #region 验证数据
        /// <summary>
        /// 验证行号
        /// </summary>
        /// <param name="obj">行号</param>
        /// <param name="messageTip">信息提示</param>
        /// <param name="dr">当前行数据</param>
        /// <returns>是否验证通过</returns>
        private bool VaildLineNo(object obj, out string messageTip, DataRow dr)
        {
            messageTip = string.Empty;
            if (dicBillDtl == null)
            {
                dicBillDtl = new Dictionary<string, LesStockCountDetail>();
            }

            double billId = 0;
            string stockCountId = dr[ImportDataHandle.ParentId].ToString().Trim();
            double.TryParse(stockCountId, out billId);
            if (bill == null)
            {
                bill = RF.GetById<LesStockCount>(billId);
            }

            string lineNo = obj.ToString();
            if (!dicBillDtl.ContainsKey(lineNo))
            {
                LesStockCountDetail scDetail = bill.LesStockCountDetailList.FirstOrDefault(p => p.LineNo == lineNo);
                if (scDetail == null)
                {
                    messageTip = "不存在于系统;".L10N();
                    AppendErrorMsg(dr, ImportDataHandle.MessageColumnName, messageTip);
                }
                else if (scDetail.State == LesCountState.Close)
                {
                    messageTip = "处于关闭状态;".L10N();
                    AppendErrorMsg(dr, ImportDataHandle.MessageColumnName, messageTip);
                }

                dicBillDtl.Add(lineNo, scDetail);
            }
            else
            {
                messageTip = "行号存在重复;".L10N();
                AppendErrorMsg(dr, ImportDataHandle.MessageColumnName, messageTip);
            }

            return true;
        }

        /// <summary>
        /// 验证數量
        /// </summary>
        /// <param name="obj">行号</param>
        /// <param name="messageTip">信息提示</param>
        /// <param name="dr">当前行数据</param>
        /// <returns>是否验证通过</returns>
        private bool VaildQty(object obj, out string messageTip, DataRow dr)
        {
            bool isValid = BaseTypeValid.ValidFloat(obj, out messageTip, dr);
            if (isValid)
            {
                decimal qty = Decimal.Parse(obj.ToString());
                if (qty < 0)
                {
                    messageTip = "必须大于等于0".L10N();
                    AppendErrorMsg(dr, ImportDataHandle.MessageColumnName, messageTip);
                }
            }

            return isValid;
        }
        #endregion

        /// <summary>
        /// 获取指定列名的索引
        /// </summary>
        /// <param name="columnName">指定列名</param>
        /// <returns>返回对应索引</returns>
        private int ColIndex(string columnName)
        {
            return ColumnNameList.IndexOf(columnName);
        }

        /// <summary>
        /// 给保存错误的数据行记录错误数据信息
        /// </summary>
        /// <param name="row">行</param>
        /// <param name="columnName">列名</param>
        /// <param name="errorMsg">错误信息</param>
        private void AppendErrorMsg(DataRow row, string columnName, string errorMsg)
        {
            row[columnName] += errorMsg;
        }
    }
}

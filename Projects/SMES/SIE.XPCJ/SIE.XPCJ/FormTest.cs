using SIE.XPCJ.Common.ApiCall;
using SIE.XPCJ.Common.Print;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SIE.XPCJ
{
    public partial class FormTest : Common.Forms.FormBase
    {
        public FormTest()
        {
            InitializeComponent();
        }

        #region SubmitReelIDReturnData
        /// <summary>
        /// 收货数据
        /// </summary>
        public class ElecReceiptData
        {
            /// <summary>
            /// 供应商数据
            /// </summary>
            public class ElecSupplierData
            {
                /// <summary>
                /// 供应商id
                /// </summary>
                public double supplierId { get; set; }

                /// <summary>
                /// 供应商编码
                /// </summary>
                public string supplierCode { get; set; }

                /// <summary>
                /// 供应商名称
                /// </summary>
                public string supplierName { get; set; }
            }

            /// <summary>
            /// 采购订单数据
            /// </summary>
            public class ElecOrderData
            {
                /// <summary>
                /// 采购订单id
                /// </summary>
                public double OrderId { get; set; }

                /// <summary>
                /// 采购订单明细id
                /// </summary>
                public double OrderDetailId { get; set; }

                /// <summary>
                /// 行号
                /// </summary>
                public string LineNo { get; set; }

                /// <summary>
                /// 采购订单号
                /// </summary>
                public string OrderNo { get; set; }

                /// <summary>
                /// 物料编码
                /// </summary>
                public string ItemCode { get; set; }

                /// <summary>
                /// 物料名称
                /// </summary>
                public string ItemName { get; set; }

                /// <summary>
                /// 物料规格
                /// </summary>
                public string Specifications { get; set; }

                /// <summary>
                /// 物料扩展属性
                /// </summary>
                public string ExtensionProperty { get; set; }

                /// <summary>
                /// 未建单数
                /// </summary>
                public decimal UnFinishQty { get; set; }

                /// <summary>
                /// 交货日期
                /// </summary>
                public string DeliverDate { get; set; }

                /// <summary>
                /// 交货日期
                /// </summary>
                public DateTime DelDate { get; set; }
            }
        }

        /// <summary>
        /// 返回ReelID数据
        /// </summary>
        public class ResultReelIDData
        {
            /// <summary>
            /// Id
            /// </summary>
            public double LabelId { get; set; }

            /// <summary>
            /// ReelID
            /// </summary>
            public string ReelID { get; set; }

            /// <summary>
            /// 物料Id
            /// </summary>
            public double ItemId { get; set; }

            /// <summary>
            /// 物料编码
            /// </summary>
            public string ItemCode { get; set; }

            /// <summary>
            /// 物料名称
            /// </summary>
            public string ItemName { get; set; }

            /// <summary>
            /// 规格
            /// </summary>
            public string SpecificationModel { get; set; }

            /// <summary>
            /// 物料批次号
            /// </summary>
            public string LotCode { get; set; }

            /// <summary>
            /// 数量
            /// </summary>
            public decimal Qty { get; set; }

            /// <summary>
            /// Rid个数
            /// </summary>
            public int RidQty { get; set; }

            /// <summary>
            /// 工单
            /// </summary>
            public string WorkOrder { get; set; }

            /// <summary>
            /// 如果是合并工单，此为子工单
            /// </summary>
            public string SourceWoNo { get; set; }

            /// <summary>
            /// 产线
            /// </summary>
            public string ProdLine { get; set; }

            /// <summary>
            /// 物料扩展属性
            /// </summary>
            public string ItemExtProp { get; set; }

            /// <summary>
            /// 物料扩展属性名称
            /// </summary>
            public string ItemExtPropName { get; set; }

            /// <summary>
            /// 更新数量
            /// </summary>
            public decimal UpdateQty { get; set; }

            /// <summary>
            /// 货主号
            /// </summary>
            public string StorerCode { get; set; }

            /// <summary>
            /// 项目号
            /// </summary>
            public string ProjectNo { get; set; }

            /// <summary>
            /// 任务号
            /// </summary>
            public string TaskNo { get; set; }

            /// <summary>
            /// 行号
            /// </summary>
            public int LineNo { get; set; }
        }

        /// <summary>
        /// 提交退货数据
        /// </summary>
        public class SubmitReturnData
        {
            /// <summary>
            /// 仓库Id
            /// </summary>
            public double WarehouseId { get; set; }

            /// <summary>
            /// 收货库位
            /// </summary>
            public string LocCode { get; set; }

            /// <summary>
            /// LPN
            /// </summary>
            public string Lpn { get; set; }

            /// <summary>
            /// ReelID退货数据集合
            /// </summary>
            public List<ResultReelIDData> ReturnReelIDDatas { get; set; } = new List<ResultReelIDData>();
        }
        #endregion

        #region GetPickingTaskData
        /// <summary>
        /// 标准拣货查询对象
        /// </summary>
        public class PickingTaskData : TaskData
        {
            /// <summary>
            /// 相关单号
            /// </summary>
            public string OrderNo { get; set; }

            /// <summary>
            /// 订单类型
            /// </summary>
            public string OrderType { get; set; }

            /// <summary>
            /// 供应商名称或者生产部门名称或者客户名称
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// 执行人
            /// </summary>
            public string Operator { get; set; }

            /// <summary>
            /// 是否序列号管理
            /// </summary>
            public bool IsSerialNumber { get; set; }

            /// <summary>
            /// 序列數量
            /// </summary>
            public decimal SerialQty { get; set; }

            /// <summary>
            /// 物料Id
            /// </summary>
            public new double ItemId { get; set; }

            /// <summary>
            /// 任务来源ID
            /// </summary>
            public double TaskSourceId { get; set; }

            /// <summary>
            /// 源库位与拣货库位是否一致
            /// </summary>
            public bool IsSameLocation { get; set; }

            /// <summary>
            /// 源Lpn与拣货LPN是否一致
            /// </summary>
            public bool IsSameLpn { get; set; }
        }

        /// <summary>
        /// 任务API交互信息
        /// </summary>
        public class TaskData
        {
            /// <summary>
            /// 任务ID
            /// </summary>
            public double TaskId { get; set; }

            /// <summary>
            /// 任务号
            /// </summary>
            public string TaskNo { get; set; }

            /// <summary>
            /// 任务释放时间
            /// </summary>
            public string ReleaseDate { get; set; }

            /// <summary>
            /// 物料编码
            /// </summary>
            public string ItemCode { get; set; }

            /// <summary>
            /// 物料名称
            /// </summary>
            public string ItemName { get; set; }

            /// <summary>
            /// 物料id
            /// </summary>
            public double? ItemId { get; set; }

            /// <summary>
            /// 规格型号
            /// </summary>
            public string SpecificationModel { get; set; }

            /// <summary>
            /// 单位
            /// </summary>
            public string UnitCode { get; set; }

            /// <summary>
            /// 包装
            /// </summary>
            public string PackRuleCode { get; set; }

            /// <summary>
            /// 包装规则名称
            /// </summary>
            public string PackRuleName { get; set; }

            /// <summary>
            /// 数量
            /// </summary>
            public decimal Qty { get; set; }

            /// <summary>
            /// 批次
            /// </summary>
            public string Lot { get; set; }

            /// <summary>
            /// 源库位编码
            /// </summary>
            public string FromLocCode { get; set; }

            /// <summary>
            /// 源LPN
            /// </summary>
            private string fromLpn;

            /// <summary>
            /// 源LPN
            /// </summary>
            public string FromLpn
            {
                get
                {
                    return fromLpn == string.Empty || fromLpn == null ? "*" : fromLpn;
                }
                set { fromLpn = value; }
            }

            /// <summary>
            /// 建议目标库位编码
            /// </summary>
            public string ToLocCode { get; set; }

            /// <summary>
            /// 建议目标LPN
            /// </summary>
            private string toLpn;

            /// <summary>
            /// 建议目标LPN
            /// </summary>
            public string ToLpn
            {
                get
                {
                    return toLpn == string.Empty || toLpn == null ? "*" : toLpn;
                }
                set { toLpn = value; }
            }

            /// <summary>
            /// 优先级
            /// </summary>
            public int Level { get; set; }

            /// <summary>
            /// 优先级
            /// </summary>
            public string LevelLabel { get; set; }

            /// <summary>
            /// 单据Id
            /// </summary>
            public double BillId { get; set; }

            /// <summary>
            /// 单据号
            /// </summary>
            public string BillNo { get; set; }

            /// <summary>
            /// 单据明细Id
            /// </summary>
            public double BillDtlId { get; set; }

            /// <summary>
            /// 任务状态
            /// </summary>
            public TaskState TaskState { get; set; }

            /// <summary>
            /// 物料扩展属性
            /// </summary>
            public string ItemExtProp { get; set; }

            /// <summary>
            /// 货主
            /// </summary>
            public string StorerCode { get; set; }

            /// <summary>
            /// 项目号
            /// </summary>
            public string ProjectNo { get; set; }

            /// <summary>
            /// 任务号
            /// </summary>
            public string Task_No { get; set; }

            /// <summary>
            /// 库存状态
            /// </summary>
            public OnhandState? OnhandState { get; set; }
        }

        /// <summary>
        /// 任务状态
        /// </summary>
        public enum TaskState
        {
            /// <summary>
            /// 创建
            /// </summary>       
            Create = 0,

            /// <summary>
            /// 释放
            /// </summary>       
            Release = 1,

            /// <summary>
            /// 指派
            /// </summary>     
            Appoint = 2,

            /// <summary>
            /// 冻结
            /// </summary>        
            Frozen = 3,

            /// <summary>
            /// 挂起
            /// </summary>

            HangUp = 4,

            /// <summary>
            /// 完工
            /// </summary>

            Finish = 5,

            /// <summary>
            /// 关闭
            /// </summary>

            Close = 6,

            /// <summary>
            /// 执行中
            /// </summary>

            Executing = 7,

            /// <summary>
            /// 异常
            /// </summary>

            Abnormal = 8,

            /// <summary>
            /// 自动完工
            /// </summary>

            AutoFinish = 9
        }

        /// <summary>
        /// 库存状态
        /// </summary>
        public enum OnhandState
        {
            /// <summary>
            /// 未质检
            /// </summary>       
            None = 1,

            /// <summary>
            /// 合格
            /// </summary>       
            Ok = 10,

            /// <summary>
            /// 不合格
            /// </summary>       
            Ng = 20,
        }
        #endregion

        //同步调用
        private void button1_Click(object sender, EventArgs e)
        {
            SubmitReturnData data = new SubmitReturnData();
            object[] parameters = new object[1];
            parameters[0] = data;
            this.ShowLoading();
            var result = ApiHelper.Post<string>("ElecReceiptController", "SubmitReelIDReturnData", parameters);
            this.CloseLoading();

            if (!result.Success)
            {
                MessageBox.Show(result.Message);
            }
        }

        //异步调用
        private void button2_Click(object sender, EventArgs e)
        {
            object[] parameters = new object[5];
            parameters[0] = 1;
            parameters[1] = "";
            parameters[2] = "";
            parameters[3] = 1;
            parameters[4] = 10;

            this.ShowLoading();
            ApiHelper.PostAsync<List<PickingTaskData>>(this, "ShipmentController", "GetPickingTaskData", GetPickingTaskDataCallback, parameters);
        }

        private void GetPickingTaskDataCallback<T>(ApiResult<T> result, string apiType, string method, string postData)
        {
            this.CloseLoading();
            if (!result.Success)
            {
                MessageBox.Show(result.Message);
                return;
            }
            List<PickingTaskData> list = result.Result as List<PickingTaskData>;
            MessageBox.Show($"获取到 {list.Count} 条数据");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var res = ApiHelper.Get("www.baidu.com");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.ShowLoading();
            ApiHelper.GetAsync(this, "www.baidu.com", GetCallBack);
        }

        private void GetCallBack(bool isOK, string result, string url)
        {
            this.CloseLoading();
            if (!isOK)
            {
                MessageBox.Show(result);
                return;
            }

        }

        private void button5_Click(object sender, EventArgs e)
        {
            XpPrinter.Instance.ShowBarTenderDesigner(@"C:\work\test.btw");
            return;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            XpPrinter.Instance.GetBarTenderPrintInfoAsync(this, GetBarTenderPrintInfoCallback, 104, LoginInfo.Instance.InvOrgId);
            this.ShowLoading();
        }

        private void GetBarTenderPrintInfoCallback<T>(ApiResult<T> result, string apiType, string method, string postData)
        {
            try
            {
                if (!result.Success)
                {
                    MessageBox.Show(result.Message);
                    return;
                }
                PrintInfo printInfo = result.Result as PrintInfo;
                XpPrinter.Instance.Print(printInfo, LoginInfo.Instance.InvOrgId);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.CloseLoading();
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            try
            {
                PrintInfo printInfo = XpPrinter.Instance.GetBarTenderPrintInfo(104, LoginInfo.Instance.InvOrgId);
                XpPrinter.Instance.Print(printInfo, LoginInfo.Instance.InvOrgId);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            XpPrinter.Instance.Print(@"D:\work\SIE.XPCJ\SMES\SIE.XPCJ\SIE.XPCJ.Starter\bin\Debug\XPCJ\Templates\垛条码-V10.btw");
        }

        private void button9_Click(object sender, EventArgs e)
        {
            //XpPrinter.Instance.Print(@"C:\work\BartenderPrint\BartenderPrint\bin\Debug\Templates\xx\longtest.siedev");

            XpPrinter.Instance.Print(@"D:\work\SIE.XPCJ\SMES\SIE.XPCJ\SIE.XPCJ.Starter\bin\Debug\XPCJ\Templates\ly外标签条码打印.siedev", string.Empty, true);
        }

        private void button10_Click(object sender, EventArgs e)
        {
            XpPrinter.Instance.GetSieDevPrintInfoAsync(this, GetSieDevPrintInfoCallback, 104, LoginInfo.Instance.InvOrgId);
            this.ShowLoading();
        }

        private void GetSieDevPrintInfoCallback<T>(ApiResult<T> result, string apiType, string method, string postData)
        {
            try
            {
                if (!result.Success)
                {
                    MessageBox.Show(result.Message);
                    return;
                }
                PrintInfo printInfo = result.Result as PrintInfo;
                XpPrinter.Instance.Print(printInfo, LoginInfo.Instance.InvOrgId);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.CloseLoading();
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {

            //XpPrinter.Instance.DownloadTempalte(2975, LoginInfo.Instance.InvOrgId);
            //return;

            try
            {
                PrintInfo printInfo = XpPrinter.Instance.GetSieDevPrintInfo(104, LoginInfo.Instance.InvOrgId);
                XpPrinter.Instance.Print(printInfo, LoginInfo.Instance.InvOrgId);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}

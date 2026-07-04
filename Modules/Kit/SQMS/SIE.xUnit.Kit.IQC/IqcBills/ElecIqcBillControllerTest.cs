using SIE.Common;
using SIE.Defects.InspectionItems;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Kit.IQC.IqcBills;
using SIE.IQC.IqcBills;
using SIE.QMS.Common;
using SIE.xUnit.Core.Common;
using SIE.xUnit.Elec.IQC.IqcBills.Fixtures;
using SIE.xUnit.PDCA.Report.Fixtures;
using SIE.xUnit.QMS.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace SIE.xUnit.Elec.IQC.IqcBills
{
    /// <summary>
    /// 电子来料检验控制器单元测试
    /// </summary>
    public class ElecIqcBillControllerTest : ElecIqcControllerTestBase, IClassFixture<ElecIqcBillFixture>, IClassFixture<ReportBaseFixture>
    {
        /// <summary>
        /// 来料检验固件
        /// </summary>
        private readonly ElecIqcBillFixture _iqcBillFixture;

        /// <summary>
        /// PDCA固件（初始化PDCA配置项环境）
        /// </summary>
        private readonly ReportBaseFixture _pdcaFixture;

        /// <summary>
        /// 来料检验控制器
        /// </summary>
        private readonly KitIqcBillController _iqcBillController;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="iqcBillFixture"></param>
        public ElecIqcBillControllerTest(ElecIqcBillFixture iqcBillFixture, ReportBaseFixture pdcaFixture)
        {
            _iqcBillFixture = iqcBillFixture;
            _pdcaFixture = pdcaFixture;
            _iqcBillController = RT.Service.Resolve<KitIqcBillController>();
        }

        /// <summary>
        /// 创建单据
        /// </summary>
        /// <returns></returns>
        private IqcBill CreateBill(bool IsSave = true)
        {
            IqcBill bill = new IqcBill();
            bill.GenerateId();
            bill.No = _iqcBillController.GetIqcBillNo();
            bill.InspectionModeId = _iqcBillFixture.FixPropInspectionMode.Id;
            bill.SupplierId = _iqcBillFixture.FixPropSupplier.Id;
            bill.ItemId = _iqcBillFixture.FixPropItem.Id;
            bill.BatchNo = $"BAT{_iqcBillFixture.FixPropItem.Id}";
            bill.AsnNo = $"Asn{_iqcBillFixture.FixPropItem.Id}";
            bill.Qty = 100;
            bill.InspectionCharacteristics = InspectionCharacteristics.NormalInspection;
            if (IsSave)
                RF.Save(bill);
            return bill;
        }

        #region 提交单据
        /// <summary>
        /// 提交单据-合格
        /// </summary>
        [Trait("提交单据", "合格")]
        [Fact]
        public void SumbitBillPassTest1()
        {
            var newbill = CreateBill();
            var bill = _iqcBillController.WritingReport(newbill.Id);
            bill.HasKeyInspItems = _iqcBillController.UpdateKeyInspItemInfo(bill);
            SetDetailsDataPass(bill);
            SetReelIds(bill);
            bill.InspectionResult = InspectionResult.Pass;

            _iqcBillController.Sumbit(bill);
            var resultBill = RF.GetById<IqcBill>(bill.Id);
            Assert.Equal(InspectionResult.Pass, resultBill.InspectionResult);
            Assert.Equal(InspectionStatus.Inspectioned, resultBill.InspectionStatus);
        }

        /// <summary>
        /// 提交单据-定性不合格
        /// </summary>
        [Trait("提交单据", "定性不合格")]
        [Fact]
        public void SumbitBillFailTest1()
        {
            var newbill = CreateBill();
            var bill = _iqcBillController.WritingReport(newbill.Id);
            SetDetailsDataFail(bill);
            SetReelIds(bill);
            bill.InspectionResult = InspectionResult.Fail;

            _iqcBillController.Sumbit(bill);
            var resultBill = RF.GetById<IqcBill>(bill.Id);
            Assert.Equal(InspectionResult.Fail, resultBill.InspectionResult);
            Assert.Equal(InspectionStatus.Inspectioned, resultBill.InspectionStatus);
        }

        /// <summary>
        /// 提交单据-定量不合格
        /// </summary>
        [Trait("提交单据", "定量不合格")]
        [Fact]
        public void SumbitBillFailTest2()
        {
            var newbill = CreateBill();
            var bill = _iqcBillController.WritingReport(newbill.Id);
            SetDetailsDataFailQuantity(bill);
            SetReelIds(bill);
            bill.InspectionResult = InspectionResult.Fail;

            _iqcBillController.Sumbit(bill);
            var resultBill = RF.GetById<IqcBill>(bill.Id);
            var detailList = _iqcBillController.GetViewInspBillDetails(bill);//加载明细

            Assert.Equal(InspectionResult.Fail, resultBill.InspectionResult);
            Assert.Equal(InspectionStatus.Inspectioned, resultBill.InspectionStatus);
            Assert.Contains(detailList, p => p.JoinDefectCodes.IsNotEmpty());   //明细加载缺陷信息

        }

        /// <summary>
        /// 异常提交单据-不录入ReelId
        /// </summary>
        [Trait("异常提交单据", "不录入ReelId")]
        [Fact]
        public void SumbitBillPassNoReelIdTest1()
        {
            var newbill = CreateBill();
            var bill = _iqcBillController.WritingReport(newbill.Id);
            bill.HasKeyInspItems = _iqcBillController.UpdateKeyInspItemInfo(bill);
            SetDetailsDataPass(bill);
            bill.InspectionResult = InspectionResult.Pass;

            Assert.Throws<ValidationException>(() => _iqcBillController.Sumbit(bill));
        }
        #endregion

        #region 确认合格
        /// <summary>
        /// 确认合格
        /// </summary>
        [Trait("确认合格", "数据合格")]
        [Fact]
        public void ConfirmBillTest1()
        {
            var newbill = CreateBill();
            var bill = _iqcBillController.WritingReport(newbill.Id);
            _iqcBillController.ConfirmPass(bill.Id);
            var resultBill = RF.GetById<IqcBill>(bill.Id);
            Assert.Equal(InspectionResult.Pass, resultBill.InspectionResult);
            Assert.Equal(InspectionStatus.Inspecting, resultBill.InspectionStatus);

            SetDetailsDataPass(resultBill);
            SetReelIds(bill);
            _iqcBillController.Sumbit(resultBill);
            var resultBillSubmit = RF.GetById<IqcBill>(resultBill.Id);
            Assert.Equal(InspectionResult.Pass, resultBillSubmit.InspectionResult);
            Assert.Equal(InspectionStatus.Inspectioned, resultBillSubmit.InspectionStatus);
        }

        /// <summary>
        /// 确认合格
        /// </summary>
        [Trait("确认合格", "数据不合格")]
        [Fact]
        public void ConfirmBillTest2()
        {
            var newbill = CreateBill();
            var bill = _iqcBillController.WritingReport(newbill.Id);
            _iqcBillController.ConfirmPass(bill.Id);
            var resultBill = RF.GetById<IqcBill>(bill.Id);
            Assert.Equal(InspectionResult.Pass, resultBill.InspectionResult);
            Assert.Equal(InspectionStatus.Inspecting, resultBill.InspectionStatus);

            SetDetailsDataFail(resultBill);
            resultBill.InspectionResult = InspectionResult.Fail;
            Assert.Throws<ValidationException>(() => _iqcBillController.Sumbit(resultBill));
        }

        /// <summary>
        /// 确认合格
        /// </summary>
        [Trait("确认合格", "确认检")]
        [Fact]
        public void ConfirmBillTest3()
        {
            var bill = CreateConfirmInspBill();
            _iqcBillController.ConfirmPass(bill.Id);
            var resultBill = RF.GetById<IqcBill>(bill.Id);
            Assert.Equal(InspectionResult.Pass, resultBill.InspectionResult);
            Assert.Equal(InspectionStatus.Inspectioned, resultBill.InspectionStatus);
        }

        /// <summary>
        /// 创建确认检单据
        /// </summary>
        private IqcBill CreateConfirmInspBill()
        {
            var bill = CreateBill(false);
            bill.InspectionCharacteristics = InspectionCharacteristics.ConfirmInspection;
            RF.Save(bill);
            return bill;
        }
        #endregion

        #region 一键判退
        /// <summary>
        /// 一键判退
        /// </summary>
        [Trait("一键判退", "常规检")]
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void RejectBillTest1(bool isWriteReport)
        {
            var newbill = CreateBill();
            IqcBill bill = newbill;
            if (isWriteReport)
                bill = _iqcBillController.WritingReport(newbill.Id);
            bill.RejectedReason = $"RejectReason{bill.Id}";
            _iqcBillController.UpdateIqcBillInfo(bill);
            var resultBill = RF.GetById<IqcBill>(bill.Id);
            Assert.Equal(InspectionResult.Fail, resultBill.InspectionResult);
            Assert.Equal(InspectionStatus.Inspectioned, resultBill.InspectionStatus);
        }
        #endregion

        #region 合并单据
        /// <summary>
        /// 合并单据
        /// </summary>
        [Trait("合并/取消合并单据", "合并单据")]
        [Fact]
        public void MergeBill()
        {
            var bill1 = CreateBill();
            var bill2 = CreateBill();
            var billIds = new List<double>() { bill1.Id, bill2.Id };
            //合并单据
            var resultBill = _iqcBillController.MergeBill(billIds);
            var srcBill1 = RF.GetById<IqcBill>(bill1.Id);
            Assert.Equal(bill1.Qty + bill2.Qty, resultBill.Qty);
            if (bill1.BatchNo == bill2.BatchNo)
                Assert.Equal($"{bill1.BatchNo}", resultBill.BatchNo);
            else
                Assert.Equal($"{bill1.BatchNo},{bill2.BatchNo}", resultBill.BatchNo);
            if (bill1.AsnNo == bill2.AsnNo)
                Assert.Equal($"{bill1.AsnNo}", resultBill.AsnNo);
            else
                Assert.Equal($"{bill1.AsnNo},{bill2.AsnNo}", resultBill.AsnNo);
            Assert.Equal(MergeState.FromMerge, resultBill.MergeState);
            Assert.Equal(MergeState.Merged, srcBill1.MergeState);
        }

        /// <summary>
        /// 取消合并单据
        /// </summary>
        [Trait("合并/取消合并单据", "取消合并子单据,2个子单据")]
        [Fact]
        public void CancelMergeBill()
        {
            var bill1 = CreateBill();
            var bill2 = CreateBill();
            var billIds = new List<double>() { bill1.Id, bill2.Id };
            //合并单据
            var resultBill = _iqcBillController.MergeBill(billIds);
            var srcBill1 = RF.GetById<IqcBill>(bill1.Id);
            //取消合并
            _iqcBillController.CancelMergeMethod(srcBill1);
            var srcBillCancelMerge1 = RF.GetById<IqcBill>(bill1.Id);
            var masterBill = RF.GetById<IqcBill>(resultBill.Id);//取消合并后主单删除
            Assert.Null(masterBill);
            Assert.Equal(MergeState.Normal, srcBillCancelMerge1.MergeState);
        }

        /// <summary>
        /// 取消合并单据
        /// </summary>
        [Trait("合并/取消合并单据", "取消合并子单据,3个子单据")]
        [Fact]
        public void CancelMergeBill2()
        {
            var bill1 = CreateBill();
            var bill2 = CreateBill();
            var bill3 = CreateBill();
            var billIds = new List<double>() { bill1.Id, bill2.Id, bill3.Id };
            //合并单据
            var resultBill = _iqcBillController.MergeBill(billIds);
            var srcBill1 = RF.GetById<IqcBill>(bill1.Id);
            //取消合并
            _iqcBillController.CancelMergeMethod(srcBill1);
            var srcBillCancelMerge1 = RF.GetById<IqcBill>(bill1.Id);
            var masterBill = RF.GetById<IqcBill>(resultBill.Id);//3个子单据，取消合并后主单不删除
            Assert.Equal(bill2.Qty + bill3.Qty, masterBill.Qty);
            if (bill2.BatchNo == bill3.BatchNo)
                Assert.Equal($"{bill2.BatchNo}", masterBill.BatchNo);
            else
                Assert.Equal($"{bill2.BatchNo},{bill3.BatchNo}", masterBill.BatchNo);
            if (bill2.AsnNo == bill3.AsnNo)
                Assert.Equal($"{bill2.AsnNo}", masterBill.AsnNo);
            else
                Assert.Equal($"{bill2.AsnNo},{bill3.AsnNo}", masterBill.AsnNo);
            Assert.Equal(MergeState.FromMerge, masterBill.MergeState);
            Assert.Equal(MergeState.Normal, srcBillCancelMerge1.MergeState);
        }

        /// <summary>
        /// 取消合并单据
        /// </summary>
        [Trait("合并/取消合并单据", "取消合并主单据")]
        [Fact]
        public void CancelMergeBill3()
        {
            var bill1 = CreateBill();
            var bill2 = CreateBill();
            var billIds = new List<double>() { bill1.Id, bill2.Id };
            //合并单据
            var resultBill = _iqcBillController.MergeBill(billIds);
            var bills = new List<IqcBill>() { resultBill };
            //取消合并
            _iqcBillController.CancelMergeMethod(bills);
            var srcBill1 = RF.GetById<IqcBill>(bill1.Id);
            var masterBill = RF.GetById<IqcBill>(resultBill.Id);//取消合并后主单删除
            Assert.Null(masterBill);
            Assert.Equal(MergeState.Normal, srcBill1.MergeState);
        }

        /// <summary>
        /// 合并单据后提交
        /// </summary>
        [Trait("合并/取消合并单据", "合并单据并提交")]
        [Fact]
        public void MergeBillSubmit1()
        {
            var bill1 = CreateBill();
            var bill2 = CreateBill();
            var billIds = new List<double>() { bill1.Id, bill2.Id };
            //合并单据
            var mergeBill = _iqcBillController.MergeBill(billIds);
            var writingBill = _iqcBillController.WritingReport(mergeBill.Id);
            SetDetailsDataFailQuantity(writingBill);
            SetReelIds(writingBill);
            writingBill.InspectionResult = InspectionResult.Fail;
            _iqcBillController.Sumbit(writingBill);

            var resultBill = RF.GetById<IqcBill>(mergeBill.Id);

            Assert.Equal(InspectionResult.Fail, resultBill.InspectionResult);
            Assert.Equal(InspectionStatus.Inspectioned, resultBill.InspectionStatus);
        }
        #endregion

        #region 设置项目明细数据
        /// <summary>
        /// 设置合格明细数据
        /// </summary>
        /// <param name="bill"></param>
        private void SetDetailsDataPass(IqcBill bill)
        {
            foreach (var detail in bill.DetailList)
            {
                detail.IsEnable = true;
                if (detail.CheckTag == CheckTag.Qualitative)
                {
                    //定性项目
                    detail.DefectQty = 0;
                    detail.InspectionResult = Common.InspectionResult.Pass;
                }
                else if (detail.CheckTag == CheckTag.Quantitative)
                {
                    //定量项目
                    for (int i = 0; i < detail.ValueList.Count; i++)
                    {
                        detail.ValueList[i].Index = i + 1;
                        detail.ValueList[i].CheckValue = QmsHelper.GetRandomCheckValue(detail.LimitLowCompare, detail.LimitLow, detail.LimitMaxCompare, detail.LimitMax);
                    }
                    detail.DefectQty = 0;
                    detail.InspectionResult = Common.InspectionResult.Pass;
                }
            }
        }

        /// <summary>
        /// 设置项目数据-定性不合格
        /// </summary>
        /// <param name="bill"></param>
        private void SetDetailsDataFail(IqcBill bill)
        {
            foreach (var detail in bill.DetailList)
            {
                detail.IsEnable = true;
                if (detail.CheckTag == CheckTag.Qualitative)
                {
                    //定性项目
                    detail.DefectQty = detail.SamplingQty;
                    detail.InspectionResult = Common.InspectionResult.Fail;
                    var defect = _iqcBillFixture.FixPropDefectList.Random();
                    detail.DefectCodes = defect.Id.ToString();
                    detail.DefectDescription = defect.Description.ToString();
                }
                else if (detail.CheckTag == CheckTag.Quantitative)
                {
                    //定量项目
                    for (int i = 0; i < detail.ValueList.Count; i++)
                    {
                        detail.ValueList[i].Index = i + 1;
                        detail.ValueList[i].CheckValue = QmsHelper.GetRandomCheckValue(detail.LimitLowCompare, detail.LimitLow, detail.LimitMaxCompare, detail.LimitMax);
                    }
                    detail.DefectQty = 0;
                    detail.InspectionResult = InspectionResult.Pass;
                }
            }
        }

        /// <summary>
        /// 设置项目明细数据-定量不合格
        /// </summary>
        /// <param name="bill"></param>
        private void SetDetailsDataFailQuantity(IqcBill bill)
        {
            foreach (var detail in bill.DetailList)
            {
                detail.IsEnable = true;
                if (detail.CheckTag == CheckTag.Qualitative)
                {
                    //定性项目
                    detail.DefectQty = 0;
                    detail.InspectionResult = Common.InspectionResult.Pass;
                }
                else if (detail.CheckTag == CheckTag.Quantitative)
                {
                    //定量项目
                    for (int i = 0; i < detail.ValueList.Count; i++)
                    {
                        detail.ValueList[i].Index = i + 1;
                        detail.ValueList[i].CheckValue = QmsHelper.GetRandomFailCheckValue(detail.LimitLowCompare, detail.LimitLow, detail.LimitMaxCompare, detail.LimitMax);
                    }
                    detail.DefectQty = detail.ValueList.Count;
                    detail.DefectCodes = string.Join(',', _iqcBillFixture.FixPropDefectList.Select(p => p.Id).ToList());
                    detail.DefectDescription = string.Join(',', _iqcBillFixture.FixPropDefectList.Select(p => p.Description).ToList());
                    detail.InspectionResult = InspectionResult.Fail;
                }
            }
        }

        /// <summary>
        /// 设置ReelID
        /// </summary>
        /// <param name="bill"></param>
        private void SetReelIds(IqcBill bill)
        {
            //录入ReelID
            var reelList = IqcBillExtension.GetReelList(bill) ?? new EntityList<BillReel>();
            foreach (var label in _iqcBillFixture.FixPropLabelList)
            {
                BillReel billReel = new BillReel()
                {
                    IqcBillId = bill.Id,
                    ReelId = label.No,
                    Quannity = label.Qty,
                };
                reelList.Add(billReel);
            }
            RF.Save(reelList);
        }
        #endregion
    }
}

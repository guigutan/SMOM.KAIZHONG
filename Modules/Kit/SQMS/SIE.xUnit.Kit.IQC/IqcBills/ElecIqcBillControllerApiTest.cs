using SIE.Common;
using SIE.Defects.InspectionItems;
using SIE.Domain;
using SIE.Kit.IQC.IqcBills;
using SIE.Kit.IQC.IqcBills.ApiModels;
using SIE.IQC.IqcBills;
using SIE.QMS.Common;
using SIE.xUnit.Core.Common;
using SIE.xUnit.Elec.IQC.IqcBills.Fixtures;
using SIE.xUnit.PDCA.Report.Fixtures;
using SIE.xUnit.QMS.Common;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace SIE.xUnit.Elec.IQC.IqcBills
{
    /// <summary>
    /// 电子-来料检验控制器单元测试-移动端接口
    /// </summary>
    public class ElecIqcBillControllerApiTest : ElecIqcControllerTestBase, IClassFixture<ElecIqcBillFixture>, IClassFixture<ReportBaseFixture>
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
        public ElecIqcBillControllerApiTest(ElecIqcBillFixture iqcBillFixture, ReportBaseFixture pdcaFixture)
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

        /// <summary>
        /// 创建单据，返回信息
        /// </summary>
        /// <returns></returns>
        private KitIqcInspBillInfo CreateBillInfo()
        {
            var newbill = CreateBill();
            KitIqcInspBillInfo newBillInfo = new KitIqcInspBillInfo() { Id = newbill.Id, InspBillType = InspectionType.IncomingInsp };
            return newBillInfo;
        }

        #region 移动端提交单据
        /// <summary>
        /// 移动端提交单据-合格
        /// </summary>
        [Trait("移动端提交单据", "合格")]
        [Fact]
        public void SumbitBillPassTest1()
        {
            KitIqcInspBillInfo newBillInfo = CreateBillInfo();
            var billInfo = _iqcBillController.IqcBillWritingReport(newBillInfo) as KitIqcInspBillInfo;
            SetDetailsDataInfoPass(billInfo);
            SetReelIds(billInfo);
            var savedBillInfo = _iqcBillController.SaveElecIqcBill(billInfo);
            savedBillInfo.InspectionResult = InspectionResult.Pass;

            _iqcBillController.SumbitIqcBill(savedBillInfo);
            var resultBill = RF.GetById<IqcBill>(savedBillInfo.Id);
            Assert.Equal(InspectionResult.Pass, resultBill.InspectionResult);
            Assert.Equal(InspectionStatus.Inspectioned, resultBill.InspectionStatus);
        }

        /// <summary>
        /// 提交单据-定性不合格
        /// </summary>
        [Trait("移动端提交单据", "定性不合格")]
        [Fact]
        public void SumbitBillFailTest1()
        {
            KitIqcInspBillInfo newBillInfo = CreateBillInfo();
            var billInfo = _iqcBillController.IqcBillWritingReport(newBillInfo) as KitIqcInspBillInfo;
            SetDetailsDataInfoFail(billInfo);
            SetReelIds(billInfo);
            var savedBillInfo = _iqcBillController.SaveElecIqcBill(billInfo);

            //获取ReelId
            var label = _iqcBillFixture.FixPropLabelList[0];
            var reel = _iqcBillController.GetReelIDInAsnNo(label.No, label.AsnNo, savedBillInfo.Id);
            Assert.Equal(label.No, reel.ReelID);
            Assert.Equal(label.Qty, reel.Qty);

            savedBillInfo.InspectionResult = InspectionResult.Fail;

            _iqcBillController.SumbitIqcBill(savedBillInfo);
            var resultBill = RF.GetById<IqcBill>(savedBillInfo.Id);
            Assert.Equal(InspectionResult.Fail, resultBill.InspectionResult);
            Assert.Equal(InspectionStatus.Inspectioned, resultBill.InspectionStatus);
        }

        /// <summary>
        /// 提交单据-定量不合格
        /// </summary>
        [Trait("移动端提交单据", "定量不合格")]
        [Fact]
        public void SumbitBillFailTest2()
        {
            KitIqcInspBillInfo newBillInfo = CreateBillInfo();
            var billInfo = _iqcBillController.IqcBillWritingReport(newBillInfo) as KitIqcInspBillInfo;
            SetReelIds(billInfo);
            var savedBillInfo = _iqcBillController.SaveElecIqcBill(billInfo);
            SetDetailsDataFailInfoQuantity(savedBillInfo);

            savedBillInfo.InspectionResult = InspectionResult.Fail;

            _iqcBillController.SumbitIqcBill(savedBillInfo);
            var resultBill = RF.GetById<IqcBill>(savedBillInfo.Id);

            Assert.Equal(InspectionResult.Fail, resultBill.InspectionResult);
            Assert.Equal(InspectionStatus.Inspectioned, resultBill.InspectionStatus);

        }

        /// <summary>
        /// 合并单据后提交
        /// </summary>
        [Trait("移动端提交单据", "合并单据提交")]
        [Fact]
        public void MergeBillSubmit1()
        {
            var bill1 = CreateBill();
            var bill2 = CreateBill();
            var billIds = new List<double>() { bill1.Id, bill2.Id };
            //合并单据
            var mergeBill = _iqcBillController.MergeBill(billIds);

            var billInfo = _iqcBillController.IqcBillWritingReport(new SIE.QMS.ApiModel.InspBillInfo() { Id = mergeBill.Id }) as KitIqcInspBillInfo;
            SetReelIds(billInfo);
            var savedBillInfo = _iqcBillController.SaveElecIqcBill(billInfo);
            SetDetailsDataFailInfoQuantity(savedBillInfo);

            savedBillInfo.InspectionResult = InspectionResult.Fail;

            _iqcBillController.SumbitIqcBill(savedBillInfo);

            var resultBill = RF.GetById<IqcBill>(savedBillInfo.Id);

            Assert.Equal(InspectionResult.Fail, resultBill.InspectionResult);
            Assert.Equal(InspectionStatus.Inspectioned, resultBill.InspectionStatus);
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
            var newBillInfo = CreateBillInfo();
            var bill = _iqcBillController.WritingReport(newBillInfo.Id);
            _iqcBillController.ConfirmPass(newBillInfo);
            var resultBill = _iqcBillController.GetIqcBillById(bill.Id);
            Assert.Equal(InspectionResult.Pass, resultBill.InspectionResult);
            Assert.Equal(InspectionStatus.Inspecting, resultBill.InspectionStatus);

            //补填检验数据
            var resultBillInfo = _iqcBillController.IqcBillWritingReport(newBillInfo) as KitIqcInspBillInfo;
            SetDetailsDataInfoPass(resultBillInfo);
            SetReelIds(resultBillInfo);
            var savedBillInfo = _iqcBillController.SaveElecIqcBill(resultBillInfo);
            _iqcBillController.SumbitIqcBill(savedBillInfo);
            var resultBillSubmit = RF.GetById<IqcBill>(savedBillInfo.Id);
            Assert.Equal(InspectionResult.Pass, resultBillSubmit.InspectionResult);
            Assert.Equal(InspectionStatus.Inspectioned, resultBillSubmit.InspectionStatus);
        }

        /// <summary>
        /// 确认合格
        /// </summary>
        [Trait("确认合格", "确认检")]
        [Fact]
        public void ConfirmBillTest2()
        {
            var bill = CreateBill(false);
            bill.InspectionCharacteristics = InspectionCharacteristics.ConfirmInspection;   //确认检单据
            RF.Save(bill);
            _iqcBillController.WritingReport(bill.Id);
            _iqcBillController.ConfirmPass(new KitIqcInspBillInfo() { Id = bill.Id });
            var resultBill = RF.GetById<IqcBill>(bill.Id);
            Assert.Equal(InspectionResult.Pass, resultBill.InspectionResult);
            Assert.Equal(InspectionStatus.Inspectioned, resultBill.InspectionStatus);
        }
        #endregion

        #region 一键判退
        /// <summary>
        /// 一键判退
        /// </summary>
        [Trait("一键判退", "常规检")]
        [Fact]
        public void RejectBillTest1()
        {
            KitIqcInspBillInfo newBillInfo = CreateBillInfo();
            var billInfo = _iqcBillController.IqcBillWritingReport(newBillInfo);
            billInfo.RejectedReason = $"RejectReason{billInfo.Id}";
            _iqcBillController.UpdateIqcBillInfo(billInfo);
            var resultBill = RF.GetById<IqcBill>(billInfo.Id);
            Assert.Equal(InspectionResult.Fail, resultBill.InspectionResult);
            Assert.Equal(InspectionStatus.Inspectioned, resultBill.InspectionStatus);
        }
        #endregion

        #region 设置项目明细数据
        /// <summary>
        /// 设置合格明细数据
        /// </summary>
        /// <param name="bill"></param>
        private void SetDetailsDataInfoPass(KitIqcInspBillInfo bill)
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
        private void SetDetailsDataInfoFail(KitIqcInspBillInfo bill)
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
                    detail.DefectIdList = new List<double>() { defect.Id };
                    detail.DefectCodeDescriptionList = new List<string>() { defect.Description };
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
        private void SetDetailsDataFailInfoQuantity(KitIqcInspBillInfo bill)
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
                    detail.DefectIdList = _iqcBillFixture.FixPropDefectList.Select(p => p.Id).ToList();
                    detail.DefectCodeDescriptionList = _iqcBillFixture.FixPropDefectList.Select(p => p.Description).ToList();
                    detail.InspectionResult = InspectionResult.Fail;
                }
            }
        }

        /// <summary>
        /// 设置ReelID
        /// </summary>
        /// <param name="bill"></param>
        private void SetReelIds(KitIqcInspBillInfo bill)
        {
            //录入ReelID
            bill.ReelIDList ??= new List<IqcReelIDInfo>();
            foreach (var label in _iqcBillFixture.FixPropLabelList)
            {
                IqcReelIDInfo billReel = new IqcReelIDInfo()
                {
                    ReelID = label.No,
                    Qty = label.Qty,
                    PersistenceStatus = PersistenceStatus.New
                };
                bill.ReelIDList.Add(billReel);
            }
        }
        #endregion


    }
}

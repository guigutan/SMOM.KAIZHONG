using SIE.Common;
using SIE.Defects.InspectionItems;
using SIE.Domain;
using SIE.Kit.IQC.IqcBills;
using SIE.EventMessages;
using SIE.IQC.IqcBills;
using SIE.IQC.IqcEvents;
using SIE.QMS.AuditFailedList;
using SIE.QMS.Common;
using SIE.xUnit.Core.Common;
using SIE.xUnit.Elec.IQC.IqcBills.Fixtures;
using SIE.xUnit.PDCA.Report.Fixtures;
using SIE.xUnit.QMS.Common;
using System;
using System.Linq;
using Xunit;

namespace SIE.xUnit.Elec.IQC.IqcBills
{
    /// <summary>
    /// 电子行业-来料检验控制器单元测试-退料报检
    /// </summary>
    public class ElecIqcBillControllerInspTest : ElecIqcControllerTestBase, IClassFixture<ElecIqcBillReturnFixture>, IClassFixture<ReportBaseFixture>
    {
        /// <summary>
        /// 来料检验固件
        /// </summary>
        private readonly ElecIqcBillReturnFixture _iqcBillFixture;

        /// <summary>
        /// PDCA固件（初始化PDCA配置项环境）
        /// </summary>
        private readonly ReportBaseFixture _pdcaFixture;

        /// <summary>
        /// 来料检验控制器
        /// </summary>
        private readonly IqcBillController _iqcBillController;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="iqcBillFixture"></param>
        public ElecIqcBillControllerInspTest(ElecIqcBillReturnFixture iqcBillFixture, ReportBaseFixture pdcaFixture)
        {
            _iqcBillFixture = iqcBillFixture;
            _pdcaFixture = pdcaFixture;
            _iqcBillController = RT.Service.Resolve<IqcBillController>();
        }

        /// <summary>
        /// 创建报检单据参数
        /// </summary>
        /// <returns></returns>
        private ReceiveGoodsEvent CreateInspBillParam()
        {
            var tempBill = new IqcBill();
            tempBill.GenerateId();
            var billId = tempBill.Id; //非重复数值
            var e = new ReceiveGoodsEvent()
            {
                ItemId = _iqcBillFixture.FixPropItem.Id,
                SupplierId = _iqcBillFixture.FixPropSupplier.Id,
                BatchNo = $"Batch{billId}",
                IsUrgent = false,
                AsnNo = $"Asn{billId}",
                InspectionId = billId + 1,
                AsnDtlId = billId + 2,
                Lot = $"Lot{billId}",
                Qty = 150,
                ReceiveGoodType = ReceiveGoodType.Return
            };
            return e;
        }

        /// <summary>
        /// 创建报检单 
        /// </summary>
        private IqcBill CreateInspBill()
        {
            var eventParam = CreateInspBillParam();
            RT.Service.Resolve<IqcBillEventsController>().CreateIqcBill(eventParam);
            return _iqcBillController.QueryIqcBills(new IqcBillCriteria() { AsnNo = eventParam.AsnNo }, RT.IdentityId).FirstOrDefault();
        }

        /// <summary>
        /// IQC退料报检
        /// </summary>
        [Trait("IQC退料报检", "成功")]
        [Fact]
        public void CreateBillTest()
        {
            var eventParam = CreateInspBillParam();
            RT.Service.Resolve<IqcBillEventsController>().CreateIqcBill(eventParam);
            var resultBills = _iqcBillController.QueryIqcBills(new IqcBillCriteria() { AsnNo = eventParam.AsnNo }, RT.IdentityId);
            Assert.Single(resultBills);
            Assert.Equal("退料".L10N(), resultBills.First().InspectionMode.Name);
        }
    }
}

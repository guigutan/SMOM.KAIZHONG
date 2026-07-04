using SIE.Barcodes;
using SIE.Domain;
using SIE.xUnit.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace SIE.xUnit.Barcodes.Barcodes
{
    public class BarcodeControllerTest : IClassFixture<TestStarup>
    {
        /// <summary>
        /// 查询条码测试
        /// </summary>
        [Fact]
        public void GetBarcodeTest()
        {
            BarcodeController barcodeController = RT.Service.Resolve<BarcodeController>();
            RT.Service.Resolve<ContextControllerTest>().InitContext();
            //新增数据 
            var sns = new List<string> { };
            var barcodeSn = DateTime.Now.ToString().Replace("/", "").Replace(":", "").Replace(" ", "");
            for (var i = 0; i < 10; i++)
            {
                sns.Add("SN" + barcodeSn + i);
            }
            RT.Service.Resolve<BarcodeTestController>().CreateBarcodeBySn(sns);
            //根据sn获取条码
            Assert.Throws<ArgumentException>(() => barcodeController.GetBarcode(null));
            var barcode1 = barcodeController.GetBarcode(Guid.NewGuid().ToString());
            Assert.Null(barcode1);
            var barcode2 = barcodeController.GetBarcode(sns.FirstOrDefault());
            Assert.NotNull(barcode2);
            Assert.Equal(sns.FirstOrDefault(), barcode2.Sn);
            //获取条码号列表所对应的所有条码列表
            var barcodes0 = barcodeController.GetBarcodesBySns(new List<string> { });
            Assert.Empty(barcodes0);
            var barcodes1 = barcodeController.GetBarcodesBySns(new List<string> { "dhsfgkjagfdh", "dgfh56458fdg", Guid.NewGuid().ToString() });
            Assert.Empty(barcodes1);
            var barcodes2 = barcodeController.GetBarcodesBySns(sns);
            Assert.Equal(10, barcodes2.Count);
            //获取条码Id所对应的所有条码列表
            var barcodesIds = barcodes2.Select(p => p.Id).ToList<double>();
            var barcodes3 = barcodeController.GetBarcodesByIds(barcodesIds);
            Assert.Equal(10, barcodes3.Count);
            var barcodes4 = barcodeController.GetBarcodesByIds(new List<double> { -5, -365366, 86867.4785788 });
            Assert.Empty(barcodes4);
            var barcodes5 = barcodeController.GetBarcodesByIds(new List<double> { });
            Assert.Empty(barcodes5);
            //条码是否存在
            var barcodeExist1 = barcodeController.Exists(null);
            Assert.False(barcodeExist1);
            var barcodeExist2 = barcodeController.Exists(sns[9]);
            Assert.True(barcodeExist2);
            var barcodeExist3 = barcodeController.Exists(Guid.NewGuid().ToString());
            Assert.False(barcodeExist3);
        }

        /// <summary>
        /// 查询条码测试
        /// </summary>
        [Fact]
        public void GetBarcodeTest1()
        {
            BarcodeController barcodeController = RT.Service.Resolve<BarcodeController>();
            RT.Service.Resolve<ContextControllerTest>().InitContext();
            //新增数据 
            var sns = new List<string> { };
            var barcodeSn = DateTime.Now.ToString().Replace("/", "").Replace(":", "").Replace(" ", "");
            for (var i = 0; i < 10; i++)
            {
                sns.Add("SNQ" + barcodeSn + i);
            }
            RT.Service.Resolve<BarcodeTestController>().CreateBarcodeBySn(sns);
            //根据查询实体获取条码
            var barcodeCriteria = new BarcodeCriteria();
            var barcodes1 = barcodeController.GetBarcodes(barcodeCriteria);
            Assert.NotEmpty(barcodes1);
            barcodeCriteria.Sn = sns[3];
            var barcodes2 = barcodeController.GetBarcodes(barcodeCriteria);
            Assert.Single(barcodes2);
            Assert.Equal(barcodes2.FirstOrDefault().Sn, sns[3]);
            barcodeCriteria.WorkOrderNo = "工单号";
            var barcodes3 = barcodeController.GetBarcodes(barcodeCriteria);
            Assert.Empty(barcodes3);
            var barcodeCriteria1 = new BarcodeCriteria();
            barcodeCriteria1.PrinterId = RT.IdentityId;
            barcodeCriteria1.State = BarcodeState.Printed;
            var barcodes4 = barcodeController.GetBarcodes(barcodeCriteria1);
            Assert.NotEmpty(barcodes4);
            barcodeCriteria1.PrintDate.BeginValue = DateTime.Now.AddDays(-1);
            barcodeCriteria1.PrintDate.EndValue = DateTime.Now.AddDays(1);
            var barcodes5 = barcodeController.GetBarcodes(barcodeCriteria1);
            Assert.NotEmpty(barcodes5);
            barcodeCriteria1.IsPending = false;
            barcodeCriteria1.IsScraped = false;
            var barcodes6 = barcodeController.GetBarcodes(barcodeCriteria1);
            Assert.NotEmpty(barcodes6);
        }

        /// <summary>
        /// 查询条码测试
        /// </summary>
        [Fact]
        public void GetBarcodeTest2()
        {
            BarcodeController barcodeController = RT.Service.Resolve<BarcodeController>();
            RT.Service.Resolve<ContextControllerTest>().InitContext();
            //新增数据 
            var sns = new List<string> { };
            var barcodeSn = DateTime.Now.ToString().Replace("/", "").Replace(":", "").Replace(" ", "");
            for (var i = 0; i < 10; i++)
            {
                sns.Add("SNW" + barcodeSn + i);
            }
            RT.Service.Resolve<BarcodeTestController>().CreateBarcodeBySn(sns);
            var barCodeInfoWithQty = barcodeController.GetBarcodeInfo(sns);
            Assert.NotEmpty(barCodeInfoWithQty);
            Assert.Equal(10, barCodeInfoWithQty.Count);
            Assert.All(barCodeInfoWithQty, p => Assert.Equal(1, p.Qty));
        }
    }
}

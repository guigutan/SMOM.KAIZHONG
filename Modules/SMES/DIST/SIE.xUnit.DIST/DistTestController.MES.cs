using SIE.Common.Configs;
using SIE.DIST;
using SIE.DIST.Distribution.Configs;
using SIE.Domain;
using SIE.Domain.Serialization.Json;
using SIE.xUnit.Core;
using SIE.xUnit.DIST.Distribution.Models;
using SIE.xUnit.Packages;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.xUnit.DIST
{
    public partial class DistTestController : DomainController
    {
        public virtual void ConfigDistributionBillNo()
        {
            var config = ConfigService.GetConfig(new BillNoConfig(), typeof(DistributionBill));
            if (config == null || config.NumberRule == null)
            {
                var controller = RT.Service.Resolve<ContextControllerTest>();
                var configValue = new BillNoConfigValue();
                configValue.NumberRule = controller.CreateNumberRule("配送单单号生成规则", "DB");
                string value = DomainJsonConvert.SerializeObject(configValue, ConfigValueSerializerSettings.Settings);
                controller.CreateConfig(typeof(DistributionBill).GetQualifiedName(), typeof(BillNoConfig).GetQualifiedName(), value);
            }
        }

        public virtual void ConfigDistributionTurnoverBoxType()
        {
            var config = ConfigService.GetConfig(new DistributionTurnoverBoxTypeConfig());
            if (config == null || config.BoxType.IsNullOrEmpty())
            {
                var boxType = RT.Service.Resolve<PkgTestController>().GetDistTurnoverBoxType();
                var configValue = new DistributionTurnoverBoxTypeConfigValue();
                configValue.BoxType = boxType;
                string value = DomainJsonConvert.SerializeObject(configValue, ConfigValueSerializerSettings.Settings);
                RT.Service.Resolve<ContextControllerTest>().CreateGlobalConfig(typeof(DistributionTurnoverBoxTypeConfig).GetQualifiedName(), value);
            }
        }

        public virtual List<string> WorkOrderDistribution(DistributionInfo info)
        {
            //配置配送周转箱
            ConfigDistributionTurnoverBoxType();
            //配置配送单单号规则
            ConfigDistributionBillNo();
            List<string> boxs = new List<string>();
            info.DetailInfos.ForEach(dtlInfo =>
            {
                var goodsIssue = new GoodsIssue()
                {
                    WorkOrderId = info.WorkOrderId,
                    ItemId = dtlInfo.ItemId,
                    UnitId = dtlInfo.UnitId,
                    BatchNo = "B018-9556-55",
                    Qty = dtlInfo.Qty,
                    RemainderQty=dtlInfo.Qty,
                    SendNo = "SN26526-5663",
                    DefectQty = 0,
                    DefectReturnQty = 0,
                    NormalReturnQty = 0
                };
                goodsIssue.GenerateId();
                RF.Save(goodsIssue);
                var box = RT.Service.Resolve<PkgTestController>().CreateDistTurnoverBox();
                boxs.Add(box.Code);
                RT.Service.Resolve<DistributionController>().SaveDistribution(goodsIssue.Id, box.Code, info.ResourceId, dtlInfo.Qty);
            });

            return boxs;
        }
    }
}
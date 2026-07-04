using SIE.Common.Configs;
using SIE.Defects.InspectionItems;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EventMessages;
using SIE.Recheck.Configs;
using SIE.Recheck.RecheckEvents;
using SIE.Recheck.RecheckInspBills;
using System;
using SIE.Kit.ReCheck.RecheckInspBills;
using SIE.Recheck;

namespace SIE.Kit.ReCheck.RecheckEvents
{
    /// <summary>
    /// 超期复检单EventBus监听控制器
    /// </summary>
    public class KitRecheckBillEventController : RecheckBillEventController
    {
        /// <summary>
        /// 根据获取到的WMS送货单信息创建超期复检单
        /// </summary>
        /// <param name="data">WMS超期复检接收信息</param>
        public override void CreateRecheckBill(IqcRecheckBillEvent data)
        {
            using (var trans = DB.TransactionScope(RecheckEntityDataProvider.ConnectionStringName))
            {
                var expiredInspModeConfig = ConfigService.GetConfig<ExpiredInspModeConfigValue>(new ExpiredInspModeConfig(), typeof(RecheckInspBill));
                var inspModeName = expiredInspModeConfig?.ExpiredInspMode;
                if (inspModeName.IsNullOrEmpty())
                    throw new ValidationException("未配置超期复检对应检验方式名称。".L10N());
                var inspectiongMode = AppRuntime.Service.Resolve<InspectionItemController>().GetInspectionMode(inspModeName);
                if (inspectiongMode == null)
                    throw new ValidationException("请配置类型为超期复检，名称为{0}的检验方式".L10nFormat(inspModeName));

                var RecheckInspBillCollected = new RecheckInspBill()
                {
                    InspLogId = data.IqcRecheckId,
                    No = AppRuntime.Service.Resolve<RecheckInspBillController>().GetRecheckInspBillNo(),
                    BatchNo = data?.Lot,
                    SupplierId = data.SupplierId,
                    ItemId = data.ItemId,
                    Qty = data.Qty,
                    InspectionMode = inspectiongMode,
                    ReceiveRecordNo = data.IqcRecheckNo,
                    CustomerId = data.CustomerId
                };
                RecheckInspBillCollected.GenerateId();
                //源ReelId数据添加到超期复检单
                EntityList<RecheckInspSourceBillReel> reelList = new EntityList<RecheckInspSourceBillReel>();
                data.ReedIds.ForEach(item =>
                {
                    reelList.Add(new RecheckInspSourceBillReel
                    {
                        ReelId = item.ReedId,
                        Quannity = item.Qty,
                        RecheckInspBillId = RecheckInspBillCollected.Id
                    });
                });

                RF.Save(RecheckInspBillCollected);
                RF.Save(reelList);
                trans.Complete();
            }
        }
    }
}

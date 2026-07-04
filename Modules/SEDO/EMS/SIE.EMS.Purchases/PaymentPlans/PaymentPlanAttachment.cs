using SIE.Common.Attachments;
using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.Purchases.PaymentPlans
{
    /// <summary>
    /// 付款计划附件
    /// </summary>
    [ChildEntity, Serializable]
    [Label("付款计划附件")]
    public partial class PaymentPlanAttachment : Attachment<PaymentPlan>
    {
    }

    /// <summary>
    ///  仓库
    /// </summary>
    [DataProvider(typeof(EmsEntityDataProvider))]
    public partial class PaymentPlanAttachmentRepository : AttachmentRepository<PaymentPlanAttachment>
    {
    }

    /// <summary>
    /// 附件资料 实体配置
    /// </summary>
    internal class PaymentPlanAttachmentConfig : AttachmentEntityConfig<PaymentPlanAttachment>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            base.ConfigMeta();
            Meta.EnableDiscriminator("EMS_PAYMENT_PLAN");
        }
    }
}

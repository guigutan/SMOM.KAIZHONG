using SIE.Common.Attachments;
using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.MeteringEquipment.Calibrations
{
    /// <summary>
    /// 计量设备定检附件
    /// </summary>
    [ChildEntity, Serializable]
    //[CriteriaQuery]
    [Label("计量设备定检附件")]
    public partial class CalibrationAttachment : Attachment<Calibration>
    {

    }

    /// <summary>
    ///  仓库
    /// </summary>
    [DataProvider(typeof(EntityDataProvider))]
    public partial class CalibrationAttachmentRepository : AttachmentRepository<CalibrationAttachment>
    {
    }
    /// <summary>
    /// 附件资料 实体配置
    /// </summary>
    internal class CalibrationAttachmentConfig : AttachmentEntityConfig<CalibrationAttachment>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            base.ConfigMeta();
            Meta.EnableDiscriminator("EMS_CAL");
        }
    }
}



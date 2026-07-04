using SIE.Common.Attachments;
using SIE.Domain;
using SIE.EMS.EquipRepair.EquipRepairs.Enums;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EMS.EquipRepair.EquipRepairs
{
    /// <summary>
	/// 附件资料
	/// </summary>
	[ChildEntity, Serializable]
    [Label("附件")]
    public partial class EquipRepairAttachment : Attachment<EquipRepairBill>
    {
        #region 来源类型 RepairSourceType
        /// <summary>
        /// 来源类型
        /// </summary>
        [Label("来源类型")]
        public static readonly Property<RepairOperationType> RepairOperationTypeProperty = P<EquipRepairAttachment>.Register(e => e.RepairOperationType);

        /// <summary>
        /// 来源类型
        /// </summary>
        public RepairOperationType RepairOperationType
        {
            get { return this.GetProperty(RepairOperationTypeProperty); }
            set { this.SetProperty(RepairOperationTypeProperty, value); }
        }
        #endregion
    }

    /// <summary>
    ///  仓库
    /// </summary>
    [DataProvider(typeof(EquipRepairEntityDataProvider))]
    public partial class EquipRepairAttachmentRepository : AttachmentRepository<EquipRepairAttachment>
    {
    }

    /// <summary>
    /// 附件资料 实体配置
    /// </summary>
    internal class EquipRepairAttachmentConfig : AttachmentEntityConfig<EquipRepairAttachment>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            base.ConfigMeta();
            Meta.MapTable("EMS_EQUIP_REPAIR_ATT").MapAllPropertiesExcept(EquipRepairAttachment.ContentProperty);//文件内容映射到数据库会卡死
            Meta.EnableDiscriminator("EMS_EQUIP_REPAIR");
        }
    }
}

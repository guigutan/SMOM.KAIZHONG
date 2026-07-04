using SIE.Domain;
using SIE.EMS.Equipments.Accounts;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.MeteringEquipment.MeteringEquipmentAccounts
{
    /// <summary>
    /// 备件清单
    /// </summary>
    [ChildEntity, Serializable]
    [Label("备件清单")]
    public partial class MeteringEquipAccountLubricaSparePart : EquipAccountLubricaSparePartBase
    {
        #region 设备台账润滑 LubricationProject
        /// <summary>
        /// 设备台账润滑项目Id
        /// </summary>
        public static readonly IRefIdProperty LubricationProjectIdProperty = P<MeteringEquipAccountLubricaSparePart>.RegisterRefId(e => e.LubricationProjectId, ReferenceType.Parent);

        /// <summary>
        /// 设备台账润滑项目Id
        /// </summary>
        public double LubricationProjectId
        {
            get { return (double)GetRefId(LubricationProjectIdProperty); }
            set { SetRefId(LubricationProjectIdProperty, value); }
        }

        /// <summary>
        /// 设备台账润滑项目
        /// </summary>
        public static readonly RefEntityProperty<MeteringEquipAccountLubricationProject> LubricationProjectProperty = P<MeteringEquipAccountLubricaSparePart>.RegisterRef(e => e.LubricationProject, LubricationProjectIdProperty);

        /// <summary>
        /// 设备台账润滑项目
        /// </summary>
        public MeteringEquipAccountLubricationProject LubricationProject
        {
            get { return GetRefEntity(LubricationProjectProperty); }
            set { SetRefEntity(LubricationProjectProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 备件清单 实体配置
    /// </summary>
    internal class EquipAccountLubricaSparePartConfig : EntityConfig<MeteringEquipAccountLubricaSparePart>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_ACCOUNT_LUBRICAT_SP").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}

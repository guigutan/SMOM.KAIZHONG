using SIE.Domain;
using SIE.Equipments.Enums;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Equipments.EquipModels
{
    /// <summary>
    /// 设备型号位置
    /// </summary>
    [Label("设备型号位置")]
    [ChildEntity, Serializable]
    public class EquipModelLocation : DataEntity
    {
        #region 分区 Subarea
        /// <summary>
        /// 分区
        /// </summary>
        [Label("分区")]
        public static readonly Property<string> SubareaProperty = P<EquipModelLocation>.Register(e => e.Subarea);

        /// <summary>
        /// 分区
        /// </summary>
        public string Subarea
        {
            get { return this.GetProperty(SubareaProperty); }
            set { this.SetProperty(SubareaProperty, value); }
        }
        #endregion

        #region 大站位 BigStance
        /// <summary>
        /// 大站位
        /// </summary>
        [Label("大站位")]
        public static readonly Property<string> BigStanceProperty = P<EquipModelLocation>.Register(e => e.BigStance);

        /// <summary>
        /// 大站位
        /// </summary>
        public string BigStance
        {
            get { return GetProperty(BigStanceProperty); }
            set { SetProperty(BigStanceProperty, value); }
        }
        #endregion

        #region 站位 Stance
        /// <summary>
        /// 站位
        /// </summary>
        [Label("站位")]
        public static readonly Property<string> StanceProperty = P<EquipModelLocation>.Register(e => e.Stance);

        /// <summary>
        /// 站位
        /// </summary>
        public string Stance
        {
            get { return this.GetProperty(StanceProperty); }
            set { this.SetProperty(StanceProperty, value); }
        }
        #endregion

        #region 站位类型 StanceType
        /// <summary>
        /// 站位类型
        /// </summary>
        [Label("站位类型")]
        public static readonly Property<StanceType> StanceTypeProperty = P<EquipModelLocation>.Register(e => e.StanceType);

        /// <summary>
        /// 站位类型
        /// </summary>
        public StanceType StanceType
        {
            get { return this.GetProperty(StanceTypeProperty); }
            set { this.SetProperty(StanceTypeProperty, value); }
        }
        #endregion

        #region 设备型号 EquipModel
        /// <summary>
        /// 设备型号Id
        /// </summary>
        [Label("设备型号")]
        public static readonly IRefIdProperty EquipModelIdProperty = P<EquipModelLocation>.RegisterRefId(e => e.EquipModelId, ReferenceType.Parent);

        /// <summary>
        /// 设备型号Id
        /// </summary>
        public double EquipModelId
        {
            get { return (double)this.GetRefId(EquipModelIdProperty); }
            set { this.SetRefId(EquipModelIdProperty, value); }
        }

        /// <summary>
        /// 设备型号
        /// </summary>
        public static readonly RefEntityProperty<EquipModel> EquipModelProperty =
            P<EquipModelLocation>.RegisterRef(e => e.EquipModel, EquipModelIdProperty);

        /// <summary>
        /// 设备型号
        /// </summary>
        public EquipModel EquipModel
        {
            get { return this.GetRefEntity(EquipModelProperty); }
            set { this.SetRefEntity(EquipModelProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 设备型号位置维护 实体配置
    /// </summary>
    internal class EquipModelLocationConfig : EntityConfig<EquipModelLocation>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_EQUIP_MODEL_LOC").MapAllProperties();
            Meta.EnableSort();
            Meta.EnablePhantoms();
        }
    }
}
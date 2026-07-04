using SIE.Andon.Andons.Enum;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Andon.Andons
{
    /// <summary>
    /// 安灯类型维护触发权限实体
    /// </summary>
    [ChildEntity, Serializable]
    [DisplayMember(nameof(ObjectCode))]
    [Label("触发权限")]
    public class AndonTypeTriggerPower : DataEntity
    {
        #region 安灯类型维护 AndonType
        /// <summary>
        /// 安灯类型维护Id
        /// </summary>
        [Label("安灯类型维护")]
        public static readonly IRefIdProperty AndonTypeIdProperty =
            P<AndonTypeTriggerPower>.RegisterRefId(e => e.AndonTypeId, ReferenceType.Parent);

        /// <summary>
        /// 安灯类型维护Id
        /// </summary>
        public double AndonTypeId
        {
            get { return (double)this.GetRefId(AndonTypeIdProperty); }
            set { this.SetRefId(AndonTypeIdProperty, value); }
        }

        /// <summary>
        /// 安灯类型维护
        /// </summary>
        public static readonly RefEntityProperty<AndonType> AndonTypeProperty =
            P<AndonTypeTriggerPower>.RegisterRef(e => e.AndonType, AndonTypeIdProperty);

        /// <summary>
        /// 安灯类型维护
        /// </summary>
        public AndonType AndonType
        {
            get { return this.GetRefEntity(AndonTypeProperty); }
            set { this.SetRefEntity(AndonTypeProperty, value); }
        }
        #endregion

        #region 对象类型 ObjectType
        /// <summary>
        /// 对象类型
        /// </summary>
        [Required]
        [Label("对象类型")]
        public static readonly Property<Enum.AndonTypeTriggerPower> ObjectTypeProperty = P<AndonTypeTriggerPower>.Register(e => e.ObjectType);

        /// <summary>
        /// 对象类型
        /// </summary>
        public Enum.AndonTypeTriggerPower ObjectType
        {
            get { return this.GetProperty(ObjectTypeProperty); }
            set { this.SetProperty(ObjectTypeProperty, value); }
        }
        #endregion

        #region 对象编码 ObjectCode
        /// <summary>
        /// 对象编码
        /// </summary>
        [Required]
        [Label("对象编码")]
        public static readonly Property<string> ObjectCodeProperty = P<AndonTypeTriggerPower>.Register(e => e.ObjectCode);

        /// <summary>
        /// 对象编码
        /// </summary>
        public string ObjectCode
        {
            get { return this.GetProperty(ObjectCodeProperty); }
            set { this.SetProperty(ObjectCodeProperty, value); }
        }
        #endregion

        #region 对象名称 ObjectName
        /// <summary>
        /// 对象名称
        /// </summary>
        [Label("对象名称")]
        public static readonly Property<string> ObjectNameProperty = P<AndonTypeTriggerPower>.Register(e => e.ObjectName);

        /// <summary>
        /// 对象名称
        /// </summary>
        public string ObjectName
        {
            get { return this.GetProperty(ObjectNameProperty); }
            set { this.SetProperty(ObjectNameProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 实体配置
    /// </summary>
    public class AndonTypeTriggerPowerConfig : EntityConfig<AndonTypeTriggerPower>
    {
        /// <summary>
        /// 元数据配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("MES_ANDONTYPE_TRIGGER_POWER").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}

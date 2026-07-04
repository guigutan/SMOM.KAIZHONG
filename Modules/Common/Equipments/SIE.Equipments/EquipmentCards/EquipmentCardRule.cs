using SIE.Domain;
using SIE.Domain.Validation;
using SIE.ManagedProperty;
using SIE.MetaModel;
using System;
using System.ComponentModel;

namespace SIE.Equipments.EquipmentCards
{
    /// <summary>
    /// 设备立卡原厂序列号重复验证
    /// </summary>
    [DisplayName("设备立卡验证规则")]
    [Description("设备立卡原厂序列号重复验证")]
    public class EquipmentCardOriginalSerialNumberRule : EntityRule<EquipmentCard>
    {
        /// <summary>
        /// 验证范围
        /// </summary>
        public EquipmentCardOriginalSerialNumberRule()
        {
            Scope = EntityStatusScopes.Add | EntityStatusScopes.Update;
        }
        /// <summary>
        /// 验证
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="e"></param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var card = entity as EquipmentCard;
            if (card.OriginalSerialNumber.IsNotEmpty() && RT.Service.Resolve<EquipmentCardController>().CheckOriginalSerialNumber(card.Id, card.OriginalSerialNumber))
            {
                e.BrokenDescription = "已存在原厂序列号为【{0}】的设备立卡！".L10nFormat(card.OriginalSerialNumber);

            }
        }
    }

    /// <summary>
    /// 设备立卡RFID重复验证
    /// </summary>
    [DisplayName("设备立卡验证规则")]
    [Description("设备立卡RFID重复验证")]
    public class EquipmentCardRfidRule : EntityRule<EquipmentCard>
    {

        /// <summary>
        /// 验证范围
        /// </summary>
        public EquipmentCardRfidRule()
        {
            Scope = EntityStatusScopes.Add | EntityStatusScopes.Update;
        }
        /// <summary>
        /// 验证
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="e"></param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var card = entity as EquipmentCard;
            if (card.Rfid.IsNotEmpty() && RT.Service.Resolve<EquipmentCardController>().CheckRfid(card.Id,card.Rfid))
            {
                e.BrokenDescription = "已存在RFID为【{0}】的设备立卡！".L10nFormat(card.Rfid);
            }
        }
    }


    /// <summary>
    /// 设备立卡使用年限验证规则
    /// </summary>
    [System.ComponentModel.DisplayName("使用年限验证规则")]
    [System.ComponentModel.Description("使用年限必须大于0")]
    public class EquipmentUsefulLifeRule : PropertyRule<EquipmentCard>
    {
        /// <summary>
        /// 验证属性
        /// </summary>
        protected override IManagedProperty Property
        {
            get
            {
                return EquipmentCard.UsefulLifeProperty;
            }
        }

        /// <summary>
        /// 验证
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="e">验证参数</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var dtl = entity as EquipmentCard;
            if (dtl != null && dtl.UsefulLife <= 0)
            {
                e.BrokenDescription = "使用年限需为正数".L10N();
            }
        }
    }

    /// <summary>
    /// 设备立卡设备名称验证规则
    /// </summary>
    [System.ComponentModel.DisplayName("设备名称非空验证规则")]
    [System.ComponentModel.Description("设备名称不能为空")]
    public class EquipmentNameRule : PropertyRule<EquipmentCard>
    {
        /// <summary>
        /// 明细非空验证
        /// </summary>
        protected override IManagedProperty Property
        {
            get
            {
                return EquipmentCard.NameProperty;
            }
        }

        /// <summary>
        /// 明细非空验证
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="e">规则</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var i = entity as EquipmentCard;
            if (i != null && i.Name.IsNullOrEmpty())
            {
                e.BrokenDescription = "设备名称不能为空".L10N();
            }
        }
    }
}

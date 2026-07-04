using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MetaModel;
using System;
using System.ComponentModel;

namespace SIE.Packages.Boxs
{
    /// <summary>
    /// 周转箱验证规则类
    /// </summary>
    [DisplayName("周转箱验证规则")]
    [Description("周转箱验证规则")]
    public class TurnoverBoxRule : EntityRule<TurnoverBox>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public TurnoverBoxRule()
        {
            Scope = MetaModel.EntityStatusScopes.Delete;
        }

        /// <summary>
        /// 验证方法
        /// </summary>
        /// <param name="entity">周转箱实体</param>
        /// <param name="e">规则参数</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var turnoverBox = entity as TurnoverBox;
            if (turnoverBox.State == BoxState.Inuse || turnoverBox.State == BoxState.Repairing)
            {
                e.BrokenDescription = "不能删除使用中维修中的周转箱".L10N();
            }
        }
    }

    /// <summary>
    /// 周转箱型号尺寸验证规则
    /// </summary>
    [DisplayName("周转箱型号尺寸验证规则")]
    [Description("周转箱型号尺寸验证规则")]
    public class TurnoverBoxModelSizeRule : EntityRule<TurnoverBoxModel>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public TurnoverBoxModelSizeRule()
        {
            Scope = MetaModel.EntityStatusScopes.Add | MetaModel.EntityStatusScopes.Update;
        }

        /// <summary>
        /// 验证方法
        /// </summary>
        /// <param name="entity">周转箱实体</param>
        /// <param name="e">规则参数</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var model = entity as TurnoverBoxModel;
            if (model.Length.HasValue && model.Length.Value <= 0)
            {
                e.BrokenDescription = "长度必须大于0".L10N();
            }
            if (model.Width.HasValue && model.Width.Value <= 0)
            {
                e.BrokenDescription = "宽度必须大于0".L10N();
            }
            if (model.Height.HasValue && model.Height.Value <= 0)
            {
                e.BrokenDescription = "高度必须大于0".L10N();
            }
        }
    }

    /// <summary>
    /// 周转工具型号被引用，不可删除
    /// </summary>
    [DisplayName("周转箱型号被引用，不可删除")]
    [Description("周转箱型号被引用，不可删除")]
    public class TurnoverModelReferencedRule : NoReferencedRule<TurnoverBoxModel>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public TurnoverModelReferencedRule()
        {
            Properties.Add(TurnoverBox.TrunoverBoxModelIdProperty);
            MessageBuilder = (o, e) =>
            {
                var entity = o as TurnoverBoxModel;
                return "型号[{0}]已经被[{1}]引用，不能删除".L10nFormat(entity.Code, "周转箱".L10N());
            };
        }
    }
}

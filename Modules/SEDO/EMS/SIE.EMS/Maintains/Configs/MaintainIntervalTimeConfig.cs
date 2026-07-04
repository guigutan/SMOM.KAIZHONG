using SIE.Common.Configs;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EMS.Maintains.Configs
{
    /// <summary>
    /// 配置
    /// </summary>
    [System.ComponentModel.DisplayName("配置保养间隔时间")]
    [System.ComponentModel.Description("配置保养间隔时间，数值类型，只能输入数字1-6，默认为天")]
    public class MaintainIntervalTimeConfig : ModuleConfig<MaintainIntervalTimeConfigValue>
    {
        /// <summary>
        /// 默认值
        /// </summary>
        readonly MaintainIntervalTimeConfigValue defaultValue = new MaintainIntervalTimeConfigValue { IntervalTime = null };

        /// <summary>
        /// 默认值
        /// </summary>
        public override MaintainIntervalTimeConfigValue DefaultValue
        {
            get { return defaultValue; }
        }
    }

    /// <summary>
    /// 配置实体
    /// </summary>
    [RootEntity, Serializable]
    [Label("设备保养间隔时间设置")]
    public class MaintainIntervalTimeConfigValue : ConfigValue
    {
        #region 是否进行保养间隔时间计算 IsIntervalTime
        /// <summary>
        /// 是否进行保养间隔时间计算
        /// </summary>
        [Label("是否进行保养间隔时间计算")]
        public static readonly Property<bool> IsIntervalTimeProperty = P<MaintainIntervalTimeConfigValue>.Register(e => e.IsIntervalTime);

        /// <summary>
        /// 是否进行保养间隔时间计算
        /// </summary>
        public bool IsIntervalTime
        {
            get { return this.GetProperty(IsIntervalTimeProperty); }
            set { this.SetProperty(IsIntervalTimeProperty, value); }
        }
        #endregion


        #region 保养间隔时间(天) IntervalTime
        /// <summary>
        /// 保养间隔时间(天)
        /// </summary>
        [Label("保养间隔时间(天)")]
        [MinValue(1)]
        [MaxValue(6)]
        public static readonly Property<int?> IntervalTimeProperty = P<MaintainIntervalTimeConfigValue>.Register(e => e.IntervalTime);

        /// <summary>
        /// 保养间隔时间(天)
        /// </summary>
        public int? IntervalTime
        {
            get { return this.GetProperty(IntervalTimeProperty); }
            set { this.SetProperty(IntervalTimeProperty, value); }
        }
        #endregion

        /// <summary>
        ///  显示值
        /// </summary>
        /// <returns>string</returns>
        public override string Display()
        {
            return IntervalTime == null ? "" : (IntervalTime.ToString() + ";");
        }
    }

    /// <summary>
    /// 保存前事件
    /// </summary>
    [System.ComponentModel.DisplayName("保养间隔时间配置提交前事件")]
    [System.ComponentModel.Description("保养间隔时间配置提交前验证事件")]
    public class MaintainIntervalTimeConfigValueSubmitting : OnSubmitting<MaintainIntervalTimeConfigValue>
    {
        /// <summary>
        /// 调用
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="e"></param>
        protected override void Invoke(MaintainIntervalTimeConfigValue entity, EntitySubmittingEventArgs e)
        {
            if (e.Action == SubmitAction.Insert || e.Action == SubmitAction.Update)
            {
                if (entity.IsIntervalTime && (entity.IntervalTime < 1 || entity.IntervalTime > 6))
                {
                    throw new ValidationException("保养间隔时间(天)只能维护1-6的值".L10N());
                }

            }
        }
    }
}

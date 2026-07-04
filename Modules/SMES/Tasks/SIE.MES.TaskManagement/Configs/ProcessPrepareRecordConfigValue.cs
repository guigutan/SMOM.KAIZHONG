using SIE.Common.Configs;
using SIE.Common.NumberRules;
using SIE.Common.Prints;
using SIE.Domain;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.MES.TaskManagement.Configs
{
    /// <summary>
    /// 工序产前准备配置值
    /// </summary>
    [RootEntity, Serializable]
    [Label("工序产前准备配置值")]
    public class ProcessPrepareRecordConfigValue : ConfigValue 
    {
        #region 是否校验工序产前准备 IsValidateProcessPrepare
        /// <summary>
        /// 是否校验工序产前准备
        /// </summary>
        [Label("是否校验工序产前准备")]
        public static readonly Property<bool> IsValidateProcessPrepareProperty = P<ProcessPrepareRecordConfigValue>.Register(e => e.IsValidateProcessPrepare);

        /// <summary>
        /// 是否校验工序产前准备
        /// </summary>
        public bool IsValidateProcessPrepare
        {
            get { return this.GetProperty(IsValidateProcessPrepareProperty); }
            set { this.SetProperty(IsValidateProcessPrepareProperty, value); }
        }
        #endregion




        /// <summary>
        /// 显示
        /// </summary>
        /// <returns>编码规则名称|打印模板名称</returns>
        public override string Display()
        {
            return "是否校验工序产前准备:{0} ".L10nFormat(IsValidateProcessPrepare);
        }
    }
}

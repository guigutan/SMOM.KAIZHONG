using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using SIE.Domain;
using SIE.Common.Platform;
using SIE.Mda.Metadatas.Modules;

namespace SIE.WorkBenchCommon.Workbench.Base
{
    /// <summary>
    /// 文档提交后事件
    /// </summary>
    [DisplayName("工作台同步模块")]
    [Description("用户册除工作台提交后，同时删除模块定义(为菜单服务)")]
    public class WorkbenchDefinitiontOnSubmitted : OnSubmitted<WorkbenchDefinition>
    {
        /// <summary>
        /// 文档提交后事件
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="e"></param>
        protected override void Invoke(WorkbenchDefinition entity, EntitySubmittedEventArgs e)
        {
            var moduleKey = "{0}{1}".FormatArgs(WorkbenchController.ModuleKeyLabel, entity.Code);
            #region 同步至模块定义
        
           
            if (e.Action == SubmitAction.Delete)
            {
                var moduleConfig = RT.Service.Resolve<ModuleConfigController>().GetModuleConfig(moduleKey);
                if (moduleConfig != null)
                    moduleConfig.PersistenceStatus = PersistenceStatus.Deleted;

                if (moduleConfig != null && moduleConfig.IsDirty)
                    RF.Save(moduleConfig); //一个模块提交一次。
            }

          
            #endregion
        }
    }
}

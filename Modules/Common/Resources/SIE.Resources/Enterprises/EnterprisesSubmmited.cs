using SIE.Domain;
using SIE.Resources.WipResources;

namespace SIE.Resources.Enterprises
{
    /// <summary>
    /// 企业模型提交后事件
    /// </summary>
    [System.ComponentModel.DisplayName("企业模型保存后逻辑")]
    [System.ComponentModel.Description("根据企业模型是否为资源，将同步到生产资源的数据进行状态更新")]
    public class EnterprisesSubmmited : OnSubmitted<Enterprise>
    {
        /// <summary>
        /// 提交后事件
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="e">e</param>
        protected override void Invoke(Enterprise entity, EntitySubmittedEventArgs e)
        {
            if (e.Action == SubmitAction.Update)
            {
                var ctl = RT.Service.Resolve<WipResourceController>();
                if (entity.IsResource)
                {
                    ctl.UpdateSchResourseState(entity.Id, SyncSourceType.Enterprise, ResourceState.Unused);
                }
                else
                {
                    ctl.UpdateSchResourseState(entity.Id, SyncSourceType.Enterprise, ResourceState.Diseffect);
                }
            }
        }
    }
}

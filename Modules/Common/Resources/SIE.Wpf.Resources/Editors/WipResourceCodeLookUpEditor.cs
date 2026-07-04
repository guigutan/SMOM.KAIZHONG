using DevExpress.Xpf.Grid.LookUp;
using SIE.Domain;
using SIE.ManagedProperty;
using SIE.MetaModel.View;
using SIE.Resources.WipResources;
using SIE.Wpf.Editors;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace SIE.Wpf.Resources.Editors
{
    /// <summary>
    /// 生产资源编号编辑器
    /// </summary>
    public class WipResourceCodeLookUpEditor : LookUpEditor
    {
        /// <summary>
        /// 编辑器名称
        /// </summary>
        public const string EditorName = "WipResourceCodeLookUpEditor";

        /// <summary>
        /// 绑定属性值
        /// </summary>
        /// <returns>托管属性</returns>
        protected override DependencyProperty BindingProperty()
        {
            return LookUpEdit.EditValueProperty;
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="propertyViewMeta">属性元数据</param>
        public override void Initialize(WPFEntityPropertyViewMeta propertyViewMeta)
        {
            propertyViewMeta.SelectionViewMeta = new SelectionViewMeta();
            propertyViewMeta.SelectionViewMeta.SelectionEntityType = typeof(WipResource);
            propertyViewMeta.SelectionViewMeta.DisplayMemberPath = WipResource.NameProperty;
            propertyViewMeta.SelectionViewMeta.SelectedValuePath = WipResource.CodeProperty;
            base.Initialize(propertyViewMeta);
        }

        /// <summary>
        /// 设置数据源
        /// </summary>
        /// <param name="source">数据源对象</param>
        /// <param name="keyword">关键词</param>
        /// <param name="titleProperty">标题信息</param>
        /// <returns>实体列表</returns>
        protected override EntityList GetDataSourceCore(Entity source, string keyword, IManagedProperty titleProperty)
        {
            var sourceList = new EntityList<WipResource>();
            var stateList = new List<ResourceState>() { ResourceState.Actived, ResourceState.Stop, ResourceState.Unused };
            var entitys = RT.Service.Resolve<WipResourceController>().GetWipResources(stateList, null, null, keyword);
            if (entitys != null && entitys.Count > 0)
                sourceList.AddRange(entitys.OrderByDescending(p => p.Code));
            return sourceList;
        }
    }
}

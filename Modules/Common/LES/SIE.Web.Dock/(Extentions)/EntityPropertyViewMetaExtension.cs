using SIE.Dock.DockAppoints;
using SIE.Dock.DockAppoints.Service;
using SIE.Dock.ViewModels;
using SIE.Domain;
using SIE.MetaModel.View;
using SIE.Web.ClientMetaModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.Dock
{
    /// <summary>
    /// 实体属性视图元数据扩展
    /// </summary>
    public static class EntityPropertyViewMetaExtension
    {
        /// <summary>
        /// 预约月台编辑器，用于返回预约月台字符串
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <param name="meta">meta</param>
        /// <param name="action">action</param>
        /// <returns>预约月台编辑器</returns>
        public static WebEntityPropertyViewMeta<T> UseSelectAppointDockEditor<T>(this WebEntityPropertyViewMeta<T> meta, Action<PagingLookUpBaseConfig, T> action = null) where T : new()
        {
            meta.ViewMeta.SelectionViewMeta = new SelectionViewMeta();
            meta.ViewMeta.SelectionViewMeta.SelectionEntityType = typeof(SelectAppointDockViewModel);
            meta.ViewMeta.SelectionViewMeta.DisplayMemberPath = SelectAppointDockViewModel.AppointTimeDisplayProperty;
            meta.ViewMeta.SelectionViewMeta.SelectedValuePath = SelectAppointDockViewModel.AppointTimeDisplayProperty;
            meta.UseDataSource((source, pagingInfo, keyword) =>
            {
                var dockAppoint = source as DockAppoint;
                if (dockAppoint == null || dockAppoint.YardZoneId <= 0)
                {
                    return new EntityList<SelectAppointDockViewModel>();
                }

                var appointDate = dockAppoint.AppointDate.AddHours(8);

                return RT.Service.Resolve<DockAppointService>().GetSelectAppointDockViewModels(appointDate, dockAppoint.YardZoneId, dockAppoint.AppointType, keyword, pagingInfo);

            }).UsePagingLookUpEditor(action);

            return meta;
        }

        /// <summary>
        /// 预计可用时间编辑器，用于返回预计可用时间
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <param name="meta">meta</param>
        /// <param name="action">action</param>
        /// <returns>预约月台编辑器</returns>
        public static WebEntityPropertyViewMeta<T> UseSelectUseHoursEditor<T>(this WebEntityPropertyViewMeta<T> meta, Action<SelectUseHoursConfig> action = null)
        {
            meta.ViewMeta.EditorName = WebEditorNames.ComboBox;
            var config = new SelectUseHoursConfig();
            meta.ViewMeta.Config = config;
            action?.Invoke(config);
            return meta;
        }
    }
}

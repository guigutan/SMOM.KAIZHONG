using SIE.Common.Configs;
using SIE.Dock.Datas;
using SIE.Dock.DockAppoints;
using SIE.Dock.DockAppoints.Configs;
using SIE.Dock.DockAppoints.Service;

namespace SIE.Web.Dock.DockAppoints.DataQueryer
{
    /// <summary>
    /// 月台预约查询器
    /// </summary>
    public class DockAppointDataQueryer : Data.DataQueryer
    {
        /// <summary>
        /// 获取配置的模板的值
        /// </summary>
        /// <returns>模板数据</returns>
        public DefaultTemplateData GetDockAppointDefaultTemplate()
        {
            var config = ConfigService.GetConfig(new DockAppointNoConfig(), typeof(DockAppoint));

            DefaultTemplateData rst = new DefaultTemplateData()
            {
                TemplateId = config?.PrintTemplateId ?? 0,
                TemplateFileName = config?.PrintTemplate?.FileName,
            };

            return rst;
        }

        /// <summary>
        /// 保存月台预约数据
        /// </summary>
        /// <param name="data">月台预约数据</param>
        public void SaveDockAppointData(DockAppoint data)
        {
            RT.Service.Resolve<DockAppointService>().SaveDockAppointDatas(data);
        }

        /// <summary>
        /// 更新月台预约数据
        /// </summary>
        /// <param name="data">月台预约数据</param>
        public void UpdateDockAppointData(DockAppoint data)
        {
            RT.Service.Resolve<DockAppointService>().UpdateDockAppointDatas(data);
        }
    }
}
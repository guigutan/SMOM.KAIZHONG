using SIE.Dock.Datas;
using SIE.Dock.DockQueues.Dao;
using SIE.Dock.DockQueues;
using SIE.Dock.DockQueues.Service;

namespace SIE.Web.Dock.DockQueues.DataQueryer
{
    /// <summary>
    /// 月台排队查询器
    /// </summary>
    public class DockQueueDataQueryer : Data.DataQueryer
    {
        /// <summary>
        /// 获取配置的模板的值
        /// </summary>
        /// <returns>模板数据</returns>
        public DefaultTemplateData GetDockQueueDefaultTemplate()
        {
            var config = RT.Service.Resolve<DockQueueDao>().GetDockQueueNumberRule();

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
        public void SaveDockQueueData(DockQueue data)
        {
            RT.Service.Resolve<DockQueueService>().SaveDockQueueDatas(data);
        }
    }
}
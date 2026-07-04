using Newtonsoft.Json;
using SIE.Traces.ReverseTraces;

namespace SIE.Web.Traces.ReverseTraces
{
    /// <summary>
    /// 反向追溯-工序采集视图配置
    /// </summary>
    internal class MesProcessCollectViewModelViewConfig : WebViewConfig<MesProcessCollectViewModel>
    {


        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.DisableEditing();
            View.ClearCommands();
            View.UseChildrenAsHorizontal();
            View.Property(p => p.CollectSn).ShowInList(width: 200).DisableSort();
            View.Property(p => p.StateName).ShowInList(width: 200).DisableSort();
            View.Property(p => p.ProcessName).ShowInList(width: 200).DisableSort();
            View.Property(p => p.StationName).ShowInList(width: 200).DisableSort();
            View.Property(p => p.ResourceName).ShowInList(width: 200).DisableSort();
            View.Property(p => p.Result).ShowInList(width: 200).DisableSort();
            View.Property(p => p.CollectTime).ShowInList(width: 200).DisableSort();
            View.Property(p => p.CollectBy).ShowInList(width: 200).DisableSort();

            View.AttachChildrenProperty(typeof(MesProcessCollectKeyItemViewModel), o =>
            {
                var args = o as ChildPagingDataWithParentEntityArgs;
                MesProcessCollectViewModel parent = JsonConvert.DeserializeObject<MesProcessCollectViewModel>(args.ParentEntity);
                var result = RT.Service.Resolve<ReverseTraceController>().GetMesProcessCollectKeyItemDatas(parent, args.PagingInfo);
                return result;
            }, MesProcessCollectKeyItemViewModelViewConfig.ListView);

         
        }
    }
}
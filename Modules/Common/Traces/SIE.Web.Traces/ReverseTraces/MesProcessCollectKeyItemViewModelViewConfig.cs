using Newtonsoft.Json;
using SIE.Traces.ForwardTraces;
using SIE.Traces.ReverseTraces;
using SIE.Web.Traces.ForwardTraces;

namespace SIE.Web.Traces.ReverseTraces
{
    /// <summary>
    /// 反向追溯-工序采集-关键件视图配置
    /// </summary>
    internal class MesProcessCollectKeyItemViewModelViewConfig : WebViewConfig<MesProcessCollectKeyItemViewModel>
    {


        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.WithoutPaging();
            View.UseLayoutSize(0.6,0.4);
            View.DisableEditing();
            View.ClearCommands();
            View.Property(p => p.ItemCode).ShowInList(width: 200).DisableSort();
            View.Property(p => p.ItemName).ShowInList(width: 200).DisableSort();
            View.Property(p => p.SourceCode).ShowInList(width: 200).DisableSort();
            View.Property(p => p.Qty).ShowInList().DisableSort();
            View.Property(p => p.ItemExtPropName).ShowInList(width: 200).DisableSort();
            View.AttachChildrenProperty(typeof(KeyItemWmsViewModel), o =>
            {
                var args = o as ChildPagingDataWithParentEntityArgs;
                MesProcessCollectKeyItemViewModel parent = JsonConvert.DeserializeObject<MesProcessCollectKeyItemViewModel>(args.ParentEntity);
                var result = RT.Service.Resolve<ReverseTraceController>().GetKeyItemDatas(parent, args.PagingInfo);
                return result;
            }, KeyItemWmsViewModelViewConfig.ListView);
        }
    }
}
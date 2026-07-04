using SIE.Domain;
using SIE.Domain.Query;
using SIE.EventMessages.Tech.Processs;
using SIE.EventMessages.Tech.Stations;
using SIE.Modules;
using SIE.Tech;
using SIE.Tech.Processs;
using SIE.Tech.Stations;

[assembly: Module(typeof(Module))]

namespace SIE.Tech
{
    /// <summary>
    /// 工艺模块
    /// </summary>
    class Module : DomainModule
    {
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="app">应用程序</param>
        public override void Initialize(IApp app)
        {
            RepositoryDataProvider.Querying += RepositoryDataProvider_Querying;
            RT.Service.Register<IProcess, ProcessController>();
            RT.Service.Register<IStation, StationController>();
        }

        /// <summary>
        /// 查询前排序
        /// </summary>
        /// <param name="sender">数据提供者</param>
        /// <param name="e">参数</param>
        private void RepositoryDataProvider_Querying(object sender, QueryingEventArgs e)
        {
            var provider = sender as TechEntityDataProvider;
            if (provider != null && provider.Repository.EntityType == typeof(ProcessCollectStep))
            {
                var order = QueryFactory.Instance.OrderBy(e.Args.Query.MainTable.FindColumn(ProcessCollectStep.StepProperty));
                e.Args.Query.OrderBy.Clear();
                e.Args.Query.OrderBy.Add(order);
            }
        }
    }
}
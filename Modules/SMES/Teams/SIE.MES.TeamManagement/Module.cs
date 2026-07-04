using SIE.Domain;
using SIE.Domain.Query;
using SIE.MES.TeamManagement;
using SIE.MES.TeamManagement.ClockingIns;
using SIE.MES.TeamManagement.SikllAuthentications;
using SIE.Modules;

[assembly: Module(typeof(Module))]

namespace SIE.MES.TeamManagement
{
    /// <summary>
    /// 模块配置
    /// </summary>
    public class Module : DomainModule
    {
        /// <summary>
        /// 模块初始化
        /// </summary>
        /// <param name="app">应用程序</param>
        public override void Initialize(IApp app)
        {
            RepositoryDataProvider.Querying += RepositoryDataProvider_Querying;
        }

        /// <summary>
        /// 查询前排序
        /// </summary>
        /// <param name="sender">数据提供者</param>
        /// <param name="e">参数</param>
        private void RepositoryDataProvider_Querying(object sender, QueryingEventArgs e)
        {
            var provider = sender as TeamManagementDataProvider;
            if (provider == null)
                return;

            if (provider.Repository.EntityType == typeof(ClockInDetail))
            {
                var s = e.Args.Query.MainTable.FindColumn(ClockInDetail.ClockInDateProperty);
                AddOrderBy(e.Args.Query, s);
            }

            if (provider.Repository.EntityType == typeof(TrainingRecord))
            {
                var s = e.Args.Query.MainTable.FindColumn(TrainingRecord.IsHistoryProperty);
                AddOrderBy(e.Args.Query, s);
            }

            if (provider.Repository.EntityType == typeof(ExamResult))
            {
                var s = e.Args.Query.MainTable.FindColumn(ExamResult.IsHistoryProperty);
                AddOrderBy(e.Args.Query, s);
            }

            if (provider.Repository.EntityType == typeof(OperationRecord))
            {
                var s = e.Args.Query.MainTable.FindColumn(OperationRecord.IsHistoryProperty);
                AddOrderBy(e.Args.Query, s);
            }
        }

        /// <summary>
        /// 添加排序
        /// </summary>
        /// <param name="query">查询器</param>
        /// <param name="column">排序类</param>
        private void AddOrderBy(IQuery query, IColumnNode column)
        {
            if (column == null)
                return;
            query.OrderBy.Clear();
            query.OrderBy.Add(QueryFactory.Instance.OrderBy(column));
        }
    }
}
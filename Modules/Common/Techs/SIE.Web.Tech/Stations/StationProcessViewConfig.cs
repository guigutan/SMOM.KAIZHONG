using SIE.Domain;
using SIE.MetaModel.View;
using SIE.Tech.Processs;
using SIE.Tech.Stations;
using SIE.Web.Common.Import.Commands;
using SIE.Web.Tech.Common.Commands;
using SIE.Web.Tech.Stations.Commands;
using System.Collections.Generic;

namespace SIE.Web.Tech.Stations
{
    /// <summary>
    /// 工位工序视图配置
    /// </summary>
    internal class StationProcessViewConfig : WebViewConfig<StationProcess>
    {
        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.AssignAuthorize(typeof(Station));
            View.UseCommands(WebCommandNames.Add, WebCommandNames.Edit, WebCommandNames.Delete,
                WebCommandNames.ExportXls, WebCommandNames.ExportXlsSelection, WebCommandNames.ExportXlsAll);
            View.UseCommands(typeof(ImportExcelCommand).FullName, "SIE.Web.Common.Import.Commands.DownloadTemplateCommand");
            View.Property(p => p.ProcessId).ShowInList(150).UsePagingLookUpEditor((m, e) =>
            {
                m.ReloadDataOnPopping = true;
            }).Cascade(p => p.WorkStepId, null);
            View.Property(p => p.WorkStepId).ShowInList(150)
                .UseDataSource((t, p, s) =>
                {
                    var stationProcess = t as StationProcess;
                    if (stationProcess == null)
                    {
                        return new EntityList<WorkStep>();
                    }
                    return RT.Service.Resolve<SIE.Tech.Processs.ProcessController>().GetWorkSteps(stationProcess.ProcessId, s, p);
                }).UsePagingLookUpEditor();
        }

        /// <summary>
        /// 配置导入视图
        /// </summary>
        protected override void ConfigImportView()
        {
            View.PropertyRef(p => p.Station.Code).ImportIndexer(true).HasLabel("工位编码");
            View.PropertyRef(p => p.Process.Code).ImportIndexer(true).HasLabel("工序编码");
            View.PropertyRef(p => p.WorkStep.Code).HasLabel("工步编码");
        }
    }
}

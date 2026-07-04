using SIE.Common.Import;
using SIE.Core.Barcodes;
using SIE.Tech.Processs;
using SIE.Tech.Processs.Event;
using SIE.Web.Common.Import.Commands;
using System.Linq;

namespace SIE.Web.Tech.Processs.Commands
{
    /// <summary>
    /// 导入工序
    /// </summary>
    public class ProcessImportCommand : ImportExcelCommand
    {
        /// <summary>
        /// 行数据读取, 默认按视图配置把数据读取后，还可以进行自定义处理
        /// </summary>
        /// <param name="data">行数据</param>
        /// <param name="cache">缓存数据</param>
        protected override void OnRowDataRead(RowData data, CacheData cache)
        {
            base.OnRowDataRead(data, cache);
            var process = data.Entity as Process;
            var processType = process.Type;
            process.Type = null;
            process.PropertyChanged += new ProcessPropertyChanged().PropertyChanged;
            process.Type = processType;

            if ((process.Type == ProcessType.BatchAssembly || process.Type == ProcessType.BatchFix || process.Type == ProcessType.BatchPacking || process.Type == ProcessType.BatchPqc))
            {
                if (!process.CollectStepList.Any(p => p.PlugType == PlugType.In))
                {
                    var step = new ProcessCollectStep()
                    {
                        BarcodeType = BarcodeType.BatchBarocde,
                        Step = 1,
                        Process = process,
                        PlugType = PlugType.In
                    };
                    process.CollectStepList.Add(step);
                }
                if (!process.CollectStepList.Any(p => p.PlugType == PlugType.Out))
                    {
                    var stepOut = new ProcessCollectStep()
                    {
                        BarcodeType = BarcodeType.BatchBarocde,
                        Step = 2,
                        Process = process,
                        PlugType = PlugType.Out
                    };
                    process.CollectStepList.Add(stepOut);
                }
            }
            else
            {
                var step = new ProcessCollectStep()
                {
                    BarcodeType = BarcodeType.SN,
                    Step = 1,
                    Process = process
                };
                process.CollectStepList.Add(step);
            }
        }
    }
}

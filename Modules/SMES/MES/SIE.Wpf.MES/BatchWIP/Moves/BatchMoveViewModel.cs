using SIE.Common.Configs;
using SIE.ManagedProperty;
using SIE.MES.BatchWIP.Assemlys;
using SIE.MES.WIP;
using SIE.MES.WIP.Configs;
using SIE.ObjectModel;
using SIE.Tech.Processs;
using System;

namespace SIE.Wpf.MES.BatchWIP.Moves
{
    /// <summary>
    /// 批次过站采集视图模型
    /// </summary>
    [RootEntity]
    [EntityWithConfig(typeof(DevicePortConfig))]
    [EntityWithConfig(typeof(SerialPortsConfig))]
    [Label("批次过站采集")]
    public class BatchMoveViewModel : BatchDataCollectionViewModel<BatchAssemblyController>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public BatchMoveViewModel()
        {
            InitWorkstation(ProcessType.BatchAssembly);
        }

        /// <summary>
        /// 入站条码扫完后处理逻辑
        /// </summary>
        /// <param name="e">属性变更参数</param>
        protected override void OnBarcodeChanged(ManagedPropertyChangedEventArgs e)
        {
            if (Barcode.IsNullOrEmpty()) return;
            try
            {
                ClearInfos();
                if (IsReceiveContainer)
                    RemoveInputBatch(InputBatch, Barcode);
                else
                {
                    if (IsMoveIn)
                        MoveIn();
                    else
                        MoveOut();
                }
            }
            catch (Exception exc)
            {
                ShowError(exc);
                if (IsReceiveContainer)
                    ShowTips("请扫描移除关联载具".L10N());
            }
            finally
            {
                Barcode = null;
            }
        }

        /// <summary>
        /// 转入批次
        /// </summary>
        private void MoveIn()
        {
            var workcell = GetWorkcell();
            var collectStep = Step.InputCollectStep;
            var collectBarcode = new CollectBarcode { Code = Barcode, Type = collectStep.BarcodeType };
            MoveIn(collectBarcode, workcell);
            ShowTips("[{0}:{1}]成功转入，请扫描{2}".L10nFormat(collectBarcode.Type.ToLabel().L10N(), Barcode, collectBarcode.Type.ToLabel().L10N()));
        }

        /// <summary>
        /// 生成转出批次
        /// </summary>
        void MoveOut()
        {
            GetWorkcell();
            var collectStep = Step.OutputCollectStep;
            GenerateOutputBatch(Barcode, collectStep.BarcodeType);
        }
    }
}
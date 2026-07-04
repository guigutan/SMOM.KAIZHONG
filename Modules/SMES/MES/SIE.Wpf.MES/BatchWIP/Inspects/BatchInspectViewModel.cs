using SIE.Common;
using SIE.Common.Configs;
using SIE.Core.Barcodes;
using SIE.Defects;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.ManagedProperty;
using SIE.MES.BatchWIP;
using SIE.MES.BatchWIP.Inspects;
using SIE.MES.WIP;
using SIE.MES.WIP.Configs;
using SIE.MES.WIP.Products;
using SIE.ObjectModel;
using SIE.Tech.Processs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Wpf.MES.BatchWIP.Inspects
{
    /// <summary>
    /// 批次检验采集视图模型
    /// </summary>
    [RootEntity, Serializable]
    [EntityWithConfig(typeof(DevicePortConfig))]
    [EntityWithConfig(typeof(SerialPortsConfig))]
    [Label("批次检验采集")]
    public class BatchInspectViewModel : BatchDataCollectionViewModel<BatchInspectController>
    {
        /// <summary>
        /// 批次检验采集视图构造函数
        /// </summary>
        public BatchInspectViewModel()
        {
            InitWorkstation(ProcessType.BatchPqc);
            BatchDefectSetVmdl = new BatchDefectiveSetViewModel();
        }

        #region 批次检验不良集合 BatchDefectSetVmdl
        /// <summary>
        /// 批次检验不良集合
        /// </summary>
        [Label("批次检验不良集合")]
        public static readonly Property<BatchDefectiveSetViewModel> BatchDefectSetVmdlProperty = P<BatchInspectViewModel>.Register(e => e.BatchDefectSetVmdl);

        /// <summary>
        /// 批次检验不良集合
        /// </summary>
        public BatchDefectiveSetViewModel BatchDefectSetVmdl
        {
            get { return this.GetProperty(BatchDefectSetVmdlProperty); }
            set { this.SetProperty(BatchDefectSetVmdlProperty, value); }
        }
        #endregion        

        /// <summary>
        /// 加载工位信息
        /// </summary>
        protected override void LoadWorkstationData()
        {
            base.LoadWorkstationData();
            try
            {
                GetWorkcell();
            }
            catch (Exception exc)
            {
                ShowError(exc);
            }
        }

        /// <summary>
        /// 入站条码扫完后处理逻辑
        /// </summary>
        /// <param name="e">属性变更参数</param>
        protected override void OnBarcodeChanged(ManagedPropertyChangedEventArgs e)
        {
            if (Barcode.IsNullOrEmpty())
            {
                return;
            }
            try
            {
                ClearInfos();
                if (IsReceiveContainer)
                {
                    RemoveInputBatch(InputBatch, Barcode);
                }
                else
                {
                    if (IsMoveIn)
                    {
                        MoveIn();
                    }
                    else
                    {
                        MoveOut();
                    }
                }
            }
            catch (Exception exc)
            {
                ShowError(exc);
                if (IsReceiveContainer)
                {
                    ShowTips("请扫描移除关联载具".L10N());
                }
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
            ////RefreshInputBatch();
            ShowTips("[{0}:{1}]成功转入，请扫描{2}".L10nFormat(collectBarcode.Type.ToLabel().L10N(), Barcode, collectBarcode.Type.ToLabel().L10N()));
        }

        /// <summary>
        /// 生成转出批次
        /// </summary>
        internal void MoveOut()
        {
            GetWorkcell();
            var collectStep = Step.OutputCollectStep;
            GenerateOutputBatch(Barcode, collectStep.BarcodeType);
        }

        /// <summary>
        /// 转出批次的校验
        /// </summary>
        /// <param name="barcode">转出条码</param>
        /// <param name="type">条码类型</param>
        internal override void ValidateOutBarcode(string barcode, BarcodeType type)
        {
            ////base.ValidateOutBarcode(barcode, type);
            if (InputBatchList.Count <= 0)
            {
                throw new ValidationException("生成转出批次失败，转入批次为空，请先转入".L10N());
            }
            var normalOupputBatchs = OutputBatchList.Where(x => !x.IsNg).ToList(); //去除出站批次中不良子批次的计数
            if (normalOupputBatchs.Count >= 1)
            {
                var output = normalOupputBatchs[0];
                var code = output.ContainerNo.IsNullOrEmpty() ? output.BatchNo : output.ContainerNo;
                code = output.SubBatchNo.IsNullOrEmpty() ? code : output.SubBatchNo;
                throw new ValidationException("已存在[转出批次：{0}]，请先转出！".L10nFormat(code));
            }
        }

        /// <summary>
        /// 转出选中批次
        /// </summary>
        /// <param name="batch">当前选中的转出批次</param>
        internal override void BatchOutput(OutputBatch batch)
        {
            if (BatchDefectSetVmdl.BatchDefectiveViewModels.Count > 0)
            {
                throw new ValidationException("存在不良批次，请先转出不良批次".L10N());
            }
            if (batch.IsNg)
            {
                batch.BarcodeType = Step.OutputCollectStep.BarcodeType;
            }
            base.BatchOutput(batch);
        }

        /// <summary>
        /// 转出检验
        /// </summary>
        /// <param name="batch">当前转出对象</param>
        internal override void BatchOutputValidation(OutputBatch batch)
        {
            ////base.BatchOutputValidation(batch);
            var splitQty = InputBatchList.Sum(p => p.SplitQty);
            if (splitQty <= 0 && !batch.IsNg)
            {
                throw new ValidationException("转出批次未关联批次，请在转入批次输入拆分数量".L10N());
            }
            if (batch.RelationBatchList.Count == 0)
            {
                throw new ValidationException("转出批次未关联批次，请在转入批次输入拆分数量".L10N());
            }
            if (splitQty != batch.RelationBatchList.Sum(p => p.Qty) && !batch.IsNg)
            {
                throw new ValidationException("拆分数量与转出批次数量不一致".L10N());
            }
        }

        /// <summary>
        /// 移除转出批次重写方法
        /// </summary>
        /// <param name="outputBatch">转出批次</param>
        internal override void RemoveOutBatch(OutputBatch outputBatch)
        {
            base.RemoveOutBatch(outputBatch);
        }

        /// <summary>
        /// 初始化采集数据
        /// </summary>
        /// <param name="collectData">采集结果</param>
        protected override void InitializedCollectData(CollectData collectData)
        {
            if(collectData == null)
            {
                return;
            }
            base.InitializedCollectData(collectData);
            var curOutputBatch = collectData.OutputBatch;
            if (curOutputBatch.IsNg)  ////不良品
            {
                collectData.NgQty = curOutputBatch.Qty;
                collectData.Result = ResultType.Fail;
                collectData.Grade = null;
                if (curOutputBatch.RelationBatchList.Count > 0)
                {
                    curOutputBatch.RelationBatchList[0].InputBatch.DefectList.ForEach(item =>
                    {
                        collectData.Defects.Add(new Defects.DefectData
                        {
                            DefectId = item.Defect.Id,
                            DefectName = item.Defect.Description,
                            CategoryId = item.Defect.DefectCategoryId,
                            CategoryName = item.Defect.DefectCategory?.Description,
                            Qty = (double)curOutputBatch.Qty
                        });
                    });
                }
                
            }
            else ////良品
            {
                if (curOutputBatch.Grade != null) ////良品-等级已设置
                {
                    collectData.NgQty = 0;
                    collectData.Result = ResultType.Custom;
                    collectData.Grade = GetEnumGrade(curOutputBatch.Grade.Code);
                }
                else ////良品-等级未设置
                {
                    collectData.NgQty = 0;
                    collectData.Result = ResultType.Pass;
                    collectData.Grade = null;
                }
            }
        }

        /// <summary>
        /// 刷新转入批次列表
        /// </summary>
        /// <param name="outputBatch">当前转出批次</param>
        internal override void RefreshInputBatch(OutputBatch outputBatch = null)
        {
            base.RefreshInputBatch(outputBatch);
            if (outputBatch == null)
            {
                return;
            }
            ////移除需要执行的代码 (先从出站集合中移除对象, 再刷新入站集合)
            if (!OutputBatchList.Any(x => x.BatchNo == outputBatch.BatchNo && x.SubBatchNo == outputBatch.SubBatchNo))
            {
                RefreshInputBatchRemoveOutputBatch(outputBatch);
            }
            else ////转出执行的代码 (先刷新入站集合, 再从出站集合中移除对象)
            {
                RefreshInputBatchMoveOutputBatch(outputBatch);
            }
        }

        /// <summary>
        /// 修改正常子批次对应的Input对象
        /// </summary>
        private void RefreshInputBatchByNormalOutputBatch()
        {
            var normalOutputBatch = OutputBatchList.FirstOrDefault(x => !x.IsNg);
            if (normalOutputBatch == null)
            {
                return;
            }
            decimal normalOutputBatchQty = normalOutputBatch.Qty;
            var normalInputBatchs = InputBatchList.Where(x => x.BatchNo == normalOutputBatch?.BatchNo).OrderBy(x => x.InputDate).ToList();
            foreach (var curInutBatch in normalInputBatchs)
            {
                if (normalOutputBatchQty <= curInutBatch.RemainQty)
                {
                    curInutBatch.SplitQty = normalOutputBatchQty;
                }
                else
                {
                    curInutBatch.SplitQty = curInutBatch.RemainQty;
                }
                normalOutputBatchQty -= curInutBatch.SplitQty;
            }
        }

        /// <summary>
        /// 修改其它不良子批次对应的Input对象
        /// </summary>
        /// <param name="outputBatch">当前转出批次</param>
        private void RefreshInputBatchByOtherNgOutputBatch(OutputBatch outputBatch)
        {
            ////修改其它不良子批次对应的Input对象
            var inputs = new List<InputBatch>();
            var ngOutputBatchs = OutputBatchList.Where(x => x.IsNg&& x.BatchNo != outputBatch.BatchNo && x.SubBatchNo != outputBatch.SubBatchNo).ToList();
            foreach (var ngOutBat in ngOutputBatchs)
            {
                foreach (var relationBatch in ngOutBat.RelationBatchList)
                {
                    InputBatch curOldInputBatch = relationBatch.InputBatch;
                    var curNewInputBatch = InputBatchList.FirstOrDefault(x => x.Id == curOldInputBatch.Id);
                    if (curNewInputBatch != null)
                    {
                        curNewInputBatch.NgQty += relationBatch.Qty;
                        if (!inputs.Contains(curNewInputBatch))
                        {
                            inputs.Add(curNewInputBatch);
                        }
                    }
                }
            }

            foreach (var item in inputs)
            {
                item.RemainQty -= item.NgQty;
            }

            inputs.Clear();
        }

        /// <summary>
        /// 修改所有不良子批次对应的Input对象
        /// </summary>
        private void RefreshInputBatchByAllNgOutputBatch()
        {
            var ngOutputBatchs = OutputBatchList.Where(x => x.IsNg).ToList();
            var inputs = new List<InputBatch>();
            foreach (var ngOutBat in ngOutputBatchs)
            {
                foreach (var relationBatch in ngOutBat.RelationBatchList)
                {
                    InputBatch curOldInputBatch = relationBatch.InputBatch;
                    var curNewInputBatch = InputBatchList.FirstOrDefault(x => x.Id == curOldInputBatch.Id);
                    if (curNewInputBatch != null)
                    {
                        curNewInputBatch.NgQty += relationBatch.Qty;
                        if (!inputs.Contains(curNewInputBatch))
                        {
                            inputs.Add(curNewInputBatch);
                        }
                    }
                }
            }

            foreach (var item in inputs)
            {
                item.RemainQty -= item.NgQty;
            }

            inputs.Clear();
        }

        /// <summary>
        /// 修改批次检验不良记录对应的Input对象
        /// </summary>
        private void RefreshInutBatchByBatchDefectiveViewModels()
        {
            var inputs = new List<InputBatch>();
            var batchDefectiveVmdls = BatchDefectSetVmdl.BatchDefectiveViewModels;
            foreach (var item in batchDefectiveVmdls)
            {
                var curNewInputBatch = InputBatchList.FirstOrDefault(x => x.BatchNo == item.Barcode && x.SubBatchNo == item.ChildBarcode);
                if (curNewInputBatch != null)
                {
                    curNewInputBatch.NgQty += item.NgQty;
                    if (!inputs.Contains(curNewInputBatch))
                    {
                        inputs.Add(curNewInputBatch);
                    }
                }
            }

            foreach (var item in inputs)
            {
                item.RemainQty -= item.NgQty;
            }

            inputs.Clear();
        }

        /// <summary>
        ///  修改当前不良子批次对应的Input对象
        /// </summary>
        /// <param name="outputBatch">当前转出批次</param>
        private void RefreshInputBatchByCurrentNgOutputBatch(OutputBatch outputBatch)
        {
            var inputs = new List<InputBatch>();
            foreach (var relationBatch in outputBatch.RelationBatchList)
            {
                var oldNgInputBatch = relationBatch.InputBatch;
                var newNgInputBatch = InputBatchList.FirstOrDefault(x => x.Id == oldNgInputBatch.Id);
                if (newNgInputBatch != null)
                {
                    oldNgInputBatch.NgQty -= relationBatch.Qty;
                    newNgInputBatch.NgQty = oldNgInputBatch.NgQty;
                    if (!inputs.Contains(newNgInputBatch))
                    {
                        inputs.Add(newNgInputBatch);
                    }
                }
            }

            foreach (var item in inputs)
            {
                item.RemainQty -= item.NgQty;
            }

            inputs.Clear();
        }

        /// <summary>
        /// 移除刷新InputBatch
        /// 先从出站集合中移除对象, 再刷新入站集合
        /// </summary>
        /// <param name="outputBatch">移除转出的对象</param>
        public void RefreshInputBatchRemoveOutputBatch(OutputBatch outputBatch)
        {
            try
            {
                IsChanged = true;
                if (outputBatch!= null && outputBatch.IsNg) //移除的是不良子批次
                {
                    RefreshInputBatchByCurrentNgOutputBatch(outputBatch); ////修改当前不良子批次对应的Input对象

                    RefreshInputBatchByOtherNgOutputBatch(outputBatch); ////修改其它不良子批次对应的Input对象

                    RefreshInutBatchByBatchDefectiveViewModels();  ////修改批次检验不良记录对应的Input对象

                    RefreshInputBatchByNormalOutputBatch(); ////修改正常子批次对应的Input对象                
                }
                else ////移除的是正常子批次
                {
                    RefreshInputBatchByAllNgOutputBatch(); ////修改所有不良子批次对应的Input对象

                    RefreshInutBatchByBatchDefectiveViewModels();  ////修改批次检验不良记录对应的Input对象
                }
            }
            finally
            {
                IsChanged = false;
            }
        }

        /// <summary>
        /// 转出刷新InputBatch
        ///  先刷新入站集合, 再从出站集合中移除对象
        /// </summary>
        /// <param name="outputBatch">出站转出的对象</param>
        public void RefreshInputBatchMoveOutputBatch(OutputBatch outputBatch)
        {
            try
            {
                IsChanged = true;
                if (outputBatch != null && outputBatch.IsNg) ////转出不良子批次
                {
                    RefreshInputBatchByOtherNgOutputBatch(outputBatch); ////其它不良子批次对应的Input对象

                    RefreshInutBatchByBatchDefectiveViewModels();  ////修改批次检验不良记录对应的Input对象

                    RefreshInputBatchByNormalOutputBatch(); ////修改正常子批次对应的Input对象
                }
                else ////转出正常子批次
                {
                    RefreshInputBatchByAllNgOutputBatch(); ////修改所有不良子批次对应的Input对象

                    RefreshInutBatchByBatchDefectiveViewModels();  ////修改批次检验不良记录对应的Input对象
                }
            }
            finally
            {
                IsChanged = false;
            }
        }

        /// <summary>
        /// 产品等级转换未枚举类型的产品等级
        /// </summary>
        /// <param name="gradeCode">产品等级编码字符</param>
        /// <returns>枚举类型的产品等级</returns>
        private ProductGrade GetEnumGrade(string gradeCode)
        {
            ProductGrade enumGrade;
            switch (gradeCode)
            {
                case "010":
                    enumGrade = ProductGrade.A;
                    break;
                case "020":
                    enumGrade = ProductGrade.B;
                    break;
                case "030":
                    enumGrade = ProductGrade.C;
                    break;
                default:
                    enumGrade = ProductGrade.Scrap;
                    break;
            }

            return enumGrade;
        }
    }
}

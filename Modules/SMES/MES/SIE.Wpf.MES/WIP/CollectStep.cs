using SIE.Domain.Validation;
using SIE.Tech.Processs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Wpf.MES.WIP
{
    /// <summary>
    /// 采集步骤，控制条码录入
    /// </summary>
    public class CollectStep
    {
        /// <summary>
        /// 当前步骤索引
        /// </summary>
        protected int _stepIndex;

        /// <summary>
        /// 按步骤采集的条码
        /// </summary>
        readonly List<string> _barcodes = new List<string>();

        /// <summary>
        /// DataCollectionViewModel
        /// </summary>
        protected DataCollectionViewModel _viewModel;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="vm">DataCollectionViewModel</param>
        public CollectStep(DataCollectionViewModel vm)
        {
            _viewModel = vm;
        }

        /// <summary>
        /// 工序采集步骤
        /// </summary>
        public IEnumerable<ProcessCollectStep> ProcessSteps
        {
            get { return _viewModel.Workstation.Process?.CollectStepList.OrderBy(p => p.Step); }
        }

        /// <summary>
        /// 当前采集步骤
        /// </summary>
        public ProcessCollectStep CurrentStep
        {
            get
            {
#pragma warning disable S2372 // Exceptions should not be thrown from property getters
                if (!ProcessSteps.Any())
                    throw new ValidationException("工序{0}未定义采集步骤".L10nFormat(_viewModel.Workstation.Process.Name));
                if (_stepIndex > ProcessSteps.Count() - 1)
                    throw new ValidationException("采集步骤异常".L10N());
#pragma warning restore S2372 // Exceptions should not be thrown from property getters
                return ProcessSteps.Skip(StepIndex).First();
            }
        }

        /// <summary>
        /// 步骤索引
        /// </summary>
        public int StepIndex { get { return _stepIndex; } }

        /// <summary>
        /// 按步骤采集的条码
        /// </summary>
        public List<string> Barcodes { get { return _barcodes; } }

        /// <summary>
        /// 下一步，如果没有下一步，返回false
        /// </summary>
        /// <returns>是否有下一步</returns>
        public virtual bool NextStep()
        {
            if (_stepIndex < ProcessSteps.Count() - 1)
            {
                _stepIndex++;
                return true;
            }

            return false;
        }

        /// <summary>
        /// 是否有下一步
        /// </summary>
        /// <returns>返回是否有下一步</returns>
        public virtual bool HasNextStep()
        {
            return Barcodes.Count < ProcessSteps?.Count();
        }

        /// <summary>
        /// 重置步骤
        /// </summary>
        public virtual void Reset()
        {
            _stepIndex = 0;
            _barcodes.Clear();
        }

        /// <summary>
        /// 回滚一步
        /// </summary>
        public virtual void Roolback()
        {
            _barcodes.RemoveAt(_stepIndex);
            _stepIndex = _barcodes.Count;
        }
    }
}

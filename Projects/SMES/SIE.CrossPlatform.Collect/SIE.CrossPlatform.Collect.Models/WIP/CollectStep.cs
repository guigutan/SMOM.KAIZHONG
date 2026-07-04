using SIE.CrossPlatform.Collect.Models.WIP.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.CrossPlatform.Collect.Models.WIP
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
        /// Workcell
        /// </summary>
        protected Workcell Workcell;

        /// <summary>
        /// 
        /// </summary>
        protected Process Process { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="vm">DataCollectionViewModel</param>
        public CollectStep(Workcell workcell, Process process)
        {
            this.Workcell = workcell;
            this.Process = process;
        }

        /// <summary>
        /// 工序采集步骤
        /// </summary>
        public IEnumerable<ProcessCollectStep> ProcessSteps
        {
            get { return this.Process?.CollectStepList.OrderBy(p => p.Step); }
        }

        /// <summary>
        /// 当前采集步骤
        /// </summary>
        public ProcessCollectStep CurrentStep
        {
            get
            {
                if (!ProcessSteps.Any())
                {
                    var msg = string.Format("工序{0}未定义采集步骤", this.Process.Name);
                    throw new Exception(msg);
                }
                if (_stepIndex > ProcessSteps.Count() - 1)
                {
                    throw new Exception("采集步骤异常");
                }
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

using SIE.Domain.Validation;
using SIE.Tech.Processs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.ForWinform.ApiModels
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class XPCollectStep
    {
        /// <summary>
        /// 步骤索引
        /// </summary>
        public int StepIndex { get; set; }

        /// <summary>
        /// 按步骤采集的条码
        /// </summary>
        public List<string> Barcodes { get; set; } = new List<string>();

        /// <summary>
        /// 
        /// </summary>
        public string ProcessName { get; set; }

        /// <summary>
        /// 工序采集步骤
        /// </summary>
        public List<XPProcessCollectStep> ProcessSteps = new List<XPProcessCollectStep>();

        /// <summary>
        /// 设置工序采集步骤
        /// </summary>
        /// <param name="process"></param>
        public void SetProcess(Process process)
        {
            this.ProcessName = process?.Name;
            this.ProcessSteps.Clear();
            foreach (ProcessCollectStep processCollectStep in process?.CollectStepList.OrderBy(p => p.Step))
            {
                this.ProcessSteps.Add(new XPProcessCollectStep(processCollectStep));
            }
        }


        /// <summary>
        /// 当前采集步骤
        /// </summary>
        public XPProcessCollectStep CurrentStep
        {
            get
            {
                if (!ProcessSteps.Any())
                    return null;
                if (StepIndex > ProcessSteps.Count() - 1)
                    return null;
                return ProcessSteps.Skip(StepIndex).First();
            }
        }


        /// <summary>
        /// 下一步，如果没有下一步，返回false
        /// </summary>
        /// <returns>是否有下一步</returns>
        public virtual bool NextStep()
        {
            if (StepIndex < ProcessSteps.Count() - 1)
            {
                StepIndex++;
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
            StepIndex = 0;
            Barcodes.Clear();
        }

        /// <summary>
        /// 回滚一步
        /// </summary>
        public virtual void Roolback()
        {
            Barcodes.RemoveAt(StepIndex);
            StepIndex = Barcodes.Count;
        }
    }
}

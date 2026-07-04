using SIE.CrossPlatform.Collect.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.CrossPlatform.Collect.Models.WIP
{
    /// <summary>
    /// 采集步骤，控制条码录入
    /// </summary>
    public class XPReworkStep : XPCollectStep
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public XPReworkStep() : base()
        {
            RewkOperate = ReworkOperate.Permute;
            ReworkCollectBarcodes = new List<CollectBarcode>();
            ReworkBarcodes = new List<string>();
            StepBatcodeTypesIni();
        }

        /// <summary>
        /// 返工采集类型
        /// </summary>
        public ReworkOperate RewkOperate { get; set; }

        /// <summary>
        /// 返工采集条码集合
        /// </summary>
        public List<WIP.CollectBarcode> ReworkCollectBarcodes { get; }

        /// <summary>
        /// 返工条码集合
        /// </summary>
        public List<string> ReworkBarcodes { get; }

        /// <summary>
        /// 采集步骤条码类型
        /// </summary>
        public List<string> StepBarcodeTypes { get; } = new List<string>();

        /// <summary>
        /// 初始化采集步骤条码类型
        /// </summary>
        private void StepBatcodeTypesIni()
        {
            StepBarcodeTypes.Clear();
            StepBarcodeTypes.Add("返工工单条码");
            StepBarcodeTypes.Add("原工单条码");
        }

        /// <summary>
        /// 重置
        /// </summary>
        public override void Reset()
        {
            base.Reset();
            ReworkCollectBarcodes.Clear();
            ReworkBarcodes.Clear();
        }

        /// <summary>
        /// 回滚一步
        /// </summary>
        public override void Roolback()
        {
            ReworkCollectBarcodes.RemoveAt(StepIndex);
            if (StepIndex < ReworkBarcodes.Count)
            {
                ReworkBarcodes.RemoveAt(StepIndex);
            }
            base.Roolback();
        }

        /// <summary>
        /// 设置StepIndex++
        /// </summary>
        public void AddStepIndex()
        {
            if (StepIndex < ProcessSteps.Count() - 1)
            {
                StepIndex++;
            }
        }

        /// <summary>
        /// 设置StepIndex--
        /// </summary>
        public void SubStepIndex()
        {
            if (StepIndex >= 1)
            {
                StepIndex--;
            }
        }

        /// <summary>
        /// 是否有下一步 
        /// </summary>
        /// <returns>有下一步返回true ; 没有下一步返回false</returns>
        public override bool NextStep()
        {
            bool checkFlag = base.NextStep();
            return checkFlag;
        }

        /// <summary>
        /// 是否有下一步
        /// </summary>
        /// <returns>返回是否有下一步</returns>
        public override bool HasNextStep()
        {
            bool checkFlag = base.HasNextStep();
            return checkFlag;
        }

        /// <summary>
        /// 返工条码Add方法
        /// </summary>
        /// <param name="rewkBarcode">返工条码</param>
        public void AddReworkBarcodes(string rewkBarcode)
        {
            if (!this.ReworkBarcodes.Contains(rewkBarcode))
            {
                this.ReworkBarcodes.Add(rewkBarcode);
            }
        }

        /// <summary>
        /// 按步骤采集的条码Add方法
        /// </summary>
        /// <param name="barcode">条码</param>
        public void AddBarcodes(string barcode)
        {
            if (!this.Barcodes.Contains(barcode))
            {
                this.Barcodes.Add(barcode);
            }
        }

        /// <summary>
        /// 返工采集条码集合
        /// </summary>
        /// <param name="collectBarcode">采集条码</param>
        public void AddReworkCollectBarcodes(CollectBarcode collectBarcode)
        {
            var containCount = this.ReworkCollectBarcodes.Count(x => x.Code == collectBarcode.Code);
            if (containCount < 1)
            {
                this.ReworkCollectBarcodes.Add(collectBarcode);
            }
        }
    }
}

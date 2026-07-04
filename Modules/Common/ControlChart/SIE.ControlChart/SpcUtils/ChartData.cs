using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.ControlChart.SpcUtils
{
    /// <summary>
    /// 控制图数据类
    /// </summary>
    [Serializable]
    public class ChartData : ChartDataBase
    {
        /// <summary>
        /// 控制图参数
        /// </summary>
        public ChartCalConst ChartConst { get; set; } 

        /// <summary>
        /// CA
        /// </summary>
        public double? Ca { get; set; }
      
        /// <summary>
        /// 上控制图控制限
        /// </summary>
        public ControlLine UpChartControlLine { get; set; } = new ControlLine();
        /// <summary>
        /// 下控制图控制限
        /// </summary>
        public ControlLine DownChartControlLine { get; set; } = new ControlLine();
        /// <summary>
        /// 自定义上控制图控制限
        /// </summary>
        public ControlLine DefineUpChartControlLine { get; set; }
        /// <summary>
        /// 自定义下控制图控制限
        /// </summary>
        public ControlLine DefineDownChartControlLine { get; set; }
        /// <summary>
        /// 标准差（整体）
        /// </summary>
        public double StdDev { get; set; }
        /// <summary>
        /// 标准差（组内）
        /// </summary>
        public double GroupStdDev { get; set; }

        /// <summary>
        /// 上限
        /// </summary>
        public double? Usl { get; set; }
        /// <summary>
        /// 下限
        /// </summary>
        public double? Lsl { get; set; }
        /// <summary>
        /// CP指数
        /// </summary>
        public double? Cp { get; set; }
        /// <summary>
        /// CPU指数
        /// </summary>
        public double? Cpu { get; set; }
        /// <summary>
        /// CPL指数
        /// </summary>
        public double? Cpl { get; set; }
        /// <summary>
        /// CPK指数
        /// </summary>
        public double? Cpk { get; set; }
        /// <summary>
        /// PP指数
        /// </summary>
        public double? Pp { get; set; }
        /// <summary>
        /// PPU指数
        /// </summary>
        public double? Ppu { get; set; }
        /// <summary>
        /// PPL指数
        /// </summary>
        public double? Ppl { get; set; }
        /// <summary>
        /// PPK指数
        /// </summary>
        public double? Ppk { get; set; }
        /// <summary>
        /// PPM指数
        /// </summary>
        public double? Ppm { get; set; }
        /// <summary>
        /// CPK等级
        /// </summary>
        public string CPKGrade { get; set; }
        /// <summary>
        /// 子组大小
        /// </summary>
        public int SubGroupSize { get; set; }
        /// <summary>
        /// 样本组数
        /// </summary>
        public int? SubGroupCount { get; set; }

        /// <summary>
        /// 所有样本
        /// </summary>
        [JsonIgnore]
        public List<double> AllSample { get; set; }=new List<double>();

        /// <summary>
        /// 所有样本组数据
        /// </summary>
        [JsonIgnore]
        public List<SampleGroup> AllDatas { get; set; }

        /// <summary>
        /// 样本组数据
        /// </summary>

        public List<SampleGroup> Datas { get; set; }
        /// <summary>
        /// 上图均值 
        /// </summary>
        public double UpChartAvg { get; set; }
        /// <summary>
        /// 下图均值
        /// </summary>
        public double DownChartAvg { get; set; }

        /// <summary>
        /// 标准差估值方式
        /// </summary>
        public EstimateMode StdEstimateMode { get; set; } = EstimateMode.RBar;

        /// <summary>
        /// 低阈值
        /// </summary>
        public double CpkLow { get; set; }

        /// <summary>
        /// 中阈值
        /// </summary>
        public double CpkMid { get; set; }

        /// <summary>
        /// 高阈值
        /// </summary>
        public double CpkHigh { get; set; }

        /// <summary>
        /// 是否显示CPK等级
        /// </summary>
        public bool IsdisplayCpk { get; set; }

        /// <summary>
        /// 计算
        /// </summary>
        public virtual void Calculate()
        {

        }

        /// <summary>
        /// 计算各指数
        /// </summary>
        public virtual void CalculateExponent(double _XAvg)
        {
            if (Usl.HasValue)
            {
                if (StdDev != 0)
                {
                    Ppu = (Usl.Value - _XAvg) / 3 / StdDev;
                    Ppk = Ppu;
                }
                if (GroupStdDev != 0)
                {
                    Cpu = (Usl.Value - _XAvg) / 3 / GroupStdDev;
                    Cpk = Cpu;
                }
            }
            if (Lsl.HasValue)
            {
                if (StdDev != 0)
                {
                    Ppl = (_XAvg - Lsl.Value) / 3 / StdDev;
                    Ppk = Ppl;
                }
                if (GroupStdDev != 0)
                {
                    Cpl = (_XAvg - Lsl.Value) / 3 / GroupStdDev;
                    Cpk = Cpl;
                }
            }
            if (Lsl.HasValue && Usl.HasValue)
            {
                if (StdDev != 0)
                {
                    Ppk = Math.Min(Ppu.Value, Ppl.Value);
                    Pp = (Usl.Value - Lsl.Value) / 6 / StdDev;
                }
                if (GroupStdDev != 0)
                {
                    Cpk = Math.Min(Cpu.Value, Cpl.Value);
                    Cp = (Usl.Value - Lsl.Value) / 6 / GroupStdDev;
                }
                Ca = (_XAvg - (Usl + Lsl) / 2) / ((Usl - Lsl) / 2);
            }
        }

        /// <summary>
        /// 计算PPM
        /// </summary>
        public virtual void CalculatePPM()
        {
            int ng_qty = 0;
            AllSample.ForEach(v =>
            {
                if (Usl.HasValue && v > Usl.Value || Lsl.HasValue && v < Lsl.Value)
                    ng_qty++;
            });
            if (Usl.HasValue || Lsl.HasValue)
                Ppm = ng_qty * 1000000 / AllSample.Count;
        }
    }
}

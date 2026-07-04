using SIE.ControlChart.ChartDatas;
using SIE.ControlChart.ChartDatas.SpcXBarRCharts;
using SIE.ControlChart.Extension;
using SIE.ControlChart.SpcUtils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.ControlChart.ChartDatas.SpcNormalDistributionCharts
{
    /// <summary>
    /// 正态分布图
    /// </summary>
    [Serializable]
    public class SpcNormalDistributionCharts : SpcChartBase
    {
        /// <summary>
        /// 数据计算精确度 
        /// </summary>
        private int _Digits { get; set; } = 2;

        /// <summary>
        /// 数据计算精确度
        /// </summary>
        public int? Digits { get; set; } = null;
        /// <summary>
        /// 显示CPK等指数
        /// </summary>
        public bool ShowIndex { get; set; }
        /// <summary>
        /// 样本数据
        /// </summary>
        public List<double> Datas { get; set; }
        /// <summary>
        /// 样本大小
        /// </summary>
        public int SubGroupSize { get; set; }
        /// <summary>
        /// 样本组
        /// </summary>
        private List<SampleGroup> Groups = new List<SampleGroup>();
        /// <summary>
        /// 目标上限
        /// </summary>
        public double? Usl { get; set; }
        /// <summary>
        /// 目标下限
        /// </summary>
        public double? Lsl { get; set; }
        /// <summary>
        /// 整体标准差
        /// </summary>
        public double StdDev { get; set; }
        /// <summary>
        /// 组内标准差
        /// </summary>
        public double GroupStdDev { get; set; }
        /// <summary>
        /// CP
        /// </summary>
        public double? Cp { get; set; }
        /// <summary>
        /// CP
        /// </summary>
        public double? Cpu { get; set; }
        /// <summary>
        /// CPU
        /// </summary>
        public double? Cpl { get; set; }
        /// <summary>
        /// CPL
        /// </summary>
        public double? Cpk { get; set; }
        /// <summary>
        /// CPK
        /// </summary>
        public double? Pp { get; set; }
        /// <summary>
        /// PP
        /// </summary>
        public double? Ppu { get; set; }
        /// <summary>
        /// PPU
        /// </summary>
        public double? Ppl { get; set; }
        /// <summary>
        /// PPL
        /// </summary>
        public double? Ppk { get; set; }
        /// <summary>
        /// PPM
        /// </summary>
        public double? Ppm { get; set; }
        /// <summary>
        /// 平均值
        /// </summary>

        public string Title { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool ShowPercent { get; set; }
        /// <summary>
        /// 显示标题
        /// </summary>
        public bool ShowTitle { get; set; }
        /// <summary>
        /// 自定义直方图数量(不自动生成直方图数)
        /// </summary>
        public int? DefineGroupCount { get; set; }

        /// <summary>
        /// 平均数
        /// </summary>
        public double Avg { get; set; }

        /// <summary>
        /// 标准差估值方式
        /// </summary>
        public EstimateMode StdEstimateMode { get; set; } = EstimateMode.RBar;

        /// <summary>
        /// 正态分布图构造函数
        /// </summary>
        public SpcNormalDistributionCharts()
        {
            Title = "正态分布".L10N();
            ShowIndex = true;
            ShowTitle = true;
        }

        /// <summary>
        /// 获取绘图需要的数据
        /// </summary>
        public override void GetDrawData()
        {
            //数据运算
            Calculate();
        }

        /// <summary>
        /// 数据运算
        /// </summary>
        private void Calculate()
        {
            //计算精度 
            if (Digits.HasValue)
                _Digits = Digits.Value;
            else
            {
                List<double> ds = new List<double>();
                ds.AddRange(Datas);
                if (Lsl.HasValue)
                    ds.Add(Lsl.Value);
                if (Usl.HasValue)
                    ds.Add(Usl.Value);
                _Digits = ChartDataBase.GetMaxDigits(ds.Select(d => d.ToObject()).ToList());
                Digits = _Digits;   //返回精度到前端
            }

            double[] sample_datas = Datas.ToArray();
            for (var i = 0; i < Datas.Count; i += SubGroupSize)
            {
                SampleGroup sg = new SampleGroup();
                sg.Datas = Datas.Skip(i).Take(SubGroupSize).ToArray();
                Groups.Add(sg);
            }
            //计算标准差，平均数 
            GroupStdDev = CalculateUtil.CalculateStdMethod(StdEstimateMode, Groups, SubGroupSize, ChartConst);
            StdDev = CalculateUtil.CalculateStdDev(sample_datas);
            Avg = sample_datas.Average();
            //计算PPU，PPK，CPU，CPK
            if (Usl.HasValue)
            {
                if (StdDev != 0)
                {
                    Ppu = (Usl.Value - Avg) / 3 / StdDev;
                    Ppk = Ppu;
                }
                if (GroupStdDev != 0)
                {
                    Cpu = (Usl.Value - Avg) / 3 / GroupStdDev;
                    Cpk = Cpu;
                }
            }
            //计算PPL，PPK，CPL，CPK
            if (Lsl.HasValue)
            {
                if (StdDev != 0)
                {
                    Ppl = (Avg - Lsl.Value) / 3 / StdDev;
                    Ppk = Ppl;
                }
                if (GroupStdDev != 0)
                {
                    Cpl = (Avg - Lsl.Value) / 3 / GroupStdDev;
                    Cpk = Cpl;
                }
            }
            //PPK，PP，CPK，CP
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
            }

            //计算PPM
            int ng_qty = 0;
            Datas.ForEach(v =>
            {
                if (Usl.HasValue && v > Usl.Value || Lsl.HasValue && v < Lsl.Value)
                    ng_qty++;
            });
            if (Usl.HasValue || Lsl.HasValue)
                Ppm = ng_qty * 1000000 / Datas.Count;
        }


        /// <summary>
        /// 自定义均值、标准差 设置控制限
        /// </summary>
        public override void SetDefineControlLineByCustomStdDeviation(double meanValue, double standardDeviation, int subGroupSize)
        {

        }

    }
}

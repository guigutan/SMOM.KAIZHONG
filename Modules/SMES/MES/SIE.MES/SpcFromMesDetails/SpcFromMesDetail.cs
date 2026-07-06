using SIE.Domain;
using SIE.MES.SpcFromMess;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.SpcFromMesDetails
{

    /// <summary>
    /// 统计过程控制明细表
    /// </summary>
    [ChildEntity, Serializable]
    [Label("统计过程控制明细表")]
    public class SpcFromMesDetail : DataEntity
    {

        #region 过程控制 SpcFromMes
        /// <summary>
        /// 过程控制ID（普通引用，非父子）
        /// </summary>
        [Label("过程控制")]
        public static readonly IRefIdProperty SpcFromMesIdProperty = P<SpcFromMesDetail>.RegisterRefId(e => e.SpcFromMesId, ReferenceType.Parent);


        /// <summary>
        /// 过程控制ID
        /// </summary>
        public double SpcFromMesId
        {
            get { return (double)this.GetRefNullableId(SpcFromMesIdProperty); }
            set { this.SetRefNullableId(SpcFromMesIdProperty, value); }
        }


        /// <summary>
        /// 过程控制（父实体引用）
        /// </summary>
        public static readonly RefEntityProperty<SpcFromMes> SpcFromMesProperty = P<SpcFromMesDetail>.RegisterRef(e => e.SpcFromMes, SpcFromMesIdProperty);


        /// <summary>
        /// 过程控制
        /// </summary>
        public SpcFromMes SpcFromMes
        {
            get { return this.GetRefEntity(SpcFromMesProperty); }
            set { this.SetRefEntity(SpcFromMesProperty, value); }
        }
        #endregion     

        #region 子组1 ObservedValue1
        /// <summary>
        /// 子组1
        /// </summary>
        [Label("子组1")]
        public static readonly Property<decimal> ObservedValue1Property = P<SpcFromMesDetail>.Register(e => e.ObservedValue1);

        /// <summary>
        /// 子组1
        /// </summary>
        public decimal ObservedValue1
        {
            get { return this.GetProperty(ObservedValue1Property); }
            set { this.SetProperty(ObservedValue1Property, value); }
        }
        #endregion

        #region 子组2 ObservedValue2
        /// <summary>
        /// 子组2
        /// </summary>
        [Label("子组2")]
        public static readonly Property<decimal> ObservedValue2Property = P<SpcFromMesDetail>.Register(e => e.ObservedValue2);

        /// <summary>
        /// 子组2
        /// </summary>
        public decimal ObservedValue2
        {
            get { return this.GetProperty(ObservedValue2Property); }
            set { this.SetProperty(ObservedValue2Property, value); }
        }
        #endregion

        #region 子组3 ObservedValue3
        /// <summary>
        /// 子组3
        /// </summary>
        [Label("子组3")]
        public static readonly Property<decimal> ObservedValue3Property = P<SpcFromMesDetail>.Register(e => e.ObservedValue3);

        /// <summary>
        /// 子组3
        /// </summary>
        public decimal ObservedValue3
        {
            get { return this.GetProperty(ObservedValue3Property); }
            set { this.SetProperty(ObservedValue3Property, value); }
        }
        #endregion

        #region 子组4 ObservedValue4
        /// <summary>
        /// 子组4
        /// </summary>
        [Label("子组4")]
        public static readonly Property<decimal> ObservedValue4Property = P<SpcFromMesDetail>.Register(e => e.ObservedValue4);

        /// <summary>
        /// 子组4
        /// </summary>
        public decimal ObservedValue4
        {
            get { return this.GetProperty(ObservedValue4Property); }
            set { this.SetProperty(ObservedValue4Property, value); }
        }
        #endregion

        #region 子组5 ObservedValue5
        /// <summary>
        /// 子组5
        /// </summary>
        [Label("子组5")]
        public static readonly Property<decimal> ObservedValue5Property = P<SpcFromMesDetail>.Register(e => e.ObservedValue5);

        /// <summary>
        /// 子组5
        /// </summary>
        public decimal ObservedValue5
        {
            get { return this.GetProperty(ObservedValue5Property); }
            set { this.SetProperty(ObservedValue5Property, value); }
        }
        #endregion

        #region 子组6 ObservedValue6
        /// <summary>
        /// 子组6
        /// </summary>
        [Label("子组6")]
        public static readonly Property<decimal> ObservedValue6Property = P<SpcFromMesDetail>.Register(e => e.ObservedValue6);

        /// <summary>
        /// 子组6
        /// </summary>
        public decimal ObservedValue6
        {
            get { return this.GetProperty(ObservedValue6Property); }
            set { this.SetProperty(ObservedValue6Property, value); }
        }
        #endregion

        #region 子组7 ObservedValue7
        /// <summary>
        /// 子组7
        /// </summary>
        [Label("子组7")]
        public static readonly Property<decimal> ObservedValue7Property = P<SpcFromMesDetail>.Register(e => e.ObservedValue7);

        /// <summary>
        /// 子组7
        /// </summary>
        public decimal ObservedValue7
        {
            get { return this.GetProperty(ObservedValue7Property); }
            set { this.SetProperty(ObservedValue7Property, value); }
        }
        #endregion

        #region 子组8 ObservedValue8
        /// <summary>
        /// 子组8
        /// </summary>
        [Label("子组8")]
        public static readonly Property<decimal> ObservedValue8Property = P<SpcFromMesDetail>.Register(e => e.ObservedValue8);

        /// <summary>
        /// 子组8
        /// </summary>
        public decimal ObservedValue8
        {
            get { return this.GetProperty(ObservedValue8Property); }
            set { this.SetProperty(ObservedValue8Property, value); }
        }
        #endregion

        #region 子组9 ObservedValue9
        /// <summary>
        /// 子组9
        /// </summary>
        [Label("子组9")]
        public static readonly Property<decimal> ObservedValue9Property = P<SpcFromMesDetail>.Register(e => e.ObservedValue9);

        /// <summary>
        /// 子组9
        /// </summary>
        public decimal ObservedValue9
        {
            get { return this.GetProperty(ObservedValue9Property); }
            set { this.SetProperty(ObservedValue9Property, value); }
        }
        #endregion

        #region 子组10 ObservedValue10
        /// <summary>
        /// 子组10
        /// </summary>
        [Label("子组10")]
        public static readonly Property<decimal> ObservedValue10Property = P<SpcFromMesDetail>.Register(e => e.ObservedValue10);

        /// <summary>
        /// 子组10
        /// </summary>
        public decimal ObservedValue10
        {
            get { return this.GetProperty(ObservedValue10Property); }
            set { this.SetProperty(ObservedValue10Property, value); }
        }
        #endregion

        #region 子组11 ObservedValue11
        /// <summary>
        /// 子组11
        /// </summary>
        [Label("子组11")]
        public static readonly Property<decimal> ObservedValue11Property = P<SpcFromMesDetail>.Register(e => e.ObservedValue11);

        /// <summary>
        /// 子组11
        /// </summary>
        public decimal ObservedValue11
        {
            get { return this.GetProperty(ObservedValue11Property); }
            set { this.SetProperty(ObservedValue11Property, value); }
        }
        #endregion

        #region 子组12 ObservedValue12
        /// <summary>
        /// 子组12
        /// </summary>
        [Label("子组12")]
        public static readonly Property<decimal> ObservedValue12Property = P<SpcFromMesDetail>.Register(e => e.ObservedValue12);

        /// <summary>
        /// 子组12
        /// </summary>
        public decimal ObservedValue12
        {
            get { return this.GetProperty(ObservedValue12Property); }
            set { this.SetProperty(ObservedValue12Property, value); }
        }
        #endregion

        #region 子组13 ObservedValue13
        /// <summary>
        /// 子组13
        /// </summary>
        [Label("子组13")]
        public static readonly Property<decimal> ObservedValue13Property = P<SpcFromMesDetail>.Register(e => e.ObservedValue13);

        /// <summary>
        /// 子组13
        /// </summary>
        public decimal ObservedValue13
        {
            get { return this.GetProperty(ObservedValue13Property); }
            set { this.SetProperty(ObservedValue13Property, value); }
        }
        #endregion

        #region 子组14 ObservedValue14
        /// <summary>
        /// 子组14
        /// </summary>
        [Label("子组14")]
        public static readonly Property<decimal> ObservedValue14Property = P<SpcFromMesDetail>.Register(e => e.ObservedValue14);

        /// <summary>
        /// 子组14
        /// </summary>
        public decimal ObservedValue14
        {
            get { return this.GetProperty(ObservedValue14Property); }
            set { this.SetProperty(ObservedValue14Property, value); }
        }
        #endregion

        #region 子组15 ObservedValue15
        /// <summary>
        /// 子组15
        /// </summary>
        [Label("子组15")]
        public static readonly Property<decimal> ObservedValue15Property = P<SpcFromMesDetail>.Register(e => e.ObservedValue15);

        /// <summary>
        /// 子组15
        /// </summary>
        public decimal ObservedValue15
        {
            get { return this.GetProperty(ObservedValue15Property); }
            set { this.SetProperty(ObservedValue15Property, value); }
        }
        #endregion

        #region 子组16 ObservedValue16
        /// <summary>
        /// 子组16
        /// </summary>
        [Label("子组16")]
        public static readonly Property<decimal> ObservedValue16Property = P<SpcFromMesDetail>.Register(e => e.ObservedValue16);

        /// <summary>
        /// 子组16
        /// </summary>
        public decimal ObservedValue16
        {
            get { return this.GetProperty(ObservedValue16Property); }
            set { this.SetProperty(ObservedValue16Property, value); }
        }
        #endregion

        #region 子组17 ObservedValue17
        /// <summary>
        /// 子组17
        /// </summary>
        [Label("子组17")]
        public static readonly Property<decimal> ObservedValue17Property = P<SpcFromMesDetail>.Register(e => e.ObservedValue17);

        /// <summary>
        /// 子组17
        /// </summary>
        public decimal ObservedValue17
        {
            get { return this.GetProperty(ObservedValue17Property); }
            set { this.SetProperty(ObservedValue17Property, value); }
        }
        #endregion

        #region 子组18 ObservedValue18
        /// <summary>
        /// 子组18
        /// </summary>
        [Label("子组18")]
        public static readonly Property<decimal> ObservedValue18Property = P<SpcFromMesDetail>.Register(e => e.ObservedValue18);

        /// <summary>
        /// 子组18
        /// </summary>
        public decimal ObservedValue18
        {
            get { return this.GetProperty(ObservedValue18Property); }
            set { this.SetProperty(ObservedValue18Property, value); }
        }
        #endregion

        #region 子组19 ObservedValue19
        /// <summary>
        /// 子组19
        /// </summary>
        [Label("子组19")]
        public static readonly Property<decimal> ObservedValue19Property = P<SpcFromMesDetail>.Register(e => e.ObservedValue19);

        /// <summary>
        /// 子组19
        /// </summary>
        public decimal ObservedValue19
        {
            get { return this.GetProperty(ObservedValue19Property); }
            set { this.SetProperty(ObservedValue19Property, value); }
        }
        #endregion

        #region 子组20 ObservedValue20
        /// <summary>
        /// 子组20
        /// </summary>
        [Label("子组20")]
        public static readonly Property<decimal> ObservedValue20Property = P<SpcFromMesDetail>.Register(e => e.ObservedValue20);

        /// <summary>
        /// 子组20
        /// </summary>
        public decimal ObservedValue20
        {
            get { return this.GetProperty(ObservedValue20Property); }
            set { this.SetProperty(ObservedValue20Property, value); }
        }
        #endregion

        #region 子组21 ObservedValue21
        /// <summary>
        /// 子组21
        /// </summary>
        [Label("子组21")]
        public static readonly Property<decimal> ObservedValue21Property = P<SpcFromMesDetail>.Register(e => e.ObservedValue21);

        /// <summary>
        /// 子组21
        /// </summary>
        public decimal ObservedValue21
        {
            get { return this.GetProperty(ObservedValue21Property); }
            set { this.SetProperty(ObservedValue21Property, value); }
        }
        #endregion

        #region 子组22 ObservedValue22
        /// <summary>
        /// 子组22
        /// </summary>
        [Label("子组22")]
        public static readonly Property<decimal> ObservedValue22Property = P<SpcFromMesDetail>.Register(e => e.ObservedValue22);

        /// <summary>
        /// 子组22
        /// </summary>
        public decimal ObservedValue22
        {
            get { return this.GetProperty(ObservedValue22Property); }
            set { this.SetProperty(ObservedValue22Property, value); }
        }
        #endregion

        #region 子组23 ObservedValue23
        /// <summary>
        /// 子组23
        /// </summary>
        [Label("子组23")]
        public static readonly Property<decimal> ObservedValue23Property = P<SpcFromMesDetail>.Register(e => e.ObservedValue23);

        /// <summary>
        /// 子组23
        /// </summary>
        public decimal ObservedValue23
        {
            get { return this.GetProperty(ObservedValue23Property); }
            set { this.SetProperty(ObservedValue23Property, value); }
        }
        #endregion

        #region 子组24 ObservedValue24
        /// <summary>
        /// 子组24
        /// </summary>
        [Label("子组24")]
        public static readonly Property<decimal> ObservedValue24Property = P<SpcFromMesDetail>.Register(e => e.ObservedValue24);

        /// <summary>
        /// 子组24
        /// </summary>
        public decimal ObservedValue24
        {
            get { return this.GetProperty(ObservedValue24Property); }
            set { this.SetProperty(ObservedValue24Property, value); }
        }
        #endregion

        #region 子组25 ObservedValue25
        /// <summary>
        /// 子组25
        /// </summary>
        [Label("子组25")]
        public static readonly Property<decimal> ObservedValue25Property = P<SpcFromMesDetail>.Register(e => e.ObservedValue25);

        /// <summary>
        /// 子组25
        /// </summary>
        public decimal ObservedValue25
        {
            get { return this.GetProperty(ObservedValue25Property); }
            set { this.SetProperty(ObservedValue25Property, value); }
        }
        #endregion

        #region 子组26 ObservedValue26
        /// <summary>
        /// 子组26
        /// </summary>
        [Label("子组26")]
        public static readonly Property<decimal> ObservedValue26Property = P<SpcFromMesDetail>.Register(e => e.ObservedValue26);

        /// <summary>
        /// 子组26
        /// </summary>
        public decimal ObservedValue26
        {
            get { return this.GetProperty(ObservedValue26Property); }
            set { this.SetProperty(ObservedValue26Property, value); }
        }
        #endregion

        #region 子组27 ObservedValue27
        /// <summary>
        /// 子组27
        /// </summary>
        [Label("子组27")]
        public static readonly Property<decimal> ObservedValue27Property = P<SpcFromMesDetail>.Register(e => e.ObservedValue27);

        /// <summary>
        /// 子组27
        /// </summary>
        public decimal ObservedValue27
        {
            get { return this.GetProperty(ObservedValue27Property); }
            set { this.SetProperty(ObservedValue27Property, value); }
        }
        #endregion

        #region 子组28 ObservedValue28
        /// <summary>
        /// 子组28
        /// </summary>
        [Label("子组28")]
        public static readonly Property<decimal> ObservedValue28Property = P<SpcFromMesDetail>.Register(e => e.ObservedValue28);

        /// <summary>
        /// 子组28
        /// </summary>
        public decimal ObservedValue28
        {
            get { return this.GetProperty(ObservedValue28Property); }
            set { this.SetProperty(ObservedValue28Property, value); }
        }
        #endregion

        #region 子组29 ObservedValue29
        /// <summary>
        /// 子组29
        /// </summary>
        [Label("子组29")]
        public static readonly Property<decimal> ObservedValue29Property = P<SpcFromMesDetail>.Register(e => e.ObservedValue29);

        /// <summary>
        /// 子组29
        /// </summary>
        public decimal ObservedValue29
        {
            get { return this.GetProperty(ObservedValue29Property); }
            set { this.SetProperty(ObservedValue29Property, value); }
        }
        #endregion

        #region 子组30 ObservedValue30
        /// <summary>
        /// 子组30
        /// </summary>
        [Label("子组30")]
        public static readonly Property<decimal> ObservedValue30Property = P<SpcFromMesDetail>.Register(e => e.ObservedValue30);

        /// <summary>
        /// 子组30
        /// </summary>
        public decimal ObservedValue30
        {
            get { return this.GetProperty(ObservedValue30Property); }
            set { this.SetProperty(ObservedValue30Property, value); }
        }
        #endregion

        #region 子组合计 SumX------只读属性
        /// <summary>
        /// ∑X：子组大小GroupCount对应数量的ObservedValue之和
        /// </summary>
        [Label("∑X")]
        public static readonly Property<decimal> SumXProperty = P<SpcFromMesDetail>.RegisterReadOnly(
            e => e.SumX,
            e => CalcSumX(e),
            ObservedValue1Property, ObservedValue2Property, ObservedValue3Property, ObservedValue4Property, ObservedValue5Property,
            ObservedValue6Property, ObservedValue7Property, ObservedValue8Property, ObservedValue9Property, ObservedValue10Property,
            ObservedValue11Property, ObservedValue12Property, ObservedValue13Property, ObservedValue14Property, ObservedValue15Property,
            ObservedValue16Property, ObservedValue17Property, ObservedValue18Property, ObservedValue19Property, ObservedValue20Property,
            ObservedValue21Property, ObservedValue22Property, ObservedValue23Property, ObservedValue24Property, ObservedValue25Property,
            ObservedValue26Property, ObservedValue27Property, ObservedValue28Property, ObservedValue29Property, ObservedValue30Property
        );

        /// <summary>
        /// ∑X
        /// </summary>
        public decimal SumX
        {
            get { return this.GetProperty(SumXProperty); }
        }
        #endregion

        #region 子组均值 AvgX------只读属性
        /// <summary>
        /// X̄：∑X / 子组大小GroupCount
        /// </summary>
        [Label("AvgX(x)")]
        public static readonly Property<decimal> AvgXProperty = P<SpcFromMesDetail>.RegisterReadOnly(
            e => e.AvgX,
            e => CalcAvgX(e),
            SumXProperty
        );

        /// <summary>
        /// X̄
        /// </summary>
        public decimal AvgX
        {
            get { return this.GetProperty(AvgXProperty); }
        }
        #endregion

        #region 子组极差 RangeX------只读属性
        /// <summary>
        /// R：子组大小GroupCount对应数量的ObservedValue最大值减最小值
        /// </summary>
        [Label("R")]
        public static readonly Property<decimal> RangeXProperty = P<SpcFromMesDetail>.RegisterReadOnly(
            e => e.RangeX,
            e => CalcRangeX(e),
            ObservedValue1Property, ObservedValue2Property, ObservedValue3Property, ObservedValue4Property, ObservedValue5Property,
            ObservedValue6Property, ObservedValue7Property, ObservedValue8Property, ObservedValue9Property, ObservedValue10Property,
            ObservedValue11Property, ObservedValue12Property, ObservedValue13Property, ObservedValue14Property, ObservedValue15Property,
            ObservedValue16Property, ObservedValue17Property, ObservedValue18Property, ObservedValue19Property, ObservedValue20Property,
            ObservedValue21Property, ObservedValue22Property, ObservedValue23Property, ObservedValue24Property, ObservedValue25Property,
            ObservedValue26Property, ObservedValue27Property, ObservedValue28Property, ObservedValue29Property, ObservedValue30Property
        );

        /// <summary>
        /// R
        /// </summary>
        public decimal RangeX
        {
            get { return this.GetProperty(RangeXProperty); }
        }
        #endregion

        #region 计算方法

        private static decimal GetObservedValue(SpcFromMesDetail detail, int index)
        {
            switch (index)
            {
                case 1: return detail.ObservedValue1;
                case 2: return detail.ObservedValue2;
                case 3: return detail.ObservedValue3;
                case 4: return detail.ObservedValue4;
                case 5: return detail.ObservedValue5;
                case 6: return detail.ObservedValue6;
                case 7: return detail.ObservedValue7;
                case 8: return detail.ObservedValue8;
                case 9: return detail.ObservedValue9;
                case 10: return detail.ObservedValue10;
                case 11: return detail.ObservedValue11;
                case 12: return detail.ObservedValue12;
                case 13: return detail.ObservedValue13;
                case 14: return detail.ObservedValue14;
                case 15: return detail.ObservedValue15;
                case 16: return detail.ObservedValue16;
                case 17: return detail.ObservedValue17;
                case 18: return detail.ObservedValue18;
                case 19: return detail.ObservedValue19;
                case 20: return detail.ObservedValue20;
                case 21: return detail.ObservedValue21;
                case 22: return detail.ObservedValue22;
                case 23: return detail.ObservedValue23;
                case 24: return detail.ObservedValue24;
                case 25: return detail.ObservedValue25;
                case 26: return detail.ObservedValue26;
                case 27: return detail.ObservedValue27;
                case 28: return detail.ObservedValue28;
                case 29: return detail.ObservedValue29;
                case 30: return detail.ObservedValue30;
                default: return 0m;
            }
        }

        private static int GetGroupCount(SpcFromMesDetail detail)
        {
            var parent = detail.SpcFromMes;
            return parent != null ? parent.GroupCount : 0;
        }

        /// <summary>
        /// ∑X = ObservedValue1 + ... + ObservedValue[GroupCount]
        /// </summary>
        private static decimal CalcSumX(SpcFromMesDetail detail)
        {
            int groupCount = GetGroupCount(detail);
            if (groupCount <= 0) return 0m;
            decimal sum = 0m;
            for (int i = 1; i <= groupCount; i++)
                sum += GetObservedValue(detail, i);
            return sum;
        }

        /// <summary>
        /// X̄ = ∑X / GroupCount
        /// </summary>
        private static decimal CalcAvgX(SpcFromMesDetail detail)
        {
            int groupCount = GetGroupCount(detail);
            if (groupCount <= 0) return 0m;
            return Math.Round(detail.SumX / groupCount, 6);
        }

        /// <summary>
        /// R = Max(ObservedValue1..N) - Min(ObservedValue1..N)，N=GroupCount
        /// </summary>
        private static decimal CalcRangeX(SpcFromMesDetail detail)
        {
            int groupCount = GetGroupCount(detail);
            if (groupCount <= 0) return 0m;
            decimal min = GetObservedValue(detail, 1);
            decimal max = min;
            for (int i = 2; i <= groupCount; i++)
            {
                decimal val = GetObservedValue(detail, i);
                if (val < min) min = val;
                if (val > max) max = val;
            }
            return max - min;
        }

        #endregion

    }



    /// <summary>
    /// 配置数据库映射
    /// </summary>
    internal class SpcFromMesDetailConfig : EntityConfig<SpcFromMesDetail>
    {
        protected override void ConfigMeta()
        {
            Meta.MapTable("SPC_FROM_MES_DETAIL").MapAllProperties();
            Meta.EnablePhantoms();
            Meta.EnableInvOrg();
        }
    }

}

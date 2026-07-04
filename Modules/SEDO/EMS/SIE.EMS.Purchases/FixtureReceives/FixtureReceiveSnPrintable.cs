using SIE.Common.Prints;
using System;
using System.ComponentModel;

namespace SIE.EMS.Purchases.FixtureReceives
{
    /// <summary>
    /// 工治具接收序列号
    /// </summary>
    [Serializable]
    [DisplayName("工治具接收序列号")]
    public class FixtureReceiveSnPrintable : LabelPrintable<FixtureReceiveSn>
    {
    }
}

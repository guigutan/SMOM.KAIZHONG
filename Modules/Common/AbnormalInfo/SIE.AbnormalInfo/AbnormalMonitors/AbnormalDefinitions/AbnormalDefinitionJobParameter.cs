using SIE.Common.Schdules;
using SIE.Domain;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.AbnormalInfo.AbnormalMonitors
{

    /// <summary>
    /// 异常定义生成任务调度通用参数
    /// </summary>
    [RootEntity, Serializable]
    public class AbnormalDefinitionJobParameter : JobParameter
    {
        //#region 异常定义Id DefineId
        ///// <summary>
        ///// 异常定义Id
        ///// </summary>
        //[Label("异常定义")]
        //public static readonly Property<double?> DefineIdProperty = P<AbnormalDefinitionJobParameter>.Register(e => e.DefineId);

        ///// <summary>
        ///// 异常定义Id
        ///// </summary>
        //public double? DefineId
        //{
        //    get { return GetProperty(DefineIdProperty); }
        //    set { SetProperty(DefineIdProperty, value); }
        //}
        //#endregion

    }
}

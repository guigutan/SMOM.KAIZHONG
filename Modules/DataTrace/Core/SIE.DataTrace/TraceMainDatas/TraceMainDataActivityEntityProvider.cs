using SIE.Core.Common;
using SIE.DataTrace.Activities.Core;
using SIE.Domain;
using SIE.WorkFlow.Base.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.DataTrace.TraceMainDatas
{
    public class TraceMainDataActivityEntityProvider : IActivityEntityProvider<TraceMainData>
    {
        /// <summary>
        /// 获取检验单据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Entity GetActivityEntity(GetActivityEntityInput input)
        {
            //1.获取不合格审核流程变量
            var model = input.VariablesJson.ToJsonObjectCore<DataTraceVariable>();
            if (model.TraceMainDataId != null)
            {
                return RF.GetById<TraceMainData>(model.TraceMainDataId.Value);
            }
            return new TraceMainData();
        }
    }
}

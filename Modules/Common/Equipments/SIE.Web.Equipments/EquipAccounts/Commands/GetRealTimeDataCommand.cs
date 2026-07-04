using SIE.Core.ApiModels;
using SIE.Domain.Validation;
using SIE.EventMessages.EAP.Equipments;
using SIE.Web.Command;
using System;

namespace SIE.Web.Equipments.EquipAccounts.Commands
{
    /// <summary>
    /// 获取设备实时数据命令
    /// </summary>
    [JsCommand("SIE.Web.Equipments.EquipAccounts.Commands.GetRealTimeDataCommand")]
    public class GetRealTimeDataCommand : ViewCommand
    {
        /// <summary>
        /// 执行升级
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>执行结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var para = args.Data.ToJsonObject<EquipEapRTValuePara>();

            try
            {
                var rtn = RT.Service.Resolve<IEquipmentEap>().GetEquipEapRTValueInfo(para);

                return rtn;
            }
            catch (System.Exception)
            {
                throw new ValidationException("接口异常：".L10N() + "从MDC获取实时值失败!".L10N());
            }
        }
    }
}

using SIE.Common.Prints;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EMS.Equipments;
using SIE.EMS.Equipments.Accounts;
using SIE.EMS.Equipments.Accounts.Printables;
using SIE.EMS.Equipments.Accounts.ViewModels;
using SIE.Equipments.EquipAccounts;
using SIE.Web.Common.Prints;
using SIE.Web.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Web.EMS.Equipments.Accounts.DataQuery
{
    /// <summary>
    /// 设备台账查询器
    /// </summary>
    public class EquipAccountDataQueryer : DataQueryer
    {
        /// <summary>
        /// 通过设备台账获取设备台账子表信息
        /// </summary>
        /// <param name="account">设备台帐</param>
        /// <returns>设备台账子表信息</returns>
        public EquipAccountInfo GetEquipModelRelateInfos(EquipAccountBase account)
        {
            return RT.Service.Resolve<EquipController>().GetEquipModelRelateInfos(account);
        }

        /// <summary>
        /// 通过设备台账获取设备台账子表信息
        /// </summary>
        /// <param name="account">设备台帐</param>
        /// <returns>设备台账子表信息</returns>
        public bool GetIsCalibration(EquipAccountBase account)
        {
            return RT.Service.Resolve<EquipController>().GetIsCalibration(account);
        }

        /// <summary>
        /// 通过点检保养项目维护获取备件清单信息
        /// </summary>
        /// <param name="lubricationProject">点检保养项目维护id</param>
        /// <returns>备件清单信息</returns>
        public EquipAccountLubricationProjectInfo GetSparePartItemInfos(EquipAccountLubricationProject lubricationProject)
        {
            return RT.Service.Resolve<EquipController>().GetAccountSparePartItemInfos(lubricationProject);
        }

        /// <summary>
        /// 通过设备台账ID获取点检项目
        /// </summary>
        /// <param name="equipId"></param>
        /// <returns></returns>
        public EquipAccountInfo GetEquipCheckProjectInfos(double equipId)
        {
            return RT.Service.Resolve<EquipController>().GetEquipCheckProjectInfos(equipId);
        }

        /// <summary>
        /// 获取设备台账编码
        /// </summary>
        /// <returns>设备台账编码</returns>
        public string GetEquipAccountNo()
        {
            return RT.Service.Resolve<EquipAccountController>().GetAccountNo();
        }

        /// <summary>
        /// 通过设备台账Id获取设备台账
        /// </summary>
        /// <param name="id">设备台帐ID</param>
        /// <returns>点检项目列表</returns>
        public List<string> GetEquipAccountInfos(double id)
        {
            var account = RF.GetById<EquipAccount>(id);
            return new List<string>() { account?.Code, account?.Name };
        }

        /// <summary>
        /// 获取打印模板
        /// </summary>
        /// <param name="lotId"></param>
        /// <returns></returns>
        public QRCodePrintCfgViewModel GetPrintTemplateInfo(double lotId)
        {
            var printInfo = new QRCodePrintCfgViewModel()
            {
                Times = 1,
            };
            return printInfo;
        }

        /// <summary>
        /// 打印
        /// </summary>
        /// <param name="accountIds"></param>
        /// <param name="printInfo"></param>
        /// <param name="isReprint">是否重打印</param>
        /// <returns></returns>
        public QRCodePrintResult Print(List<double> accountIds, QRCodePrintCfgViewModel printInfo, bool isReprint=false)
        {
            var prtResult = new QRCodePrintResult();
            try
            {
                // 1.获取打印模板
                PrintTemplate template = RF.GetById<PrintTemplate>(printInfo.TemplateId);

                //2.根据类型获取报表处理对像
                var report = ReportFactory.Current.GetReportByExtension(template.Type);

                //4.创建实体打印对像 如果清楚实体打印对像自己NEW 一个出来也行
                var printable = new EquipAccountPrintable();

                prtResult.ErrMsg = string.Empty;

                if (template.EntityType != typeof(EquipAccountPrintable).GetQualifiedName())
                    throw new ValidationException("打印模板错误，请配置【条码】类型的模板！".L10N());
                prtResult.Type = template.Type;
                var equipAccounts = RT.Service.Resolve<EquipAccountController>().GetEquipAccountsByIds(accountIds);
                if (!equipAccounts.Any())
                {
                    throw new ValidationException("请选择需要打印的数据！".L10N());
                }                

                //5.调用打印处理函数返回打印模板BASE64字符串到前台，用于传输到打印预览页面
                prtResult.Url = report.PrintProcess(printable, template.Id, template.Content, () =>
                {
                    prtResult.ErrMsg = RT.Service.Resolve<EquipAccountController>().PrintEquipAccounts(equipAccounts.ToList(), "", printInfo.Times);
                    return equipAccounts;
                });

            }
            catch (Exception exc)
            {
                prtResult.ErrMsg = exc.Message;
            }

            return prtResult;
        }

        /// <summary>
        /// 获取指定设备的维修定标Id集合
        /// </summary>
        /// <param name="equipmentId"></param>
        /// <returns></returns>
        public List<double> GetRunStandardValueIds(double equipmentId)
        {
            return RT.Service.Resolve<ElecEquipController>().GetRunStandardValueIds( equipmentId);
        }

    }
}

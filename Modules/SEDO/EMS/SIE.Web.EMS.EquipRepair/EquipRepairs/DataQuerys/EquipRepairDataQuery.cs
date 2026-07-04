using Microsoft.AspNetCore.StaticFiles;
using SIE.Common.Attachments;
using SIE.Common.Configs;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EMS.DevicePurs;
using SIE.EMS.EquipRepair.Controller;
using SIE.EMS.EquipRepair.EquipRepairs;
using SIE.EMS.EquipRepair.EquipRepairs.Configs;
using SIE.EMS.EquipRepairs.Enums;
using SIE.Equipments.EquipAccounts;
using SIE.Resources.Employees;
using SIE.Web.Data;
using SIE.Web.EMS.EquipRepair.EquipRepairs.ViewModels.Enums;
using System;
using System.IO;
using System.Linq;

namespace SIE.Web.EMS.EquipRepair.EquipRepairs.DataQuerys
{
    /// <summary>
    /// dataquery
    /// </summary>
    public class EquipRepairDataQuery : DataQueryer
    {
        /// <summary>
        /// 验证维修报告是否填写完整
        /// </summary>
        /// <param name="repairBillId">设备维修单Id</param>
        /// <returns></returns>
        public object VerifyRepairReport(double repairBillId)
        {
            return RT.Service.Resolve<RepairController>().VerifyRepairReport(repairBillId);
        }

        /// <summary>
        /// 验证维修报告是否加入过经验库
        /// </summary>
        /// <param name="repairNo">设备维修单单号</param>
        /// <returns>bool</returns>
        public object VerifyRepairReportIsAddDeopt(string repairNo)
        {
            return RT.Service.Resolve<RepairController>().VerifyRepairReportIsAddDeopt(repairNo);
        }

        /// <summary>
        /// 获取设备维修权限和维修报告填写权限
        /// </summary>
        /// <param name="repairBillId">设备维修单Id</param>
        /// <returns>object</returns>
        public object GetRepairReportPermit(double repairBillId)
        {
            EquipRepairBill repairBill = RF.GetById<EquipRepairBill>(repairBillId, new EagerLoadOptions().LoadWithViewProperty());
            EntityList<Employee> equipEmployees = null;
            bool isRepairPermit = true;

            //获取拥有设备维修权限员工列表
            if (repairBill.RepairType == EquipRepairType.EquipRepair)
            {
                equipEmployees = RT.Service.Resolve<DevicePurController>().GetDevicePurRepairEmployees(true);
                //是否拥有设备维修的权限
                if (!equipEmployees.Select(p => p.Id).Contains(RT.IdentityId))
                {
                    isRepairPermit = false;
                }
            }
            //维修状态不等于维修中,待确认，待评分
            if (repairBill.RepairState != EquipRepairState.Repairing && repairBill.RepairState != EquipRepairState.WaitConfirm && repairBill.RepairState != EquipRepairState.WaitScore)
            {
                return new { reportPermit = false, repairPermit = isRepairPermit };
            }
            else if (repairBill.RepairState == EquipRepairState.Repairing || repairBill.RepairState == EquipRepairState.WaitConfirm)
            {
                //单据状态为维修中或待确认且当前用户为维修人员
                if (repairBill.RepairMasterId != RT.IdentityId && !repairBill.RepairEmployeeIds.Split(',').Contains(RT.IdentityId.ToString()))
                {
                    return new { reportPermit = false, repairPermit = isRepairPermit };
                }
            }
            else
            {
               EntityList<Employee> employees = null;
                //获取拥有维修确认权限员工列表
                if (repairBill.RepairType == EquipRepairType.EquipRepair)
                {
                    employees = RT.Service.Resolve<DevicePurController>().GetDevicePurRepairEmployees(true);
                }
                else
                {
                    employees = RT.Service.Resolve<DevicePurController>().GetDevicePurRepairEmployees(false);
                }
                //单据状态为待评分且为维修人员或评分人员
                if (!employees.Select(p => p.Id).Contains(RT.IdentityId) && repairBill.RepairMasterId != RT.IdentityId && !repairBill.RepairEmployeeIds.Split(',').Contains(RT.IdentityId.ToString()))
                {
                    return new { reportPermit = false, repairPermit = isRepairPermit };
                }
            }
            return new { reportPermit = true, repairPermit = isRepairPermit };
        }

        /// <summary>
        /// 获取维修响应时间、执行时间、总工时
        /// </summary>
        /// <param name="repairBillId">设备维修单Id</param>
        /// <returns>维修响应时间、执行时间、总工时</returns>
        public object GetRepairTime(double repairBillId)
        {
            EquipRepairBill repairBill = RF.GetById<EquipRepairBill>(repairBillId, new EagerLoadOptions().LoadWithViewProperty());

            //获取该设备维修单的交机确认为OK的时间
            var handoverOkTime = RT.Service.Resolve<RepairController>().GetHandoverOkTime(repairBill.Id);
            DateTime dateTime;
            if (!handoverOkTime.HasValue)
            {
                if (repairBill.RepairFinishDate == null)
                {
                    throw new ValidationException("维修单【{0}】的没有启用交机确认，且维修完成时间为空。".L10N());
                }
                else
                {
                    dateTime = repairBill.RepairFinishDate.Value;
                }
            }
            else
            {
                dateTime = handoverOkTime.Value;
            }

            double respondTime = (repairBill.RepairBeginDate - repairBill.ApplyRepairDate).Value.TotalHours;
            double executeTime = (dateTime - repairBill.RepairBeginDate).Value.TotalHours;
            double repairTotalTime = repairBill.EquipRepairWorkingHoursList.Where(p => p.EndTime != null).Sum(p => (p.EndTime - p.BeginTime).Value.TotalHours);
            var result = new
            {
                RespondTime = Math.Round(respondTime, 2),
                ExecuteTime = Math.Round(executeTime, 2),
                RepairTotalTime = Math.Round(repairTotalTime, 2)
            };
            return result;
        }

        /// <summary>
        /// 自动获取编码
        /// </summary>
        /// <returns></returns>
        public string GetCode()
        {
            var code = RT.Service.Resolve<RepairController>().GenerateRepairNo();
            return code;
        }

        /// <summary>
        /// 获取设备维修信息
        /// </summary>
        /// <returns></returns>
        public object GetEquipRepairInfo(double repairId)
        {
            var repairBill = RF.GetById<EquipRepairBill>(repairId, new EagerLoadOptions().LoadWithViewProperty());
            repairBill.EquipWarrantyState = repairBill.RepairType == EquipRepairType.EquipRepair ? GetEquipWarrantyState((double)repairBill.EquipAccountId) : "";
            return repairBill;
        }

        /// <summary>
        /// 获取设备保修状态
        /// </summary>
        /// <returns></returns>
        public string GetEquipWarrantyState(double equipId)
        {
            var equipAccount = RT.Service.Resolve<EquipAccountController>().GetEquipAccountByIdNoDataAuth(equipId);
            
            if (equipAccount == null)
            {
                throw new ValidationException("没有该设备的访问权限".L10N());
            }

            string warrantyState ;

            if (equipAccount.WarrantyPeriod == null)
            {
                warrantyState = "";
            }
            else if (equipAccount.WarrantyPeriod >= DateTime.Now)
            {
                warrantyState = "保修期内".L10N();
            }
            else
            {
                warrantyState = "保修期外".L10N();
            }

            return warrantyState;

        }

        /// <summary>
        /// 获取接单详细信息
        /// </summary>
        /// <param name="equipAccountId"></param>
        /// <returns></returns>
        public object GetTakeOderInfo(double equipAccountId)
        {
            Employee employee = RF.GetById<Employee>(RT.IdentityId);

            EquipAccount account = RT.Service.Resolve<EquipAccountController>().GetEquipAccountByIdNoDataAuth(equipAccountId);

            if (account == null)
            {
                throw new ValidationException("此单无设备台账".L10N());
            }

            var warrantyPerio = account.WarrantyPeriod;

            DateTime now = DateTime.Now;

            GuaranteeRangeType guaranteeRangeType = GuaranteeRangeType.GuaranteeRangeIn;
            if (warrantyPerio > now)
            {
                guaranteeRangeType = GuaranteeRangeType.GuaranteeRangeOut;
            }

            var result = new
            {
                Employee = employee,
                GuaranteeRange = warrantyPerio,
                GuaranteeRangeType = guaranteeRangeType,
            };
            return result;
        }


        /// <summary>
        /// 判断当前用户是否有权限操作
        /// </summary>
        /// <param name="reqairId"></param>
        /// <returns></returns>
        public object PeopleIsCanOperation(double reqairId)
        {
            var equipRepairBill = RF.GetById<EquipRepairBill>(reqairId);

            var devicePur = RT.Service.Resolve<DevicePurController>().GetDevicePurByNowUserId();

            if (equipRepairBill != null && devicePur != null && equipRepairBill.RepairType == EquipRepairType.EquipRepair && devicePur.EquipMaintain)
            {
                return true;
            }

            return null;
        }

        /// <summary>
        /// 验证设备是否允许报修
        /// </summary>
        /// <param name="equipId">设备台账ID</param>
        /// <returns></returns>
        public bool ValidateEquipAccountApplyRepair(double equipId)
        {
            RT.Service.Resolve<RepairController>().ValidateEquipAccountApplyRepair(equipId);
            return true;
        }

        /// <summary>
        /// 保存附件（上传）
        /// </summary>        
        /// <param name="fileContent">文件内容</param>
        /// <param name="fileName">文件名</param>
        /// <returns></returns>
        public string SaveRepairAttachment(string fileContent, string fileName)
        {
            if (fileContent.Length <= 0)
            {
                throw new ValidationException("文件内容为空，不能上传。".L10N());
            }

            if (fileContent.Split(',').Length > 1)
            {
                var bytes = Convert.FromBase64String(fileContent.Split(',')[1]);
                const string repairPath = "RepairAttachment";
                var path = $"{repairPath}/{Guid.NewGuid()}";
                RT.Service.Resolve<AttachmentController>().FileStorage(fileName, bytes, path);
                var CompletePath = $"{path}/{fileName}";

                return CompletePath;
            }
            else
            {
                throw new ValidationException("文件内容异常，不能上传。".L10N());
            }
        }

        /// <summary>
        /// 上传委外维修
        /// </summary>
        ///  <param name="id">维修单id</param>
        /// <param name="fileContent"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public string UploadOutsourcedMaintenanceReport(double id, string fileContent, string fileName)
        {
            if (fileContent.Length <= 0)
            {
                throw new ValidationException("文件内容为空，不能上传。".L10N());
            }

            if (fileContent.Split(',').Length > 1)
            {
                var bytes = Convert.FromBase64String(fileContent.Split(',')[1]);
                const string repairPath = "RepairAttachment";
                var path = $"{repairPath}/{Guid.NewGuid()}";
                RT.Service.Resolve<SIE.Common.Attachments.AttachmentController>().FileStorage(fileName, bytes, path);
                var CompletePath = $"{path}/{fileName}";
                RT.Service.Resolve<RepairController>().SaveOutsourcedMaintenanceReport(id, CompletePath);
                return CompletePath;
            }
            else
            {
                throw new ValidationException("文件内容异常，不能上传。".L10N());
            }
        }

        /// <summary>
        /// 下载图片附件
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="fileName">文件名</param>
        /// <returns></returns>
        public object DownLoadPictureAttachment(string filePath, string fileName)
        {
            var pathFileName = System.IO.Path.GetFileName(filePath);
            var fileBytes = RT.Service.Resolve<AttachmentController>().FileDownload(filePath, pathFileName);
            return new { FileName = fileName, FileContent = fileByteToBase64(fileBytes, pathFileName) };
        }

        private string fileByteToBase64(byte[] buffer, string fileName)
        {
            Check.NotNullOrWhiteSpace(fileName, nameof(fileName));
            Check.NotNull(buffer, nameof(buffer));

            var fileExt = Path.GetExtension(fileName);
            if (fileExt.IsNullOrWhiteSpace())
            {
                throw new ValidationException("{0}文件没有扩展名，无法解析对应的文件类型".L10nFormat(fileName));
            }
            var provider = new FileExtensionContentTypeProvider();
            var contentType = provider.Mappings[fileExt];
            if (contentType.IsNullOrWhiteSpace())
            {
                throw new ValidationException("{0}扩展名无法解析对应的类型，请在应用服务的{1}节点配置中进行维护".L10nFormat(fileExt, "MimeMap"));
            }
            return "data:{0};base64,{1}".FormatArgs(contentType, Convert.ToBase64String(buffer));
        }


        /// <summary>
        /// 根据设备Id获取设备
        /// </summary>
        /// <param name="id">设备Id</param>
        /// <returns></returns>
        public EquipAccount GetEquipAccountById(double id)
        {
            return RT.Service.Resolve<EquipAccountController>().GetEquipAccountById(id);
        }

        /// <summary>
        /// 判断当前设备是否有未完成的维修单
        /// </summary>
        /// <param name="equipAccountId">设备台账id</param>
        /// <returns></returns>
        public string CheckPlanWithUnFinishRepairBill(double equipAccountId)
        {
            return RT.Service.Resolve<RepairController>().CheckPlanWithUnFinishRepairBill(equipAccountId);
        }

        /// <summary>
        /// 判断当前点检单是否有维修单
        /// </summary>
        /// <param name="equipAccountId">设备台账id</param>
        /// <param name="sourceNo">点检单号</param>
        /// <param name="sourceType">来源类型</param>
        /// <returns></returns>
        public bool CheckPlanWithRepairBill(double equipAccountId, string sourceNo, int sourceType)
        {
            return RT.Service.Resolve<RepairController>().CheckPlanWithRepairBill(equipAccountId, sourceNo, sourceType);
        }

        /// <summary>
        /// 校验维修报告页签是否填写
        /// </summary>
        /// <param name="repairBill">维修报告数据</param>
        /// <param name="activeTab">是否打开维修报告页签</param>
        /// <returns></returns>
        public string FinishiRepairValidate(EquipRepairBill repairBill, bool activeTab)
        {
            // 如果没激活维修报告页签，需要取数据库数据
            if (!activeTab)
            {
                repairBill = RT.Service.Resolve<RepairController>().GetEquipRepairBill(repairBill.Id);
            }
            return RT.Service.Resolve<RepairController>().FinishiRepairValidate(repairBill);
        }
    }
}

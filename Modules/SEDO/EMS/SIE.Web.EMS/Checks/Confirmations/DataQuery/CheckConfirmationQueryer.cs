using Newtonsoft.Json.Linq;
using SIE.Domain.Validation;
using SIE.EMS.Checks;
using SIE.EMS.Checks.ApiModels;
using SIE.EMS.Checks.Confirmations;
using SIE.EMS.Common.Utils;
using SIE.EMS.Enums;
using SIE.Web.Data;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace SIE.Web.EMS.Checks.Confirmations.DataQuery
{
    /// <summary>
    /// “点检确认”查询器
    /// </summary>
    public class CheckConfirmationQueryer : DataQueryer
    {
        /// <summary>
        /// 初始化当前用户对该点检单的数据
        /// </summary>
        /// <param name="checkPlanId">点检单Id</param>
        /// <param name="checkPlanNo">点检单号</param>
        /// <param name="equipAccountId">设备台账Id</param>
        /// <param name="departmentId"></param>
        /// <returns></returns>
        public object InitConfirmCheckPlan(double checkPlanId, string checkPlanNo, double equipAccountId, double departmentId)
        {
            //获取评分项结果和备注
            var checkConfirmItem = RT.Service.Resolve<CheckPlanController>().GetCheckPlanConfirmItemByPlanId(checkPlanId, true);

            var dptConfirm = checkConfirmItem.FirstOrDefault(p => p.DepartmentId == departmentId);
            return new
            {
                ConfirmResult = dptConfirm.ConfirmResult,
                ConfirmNote = dptConfirm.ConfirmNote,
                CheckExeState = dptConfirm.CheckExeState,
                ConfirmDeptId = departmentId,
            };
        }

        /// <summary>
        /// 保存图片
        /// </summary>
        /// <param name="fileContent"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public string SavePicture(string fileContent, string fileName)
        {
            var hepler = new FileUrlHelper();
            var checkConfirmation = hepler.GenerateAttachmentBase64StringContent(new CheckConfirmation(), fileContent, fileName) as CheckConfirmation;

            return checkConfirmation.ToJsonString();
        }

        /// <summary>
        /// 提交点检评分项
        /// </summary>
        /// <param name="jsonString">CheckConfirmation[]的json</param>
        /// <param name="planInfo">点检计划信息</param>
        /// <returns></returns>
        public string SubmitCheckConfirmation(string jsonString, CheckPlanInfo planInfo)
        {
            JArray jsonArr = JArray.Parse(jsonString);

            var list = new List<CheckConfirmationSubmitInfo>();
            foreach (var item in jsonArr)
            {
                if (item["Score"].ToString().IsNullOrEmpty() || (int)item["Score"] == 0)
                {
                    throw new ValidationException(item["ProjectName"] + " 项目未评分，请评分!".L10N());
                }
                if (item["ConfirmResult"].ToString().IsNullOrEmpty())
                {
                    throw new ValidationException("点检确认结果不能为空！".L10N());
                }
                if ((int)item["ConfirmResult"] == (int)ConfirmResult.NG && string.IsNullOrWhiteSpace((string)item["ConfirmNote"]))
                {
                    throw new ValidationException("点检确认结果为不合格时要进行备注!".L10N());
                }

                list.Add(new CheckConfirmationSubmitInfo()
                {
                    CheckPlanId = (double?)item["OwnerId"],
                    TpmScoreProjectId = (double?)item["TpmScoreProjectId"],
                    Score = (int)item["Score"],
                    ConfirmDeptId = (double?)item["ConfirmDeptId"],
                    ConfirmResult = (int)item["ConfirmResult"],
                    ConfirmNote = item["ConfirmNote"].ToString(),
                    FileName = item["FileName"].ToString(),
                    Content = item["Content"].ToString(),
                    FilePath = item["FilePath"].ToString(),
                    FileExtesion = item["FileExtesion"].ToString(),
                    FileSize = item["FileSize"].ToString()

                });
            }
            RT.Service.Resolve<CheckController>().SubmitCheckConfirmation(list.ToArray(), planInfo);
            return "";
        }

        /// <summary>
        /// 是否具有点检确认权限
        /// </summary>
        /// <param name="checkPlanId">点检计划ID</param>
        /// <param name="confirmDeptId">确认部门ID</param>
        /// <returns></returns>
        public bool CanSubmitCheckConfirmation(double checkPlanId, double confirmDeptId)
        {
            return RT.Service.Resolve<CheckPlanController>().CanSubmitCheckConfirmation(checkPlanId, confirmDeptId);
        }

        /// <summary>
        /// 获取是否评分配置项
        /// </summary>
        /// <returns></returns>
        public bool IsNeedMarkScore()
        {
            return RT.Service.Resolve<CheckPlanController>().IsNeedMarkScore();
        }
    }
}

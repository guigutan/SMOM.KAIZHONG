using Newtonsoft.Json.Linq;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EMS.Common.Utils;
using SIE.EMS.Enums;
using SIE.EMS.Maintains.ApiModels;
using SIE.EMS.Maintains.Confirmations;
using SIE.EMS.Maintains.Controller;
using SIE.EMS.Maintains.Plans;
using SIE.Web.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Web.EMS.EquipMaint.Maintains.Confirmations.DataQuery
{
    /// <summary>
    /// “保养确认”查询器
    /// </summary>
    public class MaintainConfirmationQueryer : DataQueryer
    {
        /// <summary>
        /// 初始化当前用户对该保养单的数据
        /// </summary>
        /// <param name="maintainPlanId">保养单Id</param>
        /// <param name="maintainPlanNo">保养单号</param>
        /// <param name="equipAccountId">设备台账Id</param>
        /// <param name="departmentId">确认部门Id</param>
        /// <returns></returns>
        public object InitConfirmMaintainPlan(double maintainPlanId, string maintainPlanNo, double equipAccountId, double departmentId)
        {
            //获取上次保养小结
            var upMaintainSummary = RT.Service.Resolve<MaintainController>().GetLastMaintainSummary(equipAccountId, departmentId);

            // 确认项目
            var maintainConfirmItem = RT.Service.Resolve<MaintainController>().GetMaintainConfirmations(new List<double> { maintainPlanId });
            var confirmItem = maintainConfirmItem.FirstOrDefault(p => p.DepartmentId == departmentId);

            //获取计划单
            var plan = RF.GetById<MaintainPlan>(maintainPlanId);

            return new
            {
                UpMaintainSummary = upMaintainSummary,
                ConfirmDeptId = departmentId,
                ConfirmResult = confirmItem.ConfirmResult,
                ConfirmNote = confirmItem.ConfirmNote,
                MaintianExeState = confirmItem.MaintExeState,
                PrecisePlanBeginDate = plan?.PrecisePlanBeginDate == null ? plan?.PlanBeginDate : plan?.PrecisePlanBeginDate,
                PrecisePlanEndDate = plan?.PrecisePlanEndDate == null ? plan?.PlanEndDate : plan?.PrecisePlanEndDate
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
            var maintainConfirmation = new MaintainConfirmation();

            FileUrlHelper.GenerateMaintainConfirmationBase64StringContent(maintainConfirmation, fileContent, fileName);

            return maintainConfirmation.ToJsonString();
        }

        /// <summary>
        /// 提交保养评分项
        /// </summary>
        /// <param name="jsonString">MaintainConfirmationSubmitInfo[]的json</param>
        /// <param name="planInfo">保养信息</param>
        /// <returns></returns>
        public string SubmitMaintainConfirmation(string jsonString, MaintainPlanInfo planInfo)
        {
            JArray jsonArr = JArray.Parse(jsonString);

            var list = new List<MaintainConfirmationSubmitInfo>();
            foreach (var item in jsonArr)
            {
                if (item["Score"] == null || (int)item["Score"] == 0)
                {
                    throw new ValidationException(item["ProjectName"] + "项目未评分，请评分!".L10N());
                }
                if (item["ConfirmResult"].ToString().IsNullOrEmpty())
                {
                    throw new ValidationException("保养确认结果不能为空！".L10N());
                }
                if ((int)item["ConfirmResult"] == (int)ConfirmResult.NG && string.IsNullOrWhiteSpace((string)item["ConfirmNote"]))
                {
                    throw new ValidationException("保养确认结果不合格时要进行备注!".L10N());
                }

                list.Add(new MaintainConfirmationSubmitInfo()
                {
                    MaintainPlanId = (double?)item["MaintainPlanId"],
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
            RT.Service.Resolve<MaintainController>().SubmitMaintainConfirmation(list.ToArray(), planInfo);
            return "";
        }

        /// <summary>
        /// 是否具有保养确认权限
        /// </summary>
        /// <param name="maintainPlanId">保养计划ID</param>
        /// <param name="confirmDeptId">确认部门ID</param>
        /// <returns></returns>
        public bool CanSubmitMaintainConfirmation(double maintainPlanId, double confirmDeptId)
        {
            return RT.Service.Resolve<MaintainController>().CanSubmitMaintainConfirmation(maintainPlanId, confirmDeptId);
        }

        /// <summary>
        /// 获取是否评分配置项
        /// </summary>
        /// <returns></returns>
        public bool IsNeedMarkScore()
        {
            return RT.Service.Resolve<MaintainController>().IsNeedMarkScore();
        }
    }
}

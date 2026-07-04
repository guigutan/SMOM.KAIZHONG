using SIE.Api;
using SIE.Common.Employees;
using SIE.Core.Inspections;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.QMS.ApiModel;
using SIE.QMS.InspectionGroups;
using System;
using System.Collections.Generic;

namespace SIE.Kit.QMS.InspectionGroups
{
    /// <summary>
    /// 电子套件检验组API接口控制器
    /// </summary>
    public partial class KitInspectionGroupController : InspectionGroupController
    {
        /// <summary>
        /// 与当前员工关联的来料检验组查询
        /// </summary>
        /// <param name="queryInfo"></param>
        /// <returns></returns>
        [ApiService("与当前员工关联的来料检验组查询")]
        [return: ApiReturn("检验组信息集合. 参数类型: InspGroupInfoList")]
        public virtual InspGroupInfoList GetIqcInspectionGroups([ApiParameter("检验组查询信息：{EmployeeId:员工ID}")] InspGroupQueryInfo queryInfo)
        {
            var result = new InspGroupInfoList();
            var query = Query<InspectionGroup>().Join<InspectGroupUser>((x, y) => x.Id == y.InspectionGroupId).Join<InspectGroupUser, Employee>((y, z) => y.UsersId == z.Id).Where<InspectGroupUser, Employee>((x, y, z) => z.Id == queryInfo.EmployeeId);
            query.Where(p => p.InspectionType == InspectionType.IncomingInsp);
            var list = query.ToList();
            var totalCount = query.Count();
            List<InspGroup> inspGroupList = new List<InspGroup>();
            foreach (var inspGroupInfo in list)
            {
                if (!inspGroupInfo.InspectionType.HasValue)
                    throw new ValidationException("检验组[{0}]未绑定检验类型".L10nFormat(inspGroupInfo.Name));
                inspGroupList.Add(new InspGroup
                {
                    Id = inspGroupInfo.Id,
                    Code = inspGroupInfo.Code,
                    Name = inspGroupInfo.Name,
                    InspType = inspGroupInfo.InspectionType.Value
                });
            }
            result.InspGroupList = inspGroupList;
            result.TotalCount = totalCount;
            return result;
        }
    }
}

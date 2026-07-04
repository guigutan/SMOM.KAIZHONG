using DocumentFormat.OpenXml.Wordprocessing;
using SIE.Andon.Andons.APIModel;
using SIE.Andon.Andons.ViewModels;
using SIE.Common.Organizations;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EventMessages.MES.Andons;
using SIE.Rbac.Users;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Andon.Andons
{
    /// <summary>
    /// 安灯维护控制器
    /// </summary>
    public class AndonController : DomainController, IAndon
    {
        #region 获取组织架构的部门数据
        /// <summary>
        /// 获取组织架构的部门数据
        /// </summary>
        /// <param name="pagingInfo"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public virtual EntityList<Organization> GetOrganizations(PagingInfo pagingInfo, string keyword)
        {
            var query = Query<Organization>()
                .WhereIf(!keyword.IsNullOrEmpty(), p => p.Code.Contains(keyword) || p.Name.Contains(keyword))
                .Where(p => p.Level.Type == OrganizationType.Department && p.InvOrgId == RT.InvOrg).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
            query.ForEach(organization =>
            {
                organization.TreePId = null;
            });
            return query;
        }
        #endregion

        #region 根据安灯类型获取其下消息推送
        /// <summary>
        /// 根据安灯类型获取其下消息推送
        /// </summary>
        /// <param name="andonTypeId"></param>
        /// <param name="andonTypeMessageSendIds"></param>
        /// <returns></returns>
        public virtual EntityList<AndonMessageSend> GetAndonTypeMessageSends(double andonTypeId, List<double> andonTypeMessageSendIds)
        {
            var query = Query<AndonTypeMessageSend>()
                .Where(p => p.AndonTypeId == andonTypeId)
                .OrderByDescending(p => p.UpdateDate) //根据更新时间降序获取
                .OrderBy(p => p.CreateDate)
                .ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            var andonMessageSendList = new EntityList<AndonMessageSend>();
            query.ForEach(item =>
            {
                var andonMessageSend = new AndonMessageSend
                {
                    Node = item.Node,
                    Minute = item.Minute,
                    PushPlugId = item.PushPlugId,
                    PushPlugName = item.PushPlugName,
                    MessageTemplate = item.MessageTemplate,
                };
                andonTypeMessageSendIds.Add(item.Id);
                andonMessageSendList.Add(andonMessageSend);
            });
            return andonMessageSendList;
        }
        #endregion

        #region 根据安灯类型获取其下消息推送的推送对象
        /// <summary>
        /// 根据安灯类型获取其下消息推送的推送对象
        /// </summary>
        /// <param name="andonTypePushObjectList"></param>
        /// <param name="andonTypeMessId"></param>
        /// <returns></returns>
        public virtual EntityList<AndonPushObject> GetAndonTypePushObjects(EntityList<AndonTypePushObject> andonTypePushObjectList, double andonTypeMessId)
        {
            var list = andonTypePushObjectList.Where(p => p.MessageSendId == andonTypeMessId).ToList();
            var andonPushObjectList = new EntityList<AndonPushObject>();
            list.ForEach(item => {
                var andonPushObject = new AndonPushObject
                {
                    Type = item.Type,
                    Code = item.Code,
                    Name = item.Name,
                };
                andonPushObjectList.Add(andonPushObject);
            });
            return andonPushObjectList;
        }
        #endregion

        #region 安灯责任组维护基础表

        /// <summary>
        /// 获取用户
        /// </summary>
        /// <param name="key"></param>
        /// <param name="pagingInfo"></param>
        /// <returns></returns>
        public virtual EntityList<User> GetUsers(string key,PagingInfo pagingInfo)
        {
            var q = Query<User>();

            if (!key.IsNullOrEmpty())
                q.Where(p => p.Code.Contains(key) || p.Employee.Name.Contains(key));
            var list = q.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());

            return list;
        }

        /// <summary>
        /// 替换责任人
        /// </summary>
        /// <param name="model"></param>
        public virtual void ReplaceDetailUser(UserReplaceViewModel model)
        {
            if (model.UserId == 0)
                throw new ValidationException("用户必填!".L10N());
            if(model.ReplaceUserId == 0)
                throw new ValidationException("替换用户必填!".L10N());
            if (model.ReplaceUser.State == State.Disable)
                throw new ValidationException("替换用户为禁用状态,不能进行替换!".L10N());
            if (model.ReplaceUser.Employee.EmployeeStatus == Resources.EmployeeStatus.UnJob)
                throw new ValidationException("替换用户对应的员工为离职状态,不能进行替换!".L10N());
            var list = Query<AndonGroupDetail>().Where(p => p.UserId == model.UserId).ToList();
            if (list.Count > 0)
            {
                foreach (var l in list)
                {
                    l.UserId = model.ReplaceUserId;
                    l.PersistenceStatus = PersistenceStatus.Modified;
                }
                RF.Save(list);
            }
        }

        /// <summary>
        /// 安灯责任组维护基础表查询方法
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public virtual EntityList<AndonGroup> CriteriaAndonGroup(AndonGroupCriteria criteria)
        {
            var q = DB.Query<AndonGroup>("ag");
            if (!criteria.Code.IsNullOrEmpty())
                q.Where(p => p.Code.Contains(criteria.Code));
            if (!criteria.Name.IsNullOrEmpty())
                q.Where(p => p.Name.Contains(criteria.Name));
            if (!criteria.UserCode.IsNullOrEmpty())
                q.Where(p => p.SQL<bool>("exists(select 1 from ANDON_GROUP_DTL agd inner join SYS_USER su on su.id = agd.user_id and su.is_phantom = 0 where agd.is_phantom = 0 and agd.Andon_Group_Id = ag.id and {0})".L10nFormat(criteria.UserCode.Contains("%") ? $"su.code like '{criteria.UserCode}'" : $"su.code = '{criteria.UserCode}'")));
            if (!criteria.UserName.IsNullOrEmpty())
                q.Where(p => p.SQL<bool>("exists(select 1 from ANDON_GROUP_DTL agd inner join SYS_USER su on su.id = agd.user_id and su.is_phantom = 0 inner join RES_EMP re on re.is_phantom = 0 and re.id = su.employee_id where agd.is_phantom = 0 and agd.Andon_Group_Id = ag.id and {0})".L10nFormat(criteria.UserName.Contains("%") ? $"re.name like '{criteria.UserName}'" : $"re.name = '{criteria.UserName}'")));
            if (!criteria.UserState.ToString().IsNullOrEmpty())
                
            if (criteria.UserState.ToString() == "Disable") {
                q.Where(p => p.SQL<bool>("exists(select 1 from ANDON_GROUP_DTL agd inner join SYS_USER su on su.id = agd.user_id and su.is_phantom = 0 inner join RES_EMP re on re.is_phantom = 0 and re.id = su.employee_id where agd.is_phantom = 0 and agd.Andon_Group_Id = ag.id and {0})".L10nFormat($"su.state = 0")));
            }
            else if (criteria.UserState.ToString() == "Enable")
            {
                q.Where(p => p.SQL<bool>("exists(select 1 from ANDON_GROUP_DTL agd inner join SYS_USER su on su.id = agd.user_id and su.is_phantom = 0 inner join RES_EMP re on re.is_phantom = 0 and re.id = su.employee_id where agd.is_phantom = 0 and agd.Andon_Group_Id = ag.id and {0})".L10nFormat($"su.state = 1")));
            }
            /*            else if (criteria.UserState.ToString() == "All")
                        {
                            q.Where(p => p.SQL<bool>("exists(select 1 from ANDON_GROUP_DTL agd inner join SYS_USER su on su.id = agd.user_id and su.is_phantom = 0 inner join RES_EMP re on re.is_phantom = 0 and re.id = su.employee_id where agd.is_phantom = 0 and agd.Andon_Group_Id = ag.id and {0})".L10nFormat($"su.state = 1 or su.state = 0")));
                        }*/
            q.Where(p => p.SQL<bool>("exists(select 1 from ANDON_GROUP_DTL agd inner join SYS_USER su on su.id = agd.user_id and su.is_phantom = 0 inner join RES_EMP re on re.is_phantom = 0 and re.id = su.employee_id where agd.is_phantom = 0 and agd.Andon_Group_Id = ag.id and {0})".L10nFormat($"su.state = 1 or su.state = 0")));

            var list = q.OrderBy(criteria.OrderInfoList).ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
            return list;
        }

        public virtual void DeleteAndonGroupDetailUser(double userId)
        {
            DB.Delete<AndonGroupDetail>().Where(p => p.UserId == userId).Execute();
        }


        /// <summary>
        /// 根据安灯管理获取安灯责任组用户
        /// </summary>
        /// <param name="AndonManageId"></param>
        /// <returns></returns>
        public virtual EntityList<AndonGroupDetail> GetAndonGroupDetailsByAndonManageId(double andonManageId, double andonUpholdId)
        {
            var list = Query<AndonGroupDetail>().Join<AndonGroup>((x, y) => x.AndonGroupId == y.Id)
                .Join<AndonGroup, AndonResponseDetail>((x, y) => x.Id == y.AndonGroupId && y.AndonUpholdId == andonUpholdId)
                .Join<AndonResponseDetail, Andon>((x, y) => x.AndonId == y.Id)
                .Join<Andon, AndonManage>((x, y) => x.Id == y.AndonId && y.Id == andonManageId).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            return list;               
        }

        /// <summary>
        /// 添加安灯责任组维护基础表明细
        /// </summary>
        /// <param name="datas"></param>
        public virtual void SaveAndonGroupDetail(List<AndonGroupDetail> datas)
        {
            EntityList<AndonGroupDetail> list = new EntityList<AndonGroupDetail>();
            foreach (var data in datas)
            {
                AndonGroupDetail detail = new AndonGroupDetail();

                detail.PersistenceStatus = PersistenceStatus.New;
                detail.AndonGroupId = data.AndonGroupId;
                detail.UserId = data.UserId;
                list.Add(detail);
            }
            if (list.Count > 0)
                RF.Save(list);
        }


        #endregion

        /// <summary>
        /// 根据安灯类型Id获取安灯类型
        /// </summary>
        /// <param name="andonTypeId"></param>
        /// <returns></returns>
        public virtual EntityList<AndonType> GetAndonTypes(double andonTypeId)
        {
            return Query<AndonType>().Where(p => p.Id == andonTypeId).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取所有的安灯类型列表
        /// </summary>
        /// <param name="paging"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public virtual EntityList<AndonType> GetAndonTypes(PagingInfo paging, string keyword)
        {
            var q = Query<AndonType>();
            if (keyword.IsNotEmpty()) {
                q.Where(p => p.AndonTypeCode.Contains(keyword) || p.AndonTypeName.Contains(keyword));
            }
            return q.ToList(paging, new EagerLoadOptions().LoadWithViewProperty());
        }
        
        /// <summary>
        /// 获取所有的安灯维护列表
        /// </summary>
        /// <param name="paging"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public virtual EntityList<Andon> GetAndonList(PagingInfo paging, string keyword)
        {
            var q = Query<Andon>();
            if (keyword.IsNotEmpty())
            {
                q.Where(p => p.AndonCode.Contains(keyword) || p.AndonName.Contains(keyword));
            }
            return q.ToList(paging, new EagerLoadOptions().LoadWithViewProperty());
        }
        /// <summary>
        /// 查询安灯维护列表
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public virtual EntityList<Andon> GetAndons(AndonCriteria criteria) { 
            var q = Query<Andon>();
            if (criteria.Code.IsNotEmpty()) {
                q.Where(p => p.AndonCode.Contains(criteria.Code));
            }
            if (criteria.Name.IsNotEmpty())
            {
                q.Where(p => p.AndonName.Contains(criteria.Name));
            }
            if (criteria.AndonTypeId.HasValue)
            {
                q.Where(p => p.AndonTypeId==criteria.AndonTypeId);
            }
            if (criteria.AndonClass.HasValue)
            {
                q.Where(p => p.AndonClass == criteria.AndonClass.Value);
            }
            if (criteria.State.HasValue)
            {
                q.Where(p => p.State == criteria.State.Value);
            }
            if (criteria.CreateTime.BeginValue.HasValue)
            {
                q.Where(p => p.CreateDate>=criteria.CreateTime.BeginValue);
            }
            if (criteria.CreateTime.EndValue.HasValue)
            {
                q.Where(p => p.CreateDate <= criteria.CreateTime.EndValue);
            }
            return q.ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取启用的安灯类型
        /// </summary>
        /// <param name="pagingInfo"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public virtual EntityList<AndonType> GetEnableAndonType(PagingInfo pagingInfo, string keyword)
        {
            return Query<AndonType>()
                .WhereIf(!keyword.IsNullOrEmpty(), p => p.AndonTypeName.Contains(keyword) || p.AndonTypeCode.Contains(keyword))
                .Where(p => p.State == State.Enable).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 安灯类型拉取数据
        /// </summary>
        /// <param name="andonTypeId"></param>
        /// <returns></returns>
        public virtual AndonTypeRequestInfo GetAndonTypeInfo(double andonTypeId)
        {
            var info = new AndonTypeRequestInfo();
            var andonType = Query<AndonType>().Where(p => p.Id == andonTypeId).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
            if (andonType == null)
            {
                throw new ValidationException("安灯类型数据异常!".L10N());
            }
            var andonTypeMessageSendIds = new List<double>();
            var andonMessageSentList = RT.Service.Resolve<AndonController>().GetAndonTypeMessageSends(andonTypeId, andonTypeMessageSendIds);
            var andonTypePushObjectList = andonTypeMessageSendIds.SplitContains(tempIds =>
            {
                return Query<AndonTypePushObject>().Where(p => tempIds.Contains(p.MessageSendId)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
            var index = 0;
            andonMessageSentList.ForEach(item =>
            {
                var andonTypeMessId = andonTypeMessageSendIds[index++];
                var andonTypePushObjects = GetAndonTypePushObjects(andonTypePushObjectList, andonTypeMessId);
                item.PushObjectList.AddRange(andonTypePushObjects);
            });
            info.AndonTypeClass = andonType.AndonTypeClass;
            //info.PushPlugId = andonType.PushPlugId;
            //info.PushPlugId_Display = andonType.PushPlugId != null?andonType.PushPlug.Name:string.Empty;
            info.AndonMessageList.AddRange(andonMessageSentList);
            return info;
        }



        /// <summary>
        /// 获取可疑品配置安灯维护列表
        /// </summary>
        /// <returns></returns>
        public virtual EntityList<Andon> GetAndonBySuspect()
        {
            var q = Query<Andon>().Where(p=>p.SuspectAndon==true);
            return q.ToList(null , new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取所有安灯维护
        /// </summary>
        /// <returns></returns>
        public virtual EntityList<Andon> GetAndons()
        {
            var query = Query<Andon>().ToList();
            return query;
        }
    }
}

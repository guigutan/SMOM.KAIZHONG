using SIE.Common;
using SIE.Common.Organizations;
using SIE.Core.ApiModels;
using SIE.Core.Common.Service;
using SIE.Domain;
using SIE.MES.QTimes.Daos;
using SIE.MES.QTimes.Enums;
using SIE.MES.QTimes.Handles;
using SIE.MES.QTimes.ViewModels;
using SIE.Rbac.Roles;
using SIE.Rbac.Users;
using SIE.Resources.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.QTimes.Services
{
    /// <summary>
    /// QT标准维护Service层
    /// </summary>
    public class QTimeStandardService : DomainService
    {
        /// <summary>
        /// Dao
        /// </summary>
        private readonly QTimeStandardDao _qTimeStandardDao;  

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="qTimeStandardDao"></param>
        public QTimeStandardService(QTimeStandardDao qTimeStandardDao)
        {
            _qTimeStandardDao = qTimeStandardDao;
        }

        /// <summary>
        /// QT标准维护查询
        /// </summary>
        /// <param name="qTimeStandardCriteria"></param>
        /// <returns></returns>
        public virtual EntityList<QTimeStandard> QueryQTimeEntityList(QTimeStandardCriteria qTimeStandardCriteria)
        {
            return _qTimeStandardDao.QueryQTimeEntityList(qTimeStandardCriteria);
        }

        /// <summary>
        /// 新增保存时获取同产品产线数据
        /// </summary>
        /// <param name="ids">数据Ids</param>
        /// <param name="productIds">产品Ids</param>
        /// <param name="wipIds">产线Ids</param>
        /// <returns></returns>
        public virtual EntityList<QTimeStandard> GetQTByProductAndWip(List<double> ids,List<double?> productIds, List<double?> wipIds)
        {
            return _qTimeStandardDao.GetQTByProductAndWip(ids, productIds, wipIds);
        }

        /// <summary>
        /// 按优先级获取QT标准规则
        /// </summary>
        /// <returns></returns>
        public virtual EntityList<QTimeStandard> GetQTByPriority()
        {
            return _qTimeStandardDao.GetQTByPriority();
        }

        /// <summary>
        /// QT标准维护保存命令
        /// </summary>
        /// <param name="data"></param>
        public virtual void QTimeStandardSaveCommand(EntityList data)
        {
            QTimeStandardSaveHandle qTimeStandardSaveHandle = new QTimeStandardSaveHandle(data);
            // 校验必填
            qTimeStandardSaveHandle.Required();
            // 校验开始结束同工序时不能同状态
            qTimeStandardSaveHandle.SameProcessSameState();
            // 校验开始工序和结束工序必须同为批次或单体
            qTimeStandardSaveHandle.StartTypeEqualEndType();
            // 校验前端产品+产线+开始工序+开始状态+结束工序+结束状态唯一
            qTimeStandardSaveHandle.WebRepeat();
            // 校验数据库产品+产线+开始工序+开始状态+结束工序+结束状态唯一
            qTimeStandardSaveHandle.DBRepeat();

            // 校验子表必填
            qTimeStandardSaveHandle.ChildRequired();
            // 校验子表同一推送类型推送对象不能重复
            qTimeStandardSaveHandle.ChildRepeat();
        }

        /// <summary>
        /// 根据推送类型获取数据
        /// </summary>
        /// <param name="qTPushType"></param>
        /// <param name="keyword"></param>
        /// <param name="pagingInfo"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public virtual EntityList<QTPushObjectViewModel> GetPushTypeDatas(QTPushType qTPushType, string keyword, PagingInfo pagingInfo)
        {
            EntityList<QTPushObjectViewModel> datas = new EntityList<QTPushObjectViewModel>();
            switch (qTPushType)
            {
                case QTPushType.Employee: // 员工
                    datas = GetEmployee(keyword, pagingInfo);
                    break;
                case QTPushType.UserGroup: //用户组
                    datas = GetUserGroup(keyword, pagingInfo);
                    break;
                case QTPushType.WorkGroup: // 班组
                    datas = GetWorkGroup(keyword, pagingInfo);
                    break;
                case QTPushType.Role: // 角色
                    datas = GetRole(keyword, pagingInfo);
                    break;
                case QTPushType.Department: // 部门
                    datas = GetDepartment(keyword, pagingInfo);
                    break;
                default:
                    break;
            }
            return datas;
        }

        private EntityList<QTPushObjectViewModel> GetDepartment(string keyword, PagingInfo pagingInfo)
        {
            var organizations = DB.Query<Organization>().
                WhereIf(!keyword.IsNullOrEmpty(), p => p.Code.Contains(keyword) || p.Name.Contains(keyword))
                .Where(p => p.Level.Type == OrganizationType.Department && p.InvOrgId == RT.InvOrg)
                .ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty())
                .Select(p => new QTPushObjectViewModel
                {
                    ObjectId = p.Id,
                    ObjectCode = p.Code,
                    ObjectName = p.Name,
                }).ToList().AsEntityList();
            organizations.SetTotalCount(pagingInfo.TotalCount);
            return organizations;
        }

        private EntityList<QTPushObjectViewModel> GetRole(string keyword, PagingInfo pagingInfo)
        {
            var roles = DB.Query<Role>().
                WhereIf(!keyword.IsNullOrEmpty(), p => p.Code.Contains(keyword) || p.Name.Contains(keyword))
                .ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty())
                .Select(p => new QTPushObjectViewModel
                {
                    ObjectId = p.Id,
                    ObjectCode = p.Code,
                    ObjectName = p.Name,
                }).ToList().AsEntityList();
            roles.SetTotalCount(pagingInfo.TotalCount);
            return roles;
        }

        private EntityList<QTPushObjectViewModel> GetWorkGroup(string keyword, PagingInfo pagingInfo)
        {
            var workGroups = DB.Query<WorkGroup>().
                WhereIf(!keyword.IsNullOrEmpty(), p => p.Code.Contains(keyword) || p.Name.Contains(keyword))
                .ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty())
                .Select(p => new QTPushObjectViewModel
                {
                    ObjectId = p.Id,
                    ObjectCode = p.Code,
                    ObjectName = p.Name,
                }).ToList().AsEntityList();
            workGroups.SetTotalCount(pagingInfo.TotalCount);
            return workGroups;
        }

        private EntityList<QTPushObjectViewModel> GetUserGroup(string keyword, PagingInfo pagingInfo)
        {
            var userGroups = DB.Query<UserGroup>().
                WhereIf(!keyword.IsNullOrEmpty(), p => p.Code.Contains(keyword) || p.Name.Contains(keyword))
                .ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty())
                .Select(p => new QTPushObjectViewModel
                {
                    ObjectId = p.Id,
                    ObjectCode = p.Code,
                    ObjectName = p.Name,
                }).ToList().AsEntityList();
            userGroups.SetTotalCount(pagingInfo.TotalCount);
            return userGroups;
        }

        private EntityList<QTPushObjectViewModel> GetEmployee(string keyword, PagingInfo pagingInfo)
        {
            var employees = DB.Query<Employee>()
                .WhereIf(!keyword.IsNullOrEmpty(), p => p.Code.Contains(keyword) || p.Name.Contains(keyword))
                .ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty())
                .Select(p => new QTPushObjectViewModel
                {
                    ObjectId = p.Id,
                    ObjectCode = p.Code,
                    ObjectName = p.Name,
                }).ToList().AsEntityList();
            employees.SetTotalCount(pagingInfo.TotalCount);
            return employees;
        }
    }
}

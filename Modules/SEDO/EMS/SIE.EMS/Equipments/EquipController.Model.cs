using SIE.Common.Sort;
using SIE.Core.ApiModels;
using SIE.Data;
using SIE.Domain;
using SIE.EMS.DevicePurs;
using SIE.EMS.Equipments.Models;
using SIE.EMS.Equipments.Models.ViewModels;
using SIE.EMS.Equipments.Units;
using SIE.EMS.MainenanceProjects;
using SIE.Equipments.EquipModels;
using SIE.Equipments.EquipTypes;
using SIE.Rbac.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace SIE.EMS.Equipments
{
    /// <summary>
    /// 设备机型控制器
    /// </summary>
    public partial class EquipController : DomainController
    {
        /// <summary>
        /// 获取设备单元最底层数据
        /// </summary>
        /// <param name="keyword">搜索文本</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <returns>设备单元列表</returns>
        public virtual EntityList<EquipUnit> GetSmallEquipUnit(string keyword, PagingInfo pagingInfo)
        {
            var meta = RF.Find<EquipUnit>().EntityMeta;
            var query = DB.Query<EquipUnit>("i1");
            query.Where(f => f.SQL<bool>(new FormattedSql(@"not exists(select 1 from {0} i2 where i1.{1} = i2.{2} and i2.{3} = '{4}')".FormatArgs(meta.TableMeta.TableName, meta.Property(EquipUnit.IdProperty).ColumnMeta.ColumnName, meta.Property(EquipUnit.TreePIdProperty).ColumnMeta.ColumnName, meta.Property(PhantomEntityExtension.IS_PHANTOMProperty).ColumnMeta.ColumnName, 0))));
            if (!keyword.IsNullOrEmpty())
            {
                query.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword));
            }
            return query.ToList(pagingInfo);
        }

        /// <summary>
        /// 通过设备型号Id列表获取点检项目列表
        /// </summary>
        /// <param name="modelIds">设备型号Id列表</param>
        /// <returns>点检项目列表</returns>
        public virtual EntityList<EquipModelCheckProject> GetCheckProjectsOfModels(List<double> modelIds)
        {
            var elo = new EagerLoadOptions();
            elo.LoadWithViewProperty();

            return Query<EquipModelCheckProject>().Where(p => modelIds.Contains(p.EquipModelId)).ToList(null, elo);
        }

        /// <summary>
        /// 通过设备型号Id获取点检项目列表
        /// </summary>
        /// <param name="modelId">设备型号ID</param>
        /// <param name="pagingInfo"></param>
        /// <param name="orderInfos"></param>
        /// <returns>点检项目列表</returns>
        public virtual EntityList<EquipModelCheckProject> GetCheckProjectsOfModels(double modelId,
            PagingInfo pagingInfo, IList<OrderInfo> orderInfos)
        {
            return Query<EquipModelCheckProject>()
                .Where(p => p.EquipModelId == modelId)
                .OrderBy(orderInfos)
                .ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 通过设备型号Id列表获取技术参数列表
        /// </summary>
        /// <param name="modelIds">设备型号Id列表</param>
        /// <returns>技术参数列表</returns>
        public virtual EntityList<EquipModelTechParameter> GetEquipModelTechParameters(List<double> modelIds)
        {
            return Query<EquipModelTechParameter>().Where(p => modelIds.Contains(p.EquipModelId)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据设备型号集合获取润滑项目列表
        /// </summary>
        /// <param name="modelIds">设备型号Id列表</param>
        /// <returns>润滑项目列表</returns>
        public virtual EntityList<EquipModelLubricationProject> GetEquipModelLubricationProjects(List<double> modelIds)
        {
            return Query<EquipModelLubricationProject>().Where(p => modelIds.Contains(p.EquipModelId)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 通过设备型号Id列表获取保养项目列表
        /// </summary>
        /// <param name="modelIds">设备型号Id列表</param>
        /// <returns>保养项目列表</returns>
        public virtual EntityList<EquipModelMaintainProject> GetMaintainProjectsOfModels(List<double> modelIds)
        {
            var elo = new EagerLoadOptions();
            elo.LoadWith(EquipModelMaintainProject.DepartmentProperty);
            elo.LoadWithViewProperty();

            return Query<EquipModelMaintainProject>().Where(p => modelIds.Contains(p.EquipModelId)).ToList(null, elo);
        }

        /// <summary>
        /// 通过设备型号Id获取保养项目列表
        /// </summary>
        /// <param name="modelId">设备型号ID</param>
        /// <param name="pagingInfo"></param>
        /// <param name="orderInfos"></param>
        /// <returns>保养项目列表</returns>
        public virtual EntityList<EquipModelMaintainProject> GetMaintainProjectsOfModels(double modelId,
            PagingInfo pagingInfo, IList<OrderInfo> orderInfos)
        {
            return Query<EquipModelMaintainProject>()
                .Where(p => p.EquipModelId == modelId)
                .OrderBy(orderInfos)
                .ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取维修项目列表
        /// </summary>
        /// <param name="modelId"></param>
        /// <param name="pagingInfo"></param>
        /// <param name="orderInfos"></param>
        /// <returns></returns>

        public virtual EntityList<EquipModelRepairProject> GetEquipModelRepairProjectList(double modelId,
            PagingInfo pagingInfo, IList<OrderInfo> orderInfos)
        {
            return Query<EquipModelRepairProject>()
                   .Where(p => p.EquipModelId == modelId)
                   .OrderBy(orderInfos)
                   .ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据设备信号获取维修项目中的保养维护项目
        /// </summary>
        /// <param name="modelId"></param>
        /// <param name="keyword"></param>
        /// <param name="pagingInfo"></param>
        /// <returns></returns>
        public virtual EntityList<ProjectDetail> GetEquipModelRepairProjectList(double modelId, PagingInfo pagingInfo, string keyword)
        {
            var projectDetailIds = Query<EquipModelRepairProject>().Where(p => p.EquipModelId == modelId).ToList().Select(p => p.ProjectDetailId).ToList();

            if (!projectDetailIds.Any())
            {
                return new EntityList<ProjectDetail>();
            }
            var query = Query<ProjectDetail>().Where(p => projectDetailIds.Contains(p.Id));

            if (!keyword.IsNullOrEmpty())
            {
                query.Where(p => p.Name.Contains(keyword));
            }
            var list = query.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
            return list;
        }

        /// <summary>
        /// 获取设备型号维修项目
        /// </summary>
        /// <param name="Ids"></param>
        /// <returns></returns>
        public virtual EntityList<EquipModelRepairProject> GetEquipModelRepairProjectsByIds(List<double> Ids)
        {
            return Ids.SplitContains(tempIds =>
            {
                return Query<EquipModelRepairProject>().Where(m => tempIds.Contains(m.Id)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());

            });
        }





        /// <summary>
        /// 通过设备型号Id列表获取校验项目列表
        /// </summary>
        /// <param name="modelIds">设备型号Id列表</param>
        /// <returns>校验项目列表</returns>
        public virtual EntityList<EquipModelVerifyProject> GetVerifyProjectsOfModels(List<double> modelIds)
        {
            return Query<EquipModelVerifyProject>().Where(p => modelIds.Contains(p.EquipModelId)).ToList();
        }

        /// <summary>
        /// 通过设备型号ID获取校验项目列表
        /// </summary>
        /// <param name="modelId">设备型号ID</param>
        /// <returns>校验项目列表</returns>
        public virtual EntityList<EquipModelVerifyProject> GetEquipModelVerifyProjects(double modelId)
        {
            return Query<EquipModelVerifyProject>().Where(p => p.EquipModelId == modelId).ToList();
        }

        /// <summary>
        /// 通过设备型号ID获取校验项目列表
        /// </summary>
        /// <param name="modelId">设备型号ID</param>
        /// <param name="pagingInfo"></param>
        /// <param name="orderInfos"></param>
        /// <returns>校验项目列表</returns>
        public virtual EntityList<EquipModelVerifyProject> GetEquipModelVerifyProjects(double modelId,
            PagingInfo pagingInfo, IList<OrderInfo> orderInfos)
        {
            return Query<EquipModelVerifyProject>()
                .Where(p => p.EquipModelId == modelId)
                .OrderBy(orderInfos)
                .ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 通过设备型号ID获取润滑项目列表
        /// </summary>
        /// <param name="modelId"></param>
        /// <param name="pagingInfo"></param>
        /// <param name="orderInfos"></param>
        /// <returns></returns>
        public virtual EntityList<EquipModelLubricationProject> GetLubricationProjectList(double modelId,
            PagingInfo pagingInfo, IList<OrderInfo> orderInfos)
        {
            return Query<EquipModelLubricationProject>()
                .Where(p => p.EquipModelId == modelId)
                .OrderBy(orderInfos)
                .ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取设备型号的技术参数列表
        /// </summary>
        /// <param name="modelId"></param>
        /// <param name="pagingInfo"></param>
        /// <param name="orderInfos"></param>
        /// <returns></returns>
        public virtual EntityList<EquipModelTechParameter> GetEquipModelTechParameterList(double modelId,
            PagingInfo pagingInfo, IList<OrderInfo> orderInfos)
        {
            var result = Query<EquipModelTechParameter>()
                .Where(p => p.EquipModelId == modelId)
                .ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());

            var res = new EntityList<EquipModelTechParameter>();
            res.SetTotalCount(result.Count);
            res.AddRange(result.OrderBy(f => SortExtension.GetIndex(f)));
            return res;
        }

        /// <summary>
        /// 通过设备类型列表获取型号列表
        /// </summary>
        /// <param name="type">设备类型</param>
        /// <param name="keyword">搜索文本</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <returns>型号列表</returns>
        public virtual EntityList<EquipModel> GetEquipModelsOfType(string type, string keyword, PagingInfo pagingInfo)
        {
            var query = Query<EquipModel>().Where(x => x.TypeCategory == type);
            if (!keyword.IsNullOrEmpty())
            {
                query.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword));
            }
            return query.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 通用设备型号实体查询
        /// </summary>
        /// <param name="expr">查询表达式</param>
        /// <returns></returns>
        public virtual EquipModel GetEquipModel(Expression<Func<EquipModel, bool>> expr)
        {
            return Query<EquipModel>().Where(expr).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 保存设备型号
        /// </summary>
        /// <param name="equipModel">设备型号实体</param>
        public virtual void SaveEquipModel(EquipModel equipModel)
        {
            RF.Save(equipModel);
        }


        /// <summary>
        /// 查询设备型号信息
        /// </summary>
        /// <param name="pagingInfo"></param>
        /// <param name="equipTypeId"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public virtual EntityList<EquipModel> GetEquipModels(PagingInfo pagingInfo, double? equipTypeId, string keyword,string typeCategory="")
        {
            var q = Query<EquipModel>();
            if (equipTypeId.HasValue)
            {
                q.Where(p => p.EquipTypeId == equipTypeId);
            }
            if (!typeCategory.IsNullOrEmpty())
            {
                q.Where(p => p.TypeCategory == typeCategory);
            }
            if (keyword.IsNotEmpty())
            {
                q.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword));
            }
            return q.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }


        /// <summary>
        /// 查询设备型号信息
        /// </summary>
        /// <param name="pagingInfo"></param>        
        /// <param name="keyword"></param>
        /// <returns></returns>
        public virtual EntityList<EquipModel> GetEquipModelsOfUserHasPermission(PagingInfo pagingInfo, string keyword)
        {
            EagerLoadOptions elo = new EagerLoadOptions();
            elo.LoadWithViewProperty();
            var q = Query<EquipModel>();

            if (keyword.IsNotEmpty())
            {
                q.Where(em => em.Code.Contains(keyword) || em.Name.Contains(keyword));
            }
            return q.ToList(pagingInfo, elo);
        }

        /// <summary>
        /// 查询用户有权限的设备类别信息
        /// </summary>
        /// <param name="pagingInfo"></param>        
        /// <param name="keyword"></param>
        /// <returns></returns>
        public virtual EntityList<EquipType> GetEquipTypesOfUserHasPermission(PagingInfo pagingInfo, string keyword)
        {
            var q = Query<EquipType>()
                .Exists<DeviceType>((x, y) => y.Where(z => (z.EquipTypeId == null)
                   || (z.EquipTypeId == x.Id )))
                 .Join<DeviceType, DevicePur>((a, b) => a.DevicePurId == b.Id)
                 .LeftJoin<DevicePur, UserInUserGroup>((a, b) => a.UserGroupId == b.UserGroupId)
                 .Where<DevicePur, UserInUserGroup>((a, b, c) => (b.UserId == RT.Identity.UserId || c.UserId == RT.Identity.UserId));


            if (keyword.IsNotEmpty())
            {
                q.Where(x => x.TypeCode.Contains(keyword) || x.TypeName.Contains(keyword));
            }

            return q.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据设备台账的设备型号获取其相关信息(包括位置列表)
        /// </summary>
        /// <param name="lubricationProject">润滑项目</param>
        /// <returns>设备台账信息</returns>
        public virtual EquipModelLubricationProjectInfo GetModelSparePartItemInfos(EquipModelLubricationProject lubricationProject)
        {
            EquipModelLubricationProjectInfo LubricationProjectInfo = new EquipModelLubricationProjectInfo();
            var equipCt = RT.Service.Resolve<ProjectDetailController>();
            DateTime now = RF.Find<EquipModel>().GetDbTime();
            List<double> projectDetailIds = new List<double>() { lubricationProject.ProjectDetailId };
            #region 加载初始数据
            //获取设备型号对应的点检保养项目列表

            var sparePartItemModels = equipCt.GetSparePartItem(projectDetailIds);

            #endregion

            LubricationProjectInfo.EquipModelLubricaSparePartList.AddRange(GetModelSparePartItemList(lubricationProject.Id, now, sparePartItemModels));

            return LubricationProjectInfo;
        }

        /// <summary>
        /// 获取设备台账润滑记录备件清单
        /// </summary>
        /// <param name="lubricationProjectId"></param>
        /// <param name="now"></param>
        /// <param name="sparePartItemList"></param>
        /// <returns></returns>
        private List<EquipModelLubricaSparePart> GetModelSparePartItemList(double lubricationProjectId, DateTime now,
           EntityList<SparePartItem> sparePartItemList)
        {
            List<EquipModelLubricaSparePart> lubricationProjects = new List<EquipModelLubricaSparePart>();
            sparePartItemList.ForEach(m =>
            {
                var lubricationProject = CreateEquipModelLubricaSparePart(lubricationProjectId, m, now);
                lubricationProjects.Add(lubricationProject);
            });

            return lubricationProjects;
        }


        /// <summary>
        /// 创建设备台账润滑项目的备件清单
        /// </summary>
        /// <param name="lubricationProjectId"></param>
        /// <param name="m"></param>
        /// <param name="now"></param>
        /// <returns></returns>
        private EquipModelLubricaSparePart CreateEquipModelLubricaSparePart(double lubricationProjectId,
            SparePartItem m, DateTime now)
        {
            return new EquipModelLubricaSparePart()
            {
                LubricationProjectId = lubricationProjectId,
                SparePartId = m.SparePartId,
                SparePartCode = m.SparePart.SparePartCode,
                SparePartName = m.SparePart.SparePartName,
                Qty = m.Qty,
                UpdateDate = now,
                CreateDate = now,
                PersistenceStatus = PersistenceStatus.New,
            };
        }

        /// <summary>
        /// 获取设备型号的基础数据
        /// </summary>
        /// <param name="codes"></param>
        /// <returns></returns>
        public virtual List<BaseDataInfo> GetEquipModelBaseInfos(List<string> codes)
        {
            List<BaseDataInfo> baseDataInfos = new List<BaseDataInfo>();
            codes.SplitDataExecute(temps =>
            {
                var list = Query<EquipModel>().Where(p => temps.Contains(p.Code)).Select(p => new
                {
                    Id = p.Id,
                    Code = p.Code,
                    Name = p.Name,
                }).ToList<BaseDataInfo>().ToList();
                baseDataInfos.AddRange(list);
            });
            return baseDataInfos;
        }

        /// <summary>
        /// 获取设备型号下的点检项目
        /// </summary>
        /// <param name="equipModelIds">设备型号Ids</param>
        /// <returns></returns>
        public virtual EntityList<EquipModelCheckProject> GetEquipModelCheckProjects(List<double> equipModelIds)
        {
            return equipModelIds.SplitContains(temps =>
            {
                return Query<EquipModelCheckProject>().Where(p => temps.Contains(p.EquipModelId)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
        }

        /// <summary>
        /// 获取设备型号下的保养项目
        /// </summary>
        /// <param name="equipModelIds">设备型号Ids</param>
        /// <returns></returns>
        public virtual EntityList<EquipModelMaintainProject> GetEquipModelMaintainProjects(List<double> equipModelIds)
        {
            return equipModelIds.SplitContains(temps =>
            {
                return Query<EquipModelMaintainProject>().Where(p => temps.Contains(p.EquipModelId)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
        }
    }
}
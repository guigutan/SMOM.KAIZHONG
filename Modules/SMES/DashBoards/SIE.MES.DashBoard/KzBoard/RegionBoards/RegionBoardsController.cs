using SIE.Domain;
using SIE.Resources.UserGroups;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.DashBoard.KzBoard.RegionBoards
{
    public class RegionBoardsController : DomainController
    {

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="regionBoardDetails"></param>
        public virtual void SaveRegionBoardDetail(List<RegionBoardDetail> regionBoardDetails)
        {
            EntityList<RegionBoardDetail> savedData = new EntityList<RegionBoardDetail>();
            var details = GetRegionBoardDetailsByRegionId(regionBoardDetails.FirstOrDefault().RegionBoardId);
            int sort = 0;
            if (details.Count > 0)
            {
                sort = details.OrderByDescending(p => p.Sort).FirstOrDefault().Sort + 1;
            }
            foreach (var item in regionBoardDetails)
            {
                var entity = new RegionBoardDetail();
                entity.WipResourceId = item.WipResourceId;
                entity.RegionBoardId = item.RegionBoardId;
                entity.Sort = sort;
                sort++;
                savedData.Add(entity);
            }
            if (savedData.Count > 0)
                RF.Save(savedData);

        }

        /// <summary>
        /// 获取产线明细
        /// </summary>
        /// <param name="regionId"></param>
        /// <returns></returns>
        public virtual EntityList<RegionBoardDetail> GetRegionBoardDetailsByRegionId(double regionId)
        {
            var list = Query<RegionBoardDetail>().Where(p => p.RegionBoardId == regionId).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            return list;
        }

        /// <summary>
        /// 获取产线明细(看板区域)
        /// </summary>
        /// <param name="Region"></param>
        /// <returns></returns>
        public virtual EntityList<RegionBoardDetail> GetRegionBoardDetailsByRegion(string Region, RegionBoardType regionBoardType)
        {
            var entityList = Query<RegionBoardDetail>()
                 .Exists<RegionBoard>((rd, r) => r.Where(p => p.Id == rd.RegionBoardId && p.Region == Region&&p.RegionBoardType == regionBoardType))
                 .ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            return entityList;
        }


        /// <summary>
        /// 获取看板区域MRB控制者
        /// </summary>
        /// <param name="Region"></param>
        /// <returns></returns>
        public virtual EntityList<RegionBoardMRB> GetRegionBoardMRBByRegion(string Region, RegionBoardType regionBoardType)
        {
            var entityList = Query<RegionBoardMRB>()
                 .Exists<RegionBoard>((rd, r) => r.Where(p => p.Id == rd.RegionBoardId && p.Region == Region && p.RegionBoardType == regionBoardType))
                 .ToList();
            return entityList;
        }

        /// <summary>
        /// 获取看板区域
        /// </summary>
        /// <param name="region"></param>
        /// <param name="regionBoardType"></param>
        /// <returns></returns>
        public virtual EntityList<RegionBoard> GetRegionBoard(string region, RegionBoardType regionBoardType)
        {
            var entity = Query<RegionBoard>().Where(p => p.RegionBoardType == regionBoardType).WhereIf(!region.IsNullOrEmpty(), p => p.Region == region).ToList();
            return entity;
        }
    }
}

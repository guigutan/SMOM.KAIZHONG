using SIE.Api;
using SIE.Domain;
using SIE.MES.WIP.PackRecombine;
using SIE.Packages;
using SIE.Packages.ItemLabels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.MES.ForWinform
{
    /// <summary>
    /// 包装管理API控制器
    /// </summary>
    public class WinFormPackManageApiController : DomainController
    {
        /// <summary>
        /// 扫描查询包装关系信息
        /// </summary>
        /// <param name="packNo">包装或条码号</param>
        /// <param name="isBatch">批次</param>
        /// <returns>包装关系</returns>
        [ApiService("扫描查询包装关系信息")]
        [return: ApiReturn("包装关系")]
        public virtual RecombineInfo SearchPacking([ApiParameter("包装或条码号")] string packNo, [ApiParameter("批次")] bool isBatch)
        {
            RecombineInfo recombineInfo;
            var relation = RT.Service.Resolve<PackingRelationController>().GetBatchPackingRelation(packNo, false);
            if (relation == null)
                recombineInfo = RT.Service.Resolve<PackRecombineController>().SearchPackingRelationBySn(packNo);
            else
                recombineInfo = RT.Service.Resolve<PackRecombineController>().SearchPackingRelation(packNo, isBatch);
            return recombineInfo;
        }

        /// <summary>
        /// 查询所有包装关系
        /// </summary>
        /// <param name="relationId">包装关系信息id</param>
        /// <returns>包装关系信息</returns>
        [ApiService("查询所有包装关系")]
        [return: ApiReturn("包装关系信息")]
        public virtual Tuple<List<BatchPackingRelation>, List<ItemLabel>> GetAllRelationByParents([ApiParameter("包装关系信息id")] double relationId)
        {
            var relation = RF.GetById<BatchPackingRelation>(relationId, new EagerLoadOptions().LoadWithViewProperty());
            var list = new EntityList<BatchPackingRelation> { relation };
            var relationList = RT.Service.Resolve<PackageController>().GetAllRelationByParents(list);
            var relationIds = relationList.Select(p => p.Id).ToList();
            var labels = RT.Service.Resolve<ItemLabelController>().GetItemLabelByRelationId(relationIds);
            return new Tuple<List<BatchPackingRelation>, List<ItemLabel>>(relationList.ToList(), labels.ToList());
        }

        /// <summary>
        /// 加入包装第一次扫描容器包装条码
        /// </summary>
        /// <param name="packNo">包装号</param>
        /// <param name="isBatch">是否批次包装</param>
        /// <returns>容器包装的包装关系</returns>
        [ApiService("加入包装第一次扫描容器包装条码")]
        [return: ApiReturn("容器包装的包装关系")]
        public virtual BatchPackingRelation JoinPackingRelationScanParent([ApiParameter("包装号")] string packNo, [ApiParameter("是否批次包装")] bool isBatch)
        {
            var relation = RT.Service.Resolve<PackRecombineController>().JoinPackingRelationScanParent(packNo, isBatch);
            return relation;
        }

        /// <summary>
        /// 加入包装第二次扫描加入的条码
        /// </summary>
        /// <param name="packNo">包装号</param>
        /// <param name="isBatch">是否批次包装</param>
        /// <param name="relationId">包装关系id</param>
        /// <returns>包装拆合信息</returns>
        [ApiService("加入包装第二次扫描加入的条码")]
        [return: ApiReturn("包装拆合信息")]
        public virtual RecombineInfo JoinPackingRelationScanSon([ApiParameter("包装号")] string packNo,
            [ApiParameter("是否批次包装")] bool isBatch, [ApiParameter("包装关系信息id")] double relationId)
        {
            var relation = GetById<BatchPackingRelation>(relationId);
            var info = RT.Service.Resolve<PackRecombineController>().JoinPackingRelationScanSon(packNo, relation, isBatch);
            return info;
        }

        /// <summary>
        /// 包装移除
        /// </summary>
        /// <param name="packNo">包装号</param>
        /// <param name="isBatch">是否批次包装</param>
        /// <returns>包装拆合信息</returns>
        [ApiService("包装移除")]
        [return: ApiReturn("包装拆合信息")]
        public virtual Tuple<RecombineInfo, BatchPackingRelation> SplitPackingRelation([ApiParameter("包装号")] string packNo,
            [ApiParameter("是否批次包装")] bool isBatch)
        {
            var info = RT.Service.Resolve<PackRecombineController>().SplitPackingRelation(packNo, isBatch);
            var relation = RT.Service.Resolve<PackingRelationController>().GetBatchPackingRelation(info.OldPackingNo, true);
            return new Tuple<RecombineInfo, BatchPackingRelation>(info, relation);
        }
    }
}

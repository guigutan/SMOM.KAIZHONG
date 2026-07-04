using SIE.Common.Catalogs;
using SIE.Core.Common.Controllers;
using SIE.Domain;
using SIE.Fixtures;
using SIE.Fixtures.Models;
using SIE.Warehouses;
using SIE.Web.Data;
using System;

namespace SIE.Web.Fixtures.Models.DataQueryers
{
    /// <summary>
    /// 工治具编码查询器
    /// </summary>
    public class FixtureEncodeDataQueryer : DataQueryer
    {
        /// <summary>
        /// 获取工治具编码-保养项目
        /// </summary>
        /// <returns></returns>
        public EntityList<FixtureModelMaintainProject> GetFixtureEncodeProjects(double fixtureModelId)
        {
            return RT.Service.Resolve<CoreFixtureController>().GetFixtureModelProjectsByModelId(fixtureModelId);
        }

        /// <summary>
        /// 获取编码
        /// </summary>
        /// <returns></returns>
        public string GetCode()
        {
            return RT.Service.Resolve<CoreFixtureController>().GetCode();
        }

        /// <summary>
        /// 获取工治具编码编号
        /// </summary>
        /// <returns></returns>
        public string GetFixtureEncodeNO()
        {
            var no = "";
            try
            {
                no = RT.Service.Resolve<CommonController>().GetNo<FixtureEncode>("工治具编号");
            }
            catch (Exception)
            {
                //Todo 
            }
            return no;

        }
        /// <summary>
        ///获取仓库分类快码
        /// </summary>
        /// <returns></returns>

        public EntityList<Catalog> GetCalalogForFixtureWarehouseType()
        {
            return RT.Service.Resolve<CatalogController>().GetCatalogList(Warehouse.CatalogCategory);
        }
    }
}

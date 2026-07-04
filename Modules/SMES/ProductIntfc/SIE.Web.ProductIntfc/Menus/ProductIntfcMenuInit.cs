using SIE.Common.Menus;
using System.Collections.Generic;
using SIE.ProductIntfc.FirstInsps;
using SIE.ProductIntfc.InspSettings;
using SIE.ProductIntfc.ProductInsps;
using SIE.ProductIntfc.ProductStorages;
using SIE.ProductIntfc.OutputProducts;

namespace SIE.Web.ProductIntfc
{
    /// <summary>
    /// 菜单初始化
    /// </summary>
    public class ProductIntfcMenuInit : IWebMenuInit
    {
        /// <summary>
        /// 获取菜单列表
        /// </summary>
        /// <returns></returns>
        public List<MenuDto> GetMenuDtos()
        {
            var res = new List<MenuDto>();

            res.Add(new MenuDto()
            {
                TreeKey = "MES",
                Label = "生产质量管理",
                IsLeafNode = false,
            });
            res.Add(new MenuDto()
            {
                TreeKey = "MES.生产质量管理",
                Label = "报检参数",
                EntityType = typeof(InspParameter)
            });
            res.Add(new MenuDto()
            {
                TreeKey = "MES.生产质量管理",
                Label = "首件报检",
                EntityType = typeof(FirstInsp)
            });
            res.Add(new MenuDto()
            {
                TreeKey = "MES.生产质量管理",
                Label = "成品报检",
                EntityType = typeof(ProductInsp)
            });

            res.Add(new MenuDto()
            {
                TreeKey = "MES.生产管理",
                Label = "成品入库参数",
                EntityType = typeof(ProductStorageParam)
            });
            res.Add(new MenuDto()
            {
                TreeKey = "MES.生产管理",
                Label = "成品入库",
                EntityType = typeof(StorageWorkOrder)
            });
            res.Add(new MenuDto()
            {
                TreeKey = "MES.生产管理",
                Label = "联/副产品入库",
                EntityType = typeof(OutputProduct)
            });

            return res;
        }

    }
}

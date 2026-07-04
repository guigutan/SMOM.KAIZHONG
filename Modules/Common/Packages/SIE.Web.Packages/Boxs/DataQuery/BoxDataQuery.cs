using SIE.Common.Configs;
using SIE.Items;
using SIE.Packages.Boxs;
using SIE.Packages.Boxs.Configs;
using SIE.Web.Data;
using SIE.Web.Packages.Boxs.ViewModels;
using System.Collections.Generic;

namespace SIE.Web.Packages.Boxs.DataQuery
{
    /// <summary>
    /// 周转箱查找
    /// </summary>
    public class BoxDataQuery : DataQueryer
    {
        /// <summary>
        /// 查找物料
        /// </summary>
        /// <param name="box">周转箱</param>
        /// <returns>物料列表</returns>
        public List<ItemViewModel> GetItems(TurnoverBox box)
        {

            var itemList = RT.Service.Resolve<ItemController>().GetItems();
            var list = new List<ItemViewModel>();
            foreach (var item in itemList)
            {
                ItemViewModel ivm = new ItemViewModel();
                ivm.Id = item.Id.ToString();
                ivm.Code = item.Code;
                ivm.Name = item.Name;
                ivm.Type = item.Type;
                ivm.State = item.State;
                ivm.Description = item.Description;
                list.Add(ivm);
            }

            return list;
        }

        /// <summary>
        /// 获取生产周转箱类型
        /// </summary>
        /// <returns>生产周转箱类型</returns>
        public string GetProductTrunoverBoxType()
        {
            var config = ConfigService.GetConfig(new ProductTrunoverBoxTypeConfig());
            if (config == null)
                return "生产周转箱";
            return config.BoxType;
        }
    }
}

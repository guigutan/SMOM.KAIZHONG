using SIE.Domain;
using SIE.Items;
using System.Linq;

namespace SIE.xUnit.Items
{
    /// <summary>
    /// 测试物料数据控制器
    /// </summary>
    public partial class ItemTestController
    {
        /// <summary>
        /// 创建物料
        /// </summary>
        /// <param name="count">单位数据数量</param>
        /// <returns>物料集合</returns>
        public virtual Item CreateItem()
        {
            var item = new Item();
            item.GenerateId();
            double id = item.Id;
            item.Code = $"ItemCode{id}";
            item.Name = $"ItemName{id}";
            item.Unit = CreateUnit(1).FirstOrDefault();

            RF.Save(item);
            return item;
        }
    }
}
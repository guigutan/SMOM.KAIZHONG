using System.Collections.Generic;

namespace SIE.EventMessages
{
    [Services.Service(FallbackType = typeof(ItemSubLibraryWH))]
    public interface IItemSubLibraryWH
    {
        /// <summary>
        /// 按物料子库取第一个仓库
        /// </summary>
        /// <param name="CustomerId"></param>
        double GetFirstSubLibraryWh(List<double> ItemIds);
    }

    public class ItemSubLibraryWH : IItemSubLibraryWH
    {
        public double GetFirstSubLibraryWh(List<double> ItemIds)
        {
            return -1;
        }
    }

}

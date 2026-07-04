using SIE.EventMessages.QMS.IQC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.EventMessages.Resources.WipResources
{
    /// <summary>
    /// 
    /// </summary>
    [Services.Service(FallbackType = typeof(DefaultIWipResources))]
    public interface IWipResources
    {
        /// <summary>
        /// 根据Id获取资源名称
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        string WipResourcesName(double Id);
    }

    /// <summary>
    /// 
    /// </summary>
    public class DefaultIWipResources : IWipResources
    {
        /// <summary>
        /// 根据Id获取资源名称
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public string WipResourcesName(double Id)
        {
            throw new NotImplementedException();
        }
    }
}

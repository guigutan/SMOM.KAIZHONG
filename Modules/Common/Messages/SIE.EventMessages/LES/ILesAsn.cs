using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.EventMessages.LES
{
    /// <summary>
    /// 退料创建ASN
    /// </summary>
    [Services.Service(FallbackType = typeof(DefaultILesAsn))]
    public interface ILesAsn
    {
        /// <summary>
        /// 退料创建ASN
        /// </summary>
        /// <param name="createAsnDatas"></param>
        void CreateAsn(List<CreateAsnData> createAsnDatas);

    }

    class DefaultILesAsn : ILesAsn
    {
        /// <summary>
        /// 退料创建ASN
        /// </summary>
        /// <param name="createAsnDatas"></param>
        public void CreateAsn(List<CreateAsnData> createAsnDatas)
        {
        }

    }
}

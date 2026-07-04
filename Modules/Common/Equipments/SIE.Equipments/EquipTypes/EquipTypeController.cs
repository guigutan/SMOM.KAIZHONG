using SIE.Core.ApiModels;
using SIE.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Equipments.EquipTypes
{
    /// <summary>
    /// 设备类型
    /// </summary>
    public class EquipTypeController : DomainController
    {
        /// <summary>
        /// 获取数据库中的编码
        /// </summary>
        /// <returns></returns>
        public virtual List<string> GetDBEquipTypeCodes()
        {
            return Query<EquipType>().Select(p => new { p.TypeCode }).ToList<string>().ToList();
        }

        /// <summary>
        /// 获取数据库中的编码
        /// </summary>
        /// <param name="codes">设备类型编码</param>
        /// <returns></returns>
        public virtual List<BaseDataInfo> GetDBEquipTypeBaseInfos(List<string> codes)
        {
            List<BaseDataInfo> baseDataInfos = new List<BaseDataInfo>();
            codes.SplitDataExecute(temp =>
            {
                var list = Query<EquipType>().Where(p => temp.Contains(p.TypeCode)).Select(p => new
                {
                    Id = p.Id,
                    Code = p.TypeCode,
                    Name = p.TypeName,
                }).ToList<BaseDataInfo>().ToList();
                baseDataInfos.AddRange(list);
            });
            return baseDataInfos;
        }

        /// <summary>
        /// 下拉获取设备类型
        /// </summary>
        /// <param name="pagingInfo"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public virtual EntityList<EquipType> GetEquipTypes(PagingInfo pagingInfo, string keyword)
        {
            return Query<EquipType>().WhereIf(keyword.IsNotEmpty(), p => p.TypeCode.Contains(keyword) || p.TypeName.Contains(keyword)).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }
    }
}

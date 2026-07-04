using SIE.Common.Algorithm;
using SIE.Domain.Validation;
using System;

namespace SIE.Core.Algorithms.Manufacturers
{
    /// <summary>
    /// 制造商编码段
    /// </summary>
    [Algorithm("制造商编码段算法", typeof(CodeAlgorithmConfig), AlgorithmType.Entity)]
    [RootEntity, Serializable]
    public class ManufacturerCodeSegmentAlgorithm : EntityCodeAlgorithm
    {
        /// <summary>
        /// 获取编码
        /// </summary>
        /// <returns></returns>
        public override string GetCode()
        {
            var data = Context.Data;
            if (data == null)
                return "";
            if (data is IManufacturerCodeSegment)
            {
                IManufacturerCodeSegment iface = (IManufacturerCodeSegment)(data as object);
                return iface.GetManufacturerCode();
            }
            else
                throw new ValidationException("{0}编码段无法生成编码。实体[{0}]未实现编码段接口[{1}]。".L10nFormat(data.GetType().FullName, typeof(IManufacturerCodeSegment).FullName));
        }
    }
}

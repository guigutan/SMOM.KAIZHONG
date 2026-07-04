using SIE.Common.Algorithm;
using SIE.Domain.Validation;
using System;

namespace SIE.Items.Items.Barcodes
{
    /// <summary>
    /// 物料编码段
    /// </summary>
    [Algorithm("物料编码段算法", typeof(CodeAlgorithmConfig), AlgorithmType.Entity)]
    [RootEntity, Serializable]
    public class ItemCodeSegmentAlgorithm : EntityCodeAlgorithm
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
            if (data is IItemCodeSegment)
            {
                IItemCodeSegment iface = (IItemCodeSegment)(data as object);
                return iface.GetItemCodeSegment();
            }
            else
                throw new ValidationException("{0}编码段无法生成编码。实体[{0}]未实现编码段接口[{1}]。".L10nFormat(data.GetType().FullName, typeof(IItemCodeSegment).FullName));
        }
    }
}

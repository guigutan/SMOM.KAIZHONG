using SIE.Common.Algorithm;
using SIE.Domain.Validation;
using System;

namespace SIE.CSM.Suppliers.Algorithms
{
    /// <summary>
    /// 供应商编码段
    /// </summary>
    [Algorithm("供应商编码段", typeof(CodeAlgorithmConfig), AlgorithmType.Entity)]
    [RootEntity, Serializable]
    public class SupplierCodeSegmentAlgorithm : EntityCodeAlgorithm
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
            //供应商编码段数据接口
            if (data is ISupplierCodeSegment)
            {
                ISupplierCodeSegment iface = (ISupplierCodeSegment)(data as object);
                //返回供应商编码
                return iface.GetSupplierCode();
            }
            else
                throw new ValidationException("{0}编码段无法生成编码。实体[{0}]未实现编码段接口[{1}]。".L10nFormat(data.GetType().FullName, typeof(ISupplierCodeSegment).FullName));
        }
    }
}

using SIE.Common.Algorithm;
using SIE.Domain.Validation;
using System;

namespace SIE.CSM.Customers.Algorithms
{
    /// <summary>
    /// 客户编码段
    /// </summary>
    [Algorithm("客户编码段算法", typeof(CodeAlgorithmConfig), AlgorithmType.Entity)]
    [RootEntity, Serializable]
    public class CustomerCodeSegmentAlgorithm : EntityCodeAlgorithm
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
            //接口判断
            if (data is ICusomterCodeSegment)
            {
                //获取编码段
                ICusomterCodeSegment iface = (ICusomterCodeSegment)(data as object);
                return iface.GetCustomerCode();
            }
            else
                throw new ValidationException("{0}编码段无法生成编码。实体[{0}]未实现编码段接口[{1}]。".L10nFormat(data.GetType().FullName, typeof(ICusomterCodeSegment).FullName));
        }
    }
}

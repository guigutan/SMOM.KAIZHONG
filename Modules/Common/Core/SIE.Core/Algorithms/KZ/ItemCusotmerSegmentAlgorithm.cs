using SIE.Common.Algorithm;
using SIE.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Core.Algorithms.KZ
{
    #region KZ-客户编码算法
    /// <summary>
    /// KZ-客户编码算法
    /// </summary>
    [Algorithm("KZ-客户编码算法", typeof(CodeAlgorithmConfig), AlgorithmType.Common)]
    [RootEntity, Serializable]
    public class KzCustomerAlgorithm : CodeAlgorithm<ItemCusotmerData>
    {
        /// <summary>
        /// 获取 客户
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public override string GetCode(ItemCusotmerData data)
        {
            return data?.Customer;
        }
    } 
    #endregion

    #region KZ-客户料号算法
    /// <summary>
    /// KZ-客户料号算法
    /// </summary>
    [Algorithm("KZ-客户料号算法", typeof(CodeAlgorithmConfig), AlgorithmType.Common)]
    [RootEntity, Serializable]
    public class KzCodeAliasAlgorithm : CodeAlgorithm<ItemCusotmerData>
    {
        /// <summary>
        /// 获取 客户料号
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public override string GetCode(ItemCusotmerData data)
        {
            return data?.CodeAlias;
        }
    } 
    #endregion

    #region KZ-供应商代码算法
    /// <summary>
    /// KZ-供应商代码算法
    /// </summary>
    [Algorithm("KZ-供应商代码算法", typeof(CodeAlgorithmConfig), AlgorithmType.Common)]
    [RootEntity, Serializable]
    public class KzSupplierCodeAlgorithm : CodeAlgorithm<ItemCusotmerData>
    {
        /// <summary>
        /// 获取 供应商代码
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public override string GetCode(ItemCusotmerData data)
        {
            return data?.SupplierCode;
        }
    } 
    #endregion

    #region KZ-版本号算法
    /// <summary>
    /// KZ-版本号算法
    /// </summary>
    [Algorithm("KZ-版本号算法", typeof(CodeAlgorithmConfig), AlgorithmType.Common)]
    [RootEntity, Serializable]
    public class KzVersionNoAlgorithm : CodeAlgorithm<ItemCusotmerData>
    {
        /// <summary>
        /// 获取 版本号
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public override string GetCode(ItemCusotmerData data)
        {
            return data?.VersionNo;
        }
    } 
    #endregion

    #region KZ-项目名称算法
    /// <summary>
    /// KZ-项目名称算法
    /// </summary>
    [Algorithm("KZ-项目名称算法", typeof(CodeAlgorithmConfig), AlgorithmType.Common)]
    [RootEntity, Serializable]
    public class KzProjectNameAlgorithm : CodeAlgorithm<ItemCusotmerData>
    {
        /// <summary>
        /// 获取 项目名称
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public override string GetCode(ItemCusotmerData data)
        {
            return data?.ProjectName;
        }
    } 
    #endregion

    #region KZ-图号算法
    /// <summary>
    /// KZ-图号算法
    /// </summary>
    [Algorithm("KZ-图号算法", typeof(CodeAlgorithmConfig), AlgorithmType.Common)]
    [RootEntity, Serializable]
    public class KzDrawingAlgorithm : CodeAlgorithm<ItemCusotmerData>
    {
        /// <summary>
        /// 获取 图号
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public override string GetCode(ItemCusotmerData data)
        {
            return data?.Drawing;
        }
    }
    #endregion

    #region KZ-工单数量算法
    /// <summary>
    /// KZ-工单数量算法
    /// </summary>
    [Algorithm("KZ-工单数量算法", typeof(CodeAlgorithmConfig), AlgorithmType.Common)]
    [RootEntity, Serializable]
    public class KzWorkOrderQtyAlgorithm : CodeAlgorithm<ItemCusotmerData>
    {
        /// <summary>
        /// 获取 图号
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public override string GetCode(ItemCusotmerData data)
        {
            return data?.WorkOrderQty.ToString();
        }
    }
    #endregion

    #region KZ-客户料码 扩展属性算法

    /// <summary>
    /// KZ-客户料码-属性1
    /// </summary>
    [Algorithm("KZ-客户料码-属性1", typeof(CodeAlgorithmConfig), AlgorithmType.Common)]
    [RootEntity, Serializable]
    public class KzAttribute1Algorithm : CodeAlgorithm<ItemCusotmerData>
    {
        /// <summary>
        /// 获取 图号
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public override string GetCode(ItemCusotmerData data)
        {
            return data?.Attribute1;
        }
    }

    /// <summary>
    /// KZ-客户料码-属性2
    /// </summary>
    [Algorithm("KZ-客户料码-属性2", typeof(CodeAlgorithmConfig), AlgorithmType.Common)]
    [RootEntity, Serializable]
    public class KzAttribute2Algorithm : CodeAlgorithm<ItemCusotmerData>
    {
        /// <summary>
        /// 获取 图号
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public override string GetCode(ItemCusotmerData data)
        {
            return data?.Attribute2;
        }
    }
    /// <summary>
    /// KZ-客户料码-属性3
    /// </summary>
    [Algorithm("KZ-客户料码-属性3", typeof(CodeAlgorithmConfig), AlgorithmType.Common)]
    [RootEntity, Serializable]
    public class KzAttribute3Algorithm : CodeAlgorithm<ItemCusotmerData>
    {
        /// <summary>
        /// 获取 图号
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public override string GetCode(ItemCusotmerData data)
        {
            return data?.Attribute3;
        }
    }
    /// <summary>
    /// KZ-客户料码-属性4
    /// </summary>
    [Algorithm("KZ-客户料码-属性4", typeof(CodeAlgorithmConfig), AlgorithmType.Common)]
    [RootEntity, Serializable]
    public class KzAttribute4Algorithm : CodeAlgorithm<ItemCusotmerData>
    {
        /// <summary>
        /// 获取 图号
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public override string GetCode(ItemCusotmerData data)
        {
            return data?.Attribute4;
        }
    }
    /// <summary>
    /// KZ-客户料码-属性5
    /// </summary>
    [Algorithm("KZ-客户料码-属性5", typeof(CodeAlgorithmConfig), AlgorithmType.Common)]
    [RootEntity, Serializable]
    public class KzAttribute5Algorithm : CodeAlgorithm<ItemCusotmerData>
    {
        /// <summary>
        /// 获取 图号
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public override string GetCode(ItemCusotmerData data)
        {
            return data?.Attribute5;
        }
    }
    /// <summary>
    /// KZ-客户料码-属性6
    /// </summary>
    [Algorithm("KZ-客户料码-属性6", typeof(CodeAlgorithmConfig), AlgorithmType.Common)]
    [RootEntity, Serializable]
    public class KzAttribute6Algorithm : CodeAlgorithm<ItemCusotmerData>
    {
        /// <summary>
        /// 获取 图号
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public override string GetCode(ItemCusotmerData data)
        {
            return data?.Attribute6;
        }
    }
    /// <summary>
    /// KZ-客户料码-属性7
    /// </summary>
    [Algorithm("KZ-客户料码-属性7", typeof(CodeAlgorithmConfig), AlgorithmType.Common)]
    [RootEntity, Serializable]
    public class KzAttribute7Algorithm : CodeAlgorithm<ItemCusotmerData>
    {
        /// <summary>
        /// 获取 图号
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public override string GetCode(ItemCusotmerData data)
        {
            return data?.Attribute7;
        }
    }
    /// <summary>
    /// KZ-客户料码-属性8
    /// </summary>
    [Algorithm("KZ-客户料码-属性8", typeof(CodeAlgorithmConfig), AlgorithmType.Common)]
    [RootEntity, Serializable]
    public class KzAttribute8Algorithm : CodeAlgorithm<ItemCusotmerData>
    {
        /// <summary>
        /// 获取 图号
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public override string GetCode(ItemCusotmerData data)
        {
            return data?.Attribute8;
        }
    }
    /// <summary>
    /// KZ-客户料码-属性9
    /// </summary>
    [Algorithm("KZ-客户料码-属性9", typeof(CodeAlgorithmConfig), AlgorithmType.Common)]
    [RootEntity, Serializable]
    public class KzAttribute9Algorithm : CodeAlgorithm<ItemCusotmerData>
    {
        /// <summary>
        /// 获取 图号
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public override string GetCode(ItemCusotmerData data)
        {
            return data?.Attribute9;
        }
    }
    /// <summary>
    /// KZ-客户料码-属性10
    /// </summary>
    [Algorithm("KZ-客户料码-属性10", typeof(CodeAlgorithmConfig), AlgorithmType.Common)]
    [RootEntity, Serializable]
    public class KzAttribute10Algorithm : CodeAlgorithm<ItemCusotmerData>
    {
        /// <summary>
        /// 获取 图号
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public override string GetCode(ItemCusotmerData data)
        {
            return data?.Attribute10;
        }
    }
    #endregion
}

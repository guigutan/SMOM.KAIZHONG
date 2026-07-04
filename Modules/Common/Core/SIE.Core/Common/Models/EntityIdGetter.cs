using SIE.Domain.Validation;
using System;
using System.Collections.Generic;

namespace SIE.Core.Common.Models
{
    /// <summary>
    /// 实体ID提供者
    /// </summary>
    public class EntityIdGetter
    {
        /// <summary>
        /// ID集合
        /// </summary>
        private readonly List<double> _entityIdList;

        /// <summary>
        /// 当前ID索引
        /// </summary>
        private int currentIndex;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="idList"></param>
        public EntityIdGetter(List<double> idList) => _entityIdList = idList;

        /// <summary>
        /// 获取可用ID
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        public double GetNextId()
        {
            if (currentIndex >= _entityIdList.Count)
            {
                throw new ValidationException("ID索引超出范围".L10N());
            }
            return _entityIdList[currentIndex++];
        }
    }
}

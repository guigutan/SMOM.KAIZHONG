using Microsoft.Scripting.Utils;
using SIE.Core.Common.Service;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Items;
using SIE.MES.Validitys.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.Validitys.Helpers
{
    /// <summary>
    /// 有效期标准维护帮助类
    /// </summary>
    public class ValidityHelper
    {
        #region 构造
        /// <summary>
        /// 构造
        /// </summary>
        public ValidityHelper()
        {

        }

        /// <summary>
        /// 保存构造(校验数据合法性)
        /// </summary>
        /// <param name="datas"></param>
        public ValidityHelper(EntityList datas)
        {
            SaveValiditys = datas as EntityList<ValidityStandard>;
            SaveItemIds = SaveValiditys.Select(p => p.ItemId).ToList();
            SaveIds = SaveValiditys.Select(p => p.Id).ToList();
            ValidityService = RT.Service.Resolve<ValidityService>();
        }
        #endregion

        #region 属性
        /// <summary>
        /// 标准维护数据
        /// </summary>
        public EntityList<ValidityStandard> ValidityStandards { get; set; }

        /// <summary>
        /// 物料
        /// </summary>
        public EntityList<Item> Items { get; set; }

        private ValidityService ValidityService { get; set; }

        /// <summary>
        /// 前端保存数据
        /// </summary>
        private EntityList<ValidityStandard> SaveValiditys { get; set; }

        /// <summary>
        /// 后端保存数据
        /// </summary>
        private EntityList<ValidityStandard> SaveDBValiditys {  get; set; }

        /// <summary>
        /// 保存校验启用扩展属性
        /// </summary>
        private EntityList<Item> SaveItems { get; set; }

        /// <summary>
        /// 保存数据的物料Ids
        /// </summary>
        private List<double> SaveItemIds {  get; set; }

        /// <summary>
        /// 保存数据的Ids
        /// </summary>
        private List<double> SaveIds { get; set; }
        #endregion

        #region 方法

        /// <summary>
        /// 验证必填项
        /// </summary>
        private void ValidateRequired()
        {
            if (SaveValiditys.Any(p => p.ItemId == 0))
            {
                throw new ValidationException("物料必填！".L10N());
            }
        }

        /// <summary>
        /// 校验可用时长
        /// </summary>
        private void ValidateLongLived()
        {
            if (SaveValiditys.Any(p => p.LongLived <= 0))
            {
                throw new ValidationException("可用时长寿命必须大于0！".L10N());
            }
        }

        /// <summary>
        /// 物料扩展属性验证
        /// </summary>
        /// <exception cref="ValidationException"></exception>
        private void ValidateEnableExtProp()
        {
            if (SaveValiditys.Any(p => SaveItems.FirstOrDefault(q => q.Id == p.ItemId).EnableExtendProperty && (p.ItemExtProp.IsNullOrEmpty() || p.ItemExtPropName.IsNullOrEmpty())))
            {
                throw new ValidationException("物料已启用扩展属性，请维护其对应的扩展属性值".L10N());
            }
        }

        /// <summary>
        /// 验证生效日期<=失效日期
        /// </summary>
        private void ValidateDate()
        {
            if (SaveValiditys.Where(p => p.Expiration.HasValue).Any(p => (p.Effective.Date > p.Expiration.Value.Date)))
            {
                throw new ValidationException("失效日期不能小于生效日期".L10N());
            }
        }

        /// <summary>
        /// 获取数据库同物料
        /// </summary>
        private void GetDBStandard()
        {
            // 获取同物料Id数据后续校验时间重叠
            SaveDBValiditys = ValidityService.GetValidityStandardByItemIds(SaveItemIds, SaveIds);

            // 获取物料校验启用扩展属性
            SaveItems = RT.Service.Resolve<ItemController>().GetItemList(SaveItemIds);

        }

        /// <summary>
        /// 验证同一物料(扩展属性)有效期不能有交叉
        /// </summary>
        private void ValidateNotSameDate()
        {
            var validity = new EntityList<ValidityStandard>(); 
            validity.AddRange(SaveValiditys);
            validity.AddRange(SaveDBValiditys);
            foreach (var keyValues in validity.GroupBy(p => new { p.ItemId,p.ItemExtProp }).ToList())
            {
                var isCross = false;
                var sortedKeyValues = keyValues.OrderBy(kv => kv.Effective);

                DateTime currentEnd = DateTime.MinValue;
                // 在这里对排序后的 keyValues 进行操作
                foreach(var value in sortedKeyValues)
                {
                    // 只对日期进行比较
                    if (value.Effective.Date <= currentEnd.Date)
                    {
                        isCross = true;
                    }
                    if (value.Expiration.HasValue)
                    {
                        currentEnd = value.Expiration.Value;
                    }
                    else
                    {
                        currentEnd = DateTime.MaxValue;
                    }
                }
                if (isCross)
                {
                    throw new ValidationException("同一物料、物料扩展属性的有效期标准不能出现生效时间、失效时间范围重叠".L10N());
                }
            }
        }

        /// <summary>
        /// 保存
        /// </summary>
        public void SaveCommand()
        {
            // 获取数据库信息
            GetDBStandard();


            // 验证必填
            ValidateRequired();
            // 物料扩展属性验证
            ValidateEnableExtProp();
            // 验证可用时长
            ValidateLongLived();
            // 验证生效日期<=失效日期
            ValidateDate();
            // 验证同一物料(扩展属性)有效期不能有交叉
            ValidateNotSameDate();
        }

        #endregion
    }
}

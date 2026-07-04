using SIE.Core.Common;
using SIE.Domain;
using SIE.Domain.Validation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.WorkBenchCommon.Workbench.TargetWarn
{
    /// <summary>
    /// 预警设定控制器
    /// </summary>
    public class TargetWarnSettingController : DomainController
    {
        /// <summary>
        /// 查询预警设定实体列表
        /// </summary>
        /// <param name="criteria">预警设定查询实体</param>
        /// <returns>预警设定实体列表</returns>
        public virtual EntityList<TargetWarnSetting> GetTargetWarnSetting(TargetWarnSettingCriteria criteria)
        {
            if (criteria == null)
                throw new ArgumentNullException(nameof(criteria), "查询实体参数不能为空".L10N());
            var query = Query<TargetWarnSetting>();
            if (criteria.Code.IsNotEmpty())
                query.Where(p => p.Code.Contains(criteria.Code));
            if (criteria.Name.IsNotEmpty())
                query.Where(p => p.Name.Contains(criteria.Name));
            return query.ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据名称获取目标达成率预警设定
        /// </summary>
        /// <param name="nameList">名称集合</param>
        /// <returns>返回目标达成率预警设定</returns>
        public virtual EntityList<TargetWarnSetting> GetTargetWarnSetting(List<string> nameList)
        {
            EagerLoadOptions elo = new EagerLoadOptions();
            elo.LoadWithViewProperty().LoadWith(TargetWarnSetting.TargetWarnDetailListProperty);
            var query = Query<TargetWarnSetting>().Where(p => nameList.Contains(p.Name));
            return query.ToList(null, elo);
        }

        /// <summary>
        /// 根据指标分类、指标名称获取预警的达成率区间
        /// </summary>
        /// <param name="code">指标分类</param>
        /// <param name="name">指标名称</param>
        /// <returns>返回达成率区间</returns>
        public virtual EntityList<TargetWarnDetail> GetTargetWarnDetail(string code, string name)
        {
            var query = Query<TargetWarnDetail>().Join<TargetWarnSetting>((a, b) => a.TargetWarnSettingId == b.Id).Where<TargetWarnSetting>((a, b) => b.Code == code && b.Name == name);
            return query.ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="qt"></param>
        /// <returns></returns>
        public virtual bool ExistColor(TargetWarnDetail qt)
        {
            var query = Query<TargetWarnDetail>().Where(c => c.Id != qt.Id && c.TargetWarnSettingId == qt.TargetWarnSettingId && c.TargetColor == qt.TargetColor);
            var entity = query.FirstOrDefault();
            return entity != null;
        }


        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="setting"></param>
        public virtual void Save(TargetWarnSetting setting)
        {
            setting.GetAllChildData<TargetWarnSetting, TargetWarnDetail>();
            foreach (var item in setting.TargetWarnDetailList)
            {
                if (setting.TargetWarnDetailList.ToList().Exists(c => c.Id != item.Id && c.TargetColor == item.TargetColor))
                {
                    throw new ValidationException("颜色重复".L10N());
                }
            }


            RF.Save(setting);


        }
    }
}

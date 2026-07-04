using SIE.Domain;
using SIE.Domain.Validation;
using System;

namespace SIE.Inventory.Strategy
{
    /// <summary>
    /// 
    /// </summary>
    public class TurnOverRuleDetailSortRuleViewModelController : DomainController
    {
        /// <summary>
        /// 获取排序规则
        /// </summary>
        /// <param name="turnOverRuleDetailId"></param>
        /// <returns></returns>
        public virtual EntityList<TurnOverRuleDetailSortRuleViewModel> GetList(double turnOverRuleDetailId)
        {
            TurnOverRuleDetail detail = RF.GetById<TurnOverRuleDetail>(turnOverRuleDetailId, new EagerLoadOptions().LoadWithViewProperty()) ?? new TurnOverRuleDetail();
            EntityList<TurnOverRuleDetailSortRuleViewModel> list = new EntityList<TurnOverRuleDetailSortRuleViewModel>()
            {
                new TurnOverRuleDetailSortRuleViewModel()
            {
                SortName = "排序1",
                SortField=detail.SortField1,
                FieldType=detail.FieldType1,
                SortType = detail.SortType1,
                EqualValue = detail.EqualValue1,
                LowerLimit = detail.LowerLimit1,
                UpperLimit = detail.UpperLimit1,
                LowerLimitDay = detail.LowerLimitDay1,
                UpperLimitDay=detail.UpperLimitDay1,
              
            },
                new TurnOverRuleDetailSortRuleViewModel()
                {
                    SortName = "排序2",
                    SortField = detail.SortField2,
                    FieldType = detail.FieldType2,
                    SortType = detail.SortType2,
                    EqualValue = detail.EqualValue2,
                    LowerLimit = detail.LowerLimit2,
                    UpperLimit = detail.UpperLimit2,
                    LowerLimitDay = detail.LowerLimitDay2,
                    UpperLimitDay = detail.UpperLimitDay2,
                    
                },
                new TurnOverRuleDetailSortRuleViewModel()
                {
                    SortName = "排序3",
                    SortField = detail.SortField3,
                    FieldType = detail.FieldType3,
                    SortType = detail.SortType3,
                    EqualValue = detail.EqualValue3,
                    LowerLimit = detail.LowerLimit3,
                    UpperLimit = detail.UpperLimit3,
                    LowerLimitDay = detail.LowerLimitDay3,
                    UpperLimitDay = detail.UpperLimitDay3,
                   
                },
                new TurnOverRuleDetailSortRuleViewModel()
                {
                    SortName = "排序4",
                    SortField = detail.SortField4,
                    FieldType = detail.FieldType4,
                    SortType = detail.SortType4,
                    EqualValue = detail.EqualValue4,
                    LowerLimit = detail.LowerLimit4,
                    UpperLimit = detail.UpperLimit4,
                    LowerLimitDay = detail.LowerLimitDay4,
                    UpperLimitDay = detail.UpperLimitDay4,
                   
                },
                 new TurnOverRuleDetailSortRuleViewModel()
                {
                    SortName = "排序5",
                    SortField = detail.SortField5,
                    FieldType = detail.FieldType5,
                    SortType = detail.SortType5,
                    EqualValue = detail.EqualValue5,
                    LowerLimit = detail.LowerLimit5,
                    UpperLimit = detail.UpperLimit5,
                    LowerLimitDay = detail.LowerLimitDay5,
                    UpperLimitDay = detail.UpperLimitDay5,
                    
                }
            };
            return list;
        }
        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="detail"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public virtual string SaveDatas(TurnOverRuleDetail detail, EntityList<TurnOverRuleDetailSortRuleViewModel> list)
        {
            string msg = string.Empty;
           
            detail.SortField1 = list[0].SortField;
            detail.FieldType1 = list[0].FieldType;
            detail.SortType1 = list[0].SortType;
            detail.EqualValue1 = list[0].EqualValue;
            detail.LowerLimit1 = list[0].LowerLimit;
            detail.UpperLimit1 = list[0].UpperLimit;
            detail.LowerLimitDay1 = list[0].LowerLimitDay;
            detail.UpperLimitDay1 = list[0].UpperLimitDay;
           
            detail.SortField2 = list[1].SortField;
            detail.FieldType2 = list[1].FieldType;
            detail.SortType2 = list[1].SortType;
            detail.EqualValue2 = list[1].EqualValue;
            detail.LowerLimit2 = list[1].LowerLimit;
            detail.UpperLimit2 = list[1].UpperLimit;
            detail.LowerLimitDay2 = list[1].LowerLimitDay;
            detail.UpperLimitDay2 = list[1].UpperLimitDay;
           
            detail.SortField3 = list[2].SortField;
            detail.FieldType3 = list[2].FieldType;
            detail.SortType3 = list[2].SortType;
            detail.EqualValue3 = list[2].EqualValue;
            detail.LowerLimit3 = list[2].LowerLimit;
            detail.UpperLimit3 = list[2].UpperLimit;
            detail.LowerLimitDay3 = list[2].LowerLimitDay;
            detail.UpperLimitDay3 = list[2].UpperLimitDay;
           
            detail.SortField4 = list[3].SortField;
            detail.FieldType4 = list[3].FieldType;
            detail.SortType4 = list[3].SortType;
            detail.EqualValue4 = list[3].EqualValue;
            detail.LowerLimit4 = list[3].LowerLimit;
            detail.UpperLimit4 = list[3].UpperLimit;
            detail.LowerLimitDay4 = list[3].LowerLimitDay;
            detail.UpperLimitDay4 = list[3].UpperLimitDay;
            
            detail.SortField5 = list[4].SortField;
            detail.FieldType5 = list[4].FieldType;
            detail.SortType5 = list[4].SortType;
            detail.EqualValue5 = list[4].EqualValue;
            detail.LowerLimit5 = list[4].LowerLimit;
            detail.UpperLimit5 = list[4].UpperLimit;
            detail.LowerLimitDay5 = list[4].LowerLimitDay;
            detail.UpperLimitDay5 = list[4].UpperLimitDay;
          
            TurnOverRuleDetail entity = RF.GetById<TurnOverRuleDetail>(detail.Id);
            detail.PersistenceStatus = entity == null ? PersistenceStatus.New : PersistenceStatus.Modified;
            RF.Save(detail);
            return msg;
        }
    }
}

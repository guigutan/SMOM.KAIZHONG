using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MetaModel;
using System;
using System.Linq;

namespace SIE.Fixtures.Models
{
    #region 工治具编码保养项目非重复规则
    /// <summary>
    /// 工治具编码保养项目非重复规则
    /// </summary>
    [System.ComponentModel.DisplayName("工治具编码保养项目非重复规则")]
    [System.ComponentModel.Description("工治具编码保养项目不能重复")]
    public class MaintainProjectDuplicateRule : NotDuplicateRule<FixtureEncodeMaintainProject>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public MaintainProjectDuplicateRule()
        {
            Properties.Add(FixtureEncodeMaintainProject.FixtureEncodeIdProperty);
            Properties.Add(FixtureEncodeMaintainProject.MaintainProjectIdProperty);
            MessageBuilder = (e) =>
            {
                var entity = e as FixtureEncodeMaintainProject;
                return "已存在项目名称[{0}]的保养项目".L10nFormat(entity.MaintainProject.Name);
            };
        }
    }
    #endregion

    #region 工治具编码产品清单非重复规则
    ///// <summary>
    ///// 工治具编码产品清单非重复规则
    ///// </summary>
    //[System.ComponentModel.DisplayName("工治具编码产品清单非重复规则")]
    //[System.ComponentModel.Description("工治具编码产品清单:产品+工艺面不能重复")]
    //public class ProductDetailDuplicateRule : NotDuplicateRule<FixtureEncodeProductDetail>
    //{
    //    /// <summary>
    //    /// 构造函数
    //    /// </summary>
    //    public ProductDetailDuplicateRule()
    //    {
    //        // 当工段为空时不走校验，转保存校验
    //        Properties.Add(FixtureEncodeProductDetail.FixtureEncodeIdProperty);
    //        Properties.Add(FixtureEncodeProductDetail.ItemIdProperty);
    //        Properties.Add(FixtureEncodeProductDetail.ProcessSegmentIdProperty);
    //        Properties.Add(FixtureEncodeProductDetail.DeckProperty);
    //        MessageBuilder = (e) =>
    //        {
    //            var entity = e as FixtureEncodeProductDetail;
    //            return "已存在产品[{0}]+工艺面[{1}]的产品清单".L10nFormat(entity.Item.Code, entity.Deck?.ToLabel());
    //        };
    //    }
    //}
    #endregion

    #region 工治具编码-上线保养项目规则
    /// <summary>
    /// 工治具编码-上线保养项目规则
    /// </summary>
    [System.ComponentModel.DisplayName("工治具编码-上线保养项目规则")]
    [System.ComponentModel.Description("工治具编码中【上线定期保养标准】大于0时，保养项目至少维护一行上线定期保养项目数据方可保存")]
    public class FixtureEncodeOnlineProjectRule : EntityRule<FixtureEncode>
    {
        /// <summary>
        /// 工治具编码-上线保养项目规则
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="e">规则参数</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var fixtureEncode = entity as FixtureEncode;
            var fixtureModel = RF.GetById<FixtureModel>(fixtureEncode.FixtureModelId);
            if (fixtureEncode.FixedStorage == YesNo.Yes && !fixtureEncode.FixtureEncodeStorageLocationList.Any())
            {
                e.BrokenDescription = "【固定储位】为【是】时，存储位置至少维护一行数据方可保存".L10N();
            }

            if (fixtureModel == null || fixtureModel.OnlineHour <= 0)
            {
                return;
            }
            if (fixtureEncode.PersistenceStatus == PersistenceStatus.New && !fixtureEncode.FixtureEncodeMaintainProjectList.Any(p => p.OnlineMaintain))
            {
                e.BrokenDescription = "上线定期保养的工治具，需存在勾选了上线定期保养的项目".L10N();
            }
            if (fixtureEncode.PersistenceStatus == PersistenceStatus.Modified)
            {
                if (fixtureEncode.FixtureEncodeMaintainProjectList.Any(p => p.OnlineMaintain))
                {
                    return;
                }
                var modifiedIds = fixtureEncode.FixtureEncodeMaintainProjectList.Select(p => p.Id).ToList<double>();
                if (fixtureEncode.FixtureEncodeMaintainProjectList.Deleted.Count > 0)
                {
                    var deleteds = fixtureEncode.FixtureEncodeMaintainProjectList.Deleted;
                    foreach (FixtureEncodeMaintainProject deleted in deleteds)
                    {
                        modifiedIds.Add(deleted.Id);
                    }
                }
                var isExist = RT.Service.Resolve<CoreFixtureController>().IsExistEncodeOnlineMaintains(fixtureEncode.Id, modifiedIds);
                if (!isExist)
                {
                    e.BrokenDescription = "上线定期保养的工治具，需存在勾选了上线定期保养的项目".L10N();
                }
            }
        }
    }
    #endregion	

    /// <summary>
    /// 工治具型号保养项目实体验证规则
    /// </summary>
    [System.ComponentModel.DisplayName("工治具编码保养项目实体验证规则")]
    [System.ComponentModel.Description("配置的工治具保养项目列表,提交前校验")]
    public class FixtureEncodeMaintainProjectRule : EntityRule<FixtureEncodeMaintainProject>
    {
        /// <summary>
        /// 验证配置的设备类型，必须填写位置列表至少一行
        /// </summary>
        /// <param name="entity">设备台帐</param>
        /// <param name="e">参数</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var fixtureEncodeMaintainProject = entity as FixtureEncodeMaintainProject;
            if (fixtureEncodeMaintainProject != null)
            {
                if (fixtureEncodeMaintainProject.MinValue > fixtureEncodeMaintainProject.MaxValue)
                    e.BrokenDescription = "保养项目子页签中应检测合格小值≤检测合格大值".L10N();
            }
        }
    }
}

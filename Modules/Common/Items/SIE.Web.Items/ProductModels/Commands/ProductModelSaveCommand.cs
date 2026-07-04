using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Items;
using SIE.Web.Command;
using System;
using System.Linq;

namespace SIE.Web.Items.ProductModels.Commands
{
    /// <summary>
    /// 保存
    /// </summary>
    [JsCommand("SIE.Web.Items.ProductModels.Commands.ProductModelSaveCommand")]
    public class ProductModelSaveCommand : ViewCommand<ViewArgs>
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>执行结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var data = args.Data.ToJsonObject<ProductModelSaveViewArgs>();

            //添加检查，这段校验是解决平台报了两个相同的非空提示，其他校验交给平台
            data.ProductModelLineCapacityList.ForEach(a =>
            {
                if (a.ResourceId == 0)
                {
                    throw new ValidationException("资源不能为空".L10N());
                }
            });
            data.ProductModelSkillList.ForEach(a =>
            {
                if (a.SkillId == 0)
                {
                    throw new ValidationException("技能清单不能为空".L10N());
                }
            });

            RF.Save(data.ProductModelList);
            RF.Save(data.ProductModelLineCapacityList);
            RF.Save(data.ProductModelSkillList);
            return true;
        }
    }

    /// <summary>
    /// 参数
    /// </summary>
    public class ProductModelSaveViewArgs
    {
        /// <summary>
        /// 产品机型
        /// </summary>
        public EntityList<ProductModel> ProductModelList { get; set; }

        /// <summary>
        /// 产线产能
        /// </summary>
        public EntityList<ProductModelLineCapacity> ProductModelLineCapacityList { get; set; }

        /// <summary>
        /// 机型技能
        /// </summary>
        public EntityList<ProductModelSkill> ProductModelSkillList { get; set; }
    }
}

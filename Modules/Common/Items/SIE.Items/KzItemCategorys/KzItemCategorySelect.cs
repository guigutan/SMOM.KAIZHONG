using MailKit;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Items.KzItemCategorys
{
    /// <summary>
    /// 选择分类
    /// </summary>
    [RootEntity, Serializable]
    [CriteriaQuery]
    [Label("选择分类")]
    [DisplayMember(nameof(Code))]
    public class KzItemCategorySelect : StringEntity
    {
        #region 分类编码 Code
        /// <summary>
        /// 分类编码
        /// </summary>
        [Label("分类编码")]
        public static readonly Property<string> CodeProperty = P<KzItemCategorySelect>.Register(e => e.Code);

        /// <summary>
        /// 分类编码
        /// </summary>
        public string Code
        {
            get { return this.GetProperty(CodeProperty); }
            set { this.SetProperty(CodeProperty, value); }
        }
        #endregion
    }

    internal class KzItemCategorySelectConfig : EntityConfig<KzItemCategorySelect>
    {
        protected override void ConfigMeta()
        {
            Meta.MapView("KZ_CATEGORY").MapAllProperties();
            Meta.DisableInvOrg();
            Meta.DisablePhantoms();
        }
    }
}

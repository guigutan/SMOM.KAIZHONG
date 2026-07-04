using SIE.Kit.APS.Common;
using SIE.Kit.APS.ProductLocations;
using SIE.MetaModel.View;
using SIE.Web.Kit.APS.ProductLocations.Commands;
using SIE.Web.Resources;

namespace SIE.Web.Kit.APS.ProductLocations
{
    /// <summary>
    /// 产品定位视图配置
    /// </summary>
    internal class ProductLocationViewConfig : WebViewConfig<ProductLocation>
    {
        ///<summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
        }

        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.AddBehavior("SIE.Web.Kit.APS.ProductLocations.Behaviors.ProductLocationPropertyBehavior");
            View.UseDefaultCommands();
            View.UseCommands(typeof(ImportProductLocationCommand).FullName);
            View.RemoveCommands(WebCommandNames.Copy);

            using (View.OrderProperties())
            {
                View.Property(p => p.EnterpriseId).UseFactoryEditor();
                View.Property(p => p.Classification);
                View.Property(p => p.TypeValue).UseProductLocationTypeEditor(p =>
                {
                    p.Editable = false;
                    p.ReloadDataOnPopping = true;
                    p.JsClassName = "SIE.Web.Kit.APS.ProductLocations.Scripts.ProductLocationTypeComboList";
                    p.ValueField = ClassificationInfo.KeyProperty.Name;
                }).UseListSetting(e => { e.HelpInfo = "显示选择分类的分类值"; }).ShowInList(width: 80);

                //View.Property(p => p.MinValue).UseSpinEditor(p =>
                //{
                //    p.MinValue = 0;
                //    p.AllowDecimals = true;
                //}).HasLabel("最小值(包含)").ShowInList(width: 120).Readonly();
                //View.Property(p => p.MaxValue).UseSpinEditor(p =>
                //{
                //    p.MinValue = 0;
                //    p.AllowDecimals = true;
                //}).HasLabel("最大值(不包含)").ShowInList(width: 120).Readonly();
                //View.Property(p => p.Remark);
            }
        }

        /// <summary>
        /// 配置数据导入的视图
        /// </summary>
        protected override void ConfigImportView()
        {
            View.PropertyRef(p => p.Enterprise.Code).HasLabel("工厂编码");
            View.Property(p => p.Classification).HasLabel("分类");
            View.Property(p => p.TypeValue).HasLabel("分类值");
            View.Property(p => p.MinValue).HasLabel("最小值");
            View.Property(p => p.MaxValue).HasLabel("最大值");
            View.Property(p => p.Remark).HasLabel("备注");
        }
    }
}
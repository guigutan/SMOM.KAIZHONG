using SIE.Domain;
using SIE.MetaModel.View;
using SIE.Core.QmsStaticConst;
using SIE.Web.Core.QmsStaticConst.Commands;

namespace SIE.Web.Core.QmsStaticConst
{
	/// <summary>
	/// MAS常用参数视图配置
	/// </summary>
	internal class StaticConstViewConfig : WebViewConfig<StaticConst>
	{
		
		///<summary>
		/// 配置列表视图
		/// </summary>
		protected override void ConfigListView()
		{
			View.AddBehavior("SIE.Web.Core.QmsStaticConst.Behaviors.StaticConstBehavior");
			View.InlineEdit();
			View.ClearCommands();
			View.UseCommands(typeof(AddConstCommand).FullName, typeof(SaveConstCommand).FullName, WebCommandNames.Delete,typeof(ImportCommand).FullName); 
			View.Property(p => p.Code);
			View.Property(p => p.Name);
			View.ChildrenProperty(p => p.ListControlChart).Show(ChildShowInWhere.Hide);
			View.ChildrenProperty(p => p.ListK1).Show(ChildShowInWhere.Hide);
			View.ChildrenProperty(p => p.ListK2).Show(ChildShowInWhere.Hide);
			View.ChildrenProperty(p => p.ListK3).Show(ChildShowInWhere.Hide);
			View.AttachChildrenProperty(typeof(ControlChartConst), (o) =>
			{
				var parent = o.Parent as StaticConst;
				var list = RT.Service.Resolve<StaticConstService>().GetControlChartConsts(parent.Id);			
				return list;
			}, directionParentPropertyName: StaticConst.ListControlChartProperty.Name).HasLabel("控制图参数").Show(ChildShowInWhere.All).HasOrderNo(1);
			View.AttachChildrenProperty(typeof(StaticConstK1), (o) =>
			{
				var parent = o.Parent as StaticConst;
				var list = RT.Service.Resolve<StaticConstService>().GetMsaConstK1s(parent.Id);
				return list;
			}, directionParentPropertyName: StaticConst.ListK1Property.Name).HasLabel("K1").Show(ChildShowInWhere.All).HasOrderNo(2);
			View.AttachChildrenProperty(typeof(StaticConstK2), (o) =>
			{
				var parent = o.Parent as StaticConst;
				var list = RT.Service.Resolve<StaticConstService>().GetMsaConstK2s(parent.Id);
				return list;
			}, directionParentPropertyName: StaticConst.ListK2Property.Name).HasLabel("K2").Show(ChildShowInWhere.All).HasOrderNo(3);
			View.AttachChildrenProperty(typeof(StaticConstK3), (o) =>
			{
				var parent = o.Parent as StaticConst;
				var list = RT.Service.Resolve<StaticConstService>().GetMsaConstK3s(parent.Id);
				return list;
			}, directionParentPropertyName: StaticConst.ListK3Property.Name).HasLabel("K3").Show(ChildShowInWhere.All).HasOrderNo(4);
			View.ChildrenProperty(p => p.ListD2).Show(ChildShowInWhere.All).HasOrderNo(5);
			View.ChildrenProperty(p => p.ListT).Show(ChildShowInWhere.All).HasOrderNo(6);
		}
		
		///<summary>
		/// 配置下拉视图
		/// </summary>
		protected override void ConfigSelectionView()
		{
			View.Property(p => p.Code);
			View.Property(p => p.Name);
		}

		/// <summary>
		/// 查询视图
		/// </summary>
        protected override void ConfigQueryView()
        {
			View.Property(p => p.Code);
			View.Property(p => p.Name);
			View.Property(p => p.CreateDate).UseDateRangeEditor(c=>c.DateRangeType=ObjectModel.DateRangeType.All);
		}
    }
}
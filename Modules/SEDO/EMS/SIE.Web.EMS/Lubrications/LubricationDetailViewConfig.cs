using SIE.Domain;
using SIE.EMS.Equipments.Accounts;
using SIE.EMS.Lubrications;
using SIE.MetaModel.View;
using SIE.Web.EMS.Equipments.Accounts;
using SIE.Web.EMS.Lubrications.Commands;
using System;

namespace SIE.WPF.EMS.Lubrications
{
    /// <summary>
    /// 润滑项目视图配置
    /// </summary>
    public class LubricationDetailViewConfig : WebViewConfig<LubricationDetail>
	{
		/// <summary>
		/// 查看记录
		/// </summary>
		public const string SeeView = "SeeView";

		/// <summary>
		/// 添加记录页面
		/// </summary>
		public const string AddReportView = "AddReportView";

		/// <summary>
		/// 字符显示宽度
		/// </summary>
		private const int charWidth = 20;

		///<summary>
		/// 配置视图
		/// </summary>
		protected override void ConfigView()
		{
			View.DeclareExtendViewGroup(SeeView, AddReportView);
			if (ViewGroup == SeeView)
			{
				ConfigSeeView();
			}
			if (ViewGroup == AddReportView) {
				ConfigAddReportView();
			}
		}

		///<summary>
		/// 配置列表视图
		/// </summary>
		protected override void ConfigListView()
		{
			View.AssignAuthorize(typeof(Lubrication));
			View.InlineEdit();
			View.UseCommands("SIE.Web.EMS.Lubrications.Commands.SelModelLubricationDetailCommand", WebCommandNames.Delete);
			View.UseChildrenAsHorizontal();
			View.UseLayoutSize(0.71, 0.29);

			using (View.OrderProperties())
			{
				View.Property(p => p.ProjectName).Readonly().HasLabel("项目名称").ShowInList(width: (charWidth * 12));
				View.Property(p => p.Part).Readonly().ShowInList(width: (charWidth * 10));
				View.Property(p => p.LubricatingType).Readonly().ShowInList(width: (charWidth * 4));
				View.Property(p => p.MinValue).HasLabel("加油量下限").Readonly().ShowInList(width: (charWidth * 5));
				View.Property(p => p.MaxValue).HasLabel("加油量上限").Readonly().ShowInList(width: (charWidth * 5));
				View.Property(p => p.Unit).HasLabel("加油单位").Readonly().ShowInList(width: (charWidth * 4));
				View.Property(p => p.ActualValue).Readonly().HasLabel("实际加油量").ShowInList(width: (charWidth * 5));
				View.Property(p => p.DelayDays).Readonly().ShowInList(width: (charWidth * 4));
				View.Property(p => p.Method).Readonly().ShowInList(width: (charWidth * 15));

				View.Property(p => p.CycleType).Readonly().ShowInList(width: (charWidth * 4));
				View.Property(p => p.ProjectCycle).UseSpinEditor(p =>
				{
					p.AllowDecimals = false;
					p.MinValue = 0;
				}).Readonly().ShowInList(width: (charWidth * 4));
				View.Property(p => p.WarningPeriod).Readonly().ShowInList(width: (charWidth * 4));

				View.Property(p => p.Standard).Readonly().ShowInList(width: (charWidth * 12));
				View.Property(p => p.Consumable).Readonly().ShowInList(width: (charWidth * 8));
				View.Property(p => p.UseTime).Readonly().ShowInList(width: (charWidth * 5));

				View.Property(p => p.CreateBy).Show(ShowInWhere.Hide);
				View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
				View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
				View.Property(p => p.UpdateBy).Show(ShowInWhere.Hide);
				View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
				View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
			}

		}

		/// <summary>
		/// 查看列表视图
		/// </summary>
		protected void ConfigSeeView()
		{
			View.InlineEdit();
			View.UseChildrenAsHorizontal();
			View.UseLayoutSize(0.71, 0.29);

			using (View.OrderProperties())
			{
				View.Property(p => p.ProjectName).Readonly().HasLabel("项目名称").ShowInList(width: (charWidth * 12));
				View.Property(p => p.Part).Readonly().ShowInList(width: (charWidth * 10));
				View.Property(p => p.LubricatingType).Readonly().ShowInList(width: (charWidth * 4));
				View.Property(p => p.MinValue).HasLabel("加油量下限").Readonly().ShowInList(width: (charWidth * 5));
				View.Property(p => p.MaxValue).HasLabel("加油量上限").Readonly().ShowInList(width: (charWidth * 5));
				View.Property(p => p.Unit).HasLabel("加油单位").Readonly().ShowInList(width: (charWidth * 4));
				View.Property(p => p.ActualValue).Readonly().HasLabel("实际加油量").ShowInList(width: (charWidth * 5));
				View.Property(p => p.DelayDays).Readonly().ShowInList(width: (charWidth * 4));
				View.Property(p => p.Method).Readonly().ShowInList(width: (charWidth * 15));

				View.Property(p => p.CycleType).Readonly().ShowInList(width: (charWidth * 4));
				View.Property(p => p.ProjectCycle).UseSpinEditor(p =>
				{
					p.AllowDecimals = false;
					p.MinValue = 0;
				}).Readonly().ShowInList(width: (charWidth * 4));
				View.Property(p => p.WarningPeriod).Readonly().ShowInList(width: (charWidth * 4));

				View.Property(p => p.Standard).Readonly().ShowInList(width: (charWidth * 12));
				View.Property(p => p.Consumable).Readonly().ShowInList(width: (charWidth * 8));
				View.Property(p => p.UseTime).Readonly().ShowInList(width: (charWidth * 5));

				View.Property(p => p.CreateBy).Show(ShowInWhere.Hide);
				View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
				View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
				View.Property(p => p.UpdateBy).Show(ShowInWhere.Hide);
				View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
				View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
                //加载润滑项目备件信息
                View.AttachChildrenProperty(typeof(EquipAccountLubricaSparePart), w =>
                {
                    var args = w as ChildPagingDataArgs;
                    var parent = args.Parent.CastTo<LubricationDetail>();
                    if (parent == null)
                    {
                        return new EntityList<EquipAccountLubricaSparePart>();
                    }
                    return RT.Service.Resolve<LubricationController>().GetEquipAccountLubricaSparePart(parent.Id, args.SortInfo, args.PagingInfo);
                }, EquipAccountLubricaSparePartViewConfig.SeeViewGroup).HasLabel("备件清单").HasOrderNo(6);
            }
        }

		///<summary>
		/// 配置列表视图
		/// </summary>
		protected void ConfigAddReportView()
		{
			View.AddBehavior("SIE.Web.EMS.Lubrications.Behaviors.LubricationDetailAddBehavior");
			//一键润滑
			View.AssignAuthorize(typeof(Lubrication));
			View.UseCommands(LubricationCommands.LubricationOnekeyCommand);
			View.UseChildrenAsHorizontal();
			View.UseLayoutSize(0.71, 0.29);
			
			using (View.OrderProperties())
			{
				View.Property(p => p.ProjectName).Readonly().HasLabel("项目名称").ShowInList(width: (charWidth * 12));
				View.Property(p => p.Part).Readonly().ShowInList(width: (charWidth * 10));
				View.Property(p => p.LubricatingType).Readonly().ShowInList(width: (charWidth * 4));
				View.Property(p => p.MinValue).UseSpinEditor(p =>
				{
					p.MinValue = 0;
				}).HasLabel("加油量下限").Readonly().ShowInList(width: (charWidth * 5));
				View.Property(p => p.MaxValue).UseSpinEditor(p =>
				{
					p.MinValue = 0;
				}).HasLabel("加油量上限").Readonly().ShowInList(width: (charWidth * 5));
				View.Property(p => p.Unit).HasLabel("加油单位").Readonly().ShowInList(width: (charWidth * 4));
				View.Property(p => p.ActualValue).HasLabel("实际加油量").ShowInList(width: (charWidth * 5));
				View.Property(p => p.DelayDays).UseSpinEditor(p=>
				{
					p.MinValue = 1;
				}).ShowInList(width: (charWidth * 4));
				View.Property(p => p.Method).Readonly().ShowInList(width: (charWidth * 15));

				View.Property(p => p.CycleType).Readonly().ShowInList(width: (charWidth * 4));
				View.Property(p => p.ProjectCycle).UseSpinEditor(p =>
				{
					p.AllowDecimals = false;
					p.MinValue = 0;
				}).Readonly().ShowInList(width: (charWidth * 4));
				View.Property(p => p.WarningPeriod).UseSpinEditor(p =>
				{
					p.MinValue = 0;
				}).Readonly().ShowInList(width: (charWidth * 4));

				View.Property(p => p.Standard).Readonly().ShowInList(width: (charWidth * 12));
				View.Property(p => p.Consumable).Readonly().ShowInList(width: (charWidth * 8));
				View.Property(p => p.UseTime).Readonly().ShowInList(width: (charWidth * 5));

				View.Property(p => p.CreateBy).Show(ShowInWhere.Hide);
				View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
				View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
				View.Property(p => p.UpdateBy).Show(ShowInWhere.Hide);
				View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
				View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
				//加载润滑项目备件信息
				View.AttachChildrenProperty(typeof(EquipAccountLubricaSparePart), w =>
				{
					var args = w as ChildPagingDataArgs;
					var parent = args.Parent.CastTo<LubricationDetail>();
					if (parent == null)
					{
						return new EntityList<EquipAccountLubricaSparePart>();
					}
					return RT.Service.Resolve<LubricationController>().GetEquipAccountLubricaSparePart(parent.Id, args.SortInfo, args.PagingInfo);
				}, EquipAccountLubricaSparePartViewConfig.SeeViewGroup).HasLabel("备件清单").HasOrderNo(6);
			}
			
		}
	}
}
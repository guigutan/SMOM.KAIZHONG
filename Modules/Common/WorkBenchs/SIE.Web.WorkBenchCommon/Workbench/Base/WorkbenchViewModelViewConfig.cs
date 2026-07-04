using SIE.MetaModel.View;
using SIE.WorkBenchCommon.Workbench.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.WorkBenchCommon.Workbench.Base
{
    public class WorkbenchViewModelViewConfig : WebViewConfig<WorkbenchViewModel>
    {
        public const string AddDetailsViewGroup= "AddDetailsView";
        public const string DesignDetailsViewGroup = "DesignDetailsView";
        public const string SaveDetailsViewGroup = "SaveDetailsView";
        public const string EditSaveDetailsViewGroup = "EditSaveDetailsView";

        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(AddDetailsViewGroup, SaveDetailsViewGroup);
            if (ViewGroup == AddDetailsViewGroup || ViewGroup== DesignDetailsViewGroup)
            {
                AddDetailsView();
            }
            else if (ViewGroup== SaveDetailsViewGroup)
            {
                SaveDetailsView();
            }
            else if(ViewGroup == EditSaveDetailsViewGroup)
            {
                EditSaveDetailsView();
            }
        }

        protected override void ConfigListView()
        {
            
        }

        protected override void ConfigDetailsView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.Code).Show();
                View.Property(p => p.Name).Show();
                View.Property(p => p.Description).Show();
                View.Property(p => p.LayoutCode).UseSelectionViewMeta(new SelectionViewMeta()
                {
                    SelectionEntityType = typeof(LayoutInfo),
                    DisplayMemberPath = LayoutInfo.CodeProperty,
                    SelectedValuePath = LayoutInfo.CodeProperty
                }).UseDataSource((e, p, k) => {
                    var layoutInfoList = RT.Service.Resolve<WorkbenchController>().GetLayoutList(k,p);
                  
                    return layoutInfoList;

                }).UsePagingLookUpEditor(a => {
                    a.Editable = false;
                    a.AllowBlank = false;
                    a.ReloadDataOnPopping = true;
                    a.ValueField = LayoutInfo.CodeProperty.Name;
                    a.DisplayField = LayoutInfo.CodeProperty.Name;
                }).Show();
            }
        }

        protected  void SaveDetailsView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.Code).Show();
                View.Property(p => p.Name).Show();
                View.Property(p => p.Description).Show();
            }
        }

        protected void EditSaveDetailsView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.Name).Show();
                View.Property(p => p.Description).Show();
            }
        }

        protected void AddDetailsView()
        {
            View.Property(p => p.LayoutCode).UseSelectionViewMeta(new SelectionViewMeta()
            {
                SelectionEntityType = typeof(LayoutInfo),
                DisplayMemberPath = LayoutInfo.CodeProperty,
                SelectedValuePath = LayoutInfo.CodeProperty
            }).UseDataSource((e, p, k) => {
                var layoutInfoList = RT.Service.Resolve<WorkbenchController>().GetLayoutList(k, p);

                return layoutInfoList;

            }).UsePagingLookUpEditor(a => {
                a.Editable = false;
                a.AllowBlank = false;
                a.ReloadDataOnPopping = true;
                a.ValueField = LayoutInfo.CodeProperty.Name;
                a.DisplayField = LayoutInfo.CodeProperty.Name;
            }).Show();
        }
    }
}

using SIE.Defects;
using SIE.Defects.InspectionItems;
using SIE.Defects.Measures;
using SIE.MetaModel;
using SIE.Modules;
using SIE.Web.Defects;
using System;
[assembly: Module(typeof(Module))]

namespace SIE.Web.Defects
{
    class Module : UIModule
    {
        public override void Initialize(IApp app)
        {
            app.ModuleOperations += App_ModuleOperations;
        }
        private void App_ModuleOperations(object sender, EventArgs e)
        {
            CommonModel.Modules.AddModules(
                new WebModuleMeta()
                {
                    EntityType = typeof(Defect),
                    Label = "缺陷代码",
                },
                new WebModuleMeta()
                {
                    EntityType = typeof(DefectResponsibility),
                    Label = "缺陷责任",
                },
                new WebModuleMeta()
                {
                    EntityType = typeof(InspectionMode),
                    Label = "检验方式",
                }, new WebModuleMeta()
                {
                    EntityType = typeof(RepairMeasure),
                    Label = "维修措施",
                },new WebModuleMeta()
                {
                    EntityType = typeof(DefectGrade),
                    Label = "缺陷等级",
                }, new WebModuleMeta()
                {
                    EntityType = typeof(DefectCategory),
                    Label = "缺陷代码分类",
                });
        }
    }
}

using SIE.EMS.Devices.Abnormals;
using SIE.Equipments.EquipModels;
using SIE.MetaModel.View;
using SIE.Web.EMS.Devices.Abnormals.Commands;
using System.Collections.Generic;

namespace SIE.Web.Kit.Fixture.Fixtures.Abnormals
{
    /// <summary>
    /// 设备异常维护视图配置
    /// </summary>
    internal class DeviceAbnormalViewConfig : WebViewConfig<DeviceAbnormal>
    {
        ///<summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.UseDefaultCommands();
        }

        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseCommands(WebCommandNames.Add,
                WebCommandNames.Edit,
                WebCommandNames.Delete,
                WebCommandNames.Copy,
                WebCommandNames.Save,
                typeof(ImportAbnormalCommand).FullName
                );

            //View.ReplaceCommands(WebCommandNames.Add, typeof(AddAbnormalCommand).FullName);
            View.Property(p => p.Code).Readonly(p => p.PersistenceStatus != Domain.PersistenceStatus.New).HasLabel("故障名称");
            View.Property(p => p.AbnormalType).UseEnumEditor();
            View.Property(p => p.Description);
            View.Property(p => p.EquipTypeId).UseDataSource((e, p, k) =>
            {
                return RT.Service.Resolve<EquipModelController>().GetEquipTypes(p, k, true);
            }).UsePagingLookUpEditor((m, e) =>
            {
                Dictionary<string, string> keyValues = new Dictionary<string, string>();
                keyValues.Add(nameof(e.EquipTypeName), nameof(e.EquipType.TypeName));
                m.DicLinkField = keyValues;
            }).HasLabel("设备类型编码");
            View.Property(p => p.EquipTypeName).Readonly();
            View.Property(p => p.CreateByName).Readonly();
            View.Property(p => p.CreateDate).Readonly();
            View.Property(p => p.UpdateByName).Readonly();
            View.Property(p => p.UpdateDate).Readonly();
        }

        /// <summary>
        /// 配置下拉视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.Code).Readonly();
            View.Property(p => p.AbnormalType).UseEnumEditor();
            View.Property(p => p.Description);
            View.Property(p => p.EquipTypeName).HasLabel("设备类型");
        }
    }
}
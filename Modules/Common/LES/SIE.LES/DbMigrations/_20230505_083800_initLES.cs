using SIE.Common.Catalogs;
using SIE.Common.Configs.CommonConfigs;
using SIE.Core.Common;
using SIE.Data.DbMigration;
using SIE.Domain;
using SIE.LES.MaterialMoves;
using SIE.LES.MaterialPreparations;
using SIE.LES.MaterialReturnApplys;
using SIE.LES.StockOrders;
using SIE.LES.StockOrders.Configs;
using System;

namespace SIE.LES.DbMigrations
{
    internal class _20230505_083800_initLES : ManualDbMigration
    {
        public override string DbSetting
        {
            get { return LESEntityDataProvider.ConnectionStringName; }
        }

        public override ManualMigrationType Type
        {
            get { return ManualMigrationType.Data; }
        }

        public override string Description
        {
            get { return "添加生产退料快码".L10N(); }
        }

        protected override void Down()
        {
            
        }

        protected override void Up()
        {
            this.RunCode(db =>
            {
                if (AppRuntime.InvOrg == null)
                {
                    AppRuntime.InvOrg = 1;
                }
                //if (RT.Service.Resolve<CatalogController>().GetCatalogType(MaterialReturn.ReasonMaterialReturn) == null)
                //{
                //    var catalogtype = new CatalogType()
                //    {
                //        Code = MaterialReturn.ReasonMaterialReturn,
                //        Name = "生产退料原因".L10N(),
                //        Description = "生产退料原因".L10N(),
                //    };
                //    RF.Save(catalogtype);
                //}
                LimitedMaximumStockConfigValue limitedMaximumStockConfigValue = new LimitedMaximumStockConfigValue();
                limitedMaximumStockConfigValue.IsLimited = false;
                RT.Service.Resolve<ConfigExtController>()
                .InitModuleConfig < StockOrder,LimitedMaximumStockConfig, LimitedMaximumStockConfigValue > (limitedMaximumStockConfigValue);

                SchedulingTriggeredStatusConfigValue schedulingTriggeredStatusConfigValue = new SchedulingTriggeredStatusConfigValue
                {
                    TriggeredStatus = TriggeredStatus.Created
                };

                RT.Service.Resolve<ConfigExtController>()
                .InitModuleConfig<StockOrder, SchedulingTriggeredStatusConfig, SchedulingTriggeredStatusConfigValue>(schedulingTriggeredStatusConfigValue);


                StockOrderIsAuditConfigValue stockOrderIsAuditConfigValue = new StockOrderIsAuditConfigValue
                {
                    IsAudit = false
                };
                RT.Service.Resolve<ConfigExtController>()
                .InitModuleConfig<StockOrder, StockOrderIsAuditConfig, StockOrderIsAuditConfigValue>(stockOrderIsAuditConfigValue);

                StockReceiveTypeConfigValue stockReceiveTypeConfigValue = new StockReceiveTypeConfigValue
                {
                    ReceiveType = StockReceiveType.Auto
                };
                RT.Service.Resolve<ConfigExtController>()
                .InitModuleConfig<StockOrder, StockReceiveTypeConfig, StockReceiveTypeConfigValue>(stockReceiveTypeConfigValue);


                // 备料需求单初始化
                InitMaterialPreparation();

                // 退料申请初始化
                InitMaterialReturnApply();

                // 初始化挪料记录
                InitMaterialMove();
            });
        }

        /// <summary>
        /// 备料需求单初始化
        /// </summary>
        private void InitMaterialPreparation()
        {
            // 备料需求单号编码规则
            var numberExtCtl = RT.Service.Resolve<NumberRuleExtController>();
            var numberRule = numberExtCtl.CreateFormNoNumberRule("备料需求单号编码规则", "MP",
                SIE.Common.Algorithm.DateFormat.yyMMdd, 4);
            if (numberRule != null)
            {
                NoConfigValue configValue = new NoConfigValue
                {
                    NumberRuleId = numberRule.Id
                };
                RT.Service.Resolve<ConfigExtController>().InitModuleConfig<MaterialPreparation, NoConfig, NoConfigValue>(configValue);
            }

            // 备料需求原因快码
            if (RT.Service.Resolve<CatalogController>().GetCatalogType(MaterialPreparation.MaterialPreReasonStr) == null)
            {
                var catalogtype = new CatalogType()
                {
                    Code = MaterialPreparation.MaterialPreReasonStr,
                    Name = "备料需求单原因".L10N(),
                    Description = "备料需求单原因".L10N(),
                };
                RF.Save(catalogtype);
            }
        }

        /// <summary>
        /// 退料申请初始化
        /// </summary>
        private void InitMaterialReturnApply()
        {
            // 退料申请单号编码规则
            var numberExtCtl = RT.Service.Resolve<NumberRuleExtController>();
            var numberRule = numberExtCtl.CreateFormNoNumberRule("退料申请单号编码规则", "MR",
                SIE.Common.Algorithm.DateFormat.yyMMdd, 4);
            if (numberRule != null)
            {
                NoConfigValue configValue = new NoConfigValue
                {
                    NumberRuleId = numberRule.Id
                };
                RT.Service.Resolve<ConfigExtController>().InitModuleConfig<MaterialReturnApply, NoConfig, NoConfigValue>(configValue);
            }

            // 退料原因快码
            if (RT.Service.Resolve<CatalogController>().GetCatalogType(MaterialReturnApply.MaterialReturnReasonStr) == null)
            {
                var catalogtype = new CatalogType()
                {
                    Code = MaterialReturnApply.MaterialReturnReasonStr,
                    Name = "退料申请单原因".L10N(),
                    Description = "退料申请单原因".L10N(),
                };
                RF.Save(catalogtype);
            }
        }

        /// <summary>
        /// 初始化挪料记录
        /// </summary>
        private void InitMaterialMove()
        {
            if (RT.Service.Resolve<CatalogController>().GetCatalogType(MaterialMoveRecord.MaterialMoveReasonStr) == null)
            {
                var catalogtype = new CatalogType()
                {
                    Code = MaterialMoveRecord.MaterialMoveReasonStr,
                    Name = "挪料记录原因".L10N(),
                    Description = "挪料记录挪料原因".L10N(),
                };
                RF.Save(catalogtype);
            }
        }
    }
}

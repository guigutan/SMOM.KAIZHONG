Ext.define('SIE.Web.Equipments.Abnormal.Behaviors.AbnormalAlertSysConfigBehavior',
    {
        /**
          * view生命周期函数--view生成前
          * @param {*} meta 实体视图元数据
          * @param {*} curEntity 当前操作实体(可空)
          */
        beforeCreate: function (meta, curEntity) {
            var lineNameConfig = meta.formConfig.items.first(function (p) { return p.name == "LineName" }); //产线名称配置
            var checkConfig = meta.formConfig.items.first(function (p) { return p.name == "IsAutoRestore" });

            if (checkConfig && lineNameConfig && lineNameConfig.labelWidth) {
                checkConfig.padding = "0 0 0 " + (lineNameConfig.labelWidth - 8);
            }
        },
    });

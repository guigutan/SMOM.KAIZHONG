Ext.define('SIE.Web.EMS.EquipRepair.EquipRepairs.Behaviors.EquipRepairWorkingHoursBehavior',
    {
        /**
         * view生命周期函数--view生成前
         * @param {*} meta 实体视图元数据
         * @param {*} curEntity 当前操作实体(可空)
         */
        beforeCreate: function (meta, curEntity) {

        },
        /**
        * view生命周期函数--view生成后
        * @param {*} view 生成的view
        */
        onCreated: function (view) {

        },
        onViewReady: function (view) {

        },
        /**
         * view生命周期函数--数据加载后
         * @param {any} view 逻辑视图
         */
        onDataLoaded: function (view) {
            var me = this;
            //view.getData().queryRecords().forEach(function (p) {
            //    view.mon(p, "propertyChanged", me.PropertyChanged, me);
            //});
        },
        PropertyChanged: function (arg) {
            var me = this;
            //if (arg.property == 'IsRepairMaster') {
            //    if (arg.value) {
            //        arg.entity.store.queryRecords().forEach(function (p) {
            //            if (p.data.Id != arg.entity.data.Id) {
            //                p.setIsRepairMaster(false);
            //            }
            //            else
            //                p.setIsRepairEmployee(true);
            //        });
            //    }
            //}
        }

    });

Ext.define('SIE.Web.EMS.MeteringEquipment.Calibrations.Behaviors.AddCalibrationBehavior',
    {
        SourceEquipId: null,
        /**
        * view生命周期函数--view聚合后
        * @param {*} view 生成的view
        */
        onViewReady: function (view) {
            var me = this;
            //设置主表model
            var entity = view.getCurrent();
            var params = CRT.Context.PageContext.getParams();
            if (params) {
                entity.data.TreePId = params.TreePId;
                me.SourceEquipId = params.SourceEquipId;
            }
            SIE.invokeDataQuery({
                type: "SIE.Web.EMS.MeteringEquipment.Calibrations.DataQuery.CalibrationDataQuery",
                method: "GetCalibrationNo",
                params: [],
                async: false,
                token: view.token,
                callback: function (res) {
                    entity.setInspectionNo(res.Result);
                },
            });

            if (entity)
                view.mon(entity, 'propertyChanged', this._onEntityPropertyChanged, this);
        },
        _onEntityPropertyChanged: function (e) {
            var entity = e.entity;
            if (e.property.length > 0 && e.property === "InspectionRuleId") {
                var child = e.entity.belongsView.findChild("SIE.EMS.MeteringEquipment.Calibrations.CalibrationItem");
                
                child.getData().removeAll();
            }

        }
    });

Ext.define('SIE.Web.EMS.SpecialEquipment.RegularInspections.Behaviors.RegularInspectionClearBehavior',
    {
        SourceEquipId: null,
        /**
        * view生命周期函数--view聚合后
        * @param {*} view 生成的view
        */
        onViewReady: function (view) {       
            //设置主表model
            var entity = view.getCurrent();   
            if (entity)
                view.mon(entity, 'propertyChanged', this._onEntityPropertyChanged, this);
        },
        _onEntityPropertyChanged: function (e) {
            var entity = e.entity;
            if (e.property.length > 0 && e.property === "InspectionRuleId") {              
                var child = e.entity.belongsView.findChild("SIE.EMS.SpecialEquipment.RegularInspections.RegularInspectionDetail");
                child.getData().removeAll();
            }

        }
    });
Ext.define('SIE.Web.EMS.InspectionRules.Behaviors.InspectionRuleBehavior',
    {
        /**
        * view生命周期函数--view聚合后
        * @param {*} view 生成的view
        */
        onViewReady: function (view) {
            //code here
            var entity = view.getData();
            //var childView = child_view;
            if (entity)
                view.mon(entity, 'propertyChanged', this._onEntityPropertyChanged, this);
        },

        _onEntityPropertyChanged: function (e) {
            var entity = e.entity;
            if (e.property.length > 0 && e.property === "InspectionRuleType") {
                var child = e.entity.belongsView.findChild("SIE.EMS.InspectionRules.InspectionProjectItem");
                child.getData().removeAll();
            }

        }
    });
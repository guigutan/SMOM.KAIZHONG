Ext.define('SIE.Web.EMS.Projects.Behaviors.ProjectDetailBehavior',
    {
         /**
         * view生命周期函数--view聚合后
         * @param {*} view 生成的view
         */
        onViewReady: function (view) {
            //code here
            var entity = view.getData();
            if (entity)
                view.mon(entity, 'propertyChanged', this._onEntityPropertyChanged, this);
        },

        _onEntityPropertyChanged: function (e) {
            var data = e.entity;
            if (e.property.length > 0) {
                if (data.getProjectType() == 10) {
                    data.setCycleType(2);
                    data.setCycleTypeInfoId(2);
                    data.setCycleTypeInfoId_Display("周".t());
                }
                if (data.getProjectType() == 6) {
                    data.setCycleType(1);
                    data.setCycleTypeInfoId(1);
                    data.setCycleTypeInfoId_Display("日".t());
                }
            }
        }
    });
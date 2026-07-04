Ext.define('SIE.Web.WorkBenchCommon.Workbench.KPI.Behaviors.QuotaTargetSettingCriteriaBehavior',
    {
        _view: null,
        /**
         * view生命周期函数--view聚合后
         * @param {*} view 生成的view
         */
        onViewReady: function (view) {

            var me = this;
            this._view = view;
            var entity = view.getCurrent();  
            view.mon(entity, 'propertyChanged', this.PropertyChanged, this);
        },

        PropertyChanged: function (e) {
            var entity = e.entity;
            if (e.property.length > 0) {
                if (e.property.indexOf('Code') >= 0) {
                    this._view.getCurrent().setName(null);
                }
            }
        },
    });
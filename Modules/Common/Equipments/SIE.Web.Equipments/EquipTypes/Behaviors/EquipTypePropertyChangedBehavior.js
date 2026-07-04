Ext.define('SIE.Web.Equipments.EquipTypes.Behaviors.EquipTypePropertyChangedBehavior',
    {
        /**
         * view生命周期函数--view聚合后
         * @param {*} view 生成的view
         */
        onViewReady: function (view) {
            var entity = view.getData();
            if (entity)
                view.mon(entity, 'propertyChanged', this._onEntityPropertyChanged, this);
        },

        /**
         * 属性变更处理
         * @param {any} e
         */
        _onEntityPropertyChanged: function (e) {
            var data = e.entity;
            if (e.property.length > 0) {
                if (e.property.indexOf('IsCheck') >= 0) {
                    data.setCheckCategory(null);
                }
            }
        },
    });
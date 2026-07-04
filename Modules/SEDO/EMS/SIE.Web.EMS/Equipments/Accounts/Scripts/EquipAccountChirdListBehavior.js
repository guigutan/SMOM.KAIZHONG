Ext.define('SIE.Web.EMS.Equipments.Accounts.Scripts.EquipAccountChirdListBehavior',
    {
        /**
        * 数据加载完毕的处理事件
        * @param {*} view 生成的view
        */
        onDataLoaded: function (view) {
            var me = this;
            if (view) {
                var childs = view._parent.getChildren();
                childs.forEach(function (item) {
                    var datas = item.getData().getData().items;
                    if (datas!=null&& datas.length > 0) {
                        datas.forEach(function (data) {
                            item.mon(data, 'propertyChanged', me.onEntityPropertyChanged, item);
                        });
                    }

                });
            }
        },
        /**
         * 属性变更事件
         * @param {any} e
         */
        onEntityPropertyChanged: function (e) {
            debugger
            var parentEntity = this._parent.getCurrent()
            if (parentEntity) {
                parentEntity.dirty = true;
            }
        },
    });
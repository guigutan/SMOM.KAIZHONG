Ext.define('SIE.Web.Items.Scripts.ItemUnitBehavior',
    {
        /**
         * view生命周期函数--view生成前
         * @param {*} meta 实体实体元数据
         * @param {*} curEntity 当前操作实体(可空)
         */
        beforeCreate: function (meta, curEntity) {
            //code here
        },

        /**
         * view生命周期函数--view生成后
         * @param {*} view 生成的view
         */
        onCreated: function (view) {
            //code here
        },

        /**
         * view生命周期函数--数据加载后
         * @param {any} view 逻辑视图
         */
        onDataLoaded: function (view) {
            var itenUnitDatas = view.getData().data.items;
            var ids = itenUnitDatas.select(function (p) { return p.data.Id; });
            if (ids.length == 0) return;
            itenUnitDatas.forEach(function (p) {
                if (p.data.UnitSource != 1) {//不是基准单位，增加说明
                    var first = p.getItemUnitName();
                    var sec = p.getUnitName();
                    var changeDesc = "";
                    if (p.getDenominator() != 1)
                        changeDesc = "1" + first + " = " + p.getNumerator() + "/" + p.getDenominator() + sec;
                    else
                        changeDesc = "1" + first + " = " + p.getNumerator() + sec;
                    p.setChangeDesc(changeDesc);
                    p.markSaved();
                }
                view.mon(p, 'propertyChanged', SIE.Web.Items.Scripts.ItemAction.onEntityPropertyChanged, view);
            });
        }
    });
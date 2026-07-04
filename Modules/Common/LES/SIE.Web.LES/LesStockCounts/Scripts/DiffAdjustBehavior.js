Ext.define('SIE.Web.LES.LesStockCounts.Scripts.DiffAdjustBehavior',
    {
        /**
         * view生命周期函数--view生成前
         * @param {*} meta 实体视图元数据
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
          
        },

        onViewReady: function (view) {

        },

        /**
         * view生命周期函数--数据加载后
         * @param {any} view 逻辑视图
         */
        onDataLoaded: function (view) {
            var cur = view.getParent().getCurrent().data;
            SIE.invokeDataQuery({
                async: false,
                type: "SIE.Web.LES.LesStockCounts.DataQueryer.LesStockCountDataQueryer",
                method: 'GetAdjustWorkOrderViewModels',
                token: view.token,
                params: [cur.DtlId],
                callback: function (res) {
                    if (res.Result && res.Result.length > 0) {
                        view.getData().add(res.Result);
                    }
                }
            });
        }
    });

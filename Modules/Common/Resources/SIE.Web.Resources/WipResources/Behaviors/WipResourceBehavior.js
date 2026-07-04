Ext.define('SIE.Web.Resources.WipResources.Behaviors.WipResourceBehavior',
    {
        /**
         * view生命周期函数--数据加载后
         * @param {any} view 逻辑视图
         */
        onDataLoaded: function (view) {
            var entity = view.getData();
            var mainView = view;
            if (entity) {
                entity.on('propertyChanged', function (e) {
                    if (e.property == 'IsOutMade') {
                        var isOutMade = entity.getIsOutMade();
                        if (!isOutMade) {
                            entity.setSupplierId(null);
                            entity.setSupplierId_Display(null);
                        }
                    }
                    //if (e.property = "ProcessTechTypeId") {
                    //    SIE.invokeDataQuery({
                    //        type: "SIE.Web.Resources.WipResources.DataQuery.WipResourcesDataQueryer",
                    //        method: "GetProcessSegmentById",
                    //        params: [entity.data.ProcessTechTypeId],
                    //        async: false,
                    //        token: mainView.token,
                    //        callback: function (res) {
                    //            if (res.Success && res.Result != null) {
                    //                if (res.Result.data.items.length > 0) {
                    //                    entity.setProcessSegmentId(res.Result.data.items[0].data.Id)
                    //                    entity.setProcessSegmentId_Display(res.Result.data.items[0].data.Code)
                    //                } else {
                    //                    entity.setProcessSegmentId(0);
                    //                    entity.setProcessSegmentId_Display("");
                    //                }
                    //            }
                    //        }
                    //    });
                    //}
                });
            }
        }
    });
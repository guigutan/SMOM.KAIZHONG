Ext.define('SIE.Web.Fixtures.Demands.Scripts.AddDemandBehavior',
    {
        /**
        * onViewReady 视图加载完成
        * @param {*} view 当前视图
        */
        onViewReady: function (view) {
            var me = this;
            var isEdit = false
            var params = CRT.Context.PageContext.getParams();
            if (params) {
                view.tabId = params.tabId;
                isEdit = params.isEdit;
            }
            if (!isEdit) {
                me.bindInfos(view);
                var entity = view.getCurrent();
                view.mon(entity, 'propertyChanged', me.onEntityPropertyChanged, view);
            }
        },
        /**
        * bindInfos 绑定信息
        * @param {*} me 当前界面
        */
        bindInfos: function (view) {
            SIE.invokeDataQuery({
                type: "SIE.Web.Fixtures.Demands.DataQuery.FixtureDemandDataQueryer",
                method: "GetDemandInfo",
                params: [],
                async: false,
                token: view.token,
                callback: function (res) {
                    if (res.Success) {
                        if (res.Result.ErrMsg) {
                            SIE.Msg.showWarning(res.Result.ErrMsg);
                        }
                        else {
                            var entity = new view._model(res.Result.Data);
                            view.setData(entity);
                            view.setCurrent(entity);
                        }
                    }
                },
            });
        },
        /**
        * onEntityPropertyChanged 属性变更事件
        * @param {*} e 参数
        */
        onEntityPropertyChanged: function (e) {
            var me = this;
            if (e.property.length > 0) {
                var data = e.entity.data;
                me.data = data;
                if (e.property === 'WorkOrderId') {
                    var childView = me.findChild('SIE.Fixtures.FixtureDemands.FixtureDemandDetail');
                    if (childView) {
                        childView.getData().data.removeAll();
                        SIE.invokeDataQuery({
                            type: "SIE.Web.Fixtures.Demands.DataQuery.FixtureDemandDataQueryer",
                            method: "GetBindWoInfo",
                            params: [e.value],
                            async: false,
                            token: childView.token,
                            callback: function (res) {
                                if (res.Success && res.Result) {
                                    e.entity.setWorkOrderProductCode(res.Result.ProductCode);
                                    e.entity.setProcessSegmentId(res.Result.ProcessSegmentId);
                                    e.entity.setProcessSegmentId_Display(res.Result.ProcessSegment_Display);
                                    e.entity.setProcessSurface(res.Result.Desk);
                                    e.entity.setDemandTime(res.Result.PlanDateTime);
                                }
                            },
                        });


                    }
                }
                //if (e.property == "ProcessSegmentId")
                //    var childView = me.findChild('SIE.Fixtures.FixtureDemands.FixtureDemandDetail');
                //if (childView) {
                //    var parent = childView._parent.getCurrent();
                //    if (data.WorkOrderId != null && data.ProcessSegmentId != null) {
                //        childView.getData().data.removeAll();
                //        SIE.invokeDataQuery({
                //            type: "SIE.Web.Fixtures.Demands.DataQuery.FixtureDemandDataQueryer",
                //            method: "GetFixtureDemandDetailList",
                //            params: [data.WorkOrderId, data.ProcessSegmentId, data.Id],
                //            async: false,
                //            token: childView.token,
                //            callback: function (res) {
                //                if (res.Success) {
                //                    if (res.Result.ErrMsg) {
                //                        SIE.Msg.showWarning(res.Result.ErrMsg);
                //                    }
                //                    else {
                //                        for (var i = 0; i < res.Result.data.items.length; i++) {
                //                            var resdata = res.Result.data.items[i].data;
                //                            var newData = childView.addNew();
                //                            var store = childView.getData();
                //                            newData.data.ItemId = resdata.ItemId;
                //                            newData.data.FixtureModelId_Display = resdata.ModelCode;
                //                            newData.data.FixtureModelId = resdata.FixtureModelId;
                //                            newData.data.ModelName = resdata.ModelName;
                //                            newData.data.ModelCode = resdata.ModelCode;
                //                            newData.data.FixtureEncodeId = resdata.FixtureEncodeId;
                //                            newData.data.FixtureEncodeCode = resdata.FixtureEncodeCode;
                //                            newData.data.FixtureEncodeId_Display = resdata.FixtureEncodeCode;
                //                            newData.data.FixtureTypeId = resdata.FixtureTypeId;
                //                            newData.data.FixtureTypeId_Display = resdata.FixtureTypeCode;
                //                            newData.data.FixtureTypeCode = resdata.FixtureTypeCode;
                //                            newData.data.ProcessSegmentId = resdata.ProcessSegmentId;
                //                            newData.data.ProcessSegmentId_Display = resdata.ProcessSegmentCode;
                //                            newData.data.ProcessSegmentCode = resdata.ProcessSegmentCode;
                //                            newData.data.FixtureDemandId = resdata.FixtureDemandId;
                //                            newData.data.ProcessSurface = resdata.ProcessSurface;
                //                            newData.data.DemandQty = resdata.DemandQty;
                //                            store.add(newData);
                //                            parent[childView._childProperty]().getData().add(newData);
                //                        }

                //                    }
                //                }
                //            },
                //        });
                //    }

                //}
            }
        },
    });
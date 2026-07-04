Ext.define('SIE.Web.EMS.SpecialEquipment.RegularInspections.RegularInspectionDetailBehavior',
    {
        /**
         * @override 加载数据
         * @param {any} view
         */
        onDataLoaded: function (view) {
            var detailView = view;
            if (!detailView) return;
            var parentData = detailView.getParent().getCurrent().getData();
            var isInit = Ext.isEmpty(parentData.VoAllQuantitativeValues);    //是否首次加载
            detailView.getData().getData().items.forEach(function (detail) {
                if (detail.getId() == null || detail.getId() == 0) {
                    //未保存的数据
                    detail.generateId();
                    detail.phantom = true;
                    detail.dirty = true;
                }
                if (isInit) {
                    //首次加载，初始化数据
                    for (var i = 0; i < parentData.VoInitialSamplingQty; i++) {
                        var newValue = new SIE.EMS.SpecialEquipment.RegularInspections.RegularInspectionValue();
                        newValue.setIndex(i + 1);
                        newValue.setRegularInspectionDetailId(detail.getId());
                        parentData.VoAllQuantitativeValues.push(newValue.data);
                    }

                    detailView.setDefaultValue(detail);
                }
            });
            var ctl = view.getController();
            ctl.onQuantativeStoreLoad(detailView.getData().getData().items, detailView);
        },
    });

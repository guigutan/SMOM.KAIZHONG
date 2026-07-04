Ext.define('SIE.Web.EMS.SpareParts.Behaviors.SparePartBehavior', {
    /*
     * view生命周期函数--view生成前
     * @param {*} meta 实体视图元数据
     * @param {*} curEntity 当前操作实体(可空)
     */
    beforeCreate: function (meta, curEntity) {

        var render = {
            renderer: function (value, cell, record) {
                if (!Ext.isEmpty(record.getSafeStock())) {
                    if (record.getGoodNumber() == null || (record.getGoodNumber() < record.getSafeStock())) {
                        cell.tdStyle = "border-right: 1px solid white; background: orange;";
                    }
                }
                return value;
            }
        };

        meta.gridConfig.columns.forEach(function (e) {
            if (e.dataIndex === 'GoodNumber') {
                Object.assign(e, render);
            }
        });

    },
    /**
    * view生命周期函数--view生成后
    * @param {*} view 生成的view
    */
    onCreated: function (view) {

        SIE.invokeDataQuery({
            method: 'VerifyIsWmsControl',
            params: [],
            async: false,
            action: 'queryer',
            type: 'SIE.Web.EMS.SpareParts.DataQuery.SparePartDataQueryer',
            token: view.token,
            success: function (res) {
                if (res.Success) {
                    CRT.Context.PageContext.setContext("IsWmsControl", res.Result);
                }
            }
        });
    },
    onViewReady: function (view) {
        view.mon(view, 'currentChanged', this.currentChanged, view);
    },
    currentChanged: function (e) {

        var view = this;
        var picture = document.getElementById("sparePartPicture");

        if (picture) {
            if (e.newValue) {
                var pictureChildView = view.findChild('SIE.EMS.SpareParts.SparePartPictureAttachment');
                if (pictureChildView) {
                    var store = pictureChildView.getData();
                    setTimeout(function () {
                        if (store.getCount() > 0) {
                            pictureChildView.getControl().getSelectionModel().select(store.getAt(0), true);
                        }
                        else {
                            picture.src = "";
                        }
                    }, 200);
                }
            }
            else {
                picture.src = "";
            }
        }
    },
});
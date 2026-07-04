Ext.define('SIE.Web.EMS.Tpms.Scripts.AddRecordBehavior',
    {
        /**
        * onViewReady 视图加载完成
        * @param {*} view 当前视图
        */
        onViewReady: function (view) {
            var me = this;
            me.bindInfos(view);
        },
        /**
        * bindInfos 绑定信息
        * @param {*} me 当前界面
        */
        bindInfos: function (view) {
            SIE.invokeDataQuery({
                type: "SIE.Web.EMS.Tpms.DataQuery.TpmRecordDataQueryer",
                method: "GetTpmRecordInfo",
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
                            var childView = view.findChild('SIE.EMS.Tpms.TpmRecordDetail');
                            if (childView) {
                                childView.loadData();
                          }
                        }
                    }
                },
            });
        },
    });
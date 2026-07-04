Ext.define('SIE.Web.Resources.Enterprises.Behaviors.EnterpriseBehavior',
    {
        /**
         * view生命周期函数-view聚合后
         * @param {any} view
         */
        onViewReady: function (view) {
            //设置是否启动异步加载
            view._AsynLoadData = true; 
            this.InitNewTreeStore(view);
        },
        InitNewTreeStore: function (view) {
            var tree = view._control;
            var store = tree.getStore();
            store.addListener('beforeload', function (store, operation, eOpts) {
                if (operation._id != 0) {
                    SIE.invokeDataQuery({
                        method: 'GetNodes',
                        params: [operation._id],
                        action: 'queryer',
                        type: 'SIE.Web.Resources.Enterprises.DataQuery.EnterpriseNodeQuery',
                        token: view.token,
                        success: function (res) {
                            var current = view.getCurrent();
                            if (current != null) {
                                var newNodes = res.Result.getData().items.map(x => x.data);
                                if (newNodes.length > 0) {
                                    operation.node.appendChild(newNodes);
                                } else {
                                    operation.node.set('leaf', true);
                                }
                                var idPath = operation.node.getPath();
                                tree.expandPath(idPath);
                                current.markSaved();
                            }
                        }
                    })
                    return false;
                }
            })
        }
    });




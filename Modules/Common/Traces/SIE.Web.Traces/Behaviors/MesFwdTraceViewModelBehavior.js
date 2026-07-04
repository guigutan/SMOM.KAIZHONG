Ext.define('SIE.Web.Traces.Behaviors.MesFwdTraceViewModelBehavior',
    {     

        /**
        * view生命周期函数--数据加载后
        * @param {any} view 逻辑视图
        */
        onDataLoaded: function (view) {

            var childrens = view.getChildren();
            childrens.forEach(function (c) {
                c._curPid = null;//该视图是树形视图，数据加载之后这个属性置空一下，为了解决再次查询主表时，选中子表一行数据，加载当前孙表数据之后，再切换回之前加载过的其它孙表不加载数据的问题
            });

        }
    });
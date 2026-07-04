Ext.define('SIE.Web.Core.QmsStaticConst.Behaviors.StaticConstTBehavior',
    {
        //当前视图
        _view: null,
        /**
          * view生命周期函数--view生成前
          * @param {*} meta 实体视图元数据
          * @param {*} curEntity 当前操作实体(可空)
          */
        beforeCreate: function (meta, curEntity) {
            var a = 1;
        },
        onViewReady: function (view) {
            var me = this;
            this._view = view;
            ctl = view.getController();
            if (!ctl) {
                ctl = new SIE.Web.Static.StaticConstTController();
                view.setController(ctl);
            }
        },
      

        /**
        * view生命周期函数--数据加载后
        * @param {any} view 逻辑视图
        */
        onDataLoaded: function (view) {
            ctl = view.getController();
            if (ctl)
                ctl.drawGrid(view);
        },
    });

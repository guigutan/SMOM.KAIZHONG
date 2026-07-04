Ext.define('SIE.Web.Core.QmsStaticConst.Behaviors.StaticConstBehavior',
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
            //view.mon(view, "newentityadded", me.onEntityAdded, this); //添加完成事件，在添加命令中触发
            view.mon(view, "selectionChanged", me.onCustomSelectionChanged, this);//选中行事件
        },


        /**
        * view生命周期函数--数据加载后
        * @param {any} view 逻辑视图
        */
        onDataLoaded: function (view) {
            var a = 1;
        },

        /** */
        onEntityAdded: function (newEntity) {
            this._view.mon(newEntity, "propertyChanged", this.onPropertyChanged, this);
        },
        onCustomSelectionChanged: function (e) {
            var childrenViews = e.newValue[0].belongsView.getChildren();
            childrenViews.forEach(function (childrenView, index) {
                if (childrenView.model === "SIE.Core.QmsStaticConst.StaticConstT") {
                    var ctl = childrenView.getController();
                    if (ctl)
                        ctl.drawGrid(childrenView);
                }
                if (childrenView.model === "SIE.Core.QmsStaticConst.StaticConstD2") {
                    var ctl = childrenView.getController();
                    if (ctl)
                        ctl.drawGrid(childrenView);
                }
            });

        }
    });

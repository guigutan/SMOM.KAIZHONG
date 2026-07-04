Ext.define('SIE.Web.ERPInterface.Scripts.ErpUploadLogBehavior',
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
            var ctl = view;
            var requestStrInputId = view.getControl().items.items.where(function (a) { return a.name == 'RequestStr' })[0].inputId;
            var responseStrInputId = view.getControl().items.items.where(function (a) { return a.name == 'ResponseStr' })[0].inputId;
            document.getElementById(requestStrInputId).style.height = "300px";
            document.getElementById(responseStrInputId).style.height = "300px";
        }
    });

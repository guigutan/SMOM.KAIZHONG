Ext.define('SIE.Web.Fixtures.Querys.Scripts.FixtureQueryBehavior',
    {
        /**
         * view生命周期函数--view生成前
         * @param {*} meta 实体视图元数据
         * @param {*} curEntity 当前操作实体(可空)
         */
        beforeCreate: function (meta, curEntity) {
            me = this;
            if (!meta)
                return;
            if (meta.model != 'SIE.Fixtures.Querys.ViewModels.FixtureQueryViewModel')
                return;
            meta.gridConfig.dockedItems[0] = null;  //移除分页
        },
    });
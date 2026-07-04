Ext.define('SIE.data.StaticConstSerializer', {
    //是否要递归序列化聚合子集合。
    //默认为: false, 是否需要提取子视图中的数据。
    _withChildren: false,
    _view: null,

    //-------------------------------------  API -------------------------------------
    statics: {
        serialize: function (component, withChildren, view) {
            /// <summary>
            /// 序列化指定的实体对象或数据集。
            /// </summary>
            /// <param name="component">要序列化的实体对象或数据集。</param>
            /// <param name="withChildren">是否要递归序列化聚合子集合。</param>
            /// <returns type="Object">存放数据的对象。</returns>

            var instance = new SIE.data.StaticConstSerializer();
            instance._withChildren = withChildren;
            instance._view = view;

            var changeSet = new SIE.data.ListChangeSet();

            if (component.isModel) {
                changeSet._data = instance._serializeEntity(component);
                changeSet._model = Ext.getClass(component);
            }
            else {
                changeSet._data = instance._serializeStore(component);
                changeSet._model = component.model;
            }

            return changeSet;
        }
    },

    _serializeEntity: function (entity) {
        /// <summary>
        /// 获取某个实体中需要提交到服务器上的数据。
        /// </summary>
        /// <param name="entity" type="SIE.data.Entity">要序列化的实体对象。</param>
        /// <returns type="Object">返回存放实体数据的纯 json 对象。</returns>

        var me = this;
        var crudState = SIE.data.crudState;

        //注意，单个实体的数据，依然是以 EntityList 的方式提交。
        //这样不但统一了数据的格式，而且还简单用实体列表的集合来分辨当前实体的状态（IsNew、IsDeleted）。
        //添加属性 _isEntityHost 用于分辨二者。
        var dto = { _isEntityHost: 1 };

        if (entity.isNew()) {
            dto.c = [entity];
            me._getPersistArray(dto, crudState.C);
        }
        else if (entity.isSelfDirty()) {
            dto.u = [entity];
            me._getPersistArray(dto, crudState.U);
        }
        else if (entity.isDirty()) {
            dto.uc = [entity];
            me._getPersistArray(dto, crudState.UC);
        }

        return dto;
    },
    _serializeStore: function (store) {
        /// <summary>
        /// 序列化指定的数据集。
        /// </summary>
        /// <param name="store">要序列化的数据集。</param>
        /// <returns type="Object">存放数据的对象。</returns>

        var dto = null;
        if (store instanceof Ext.data.TreeStore) {
            dto = this._serializeStore_TreeStore(store);
        }
        else {
            dto = this._serializeStore_Store(store);
        }

        return dto;
    },
    _serializeChildrenRecur: function (entity, dto) {
        /// <summary>
        /// 把某个实体 entity 中已经加载的所有关联子实体都序列化到 dto 中。
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="dto"></param>
        var me = this;
        var childrenList = entity.getEntityChildren();
        childrenList.eachKey(function (key, item, index, len) {
            if (item) {
                if (item instanceof Ext.data.Model) {
                    if (Ext.getClassName(entity) === Ext.getClassName(item)) {
                        //附加子实体与父实体相同时,则处理变更过的属性值
                        var changes = item.getChanges();
                        if (SIE.hasAnyProperty(changes)) {
                            for (var prop in changes) {
                                if (changes.hasOwnProperty(prop)) {
                                    dto[prop] = changes[prop];
                                }
                            }
                        }
                    } else {
                        //不同时则作为父实体的一个关联属性
                        var listData = [];
                        listData.push(item.data);
                        if (item.getEntityChildren()) {
                            var serializeObj = me._serializeStore(item.store);
                            dto[item.associateView.getAssociateProperty()] = serializeObj;
                        }
                        else {
                            if (item.phantom == true || item.data.Id <= 0) {
                                dto[item.associateView.getAssociateProperty()] = { c: listData };
                            } else {
                                dto[item.associateView.getAssociateProperty()] = { u: listData };
                            }
                        }
                    }
                }
                else if (item instanceof Ext.data.Store || item.$className == "Ext.data.Store") {
                    if ((item.getCount() + item.getTotalCount() + item.getRemovedRecords().length) > 0) {
                        var serializeObj;
                        if (item.model.$className === 'SIE.Core.QmsStaticConst.StaticConstT') {
                            serializeObj = me._serializeStore_Store_ConstT(item);
                        }
                        else if (item.model.$className === 'SIE.Core.QmsStaticConst.StaticConstD2') {
                            serializeObj = me._serializeStore_Store_ConstD2(item);
                        }
                        else {
                            serializeObj = me._serializeStore(item);
                        }
                        //方案1：子列表不传空数据到后台，让后台使用自身懒加载数据通过实体验证
                        if (!SIE.isEmptyObject(serializeObj)) {
                            var prop = item.associateView ? item.associateView.getAssociateProperty() : key;
                            //补充场景，同一个实体属性，拆分为多个页面去承载显示，提交时需要合并数据json，如检验单据明细属性(拆分为定性，定量显示）
                            dto[prop] = me._getPersionComplexData(dto[prop], serializeObj);
                        }
                    }
                }
            }
        });
    },

    //-------------------------------------  Common -------------------------------------
    /**
     * 获取某个实体序列化后的数据
     * @param {Object} entity-实体对象
     * @param {Boolean} deleted-是否已经被删除标识
     * @returns {Object} 普通对象
     */
    _getPersistData: function (entity, deleted) {
        var dto = {};

        //如果该对象是被删除了，则只需要传输 Id 即可。
        if (deleted) {
            dto[SIE._KeyPropertyName] = entity.getId();
            var title;
            if (this._view)
                title = this._view.title;//支持view主属性传输,为信息提示
            if (title)
                dto[title] = entity.get(title);
        }
        else {
            //属性需要可以保存，并且是大写
            var fields = SIE.data.Utils.getPersistFields(entity);
            SIE.each(fields, function (f) { dto[f.name] = entity.get(f.name); });

            if (this._withChildren) {
                this._serializeChildrenRecur(entity, dto);
            }
        }

        return dto;
    },
    /**
     * 把 dto 中指定名称 property 的一个实体集合转换为数据的集合。
     * @param {type} dto-数据传输对象
     * @param {type} property-属性名称(c,r,u,d)
     */
    _getPersistArray: function (dto, property) {
        var me = this;

        var raw = dto[property];
        if (!raw || raw.length == 0) {
            delete dto[property];
            return;
        }

        var deleted = property == 'd';

        var list = [];
        SIE.each(raw, function (item) {
            list.push(me._getPersistData(item, deleted));
        });
        dto[property] = list;
    },
    /**
     * 获取持久化的复合数据结构
     * @param {Object} source--源对象结构
     * @param {Object} dest--目标对象结构
     * @returns {Object} 合并后的对象结构
     */
    _getPersionComplexData: function (source, dest) {
        var crudState = SIE.data.crudState;
        if (SIE.isEmpty(source)) {
            source = dest;
        }
        else {
            this._getPersionComplexArray(source, dest, crudState.C);
            this._getPersionComplexArray(source, dest, crudState.U);
            this._getPersionComplexArray(source, dest, crudState.D);
            this._getPersionComplexArray(source, dest, crudState.UC);
        }

        return source;
    },
    /**
     * 合并具体的属性
     * @param {Object} source--源对象结构
     * @param {Object} dest--目标对象结构
     * @returns {Array} 合并后的对象数组结构
     */
    _getPersionComplexArray: function (source, dest, property) {
        if (source[property] && dest[property]) {
            var arr = Ext.Array.union(source[property], dest[property]);
            source[property] = arr;
        }
        else if (!source[property] && dest[property]) {
            source[property] = dest[property];
        }
    },
    //-------------------------------------  Store -------------------------------------
    _serializeStore_Store: function (store) {
        var crudState = SIE.data.crudState;
        var data = {
            c: store.getNewRecords(),
            u: store.getUpdatedRecords(),
            d: Ext.Array.filter(store.getRemovedRecords(), function (i) { return !i.isNew(); }),
            //本身未改变，组合子发生改变的实体，放到 uc 集合中提交。
            uc: store.data.filterBy(
                function (item) {
                    if (item.isModel && item.$className)
                        return !item.isSelfDirty() && item.isEntityChildrenDirty();
                    else
                        return false;
                }).items
        };

        this._getPersistArray(data, crudState.C);//toCreate
        this._getPersistArray(data, crudState.U);//toUpdate
        this._getPersistArray(data, crudState.D);//toDelete
        this._getPersistArray(data, crudState.UC);//unchanged

        return data;
    },
    _serializeStore_Store_ConstT: function (store) {
        var newRecords = [];
        var updateRecords = [];
        var delRecords = [];
        for (var i = 0; i < store.data.items.length; i++) {
            var oldRecord = store.data.items[i];

            for (var j = 0; j < store.changeColumns.length; j++) {
                var column = store.changeColumns[j];
                if (column.dataIndex.indexOf('Alpha_') >= 0) {
                    var record = {};
                    record["SampleQty"] = oldRecord.data.SampleQty;
                    record["Alpha"] = Ext.Number.from(column.header, -1);
                    record["Value"] = oldRecord.data[column.dataIndex];

                    if (oldRecord.isAdd) {
                        //如果这一行是新的，代表这一行都是新建
                        newRecords.push(record);
                    }
                    else {
                        //如果此行不是新增的，则要判断此列是否是新增的，
                        if (column.isAdd) {
                            newRecords.push(record);
                        }
                        else {
                            updateRecords.push(record);
                        }

                    }

                }
            }
        }
        var crudState = SIE.data.crudState;
        var data = {
            c: newRecords,
            u: updateRecords,
            d: delRecords,
            uc: []
        };

        return data;
    },
    _serializeStore_Store_ConstD2: function (store) {
        var newRecords = [];
        var updateRecords = [];
        var delRecords = [];
        for (var i = 0; i < store.data.items.length; i++) {
            var oldRecord = store.data.items[i];

            for (var j = 0; j < store.changeColumns.length; j++) {
                var column = store.changeColumns[j];
                if (column.dataIndex.indexOf('TestQty_') >= 0) {
                    var record = {};

                    if (oldRecord.data.MsaConstD2Type == SIE.Core.QmsStaticConst.StaticConstD2Type.d2) {
                        record["SampleQty"] = 1;
                        record["MsaConstD2Type"] = SIE.Core.QmsStaticConst.StaticConstD2Type.d2;
                        record["TestQty"] = Ext.Number.from(column.header, -1);

                    }
                    else if (oldRecord.data.MsaConstD2Type == SIE.Core.QmsStaticConst.StaticConstD2Type.cd) {
                        record["SampleQty"] = 1;
                        record["MsaConstD2Type"] = SIE.Core.QmsStaticConst.StaticConstD2Type.cd;
                        record["TestQty"] = Ext.Number.from(column.header, -1);
                    }
                    else if (oldRecord.data.MsaConstD2Type == SIE.Core.QmsStaticConst.StaticConstD2Type.V) {
                        record["SampleQty"] = oldRecord.data.SampleQty;
                        record["MsaConstD2Type"] = SIE.Core.QmsStaticConst.StaticConstD2Type.V;
                        record["TestQty"] = Ext.Number.from(column.header, -1);
                    }
                    else if (oldRecord.data.MsaConstD2Type == SIE.Core.QmsStaticConst.StaticConstD2Type.D2s) {
                        record["SampleQty"] = oldRecord.data.SampleQty;
                        record["MsaConstD2Type"] = SIE.Core.QmsStaticConst.StaticConstD2Type.D2s;
                        record["TestQty"] = Ext.Number.from(column.header, -1);
                    }
                    record["Value"] = oldRecord.data[column.dataIndex];

                    if (oldRecord.isAdd) {
                        //如果这一行是新的，代表这一行都是新建
                        newRecords.push(record);
                    }
                    else {
                        //如果此行不是新增的，则要判断此列是否是新增的，
                        if (column.isAdd) {
                            newRecords.push(record);
                        }
                        else {
                            updateRecords.push(record);
                        }

                    }
                }

            }
        }
        var data = {
            c: newRecords,
            u: updateRecords,
            d: delRecords,
            uc: []
        };

        return data;
    },

    //-------------------------------------  TreeStore -------------------------------------
    _serializeStore_TreeStore: function (treeStore) {
        var crudState = SIE.data.crudState;
        var data = {
            c: treeStore.getNewRecords(),
            u: treeStore.getUpdatedRecords(),
            d: Ext.Array.filter(treeStore.getRemovedRecords(), function (i) { return !i.isNew(); })
        };
        this._getPersistArray(data, crudState.C);//toCreate
        this._getPersistArray(data, crudState.U);//toUpdate
        this._getPersistArray(data, crudState.D);//toDelete
        return data;
    },
    _convertNodeRecur: function (node) {
        var item = this._getPersistData(node);

        if (node.isNew()) {
            item.isNew = 1;
        }

        if (!node.isLeaf()) {
            item.TreeChildren = [];
            for (var i = 0; i < node.childNodes.length; i++) {
                var child = this._convertNodeRecur(node.childNodes[i]);
                item.TreeChildren.push(child);
            }
        }

        return item;
    }
});
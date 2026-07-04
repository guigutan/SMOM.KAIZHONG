using DevExpress.Xpf.Grid;
using SIE.Domain;
using SIE.Wpf.Tech.Processs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Wpf.Tech.Helpers
{
    /// <summary>
    /// 树帮助类
    /// </summary>
    public class TreeHelper
    {
        /// <summary>
        /// 获取Key
        /// </summary>
        /// <param name="entity">树信息</param>
        /// <returns>键</returns>
        string GetKey(TreeInfo entity)
        {
            return "{0}_{1}".FormatArgs(entity.Tag.GetType().ToString(), (entity.Tag as DataEntity)?.Id);
        }

        /// <summary>
        /// 展开节点
        /// </summary>
        /// <param name="entity">树信息</param>
        public void ExpandNode(TreeInfo entity)
        {
            string key = GetKey(entity);
            if (_treeListNodes.ContainsKey(key))
                _treeListNodes[key].IsExpanded = true;
        }

        /// <summary>
        /// 选中节点
        /// </summary>
        /// <param name="entity">树信息</param>
        public void SelectedTreeViewItem(TreeInfo entity)
        {
            string key = GetKey(entity);
            if (_treeListNodes.ContainsKey(key))
                _treeListNodes[key].IsChecked = true;
        }

        /// <summary>
        /// 根据之前保存的 TreeViewItem.IsExpanded 设置 TreeViewItem.IsExpanded 是否展开
        /// </summary>
        /// <param name="entity">树信息</param>
        /// <param name="treeViewItem">树节点</param>
        public void SetIsExpanded(TreeInfo entity, TreeListNode treeViewItem)
        {
            string key = GetKey(entity);
            var treeViewItemOld = GetTreeListNode(key);
            if (treeViewItemOld != null)
                treeViewItem.IsExpanded = treeViewItemOld.IsExpanded;
            AddTreeListNode(key, treeViewItem);
        }

        /// <summary>
        /// 树节点字典
        /// </summary>
        readonly Dictionary<string, TreeListNode> _treeListNodes = new Dictionary<string, TreeListNode>();

        /// <summary>
        /// 添加树节点
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="node">树节点</param>
        void AddTreeListNode(string key, TreeListNode node)
        {
            if (_treeListNodes.ContainsKey(key))
            {
                _treeListNodes[key] = node;
            }
            else
            {
                _treeListNodes.Add(key, node);
            }
        }

        /// <summary>
        /// 获取树节点
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>树节点</returns>
        TreeListNode GetTreeListNode(string key)
        {
            if (_treeListNodes.ContainsKey(key))
            {
                return _treeListNodes[key];
            }

            return null;
        }

        /// <summary>
        /// 刷新时展开已展开节点
        /// </summary>
        /// <param name="gridControl">树列表控件</param>
        /// <param name="treeInfoList">全部树节点信息</param>
        /// <param name="keyValuePairs">已打开节点集合（key:Type + "_" + EntityId; value: Type, EntityId）</param>
        /// <param name="currentInfo">当前选择节点</param>
        public void ExpandingNodes(GridControl gridControl, IList<TreeInfo> treeInfoList, Dictionary<string, Tuple<string, double>> keyValuePairs, TreeInfo currentInfo)
        {
            var treeListView = (gridControl.View as TreeListView);

            keyValuePairs.ForEach(p =>
            {
                var node = treeListView.GetNodeByContent(treeInfoList.FirstOrDefault(q => q.Type == p.Value.Item1 && q.EntityId == p.Value.Item2));
                if (node != null) node.IsExpanded = true;
            });

            var info = treeInfoList.FirstOrDefault(q => q.Type == currentInfo?.Type && q.EntityId == currentInfo?.EntityId);

            var currentNode = treeListView.GetNodeByContent(info);
            while (currentNode?.ParentNode != null)
            {
                currentNode = currentNode.ParentNode;
                currentNode.IsExpanded = true;
            }

            gridControl.CurrentItem = info;
            gridControl.SelectedItem = info;
        }
    }
}

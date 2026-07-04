using DocumentFormat.OpenXml.Wordprocessing;
using Irony;
using Microsoft.Scripting.Utils;
using SIE.MES.Traces.Models;
using SIE.Tech.Routings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.Traces
{
    /// <summary>
    /// 树结构处理类
    /// </summary>
    public class TreeProcessor
    {

        private int currentId = 1;

        /// <summary>
        /// 拆分并反转树
        /// </summary>
        /// <param name="treeList"></param>
        /// <returns></returns>
        public List<RecursiveProductInfo> FlattenAndReverseTrees(List<RecursiveProductInfo> treeList)
        {
            var flatttenTree = FlattenTree(treeList);
            var result = ReverseTree(flatttenTree);
            return result;
        }

        private List<RecursiveProductInfo> ReverseTree(List<RecursiveProductInfo> dataList)
        {
            var rootNodes = dataList.FindAll(c => c.TreeId.IsNullOrEmpty());
            foreach (var rootNode in rootNodes)
            {
                RecursiveReverse(rootNode, dataList);
            }
            return dataList;
        }

        private void RecursiveReverse(RecursiveProductInfo node, List<RecursiveProductInfo> dataList)
        {
            var children = dataList.Find(c => c.TreeId == node.Id);
            if (children != null)
            {           
                children.TreeId = string.Empty;
                RecursiveReverse(children, dataList);
                node.TreeId = children.Id;
            }
        }


        private List<RecursiveProductInfo> FlattenTree(List<RecursiveProductInfo> treeList)
        {
            var result = new List<RecursiveProductInfo>();
            AssignNewIds(treeList);
            var leafNodes = GetLeafNodes(treeList);

            foreach (var leafNode in leafNodes)
            {
                RecursiveMakeNewNode(leafNode, result, treeList);
            }

            return result;
        }

        private void AssignNewIds(List<RecursiveProductInfo> treeList)
        {
            foreach (var tree in treeList)
            {
                var id = tree.Id; ;
                tree.Id = "New" + (currentId++);
                var childrenList = treeList.FindAll(x => x.TreeId == id);
                childrenList.ForEach(x => x.TreeId = tree.Id);
            }
        }

        private List<RecursiveProductInfo> GetLeafNodes(List<RecursiveProductInfo> treeList)
        {
            var result = new List<RecursiveProductInfo>();

            foreach (var tree in treeList)
                if (!treeList.Any(t => t.TreeId == tree.Id))
                    result.Add(tree);

            return result;

        }

        private RecursiveProductInfo MakeNewNode(RecursiveProductInfo node)
        {
            currentId++;
            var result = new RecursiveProductInfo
            {
                Id = currentId.ToString(),
                TreeId = node.TreeId,
                SnId = node.SnId,
                ProductId = node.ProductId,
                ProductSn = node.ProductSn,
                ProductExtPropName = node.ProductExtPropName,
                WorkOrderId = node.WorkOrderId,
                CreateDate = node.CreateDate,
                ItemExtPropName = node.ItemExtPropName,
                ItemId = node.ItemId,
                ItemSourceCode = node.ItemSourceCode,
            };
            return result;
        }

        private void RecursiveMakeNewNode(RecursiveProductInfo node, List<RecursiveProductInfo> newTreeList, List<RecursiveProductInfo> treeList)
        {
            var newNode = MakeNewNode(node);
            newTreeList.FindAll(c => c.TreeId == node.Id).ForEach(c => c.TreeId = newNode.Id);
            newTreeList.Add(newNode);

            if (newNode.TreeId != null)
            {
                var parent = treeList.Find(c => c.Id == newNode.TreeId);
                if (parent != null)
                    RecursiveMakeNewNode(parent, newTreeList, treeList);
            }
        }
    }
}

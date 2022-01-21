using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace NPC
{
    /// <summary>
    /// 重载节点的行为树
    /// </summary>
    [CreateAssetMenu(fileName = "行为树控制器Override", menuName = "角色/行为树控制器Override")]
    public class BehaviorTreeOverride : SerializedScriptableObject, IRunable
    {

        /// <summary>
        /// 原型行为树
        /// </summary>
        [OdinSerialize]
        public BehaviorTree Prototype
        {
            get => _prototype;
            set => _prototype = value;
        }
        private BehaviorTree _prototype;

        [ListDrawerSettings(IsReadOnly =true, HideAddButton = true, HideRemoveButton = true), ReadOnly]
        public SortedList<string, Node> Nodes;

        public NodeStatus Tick(BehaviorTreeRunner runner)
        {
            return _prototype.Tick(runner);
        }

        public IRunable Clone()
        {
            var tree = CreateInstance<BehaviorTreeOverride>();
            tree._prototype = Prototype.Clone() as BehaviorTree;
            foreach(var name in Nodes.Keys)
            {
                var node = tree._prototype.Nodes.Find(node => node.Name == (name + "(Clone)"));
                if(node is RootNode) { continue; }

                var replace = Nodes[name].Clone(true);
                if(replace is CompositeNode)
                {
                    (replace as CompositeNode).Childrens = new List<Node>((node as CompositeNode).Childrens);
                }
                if(replace is DecoratorNode)
                {
                    (replace as DecoratorNode).Child = (node as DecoratorNode).Child;
                }

                var parent = tree._prototype.FindParent(node, tree._prototype.RootNode as Node);
                if(parent is CompositeNode)
                {
                    var c = parent as CompositeNode;
                    int n = c.Childrens.FindIndex(n => n == node);
                    c.Childrens[n] = replace;
                }
                else if (parent is DecoratorNode)
                {
                    var c = parent as DecoratorNode;
                    c.Child = replace;
                }

                tree._prototype.Nodes.Remove(node);
                tree._prototype.Nodes.Add(replace);
            }

            return tree;
        }

#if UNITY_EDITOR
        [Button]
        public void RefreshList()
        {
            if(Nodes == null)
            {
                Nodes = new SortedList<string, Node>();
            }
            List<string> toRemove = new List<string>();
            List<string> toAdd = new List<string>();

            if(_prototype == null)
            {
                foreach(var node in Nodes)
                {
                    AssetDatabase.RemoveObjectFromAsset(node.Value);
                    AssetDatabase.SaveAssets();
                }
                Nodes = null;
                return;
            }

            foreach(var name in Nodes.Keys)
            {
                if(_prototype.Nodes.Find(node=>node.Name == name) == null)
                {
                    toRemove.Add(name);
                }
            }
            foreach(var node in _prototype.Nodes)
            {
                if (!Nodes.ContainsKey(node.name))
                {
                    toAdd.Add(node.name);
                }
            }

            toRemove.ForEach(name => 
            {
                AssetDatabase.RemoveObjectFromAsset(Nodes[name]);
                Nodes.Remove(name);
                AssetDatabase.SaveAssets();
            });
            toAdd.ForEach(name =>
            {
                Node node = _prototype.Nodes.Find(node => node.Name == name).Clone(true);
                Nodes.Add(name, node);
                AssetDatabase.AddObjectToAsset(node, this);
                AssetDatabase.SaveAssets();
            });
        }
#endif
    }
}
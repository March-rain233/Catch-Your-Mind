using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogue
{

    public class InstantiateNode : ActionNode
    {
        [System.Serializable]
        public struct ObjectInfo
        {
            public string Name;
            public GameObject Prefabs;
            public string Parent;
            public Vector3 Position;
        }

        public List<ObjectInfo> Objects;

        protected override NodeStatus OnUpdate(DialogueTree tree)
        {
            Objects.ForEach(obj =>
            {
                Transform parent = null;
                if (!string.IsNullOrEmpty(obj.Parent))
                {
                    if (tree.Variables.ContainsKey(obj.Parent))
                    {
                        var temp = tree.Variables[obj.Parent].Object;
                        if (temp is GameObject)
                        {
                            parent = (temp as GameObject).transform;
                        }
                        if (temp is Component)
                        {
                            parent = (temp as Component).transform;
                        }
                    }
                    else
                    {
                        parent = GameObject.Find(obj.Parent).transform;
                    }
                }
                var o = Instantiate(obj.Prefabs, parent);
                o.transform.position = obj.Position;
                if (tree.Variables.ContainsKey(obj.Name))
                {
                    var t = tree.Variables[obj.Name];
                    t.Object = o;
                    tree.Variables[obj.Name] = t;
                }
                else
                {
                    tree.Variables.Add(obj.Name, new EventCenter.EventArgs() { Object = o });
                }
            });
            return NodeStatus.Success;
        }
    }
}

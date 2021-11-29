using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogue 
{

    public class ChangeSceneNode : ActionNode
    {
        public string Name;

        protected override NodeStatus OnUpdate(DialogueTree tree)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(Name);
            return NodeStatus.Success;
        }
    }
}

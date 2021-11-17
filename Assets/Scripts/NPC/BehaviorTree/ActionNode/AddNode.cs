using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace NPC
{
    /// <summary>
    /// ÀÛ¼Ó½Úµã
    /// </summary>
    public class AddNode : ActionNode
    {
        [System.Serializable]
        private enum Type
        {
            Int,
            Float
        }

        [SerializeField]
        private Type _type;

        [SerializeField]
        private string _valueName;

        [SerializeField, ShowIf("_type", Type.Int)]
        private int _oriInt;

        [SerializeField, ShowIf("_type", Type.Float)]
        private float _oriFloat;

        [SerializeField, ShowIf("_type", Type.Int)]
        private int _stepInt;

        [SerializeField, ShowIf("_type", Type.Float)]
        private float _stepFloat;

        [SerializeField, ShowIf("_type == Type.Int && !_noneMax")]
        private int _maxInt;

        [SerializeField, ShowIf("_type == Type.Int && !_noneMin")]
        private int _minInt;

        [SerializeField, ShowIf("_type == Type.Float && !_noneMax")]
        private float _maxFloat;

        [SerializeField, ShowIf("_type == Type.Float && !_noneMin")]
        private float _minFloat;

        private bool _noneMax = true;

        private bool _noneMin = true;

        protected override void OnAbort(BehaviorTreeRunner runner)
        {
            
        }

        protected override void OnEnter(BehaviorTreeRunner runner)
        {
            
        }

        protected override void OnExit(BehaviorTreeRunner runner)
        {
            
        }

        protected override void OnResume(BehaviorTreeRunner runner)
        {
            
        }

        protected override NodeStatus OnUpdate(BehaviorTreeRunner runner)
        {
            switch (_type)
            {
                case Type.Int:
                    var temp = runner.Variables[_valueName];
                    if (!_noneMax && temp.Int + _stepInt > _maxInt)
                    {
                        temp.Int = _maxInt;
                    }
                    else if (!_noneMin && temp.Int + _stepInt < _maxInt)
                    {
                        temp.Int = _minInt;
                    }
                    else
                    {
                        temp.Int += _stepInt;
                    }
                    runner.Variables[_valueName] = temp;
                    break;
                case Type.Float:
                    var tempFloat = runner.Variables[_valueName];
                    if (!_noneMax && tempFloat.Float + _stepFloat > _maxFloat)
                    {
                        tempFloat.Float = _maxFloat;
                    }
                    else if (!_noneMin && tempFloat.Float + _stepFloat < _maxFloat)
                    {
                        tempFloat.Float = _minFloat;
                    }
                    else
                    {
                        tempFloat.Float += _stepFloat;
                    }
                    runner.Variables[_valueName] = tempFloat;
                    break;
            }
            return NodeStatus.Success;
        }
    }
}

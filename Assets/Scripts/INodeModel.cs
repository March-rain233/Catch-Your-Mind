using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface INodeModel
{
    string Guid { get; set; }
    string Name { get; }
    Vector2 ViewPosition { get; set; }
}

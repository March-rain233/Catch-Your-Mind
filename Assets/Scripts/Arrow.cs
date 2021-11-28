using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public Transform Target;
    private void Update()
    {
        if(Target == null) { return; }
        Vector2 dir = Target.position - transform.position;
        transform.eulerAngles = new Vector3(0,0, Vector2.SignedAngle(Vector2.right, dir));
    }
}

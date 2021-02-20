using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static partial class VectorExtensions
{
    public static Vector2 AsVector2(this Vector3 vector3)
    {
        return new Vector3(vector3.x, vector3.y);
    }

    public static Vector3 AsVector3(this Vector2 vector2)
    {
        return new Vector3(vector2.x, vector2.y, 0f);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CBVector2
{
    public float x;
    public float y;

    public CBVector2(float x, float y)
    {
        this.x = x;
        this.y = y;
    }

    public CBVector2(Vector2 vec)
    {
        this.x = vec.x;
        this.y = vec.y;
    }
}

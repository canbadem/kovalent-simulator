using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuAtom : MonoBehaviour
{

    public Rigidbody2D r;

    void Start()
    {
        r = this.GetComponent<Rigidbody2D>();
    }

    void OnMouseDrag()
    {
        Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
        Vector3 objPosition = Camera.main.ScreenToWorldPoint(mousePosition);

        objPosition.z = 0;

        Vector2 objPos = new Vector2(objPosition.x, objPosition.y);

        float speed = 10;

        Vector2 velocity = (objPos  - r.position) * speed;

        r.velocity = velocity;
        //r.MovePosition(objPosition);
    }


}

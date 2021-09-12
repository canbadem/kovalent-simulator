using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtomSpawner : MonoBehaviour
{

    public Manager manager;
    public bool spawn = true;
    public int protonNumber = 1;

    void Start()
    {
        manager = GameObject.FindGameObjectWithTag("GameController").GetComponent<Manager>();
    }

    void Update()
    {
        Vector3 pos = Camera.main.ScreenToWorldPoint(this.transform.position);

        Collider2D[] arr = Physics2D.OverlapCircleAll(pos, 0.2f);

        if (arr.Length < 1)
        {       
            manager.spawnAtom(Atom.GetAtomType(protonNumber), new Vector3(pos.x, pos.y, pos.z + 5), Quaternion.identity);
        }

    }
}

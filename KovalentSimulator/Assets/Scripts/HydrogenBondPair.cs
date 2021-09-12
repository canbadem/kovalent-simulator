using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HydrogenBondPair
{

    public Atom a1;
    public Atom a2;

    public GameObject line;

    public HydrogenBondPair(Atom e1, Atom e2, GameObject line)
    {
        this.a1 = e1;
        this.a2 = e2;
        this.line = line;


    }

    public void destroy()
    {
        GameObject.Destroy(line);

    }

}

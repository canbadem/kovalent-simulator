using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BoundElectronPair
{

    public Electron e1;
    public Electron e2;

    public GameObject line;

    public bool polar = true;

    public BoundElectronPair(Electron e1, Electron e2, GameObject line)
    {
        this.e1 = e1;
        this.e2 = e2;
        this.line = line;

        this.e1.boundTo = e2;
        this.e2.boundTo = e1;

        this.e1.setType(Electron.ElectronType.BOUND);
        this.e2.setType(Electron.ElectronType.BOUND);

        if (e1.atom.type.Equals(e2.atom.type))
            polar = false;

        if (!polar)
            line.GetComponent<LineRenderer>().material.color = Color.red;


    }

    public void destroy()
    {
        GameObject.Destroy(line);


        e1.setType(Electron.ElectronType.NOT_BOUND);
        e2.setType(Electron.ElectronType.NOT_BOUND);

        this.e1.boundTo = null;
        this.e2.boundTo = null;

    }

}

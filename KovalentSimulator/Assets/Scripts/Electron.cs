using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Electron : MonoBehaviour {

    public ElectronType electronType;
    public Electron boundTo;
    public Atom atom;

    public Transform connectionTransform;

    public enum ElectronType
    {
        NOT_BOUND,
        BOUND
    }

    public Transform getConnectionTransform()
    {
        return connectionTransform;
    }

    public void setType(ElectronType type)
    {
        this.electronType = type;

        switch (electronType)
        {

            case ElectronType.BOUND:
                gameObject.GetComponent<SpriteRenderer>().enabled = false;
                break;
            case ElectronType.NOT_BOUND:
                gameObject.GetComponent<SpriteRenderer>().enabled = true;
                break;
            default:
                gameObject.GetComponent<SpriteRenderer>().enabled = true;
                break;
        }

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Electron e = other.GetComponent<Electron>();

        if (e == null)
            return;

        atom.triggerEnter(this, e);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        Electron e = other.GetComponent<Electron>();

        if (e == null)
            return;

        atom.triggerExit(this, e);
    }

}

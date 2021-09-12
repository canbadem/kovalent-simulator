using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.EventSystems;

[System.Serializable]
public class Atom : MonoBehaviour
{

    public struct AtomInfo
    {
        public string atomName;
        public string atomSymbol;
        public int protonNumber;
        public int defaultElectronNumber;
        public double atomicMassUnit;
        public double electroNegativity;

        public AtomInfo(string atomName, string atomSymbol, int protonNumber, int defaultElectronNumber, double atomicMassUnit, double electroNegativity)
        {
            this.atomName = atomName;
            this.atomSymbol = atomSymbol;
            this.protonNumber = protonNumber;
            this.defaultElectronNumber = defaultElectronNumber;
            this.atomicMassUnit = atomicMassUnit;
            this.electroNegativity = electroNegativity;
        }

    }

    public enum AtomType
    {
        Hydrogen,
        Boron,
        Carbon,
        Nitrogen,
        Oxygen,
        Fluorine,
        Phosphorus,
        Sulfur,
        Chlorine,
        Selenium,
        Bromine,
        Iodine,
        Astatine
    }

    public bool born;
    public bool tripleMode;

    public Canvas panelCanvas;

    public GameObject subAtom;

    public GameObject unbindablePairNorth;
    public GameObject unbindablePairEast;
    public GameObject unbindablePairSouth;
    public GameObject unbindablePairWest;

    public Electron electronNorth;
    public Electron electronEast;
    public Electron electronSouth;
    public Electron electronWest;
    public Electron electronWestTriple;
    public Electron electronEastTriple;

    public Manager manager;

    public TMP_Text titleText;

    public SpriteRenderer atomRenderer;

    [Header("Atom Info")]
    public AtomType type;
    public int electronCount;


    public void selectedMark(bool selected)
    {
        if (!this.born)
            bornAtom();

        Color nColor = new Color32(140,200,240,255);
        Color sColor = new Color32(255,180,0,255);
        
        if (selected)
        {
            atomRenderer.color = sColor;
        }
        else
        {
            atomRenderer.color = nColor;
        }
    }

    public void triggerExit(Electron electron, Electron other)
    {
        manager.electronExit(electron, other);
    }

    public void triggerEnter(Electron electron, Electron other)
    {
        manager.electronEnter(electron, other);
    }

    public static AtomType GetAtomType(int protonNumber)
    {
        foreach(AtomType at in Enum.GetValues(typeof(AtomType)))
        {
            if (GetInfo(at).protonNumber.Equals(protonNumber))
            {
                return at;
            }
        }
        return AtomType.Hydrogen;
    }

    public static AtomInfo GetInfo(AtomType type)
    {
        switch (type)
        { // Atom adı, Atom Sembolü, Proton Numarası, Elektron Numarası, Atomik Kütle Birimi, Elektronegatiflik
            case AtomType.Hydrogen:
                return new AtomInfo("Hidrojen", "H", 1, 1, 1.0079f, 2.2f);

            case AtomType.Boron:
                return new AtomInfo("Bor", "B", 5, 5, 10.811f, 2.04f);

            case AtomType.Carbon:
                return new AtomInfo("Karbon", "C", 6, 6, 12.0107f, 2.55f);

            case AtomType.Nitrogen:
                return new AtomInfo("Azot", "N", 7, 7, 14.0067f, 3.04f);

            case AtomType.Oxygen:
                return new AtomInfo("Oksijen", "O", 8, 8, 15.9994f, 3.44f);

            case AtomType.Fluorine:
                return new AtomInfo("Flor", "F", 9, 9, 18.9984f, 3.98f);

            case AtomType.Phosphorus:
                return new AtomInfo("Fosfor", "P", 15, 15, 30.9738f, 2.19f);

            case AtomType.Sulfur:
                return new AtomInfo("Sulfur", "S", 16, 16, 32.065f, 2.58f);

            case AtomType.Chlorine:
                return new AtomInfo("Klor", "Cl", 17, 17, 35.453f, 3.16f);

            case AtomType.Selenium:
                return new AtomInfo("Selenium", "Se", 34, 34, 1, 0);

            case AtomType.Bromine:
                return new AtomInfo("Bromine", "Br", 35, 35, 1, 0);

            case AtomType.Iodine:
                return new AtomInfo("Iodine", "I", 53, 53, 1, 0);

            case AtomType.Astatine:
                return new AtomInfo("Astatine", "At", 85, 85, 1, 0);

            default:
                return new AtomInfo("Can Badem", "Can Badem", 1, 1, 1, 0);
        }

    }

    public AtomInfo getInfo()
    {
        return GetInfo(type);
    }

    public List<Electron> getAvailableElectrons()
    {

        List<Electron> electrons = new List<Electron>();

        if (electronNorth.gameObject.activeSelf)
        {
            electrons.Add(electronNorth);
        }

        if (electronEast.gameObject.activeSelf)
        {
            electrons.Add(electronEast);
        }

        if (electronSouth.gameObject.activeSelf)
        {
            electrons.Add(electronSouth);
        }

        if (electronWest.gameObject.activeSelf)
        {
            electrons.Add(electronWest);

        }

        if (electronEastTriple.gameObject.activeSelf)
        {
            electrons.Add(electronEastTriple);
        }

        if (electronWestTriple.gameObject.activeSelf)
        {
            electrons.Add(electronWestTriple);
        }

        return electrons;   

    }

    public void setupElectrons()
    {
        int e = getLastOrbitalElectronCount(this.electronCount);

        electronNorth.gameObject.SetActive(false);
        electronEast.gameObject.SetActive(false);
        electronSouth.gameObject.SetActive(false);
        electronWest.gameObject.SetActive(false);

        unbindablePairNorth.SetActive(false);
        unbindablePairEast.SetActive(false);
        unbindablePairSouth.SetActive(false);
        unbindablePairWest.SetActive(false);

        electronWestTriple.gameObject.SetActive(false);
        electronEastTriple.gameObject.SetActive(false);

        switch (e)
        {
            case 1:
                electronNorth.gameObject.SetActive(true);
                break;

            case 2:
                electronNorth.gameObject.SetActive(true);
                electronEast.gameObject.SetActive(true);
                break;

            case 3:
                electronNorth.gameObject.SetActive(true);
                electronEast.gameObject.SetActive(true);
                electronSouth.gameObject.SetActive(true);
                break;

            case 4:
                electronNorth.gameObject.SetActive(true);
                electronEast.gameObject.SetActive(true);
                electronSouth.gameObject.SetActive(true);
                electronWest.gameObject.SetActive(true);
                break;

            case 5:
                unbindablePairNorth.SetActive(true);
                electronEast.gameObject.SetActive(true);
                electronSouth.gameObject.SetActive(true);
                electronWest.gameObject.SetActive(true);
                break;

            case 6:
                unbindablePairNorth.SetActive(true);
                unbindablePairEast.SetActive(true);
                electronSouth.gameObject.SetActive(true);
                electronWest.gameObject.SetActive(true);
                break;

            case 7:
                unbindablePairNorth.SetActive(true);
                unbindablePairEast.SetActive(true);
                unbindablePairSouth.SetActive(true);
                electronWest.gameObject.SetActive(true);
                break;

            case 8:
                unbindablePairNorth.SetActive(true);
                unbindablePairEast.SetActive(true);
                unbindablePairSouth.SetActive(true);
                unbindablePairWest.SetActive(true);
                break;

            default:
                break;
        }



    }

    public void setTripleMode(bool state)
    {

        if (state && !tripleMode)
        {
            if (electronWest.gameObject.activeSelf && electronEast.gameObject.activeSelf)
            {

                if (electronWest.boundTo != null)
                {
                    this.manager.bondManager.unbindElectrons(electronWest, electronWest.boundTo);
                }

                if (electronEast.boundTo != null)
                {
                    this.manager.bondManager.unbindElectrons(electronEast, electronEast.boundTo);
                }


                electronWestTriple.gameObject.SetActive(true);
                electronEastTriple.gameObject.SetActive(true);

                electronWest.gameObject.SetActive(false);
                electronEast.gameObject.SetActive(false);

            }
        }

        if (!state && tripleMode)
        {

            if (electronWestTriple.gameObject.activeSelf && electronEastTriple.gameObject.activeSelf)
            {

                if (electronWestTriple.boundTo != null)
                {
                    this.manager.bondManager.unbindElectrons(electronWestTriple, electronWestTriple.boundTo);
                }

                if (electronEastTriple.boundTo != null)
                {
                    this.manager.bondManager.unbindElectrons(electronEastTriple, electronEastTriple.boundTo);
                }


                electronWestTriple.gameObject.SetActive(false);
                electronEastTriple.gameObject.SetActive(false);

                electronWest.gameObject.SetActive(true);
                electronEast.gameObject.SetActive(true);

            }

        }

        this.tripleMode = state;

    
    }


    public int getDefaultValenceElectrons()
    {
        return getLastOrbitalElectronCount(this.getInfo().defaultElectronNumber);
    }


    public int getValenceElectrons()
    {
        return getLastOrbitalElectronCount(this.electronCount);
    }

   
    public void setElectronCount(int electronNumber)
    {
        int defaultElectron = getInfo().defaultElectronNumber;

        this.electronCount = electronNumber;

    }

    public int getFormalCharge()
    {
        int sharedElectrons = 0;
        int nonBoundElectrons = 0;
        int valence = getDefaultValenceElectrons();

        foreach (Electron e in this.getAvailableElectrons())
        {

            if(e.boundTo != null && e.electronType == Electron.ElectronType.BOUND)
            {
                sharedElectrons++;
            }
            else
            {
                nonBoundElectrons++;
            }
        }

        nonBoundElectrons += (this.getValenceElectrons() - sharedElectrons);


        return valence - nonBoundElectrons - sharedElectrons;

    }

    public bool areElectronsSatisfied()
    {

        int e = this.getValenceElectrons();

        foreach (Electron el in this.getAvailableElectrons())
        {
            if (el.boundTo != null && el.electronType == Electron.ElectronType.BOUND)
            {
                e++;
            }
        }

        if(this.getInfo().protonNumber == 5) {

            return e == 6;
            
        }


        if (this.electronCount > 2)
        {
            //Check octet

            return e == 8;

        }else
        {
            //Check duplet

            return e == 2;
        }

    }

    public string getLastOrbitName()
    {

       if(this.electronCount <= 2)
       {
           return "Dublet";
       }
       else
       {
           return "Oktet";
       }

    }

    public void bornAtom()
    {
        setElectronCount(getInfo().defaultElectronNumber);

        atomRenderer.gameObject.GetComponent<Animation>().Play();
        setupElectrons();

        this.born = true;

    }

    public void removeAtom()
    {
        unbindFromAllAtoms();
        Destroy(gameObject);
    }

    public void unbindFromAllAtoms()
    {
        foreach(Electron e in this.getAvailableElectrons())
        {
            if(e != null)
            {
                if(e.boundTo != null && e.electronType == Electron.ElectronType.BOUND)
                {
                    manager.bondManager.unbindElectrons(e, e.boundTo);
                }
            }
        }
    }

    void Start () {

        manager = GameObject.FindGameObjectWithTag("GameController").GetComponent<Manager>();

        setElectronCount(getInfo().defaultElectronNumber);

        titleText.text = getInfo().atomSymbol;
        this.gameObject.name = getInfo().atomName;        
    }

    public void buttonRotateClockwise()
    {
        Vector3 q = subAtom.gameObject.transform.eulerAngles;
        this.subAtom.gameObject.transform.eulerAngles = new Vector3(0, 0, q.z - 45);
    }

    public void buttonRotateCounterClockwise()
    {
        Vector3 q = subAtom.gameObject.transform.eulerAngles;
        this.subAtom.gameObject.transform.eulerAngles = new Vector3(0, 0, q.z + 45);
    }

    public void buttonTripleMode()
    {
        this.setTripleMode(!tripleMode);
    }

    private void OnMouseDown()
    {
        manager.mouseDown(this);
    }

    private void OnMouseUp()
    {
        manager.mouseUp();
    }

    public static int getLastOrbitalElectronCount(int e) //ElectronNumber
    {
        if (e <= 2)
            return e;
        else
        {
            int b = (e - 2) / 8;
            int f = e - b * 8 - 2;

             
            if (f == 0)
                return 8;
            else
                return f;

        }

    }

    public static int getLastOrbitalBindableElectronCount(int electronNumber)
    {
        int l = getLastOrbitalElectronCount(electronNumber);

        if (l < 4)
            return l;
        else
            return Math.Abs(l - 8);
    }

}

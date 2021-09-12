using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class Molecule {

    public List<Atom> atoms = new List<Atom>();
    public List<BoundElectronPair> boundElectronPairs = new List<BoundElectronPair>();

    public Atom getWaterOxygen()
    {
      /*  if (!isWater())
            return null;*/

        foreach(Atom a in atoms)
        {
            if (a.type.Equals(Atom.AtomType.Oxygen) || a.type.Equals(Atom.AtomType.Fluorine))
            {
                return a;
            }
        }
        return null;
    }

    public Atom getNearestWaterHydrogen(GameObject g)
    {
      /*  if (!isWater())
            return null;*/

        Vector3 gg = g.transform.position;

        Atom b = null;
        float bDist = 0;

        foreach (Atom a in atoms)
        {
            if (a.type.Equals(Atom.AtomType.Hydrogen))
            {
                if (b != null)
                {
                    float dist = Vector3.Distance(gg, a.transform.position);

                    if (dist < bDist)
                        return a;
                    else
                        return b;

                }
                else
                {
                    b = a;
                    bDist = Vector3.Distance(gg, a.transform.position);
                }
            }
        }
        return null;

    }

    public bool isWater()
    {
        if (atoms.Count != 3)
            return false;

        int hydrogen = 0;
        int oxygen = 0;

        foreach(Atom a in atoms)
        {
            if (a.type.Equals(Atom.AtomType.Hydrogen))
            {
                hydrogen++;
            }

            if (a.type.Equals(Atom.AtomType.Oxygen))
            {
                oxygen++;
            }
        }

        if (hydrogen == 2 && oxygen == 1)
            return true;
        else
            return false;
    }

    public bool isHydrocarbon()
    {
        bool carbon = false;
        bool hydrogen = false;

        foreach(Atom a in atoms)
        {
            if (a.type.Equals(Atom.AtomType.Hydrogen))
                hydrogen = true;

            if (a.type.Equals(Atom.AtomType.Carbon))
                carbon = true;

            if (!a.type.Equals(Atom.AtomType.Carbon) && !a.type.Equals(Atom.AtomType.Hydrogen))
                return false;
        }

        if (carbon && hydrogen)
            return true;
        else
            return false;
    }

    public bool isPolar()
    {
        bool polar = true;

        if (this.isHydrocarbon())
            return false;
            
        bool foundPolar = false;
        foreach(BoundElectronPair bep in boundElectronPairs)
        {
            if (bep.polar)
            {
                foundPolar = true;
                break;
            }
        }

        if (!foundPolar)
        {
            return false;
        }



        return polar;
            

    }

    public bool isStable()
    {
        foreach(Atom a in this.atoms)
        {

            if (!a.areElectronsSatisfied())
                return false;
        }

        return true;
    }

    public int getCharge()
    {
        int c = 0;

        foreach (Atom a in this.atoms)
        {
            c += a.getFormalCharge();
        }

        return c;
    }

    public void removeAllElectronPairs()
    {
        List<BoundElectronPair> beps = new List<BoundElectronPair>(boundElectronPairs);
        foreach(BoundElectronPair bep in beps)
        {
            bep.destroy();
            boundElectronPairs.Remove(bep);
        }
    }

    public BoundElectronPair getElectronPair(Electron e)
    {
        foreach(BoundElectronPair bep in boundElectronPairs){
            if(bep.e1.Equals(e) || bep.e2.Equals(e))
            {
                return bep;
            }
        }

        return null;
    }

    public void removeElectronPair(Electron e, bool destroy)
    {
        BoundElectronPair pairToRemove = null;

        foreach(BoundElectronPair bep in boundElectronPairs)
        {
            if(bep.e1.Equals(e) || bep.e2.Equals(e))
            {
                pairToRemove = bep;
                break;
            }

        }

        if (pairToRemove != null)
        {
            if (destroy)
            {
                pairToRemove.destroy();
            }
            boundElectronPairs.Remove(pairToRemove);
        }
    }

    public void addAtom(Atom a)
    {
        if (atoms.Contains(a))
            return;

        atoms.Add(a);
    }

    public void removeAtom(Atom a)
    {
        if (!atoms.Contains(a))
            return;

        atoms.Remove(a);
    }

    public bool contains(Atom a)
    {
        return atoms.Contains(a);
    }

    public double getMolarMass()
    {

        double mass = 0;

        foreach(Atom a in atoms)
        {
            mass += a.getInfo().atomicMassUnit;
        }

        return mass;
    }
    

    public string getName()
    {

        string name = "";
        string formula = getFormula();

        Dictionary<Atom.AtomType, List<Atom>> dic = new Dictionary<Atom.AtomType, List<Atom>>();

        foreach (Atom a in atoms)
        {
            if (!dic.ContainsKey(a.type))
            {
                dic.Add(a.type, new List<Atom>() { a });
            }
            else
            {
                List<Atom> l = dic[a.type];

                l.Add(a);

                dic[a.type] = l;

            }
        }

        List<Atom.AtomType> list = dic.Keys.ToList();

        if(list.Count > 2)
        {
            return formula;
        }

        list.Sort(SortMolecules);
        int index = 0;
        foreach (Atom.AtomType key in list)
        {
            List<Atom> value = dic[key];

            if (value.Count.Equals(1) && index == 0)
                name += (Atom.GetInfo(key).atomName + " ");
            else if(index == 0)
                name += CBTextUtils.fixNamingErrors(CBTextUtils.getLatinNumberNames(value.Count)+Atom.GetInfo(key).atomName + " ");
            else if(index > 0)
                name += CBTextUtils.fixNamingErrors(CBTextUtils.getLatinNumberNames(value.Count) + CBTextUtils.getAnyonNames(key) + " ");

            index++;
        }

        return name + " ("+formula+")";

    }

    public string getFormula()
    {
        string name = "";

        Dictionary<Atom.AtomType, List<Atom>> dic = new Dictionary<Atom.AtomType, List<Atom>>();

        foreach(Atom a in atoms)
        {
            if (!dic.ContainsKey(a.type))
            {
                dic.Add(a.type, new List<Atom>() { a });
            }
            else
            {
                List<Atom> l = dic[a.type];

                l.Add(a);

                dic[a.type] = l;

            }
        }

        List<Atom.AtomType> list = dic.Keys.ToList();

        list.Sort(SortMolecules);

        foreach(Atom.AtomType key in list)
        {
            List<Atom> value = dic[key];

            if (value.Count.Equals(1))
                name += (Atom.GetInfo(key).atomSymbol);
            else
                name += (Atom.GetInfo(key).atomSymbol + CBTextUtils.getSubscript(value.Count.ToString()));

        }

        return name + CBTextUtils.getChargeString(this.getCharge());
    }

    private int SortMolecules(Atom.AtomType x, Atom.AtomType y)
    {
        

        Atom.AtomInfo ix = Atom.GetInfo(x); //4
        Atom.AtomInfo iy = Atom.GetInfo(y); //1

        if (ix.atomSymbol.Equals("C"))
            return -1;

        int toReturn = ix.electroNegativity.CompareTo(iy.electroNegativity);
    //    Debug.Log(ix.atomSymbol + "("+ix.electroNegativity+") ," + iy.atomSymbol + "("+iy.electroNegativity+") | " + toReturn);
        return toReturn;
    }
}

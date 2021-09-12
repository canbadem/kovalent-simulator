using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BondManager : MonoBehaviour
{

    public List<Molecule> molecules = new List<Molecule>();

    public Manager manager;
    public GameObject linePrefab;

    void Update()
    {

        foreach (Molecule m in molecules)
        {


            foreach (BoundElectronPair bep in m.boundElectronPairs)
            {
                if (bep.line != null)
                {
                    LineRenderer lr = bep.line.GetComponent<LineRenderer>();

                    lr.SetPosition(0, bep.e1.getConnectionTransform().position);
                    lr.SetPosition(1, bep.e2.getConnectionTransform().position);

                }
            }
        }
    }

    public List<Atom> getAtomColony(Atom a, List<Atom> colony)
    {
        
        if(colony == null)
        {
            colony = new List<Atom>
            {
                a
            };
        }

        if(!colony.Contains(a))
            colony.Add(a);

        List<Atom> boundAtoms = getBoundTo(a);

        List<Atom> toReturn = new List<Atom>();

        foreach(Atom bound in boundAtoms)
        {
            if (!colony.Contains(bound)){
                toReturn.AddRange(getAtomColony(bound,colony));
            }
        }

        if (!toReturn.Contains(a))
            toReturn.Add(a);

        return toReturn;

    }

    public List<Atom> getBoundTo(Atom a)
    {
        List<Atom> boundElectrons = new List<Atom>();

        foreach(Electron e in a.getAvailableElectrons())
        {
             if(e.electronType.Equals(Electron.ElectronType.BOUND) && e.boundTo != null)
            {
                boundElectrons.Add(e.boundTo.atom);
            }
        }

        return boundElectrons;
    }

    public void removeMolecule(Molecule m)
    {
        List<BoundElectronPair> beps = new List<BoundElectronPair>(m.boundElectronPairs);

        foreach (BoundElectronPair bep in beps)
        {
            m.removeElectronPair(bep.e1, true);
        }

        List<Atom> ats = new List<Atom>(m.atoms);

        foreach (Atom aa in ats)
        {
            aa.removeAtom();
        }

        molecules.Remove(m);

    }

    public void bindElectrons(Electron electron, Electron other)
    {
        if(electron.electronType.Equals(Electron.ElectronType.BOUND) || other.electronType.Equals(Electron.ElectronType.BOUND))
        {
            return;
        }

        Atom atom = electron.atom;
        Atom otherAtom = other.atom;

        Molecule molecule = null;

        if(getAtomMolecule(atom) == null && getAtomMolecule(otherAtom) == null)
        {
            molecule = new Molecule();
            molecule.addAtom(atom);
            molecule.addAtom(otherAtom);

        }else if(getAtomMolecule(atom) == null)
        {
            molecule = getAtomMolecule(otherAtom);
            molecule.addAtom(atom);

        }else if(getAtomMolecule(otherAtom) == null)
        {
            molecule = getAtomMolecule(atom);

            molecule.addAtom(otherAtom);
        }else
        {

           // bool seperateMolecules = false;

            

            molecule = getAtomMolecule(atom);

            Molecule otherMolecule = getAtomMolecule(otherAtom);

            this.molecules.Remove(otherMolecule);

            foreach (Atom a in otherMolecule.atoms)
            {
                molecule.addAtom(a);
            }

            if (!molecule.Equals(otherMolecule))
            {

                foreach (BoundElectronPair b in otherMolecule.boundElectronPairs)
                {
                    molecule.boundElectronPairs.Add(b);
                }
            }
            else
            {
                

                foreach (BoundElectronPair b in otherMolecule.boundElectronPairs)
                {
                    Atom a1 = b.e1.atom;
                    Atom a2 = b.e2.atom;

                    if (otherMolecule.contains(a1) || otherMolecule.contains(a2))
                    {
                        if (!otherMolecule.boundElectronPairs.Contains(b))
                        {
                            molecule.boundElectronPairs.Add(b);
                        }
                    }

                }
            }



                // calculateMolecules();

            }

        if (!molecules.Contains(molecule))
            molecules.Add(molecule);

        if (manager.selectingType.Equals(Manager.SelectingType.MOLECULE))
        {
            manager.mouseUp();
            manager.deselectMolecule();
            manager.selectMolecule(molecule);
        }


        if(molecule.getElectronPair(electron) == null)
        {
            GameObject go = Instantiate(linePrefab, Vector3.zero, Quaternion.identity);

            go.GetComponent<LineRenderer>().SetPosition(0, electron.getConnectionTransform().position);
            go.GetComponent<LineRenderer>().SetPosition(1, other.getConnectionTransform().position);

            BoundElectronPair bep = new BoundElectronPair(electron, other, go);
            molecule.boundElectronPairs.Add(bep);

   /*         foreach (Atom mm in getConnectedAtomTree(atom, null))
            {
                Debug.Log(mm);
            }
            */

        }

        manager.audioManager.pop();

        manager.updateSelectedText();
        manager.updateMoleculeList();
    }

    public void unbindElectrons(Electron electron, Electron other)
    {
        

        if (electron.electronType.Equals(Electron.ElectronType.NOT_BOUND) || other.electronType.Equals(Electron.ElectronType.NOT_BOUND))
        {
            return;
        }

        Atom atom = electron.atom;
        Atom otherAtom = other.atom;

        Molecule m = getAtomMolecule(atom);

        if (m == null)
        {

            electron.boundTo = null;
            other.boundTo = null;

            electron.setType(Electron.ElectronType.NOT_BOUND);
            other.setType(Electron.ElectronType.NOT_BOUND);

            return;
        }
       

        BoundElectronPair bep = m.getElectronPair(electron);

        if (bep == null)
            return;

        bep.destroy();

        m.boundElectronPairs.Remove(bep);

        if (areAtomsConnected(atom, otherAtom))
            return;


        List<Atom> atomTree = getConnectedAtomTree(atom, null);
        List<Atom> otherAtomTree = getConnectedAtomTree(otherAtom, null);

        if(atomTree.Count == 0 && otherAtomTree.Count == 0)
        {

            m.removeAllElectronPairs();

            m.removeAtom(atom);
            m.removeAtom(otherAtom);

            this.molecules.Remove(m);

        }else if(atomTree.Count == 0)
        {
            Debug.Log("Removing main atom from the molecule");
            m.removeAtom(atom);

            if (m.atoms.Count == 0)
            {
                this.molecules.Remove(m);
                Debug.Log("Removing molecule");
            }

        }
        else if(otherAtomTree.Count == 0)
        {
            Debug.Log("Removing other atom from the molecule");
            m.removeAtom(otherAtom);

            if (m.atoms.Count == 0)
            {
                this.molecules.Remove(m);
                Debug.Log("Removing molecule");
            }
        }
        else
        {

            if (m.Equals(getAtomMolecule(otherAtom))){

                Molecule nMolecule = new Molecule();
                Molecule nOtherMolecule = new Molecule();

                nMolecule.addAtom(atom);
                nOtherMolecule.addAtom(otherAtom);

               

                foreach(Atom a in atomTree)
                {
                    Debug.Log("Main Atom Tree: " + a.getInfo().atomName);
                    nMolecule.addAtom(a);
                }
                foreach (Atom a in otherAtomTree)
                {
                    Debug.Log("Other Atom Tree: " + a.getInfo().atomName);
                    nOtherMolecule.addAtom(a);
                }
                foreach (Atom a in nMolecule.atoms)
                {
                    foreach (Electron e in a.getAvailableElectrons())
                    {
                        if (e.electronType == Electron.ElectronType.BOUND)
                        {
                            BoundElectronPair b = m.getElectronPair(e);

                            if (!nMolecule.boundElectronPairs.Contains(b) && b.line != null)
                                nMolecule.boundElectronPairs.Add(b);
                        }
                    }
                }

                foreach (Atom a in nOtherMolecule.atoms)
                {
                    foreach (Electron e in a.getAvailableElectrons())
                    {
                        if (e.electronType == Electron.ElectronType.BOUND)
                        {
                            BoundElectronPair b = m.getElectronPair(e);

                            if (!nOtherMolecule.boundElectronPairs.Contains(b) && b.line != null)
                                nOtherMolecule.boundElectronPairs.Add(b);
                        }
                    }
                }

                molecules.Add(nMolecule);
                molecules.Add(nOtherMolecule);
                molecules.Remove(m);
            }
            
            manager.updateMoleculeList();
        }

        manager.audioManager.pop();

        manager.updateSelectedText();
        manager.updateMoleculeList();


    }

    public bool areAtomsConnected(Atom a, Atom b)
    {
        foreach (Electron e in a.getAvailableElectrons())
        {
            if (e.electronType.Equals(Electron.ElectronType.BOUND)) { 
                if (e.boundTo.atom.Equals(b))
                {
                    return true;
                }
            }
        }

        return false;
    }

    public List<Atom> getConnectedAtomTree(Atom a, Atom origin)
    {

        List<Atom> boundAtoms = getBoundAtoms(a);
        
        if (origin != null)
            boundAtoms.Remove(origin);

        List<Atom> toReturn = new List<Atom>();

        foreach(Atom t in boundAtoms)
        {
            List<Atom> otherList = getConnectedAtomTree(t, a);
            toReturn.AddRange(otherList);
        }

        if (origin != null && !toReturn.Contains(a))
            toReturn.Add(a);

        return toReturn;

    }

    public List<Atom> getBoundAtoms(Atom a)
    {
        List<Atom> r = new List<Atom>();

        foreach(Electron e in a.getAvailableElectrons())
        {

            if (e.electronType.Equals(Electron.ElectronType.BOUND))
            {
                if(!r.Contains(e.boundTo.atom))
                r.Add(e.boundTo.atom);
            }

        }

        return r;
    }

    public Molecule getAtomMolecule(Atom a)
    {
        foreach(Molecule m in molecules) {
            if (m.contains(a))
            {
                return m;
            }
        }

        return null;

    }

}

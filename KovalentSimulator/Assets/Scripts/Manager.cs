using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour {

    public IEnumerator coroutineSelectCopiedMolecule(Atom a)
    {
        yield return new WaitForSeconds(0.2f);

        this.selectMolecule(this.bondManager.getAtomMolecule(a));
       
    }

    void Update () {


        if (Input.GetKeyDown(KeyCode.Delete))
        {
            if (selectingAtom != null)
            {
                selectingAtom.removeAtom();
                selectingAtom = null;
            }
            else if (draggingObjectMolecule != null)
            {
                Atom a = draggingObjectMolecule.GetComponentInChildren<Atom>();

                if (a != null)
                {
                    Molecule m = bondManager.getAtomMolecule(a);
                    if (m != null)
                    {

                        bondManager.removeMolecule(m);

                        Destroy(draggingObjectMolecule);
                        draggingObjectMolecule = null;
                        selectingMolecule = null;


                    }
                }
            }
            else if(selectingMolecule != null)
            {

                bondManager.removeMolecule(selectingMolecule);
                selectingMolecule = null;

            }
            updateMoleculeList();
            updateSelectedText();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isMenuOpened)
                closeMenu();
            else
                openMenu();
        }

        if (Input.GetKeyDown(KeyCode.W) && selectingAtom != null)
        {
            Vector3 q = selectingAtom.subAtom.gameObject.transform.eulerAngles;

            selectingAtom.subAtom.gameObject.transform.eulerAngles = new Vector3(0, 0, q.z - 180);
        }
        else if (Input.GetKeyDown(KeyCode.S) && selectingAtom != null)
        {
            Vector3 q = selectingAtom.subAtom.gameObject.transform.eulerAngles;

            selectingAtom.subAtom.gameObject.transform.eulerAngles = new Vector3(0, 0, q.z + 180);
        }
        else if (Input.GetKeyDown(KeyCode.D) && selectingAtom != null)
        {
            Vector3 q = selectingAtom.subAtom.gameObject.transform.eulerAngles;

            selectingAtom.subAtom.gameObject.transform.eulerAngles = new Vector3(0, 0, q.z - 90);
        }
        else if (Input.GetKeyDown(KeyCode.A) && selectingAtom != null)
        {
            Vector3 q = selectingAtom.subAtom.gameObject.transform.eulerAngles;

            selectingAtom.subAtom.gameObject.transform.eulerAngles = new Vector3(0, 0, q.z+90);
        }
        

        if (Input.GetMouseButtonDown(0))
        {

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D h = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);


            if (h.transform == null)
            {



                if (this.graphicRaycast().Count == 0)
                {
                    if (electronSlider.GetComponent<CanvasGroup>().alpha != 0)
                        electronSlider.GetComponent<Animation>().Play("cgroupfadeout");

                    if (this.selectingAtom != null)
                    {

                        PointerEventData ped = new PointerEventData(null);
                        ped.position = Input.mousePosition;
                        List<RaycastResult> results = new List<RaycastResult>();
                        selectingAtom.panelCanvas.GetComponent<GraphicRaycaster>().Raycast(ped, results);

                           
                        if(results.Count == 0)
                            this.deselectAtom();

                    }
                }
                

                this.deselectMolecule();

                if (this.selectingType != SelectingType.ATOM && selectingMolecule == null) //Order!
                    this.setSelectingType(SelectingType.ATOM);
            }
            
        }

        double wheel = Input.GetAxis("Mouse ScrollWheel");

        if (selectingAtom != null)
        {
            Vector3 q = selectingAtom.subAtom.gameObject.transform.eulerAngles;
            if (wheel > 0)
            {

                selectingAtom.subAtom.gameObject.transform.eulerAngles = new Vector3(0, 0, q.z + 15);
            }
            else if (wheel < 0)
            {
                selectingAtom.subAtom.gameObject.transform.eulerAngles = new Vector3(0, 0, q.z - 15);
            }
        }

        if (draggingObjectMolecule != null)
        {
            Vector3 q = draggingObjectMolecule.gameObject.transform.eulerAngles;
            if (wheel > 0)
            {

                draggingObjectMolecule.gameObject.transform.eulerAngles = new Vector3(0, 0, q.z + 15);
            }
            else if (wheel < 0)
            {
                draggingObjectMolecule.gameObject.transform.eulerAngles = new Vector3(0, 0, q.z - 15);
            }
        }
        

        if (dragging && selectingAtom != null && selectingType == SelectingType.ATOM)
        {
            Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10);
            Vector3 objPosition = Camera.main.ScreenToWorldPoint(mousePosition);

            float dist = Vector3.Distance(objPosition, selectingAtom.gameObject.transform.position);
          //  if(dist>0.3f)
            selectingAtom.transform.position = Snap(objPosition, 0.25f);
        }

        if (dragging && draggingObjectMolecule != null && selectingType == SelectingType.MOLECULE)
        {
            Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10);
            Vector3 objPosition = Camera.main.ScreenToWorldPoint(mousePosition);

            draggingObjectMolecule.transform.position = Snap(objPosition, 0.25f);
        }

    }

    public List<Transform> electronsToTransformList(List<Electron> list)
    {
        List<Transform> transforms = new List<Transform>();

        foreach(Electron e in list)
        {
            transforms.Add(e.transform);
        }

        return transforms;
    }

    public Transform GetClosestTransform(List<Transform> transforms, Transform thisTransform)
    {
        Transform bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = thisTransform.position;
        foreach (Transform potentialTarget in transforms)
        {
            Vector3 directionToTarget = potentialTarget.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = potentialTarget;
            }
        }
        return bestTarget;
    }

    public enum SelectingType
    {
        ATOM,
        MOLECULE
    }

    [Header("UI")]
    public TMP_Text moleculeListText;
    public TMP_Text selectedText;
    public GameObject moleculePanel;
    public GameObject deletePanel;
    public GameObject informationPanel;
    public GameObject menuGameobject;
    public GameObject blurGameobject;

    public GraphicRaycaster graphicRaycaster;
    public ElectronSlider electronSlider;

    [Header("Dragging/Selecting objects")]
    public SelectingType selectingType = SelectingType.ATOM;
    public bool dragging;
    public GameObject draggingObjectMoleculePrefab;
    public GameObject draggingObjectMolecule;
    public Atom selectingAtom;
    public Molecule selectingMolecule = null;

    [Header("Managers")]
    public BondManager bondManager;
    public GameplayManager gameplayManager;
    public TutorialManager tutorialManager;
    public AudioManager audioManager;

    public GameObject atomPrefab;

    public bool isMenuOpened;
    public bool timerCache;

    public CursorLockMode cachedCursorLockMode;

    public void setSelectingType(SelectingType selectingType)
    {
        this.selectingType = selectingType;
        this.deselectAtom();
        this.deselectMolecule();

        switch (selectingType)
        {
            case SelectingType.MOLECULE:
              //  selectingTypeText.text = "Molecules (Q)";
                Camera.main.GetComponent<Animation>().Play("camera_atomToMolecule");
                break;

            case SelectingType.ATOM:
             //   selectingTypeText.text = "Atoms (Q)";
                Camera.main.GetComponent<Animation>().Play("camera_moleculeToAtom");
                break;

            default:
                break;
        }
    }

    public Atom spawnAtom(int protonNumber)
    {
        return spawnAtom(Atom.GetAtomType(protonNumber), Vector3.zero,Quaternion.identity);
    }

    public Atom spawnAtom(Atom.AtomType at)
    {
        return spawnAtom(at, Vector3.zero, Quaternion.identity);
    }

    public void returnToMainMenu()
    {
        SceneManager.LoadScene("MenuScene");
    }

    public void openMenu()
    {
        if(gameplayManager != null)
        {
            timerCache = gameplayManager.timer;
            gameplayManager.timer = false;
        }

        if(tutorialManager != null)
        {
            tutorialManager.pause = true;
            cachedCursorLockMode = Cursor.lockState;
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        menuGameobject.SetActive(true);
        blurGameobject.SetActive(true);
        isMenuOpened = true;
    }

    public void closeMenu()
    {
        if (gameplayManager != null)
        {
            if(gameplayManager.timer != true)
                gameplayManager.timer = timerCache;
        }

        if (tutorialManager != null)
        {
            tutorialManager.pause = false;
            Cursor.lockState = cachedCursorLockMode;
            if(cachedCursorLockMode == CursorLockMode.Locked)
            {
                Cursor.visible = false;
            }
            else
            {
                Cursor.visible = true;
            }
        }

        menuGameobject.SetActive(false);
        blurGameobject.SetActive(false);
        isMenuOpened = false;
    }


    public void resetG()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public Atom spawnAtom(Atom.AtomType at, Vector3 vector,Quaternion rotation)
    {

        GameObject go = Instantiate(atomPrefab,vector,rotation);

        Atom a = go.GetComponent<Atom>();
        a.type = at;
        
        return a;
    }

    public void selectAtom(Atom a)
    {
        
            deselectAtom();
       
            a.selectedMark(true);
            selectingAtom = a;

        if (!a.panelCanvas.gameObject.activeSelf)
        {
            a.panelCanvas.gameObject.SetActive(true);



        }

        updateMoleculeList();
        updateSelectedText();

    }

    public void deselectAtom()
    {
        if(selectingAtom != null)
        {
            selectingAtom.selectedMark(false);

            if (selectingAtom.panelCanvas.gameObject.activeSelf)
                selectingAtom.panelCanvas.gameObject.SetActive(false);

        }

        selectingAtom = null;

        updateMoleculeList();
        updateSelectedText();
    }

    public void selectMolecule(Molecule m)
    {
        
            deselectMolecule();

        if (m == null)
            return;
        

            foreach (Atom a in m.atoms)
            {
                a.selectedMark(true);

            }
            selectingMolecule = m;
        updateSelectedText();
        updateMoleculeList();

    }

    public void deselectMolecule()
    {

        if (selectingMolecule == null)
            return;

        foreach (Atom a in selectingMolecule.atoms)
        {
            a.selectedMark(false);

        }

        selectingMolecule = null;
        updateMoleculeList();
        updateSelectedText();
    }

    public void electronExit(Electron electron, Electron other)
    {
        if (draggingObjectMolecule != null)
            return;

        bondManager.unbindElectrons(electron, other);
    }

    public void electronEnter(Electron electron, Electron other)
    {
     /*   if (draggingObjectMolecule != null)
            return;*/
        
        bondManager.bindElectrons(electron, other);
    }

    public void mouseDown(Atom a)
    {
        if (selectingType == SelectingType.ATOM)
        {
            if (selectingAtom != a)
            {

                selectAtom(a);
            }
            //  else
            // {
            //     dragging = true;
            //  }

            Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10);
            Vector3 objPosition = Camera.main.ScreenToWorldPoint(mousePosition);
            deletePanel.GetComponent<Animation>().Play("deletepanelfadein");
            if(electronSlider.GetComponent<CanvasGroup>().alpha != 1)
                electronSlider.GetComponent<Animation>().Play("cgroupfadein");
            dragging = true;
        }else if(selectingType == SelectingType.MOLECULE)
        {
            Molecule m = bondManager.getAtomMolecule(a);

            if (m == null)
            {
                if (a != null)
                {
                    setSelectingType(SelectingType.ATOM);
                    mouseDown(a);
                   // selectAtom(a);
                }
                return;
            }

            if (a == null)
                return;

                selectMolecule(m);

                float x = 0;
                float y = 0;

                foreach (Atom atom in selectingMolecule.atoms)
                {
                    x += atom.transform.position.x;
                    y += atom.transform.position.y;
                }

                x = x / selectingMolecule.atoms.Count;
                y = y / selectingMolecule.atoms.Count;

                draggingObjectMolecule = Instantiate(draggingObjectMoleculePrefab, new Vector3(x, y, 0), Quaternion.identity);

                foreach (Atom atom in selectingMolecule.atoms)
                {
                    atom.transform.SetParent(draggingObjectMolecule.transform);
                }

                deletePanel.GetComponent<Animation>().Play("deletepanelfadein");
                dragging = true;

        }
    }

    public void setSelectedAtomTriple()
    {
        if(this.selectingAtom != null)
        {
            selectingAtom.setTripleMode(!selectingAtom.tripleMode);
        }
    }

    public void clearScreen()
    {

        foreach(Molecule m in new List<Molecule>(bondManager.molecules))
        {

            bondManager.removeMolecule(m);

        }

        foreach(GameObject g in GameObject.FindGameObjectsWithTag("Atom"))
        {
            Destroy(g);
        }

        this.updateMoleculeList();
        this.updateSelectedText();

    }

    public void displayAtomProperties(Atom a)
    {

        StringBuilder sb = new StringBuilder();

        sb.Append(a.getInfo().atomName + " (" + a.getInfo().atomSymbol + ")");
        sb.AppendLine(" ");
        sb.AppendLine("Proton Sayısı: " + a.getInfo().protonNumber);
        sb.AppendLine("Kütle:" + a.getInfo().atomicMassUnit.ToString("0.##")+" akb");
        sb.AppendLine("Elektronegatiflik: " + a.getInfo().electroNegativity.ToString("0.##"));
        sb.AppendLine("Formal Yük: " + a.getFormalCharge());


        if (a.type != Atom.AtomType.Boron)
        {

            string orbitMessage = "Hata.";

            if (a.areElectronsSatisfied())
            {
                orbitMessage = "Tamamlandı";
            }
            else
            {
                orbitMessage = "Tamamlanmadı";
            }

            sb.AppendLine(a.getLastOrbitName() + ": " + orbitMessage);

        }

        selectedText.text = sb.ToString();

    }

    public void displayMoleculeProperties(Molecule m)
    {

        StringBuilder sb = new StringBuilder();

        sb.Append(m.getFormula() + "|" + m.getName());
        sb.AppendLine(" ");
        sb.AppendLine("Molar Kütle: " + m.getMolarMass().ToString("0.##") +" g/mol");
        sb.AppendLine("Formal Yük: " + m.getCharge());
        //sb.AppendLine("Stabil: " + m.isStable());


        selectedText.text = sb.ToString();

    }

    public void updateSelectedText()
    {
        if (selectingAtom != null)
        {

            displayAtomProperties(selectingAtom);
            updateElectronSlider();

        }
        else if (selectingMolecule != null)
        {
            if (selectingMolecule.getMolarMass() > 0)
                displayMoleculeProperties(selectingMolecule);
            else
                selectedText.text = "";
        }
        else
            selectedText.text = "";
    }

    public void updateMoleculeList()
    {
        if (bondManager.molecules.Count < 1)
            moleculePanel.SetActive(false);
        else
            moleculePanel.SetActive(true);

        RectTransform moleculePanelRect = moleculePanel.GetComponent<RectTransform>();

        float width = 100;
        float height = 100 + bondManager.molecules.Count * 10;

        moleculePanelRect.sizeDelta = new Vector2(width,height);

        moleculeListText.rectTransform.sizeDelta = moleculePanelRect.sizeDelta;

        moleculeListText.text = "";
        foreach (Molecule m in bondManager.molecules)
        {
            if(m.isStable())
                moleculeListText.text = moleculeListText.text + m.getFormula() + "\n";
            else
                moleculeListText.text = moleculeListText.text + "<color=#990000>" + m.getFormula() +"</color>" + "\n";
        }
    }

    public void onElectronSliderChange(int electrons)
    {
        if(selectingAtom != null)
        {

            //selectingAtom.setElectronCount(electrons);

            int defaultElectrons = selectingAtom.getInfo().defaultElectronNumber;
            int defaultLastOrbital = Atom.getLastOrbitalElectronCount(defaultElectrons);

            int toSet = defaultElectrons - defaultLastOrbital + electrons;


            if (toSet != selectingAtom.electronCount)
            {

                selectingAtom.unbindFromAllAtoms();

                selectingAtom.setElectronCount(toSet);

                selectingAtom.setupElectrons();
                    
            }
        }
    }

    public void updateElectronSlider()
    {

        if(selectingAtom != null)
        {

            /*  electronSlider.setSlider(selectingAtom.getMinElectronCount(), selectingAtom.getMaxElectronCount(), selectingAtom.electronCount, selectingAtom.getInfo().defaultElectronNumber);
              electronSlider.setSlider(selectingAtom.getMinElectronCount(), selectingAtom.getMaxElectronCount(), selectingAtom.electronCount, selectingAtom.getInfo().defaultElectronNumber);
              */

            int min = 1;
            int max = 8;

            if (selectingAtom.getInfo().protonNumber < 3)
                max = 2;

            electronSlider.setSlider(min, max, Atom.getLastOrbitalElectronCount(selectingAtom.electronCount), Atom.getLastOrbitalElectronCount(selectingAtom.getInfo().defaultElectronNumber));

        }

    }
    public void mouseUp()
    {
        dragging = false;

        deletePanel.GetComponent<Animation>().Play("deletepanelfadeout");

        RectTransform rectTransformDeletePanel = deletePanel.GetComponent<RectTransform>();

        foreach (RaycastResult rr in this.graphicRaycast())
        {

            if(rr.gameObject.tag == "SubmitPanel")
            {
                if(selectingMolecule != null && selectingMolecule.getMolarMass() > 0)
                {
                    gameplayManager.onMoleculeSubmitted(selectingMolecule);
                }
            }

            if (rr.gameObject.tag == "DeletePanel")
            {
                if (selectingAtom != null)
                {
                    selectingAtom.removeAtom();
                    selectingAtom = null;
                }
                else if (draggingObjectMolecule != null)
                {
                    Atom a = draggingObjectMolecule.GetComponentInChildren<Atom>();

                    if (a != null)
                    {
                        Molecule m = bondManager.getAtomMolecule(a);
                        if (m != null)
                        {

                            bondManager.removeMolecule(m);

                            Destroy(draggingObjectMolecule);
                            draggingObjectMolecule = null;
                            selectingMolecule = null;


                        }
                    }
                }
                updateMoleculeList();
                updateSelectedText();
            }
        }

        if (draggingObjectMolecule != null)
        {
            if (selectingMolecule != null)
            {
                foreach (Atom at in selectingMolecule.atoms)
                {

                    float rot = at.transform.rotation.eulerAngles.z;

                    at.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                    at.subAtom.transform.rotation = Quaternion.Euler(at.subAtom.transform.rotation.eulerAngles + new Vector3(0, 0, rot));


                }

            }

            draggingObjectMolecule.transform.DetachChildren();

            Destroy(draggingObjectMolecule);

            draggingObjectMolecule = null;

        }

        
    }

    public List<RaycastResult> graphicRaycast()
    {
        PointerEventData ped = new PointerEventData(null);
        ped.position = Input.mousePosition;
        List<RaycastResult> results = new List<RaycastResult>();
        graphicRaycaster.Raycast(ped, results);

        return results;
    }

    public Vector3 Snap(Vector3 pos, float v)
    {
        float x = pos.x;
        float y = pos.y;
        float z = pos.z;
        x = Mathf.FloorToInt(x / v) * v;
        y = Mathf.FloorToInt(y / v) * v;
        z = Mathf.FloorToInt(z / v) * v;
        return new Vector3(x, y, z);
    }
}

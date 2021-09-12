using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectingRectManager : MonoBehaviour
{

    public RectTransform panel;
    public Manager manager;

    public CBVector2 originPos = null;

    public bool holding;
    public bool firstHoldingFlag;
    public bool selection;

    void Update()
    {

        holding = Input.GetMouseButton(0);

        if (holding)
        {
            if (!firstHoldingFlag)
            {
                //First holding

                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit2D h = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);

                bool atomPanel = true;

                if(manager.selectingAtom != null)
                {
                    PointerEventData ped = new PointerEventData(null);
                    ped.position = Input.mousePosition;
                    List<RaycastResult> results = new List<RaycastResult>();
                    manager.selectingAtom.panelCanvas.GetComponent<GraphicRaycaster>().Raycast(ped, results);

                    if (results.Count != 0)
                    {
                        atomPanel = false;
                    }
                }

                if (manager.selectingType == Manager.SelectingType.ATOM && manager.graphicRaycast().Count == 0 && h .transform == null && atomPanel)
                {
                    selection = true;
                    panel.sizeDelta = Vector2.zero;
                    setCurrentPosOrigin();
                }
                else
                {
                    selection = false;
                }

                firstHoldingFlag = true;
            }

            if(selection)
                doSizing();
        }
        else
        {
            firstHoldingFlag = false; //Reset first holding flag
            selection = false;
        }

        if (selection)
        {
            List<Atom> containingAtoms = getContainingAtoms();

            Molecule m = selectMolecule(containingAtoms);

            if (m != null)
                selection = false;

        }

        panel.gameObject.SetActive(selection);
    }

    public Molecule selectMolecule(List<Atom> containingAtoms)
    {
        if (containingAtoms.Count > 1)
        {
            Molecule m = null;

            foreach (Atom a in containingAtoms)
            {
                m = manager.bondManager.getAtomMolecule(a);
                if (m != null)
                {
                    break;
                }
            }

            if (m != null)
            {
                manager.setSelectingType(Manager.SelectingType.MOLECULE);
                manager.selectMolecule(m);
            }

            return m;
        }
        return null;
    }

    public List<Atom> getContainingAtoms()
    {
        Vector2 mouseVec = getMouseVector2();

        GameObject[] arr = GameObject.FindGameObjectsWithTag("Atom");

        List<Atom> containingAtoms = new List<Atom>();

        foreach (GameObject go in arr)
        {
            Vector2 pos = Camera.main.WorldToScreenPoint(go.transform.position);

            //Debug.Log("Origin: " + originPos + " | Pos: " + pos + " | MouseVec: " + mouseVec);

            if (((originPos.x > pos.x && pos.x > mouseVec.x) || (mouseVec.x > pos.x && pos.x > originPos.x)) && ((originPos.y > pos.y && pos.y > mouseVec.y) || (mouseVec.y > pos.y && pos.y > originPos.y)))
            {
                containingAtoms.Add(go.GetComponent<Atom>());
            }

        }

        return containingAtoms;
    }

    public void doSizing() {
        Vector2 mouseVec = getMouseVector2();

        float widthFactor = Screen.width / 1120f;
        float heightFactor = Screen.height / 600f;

        float deltaOriginX = (mouseVec.x - originPos.x) / widthFactor;
        float deltaOriginY = (originPos.y - mouseVec.y) / heightFactor;

        float threshold = 20;

        if (Mathf.Abs(deltaOriginX) > threshold || Mathf.Abs(deltaOriginY) > threshold)
        {
            panel.gameObject.SetActive(true);
            panel.position = new Vector3(originPos.x, originPos.y, 0);

            Vector2 size = new Vector2(Mathf.Abs(deltaOriginX), Mathf.Abs(deltaOriginY));

            if (deltaOriginX < 0 && deltaOriginY < 0)
            {
                panel.rotation = Quaternion.Euler(0, 0, 180);
            }
            else if (deltaOriginX < 0)
            {
                panel.rotation = Quaternion.Euler(0, 180, 0);
            }
            else if (deltaOriginY < 0)
            {
                panel.rotation = Quaternion.Euler(0, 180, 180);
            }
            else
            {
                panel.rotation = Quaternion.Euler(0, 0, 0);
            }

            panel.sizeDelta = size;
        }
    }

    public void setCurrentPosOrigin()
    {
        Vector2 mouseVec = getMouseVector2();
        originPos = new CBVector2(mouseVec);
    }

    public bool isMouseOnEmpty()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D h = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);
        if (h.transform == null)
        {
            if (manager.graphicRaycast().Count == 0)
            {
                return true;
            }
        }
        return false;
    }

    public Vector2 getMouseVector2()
    {
        Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
        return new Vector2(mousePosition.x, mousePosition.y);
    }
    public Vector2 getMouseWorldVector2()
    {
        Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
        Vector3 explosionPos_ = Camera.main.ScreenToWorldPoint(mousePosition);
        return new Vector2(explosionPos_.x, explosionPos_.y);
    }
}

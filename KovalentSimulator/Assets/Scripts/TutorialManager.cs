using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{

    public Manager manager;

    public Image checkmark_;
    public TMP_Text stateText;
    public TMP_Text tutorialText;

    public bool finishedWriting = true;
    public bool canContinue = false;

    public int state = 0;

    public List<Atom> cachedAtoms = new List<Atom>();

    public bool pause;

    public GameObject questionGameobject;
    public GameObject puanExplainer;
    public GameObject timeExplainer;
    public GameObject formulaButtonExplainer;
    public GameObject submitPanelExplainer;
    public GameObject atomSpawners;

    void Awake()
    {
        manager.gameplayManager.enabled = false;
        questionGameobject.SetActive(false);

        puanExplainer.SetActive(false);
        timeExplainer.SetActive(false);
        formulaButtonExplainer.SetActive(false);
        submitPanelExplainer.SetActive(false);
        atomSpawners.SetActive(false);
    }

    void Start()
    {
        QuestionMoleculeDictionary.initialize();
        doTutorial();
    }

    void Update()
    {
  
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            continueTutorial();
        }

        if(state == 2 && finishedWriting)
        {
            if(manager.selectingAtom != null)
            {
                Vector3 vec = manager.selectingAtom.transform.position;
                if (Mathf.Abs(vec.x) > 0.5 || Mathf.Abs(vec.y) > 0.5)
                {
                    state = 3;
                    doTutorial();
                }

            }
        }

        if (state == 4 && finishedWriting)
        {
            if (manager.selectingAtom != null)
            {

                float z = manager.selectingAtom.subAtom.transform.localRotation.eulerAngles.z;

                if (300 >= z && z >= 45)
                {
                    state = 5;
                    doTutorial();
                }

            }
        }

        if(state == 6 && finishedWriting)
        {
            if(manager.bondManager.molecules.Count > 0)
            {
                state = 8;
                doTutorial();
            }
        }

        if(state == 9 && finishedWriting)
        {
            if(manager.bondManager.molecules.Count > 0)
            {


                QuestionMolecule qm = QuestionMoleculeDictionary.dioksijen;
                Molecule m = manager.bondManager.molecules[0];

                if(qm == null)
                {
                    QuestionMoleculeDictionary.initialize();
                    qm = QuestionMoleculeDictionary.dioksijen;
                }

                if(manager.gameplayManager.doesQuestionMoleculeMatch(m, qm))
                {
                    state = 11;
                    doTutorial();
                }

                

            }
        }

        if(state == 14 && finishedWriting)
        {
            if(manager.bondManager.molecules.Count > 0)
            {

                if(manager.selectingMolecule != null && manager.selectingMolecule.getMolarMass()>0)
                {
                    state = 15;
                    doTutorial();
                }

            }
        }

        if(state == 17 && finishedWriting)
        {
            if(manager.bondManager.molecules.Count > 0)
            {

                QuestionMolecule qm = QuestionMoleculeDictionary.diazot;
                Molecule m = manager.bondManager.molecules[0];

                if (qm == null)
                {
                    QuestionMoleculeDictionary.initialize();
                    qm = QuestionMoleculeDictionary.diazot;
                }

                if (manager.gameplayManager.doesQuestionMoleculeMatch(m, qm))
                {
                    state = 18;
                    doTutorial();
                }
            }
        }

        if(state == 20 & finishedWriting)
        {
            if (manager.bondManager.molecules.Count > 0)
            {

                
                Molecule m = manager.bondManager.molecules[0];

                List<QuestionAtom> list = new List<QuestionAtom>();

                list.Add(new QuestionAtom(Atom.AtomType.Hydrogen, 1, 0, 1, new List<Atom.AtomType> { Atom.AtomType.Nitrogen }));
                list.Add(new QuestionAtom(Atom.AtomType.Hydrogen, 1, 0, 1, new List<Atom.AtomType> { Atom.AtomType.Nitrogen }));
                list.Add(new QuestionAtom(Atom.AtomType.Hydrogen, 1, 0, 1, new List<Atom.AtomType> { Atom.AtomType.Nitrogen }));
                list.Add(new QuestionAtom(Atom.AtomType.Hydrogen, 1, 0, 1, new List<Atom.AtomType> { Atom.AtomType.Nitrogen }));
                list.Add(new QuestionAtom(Atom.AtomType.Nitrogen, 6, 1, 4, new List<Atom.AtomType> { Atom.AtomType.Hydrogen, Atom.AtomType.Hydrogen, Atom.AtomType.Hydrogen, Atom.AtomType.Hydrogen }));

                QuestionMolecule qm = new QuestionMolecule("Amonyum",new List<QuestionAtom>(list), 1, 4);


                if (manager.gameplayManager.doesQuestionMoleculeMatch(m, qm))
                {
                    state = 21;
                    doTutorial();
                }
            }
        }

        if(state == 23 && finishedWriting)
        {
            int nonNullAtoms = 0;

            foreach(Atom a in cachedAtoms)
            {
                if(a != null)
                {
                    nonNullAtoms++;
                }
            }

            if (nonNullAtoms == 0)
            {
                state = 24;
                doTutorial();
            }
        }

    }

    public void continueTutorial()
    {

        if (!canContinue)
            return;

        SetStateText(StateTextEnum.HALT);

        state++;

        doTutorial();

    }

    public void doTutorial()
    {

        
        if(state == 0)
        {
            clearText();
            textAndContinue("Eğitim moduna hoşgeldiniz. Burada uygulamayı kullanabilmek için gerekli bilgilerin üzerinden geçeceğiz.");
        }

        if(state == 1)
        {

            clearText();

            cachedAtoms.Add(manager.spawnAtom(Atom.AtomType.Oxygen));

            cachedAtoms[0].bornAtom();

            textAndContinue("Bu oksijen atomunun lewis yapısıdır. Etrafındaki beyaz noktalar, ortaklanmamış elektronları; çift siyah noktalar ise ortaklanmış elektron çiftlerini temsil eder. ");

        }

        if(state == 2)
        {
            clearText();
            textAndObjective("Atomları seçmek için üstlerine gelip sol tıklamanız gerekmektedir. Seçilen atomlar turuncu rengini alacaktır. Atomları hareket ettirmek için ise sol tıka basılı tutarak sürüklemeniz gerekmektedir.","Oksijen atomunu hareket ettirin.");
        }

        if(state == 3)
        {
            taskCompletedContinue();
        }

        if(state == 4)
        {
            clearText();
            textAndObjective("Sol tıka basılı tutup fare tekerleğini kullanarak ya da atom panelindeki döndürme tuşlarını kullanarak atomu döndürebilirsiniz.", "Oksijen atomunu döndürün");
        }

        if (state == 5)
        {
            taskCompletedContinue();
        }

        if(state == 6)
        {
            clearText();

            clearCachedAtoms();          
            
            cachedAtoms.Add(manager.spawnAtom(Atom.AtomType.Hydrogen, new Vector3(2, 0, 0), Quaternion.identity));
            cachedAtoms.Add(manager.spawnAtom(Atom.AtomType.Hydrogen, new Vector3(-2, 0, 0), Quaternion.identity));

            cachedAtoms[0].bornAtom();
            cachedAtoms[1].bornAtom();

            textAndObjective("Elektron paylaşımı ve kovalent bağ oluşturmak için atomlardaki ortaklanmamış elektronları (beyaz noktaları) üst üste getirmeniz gerekmektedir.", "Hidrojen atomlarını kulllanarak bir kovalent bağ oluşturun.");

        }

        if(state == 8)
        {

            taskCompletedContinue();

        }

        if(state == 9)
        {
            clearText();

            clearCachedAtoms();

            cachedAtoms.Add(manager.spawnAtom(Atom.AtomType.Oxygen, new Vector3(2, 0, 0), Quaternion.identity));
            cachedAtoms.Add(manager.spawnAtom(Atom.AtomType.Oxygen, new Vector3(-2, 0, 0), Quaternion.identity));

            cachedAtoms[0].bornAtom();
            cachedAtoms[1].bornAtom();

            textAndObjective("Oluşturulan moleküller sağ üst köşedeki molekül listesinde gözükecektir. Şimdi ise atomları döndürerek ve hareket ettirerk çift bağ yapalım.", "Oksijen atomları ile ikili kovalent bağ yapın.");
        }

        if(state == 11)
        {
            taskCompletedContinue();
        }

        if(state == 12)
        {
            clearText();

            manager.informationPanel.SetActive(true);

            textAndContinue("Sol alt köşede bulunan bilgi paneli ile seçtiğiniz nesnenin özelliklerini görüntüleyebilirsiniz.");

        }

        if(state == 13)
        {
            clearText();

            //manager.selectingTypeText.gameObject.SetActive(true);

            textAndContinue("Boş bir alanda sol tıka basılı tutup imleçinizi hareket ettirdiğiniz zaman birden fazla atomu seçebilirsiniz. Birbirine bağlı iki atomu seçtiğiniz zaman o molekül tamamen seçilecektir. Molekül seçim moduna girdiğinizde arkaplan renk değiştirecektir. Molekül seçim modundan çıkmak için boş bir yere ya da bir atoma tıklayabilirsiniz.");
        }

        if(state == 14)
        {
            clearText();

            manager.deselectAtom();
            manager.deselectMolecule();

            text("Sol tıka basılı tutup seçme alanı ile bir molekül seçin. Aynı zamanda özelliklerine de bakabilirsiniz.");


        }

        if(state == 15)
        {
            taskCompletedContinue();
        }

        if(state == 16)
        {
            clearCachedAtoms();
            textAndContinue("Bazı durumlarda üçlü kovalent bağ yapmanız gerekebilir. Üçlü kovalent bağ yapabilen atomu olması gereken dizilime getirmek için atomu seçin ve bilgi panelinin yanındaki '|||' butonunu kullanın.");

        }

        if(state == 17)
        {
            cachedAtoms.Add(manager.spawnAtom(Atom.AtomType.Nitrogen, new Vector3(-2, 0, 0), Quaternion.identity));
            cachedAtoms.Add(manager.spawnAtom(Atom.AtomType.Nitrogen, new Vector3(2, 0, 0), Quaternion.identity));

            cachedAtoms[0].bornAtom();
            cachedAtoms[1].bornAtom();

            text("Üçlü kovalent bağ bulunduran bir molekül yapın.");
        }

        if(state == 18)
        {
            taskCompletedContinue();
        }

        if(state == 19)
        {
            clearCachedAtoms();
            clearText();

            manager.electronSlider.gameObject.SetActive(true);
            manager.electronSlider.GetComponent<Animation>().Play("cgroupfadein");

            textAndContinue("İyonların lewis yapısını göstermeye ihtiyaç duyduğunuzda ekranın sağında bulunan valans elektron sürgüsünü kullanabilirsiniz. Valans elektron sürgüsü atomları seçtiğinizde belirecektir.");
        }

        if(state == 20)
        {
            manager.electronSlider.GetComponent<Animation>().Play("cgroupfadeout");
            clearText();

            cachedAtoms.Add(manager.spawnAtom(Atom.AtomType.Hydrogen, new Vector3(0, 3, 0), Quaternion.identity));
            cachedAtoms.Add(manager.spawnAtom(Atom.AtomType.Nitrogen, new Vector3(0, 1, 0), Quaternion.identity));
            cachedAtoms.Add(manager.spawnAtom(Atom.AtomType.Hydrogen, new Vector3(0, -1, 0), Quaternion.identity));
            cachedAtoms.Add(manager.spawnAtom(Atom.AtomType.Hydrogen, new Vector3(2, 1, 0), Quaternion.identity));
            cachedAtoms.Add(manager.spawnAtom(Atom.AtomType.Hydrogen, new Vector3(-2, 1, 0), Quaternion.identity));

            cachedAtoms[0].bornAtom();
            cachedAtoms[1].bornAtom();
            cachedAtoms[2].bornAtom();
            cachedAtoms[3].bornAtom();
            cachedAtoms[4].bornAtom();

            text("Amonyum iyonunda (H₄N⁺¹) bir tane azot ile dört tane hidrojen bağ yapmaktadır. Valans elektron sürgüsünü kullanarak azotun dört bağ yapmasını sağlayabilirsiniz.\n Amonyum iyonu (H₄N⁺¹) yapın.");

        }

        if(state == 21)
        {
            taskCompletedContinue();
        }

        if(state == 22)
        {
            clearText();

            manager.deletePanel.SetActive(true);
            manager.deletePanel.GetComponent<Animation>().Play("deletepanelfadein");

            textAndContinue("Sağ altta bulunan silme paneli ile molekülleri bütün olarak ya da atomları teker teker sürükleyerek silebilirsiniz.");
        }

        if(state == 23)
        {
            text("Bütün atomları silin.");
        }

        if(state == 24)
        {
            taskCompletedContinue();
        }

        if(state == 25)
        {
            textAndContinue("Şimdi ise oyun içinden örnek bir soruya bakalım.");
        }

        if(state == 26)
        {
            StartCoroutine(revealExampleQuestion());


            StringBuilder sb = new StringBuilder();

            sb.AppendLine("Formül Butonu: Bir miktar puandan ödün vererek molekülün formülünü söyler.");
            sb.AppendLine("Puan: Şu anki oturumdaki toplam puan.");
            sb.AppendLine("Kalan Zaman: Soruyu çözmeniz için kalan zaman.");
            sb.AppendLine("Molekül Teslim Paneli: Soruların sizden istediği molekülleri seçip buraya sürükleyin.");

            textAndContinue(sb.ToString());


        }

        if (state == 27)
        {
            clearText();

            puanExplainer.SetActive(false);
            timeExplainer.SetActive(false);
            formulaButtonExplainer.SetActive(false);
            submitPanelExplainer.SetActive(false);

            manager.gameplayManager.enabled = true;
            manager.gameplayManager.setQuestion(new Question(30,0,QuestionMoleculeDictionary.su));
            atomSpawners.SetActive(true);

        }

        if(state == 28)
        {

            textAndContinue("Eğitim tamamlandı.");

        }

        if(state == 29)
        {
            finishTutorial();
        }

    }

    IEnumerator revealExampleQuestion()
    {
       
        questionGameobject.SetActive(true);

        yield return new WaitForSeconds(1);

        puanExplainer.SetActive(true);

        yield return new WaitForSeconds(1);

        timeExplainer.SetActive(true);

        yield return new WaitForSeconds(1);

        formulaButtonExplainer.SetActive(true);

        yield return new WaitForSeconds(1);

        submitPanelExplainer.SetActive(true);

    }

    public void finishTutorial()
    {
        PlayerPrefs.SetInt("tutorialCompleted", 1);
        SceneManager.LoadScene("MenuScene", LoadSceneMode.Single);
    }

    public void textAndContinue(string msg)
    {
        StartCoroutine(CoroutineTextAndContinue(msg));
    }

    public void textAndObjective(string msg, string objective)
    {
        StartCoroutine(CoroutineTextAndObjective(msg, objective));
    }

    public void text(string msg)
    {
        StartCoroutine(CoroutineText(msg));
    }

    public void taskCompletedContinue()
    {
        StartCoroutine(CoroutineTaskCompletedContinue());
    }
    
    public void clearText()
    {
        tutorialText.text = "";
    }

    IEnumerator CoroutineTaskCompletedContinue()
    {
        checkmark(true);

        yield return new WaitForSeconds(0.5f);

        SetStateText(StateTextEnum.OBJSUCCESS);
    }

    IEnumerator CoroutineText(string msg)
    {
        checkmark(false);
        SetStateText(StateTextEnum.HALT);

        yield return StartCoroutine(revealText(msg));
    }

    IEnumerator CoroutineTextAndContinue(string msg)
    {

        checkmark(false);
        SetStateText(StateTextEnum.HALT);

        yield return StartCoroutine(revealText(msg));

        yield return new WaitForSeconds(0.5f);

        SetStateText(StateTextEnum.CONTINUE);
    }

    IEnumerator CoroutineTextAndObjective(string msg, string objective)
    {

        checkmark(false);
        SetStateText(StateTextEnum.HALT);

        yield return StartCoroutine(revealText(msg));

        yield return new WaitForSeconds(0.5f);

        SetStateText(objective);
    }

    IEnumerator revealText(string msg)
    {
        finishedWriting = false;
        tutorialText.maxVisibleCharacters = 0;
        tutorialText.text = msg;
        yield return new WaitForSeconds(0.01f);

        int totalVisibleCharacters = tutorialText.textInfo.characterCount;
        int counter = 0;

        int visibleCount = 0;


        while (visibleCount < totalVisibleCharacters)
        {
            if (!pause)
            {
                visibleCount = counter % (totalVisibleCharacters + 1);

                tutorialText.maxVisibleCharacters = visibleCount;

                counter += 1;
            }

            float waitTime = 0.04f;

            if(visibleCount < totalVisibleCharacters && visibleCount > 0)
            {
                char ch = msg[visibleCount-1];

                if (ch == '.')
                    waitTime = 0.5f;
                else if (ch == ',')
                    waitTime = 0.2f;
                else if (ch == ';')
                    waitTime = 0.2f;

            }

            yield return new WaitForSeconds(waitTime);

        }

        finishedWriting = true;

    }

    IEnumerator revealStateText(string msg)
    {
        stateText.maxVisibleCharacters = 0;
        stateText.text = msg;
        yield return new WaitForSeconds(0.01f);

        int totalVisibleCharacters = stateText.textInfo.characterCount;
        int counter = 0;

        int visibleCount = 0;


        while (visibleCount < totalVisibleCharacters)
        {
            if (!pause)
            {
              //  Debug.Log(visibleCount + "," + counter + "," + totalVisibleCharacters);
                visibleCount = counter % (totalVisibleCharacters + 1);

                stateText.maxVisibleCharacters = visibleCount;

                counter += 1;
            }

            float waitTime = 0.03f;

            if (visibleCount < totalVisibleCharacters && visibleCount > 0)
            {
                char ch = msg[visibleCount - 1];

                if (ch == '.')
                    waitTime = 0.5f;
                else if (ch == ',')
                    waitTime = 0.2f;

            }

            yield return new WaitForSeconds(waitTime);

        }

    }

    public void clearCachedAtoms()
    {

        foreach(Atom a in cachedAtoms)
        {
            a.removeAtom();
        }

        cachedAtoms.Clear();

    }

    private void SetStateText(StateTextEnum e)
    {
        switch (e)
        {
            case StateTextEnum.HALT:
                stateText.text = "";
                canContinue = false;
                break;
            case StateTextEnum.CONTINUE:
                StartCoroutine(revealStateText("Devam etmek için BOŞLUK / SOL TIK basınız."));
                canContinue = true;
                break;
            case StateTextEnum.OBJSUCCESS:
                StartCoroutine(revealStateText("Başarılı. Devam etmek için BOŞLUK / SOL TIK basınız."));
                canContinue = true;
                break;
        }
    }

    private void SetStateText(string objective)
    {
        StartCoroutine(revealStateText("Görev: " + objective));
    }

    public void checkmark(bool state)
    {
        checkmark_.gameObject.SetActive(state);
    }

    private enum StateTextEnum
    {
        OBJSUCCESS,
        CONTINUE,
        HALT,
        OBJECTIVE
    }

}

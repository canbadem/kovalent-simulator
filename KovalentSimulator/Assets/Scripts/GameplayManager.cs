using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Playables;
using UnityEngine.Animations;

public class GameplayManager : MonoBehaviour
{

    public Manager manager;
    public MusicManager musicManager;
    public LeaderboardManager leaderboardManager;

    public Animator startCountdown;

    [Header("UI")]
    public GameObject moleculePanel;
    public GameObject answerGameobject;
    public TMP_Text questionText;
    public TMP_Text questionSubmitPanelMoleculeText;
    public TMP_Text pointsText;
    public TMP_Text timeText;
    public TMP_InputField answerInputField;
    public TMP_Text invalidInputText;

    public Button formulaButton;

    public Question currentQuestion;
    public Question lastDisplayedQuestion;

    [Header("Variables")]

    public bool displayQuestionDuplicate;
    public bool isFormulaHintUsed;
    public bool finishedWriting;

    public bool isSubmittedCorrect;
    public string submittedIncorrectReason;
    public int points;

    public bool timer;
    public float timeleft = 30;

    public List<Question> questions = new List<Question>();

    public bool tutorialMode;

    void Start()
    {
        tutorialMode = SceneManager.GetActiveScene().name == "TutorialScene";
        leaderboardManager = GameObject.FindGameObjectWithTag("LeaderboardManager").GetComponent<LeaderboardManager>();

        QuestionMoleculeDictionary.initialize();
        this.setPoints(0);

      //  questions.Add(new Question(60, 10, "CO₂ molekülünün mol kütlesi kaçtır?",44.0));
        //questions.Add(new Question(60, 10, "H₂O bileşiğinin yaygın adı nedir?", "su"));
        questions.Add(new Question(80, 20, QuestionMoleculeDictionary.amonyak));
        questions.Add(new Question(50, 20, QuestionMoleculeDictionary.diazot));
        questions.Add(new Question(50, 20, QuestionMoleculeDictionary.dioksijen));
        questions.Add(new Question(80, 20, QuestionMoleculeDictionary.hidrojenflorür));
        questions.Add(new Question(80, 20, QuestionMoleculeDictionary.hidrojensiyanür));
        questions.Add(new Question(80, 20, QuestionMoleculeDictionary.hidrojenklorür));
        questions.Add(new Question(80, 20, QuestionMoleculeDictionary.hidrojensulfur));
        questions.Add(new Question(80, 20, QuestionMoleculeDictionary.karbondioksit));
        questions.Add(new Question(80, 20, QuestionMoleculeDictionary.karbontetraklorür));
        questions.Add(new Question(140, 40, QuestionMoleculeDictionary.asetikasit));
        questions.Add(new Question(100, 40, QuestionMoleculeDictionary.formikasit));
        questions.Add(new Question(80, 30, QuestionMoleculeDictionary.boran));
        questions.Add(new Question(80, 20, QuestionMoleculeDictionary.oksijendiflorür));
        questions.Add(new Question(80, 20, QuestionMoleculeDictionary.fosfortriflorür));
        questions.Add(new Question(80, 30, QuestionMoleculeDictionary.karbondisülfür));

        if (!tutorialMode)
        {
            /*     startCountdown.SetTrigger("Countdown");

                 newQuestion();
            */
            StartCoroutine(StartGame());
        }
    }

    IEnumerator StartGame()
    {
        manager.blurGameobject.SetActive(true);

        startCountdown.Play("StartCountdown");

        while (!startCountdown.GetCurrentAnimatorStateInfo(0).IsName("StartCountdown"))
        {
            yield return null;
        }

        while ((startCountdown.GetCurrentAnimatorStateInfo(0).normalizedTime) % 1 < 0.99f)
        {
            yield return null;
        }

        manager.blurGameobject.SetActive(false);
        newQuestion();
        musicManager.Play();
        
    }


    void Update()
    {
        if (timeleft > 0 && timer)
            timeleft -= Time.deltaTime;
        else if (timeleft < 0)
            timeleft = 0;

        if(timeleft == 0 && timer)
        {
            timer = false;
            StartCoroutine(timeIsUp());
        }

        timeText.text = timeleft.ToString("0.0") +"s";

        
    }

    public void onGetFormulaButtonClicked()
    {
        if (!this.finishedWriting)
            return;

        if (this.currentQuestion == null)
            return;

        if (this.currentQuestion.qt != Question.QuestionType.Molecule)
            return;

        addPoints(-2);

        questionText.text = questionText.text += "\n\nFormül: " + currentQuestion.qm.getFormula();
        questionText.maxVisibleCharacters = questionText.maxVisibleCharacters + 20;
        questionText.ForceMeshUpdate();

        formulaButton.interactable = false;
        isFormulaHintUsed = true;
    }

    public void setQuestion(Question q)
    {
        this.currentQuestion = q;
        this.timer = false;

        if (q != null)
        {
            this.timeleft = q.time;
            questions.Remove(q);
        }

        isFormulaHintUsed = false;

        displayCurrentQuestion();
    }

    public void displayCurrentQuestion()
    {
        if (currentQuestion != null) {
            if (currentQuestion.Equals(lastDisplayedQuestion))
            {
                displayQuestionDuplicate = true;
            }
            else
            {
                displayQuestionDuplicate = false;
            }

        }
        else
        {
            displayQuestionDuplicate = false;
        }

        lastDisplayedQuestion = currentQuestion;

        if(currentQuestion == null)
        {
            moleculePanel.SetActive(false);
            answerGameobject.SetActive(false);
            return;
        }

        switch (currentQuestion.qt)
        {
            case Question.QuestionType.Molecule:
                questionText.text = "<color=#D2FF00>" + this.currentQuestion.qm.name + "</color> molekülünü yapınız.";
                moleculePanel.SetActive(true);
                answerGameobject.SetActive(false);
                questionSubmitPanelMoleculeText.text = "";
                formulaButton.gameObject.SetActive(true);
                if (this.isFormulaHintUsed)
                {

                    questionText.text = questionText.text += "\n\nFormül: " + currentQuestion.qm.getFormula();

                }
                break;
            case Question.QuestionType.Double:
                questionText.text = currentQuestion.question;
                moleculePanel.SetActive(false);
                answerGameobject.SetActive(true);
                answerInputField.text = "";
                formulaButton.gameObject.SetActive(false);
                break;
            case Question.QuestionType.String:
                questionText.text = currentQuestion.question;
                moleculePanel.SetActive(false);
                answerGameobject.SetActive(true);
                answerInputField.text = "";
                formulaButton.gameObject.SetActive(false);
                break;
        }


        StartCoroutine(waitForIENAndSetFormulaButtonInteractable(waitForIENAndStartTimer(revealText())));
    }

    public void checkQuestion(string answerString, double answerDouble)
    {

        if (currentQuestion.qt == Question.QuestionType.Molecule) 
        {
            if (isSubmittedCorrect)
            {
                StartCoroutine(rightAnswer());
            }
            else
            {
                questionText.text = submittedIncorrectReason;
                StartCoroutine(wrongAnswer());
            }
        }
        else if (currentQuestion.qt == Question.QuestionType.Double)
        {
            if(currentQuestion.answerDouble == answerDouble)
            {
                StartCoroutine(rightAnswer());
            }
            else
            {
                questionText.text = "Yanlış cevap";
                StartCoroutine(wrongAnswer());
            }
        }
        else if (currentQuestion.qt == Question.QuestionType.String)
        {

            if (currentQuestion.answerString.ToLower() == answerString.ToLower())
            {
                StartCoroutine(rightAnswer());
            }
            else
            {
                questionText.text = "Yanlış cevap";
                StartCoroutine(wrongAnswer());
            }
        }

    }

    public void onAnswerSubmitted()
    {

        string rawInput = answerInputField.text;

        if (!timer)
            return;

        if (currentQuestion.qt == Question.QuestionType.Molecule)
            return;

        if(currentQuestion.qt == Question.QuestionType.Double)
        {
            try
            {
                double d = double.Parse(rawInput.Trim());
                checkQuestion("", d);
            }catch(FormatException fe)
            {
                Debug.Log(fe.StackTrace);
                StartCoroutine(invalidInput());
            }

        }else if(currentQuestion.qt == Question.QuestionType.String){
            checkQuestion(rawInput.Trim(),0);
        }

    }

    public void onMoleculeSubmitted(Molecule m)
    {

        if (currentQuestion != null)
        {
            if (currentQuestion.qt == Question.QuestionType.Molecule)
            {
                questionSubmitPanelMoleculeText.text = m.getFormula();

                isSubmittedCorrect = false;

                submittedIncorrectReason = doesQuestionMoleculeMatchAndReason(m, currentQuestion.qm);

                if (submittedIncorrectReason.Length == 0)
                    isSubmittedCorrect = true;


                manager.bondManager.removeMolecule(m);
                Destroy(manager.draggingObjectMolecule);
                manager.draggingObjectMolecule = null;
                manager.selectingMolecule = null;
                manager.updateMoleculeList();
                manager.updateSelectedText();

                checkQuestion("", 0);

            }
        }

    }

    public IEnumerator waitForIENAndStartTimer(IEnumerator ien)
    {
        yield return StartCoroutine(ien);
        if (!tutorialMode)
            timer = true;
    }

    public IEnumerator waitForIENAndSetFormulaButtonInteractable(IEnumerator ien)
    {
        yield return StartCoroutine(ien);

        if(!isFormulaHintUsed)
            formulaButton.interactable = true;
    }

    public void newQuestion()
    {

        if (questions.Count == 0)
        {
            questionsFinished();
        }
        else
        {

            System.Random random = new System.Random();

            int index = random.Next(questions.Count);
            Question q = questions[index];
            this.setQuestion(q);
        }
    }

    public void questionsFinished()
    {
        if (leaderboardManager.currentUser.Points < points)
            leaderboardManager.SetCurrentUserPoints(points);

        manager.returnToMainMenu();
    }

    IEnumerator timeIsUp()
    {
        questionText.text = "Zaman bitti.";
        yield return StartCoroutine(revealText());
        yield return new WaitForSeconds(2);
        newQuestion();
    }

    IEnumerator rightAnswer()
    {
        manager.deselectMolecule();
        manager.setSelectingType(Manager.SelectingType.ATOM);

        manager.audioManager.correct();

        timer = false;
        
        addPoints(currentQuestion.calculatePoints(timeleft));

        this.setQuestion(null);

        questionText.text = "Doğru cevap";

        yield return StartCoroutine(revealText());

        yield return new WaitForSeconds(1);

        questionText.text = "";

        if (!tutorialMode)
            newQuestion();
        else
        {
            manager.tutorialManager.state++;
            manager.tutorialManager.doTutorial();
        }
    }

    IEnumerator wrongAnswer()
    {

        manager.audioManager.wrong();

        bool cacheTimer = timer;

        timer = false;

        yield return StartCoroutine(revealText());

        yield return new WaitForSeconds(2);

        if (timer != true)
            timer = cacheTimer;

        displayCurrentQuestion();
    }

    IEnumerator revealText()
    {
        finishedWriting = false;
        questionText.maxVisibleCharacters = 0;
        yield return new WaitForSeconds(0.01f);

        int totalVisibleCharacters = questionText.textInfo.characterCount;
        int counter = 0;

        int visibleCount = 0;


        while (visibleCount < totalVisibleCharacters)
        {

            visibleCount = counter % (totalVisibleCharacters + 1);

            questionText.maxVisibleCharacters = visibleCount;

            counter += 1;

            yield return new WaitForSeconds(0.03f);

        }

        finishedWriting = true;
    }

    IEnumerator invalidInput()
    {
        invalidInputText.gameObject.SetActive(true);
        yield return new WaitForSeconds(2);
        invalidInputText.gameObject.SetActive(false);
    }

    public void setPoints(int points_)
    {
        this.points = points_;
        pointsText.text = "Puan " + points;
    }

    public void addPoints(int points_)
    {
        this.points += points_;
        pointsText.text = "Puan " + points;
    }

    public string doesQuestionMoleculeMatchAndReason(Molecule m, QuestionMolecule qm)//Eşleşiyor ise -> ""
    {

        bool match = doesQuestionMoleculeMatch(m,qm);

        if (match)
            return "";
        else
        {

            //Farklı atom kontrolü

            List<Atom> typeList = new List<Atom>(m.atoms);

            foreach(QuestionAtom qa in qm.atomList)
            {
                foreach(Atom a in m.atoms)
                {
                    if (a.type.Equals(qa.atomType))
                    {
                        typeList.Remove(a);
                        
                    }
                }
                
            }

            if (typeList.Count == 1)
                return typeList[0].name + " atomu istediğim molekülde yok.";
            else if(typeList.Count > 1)
            {

                string mmessage = "";

                for(int i = 0; i < typeList.Count; i++) 
                {

                    Atom a = typeList[i];
                    
                    if(i == (typeList.Count - 1))
                    {
                        mmessage += a.name;
                    }else if(i == (typeList.Count - 2))
                    {
                        mmessage += a.name + " ve ";
                    }else
                    {
                        mmessage += a.name + ", ";
                    }

                }

                mmessage += " atomları istediğim molekülde yok.";
                return mmessage;
            }

            //Farklı atom kontrolü bitti.

            //Yeteri kadar atom kontrolü

            Dictionary<Atom.AtomType, int> wantedDictionary = new Dictionary<Atom.AtomType, int>();
            Dictionary<Atom.AtomType, int> currentDictionary = new Dictionary<Atom.AtomType, int>();
            Dictionary<Atom.AtomType, int> neededDictionary = new Dictionary<Atom.AtomType, int>();

            foreach(QuestionAtom qa in qm.atomList)
            {
                if (!wantedDictionary.ContainsKey(qa.atomType))
                {
                    wantedDictionary[qa.atomType] = 1;
                }
                else
                {
                    wantedDictionary[qa.atomType] = wantedDictionary[qa.atomType] + 1;
                }
            }

            foreach (Atom a in m.atoms)
            {
                if (!currentDictionary.ContainsKey(a.type))
                {
                    currentDictionary[a.type] = 1;
                }
                else
                {
                    currentDictionary[a.type] = currentDictionary[a.type] + 1;
                }
            }

            List<Atom.AtomType> neededAtomTypes = new List<Atom.AtomType>();

            foreach (Atom.AtomType at in wantedDictionary.Keys)
            {

                int wantedAmount = wantedDictionary[at];

                if (currentDictionary.ContainsKey(at))
                {

                    int currentAmount = currentDictionary[at];

                    neededDictionary[at] = wantedAmount - currentAmount;
                }
                else
                {
                    neededAtomTypes.Add(at);
                }
            }


            if (neededAtomTypes.Count > 0)
            {
                if (neededAtomTypes.Count == 1)
                    return "İstediğim molekülde " + Atom.GetInfo(neededAtomTypes[0]).atomName + " atomu da var.";
                else if (neededAtomTypes.Count > 1)
                {

                    string mmessage = "";

                    for (int i = 0; i < neededAtomTypes.Count; i++)
                    {

                        Atom.AtomType a = neededAtomTypes[i];

                        if (i == (neededAtomTypes.Count - 1))
                        {
                            mmessage += Atom.GetInfo(a).atomName;
                        }
                        else if (i == (neededAtomTypes.Count - 2))
                        {
                            mmessage += Atom.GetInfo(a).atomName + " ve ";
                        }
                        else
                        {
                            mmessage += Atom.GetInfo(a).atomName + ", ";
                        }

                    }

                    mmessage += " atomları da var.";
                    return "İstediğim molekülde " + mmessage;

                }
            }


            string message = "";

            foreach (Atom.AtomType at in neededDictionary.Keys)
            {
                int neededAmount = neededDictionary[at];
                if (neededAmount != 0 && neededAmount > 0) {
                    message += CBTextUtils.getTurkishNumberNames(neededAmount) +" tane daha fazla " + Atom.GetInfo(at).atomName.ToLower()+", ";
                }else if(neededAmount != 0)
                {
                    message += CBTextUtils.getTurkishNumberNames(-neededAmount) + " tane daha az " + Atom.GetInfo(at).atomName.ToLower() + ", ";
                }
            }
            Debug.Log("A");

            if (message.Length > 0)
            {

                message = message.Substring(0, message.Length - 2);

                message += " atomu var.";
                return "İstediğim molekülde " + message;
            }

            //Yeteri kadar atom kontrolü bitti.

            //Oktet dublet testi

            List<Atom> notCompleted = new List<Atom>();

            foreach(Atom a in m.atoms)
            {

                if (!a.areElectronsSatisfied())
                {
                    notCompleted.Add(a);
                }
                    
            }

            if(notCompleted.Count > 0)
            {
         /*       string message_ = "";

                foreach(Atom a in notCompleted)
                {
                    message_ += a.getInfo().atomName + " " + a.getLastOrbitName().ToLower() + " kuralına, ";
                }

                message_ = message_.Substring(0, message_.Length - 2);

                message_ += " uymuyor.";*/


                if(notCompleted.Count == 1)
                    return "Kararlı yapısına ulaşmamış bir atom var."; 
                else
                    return "Kararlı yapılarına ulaşmamış atomlar var.";

            }


            //


        }

        return "Lewis yapısı yanlış.";
    }

    public bool doesQuestionMoleculeMatch(Molecule m, QuestionMolecule qm)
    {

        if(!m.atoms.Count.Equals(qm.atomList.Count))
        {
            return false;
        }

        if(m.boundElectronPairs.Count != qm.boundElectronPairs)
        {
            return false;
        }

        if(m.getCharge() != qm.formalCharge)
        {
            return false;
        }

        List<Atom> l = new List<Atom>(m.atoms);

        foreach (QuestionAtom qa in qm.atomList)
        {

            foreach (Atom a in m.atoms)
            {

                if(qa.atomType == a.type)
                {

                    if (qa.electronCount == a.electronCount)
                    {

                        if (qa.formalCharge == a.getFormalCharge())
                        {

                            int boundElectronCount = 0;
                            foreach(Electron e in a.getAvailableElectrons())
                            {
                                if(e.electronType.Equals(Electron.ElectronType.BOUND) && e.boundTo != null)
                                {
                                    boundElectronCount++;
                                }
                            }

                            if (qa.boundElectrons == boundElectronCount)
                            {

                                List<Atom.AtomType> tt = new List<Atom.AtomType>(qa.boundTo);

                                foreach(Atom aa in manager.bondManager.getBoundAtoms(a))
                                {
                                    foreach (Atom.AtomType at in qa.boundTo)
                                    {
                                        if (at.Equals(aa.type))
                                        {
                                            tt.Remove(at);
                                            break;
                                        }
                                    }
                                }

                                l.Remove(a);
                            }


                        }
                    }
                }

            }

        }

        if (l.Count > 0)
            return false;
        else
            return true;

    }


}

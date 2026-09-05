using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class QuestionData
{
    public string id;
    public string section;
    public string question;

    public string choiceA;
    public string choiceB;

    public string choiceATarget;
    public string choiceBTarget;
}

public class QuizManager : MonoBehaviour
{
    [Header("CSV")]
    [SerializeField] private TextAsset csvFile;

    [Header("Question UI")]
    [SerializeField] private TMP_Text sectionText;
    [SerializeField] private TMP_Text questionNumberText;
    [SerializeField] private TMP_Text questionText;

    [Header("Choice UI")]
    [SerializeField] private Button buttonA;
    [SerializeField] private Button buttonB;

    [SerializeField] private TMP_Text textA;
    [SerializeField] private TMP_Text textB;

    [Header("Navigation UI")]
    [SerializeField] private Button nextButton;

    [Header("Answered Sprite")]
    [SerializeField] private Sprite answeredChoice;

    [Header("Progress Bar")]
    [SerializeField] private Image[] progressImages;
    [SerializeField] private Sprite emptyProgressSprite;
    [SerializeField] private Sprite answeredProgressSprite;

    [Header("Result Reference")]
    [SerializeField] private GameObject quizCanvas;
    [SerializeField] private GameObject resultCanvas;
    [SerializeField] private ResultManager resultManager;

    [Header("Question Settings")]
    [SerializeField] private int questionsPerRound = 5;
    [SerializeField] private float autoAdvanceDelay = 0.25f;

    private List<QuestionData> questions = new List<QuestionData>();
    private int currentQuestion = 0;
    
    private List<int> selectedChoiceIndices = new List<int>();
    private List<string> answers = new List<string>();

    private Image buttonAImage;
    private Image buttonBImage;
    private Sprite originalSpriteA;
    private Sprite originalSpriteB;
    private CanvasGroup nextButtonCanvasGroup;

    private bool isTransitioning = false; 

    private void Awake()
    {
        buttonAImage = buttonA.GetComponent<Image>();
        buttonBImage = buttonB.GetComponent<Image>();

        originalSpriteA = buttonAImage.sprite;
        originalSpriteB = buttonBImage.sprite;

        // ปิด Navigation เพื่อป้องกันการส่ง Focus ซ้อนทับใน Unity UI
        Navigation navNone = new Navigation { mode = Navigation.Mode.None };
        buttonA.navigation = navNone;
        buttonB.navigation = navNone;

        if (nextButton != null)
        {
            nextButton.navigation = navNone;
            nextButton.interactable = true; // เปิดไว้เสมอเพื่อไม่ให้สี Disabled Color (สีเทา) ทำงาน

            nextButtonCanvasGroup = nextButton.GetComponent<CanvasGroup>();
            if (nextButtonCanvasGroup == null)
            {
                nextButtonCanvasGroup = nextButton.gameObject.AddComponent<CanvasGroup>();
            }
        }
    }

    private void Start()
    {
        LoadCSV();

        buttonA.onClick.RemoveAllListeners();
        buttonB.onClick.RemoveAllListeners();
        buttonA.onClick.AddListener(() => SelectAnswer(1));
        buttonB.onClick.AddListener(() => SelectAnswer(2));

        if (nextButton != null)
        {
            nextButton.onClick.RemoveAllListeners();
            nextButton.onClick.AddListener(NextQuestion);
        }

        ShowQuestion();
    }

    // =========================================================
    // LOAD CSV 
    // =========================================================
    private void LoadCSV()
    {
        if (csvFile == null)
        {
            Debug.LogError("ยังไม่ได้ลากไฟล์ CSV ใส่ในช่อง Csv File ของ QuizManager!");
            return;
        }

        questions.Clear();

        string[] lines = csvFile.text.Split(
            new[] { '\r', '\n' },
            StringSplitOptions.RemoveEmptyEntries
        );

        for (int i = 1; i < lines.Length; i++)
        {
            string line = lines[i].Trim();
            if (string.IsNullOrEmpty(line)) continue;

            string[] data = line.Split(',');

            if (data.Length < 7)
            {
                Debug.LogWarning($"แถวที่ {i + 1} มีข้อมูลไม่ครบ 7 ช่อง: {line}");
                continue;
            }

            questions.Add(new QuestionData
            {
                id = data[0].Trim(),
                section = data[1].Trim(),
                question = data[2].Trim(),
                choiceA = data[3].Trim(),
                choiceB = data[4].Trim(),
                choiceATarget = data[5].Trim(),
                choiceBTarget = data[6].Trim()
            });
        }

        Debug.Log($"โหลดคำถามสำเร็จทั้งหมด: {questions.Count} ข้อ");
    }

    // =========================================================
    // SHOW QUESTION
    // =========================================================
    private void ShowQuestion()
    {
        StopAllCoroutines();

        if (currentQuestion >= questions.Count)
        {
            FinishQuiz();
            return;
        }

        QuestionData q = questions[currentQuestion];

        if (sectionText != null) sectionText.text = q.section;
        if (questionNumberText != null) questionNumberText.text = q.id;
        if (questionText != null) questionText.text = q.question;

        if (textA != null) textA.text = q.choiceA;
        if (textB != null) textB.text = q.choiceB;

        // เช็คว่าข้อนี้เคยตอบไปแล้วหรือไม่ (เช่น ตอนกดย้อนกลับมาดู)
        bool hasAnswered = (currentQuestion < selectedChoiceIndices.Count && selectedChoiceIndices[currentQuestion] != 0);

        if (hasAnswered)
        {
            int savedChoice = selectedChoiceIndices[currentQuestion];
            if (buttonAImage != null) SetAnsweredButton(buttonAImage, savedChoice == 1);
            if (buttonBImage != null) SetAnsweredButton(buttonBImage, savedChoice == 2);
        }
        else
        {
            ResetChoiceButton();
        }

        // คุมปุ่ม Next ให้อยู่ที่เดิมเสมอ แค่ปรับความโปร่งใสและการบล็อกคลิก
        UpdateNextButtonState(hasAnswered);

        UpdateProgressBar();
        isTransitioning = false;
    }

    // =========================================================
    // ANSWER
    // =========================================================
private void SelectAnswer(int answer)
    {
        if (isTransitioning) return;

        QuestionData q = questions[currentQuestion];
        string target = (answer == 1) ? q.choiceATarget : q.choiceBTarget;

        if (buttonAImage != null) SetAnsweredButton(buttonAImage, answer == 1);
        if (buttonBImage != null) SetAnsweredButton(buttonBImage, answer == 2);

        bool isReviewingOldQuestion = (currentQuestion < selectedChoiceIndices.Count);

        if (isReviewingOldQuestion)
        {
            selectedChoiceIndices[currentQuestion] = answer;
            answers[currentQuestion] = target;
        }
        else
        {
            selectedChoiceIndices.Add(answer);
            answers.Add(target);
        }

        UpdateProgressBar();

        // เปิดปุ่ม Next ให้พร้อมกด
        UpdateNextButtonState(true);

        // ถ้าเป็นข้อสุดท้ายให้อยู่หน้านี้ต่อ รอให้ผู้เล่นกดปุ่มถัดไปเอง
        bool isLastQuestion = (currentQuestion == questions.Count - 1);
        if (isLastQuestion)
        {
            return; 
        }

        // ถ้ายังไม่ใช่ข้อสุดท้าย ให้เลื่อนข้ออัตโนมัติตามเดิม
        isTransitioning = true;
        StopAllCoroutines();
        StartCoroutine(AutoAdvanceRoutine());
    }

    private IEnumerator AutoAdvanceRoutine()
    {
        yield return new WaitForSeconds(autoAdvanceDelay);
        currentQuestion++;
        ShowQuestion();
    }

    // =========================================================
    // BUTTON STATE / SPRITE
    // =========================================================
    private void SetAnsweredButton(Image image, bool answered)
    {
        if (image == buttonAImage)
        {
            image.sprite = answered ? answeredChoice : originalSpriteA;
        }
        else if (image == buttonBImage)
        {
            image.sprite = answered ? answeredChoice : originalSpriteB;
        }

        image.color = Color.white;
    }

    private void ResetChoiceButton()
    {
        if (buttonAImage != null)
        {
            buttonAImage.sprite = originalSpriteA;
            buttonAImage.color = Color.white;
        }

        if (buttonBImage != null)
        {
            buttonBImage.sprite = originalSpriteB;
            buttonBImage.color = Color.white;
        }
    }

    private void UpdateNextButtonState(bool canClick)
    {
        if (nextButton == null) return;

        // ไม่ปิด GameObject เพื่อให้ Layout คงรูปเดิมเสมอ ไม่เด้ง
        nextButton.gameObject.SetActive(true);
        nextButton.interactable = true;

        if (nextButtonCanvasGroup != null)
        {
            // ถ้าตอบแล้ว: สว่างเต็ม 100% และคลิกได้
            // ถ้ายังไม่ตอบ: จางลงเป็นสีเดิมแบบโปร่งแสง (Alpha 0.4) ไม่กลายเป็นสีเทา และคลิกไม่โดน
            nextButtonCanvasGroup.alpha = canClick ? 1.0f : 0.4f;
            nextButtonCanvasGroup.blocksRaycasts = canClick;
        }
    }

    // =========================================================
    // NEXT
    // =========================================================
    public void NextQuestion()
    {
        if (isTransitioning) return;

        // ต้องตอบข้อนี้ก่อนถึงจะกดไปต่อได้
        bool hasAnswered = (currentQuestion < selectedChoiceIndices.Count && selectedChoiceIndices[currentQuestion] != 0);
        if (!hasAnswered) return;

        StopAllCoroutines();

        // ถ้าเป็นข้อสุดท้ายแล้วกดถัดไป ให้ไปหน้าผลลัพธ์เลย!
        if (currentQuestion >= questions.Count - 1)
        {
            FinishQuiz();
            return;
        }

        currentQuestion++;
        ShowQuestion();
    }

    // =========================================================
    // BACK
    // =========================================================
    public void PreviousQuestion()
    {
        if (currentQuestion <= 0) return;

        StopAllCoroutines();
        currentQuestion--;
        ShowQuestion();
    }

    // =========================================================
    // PROGRESS BAR
    // =========================================================
    private void UpdateProgressBar()
    {
        if (progressImages == null || progressImages.Length == 0) return;

        int indexInRound = currentQuestion % questionsPerRound;
        bool currentQuestionAnswered = (currentQuestion < selectedChoiceIndices.Count && selectedChoiceIndices[currentQuestion] != 0);
        int filledCount = indexInRound + (currentQuestionAnswered ? 1 : 0);

        for (int i = 0; i < progressImages.Length; i++)
        {
            if (progressImages[i] != null)
            {
                progressImages[i].sprite = (i < filledCount) ? answeredProgressSprite : emptyProgressSprite;
            }
        }
    }

    // =========================================================
    // FINISH
    // =========================================================
    private void FinishQuiz()
    {
        Debug.Log($"จบแบบทดสอบแล้ว! ตอบครบ {answers.Count} ข้อ");

        // ปิดหน้า QuizCanvas
        if (quizCanvas != null) quizCanvas.SetActive(false);

        // เปิดหน้า ResultCanvas
        if (resultCanvas != null) resultCanvas.SetActive(true);

        // ส่งคำตอบไปให้ ResultManager คำนวณ MBTI และแสดงผล
        if (resultManager != null)
        {
            resultManager.ShowResult(answers);
        }
    }
}
using System;
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

    [Header("Answered Sprite")]
    [SerializeField] private Sprite answeredChoice;

    [Header("Progress Bar")]
    [SerializeField] private Image[] progressImages;
    [SerializeField] private Sprite emptyProgressSprite;
    [SerializeField] private Sprite answeredProgressSprite;

    [Header("Question Settings")]
    [SerializeField] private int questionsPerRound = 5;

    private List<QuestionData> questions = new List<QuestionData>();
    private int currentQuestion = 0;
    private int currentAnswer = 0;
    private List<string> answers = new List<string>();

    private Image buttonAImage;
    private Image buttonBImage;
    private Sprite originalSpriteA;
    private Sprite originalSpriteB;

    private void Awake()
    {
        buttonAImage = buttonA.GetComponent<Image>();
        buttonBImage = buttonB.GetComponent<Image>();

        originalSpriteA = buttonAImage.sprite;
        originalSpriteB = buttonBImage.sprite;
    }

    private void Start()
    {
        Debug.Log(">>>working<<<");
        LoadCSV();

        buttonA.onClick.AddListener(() => SelectAnswer(1));
        buttonB.onClick.AddListener(() => SelectAnswer(2));

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

            // ตรวจสอบว่าคอลัมน์ครบ 7 ช่องจริงไหม ป้องกัน Index หลุด
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
        if (currentQuestion >= questions.Count)
        {
            FinishQuiz();
            return;
        }

        QuestionData q = questions[currentQuestion];

        sectionText.text = q.section;
        questionNumberText.text = q.id;
        questionText.text = q.question;

        textA.text = q.choiceA;
        textB.text = q.choiceB;

        currentAnswer = 0;
        ResetChoiceButton();
        UpdateProgressBar();
    }

    // =========================================================
    // ANSWER
    // =========================================================
    private void SelectAnswer(int answer)
    {
        Debug.Log("คลิกเลือกคำตอบ: " + answer);
        currentAnswer = answer;
        QuestionData q = questions[currentQuestion];
        string target;

        if (answer == 1)
        {
            target = q.choiceATarget;
            SetAnsweredButton(buttonAImage, true);
            SetAnsweredButton(buttonBImage, false);
        }
        else
        {
            target = q.choiceBTarget;
            SetAnsweredButton(buttonAImage, false);
            SetAnsweredButton(buttonBImage, true);
        }

        answers.Add(target);
        UpdateProgressBar();
    }

    // =========================================================
    // BUTTON COLOR / SPRITE
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
        buttonAImage.sprite = originalSpriteA;
        buttonBImage.sprite = originalSpriteB;

        buttonAImage.color = Color.white;
        buttonBImage.color = Color.white;
    }

    // =========================================================
    // NEXT
    // =========================================================
    public void NextQuestion()
    {
        if (currentAnswer == 0) return;

        currentQuestion++;

        if (currentQuestion % questionsPerRound == 0)
        {
            ResetProgressBar();
        }

        ShowQuestion();
    }

    // =========================================================
    // BACK
    // =========================================================
    public void PreviousQuestion()
    {
        if (currentQuestion <= 0) return;

        currentQuestion--;

        if (answers.Count > currentQuestion)
        {
            answers.RemoveAt(answers.Count - 1);
        }

        ShowQuestion();
    }

    // =========================================================
    // PROGRESS BAR
    // =========================================================
    private void UpdateProgressBar()
    {
        if (progressImages == null) return;

        int positionInRound = currentQuestion % questionsPerRound;

        for (int i = 0; i < progressImages.Length; i++)
        {
            progressImages[i].sprite = (i < positionInRound) ? answeredProgressSprite : emptyProgressSprite;
        }
    }

    private void ResetProgressBar()
    {
        if (progressImages == null) return;

        foreach (Image image in progressImages)
        {
            image.sprite = emptyProgressSprite;
        }
    }

    // =========================================================
    // FINISH
    // =========================================================
    private void FinishQuiz()
    {
        Debug.Log("จบแบบทดสอบแล้ว!");
    }
}
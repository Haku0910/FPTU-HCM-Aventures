using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class QuestionByNPCTeacher : MonoBehaviour
{
    [SerializeField] TMP_Text questionText;
    [SerializeField] Button[] answerButtons;
    private QuestionList questionList;
    private int currentQuestionIndex;
    private bool isAnswered;
    [SerializeField] GameObject gameContainer;
    [SerializeField] GameObject canvasGameQuestion;
    [SerializeField] GameObject canvasGameTutorial;
    [SerializeField] GameObject buttonYes;
    [SerializeField] GameObject buttonNo;
    [SerializeField] GameObject buttonMessage;
    [SerializeField] GameObject eventtrigger;
    [SerializeField] TMP_Text dialogText;
    [SerializeField] int lettersPerSecond;
    [SerializeField] List<string> lines;
    [SerializeField] GameObject dialogBox;
    [SerializeField] NPCQuestionAndAnswer npcQuestionAndAnswer;
    [SerializeField] private Image uiFill;
    [SerializeField] private TMP_Text uiText;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private GameObject textError;
    [SerializeField] GameObject checkActiveMissionSuccess;

    private bool Pause;
    private bool inCorrect;

    public int Duration;
    private double score = 100;
    private int remainingDuration;
    public float gameEndDelay = 2f;
    private int currentLineIndex = 0;
    void Start()
    {
        if (PlayerPrefs.GetInt("SuccessLineQuestion", 0) == 1)
        {
            if (checkActiveMissionSuccess.activeSelf == false)
            {
                StartCoroutine(TypeDialog2("Hãy nh?n vào nút Yes ?? hoàn thành nhi?m v? nhe!"));

                buttonYes.gameObject.SetActive(true);
                buttonNo.gameObject.SetActive(true);
            }
            else
            {
                gameContainer.SetActive(false);
            }

        }
        else
        {
            ShowDialog();

        }
    }

    IEnumerator GetQuestionsFromAPI()
    {
        string url = $"http://anhkiet-001-site1.htempurl.com/api/Answers/GetAnswers/{npcQuestionAndAnswer.major}";

        UnityWebRequest www = UnityWebRequest.Get(url);

        string authToken = PlayerPrefs.GetString("token");
        www.SetRequestHeader("Authorization", "Bearer " + authToken);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("L?i khi g?i API: " + www.error);
        }
        else
        {
            string jsonResponse = www.downloadHandler.text;
            questionList = JsonUtility.FromJson<QuestionList>(jsonResponse);
            if (questionList != null)
            {
                currentQuestionIndex = 0;
                isAnswered = false;
                remainingDuration = Duration;
                StartCoroutine(UpdateTimer());
            }
            else
            {
                StartCoroutine(TypeDialog2("Ban vui long doi trong giay lat!"));
                yield return new WaitForSeconds(4f);
                StartCoroutine(GetQuestionsFromAPI());
            }

        }
    }

    void DisplayCurrentQuestion()
    {
        if (currentQuestionIndex < questionList.data.Length)
        {
            var question = questionList.data[currentQuestionIndex];

            questionText.text = question.questionName;

            var answers = question.answerDtos;

            int maxAnswersToDisplay = Mathf.Min(answers.Length, 4);


            for (int i = 0; i < maxAnswersToDisplay; i++)
            {
                var answerButton = answerButtons[i];
                var answerText = answerButton.GetComponentInChildren<TMP_Text>();
                answerText.text = answers[i].answerName;

                int answerIndex = i;
                answerButton.onClick.RemoveAllListeners();

                if (!isAnswered)
                {
                    answerButton.onClick.AddListener(() => OnAnswerSelected(answerIndex));
                }

                answerButton.interactable = !isAnswered;
                answerButton.image.color = Color.white;
            }
        }
        else
        {
            questionText.text = "C?m ?n b?n ?ã tr? l?i câu h?i!";
            Pause = true;
            eventtrigger.SetActive(false);
            DisableAnswerButtons();
            npcQuestionAndAnswer.QuestionAndAnswer(score);
        }
    }

    void OnAnswerSelected(int answerIndex)
    {
        var question = questionList.data[currentQuestionIndex];
        var selectedAnswer = question.answerDtos[answerIndex];

        inCorrect = selectedAnswer.isRight;

        var answerButton = answerButtons[answerIndex];
        answerButton.image.color = inCorrect ? Color.green : Color.red;
        if (inCorrect)
        {
            // T?ng ?i?m n?u tr? l?i ?úng
            score += 0; // ?i?m có th? thay ??i theo mong mu?n
        }
        else
        {
            if (score == 0)
            {
                score = 0;
            }
            else
            {
                score -= 10;

            }
        }

        isAnswered = true;
        UpdateScoreUI();
        DisableAnswerButtons();
        StartCoroutine(NextQuestionDelay());
    }
    void UpdateScoreUI()
    {
        // C?p nh?t giao di?n hi?n th? ?i?m ? ?ây
        scoreText.text = "Score: " + score.ToString();
    }
    void DisableAnswerButtons()
    {
        foreach (var answerButton in answerButtons)
        {
            answerButton.interactable = false;
        }
    }

    IEnumerator NextQuestionDelay()
    {
        yield return new WaitForSeconds(gameEndDelay);
        currentQuestionIndex++;
        isAnswered = false;
        remainingDuration = Duration;
        StartCoroutine(UpdateTimer());
    }
    public void ShowDialog()
    {

        dialogBox.SetActive(true);
        StartCoroutine(TypeDialog2(lines[currentLineIndex]));
    }

    public IEnumerator TypeDialog2(string line)
    {
        dialogText.text = "";
        foreach (char letter in line.ToCharArray())
        {
            dialogText.text += letter;
            yield return new WaitForSeconds(1f / lettersPerSecond);
        }

        yield return new WaitForSeconds(1f);

        currentLineIndex++;

        if (currentLineIndex < lines.Count)
        {
            StartCoroutine(TypeDialog2(lines[currentLineIndex]));
        }
        else
        {
            buttonYes.gameObject.SetActive(true);
            buttonNo.gameObject.SetActive(true);
            PlayerPrefs.SetInt("SuccessLineQuestion", 1);
        }
    }

    public void Yes()
    {
        if (TaskManager.Instance.DetailBtn.gameObject.activeSelf == false)
        {
            canvasGameQuestion.SetActive(true);
            canvasGameTutorial.SetActive(false);
            UpdateScoreUI();
            StartCoroutine(GetQuestionsFromAPI());
        }
        else
        {
            textError.SetActive(true);
        }


    }
    public void No()
    {
        gameContainer.SetActive(false);
    }

    //Xu ly timer 
    private IEnumerator UpdateTimer()
    {
        DisplayCurrentQuestion();
        while (remainingDuration >= 0)
        {
            uiText.text = $"{remainingDuration / 60:00}:{remainingDuration % 60:00}";
            uiFill.fillAmount = Mathf.InverseLerp(0, Duration, remainingDuration);
            remainingDuration--;
            yield return new WaitForSeconds(1f);
        }
        OnEnd();
    }

    private void OnEnd()
    {
        DisableAnswerButtons();
        inCorrect = false;
        print("End");
        StartCoroutine(NextQuestionDelay());

    }

    public void CloseTextError()
    {
        textError.SetActive(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestionDialog : MonoBehaviour
{
    public List<QuestionAndAnswers> QnA;
    public GameObject[] options;
    private int currentQuestion;
    public TMP_Text QuestionTxt;
    public bool canAnswer = true;
    public GameObject LoadingScene, QuestionScene, BackGroundImage, Avatar;
    private string playerNickname;
    int totalQuestions = 0;
    private int score = 0;
    private int scorewrong = 0;
    public int rightScore = 100;
    public int wrongScore = 50;
    public static int scoreValue;

    private void Start()
    {
        totalQuestions = QnA.Count;
        generateQuestion();
    }

    public void correct()
    {
        score += 1;
        QnA.RemoveAt(currentQuestion);
        generateQuestion();
        Debug.Log("Score right " + score);
    }

    public void wrong()
    {
        scorewrong += 1;
        QnA.RemoveAt(currentQuestion);
        generateQuestion();
        Debug.Log("Score wrong " + scorewrong);
    }

    void SetAnswers()
    {
        if (currentQuestion < QnA.Count)
        {
            for (int i = 0; i < options.Length; i++)
            {
                options[i].GetComponent<AnswerScript>().isCorrect = false;
                options[i].transform.GetChild(0).GetComponent<TMP_Text>().text = QnA[currentQuestion].Answers[i];

                if (QnA[currentQuestion].CorrectAnswer == i + 1)
                {
                    options[i].GetComponent<AnswerScript>().isCorrect = true;
                }
            }
        }
        else
        {
            GameOver();
        }
    }

    public IEnumerator DelayedResetQuestion()
    {
        yield return new WaitForSeconds(0.1f);

        foreach (GameObject option in options)
        {
            Image buttonImage = option.GetComponent<Image>();
            buttonImage.color = Color.white;

            Button button = option.GetComponent<Button>();
            button.interactable = true;
            button.enabled = true;
            button.gameObject.SetActive(true);
        }

        generateQuestion();
    }

    private void generateQuestion()
    {
        if (QnA.Count > 0)
        {
            currentQuestion = Random.Range(0, QnA.Count);

            QuestionTxt.text = QnA[currentQuestion].Question;
            SetAnswers();
            canAnswer = true;

            for (int i = 0; i < options.Length; i++)
            {
                Button button = options[i].transform.GetComponent<Button>();
                button.image.color = button.colors.normalColor;
                button.interactable = true;
            }
        }
        else
        {
            Debug.Log("Out of Questions");
            GameOver();
        }
    }

    public void GameOver()
    {
        if (score != 0)
        {
            Debug.Log("Score right game over: " + score);
        }
        if (scorewrong != 0)
        {
            Debug.Log("Score wrong game over: " + scorewrong);
        }

        QuestionScene.SetActive(false);
        Avatar.SetActive(false);
        BackGroundImage.SetActive(false);
        LoadingScene.SetActive(true);
        scoreValue = score * rightScore + scorewrong * wrongScore;
    }
}

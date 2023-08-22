using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnswerScript : MonoBehaviour
{
    public bool isCorrect = false;
    public QuestionDialog questionManager;
    public Color startColor;
    public Button button;

    private void Start()
    {
        button = GetComponent<Button>();
        button.image.color = button.colors.normalColor;
    }

    public void Answer()
    {
        if (!questionManager.canAnswer)
        {
            button.image.color = startColor;
            button.interactable = true;

            // Check if answering is allowed
            return;
        }

        questionManager.canAnswer = false;

        if (isCorrect)
        {
            Debug.Log("Correct Answer");
            questionManager.correct();
            button.image.color = Color.green;
        }
        else
        {
            Debug.Log("Wrong Answer");
            questionManager.wrong();
            button.image.color = Color.red;
        }

        if (button.gameObject.activeInHierarchy)
        {
            StartCoroutine(questionManager.DelayedResetQuestion());
        }
    }
}

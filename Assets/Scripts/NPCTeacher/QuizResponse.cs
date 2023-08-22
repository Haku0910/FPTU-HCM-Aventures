using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuizResponse
{
    public string questionName;
    public AnswerData[] answerDtos;
    public bool isRight;
}


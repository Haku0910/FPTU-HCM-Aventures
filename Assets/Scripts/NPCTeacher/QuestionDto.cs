using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestionDto
{
    public string questionName;
    public AnswerData[] answerDtos;
    public int correctAnswerIndex;

}
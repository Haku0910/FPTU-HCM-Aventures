using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogCheckIn : MonoBehaviour
{
    [SerializeField] GameObject dialogBox;
    [SerializeField] GameObject ToActive;
    [SerializeField] GameObject EventTrigger;
    [SerializeField] TMP_Text dialogText;
    [SerializeField] Button checkIn;
    [SerializeField] Button closeBtn;
    [SerializeField] GameObject checkActiveMissionSuccess;
    [SerializeField] int lettersPerSecond = 45;
    [SerializeField] List<string> lines;
    private int currentLineIndex = 0;

    private void Start()
    {
        ShowDialog2();
    }
    public void ShowDialog2()
    {
        if(PlayerPrefs.GetInt("SuccessLine",0) == 1)
        {
            if(checkActiveMissionSuccess.activeSelf == false)
            {
                StartCoroutine(TypeDialog2("Hãy nh?n vào nút CHECKIN ?? hoàn thành nhi?m v? nhe!"));

                checkIn.gameObject.SetActive(true);
                closeBtn.gameObject.SetActive(true);
            }
            else
            {
                EventTrigger.SetActive(false);
                ToActive.SetActive(false);
            }
           
        }
        else
        {
            dialogBox.SetActive(true);
            StartCoroutine(TypeDialog2(lines[currentLineIndex]));
        }
      
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
            PlayerPrefs.SetInt("SuccessLine", 1);
            checkIn.gameObject.SetActive(true);
            closeBtn.gameObject.SetActive(true);
        }
    }
    
    public void Recover()
    {
        ToActive.SetActive(false);
    }
}

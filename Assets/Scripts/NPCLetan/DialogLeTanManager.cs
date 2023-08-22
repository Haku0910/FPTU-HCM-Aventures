using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DialogLeTanManager : MonoBehaviour
{
    [SerializeField] GameObject dialogBox;
    [SerializeField] Text dialogText;
    [SerializeField] int lettersPerSecond;
    [SerializeField] List<string> lines;

    private int currentLineIndex = 0;

    private void Start()
    {
        ShowDialog();
    }

    public void ShowDialog()
    {
        dialogBox.SetActive(true);
        StartCoroutine(TypeDialog(lines[currentLineIndex]));
    }

    public IEnumerator TypeDialog(string line)
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
            StartCoroutine(TypeDialog(lines[currentLineIndex]));
        }
        else
        {
            EndDialog();
        }
    }

    private void EndDialog()
    {
        dialogBox.SetActive(false);
        // G?i các hành ??ng k?t thúc h?i tho?i ? ?ây (n?u có)
        SceneManager.LoadScene("Playground1");
    }
}

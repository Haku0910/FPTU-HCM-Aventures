using System.Collections;
using TMPro;
using UnityEngine;

public class GameCoins : MonoBehaviour
{

    [SerializeField] TMP_Text allCoinsUIText;

    private double Coins = 0.0;
    private Player player;
    private bool isDataLoaded = false;

    void Start()
    {
        Debug.Log(ManageButton.Instance.point);
        StartCoroutine(GetDataCoroutine());
    }

    private IEnumerator GetDataCoroutine()
    {
        yield return StartCoroutine(NamePrefab.Instance.CheckPlayer(OnPlayerIdReceived));
        Debug.Log("Points: " + NamePrefab.Instance.currentPoint);
        Coins = NamePrefab.Instance.currentPoint;
        UpdateAllCoinsUIText();
    }

    private void OnPlayerIdReceived(Player playerData)
    {
        player = playerData;
    }


    public void UseCoins(double amount)
    {
        Debug.Log("amount" + amount);
        Coins -= amount;

        if (player != null)
        {
            Debug.Log("Point player" + player.totalPoint);
            Debug.Log("Vao day");
            player.totalPoint = player.totalPoint - amount;
            player.isplayer = true;
            StartCoroutine(NamePrefab.Instance.UpdatePlayer(player));
            allCoinsUIText.text = player.totalPoint.ToString();
            ManageButton.Instance.UpdateToCoint(player.totalPoint);
        }
    }

    public void CheatCoins(double amount)
    {
        Debug.Log("amount" + amount);
        Coins -= amount;
        if (player != null)
        {
            player.totalPoint = player.totalPoint - amount;
            player.isplayer = true;
            StartCoroutine(NamePrefab.Instance.UpdatePlayer(player));
            allCoinsUIText.text = player.totalPoint.ToString();
            ManageButton.Instance.UpdateToCoint(player.totalPoint);
        }
    }

    public bool HasEnoughCoins(double amount)
    {
        return (Coins >= amount);
    }

    public void UpdateAllCoinsUIText()
    {

        allCoinsUIText.text = ManageButton.Instance.point.ToString();
    }
}

using UnityEngine;
using UnityEngine.UI;

public class LocationName : MonoBehaviour
{
    [SerializeField] private Text textSpace;

    private void Start()
    {
        textSpace.text = gameObject.name;
    }
}

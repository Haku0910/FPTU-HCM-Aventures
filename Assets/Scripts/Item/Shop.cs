using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Item", menuName = "Shop/Item")]

public class Shop : ScriptableObject
{
    public string Id;
    public Sprite Image;
    public double Price;
    public string Name;
    public int Quanity;
    public bool IsPurchased = false;
}

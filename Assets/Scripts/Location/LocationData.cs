using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LocationData 
{
    public Guid id;
    public double x;
    public double y;
    public double z;
    public string locationName;
    public string status;

    public static implicit operator List<object>(LocationData v)
    {
        throw new NotImplementedException();
    }
}

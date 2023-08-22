using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCHeadLookAt : MonoBehaviour
{
    //[SerializeField] private Rig rig;
    [SerializeField] private Transform headLookAtTransform;

    private bool isLookingAtPosition;

    /*private void Update()
    {
        float targetWeight = isLookingAtPosition ? 1f : 0f;
        float lerpSpeed = 2f;
        //rig.weight = Mathf.Lerp(rig.weight, targetWeight, Time.deltaTime * lerpSpeed);
    }*/

    public void LookAtPosition(Vector3 lookAtPosition)
    {
        isLookingAtPosition = true; // ?ánh d?u r?ng NPC ?ang nhìn vào m?t v? trí c? th?
        headLookAtTransform.position = lookAtPosition; // C?p nh?t v? trí c?a transform "headLookAtTransform" ?? nhìn vào v? trí ???c ch? ??nh
    }

}


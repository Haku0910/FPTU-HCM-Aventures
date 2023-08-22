using Photon.Pun;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class ManageComponent : MonoBehaviourPun
{
    [SerializeField] private Animator animator;
    [SerializeField] private CharacterMoving characterMoving;
    [SerializeField] private PlayerInput playerInput;

    void Start()
    {
        if (photonView.IsMine)
        {
            animator.enabled = true;
            characterMoving.enabled = true;
            playerInput.enabled = true;
        }
    }
}
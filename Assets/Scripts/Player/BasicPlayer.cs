using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicPlayer : MonoBehaviour
{
    [SerializeField] private new GameObject camera;

    private Animator animator;
    private PhotonView photonView;

    private readonly static int WALK_ANIM = Animator.StringToHash("isWalk");

    private void Awake()
    {
        animator = GetComponent<Animator>();
        photonView = GetComponent<PhotonView>();

        if (photonView.IsMine) camera.SetActive(true);
    }

    private void Update()
    {
        if (photonView.IsMine)
        {
            var x = Input.GetAxis("Horizontal") * Time.deltaTime * 150.0f;
            var z = Input.GetAxis("Vertical") * Time.deltaTime * 3.0f;

            transform.Rotate(0, x, 0);
            transform.Translate(0, 0, z);

            animator.SetBool(WALK_ANIM, z != 0);
        }
    }
}

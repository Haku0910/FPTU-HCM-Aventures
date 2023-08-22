using UnityEngine;
using UnityEngine.AI;

public class SecurityAI2 : MonoBehaviour
{
    public NavMeshAgent agent;
    public Animator animator;

    public GameObject PATH;
    public Transform[] PathPoint;

    public float minDistance = 10f;


    public int index = 0;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        PathPoint = new Transform[PATH.transform.childCount];
        for (int i = 0; i < PathPoint.Length; i++)
        {
            PathPoint[i] = PATH.transform.GetChild(i);
        }

    }

    // Update is called once per frame
    void Update()
    {
        roam();
    }

    void roam()
    {
        if (Vector3.Distance(transform.position, PathPoint[index].position) < minDistance)
        {
            if (index >= 0 && index < PathPoint.Length - 1)
            {
                index += 1;
            }
            else
            {
                index = 0;
            }
        }
        agent.SetDestination(PathPoint[index].position);
        animator.SetFloat("vertical", !agent.isStopped ? 1 : 0);
    }
}

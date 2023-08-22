using UnityEngine;
using UnityEngine.AI;

public class SecurityAI : MonoBehaviour
{
    GameObject playAI;
    NavMeshAgent agent;
    [SerializeField] private LayerMask layerMask, BotMask;

    Vector3 despoint;
    bool walkPointSet;
    [SerializeField] float range;

    // Start is called before the first frame update
    void Start()
    {
        playAI = GameObject.Find("FPT_Student_Model");
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        patrol();
    }

    void patrol()
    {
        if (!walkPointSet) SearchForDest();
        if (walkPointSet)
        {
            agent.SetDestination(despoint);
        }
        if (Vector3.Distance(transform.position, despoint) < 10) walkPointSet = false;

    }

    void SearchForDest()
    {
        float z = Random.Range(-range, range);
        float x = Random.Range(-range, range);

        despoint = new Vector3(transform.position.x + x, transform.position.y, transform.position.z + z);

        if (Physics.Raycast(despoint, Vector3.down, layerMask))
        {
            walkPointSet = true;
        }
    }
}

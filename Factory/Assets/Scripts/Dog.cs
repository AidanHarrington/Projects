using System.Collections;
using System.Collections.Generic;
using System.Security;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Dog : MonoBehaviour
{
    public float lookRadius = 100f;
    public bool jumped;
    public Text cuddleDog;

    [SerializeField] private Animator dog = null;

    Transform target;
    NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        target = PlayerManager.instance.player.transform;
        agent = GetComponent<NavMeshAgent>();
        jumped = false;
        cuddleDog.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(target.position, transform.position);

        if (distance <= lookRadius)
        {
            agent.SetDestination(target.position);

            if (distance > agent.stoppingDistance && !jumped)
            {
                cuddleDog.enabled = false;
                dog.SetInteger("Walk", 1);
                FaceTarget();
            }
            else if (distance <= agent.stoppingDistance || jumped)
            {
                dog.SetInteger("Walk", 0);

                cuddleDog.enabled = true;

                if (Input.GetKeyDown(KeyCode.Q))
                {
                    jumped = true;
                    agent.isStopped = true;
                    StartCoroutine(jumpAnim());

                }
            }

        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }

    void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    IEnumerator jumpAnim()
    {
        dog.SetTrigger("jump");
        yield return new WaitForSeconds(3f);
        agent.isStopped = false;
        jumped = false;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    //movement
    private Rigidbody rb;
    public List<GameObject> hovers;
    public GameObject prop, cm;
    public Transform cam;

    public float turnSmoothTime = 0.1f;
    private float turnSmoothVelocity;

    //other
    public GameObject enemy, enemy2, enemy3;
    public float dist, dist2, dist3, maxHealth, health;

    public static bool destroyed = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = cm.transform.localPosition;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        health = maxHealth;
    }

    private void Update()
    {
        inRange();

        if (health <= 0)
        {
            Die();
        }
    }

    private void FixedUpdate()
    {
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(0f, 0f, vertical).normalized;

        float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
        transform.rotation = Quaternion.Euler(0f, angle, 0f);

        rb.AddForceAtPosition(Time.deltaTime * transform.TransformDirection(Vector3.forward) * Input.GetAxis("Vertical") * 400, prop.transform.position);
        Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
        rb.AddTorque(Time.deltaTime * moveDir * 25);

        foreach (GameObject hover in hovers)
        {
            RaycastHit hit;
            if (Physics.Raycast(hover.transform.position, transform.TransformDirection(Vector3.down), out hit, 1.5f))
            {
                rb.AddForceAtPosition(Time.deltaTime * transform.TransformDirection(Vector3.up) * Mathf.Pow(3f - hit.distance, 2) / 3f * 150f, hover.transform.position);
            }
            rb.AddForce(-Time.deltaTime * transform.TransformDirection(Vector3.right) * transform.InverseTransformVector(rb.velocity).x * 5f);
        }
    }

    void Die()
    {
        SceneManager.LoadScene("Lose");
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void inRange()
    {
        if (Enemy.isDead)
        {
            dist = 0;
            dist2 = 0;
            dist3 = 0;
        }
        else
        {
            dist = Vector3.Distance(transform.position, enemy.transform.position);
            dist2 = Vector3.Distance(transform.position, enemy2.transform.position);
            dist3 = Vector3.Distance(transform.position, enemy3.transform.position);

            if (dist < 100f)
            {
                Enemy.inRange = true;
                print("in range");
            }
            else if (dist2 < 100f)
            {
                Enemy.inRange = true;
            }
            else if (dist3 < 100f)
            {
                Enemy.inRange = true;
            }
        }

    }
}

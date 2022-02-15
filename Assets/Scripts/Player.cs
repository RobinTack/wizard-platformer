using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Transform groundCheckTransform;
    [SerializeField] private LayerMask playerMask;
    [SerializeField] private GameObject fallDetector;

    private bool jumpKeyPressed;
    private float horizontalInput;
    private new Rigidbody rigidbody;
    private int points;
    private Vector3 respawnPoint;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        // Where ever the player is at the start of the game, that's where he'll respawn
        respawnPoint = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpKeyPressed = true;
        }

        // check input manager to see that the horizontal axis exists
        horizontalInput = Input.GetAxis("Horizontal");

        // make the falldetector follow the player
        fallDetector.transform.position = new Vector3(transform.position.x, fallDetector.transform.position.y, fallDetector.transform.position.z);
    }

    // FixedUpdate is called every physics update, the physics engine runs regardless of fps
    private void FixedUpdate()
    {
        rigidbody.velocity = new Vector3(horizontalInput * 2, rigidbody.velocity.y, 0);

        if (Physics.OverlapSphere(groundCheckTransform.position, 0.1f, playerMask).Length == 0)
        {
            return;
        }

        if (jumpKeyPressed)
        {
            // we use velocitychange because we want to ignore the mass in a simple platformer
            rigidbody.AddForce(Vector3.up * 6, ForceMode.VelocityChange);
            jumpKeyPressed = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "FallDetector")
        {
            transform.position = respawnPoint;
        }
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 100, 20), "Score: " + points);
    }

    public void IncrementPoints()
    {
        points++;
    }
}

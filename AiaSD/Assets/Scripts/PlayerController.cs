using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float playerSpeed = 10; public float gravityForce = 0.981f;
    private CharacterController controller;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 movement = transform.right * x + transform.forward * z + transform.up * -gravityForce;
        movement *= Time.deltaTime * playerSpeed;
        movement += transform.up * -gravityForce * Time.deltaTime;

        controller.Move(movement);
    }
}

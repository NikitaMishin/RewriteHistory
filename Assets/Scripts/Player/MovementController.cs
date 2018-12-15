using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{


    [SerializeField]
    private Camera camera;

    [SerializeField]
    private float jumpForce = 0.3f;
    [SerializeField]
    private float speed = 10;
    [SerializeField]
    private float acceleration;
    [SerializeField]
    private bool isGrounded = true;
    [SerializeField]
    private LayerMask groundLayers;
    [SerializeField]
    private LayerMask layerMask;

    private bool isSeated = false;
    private float seatHeight = 0;

    private bool goRight = true;
    private bool canGoRight = true;
    private bool canGoLeft = true;

    private Rigidbody rigidBody;
    private CapsuleCollider capsule;


    // Use this for initialization
    void Start()
    {
    }

    void Awake()
    {
        rigidBody = gameObject.GetComponent<Rigidbody>();
        capsule = gameObject.GetComponent<CapsuleCollider>();
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKey(KeyCode.S) && isGrounded)
        {
            seat();
        }
        else if (canStand())
        {
            stand();
        }

        if (Input.GetKey(KeyCode.D) && canGo(Vector3.right))
        {
            moveRight();
        }
        else if (Input.GetKey(KeyCode.A) && canGo(Vector3.left))
        {
            moveLeft();
        }


        if (Input.GetKey(KeyCode.W) && isGrounded && canStand())
        {
            jump();
            isGrounded = false;
        }

        setCameraPosition();
    }

    private void seat()
    {
        if (!isSeated)
        {
            transform.localScale = new Vector3(0.5f, 0.25f, 0.5f);
            transform.position = new Vector3(transform.position.x, transform.position.y - 0.25f, transform.position.z);
            isSeated = true;
        }
    }

    private void stand()
    {
        if (isSeated)
        {
            transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            transform.position = new Vector3(transform.position.x, transform.position.y + 0.25f, transform.position.z);
            isSeated = false;
        }
    }

    private void moveRight()
    {
        transform.position = new Vector3(
            transform.position.x + speed * Input.GetAxis("Horizontal") * Time.deltaTime,
            transform.position.y,
            2);
    }

    private void moveLeft()
    {
        transform.position = new Vector3(
            transform.position.x + speed * Input.GetAxis("Horizontal") * Time.deltaTime,
            transform.position.y,
            2);
    }

    private void jump()
    {
        rigidBody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }


    private void setCameraPosition()
    {
        camera.transform.position = new Vector3(transform.position.x, capsule.bounds.min.y + 5, -4);
    }

    private bool canGo(Vector3 vector)
    {
        return
            !Physics.Raycast(
                    transform.position,
                    transform.TransformDirection(vector),
                    capsule.radius,
                    layerMask
            )
           /* && !Physics.Raycast(
                    new Vector3(
                        capsule.bounds.center.x,
                        capsule.bounds.min.y + 0.1f,
                        capsule.bounds.center.z
                    ),
                    transform.TransformDirection(vector),
                    capsule.radius,
                    layerMask
            )*/
            && !Physics.Raycast(
                    new Vector3(
                        capsule.bounds.center.x,
                        capsule.bounds.max.y - 0.1f,
                        capsule.bounds.center.z
                    ),
                    transform.TransformDirection(vector),
                    capsule.radius,
                    layerMask
            );

    }

    private bool canStand()
    {
        return
           !Physics.Raycast(
                   new Vector3(
                       capsule.bounds.center.x,
                       capsule.bounds.center.y,
                       capsule.bounds.center.z
                   ),
                   transform.TransformDirection(Vector3.up),
                   capsule.height * 1.5f,
                   layerMask
           )
           && !Physics.Raycast(
                   new Vector3(
                       capsule.bounds.min.x,
                       capsule.bounds.center.y,
                       capsule.bounds.center.z
                   ),
                   transform.TransformDirection(Vector3.up),
                   capsule.radius,
                   layerMask
           )
           && !Physics.Raycast(
                   new Vector3(
                       capsule.bounds.max.x,
                       capsule.bounds.center.y,
                       capsule.bounds.center.z
                   ),
                   transform.TransformDirection(Vector3.up),
                   capsule.radius,
                   layerMask
           );

    }

    private void OnCollisionStay(Collision collision)
    {
        isGrounded = Physics.CheckCapsule(
            capsule.bounds.center,
            new Vector3(capsule.bounds.center.x, capsule.bounds.min.y, capsule.bounds.center.z),
            0.1f,
            groundLayers
        );
    }


}

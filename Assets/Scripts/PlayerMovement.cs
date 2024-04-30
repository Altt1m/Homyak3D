using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //классическое перемещение игрока

    public float movespeed;
    public float sensitivity;
    public float gravity;
    private float mouseX;
    private float mouseY;
    private float vertical;
    private float horizontal;

    public Vector2 clampangle;
    private Vector3 Velocity;
    private Vector2 angle;
    public Transform cameraTransform; //камера внутри игрока
    public static bool isBusy = false;

    private CharacterController charactercontroller;

    private Animator animator;

    private void Start()
    {
        charactercontroller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        if (!isBusy)
        {
            vertical = Input.GetAxis("Vertical");
            horizontal = Input.GetAxis("Horizontal");

            Vector3 playerMovementInput = new Vector3(horizontal, 0.0f, vertical);
            Vector3 moveVector = transform.TransformDirection(playerMovementInput);

            if (Input.GetAxis("Vertical") > 0 || Input.GetAxis("Vertical") < 0 || Input.GetAxis("Horizontal") > 0 || Input.GetAxis("Horizontal") < 0)
            {
                if (Input.GetKey(KeyCode.LeftShift) && Input.GetAxis("Vertical") > 0)
                {
                    movespeed = 8;
                    //бежит
                    animator.SetInteger("Switch", 2);
                }
                else
                {
                    movespeed = 3f;
                    //идет
                    animator.SetInteger("Switch", 1);
                }
            }
            else
            {
                //стоит
                animator.SetInteger("Switch", 0);
            }

            if (charactercontroller.isGrounded)
            {
                Velocity.y = -1f;
            }
            else
            {
                Velocity.y -= gravity * Time.deltaTime;
            }

            charactercontroller.Move(moveVector * movespeed * Time.deltaTime);
            charactercontroller.Move(Velocity * Time.deltaTime);

            mouseX = Input.GetAxis("Mouse X");
            mouseY = Input.GetAxis("Mouse Y");

            angle.x -= mouseY * sensitivity;
            angle.y -= mouseX * -sensitivity;

            angle.x = Mathf.Clamp(angle.x, -clampangle.x, clampangle.y);

            Quaternion rotation = Quaternion.Euler(angle.x, angle.y, 0.0f);
            Quaternion rotationTwo = Quaternion.Euler(0.0f, angle.y, 0.0f);
            transform.rotation = rotationTwo;
            cameraTransform.rotation = rotation;
        }
        else
        {
            animator.SetInteger("Switch", 0);
        }
    }

}

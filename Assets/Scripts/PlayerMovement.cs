using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //������������ ����������� ������

    public float movespeed;
    public float sensitivity;
    public float gravity;
    private float mouseX;
    private float mouseY;
    private float vertical;
    private float horizontal;
    private bool isSeated;

    public Vector2 clampangle;
    private Vector3 Velocity;
    private Vector2 angle;
    public Transform cameraTransform; //������ ������ ������
    public static bool isBusy;

    private CharacterController charactercontroller;

    private Animator animator;

    private void Start()
    {
        charactercontroller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        isSeated = false;
        isBusy = false;
    }

    private void Update()
    {
        if (!isBusy)
        {
            vertical = Input.GetAxis("Vertical");
            horizontal = Input.GetAxis("Horizontal");

            Vector3 playerMovementInput = new Vector3(horizontal, 0.0f, vertical);
            Vector3 moveVector = transform.TransformDirection(playerMovementInput);

            if (Input.GetKey(KeyCode.LeftControl))
            {
                cameraTransform.position = new Vector3(transform.position.x, transform.position.y+0.75f, transform.position.z);
                isSeated = true;
            }
            else
            {
                cameraTransform.position = new Vector3(transform.position.x, transform.position.y+1.5f, transform.position.z);
                isSeated = false;
            }

            if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)
            {
                if (!isSeated)
                {
                    cameraTransform.position = new Vector3(transform.position.x, transform.position.y+1.5f, transform.position.z);
                }
                
                if (Input.GetKey(KeyCode.LeftShift) && Input.GetAxis("Vertical") > 0)
                {
                    if (!isSeated)
                    {
                        movespeed = 8;
                    }
                    else movespeed = 4;

                    //�����
                    animator.SetInteger("Switch", 2);
                }
                else if (Input.GetKey(KeyCode.LeftShift) && Input.GetAxis("Horizontal") != 0)
                {
                    if (!isSeated) movespeed = 6.5f;
                    else movespeed = 3f;
                    //����
                    animator.SetInteger("Switch", 2);
                }
                else if (Input.GetKey(KeyCode.LeftShift) && Input.GetAxis("Vertical") < 0)
                {
                    if (!isSeated) movespeed = 4.5f;
                    else movespeed = 2f;
                    //����
                    animator.SetInteger("Switch", 2);
                }
                else
                {
                    if (!isSeated) movespeed = 3f;
                    else movespeed = 2f;
                    //����
                    animator.SetInteger("Switch", 1);
                }
            }
            else
            {
                //�����
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

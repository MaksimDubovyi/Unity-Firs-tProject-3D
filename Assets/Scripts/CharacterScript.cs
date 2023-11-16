using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterScript : MonoBehaviour
{
    [SerializeField]
    private Camera mainCamera;
    private CharacterController characterController;

    private float moveSpped = 3f;
    private float runFactor = 5f;

    private Animator animator;

    public float gravity;//прижок 
    public float jumpSpeed;

    float jspeed = 0;

    void Start()
    {
        characterController=this.GetComponent<CharacterController>();
        animator=this.GetComponent<Animator>();
    }


    void Update()
    {


        float fx = Input.GetAxis("Horizontal");     //ліво право
        float fz = Input.GetAxis("Vertical");       //перед  назад

        //-----stert-----------------Jump
        if (characterController.isGrounded)
        {
            jspeed = 0;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                jspeed = jumpSpeed;
            }
            if (Input.GetKeyDown(KeyCode.X))
            {
                
            }
        }
        jspeed += gravity * Time.deltaTime * 6;
        //----exit------------------Jump



        //Vector3 moveDirection =// new Vector3(fx,0,fz); -- вектор у світовому просторі Unity (сцени)
        //    для правельного руху треба привязуватись до повороту персонажу
        //     його векторів оріентації у просторі.

        Vector3 moveDirection =
            fx * moveSpped * Time.deltaTime * transform.right + //відповідає за рух вздовж осі X (ліво-право). fx - це вхідне значення з клавіатури (ліво (-1), право (1)).
            jspeed * Time.deltaTime * Vector3.up + //відповідає за вертикальний рух (вгору або вниз) в залежності від jspeed, що представляє швидкість прижка. Vector3.up - це вектор, що вказує вгору вздовж осі Y.
            fz * moveSpped * Time.deltaTime * transform.forward; //рух вздовж осі Z (перед-назад)


        //--stert---------------Animation
        int moveState = 0;
       
        if (Mathf.Abs(fx) > Mathf.Abs(fz))//перевірка вперед назад чи посторонам  (помодулю )
        {
            if (fx != 0)
            {
                if (fx < 0)
                {
                    moveState = 3; // Walk вліво
                }
                else
                {
                    moveState = 4; //Walk вправо
                }
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    moveState = 9; //Jump
                }
            }
            else
            {
                moveState = 0;
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    moveState = 6; //Jump
                }

            }
        }
        else
        {
            if (fz != 0)
            {
                if (fz < 0)
                {
                    moveState = 5; // Walk назад
                    if (Input.GetKeyDown(KeyCode.Space))
                    {
                        moveState = 8; //Jump
                    }
                }
                else
                {
                    moveState = 1; //Walk вперед
                    if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                    {
                        // moveDirection *= runFactor;
                        moveState = 2; //Run
                    }
                    if (Input.GetKeyDown(KeyCode.Space))
                    {
                        moveState = 7; //Jump
                    }
                }
            }
            else
            {
                moveState = 0; 
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    moveState = 6; //Jump
                }
            }
            
        }

        if (Input.GetMouseButtonDown(0))
        {
            moveState = 11; // Нажата левая кнопка мыши (Mouse0)
        }
        if (Input.GetMouseButtonDown(1))
        {
            moveState = 12; // Нажата левая кнопка мыши (Mouse0)
        }
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            moveDirection *= runFactor;
            //   moveState = 2; //Run
        }
        //-----exit------------Animation
        characterController.Move(moveDirection); //делегуємо персонажу координати 

        animator.SetInteger("MoveState", moveState); //задаємо стан аніматора він має обрати кліп

       
    }

    private void LateUpdate()
    {
        this.transform.forward= new Vector3( 
            mainCamera.transform.forward.x,
            0,
            mainCamera.transform.forward.z).normalized;
    }
}

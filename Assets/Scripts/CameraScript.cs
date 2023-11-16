using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{

    private float cameraXAngle; // Накопиченірухами миші кути повороту камери
    private float cameraYAngle; // Накопиченірухами миші кути повороту камери
    private Vector3 rod; //взаємне розташування камери і персонажа
    [SerializeField]
    private GameObject cameraAncor; //опорна точка навколо якої рухається камера і до якої наближається при переході до першої особи

    void Start()
    {
        cameraXAngle = this.transform.eulerAngles.x;
        cameraYAngle= this.transform.eulerAngles.y;
        //фіксуємо на старті взаємне розташування камери та персонажа 
        rod = this.transform.position - cameraAncor.transform.position;
    }

    void Update()
    {
        //Mouse X - переміщення миші по горизонталі (не позиція)

        cameraYAngle+=Input.GetAxis("Mouse X")*MenuScript.cameraSensityvityX; //поворот камери для огляду горизонталі це поворот навколо осі Y
        // поворот по вертикалі - у залежності від побажаннь
        // += --> миша вгору -> камера вниз
        // -= --> миша вниз -> камера вгору
        float my = Input.GetAxis("Mouse Y") * MenuScript.cameraSensityvityY; //поворот камери для огляду вертикалі це поворот навколо осі X
        cameraXAngle= MenuScript.cameraInverseY
            ?cameraXAngle+my
            : cameraXAngle - my;


    }
    private void LateUpdate() //події для привязки для кадру (події для користувача)
    {
        // Поворот (Rotate) є "каскадним" - спочатку по Х, потім (вже с поворотом по X)- по Y.
        // Це призводить до появи також осі Z.
        // this.transform.Rotate(cameraXAngle,cameraYAngle,0);

        
        // відстежуємо рух персонажа (його опорної точки ) і слідуємо за нею - переносимо камеру з 
        // відносно положення - rod
        this.transform.position = cameraAncor.transform.position+
          Quaternion.Euler(0,cameraYAngle,0)* rod;

        // вирішення проблеми - пряме встановлення кутів , замість процедури повороту
        // Кути до осей називають кутами Ейлера
        this.transform.eulerAngles=new Vector3(cameraXAngle, cameraYAngle, 0);

       
      
    }
}

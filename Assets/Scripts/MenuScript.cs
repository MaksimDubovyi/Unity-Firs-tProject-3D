using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/* Відображення інформації / інтерфейс користувача
* 1. Розміщуємо холст (Canvas)
*   Screen Space - Overlay -- прив'язка до "монітора", видно з усіх камер
*   Canvas Scaler -> Scale With Screen Size -- змінювати розмір холста
*     під реальний розмір екрану шляхом масштабування
*   Reference Resolution -> 1920 x 1080 -- опорний розмір екрану, від
*     якого відбуватиметься масштабування
*   Screen Match Mode -> Match Width or Height -- адаптуватись шляхом
*     заповнення екрану або за шириною, або за висотою
*  
* 2. Визначаємось із прив'язкою елементів (з якорями)
*   Якір визначає точку і правила відліку при розміщенні елемента.
*   Є встановлені пресети для опорних точок по кутах та центру холста
*   Наприклад, якщо якір у лівому верхньому куті, то зміщення елементу
*   вираховуються саме від цієї точки, відповідно, при масштабуванні
*   екрану (у т.ч. непропорційному) залишається "прив'язка" до лівого
*   верхнього кута.
*   Кожен елемент холста має свої якоря.
* 
* Д.З. У меню паузи реалізувати блок кнопок: "Закрити" "Вихід" "Default" - 
* відновлення конфігурації за замовчанням.
* Розмістити їх в еластичному блоці, що адаптує розміри під розміри
* екрану.
* ** Реалізувати зупинку часу при активації меню паузи, відновлення
*    при закритті.
*/

public class MenuScript : MonoBehaviour
{
    public static float cameraSensityvityX;
    public static float cameraSensityvityY;
    public static bool cameraInverseY;
    public static int CountCoint;
    public static int ToggleScoringPointst;
    public static int IsToggleScoringPointst; //0=LightToggle 1=MediumToggle  2=HeavyToggle
    public static Vector3 coinPosition;
    public static Vector3 characterPosition;
    public static Vector3 characterForward;

    [SerializeField] // якщо буде не активний обєкт то ми його не знайдемо без  [SerializeField]
    private GameObject PauseMenu;

    [SerializeField] // якщо буде не активний обєкт то ми його не знайдемо без  [SerializeField]
    private GameObject StepBlock;
    [SerializeField] // якщо буде не активний обєкт то ми його не знайдемо без  [SerializeField]
    private GameObject CompasArrow;

    [SerializeField]
    private Slider sensXSlider;
    [SerializeField]
    private Slider sensYSlider;
    [SerializeField]
    private Toggle invYToggle;

    [SerializeField]
    private Toggle LightToggle;
    [SerializeField]
    private Toggle MediumToggle;
    [SerializeField]
    private Toggle HeavyToggle;

    [SerializeField]
    private TMPro.TextMeshProUGUI clock;

    [SerializeField]
    private TMPro.TextMeshProUGUI countStep;

    [SerializeField]
    private TMPro.TextMeshProUGUI countCoint;
   
    [SerializeField]
    private TMPro.TextMeshProUGUI scoringPoints;

    [SerializeField]
    private Image compasArrow;

    private float totalTime;
    private bool totalTimePause;

    void Start()
    {
        OnCameraSensXSlider(sensXSlider.value);
        OnCameraSensYSlider(sensYSlider.value);
        OnCameraInverseToggle(invYToggle.isOn);
        PauseMenu.SetActive(!PauseMenu.activeInHierarchy);
        totalTime = 0;
        totalTimePause = true;


        IsToggleScoringPointst = 0;
        LightToggle.isOn = true;
        MediumToggle.isOn = false;
        HeavyToggle.isOn = false;
    }

 
    void Update()
    {
        if (totalTimePause)
        totalTime += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Escape)) 
        {
            if(totalTimePause)
            totalTimePause=false;
            else
            {
                totalTimePause = true;
            }
            PauseMenu.SetActive(!PauseMenu.activeInHierarchy);
        }
    }

    private void LateUpdate()
    {
        IsToggle();
        int time = (int)totalTime;
        int hour = time / 3600;
        int minute = (time % 3600) / 60;
        int second = time % 60;

        clock.text = $"{hour.ToString("00")}:{minute.ToString("00")}:{second.ToString("00")}";

        compasArrow.transform.eulerAngles= new Vector3(0, 0,
            Vector3.SignedAngle(characterForward, coinPosition-characterPosition, Vector3.down));

     
        float distance = Vector3.Distance(coinPosition, characterPosition);
        int distanceInt = (int)distance;
        CountStepColor(distanceInt);

        countStep.text = distanceInt.ToString();

        countCoint.text=CountCoint.ToString();
        scoringPoints.text=ToggleScoringPointst.ToString();
    }

    public void OnCameraInverseToggle(Boolean value)
    {
        MenuScript.cameraInverseY = value;
    }

    public void OnCameraSensXSlider(Single value)
    {
        MenuScript.cameraSensityvityX = value+1;
    }

    public void OnCameraSensYSlider(float value)
    {
        MenuScript.cameraSensityvityY = value + 1;
    }

    //обробник для події UI (кнопки)
    public void OnСloseButtonClick()
    {
        totalTimePause = true;
        PauseMenu.SetActive(!PauseMenu.activeInHierarchy);
    }
    public void OnDefaultButtonClick()
    {

        sensXSlider.SetValueWithoutNotify(0.5f);
        sensYSlider.SetValueWithoutNotify(0.5f); 
        invYToggle.isOn = false;
    }
    public void OnExitButtonClick()
    {       
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Зупинка в редакторі Unity
        #else
        Application.Quit(); // Закриття додатка на інших платформах
        #endif
    }

    private void CountStepColor(int distanceInt)
    {
        if (distanceInt > 50)
        {
            countStep.color = Color.red;
        }
        else if (distanceInt < 50)
        {
            countStep.color = Color.white;
        }
        if (distanceInt < 10)
        {
            countStep.color = Color.green;
        }
    }

    public void IsToggle()
    {
        if (LightToggle.isOn)
            IsToggleScoringPointst = 0;
        else if (MediumToggle.isOn)
            IsToggleScoringPointst = 1;
        else if (HeavyToggle.isOn)
            IsToggleScoringPointst = 2;
    }
    public void IsToggleShowMenu()
    {
        if (LightToggle.isOn)
        {
            StepBlock.SetActive(true);
            CompasArrow.SetActive(true);
        }
        if (MediumToggle.isOn)
        {
            StepBlock.SetActive(false);
            CompasArrow.SetActive(true);
        }
        if (HeavyToggle.isOn)
        {

            StepBlock.SetActive(false);
            CompasArrow.SetActive(false);
        }
    }


}


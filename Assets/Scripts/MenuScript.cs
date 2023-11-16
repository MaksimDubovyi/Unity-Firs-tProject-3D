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

    [SerializeField] // якщо буде не активний обєкт то ми його не знайдемо без  [SerializeField]
    private GameObject PauseMenu;

    [SerializeField]
    private Slider sensXSlider;
    [SerializeField]
    private Slider sensYSlider;
    [SerializeField]
    private Toggle invYToggle;

    [SerializeField]
    private TMPro.TextMeshProUGUI clock;

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
        int time = (int)totalTime;
        int hour = time / 3600;
        int minute = (time % 3600) / 60;
        int second = time % 60;

        clock.text = $"{hour.ToString("00")}:{minute.ToString("00")}:{second.ToString("00")}";
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

    
    private void TimeOnOff()
    {

    }
}


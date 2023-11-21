using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightScript : MonoBehaviour
{
    [SerializeField] 
    private Material daySkybox;

    [SerializeField]
    private Material nightSkybox;

    [SerializeField]
    private Light directionalLight; // джерело світла (Сонце)


    private AudioSource daySoung;
    private AudioSource nightSoung;

    private float dayTime; // 0..24 - умовний час доби 
    private const float dayPeriod = 20; // 20 секунд на добовий цикл

    void Start()
    {
 

        // Робота з кількома однотипними компонентами
        // GetComponent<AudioSource>() - знайти перший з компонентів
        // GetComponents<AudioSource>() - масив усіх компонентів.
        // не рекомендується зберігати масив та звертатись до його індексів
        // рекомендується створити іменовані змінні для різних індексів

        AudioSource[] audioSources= GetComponents<AudioSource>();
        //порядок компонентів відповідає порядку у інспеккторі юніті
        daySoung= audioSources[0];
        nightSoung= audioSources[1];

      // на старті день 
        dayTime = 12;
        daySoung.Play();

        // взаємо дія скриптів (демонстрація) 
        //GameObject.Find("Joni(1)").GetComponent<CharacterScript>().booserBonus = true;

    }

    void Update()
    {
        dayTime+= Time.deltaTime/dayPeriod*24;
        dayTime %=  24;
    }
    private void LateUpdate()
    {
        if(dayTime>8f&&dayTime<20f)// день
        {
            if(RenderSettings.skybox!=daySkybox)
            {
                RenderSettings.skybox = daySkybox;
                directionalLight.intensity= 1f;
                daySoung.Play();
                nightSoung.Stop();

            }
          
        }
        else //ніч
        {
            if (RenderSettings.skybox != nightSkybox)
            {
                RenderSettings.skybox = nightSkybox;
                directionalLight.intensity = 0.3f;
                daySoung.Stop();
                nightSoung.Play();
            }
        }
        RenderSettings.skybox.SetFloat("_Rotation", dayTime * 10f);
    }
}

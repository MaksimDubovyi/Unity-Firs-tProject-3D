using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CoinScript : MonoBehaviour
{
    [SerializeField]
    private GameObject coinPrefab;
    private Animator animator;


    private float coinSpuwnHeight; //Висота появи над зимлею
  
    void Start()
    {
       animator=GetComponent<Animator>();

     
        coinSpuwnHeight= this.transform.position.y- //висота позиції ассету мінус
            //  Висота Землі у точці розміщення this (монети)
            Terrain.activeTerrain.SampleHeight(this.transform.position);
    }

  
    void Update()
    {
        //для компасу
        MenuScript.coinPosition=transform.position;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.name.Equals("Joni (1)")&& animator.GetBool("IsCaught" )== false)
        {
            var newCoin = GameObject.Instantiate(coinPrefab);
            //Визначаємо позицію нової монети:
            //1. Не ближче за 10 одиниць від відстані до персонажа (попереднього положення)
            //2. Не виходить за межі гір (100-900 по Землі)
            //3. На постійній відстані по висоті над землею

            Vector3 newPosition;
            do
            {
                newPosition = transform.position;
                newPosition.x += Random.Range(-15f, 15f);
                newPosition.z += Random.Range(-15f, 15f);


            } while (Vector3.Distance(newPosition, transform.position) < 10
                        || (newPosition.x < 100 || newPosition.x > 900)
                        || (newPosition.z < 100 || newPosition.z > 900));

            newPosition.y = coinSpuwnHeight + Terrain.activeTerrain.SampleHeight(newPosition);

            newCoin.transform.position = newPosition;

         
            animator.SetBool("IsCaught", true);
        }
        Debug.Log(other.name);
    }

    public void OnDisapearEnds() // подія для  її запуску з аніматора (наша назву можемо задавати любу)
    {
       GameObject.Destroy(this.gameObject);
       MenuScript.CountCoint++;
        if (MenuScript.IsToggleScoringPointst == 0)
            MenuScript.ToggleScoringPointst++;
        else if (MenuScript.IsToggleScoringPointst == 1)
            MenuScript.ToggleScoringPointst+=2;
        else if (MenuScript.IsToggleScoringPointst == 2)
            MenuScript.ToggleScoringPointst += 3;
    }
}

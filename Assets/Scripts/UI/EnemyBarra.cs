using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class EnemyBarra : MonoBehaviour
{
    public static EnemyBarra InterfaceEnemy;
    //public Image barraVidaEnemy;
    private Camera camara;
    // Start is called before the first frame update
    void Start()
    {
        camara = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActualizarVidaEnemy(Image barraVidaEnemy, float porcentaje){
        barraVidaEnemy.fillAmount = porcentaje;
    }
}

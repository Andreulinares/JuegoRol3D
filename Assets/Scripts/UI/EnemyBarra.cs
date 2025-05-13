using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class EnemyBarra : MonoBehaviour
{
    public static EnemyBarra InterfaceEnemy;
    //public Image barraVidaEnemy;
    private Camera camara;

    private void Awake(){
        if (InterfaceEnemy == null){
            InterfaceEnemy = this;
        }else{
            Destroy(gameObject);
        }
    }

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
        //porcentaje = Mathf.Clamp(porcentaje, 0f, 1f);
        barraVidaEnemy.fillAmount = porcentaje;
    }
}

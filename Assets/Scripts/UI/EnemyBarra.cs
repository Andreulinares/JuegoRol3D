using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class EnemyBarra : MonoBehaviour
{
    public static EnemyBarra InterfaceEnemy;
    //public Image barraVidaEnemy;
    private Transform cam;

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
        cam = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void LateUpdate()
    {
        transform.LookAt(transform.position + cam.forward);
    }

    public void ActualizarVidaEnemy(Image barraVidaEnemy, float porcentaje){
        //porcentaje = Mathf.Clamp(porcentaje, 0f, 1f);
        barraVidaEnemy.fillAmount = porcentaje;
    }
}

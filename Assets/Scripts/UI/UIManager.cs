using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System.Linq;

public class UIManager : MonoBehaviour
{

    public static UIManager Interface;

    public Image BarraVida;
    public List <Image> barrasMana;

    private void Awake(){
        if (Interface == null){
            Interface = this;
        }else{
            Destroy(gameObject);
        }
    }

    public void ActualizarVida(float porcentaje){
        BarraVida.fillAmount = porcentaje;
    }

    public void ActualizarMana(int manaActual){
        for (int i = 0; i < barrasMana.Count; i++){
            if (i < manaActual){
                barrasMana[i].enabled = true;
            }else{
                barrasMana[i].enabled = false;
            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

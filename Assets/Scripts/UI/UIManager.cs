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

    public Image ruedaElementos;
    public List<Sprite> ElementosSeleccionados;

    private int ElementoSeleccionado = 0;

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

    public void ActualizarMana(float manaActual){
        /*for (int i = 0; i < barrasMana.Count; i++){
            if (i < manaActual){
                barrasMana[i].enabled = true;
            }else{
                barrasMana[i].enabled = false;
            }
        }*/

        float manaBarra = 1f /
        barrasMana.Count;

        for (int i = 0; i < barrasMana.Count; i++){
            float cantidadEnBarra = Mathf.Clamp01(manaActual / manaBarra);

            barrasMana[i].fillAmount = cantidadEnBarra;

            manaActual -= manaBarra;
        }
    }

    public void CambiarElemento(int direccion){
        ElementoSeleccionado += direccion;

        if (ElementoSeleccionado < 0){
            ElementoSeleccionado = ElementosSeleccionados.Count - 1;
        }else if (ElementoSeleccionado >= ElementosSeleccionados.Count){
            ElementoSeleccionado = 0;
        }

        ruedaElementos.sprite = ElementosSeleccionados[ElementoSeleccionado];
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

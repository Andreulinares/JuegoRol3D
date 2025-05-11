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
    private Sprite spritePorDefecto;
    public List<Sprite> ElementosSeleccionados;

    private int ElementoSeleccionado = 0;

    private void Awake(){
        if (Interface == null){
            Interface = this;
            DontDestroyOnLoad(gameObject);
        }else{
            Destroy(gameObject);
        }
    }

    public void ActualizarVida(float porcentaje){
        BarraVida.fillAmount = porcentaje;
    }

    public void ActualizarMana(float manaActual)
    {
    
        manaActual = Mathf.Clamp01(manaActual); // Asegurarse de que esté entre 0 y 1
        float porcentajePorBarra = 1f / barrasMana.Count; // Por ejemplo, 0.25 si hay 4 barras

        for (int i = 0; i < barrasMana.Count; i++)
        {
            float inicioBarra = i * porcentajePorBarra;
            float finBarra = inicioBarra + porcentajePorBarra;

            if (manaActual >= finBarra)
            {
                // Barra completamente llena
                barrasMana[i].fillAmount = 1f;
            }
            else if (manaActual <= inicioBarra)
            {
                // Barra completamente vacía
                barrasMana[i].fillAmount = 0f;
            }
            else
            {
                // Barra parcialmente llena
                float cantidad = (manaActual - inicioBarra) / porcentajePorBarra;
                barrasMana[i].fillAmount = cantidad;
            }
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
    public void mostrarFuego()
    {
        ElementoSeleccionado = 0; // Asumimos que el fuego está en la posición 0 de la lista
        ruedaElementos.sprite = ElementosSeleccionados[ElementoSeleccionado];
    }
    public void mostrarTierra()
    {
        ElementoSeleccionado = 1; // Asumimos que el fuego está en la posición 0 de la lista
        ruedaElementos.sprite = ElementosSeleccionados[ElementoSeleccionado];
    }
    public void mostrarAgua()
    {
        ElementoSeleccionado = 2; // Asumimos que el fuego está en la posición 0 de la lista
        ruedaElementos.sprite = ElementosSeleccionados[ElementoSeleccionado];
    }
    public void mostrarElectricidad()
    {
        ElementoSeleccionado = 3; // Asumimos que el fuego está en la posición 0 de la lista
        ruedaElementos.sprite = ElementosSeleccionados[ElementoSeleccionado];
    }
    public void mostrarNinguno()
    {
        ruedaElementos.sprite = spritePorDefecto;
    }
    void Start()
    {
        spritePorDefecto = ruedaElementos.sprite;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System.Linq;
using TMPro;

public class UIManager : MonoBehaviour
{

    public static UIManager Interface;

    public Image BarraVida;
    public List<Image> barrasMana;

    public Image ruedaElementos;
    private Sprite spritePorDefecto;
    public List<Sprite> ElementosSeleccionados;
    public Image elementoDeFuego;
    public Image elementoDeFuegoDes;

    public Image elementoDeTierra;
    public Image elementoDeTierraDes;
    public Image elementoDeElectricidad;
    public Image elementoDeElectricidadDes;

    public Image VelocidadAumentada;
    public Image RegeneracionVida;

    private int ElementoSeleccionado = 0;

    public TMP_Text textFire;

    private void Awake()
    {
        if (Interface == null)
        {
            Interface = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ActualizarVida(float porcentaje)
    {
        BarraVida.fillAmount = porcentaje;
    }

    public void ActualizarMana(float manaActual)
    {

        manaActual = Mathf.Clamp01(manaActual);
        float porcentajePorBarra = 1f / barrasMana.Count;

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

    public void CambiarElemento(int direccion)
    {
        ElementoSeleccionado += direccion;

        if (ElementoSeleccionado < 0)
        {
            ElementoSeleccionado = ElementosSeleccionados.Count - 1;
        }
        else if (ElementoSeleccionado >= ElementosSeleccionados.Count)
        {
            ElementoSeleccionado = 0;
        }

        ruedaElementos.sprite = ElementosSeleccionados[ElementoSeleccionado];
    }
    // Start is called before the first frame update
    public void mostrarFuego()
    {
        ElementoSeleccionado = 0;
        ruedaElementos.sprite = ElementosSeleccionados[ElementoSeleccionado];
    }
    public void mostrarTierra()
    {
        ElementoSeleccionado = 1;
        ruedaElementos.sprite = ElementosSeleccionados[ElementoSeleccionado];
    }
    public void mostrarAgua()
    {
        ElementoSeleccionado = 2;
        ruedaElementos.sprite = ElementosSeleccionados[ElementoSeleccionado];
    }
    public void mostrarElectricidad()
    {
        ElementoSeleccionado = 3;
        ruedaElementos.sprite = ElementosSeleccionados[ElementoSeleccionado];
    }
    public void mostrarNinguno()
    {
        ruedaElementos.sprite = spritePorDefecto;
    }
    public void coleccionadoFuego()
    {
        elementoDeFuego.gameObject.SetActive(false);
    }

    public void coleccionadoTierra()
    {
        elementoDeTierra.gameObject.SetActive(true);
    }
    public void coleccionadoElectricidad()
    {
        elementoDeElectricidad.gameObject.SetActive(true);
    }
    void Start()
    {
        spritePorDefecto = ruedaElementos.sprite;
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    //INDICADORES PODERES ELEMENTALES UI

    //Mostrar notificacion cuando se seleccione el elemento de fuego
    public void MostrarNotificacionFire()
    {
        textFire.gameObject.SetActive(true);
        Invoke("DesactivarNotificacion", 2f);
    }

    private void DesactivarNotificacion()
    {
        textFire.gameObject.SetActive(false);
    }

    public void MostrarImagenVelocidad()
    {
        VelocidadAumentada.gameObject.SetActive(true);
        Invoke("DesactivarVelocidad", 5f);
    }

    private void DesactivarVelocidad()
    { 
        VelocidadAumentada.gameObject.SetActive(false);
    }
}

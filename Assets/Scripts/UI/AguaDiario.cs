using UnityEngine;
using UnityEngine.UI;

public class AguaDiario : MonoBehaviour
{
    public Image imagenElemento;     // La imagen en la UI
    public Sprite spriteAguaNuevo;   // El nuevo sprite que quieres mostrar al recoger el elemento

    void Start()
    {

    }

    public void ColeccionadoAgua()
    {
        imagenElemento.sprite = spriteAguaNuevo;
    }
}
using UnityEngine;
using UnityEngine.UI;

public class FuegoDiario : MonoBehaviour
{
    public Image imagenElemento;     // La imagen en la UI
    public Sprite spriteFuegoNuevo;   // El nuevo sprite que quieres mostrar al recoger el elemento

    void Start()
    {

    }

    public void ColeccionadoFuego()
    {
        imagenElemento.sprite = spriteFuegoNuevo;
    }
}
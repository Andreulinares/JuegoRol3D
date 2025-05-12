using UnityEngine;
using UnityEngine.UI;

public class TierraDiario : MonoBehaviour
{
    public Image imagenElemento;     // La imagen en la UI
    public Sprite spriteTierraNuevo;   // El nuevo sprite que quieres mostrar al recoger el elemento

    void Start()
    {

    }

    public void ColeccionadoTierra()
    {
        imagenElemento.sprite = spriteTierraNuevo;
    }
}
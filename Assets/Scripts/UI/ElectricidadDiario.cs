using UnityEngine;
using UnityEngine.UI;

public class ElectricidadDiario : MonoBehaviour
{
    public Image imagenElemento;     // La imagen en la UI
    public Sprite spriteElectricidadNuevo;   // El nuevo sprite que quieres mostrar al recoger el elemento

    void Start()
    {

    }

    public void ColeccionadoElectricidad()
    {
        imagenElemento.sprite = spriteElectricidadNuevo;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine;
using UnityEngine.Rendering;

public class MenuManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject panelOpciones;
    public GameObject panelControles;

    public void Jugar()
    {
        //GraphicsSettings.renderPipelineAsset = Resources.Load<RenderPipelineAsset>("UniversalRenderPipelineAsset");
        SceneManager.LoadScene("SeleccionPersonaje"); 
    }

    public void AbrirOpciones()
    {
        panelOpciones.SetActive(true); 
    }

    public void CerrarOpciones()
    {
        panelOpciones.SetActive(false); 
    }

    public void AbrirControles()
    {
        panelControles.SetActive(true); 
    }

    public void CerrarControles()
    {
        panelControles.SetActive(false); 
    }

    public void salir(){
        //salir
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Diario : MonoBehaviour
{
    public GameObject panelDiario;
    public GameObject[] paginas = new GameObject[7];
    public KeyCode teclaAvanzar     = KeyCode.RightArrow;
    public KeyCode teclaRetroceder  = KeyCode.LeftArrow;
    private bool diarioAbierto = false;
    private int paginaActual   = 0;
    // Start is called before the first frame update
    void Start()
    {
        panelDiario.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y) || Input.GetKeyDown(KeyCode.JoystickButton3))
        {
            ToggleDiario();
        }
        if (diarioAbierto)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                CambiarPagina(1);
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                CambiarPagina(-1);
            }
        }
    }

    private void ToggleDiario()
    {
        diarioAbierto = !diarioAbierto;
        panelDiario.SetActive(diarioAbierto);
        
        Time.timeScale = diarioAbierto ? 0 : 1;
        if (diarioAbierto)
        {
            MostrarPagina(paginaActual);
        }
        else
        {
            DesactivarTodasLasPaginas();
        }
    }
    private void CambiarPagina(int direccion)
    {
        paginaActual += direccion;

        // Limitar el rango entre 0 y paginas.Length - 1
        paginaActual = Mathf.Clamp(paginaActual, 0, paginas.Length - 1);

        MostrarPagina(paginaActual);
    }
    private void MostrarPagina(int indice)
    {
        for (int i = 0; i < paginas.Length; i++)
        {
            paginas[i].SetActive(i == indice);
        }
    }
    private void DesactivarTodasLasPaginas()
    {
        foreach (GameObject pagina in paginas)
        {
            pagina.SetActive(false);
        }
    }
}

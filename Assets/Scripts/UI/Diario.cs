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
    public GameObject[] textosPaginas = new GameObject[7];
    public bool pagina1 = false;
    public bool pagina2 = false;
    public bool pagina3 = false;
    public bool pagina4 = false;
    public bool pagina5 = false;
    public bool pagina6 = false;
    public bool pagina7 = false;
    // Start is called before the first frame update
    void Start()
    {
        panelDiario.SetActive(false);
        DesactivarTodasLasPaginas();
        DesactivarTextosPaginas();
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
            if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.JoystickButton5))
            {
                CambiarPagina(1);
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.JoystickButton4))
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

    public void ActivarPaginaPorNumero(int numero)
    {
        if (numero >= 1 && numero <= textosPaginas.Length)
        {
            
            textosPaginas[numero - 1].SetActive(true);

            
            switch (numero)
            {
                case 1: pagina1 = true; break;
                case 2: pagina2 = true; break;
                case 3: pagina3 = true; break;
                case 4: pagina4 = true; break;
                case 5: pagina5 = true; break;
                case 6: pagina6 = true; break;
                case 7: pagina7 = true; break;
            }

            activarPagina(); 
        }
    }

    public void activarPagina()
    {
        if (pagina1 == true)
        {
            textosPaginas[0].SetActive(true);
        }
        if (pagina2 == true)
        {
            textosPaginas[1].SetActive(true);
        }
        if (pagina3 == true)
        {
            textosPaginas[2].SetActive(true);
        }
        if (pagina4 == true)
        {
            textosPaginas[3].SetActive(true);
        }
        if (pagina5 == true)
        {
            textosPaginas[4].SetActive(true);
        }
        if (pagina6 == true)
        {
            textosPaginas[5].SetActive(true);
        }
        if (pagina7 == true)
        {
            textosPaginas[6].SetActive(true);
        }
    }
    public void DesactivarTextosPaginas()
    {
        foreach (GameObject pagina in textosPaginas)
        {
            pagina.SetActive(false);
        }
    }
}

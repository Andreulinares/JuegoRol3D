using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Boss_bt : MonoBehaviour
{
    private Animator animator;
    private GameObject jugador;
    private NavMeshAgent agent;
    public GameObject portal;

    private bool isAlive = true;
    public float vida = 100;
    public bool fuegoActivo = true;
    public bool aguaActivo = true;
    public bool tierraActivo = true;
    public bool electricidadActivo = true;

    public bool fragmentoFuegoColocado = false;
    public bool fragmentoAguaColocado = false;
    public bool fragmentoTierraColocado = false;
    public bool fragmentoElectricidadColocado = false;

    public bool jugadorDetectado = false;

    public float rangoDeAtaque = 0f;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        jugador = GameObject.FindWithTag("Player");
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (jugador == null)
        {
            jugador = GameObject.FindWithTag("Player");
        }

        if (!isAlive)
        {
            Muerte();
        }
        else
        {
            if (fragmentoFuegoColocado)
            {
                fuegoActivo = false;
                vida -= 25;
            }
            else if (fragmentoAguaColocado)
            {
                aguaActivo = false;
                vida -= 25;
            }
            else if (fragmentoTierraColocado)
            {
                tierraActivo = false;
                vida -= 25;
            }
            else if (fragmentoElectricidadColocado)
            {
                electricidadActivo = false;
                vida -= 25;
            }
            else
            {
                if (jugadorDetectado)
                {
                    if (JugadorEstaCerca())
                    {
                        ComprobarFuegoActivo();
                        if (ComprobarFuegoActivo())
                        {
                            UsarFuego();
                        }
                        else
                        {
                            AtaqueNormal(); 
                        }
                    }
                }
                else
                {

                }
            }
        }
    }

    bool JugadorEstaCerca()
    {
        return true;
    }

    public void TakeDamage(int daño)
    {
        float vidaNueva;

        vidaNueva = Mathf.Max(vida - daño);

        float vidaActual = vidaNueva;

        if (vidaActual <= 0)
        {
            isAlive = false;
        }
        //Actualizar barra
        //Animacion muerte
    }
    void Muerte()
    {
        //animator.SetTrigger("isDead");
        //animator.SetBool("isAttack", false);
        //Destroy(gameObject, 3f);

        if (portal != null)
        {
            portal.SetActive(true);
        }
    }

    bool ComprobarFuegoActivo()
    {
        if (fuegoActivo == true)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void UsarFuego()
    {

    }

    void UsarAgua()
    {

    }

    void UsarElectricidad()
    {

    }

    void UsarTierra()
    {

    }

    void AtaqueNormal()
    {
        
    }
}

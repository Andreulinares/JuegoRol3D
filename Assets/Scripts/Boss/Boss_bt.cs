using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Boss_bt : MonoBehaviour
{
    private Animator animator;
    private GameObject jugador;
    private NavMeshAgent agent;
    public GameObject portal;
    private Boss_patrol BossPatrolScript;

    private bool isAlive = true;
    public float vida = 100;
    public float vidaActual;
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

    public EnemyBarra enemyBarra;

    public Image barraVidaBoss;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        jugador = GameObject.FindWithTag("Player");
        animator = GetComponent<Animator>();
        BossPatrolScript = GetComponent<Boss_patrol>();

        ActualizarBarraVida();
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
                        else if (ComprobarTierraActivo())
                        {
                            UsarTierra();
                        }
                        else
                        {
                            AtaqueNormal();
                        }
                    }
                    else if (tengoPocaVida())
                    {
                        ComprobarAguaActivo();
                        if (ComprobarAguaActivo())
                        {
                            UsarAgua();
                        }
                        else
                        {
                            AtaqueNormal();
                        }
                    }
                    else if (ComprobarElectricidadActivo())
                    {
                        if (ComprobarElectricidadActivo())
                        {
                            UsarElectricidad();
                        }
                        else
                        {
                            AtaqueNormal();
                        }
                    }
                }
                else
                {
                    Patrullar();
                }
            }
        }
    }

    //Booleanos para comprobar si el jugador esta cerca y si el boss tiene poca vida
    bool JugadorEstaCerca()
    {
        return true;
    }

    bool tengoPocaVida()
    {
        return true;
    }

    //Manejar vida y muerte 
    public void TakeDamage(int daño)
    {
        float vidaNueva;

        vidaNueva = Mathf.Max(vida - daño);

        vidaActual = vidaNueva;

        ActualizarBarraVida();

        if (vidaActual <= 0)
        {
            isAlive = false;
        }
        //Actualizar barra
        //Animacion muerte y knockback
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

    //Funcion para actualizar la barra de vida del boss cuando el jugador le golpea
    void ActualizarBarraVida()
    {
        float porcentaje = vidaActual / vida;
        enemyBarra.ActualizarVidaEnemy(barraVidaBoss, porcentaje);
    }

    //Comprobar si las siguientes habilidades elementales estan activas
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

    bool ComprobarAguaActivo()
    {
        if (aguaActivo == true)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    bool ComprobarTierraActivo()
    {
        if (aguaActivo == true)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    bool ComprobarElectricidadActivo()
    {
        if (aguaActivo == true)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //Ataques del boss
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

    //Patrullaje por defecto
    void Patrullar()
    {
        BossPatrolScript.ActivarPatrullaje();

        //Animaciones boss caminar 
    }
}

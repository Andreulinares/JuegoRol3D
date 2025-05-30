using System;
using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReiniciarJuego : MonoBehaviour
{
    public GameManager gameManager;
    public ProgressManager progressManager;
    public BossAI bossAI;
    public PlayerController playerController;
    public ArqueroController arqueroController;
    public UIManager uiManager;
    public void reiniciarJuego()
    {
        // Destruir enemigos antes de reiniciar
    /*GameObject[] enemigos = GameObject.FindGameObjectsWithTag("Enemy");
    foreach (GameObject enemigo in enemigos)
    {
        Destroy(enemigo);
    }

    // Resetear valores del jugador
    playerController.vidaActualPlayer = playerController.vidaMaxPlayer;
    playerController.manaActualPlayer = playerController.manaMaxPlayer;
    playerController.transform.position = gameManager.puntoSpawn.position;

    arqueroController.vidaActualPlayer = arqueroController.vidaMaxPlayer;
    arqueroController.manaActualPlayer = arqueroController.manaMaxPlayer;
    arqueroController.transform.position = gameManager.puntoSpawn.position;

    // Resetear valores del boss
    bossAI.PVActual = bossAI.PVMax;
    bossAI.isApproaching = false;
    bossAI.isChasing = false;
    bossAI.Patrol();
    bossAI.transform.position = bossAI.puntoSpawnBoss.position;
    bossAI.agent.isStopped = false;
    bossAI.agent.enabled = true;
    bossAI.muerto = false;
    bossAI.ActualizarFragmentos(0);

    // Resetear la UI
    uiManager.mostrarNinguno();

    */
    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}

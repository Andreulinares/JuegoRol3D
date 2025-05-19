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
        SceneManager.LoadScene("Menu");
        Destroy(gameManager.gameObject);
        Destroy(progressManager.gameObject);
        playerController.transform.position = gameManager.puntoSpawn.position;
        playerController.vidaActualPlayer = playerController.vidaMaxPlayer;
        playerController.manaActualPlayer = playerController.manaMaxPlayer;
        arqueroController.transform.position = gameManager.puntoSpawn.position;
        arqueroController.vidaActualPlayer = arqueroController.vidaMaxPlayer;
        arqueroController.manaActualPlayer = arqueroController.manaMaxPlayer;
        uiManager.mostrarNinguno();
        bossAI.PVActual = bossAI.PVMax;
        bossAI.isApproaching = false;
        bossAI.isChasing = false;
        bossAI.Patrol();
        bossAI.transform.position = bossAI.puntoSpawnBoss.position;
        bossAI.agent.isStopped = false;
        bossAI.agent.enabled = true;
        bossAI.muerto = false;
        bossAI.ActualizarFragmentos(0);

    }
}

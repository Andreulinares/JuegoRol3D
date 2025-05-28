using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IconDiary : MonoBehaviour
{
    public GameObject iconUI;
    public float interactionRango = 2f;
    private Transform player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null)
        { 
            player = GameObject.FindGameObjectWithTag("Player")?.transform;
        }

        float distance = Vector3.Distance(player.position, transform.position);

        if (distance < interactionRango)
        {
            iconUI.SetActive(true);
        }
        else
        {
            iconUI.SetActive(false);
        }
    }

        //El icono mira al player en cualquier direccion
        void LateUpdate()
    {
        iconUI.transform.LookAt(Camera.main.transform);
        iconUI.transform.Rotate(0, 180f, 0); 
    }
}

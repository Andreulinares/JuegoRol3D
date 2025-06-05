using UnityEngine;
using Cinemachine;
using UnityEngine.UI;
using TMPro;

public class Waypoint : MonoBehaviour
{
    public Image img;
    // The target (location, enemy, etc..)
    public Transform target;
    // UI Text to display the distance
    public TMP_Text meter;
    // To adjust the position of the icon
    public Vector3 offset;

    private Transform player;

    private void Update()
    {

        if (player == null)
        { 
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }

        float minX = img.GetPixelAdjustedRect().width / 2;
        
        float maxX = Screen.width - minX;

        
        float minY = img.GetPixelAdjustedRect().height / 2;
        
        float maxY = Screen.height - minY;

        
        Vector2 pos = Camera.main.WorldToScreenPoint(target.position + offset);

        
        if (Vector3.Dot(target.position - transform.position, transform.forward) < 0)
        {
            
            if (pos.x < Screen.width / 2)
            {
                
                pos.x = maxX;
            }
            else
            {
                
                pos.x = minX;
            }
        }

        
        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);

        
        img.transform.position = pos;
        
        meter.text = ((int)Vector3.Distance(target.position, player.position)).ToString() + "m";

        if ( Vector3.Distance(target.position, player.position) < 2f) 
        {
            gameObject.SetActive(false);
        }
    }
}

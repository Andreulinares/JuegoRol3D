using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteElement : MonoBehaviour
{
    // Start is called before the first frame update
    void LateUpdate()
    {
        if (Camera.main == null) return;

        // El sprite mirara siempre Hacia la camara, sera siempre visible
        transform.forward = Camera.main.transform.forward;
    }
}

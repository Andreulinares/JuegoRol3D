using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SubtitleController : MonoBehaviour
{
    // Start is called before the first frame update
    public TextMeshProUGUI substitleText;

    public void MostrarSubtitol(string text)
    {
        substitleText.gameObject.SetActive(true);
    }

    public void AmagaSubtitol()
    {
        substitleText.gameObject.SetActive(false);
    }
}

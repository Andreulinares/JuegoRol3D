using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SubtitleController : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioSource audioSource;
    public TextMeshProUGUI substitleText;
    public float substitleDuration = 3f;

    private bool hasPlayed = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!hasPlayed && other.CompareTag("Player"))
        {
            hasPlayed = true;

            audioSource.Play();
            substitleText.gameObject.SetActive(true);

            Invoke(nameof(AmagaSubstitle), substitleDuration);
        }
    }

    private void AmagaSubstitle()
    {
        substitleText.gameObject.SetActive(false);
    }
}

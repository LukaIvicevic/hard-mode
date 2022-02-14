using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayOneShot : MonoBehaviour
{
    public AudioClip AudioClip { get; set; }

    [SerializeField]
    private AudioSource audioSource;

    void Start()
    {
        SoundManager.Instance.PlayOneShot(audioSource, AudioClip);
    }
}

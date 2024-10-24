using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] AudioClip[] ac_hit;

    AudioSource audioSource;

    void Start()
    {
        instance = this;

        audioSource= GetComponent<AudioSource>();
    }

    public void AudioClip_Play(AudioClip _ac)
    {
        audioSource.PlayOneShot(_ac);
    }

    public void AudioClip_Hit_Play()
    {
        audioSource.PlayOneShot(ac_hit[Random.Range(0, ac_hit.Length)]);
    }
}
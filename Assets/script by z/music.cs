using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    public AudioClip music1;
    public AudioClip music2;

    private AudioSource audioSource;
    private bool hasPlayedDeathMusic = false;

    void Awake()
    {
        // ����ģʽ
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.loop = true;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // ��������1
    public void PlayMusic1()
    {
        audioSource.clip = music1;
        audioSource.Play();
        hasPlayedDeathMusic = false;
    }

    // 2
    public void PlayMusic2()
    {
        if (!hasPlayedDeathMusic)
        {
            audioSource.clip = music2;
            audioSource.Play();
            hasPlayedDeathMusic = true;
        }
    }
}

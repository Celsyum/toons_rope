using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public SoundData[] musicSounds;
    public SoundData[] sfxSounds;

    public AudioSource musicSource;
    public AudioSource sfxSource;


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        PlayMusic("bg");
    }

    public void PlayMusic(string name)
    {
        SoundData sound = System.Array.Find(musicSounds, s => s.name == name);
        if (sound == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        musicSource.clip = sound.clip;

        musicSource.Play();
    }

    public void PlaySFX(string name)
    {
        SoundData sound = System.Array.Find(sfxSounds, s => s.name == name);
        if (sound == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        sfxSource.PlayOneShot(sound.clip);
    }
}

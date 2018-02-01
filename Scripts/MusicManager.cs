using System.Xml.Serialization;
using UnityEngine;
public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    public AudioClip[] sfx;
    public AudioClip[] music;

    void Awake()
    {
        if (Instance)
            DestroyImmediate(gameObject);
        else
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
    }

    public void playSfx(int index)
    {
        GetComponents<AudioSource>()[0].clip = sfx[index];
        GetComponents<AudioSource>()[0].Play();
    }

    public void playMusic(int index, bool state)
    {
            GetComponents<AudioSource>()[1].loop = state;
            GetComponents<AudioSource>()[1].clip = music[index];
            GetComponents<AudioSource>()[1].Play();
    }
}
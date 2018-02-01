using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public GameObject Tutoholder;
    private int index;
    public Sprite[] tuto;
    public Sprite on, off;
    public GameObject sfxPicture;
    private GameObject musicManager;

    void Start()
    {
        Tutoholder.SetActive(PlayerPrefs.GetInt("tuto") == 0);
        PlayerPrefs.SetInt("tuto",1);
        index = 0;
        sfxPicture.GetComponent<Image>().sprite = PlayerPrefs.GetInt("sound") == 1 ? on : off;
        musicManager = GameObject.Find("Music Manager");
        musicManager.GetComponents<AudioSource>()[0].volume = PlayerPrefs.GetInt("sound");
        musicManager.GetComponents<AudioSource>()[1].volume = PlayerPrefs.GetInt("sound");
        musicManager.GetComponent<MusicManager>().playMusic(2, false);
    }

    public void startGame()
    {
        GameObject.Find("Music Manager").GetComponents<AudioSource>()[1].Stop();
        SceneManager.LoadScene("Main");
    }

    public void quitGame()
    {
        Application.Quit();
    }

    public void sfx()
    {
        PlayerPrefs.SetInt("sound",PlayerPrefs.GetInt("sound") == 1 ? 0 : 1);
        sfxPicture.GetComponent<Image>().sprite = PlayerPrefs.GetInt("sound") == 1 ? on : off;
        musicManager.GetComponents<AudioSource>()[0].volume = PlayerPrefs.GetInt("sound");
        musicManager.GetComponents<AudioSource>()[1].volume = PlayerPrefs.GetInt("sound");
        musicManager.GetComponent<MusicManager>().playSfx(0);
    }

    public void credits()
    {
        SceneManager.LoadScene("Credits");
    }

    public void right()
    {
        index++;
        if (index >= tuto.Length) Tutoholder.SetActive(false);
        else Tutoholder.GetComponent<Image>().sprite = tuto[index];
    }
    public void left()
    {
        index--;
        if (index < 0) index = 0;
        Tutoholder.GetComponent<Image>().sprite = tuto[index];

    }

    public void facebpook()
    {
        Application.OpenURL("https://web.facebook.com/JetLightstudio");
    }
    public void insta()
    {
        Application.OpenURL("https://www.instagram.com/jetlightstd/");
    }
    public void playStore()
    {
        Application.OpenURL("http://play.google.com/store/apps/dev?id=5638230701137828274");
    }

}

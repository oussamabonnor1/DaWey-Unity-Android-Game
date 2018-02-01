using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class creditsManager : MonoBehaviour
{
    public string[] infos;
    public GameObject infoPanel;


	void Start () {
		GameObject.Find("Music Manager").GetComponent<MusicManager>().playMusic(1,true);
	}

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameObject.Find("Music Manager").GetComponents<AudioSource>()[1].Stop();
            SceneManager.LoadScene("Menu");
        }
    }
    public void bruddaOne()
    {
        DisplayName(infos[0]);
    }
    public void bruddaTwo()
    {
        DisplayName(infos[1]);
    }
    public void bruddaThree()
    {
        DisplayName(infos[2]);
    }
    public void bruddaFour()
    {
        DisplayName(infos[3]);
    }

    void DisplayName(string text)
    {
        infoPanel.GetComponent<Text>().text = text;
    }
}

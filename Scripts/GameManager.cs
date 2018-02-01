using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public AudioClip[] sfx;
    public GameObject[] Obstacle;
    public GameObject level;
    public GameObject[] battery;
    public GameObject energy;
    public GameObject gameOverPanel;
    public GameObject cam;
    public GameObject bruddaOne;
    public GameObject bruddaTwo;
    public GameObject PausePanel;
    public GameObject ScoreText;
    public GameObject Slider;
    private Vector2 spawnPosition = new Vector2(0, 0);
    public bool leftPlayer;
    public bool transmission;
    public float cameraSpeed;
    public float score;
    public static bool lost;
    public float BatteryLife;
    private int index = 0;

    // Use this for initialization
    void Start()
    {
        lost = false;
        transmission = false;
        score = 0f;
        leftPlayer = Random.Range(1, 3) == 1;
        GameObject targetPlayer;
        targetPlayer = leftPlayer ? bruddaOne : bruddaTwo;
        energy.transform.position = targetPlayer.transform.position;
        BatteryLife = 100;
        StartCoroutine(spawnBatteries());
        StartCoroutine(spawnObstacles());
        if(!GameObject.Find("Music Manager").GetComponents<AudioSource>()[1].isPlaying) GameObject.Find("Music Manager").GetComponent<MusicManager>().playMusic(0,true);
    }

    void Update()
    {
        if (!lost)
        {
            cameraSpeed += Time.fixedDeltaTime/ 800;
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                resume();
            }
            if (Input.GetMouseButtonDown(0) && !transmission)
            {
                leftPlayer = !leftPlayer;
                transmission = true;
                energy.SetActive(true);
                GameObject.Find("Music Manager").GetComponent<MusicManager>().playSfx(1);
                GameObject activePlayer = !leftPlayer ? bruddaOne : bruddaTwo;
                GameObject targetPlayer = leftPlayer ? bruddaOne : bruddaTwo;
                energy.transform.position = activePlayer.transform.position;
                targetPlayer.GetComponent<PlayerController>().speed = cameraSpeed;
            }
            bruddaOne.GetComponent<PlayerController>().active = leftPlayer;
            bruddaTwo.GetComponent<PlayerController>().active = !leftPlayer;
            
            if (Mathf.Min(bruddaOne.transform.position.y, bruddaTwo.transform.position.y)
                - cam.transform.position.y > -0.9)
                cam.transform.position = Vector3.Lerp(cam.transform.position, new Vector3(0, Mathf.Min(bruddaOne.transform.position.y, bruddaTwo.transform.position.y) + 3.2f, -10), cameraSpeed);

            cam.transform.Translate(new Vector3(0, cameraSpeed, 0));

            score += Time.deltaTime * 1.5f;
            ScoreText.GetComponent<Text>().text = "" + (int) score + "M";

            BatteryLife = BatteryLife - Time.deltaTime * 2f;
            Slider.GetComponent<Slider>().value = BatteryLife;
            if (BatteryLife <= 20 && !Slider.GetComponent<Animation>().isPlaying)
            {
                Slider.GetComponent<AudioSource>().Play();
                Slider.transform.GetChild(1).transform.GetChild(0).GetComponent<Image>().color = Color.red;
                Slider.GetComponent<Animation>().Play();
            }
            if(BatteryLife >= 20 && Slider.GetComponent<Animation>().isPlaying)
            {
                Slider.GetComponent<AudioSource>().Stop();
                Slider.transform.GetChild(1).transform.GetChild(0).GetComponent<Image>().color = new Color(71,252,218);
                Slider.GetComponent<Animation>().Stop();
            }
            if (BatteryLife <= 0) lostGame();

            //spawning level road
            GameObject[] roads = GameObject.FindGameObjectsWithTag("road");
            //if (index <= 3)
                if (cam.transform.position.y > roads[roads.Length-1].transform.position.y-1)
                {
                    spawnPosition += new Vector2(0, level.GetComponent<SpriteRenderer>().sprite.bounds.size.y);
                    Instantiate(level, spawnPosition,
                        Quaternion.identity);
                if(roads.Length > 4) Destroy(roads[0]);
                }

            if (transmission)
            {
                GameObject targetPlayer = leftPlayer ? bruddaOne : bruddaTwo;
                energy.transform.position = Vector3.Lerp(energy.transform.position, targetPlayer.transform.position,
                    PlayerController.energySpeed + (5 * Time.deltaTime));
            }
        }
    }

    public void Retart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name.Contains("Brudda"))
        {
            lostGame();
        }
    }
    
    IEnumerator spawnBatteries()
    {
        yield return new WaitForSeconds(Random.Range(4, 7));
        float height = level.GetComponent<SpriteRenderer>().bounds.extents.y;
        int i = Random.Range(1, 3) == 1 ? 1 : -1;
        int j = Random.Range(0, 4);
        if (j > 0) j = 1;
        Instantiate(battery[j], cam.transform.position + new Vector3(i * 1.5f, height, 10), battery[j].transform.rotation);
        StartCoroutine(spawnBatteries());
    }

    IEnumerator spawnObstacles()
    {
        yield return new WaitForSeconds(Random.Range(8, 13));
        float height = level.GetComponent<SpriteRenderer>().bounds.extents.y;
        int i = Random.Range(0, Obstacle.Length);
        int j = Random.Range(1, 3) == 1 ? 1 : -1;
        Instantiate(Obstacle[i], cam.transform.position + new Vector3(j * 1.5f, height, 10),
            Obstacle[i].transform.rotation);
        StartCoroutine(spawnObstacles());
    }

    public void lostGame()
    {
        Handheld.Vibrate();
        lost = true;
        bruddaOne.transform.GetChild(0).gameObject.SetActive(false);
        bruddaTwo.transform.GetChild(0).gameObject.SetActive(false);
        gameOverPanel.SetActive(true);
        if(score > PlayerPrefs.GetInt("score")) PlayerPrefs.SetInt("score", (int) score);
        gameOverPanel.transform.GetChild(2).GetComponent<Text>().text = "Score: " + (int) score +"\nBest Score: "+PlayerPrefs.GetInt("score");
        StopAllCoroutines();
    }

    public void quit()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Menu");
    }


    public void resume()
    {
        Time.timeScale = (int) Time.timeScale == 1 ? 0 : 1;
        lost = Time.timeScale != 1;
        PausePanel.SetActive(Time.timeScale != 1);
    }

}
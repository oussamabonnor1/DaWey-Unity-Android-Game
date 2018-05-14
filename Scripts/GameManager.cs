using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public float Speed;
    public float Acceleration;
    public float RedSpeed;
    public float BlueSpeed;
    public List<GameObject> Roads;
    public GameObject Bridge;
    public GameObject[] Obstacles;
    public Sprite[] RoadsSprites; 
    public GameObject bruddaOne;
    public GameObject bruddaTwo;
    public GameObject energy;
    public bool transmission;
    public static bool lost;
    public bool leftPlayer;
    private float Height;

    // Use this for initialization
    void Start ()
    {
        Height = Camera.main.ScreenToWorldPoint(new Vector2(0, Screen.height)).y;
        leftPlayer = Random.Range(1, 3) == 1;
        GameObject targetPlayer;
        transmission = false;
        targetPlayer = leftPlayer ? bruddaOne : bruddaTwo;
        energy.transform.position = targetPlayer.transform.position;
    }
	
	// Update is called once per frame
	void Update ()
	{
	    //speed = Mathf.Lerp(speed,20f,Acceleration);
	    Speed = 20 * (1 - Mathf.Exp(-Acceleration * Time.time));
        if (!lost)
	    {
	        if (Input.GetKeyDown(KeyCode.Escape))
	        {
	            resume();
	        }
	        if (Input.GetMouseButtonDown(0) && !transmission)
	        {
	            leftPlayer = !leftPlayer;
	            transmission = true;
	            energy.SetActive(true);
	            GameObject activePlayer = !leftPlayer ? bruddaOne : bruddaTwo;
	            energy.transform.position = activePlayer.transform.position;
	        }
	        bruddaOne.GetComponent<PlayerController>().active = leftPlayer;
	        bruddaTwo.GetComponent<PlayerController>().active = !leftPlayer;

	        if (transmission)
	        {
	            GameObject targetPlayer = leftPlayer ? bruddaOne : bruddaTwo;
	            energy.transform.position = Vector3.Lerp(energy.transform.position, targetPlayer.transform.position,
	                PlayerController.energySpeed + ((5 + Speed) * Time.deltaTime));
	        }

	        for (int i = 0; i < Roads.Count; i++)
	        {
	            Roads[i].transform.Translate(0,-Speed * Time.deltaTime,0);
	        }
             for (int i = 0; i < Roads.Count; i++)
	        {
	           if (Roads[i].transform.position.y <= -Height * 2)
	            {
                    // Roads[i].transform.position = new Vector3(0,  Height * 2 -(speed * Time.deltaTime), 0);
	                int j = (i + Roads.Count - 1) % Roads.Count;
                    int SpawnIndex = Random.Range(0,RoadsSprites.Length);
                    Roads[i].GetComponent<SpriteRenderer>().sprite = RoadsSprites[SpawnIndex];
                    if(i==0) SpawnBridge(SpawnIndex);
                    Roads[i].transform.position = new Vector3(0,Roads[j].transform.position.y + 10,0);
                    if(i==1) SpawnObstacle();
	            }
	        }

            RedSpeed = leftPlayer ? +Time.deltaTime: -Time.deltaTime; 
            BlueSpeed = leftPlayer ? -Time.deltaTime: +Time.deltaTime; 
            bruddaOne.transform.Translate(0,RedSpeed,0);
            bruddaTwo.transform.Translate(0,BlueSpeed,0);
        }
	}

    void SpawnBridge(int SpawnIndex){
        if(SpawnIndex == 0 && Random.Range(0,2) == 0) Bridge.SetActive(true);
        else  Bridge.SetActive(false);
    }
    void SpawnObstacle(){
        if(Random.Range(0,2)==0){
                            int ObstacleIndexToActivate = Random.Range(0,2);
                            Obstacles[ObstacleIndexToActivate].SetActive(true);
                            Obstacles[(1 - ObstacleIndexToActivate)%2].SetActive(false);
                            }else{
                                    Obstacles[0].SetActive(false);
                                    Obstacles[1].SetActive(false);
                            }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name.Contains("Brudda"))
        {
            lostGame();
        }
    }

    public void quit()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Menu");
    }

    public void lostGame()
    {
        /*Handheld.Vibrate();
        lost = true;
        bruddaOne.transform.GetChild(0).gameObject.SetActive(false);
        bruddaTwo.transform.GetChild(0).gameObject.SetActive(false);
        gameOverPanel.SetActive(true);
        if (score > PlayerPrefs.GetInt("score")) PlayerPrefs.SetInt("score", (int)score);
        gameOverPanel.transform.GetChild(2).GetComponent<Text>().text =
            "Score: " + (int)score + "\nBest Score: " + PlayerPrefs.GetInt("score");
        StopAllCoroutines();*/
    }

    public void resume()
    {
        Time.timeScale = (int)Time.timeScale == 1 ? 0 : 1;
        lost = Time.timeScale != 1;
    }
}

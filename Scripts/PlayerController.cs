using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool active;
    public float speed;
    public float topSpeed;
    public static float energySpeed;
    public GameObject gameManager;
    public Sprite lightVersion;
    public Sprite darkVersion;

    // Use this for initialization
    void Start()
    {
        speed = 0f;
        energySpeed = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.lost)
        {
            topSpeed += Time.deltaTime / 1000;
            if (active)
            {
                speed += Time.deltaTime / 100;
                energySpeed += Time.deltaTime / 500;
            }
            else
            {
                speed -= Time.deltaTime / 50;
            }
            if (speed < 0) speed = 0;
            if (speed > topSpeed) speed = topSpeed;
            transform.Translate(new Vector2(0, speed));
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name.Contains("Battery"))
        {
            if (active)
            {
                if (other.gameObject.name.Contains("Negative"))
                {
                    GameObject.Find("Music Manager").GetComponent<MusicManager>().playSfx(3);
                    //gameManager.GetComponent<GameManager>().BatteryLife -= 10;
                }
                else
                {
                    GameObject.Find("Music Manager").GetComponent<MusicManager>().playSfx(2);
                    //gameManager.GetComponent<GameManager>().BatteryLife += 20;
                }
                Destroy(other.gameObject);
            }
        }

        if (other.gameObject.tag.Equals("Obstacle"))
        {
            if (!active)
                gameManager.GetComponent<GameManager>().lostGame();
            else
            {
                //GameObject.Find("Music Manager").GetComponent<MusicManager>().playSfx(4);
                GetComponent<Animation>().Play();
            }
        }

        if (other.gameObject.name.Contains("Energy"))
        {
            if (active)
            {
                gameManager.GetComponent<GameManager>().transmission = false;
                gameManager.GetComponent<GameManager>().energy.SetActive(false);
                GetComponent<SpriteRenderer>().sprite = lightVersion;
                transform.GetChild(0).gameObject.SetActive(true);
                if (gameManager.GetComponent<GameManager>().bruddaOne.name.Equals(gameObject.name))
                {
                    GameObject target = gameManager.GetComponent<GameManager>().bruddaTwo;
                    target.GetComponent<SpriteRenderer>().sprite = target.GetComponent<PlayerController>().darkVersion;
                    target.transform.GetChild(0).gameObject.SetActive(false);
                }
                else
                {
                    GameObject target = gameManager.GetComponent<GameManager>().bruddaOne;
                    target.GetComponent<SpriteRenderer>().sprite = target.GetComponent<PlayerController>().darkVersion;
                    target.transform.GetChild(0).gameObject.SetActive(false);
                }
            }
        }
    }
}
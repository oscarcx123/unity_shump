using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb2d;
    public GameObject Bullet;
    public GameObject meteorBrown_big1;
    public GameObject meteorBrown_big2;
    public GameObject meteorBrown_big3;
    public GameObject meteorBrown_big4;
    public GameObject meteorBrown_med1;
    public GameObject meteorBrown_med2;
    public GameObject meteorBrown_small1;
    public GameObject meteorBrown_small2;
    private GameObject[] AllMeteor;
    public float MoveSpeed; // aircraft speed
    public float ShootDelay; // bullet cooldown
    private float LastShotTime;
    public Text ScoreText;
    public int PlayerScore;
    public Text LifeText;
    public int PlayerLife;
    private GameObject[] pauseObjects;
    public Button RestartButton;

    // Start is called before the first frame update
    void Start()
    {
        init_ui();
        rb2d = GetComponent<Rigidbody2D>();
        Button restart_btn = RestartButton.GetComponent<Button>();
        restart_btn.onClick.AddListener(RestartOnClick);
        LastShotTime = -1f;
        init_meteor();
    }

    private void init_ui() {
        PlayerScore = 0;
        PlayerLife = 3;
        SetScoreText();
        SetLifeText();
        pauseObjects = GameObject.FindGameObjectsWithTag("ShowOnPause");
        hidePaused();
    }

    private void init_meteor() {
        int InitialMeteorCount = 15;
        AllMeteor = new GameObject[] {meteorBrown_big1, meteorBrown_big2, meteorBrown_big3, meteorBrown_big4, meteorBrown_med1, meteorBrown_med2, meteorBrown_small1, meteorBrown_small2};
        for (int i = 0; i < InitialMeteorCount; i++) {
            GenerateMeteor();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate() {
        Vector2 PlayerPosition = rb2d.position;
        if (Input.GetKey(KeyCode.UpArrow)) {
            PlayerPosition.y += MoveSpeed;
        }
        if (Input.GetKey(KeyCode.DownArrow)) {
            PlayerPosition.y -= MoveSpeed;
        }
        if (Input.GetKey(KeyCode.LeftArrow)) {
            PlayerPosition.x -= MoveSpeed;
        }
        if (Input.GetKey(KeyCode.RightArrow)) {
            PlayerPosition.x += MoveSpeed;
        }
        if (Input.GetKey(KeyCode.Space)) {
            ShootBullet(PlayerPosition);
        }
        rb2d.MovePosition(PlayerPosition);
    }

    private void ShootBullet(Vector2 PlayerPosition) {
        float CurrTime = Time.time;
        if (CurrTime - LastShotTime > ShootDelay) {
            LastShotTime = CurrTime;
            Vector2 BulletPosition = PlayerPosition + new Vector2(0, +0.5f);
            Instantiate(Bullet, BulletPosition, Quaternion.identity);
        }
    }

    public void GenerateMeteor() {
        Vector2 MeteorPosition = new Vector2(Random.Range(-8.0f, 8.0f), 6);
        int idx = Random.Range(0, AllMeteor.Length);
        Instantiate(AllMeteor[idx], MeteorPosition, Quaternion.identity);
    }

    public void SetScoreText() {
        ScoreText.text = "Score: " + PlayerScore.ToString();
    }

    public void SetLifeText() {
        LifeText.text = "Life: " + PlayerLife.ToString();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Meteor")) {
            PlayerLife -= 1;
            if (PlayerLife <= 0) {
                GameOver();
            }
            else {
                SetLifeText();
            }
        }
    }

    private void GameOver() {
        LifeText.text = "Game Over!";
        pauseControl();
    }

    public void pauseControl() {
        if(Time.timeScale == 1) {
            Time.timeScale = 0;
            showPaused();
        }
        else if (Time.timeScale == 0) {
            Time.timeScale = 1;
            hidePaused();
        }
	}

    public void showPaused() {
		foreach(GameObject i in pauseObjects) {
			i.SetActive(true);
		}
	}

	public void hidePaused() {
		foreach(GameObject i in pauseObjects) {
			i.SetActive(false);
		}
	}

    private void RestartOnClick() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        pauseControl();
    }
}

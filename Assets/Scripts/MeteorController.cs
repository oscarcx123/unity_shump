using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MeteorController : MonoBehaviour
{
    private Rigidbody2D rb2d;
    private int[] MeteorScore = {10, 20, 50};
    private string[] BigMeteor = {"meteorBrown_big1(Clone)", "meteorBrown_big2(Clone)", "meteorBrown_big3(Clone)", "meteorBrown_big4(Clone)"};
    private string[] MedMeteor = {"meteorBrown_med1(Clone)", "meteorBrown_med2(Clone)"};
    private string[] SmallMeteor = {"meteorBrown_small1(Clone)", "meteorBrown_small2(Clone)"};
    private string[] WallTags = {"WallDown", "WallLeft", "WallRight"};
    private string[] ScoreTags = {"Player", "Bullet"};
    // Start is called before the first frame update
    void Start()
    {
        float x_speed = Random.Range(-2.5f, 2.5f);
        float y_speed = Random.Range(1f, 5f);
        int rotationSpeed = Random.Range(15, 45);
        rb2d = GetComponent<Rigidbody2D>();
        rb2d.velocity = new Vector2(x_speed, -y_speed);
        rb2d.angularVelocity = rotationSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (WallTags.Contains(other.gameObject.tag)) {
            DestroyMeteor();
        }
        else if (ScoreTags.Contains(other.gameObject.tag)) {
            CalcScore();
            DestroyMeteor();
        }
    }

    private void DestroyMeteor() {
        PlayerController PlayerCon = GameObject.Find("Player").GetComponent<PlayerController>();
        PlayerCon.GenerateMeteor();
        Destroy(gameObject);
    }

    private void CalcScore() {
        PlayerController PlayerCon = GameObject.Find("Player").GetComponent<PlayerController>();
        if (BigMeteor.Contains(gameObject.name)) {
            PlayerCon.PlayerScore += MeteorScore[0];
        }
        else if (MedMeteor.Contains(gameObject.name)) {
            PlayerCon.PlayerScore += MeteorScore[1];
        }
        else {
            PlayerCon.PlayerScore += MeteorScore[2];
        }
        PlayerCon.SetScoreText();
    }
}

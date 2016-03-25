using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameUI : MonoBehaviour {

    public Image fadePlane;
    public GameObject gameOverUI;
    public Slider healthBarSlider;
    public GameObject BottomLeftPanel;
    public GameObject TopPanel;
    public Text waveText;
    public Text scoreText;
    public Text enemiesLeftText;
    public int score;
    Player player;

	// Use this for initialization
	void Start () {
        FindObjectOfType<Player>().OnDeath += OnGameOver;
        player = FindObjectOfType<Player>();
        healthBarSlider.maxValue = player.startingHealth;
        scoreText.text = "Score: 0";
	}
	
	void OnGameOver() {
        StartCoroutine( Fade(Color.clear, Color.black, 1f) );
        gameOverUI.SetActive( true );
        BottomLeftPanel.SetActive(false);
        TopPanel.SetActive(false);

    }

    void Update()
    {
        healthBarSlider.value = player.health;
    }

    IEnumerator Fade(Color from, Color to, float time ) {
        float speed = 1 / time;
        float percent = 0;

        while ( percent < 1 ) {
            percent += Time.deltaTime * speed;
            fadePlane.color = Color.Lerp( from, to, percent );
            yield return null;
        }
    }

    //UI INput

    public void StartNewGame() {
        Application.LoadLevel( Application.loadedLevel );
    }
}

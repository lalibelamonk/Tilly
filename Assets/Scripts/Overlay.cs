using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Overlay : MonoBehaviour {

	// TODO privatize the text variables
	public Tree tree;
	public Text lifeTotal;
	public Text loserText;
	public Text slainEnemyCount;
	public Text levelText;
	public Text mouseText;
	public CanvasGroup pausePane;
	public CanvasGroup lostPane;
	public CanvasGroup infoPane;

	private int oldHealth;
	private int oldEnemiesSlain;
	private CanvasGroup changingPane;

	private bool gameOver;
	private bool isPaused;
	//private bool completelyFaded;
	private GameController gc;

	// Use this for initialization
	void Start () {
		pausePane.interactable = false;
		pausePane.GetComponent<Canvas>().enabled = false;
		lostPane.interactable = false;
		lostPane.GetComponent<Canvas>().enabled = false;
		isPaused = false;
		gameOver = false;
		//completelyFaded = false;
		gc = GameObject.FindObjectOfType<GameController>();
		oldHealth = -1;
		oldEnemiesSlain = -1;
		levelText.text = gc.level.ToString();
		if (gc.tree != null) {
			tree = gc.tree;
		}
	}
	
	// Update is called once per frame
	void Update () {
		int health = tree.GetHealth();
		if (health != oldHealth) {
			lifeTotal.text = health.ToString();
			oldHealth = health;
			if (health <= 0 && !gameOver) {
				gameOver = true;
				FadePane(lostPane, 2000f, 1f);
			}
		}

		int enemiesSlain = gc.enemiesSlain;
		if (enemiesSlain != oldEnemiesSlain) {
			slainEnemyCount.text = enemiesSlain.ToString();
			oldEnemiesSlain = enemiesSlain;
		}
		/*
		if (Input.GetMouseButtonDown(0)) {
			Vector3 mousePos = Camera.main.ViewportToWorldPoint(Input.mousePosition);
			mouseText.text = (mousePos.x + ":" + mousePos.y);
		}
		*/
		if (Input.GetKeyDown("escape")) {
			TogglePause();
		}
	}

	void TogglePause() {
		isPaused = !isPaused;
		gc.TogglePause(isPaused);
		FadePane(pausePane, 0f, (isPaused ? 1f : 0f));
		FadePane(infoPane, 0f, (isPaused ? 0f : 1f));
	}

	void FadePane(CanvasGroup pane, float time, float alpha) {
		pane.alpha = alpha;
        TogglePane(pane);
	}

	void TogglePane(CanvasGroup pane) {
		pane.interactable = !pane.interactable;
		pane.GetComponent<Canvas>().enabled = !pane.GetComponent<Canvas>().enabled;
	}

    IEnumerator PauseScreen() {
    	bool faded = false;
    	do {
    		Color fadeColor = Color.black;
    		fadeColor.a = .3f;
    		//faded = FadeImage(blackFade, fadeColor, 1.5f);
    		yield return null;
    	} while (!faded);
  	}
}

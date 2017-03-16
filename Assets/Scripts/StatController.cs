using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StatController : MonoBehaviour {

	public Text schmeckles;
	public Text hp;

	private GameController gc;
	private Tree tree;

	private int oldHealth;
	private int oldSchmeckles;

	void Start () {
		gc = GameObject.FindObjectOfType<GameController>();
		tree = gc.tree;

		//add some value
		gc.schmeckles += 75;
	}

	void Update () {
		if (gc.schmeckles != oldSchmeckles) {
			schmeckles.text = gc.schmeckles.ToString();
			oldSchmeckles = gc.schmeckles;
		}
		if (gc.tree.GetHealth() != oldHealth) {
			hp.text = gc.tree.GetHealth().ToString() + " / " + gc.tree.GetMaxHealth().ToString();
			oldHealth = gc.tree.GetHealth();
		}
	}

	public void Repair10() {
		Repair(10, 10);
	}

	public void Repair50() {
		Repair(50, 45);
	}

	public void Upgrade10() {
		Upgrade(10, 100);
	}

	public void Upgrade50() {
		Upgrade(50, 450);
	}

	public void Repair(int amount, int cost) {
		if (gc.schmeckles - cost >= 0 && tree.GetHealth() != tree.GetMaxHealth()) {
			tree.RepairHealth(amount);
			gc.schmeckles -= cost;
		}
	}

	public void Upgrade(int amount, int cost) {
		if (gc.schmeckles - cost >= 0) {
			tree.AddMaxHealth(amount);
			gc.schmeckles -= cost;
		}
	}

	public void NextLevel() {
		gc.GoToScene("default_level");
	}
}

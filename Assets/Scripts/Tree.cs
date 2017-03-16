using UnityEngine;
using System.Collections;
using UnityEngine.UI;

//TODO RENAME TO TREEHOUSE!!!!
public class Tree : Persistent {

	private int health;
	private int maxHealth;
	private GameController gc;

	void Awake () {
		base.Awake();
		gc = GameObject.FindObjectOfType<GameController>();
		gc.tree = this;
	}

	void Start () {
		health = 50;
		maxHealth = 50;
	}

	public int GetHealth() {
		return health;
	}

	public void RepairHealth(int amount) {
		health = Mathf.Min(health + amount, maxHealth);
	}

	public void AddMaxHealth(int amount) {
		maxHealth += amount;
		health += amount;
	}

	public int GetMaxHealth() {
		return maxHealth;
	}

	public int TakeDamage(int damage) {
		health = health - damage;
		return health;
	}

	public void Reset() {
		health = 50;
		maxHealth = 50;
	}
}

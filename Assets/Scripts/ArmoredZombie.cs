using UnityEngine;
using System.Collections;

public class ArmoredZombie : Enemy {

	// Use this for initialization
	public override void Start () {
		base.Start();
		attackSpeed = 1;
		attackDamage = 3;
		hp = 100;
	}
}


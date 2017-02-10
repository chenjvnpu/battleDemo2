using UnityEngine;
using System.Collections;

public class PlayerAttack : BaseAttack {

	// Use this for initialization
	void Start () {
		init ();
	}
	
	// Update is called once per frame
	void Update () {
		OnUpdate ();
	}

	protected override void FindAttackTarget ()
	{
		base.FindAttackTarget ();
	}
}

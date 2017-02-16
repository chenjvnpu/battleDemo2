﻿using UnityEngine;
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
		BattleContle.Instance.state = GameState.PlayerRound;
		int count = SpwanObj.Instance.enemyList.Count;
		if (count > 0) {
			attackTarget=SpwanObj.Instance.enemyList[Random.Range(0,SpwanObj.Instance.enemyList.Count)].transform;
			endPos = attackTarget.position;
		} else {
			BattleContle.Instance.state = GameState.EndRound;
			state = FightState.Idle;
		}

	}

	protected override void GetDamega (int demageNum)
	{
		base.GetDamega (demageNum);
		if(hp<=0){
			SpwanObj.Instance.playerList.Remove (this.gameObject);
			if(SpwanObj.Instance.playerList.Count<=0){
				BattleContle.Instance.state = GameState.EndRound;
			}
			Destroy (this.gameObject);
		}
	}
}

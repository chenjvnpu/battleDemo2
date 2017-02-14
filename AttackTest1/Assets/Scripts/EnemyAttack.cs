using UnityEngine;
using System.Collections;

public class EnemyAttack : BaseAttack {

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
		BattleContle.Instance.state = GameState.EnemyRound;
		attackTarget=SpwanObj.Instance.playerList[Random.Range(0,SpwanObj.Instance.playerList.Count)].transform;
		Debug.Log ("attackTarget.name="+attackTarget.name);
		endPos = attackTarget.position;
	}

	protected override void GetDamega (int demageNum)
	{
		base.GetDamega (demageNum);
		if(hp<=0){
			SpwanObj.Instance.enemyList.Remove (this.gameObject);
			if(SpwanObj.Instance.enemyList.Count<=0){
				BattleContle.Instance.state = GameState.EndRound;
			}

		}
	}
}

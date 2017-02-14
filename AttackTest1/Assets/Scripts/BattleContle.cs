using UnityEngine;
using System.Collections;
public enum GameState{
	SpwanObj,
	WaitRound,
	PlayerRound,
	EnemyRound,
	EndRound
}
public class BattleContle : MonoBehaviour {
	public bool waitRound=false;
	public static BattleContle Instance;
	[HideInInspector]
	public GameState state = GameState.SpwanObj;


	// Use this for initialization
	void Start () {
		Instance = this;
	}
	
	// Update is called once per frame
	void Update () {
		Debug.Log ("state:"+state);
		switch (state) {
		case GameState.WaitRound:
			break;
		case GameState.SpwanObj:
			break;
		case GameState.PlayerRound:
			break;
		case GameState.EnemyRound:
			break;
		case GameState.EndRound:
			break;
		default:
			break;
		}

	
	}

	 bool isWaitRount(){
		if(state == GameState.PlayerRound || state == GameState.EnemyRound){
			return false;
		}else {
			return true;
		}


	}
}

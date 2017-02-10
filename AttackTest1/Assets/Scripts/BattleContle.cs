using UnityEngine;
using System.Collections;

public class BattleContle : MonoBehaviour {
	public bool waitRound=false;
	public static BattleContle Instance;

//	public static BattleContle Instance{
//		get{ 
//			if (instance == null) {
//				instance = this;
//			}
//			return instance;
//		}
//	}
	// Use this for initialization
	void Start () {
		Instance = this;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

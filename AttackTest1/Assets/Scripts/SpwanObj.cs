using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpwanObj : MonoBehaviour {
	public Transform playerSpwanPoint;
	public Transform enemySpwanPoint;
	public List<GameObject> playerList = new List<GameObject> ();
	public List<GameObject> enemyList = new List<GameObject> ();

	void Start () {
		SpwanPlayer ();
		StartCoroutine (SpwanPlayer());
		StartCoroutine (SpwanEnemy());
	}
	
	// Update is called once per frame
	void Update () {
	
	}



	IEnumerator SpwanPlayer(){
		GameObject playerPrefab = Resources.Load<GameObject> ("Prefabs/player1");
		yield return null;
		int max = 9;
		int index = 0;
		for (int i = 0; i < 3; i++) {
			for (int j = 0; j < 3; j++) {
				index++;
				if (index >max)
					break;
				Debug.Log (Constant.Formationx[j]+","+Constant.Formationy[i]);
				GameObject gob= Instantiate (playerPrefab, playerSpwanPoint.position+new Vector3(Constant.Formationx[j],0,Constant.Formationy[i]), Quaternion.identity) as GameObject;
				gob.AddComponent<PlayerAttack> ();
				playerList.Add (gob);
				yield return new WaitForSeconds(1);
			}
		}
	}

	IEnumerator SpwanEnemy(){
		GameObject enemyPrefab = Resources.Load<GameObject> ("Prefabs/enemy");
		yield return null;
		int max = 9;
		int index = 0;
		for (int i = 0; i < 3; i++) {
			for (int j = 0; j < 3; j++) {
				index++;
				if (index >max)
					break;
				Debug.Log (Constant.Formationx[j]+","+Constant.Formationy[i]);
				GameObject gob= Instantiate (enemyPrefab, enemySpwanPoint.position+new Vector3(Constant.Formationx[j],0,-Constant.Formationy[i]), Quaternion.identity) as GameObject;
				gob.AddComponent<EnemyAttack> ();
				enemyList.Add (gob);
				yield return new WaitForSeconds(1);
			}
		}
	}
}

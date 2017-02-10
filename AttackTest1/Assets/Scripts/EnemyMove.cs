using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public enum EnemyState{
	NUll,
	Idle,
	Patrol,
	Chase,
	Attack,
	Dead
}
public class EnemyMove : MonoBehaviour {
	public EnemyState state = EnemyState.NUll;
	public Transform[] points;
	float freTime=2;
	float timmer=0;

	int index=0;
	float speed=3f;
	Transform target;
	float distance_atk=1.5f;
	float distance_chase=8;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		switch (state) {
		case EnemyState.Idle:
			break;
		case EnemyState.Patrol:
			PlayPatrol ();
			break;
		case EnemyState.Chase:
			PlayChase ();
			break;
		default:
			break;
		}

	
	}

	void PlayPatrol(){
		if(Vector3.Distance(transform.position,points[index].position)<1f){
			index++;
			if (index >= points.Length)
				index = 0;
		}
		transform.LookAt (points[index]);
		transform.Translate (transform.forward * Time.deltaTime * speed, Space.World);
		Collider[] cols= Physics.OverlapSphere (transform.position, distance_chase);
		for (int i = 0; i < cols.Length; i++) {
			if(cols[i].tag==Tags.LEADER || cols[i].tag==Tags.PLAYER){
				target = cols [i].transform;
				state = EnemyState.Chase;
			}
		}
	}

	void PlayChase(){
		if (target == null)
			return;
		if (Vector3.Distance (transform.position, target.position) < distance_atk) {
			state = EnemyState.Idle;
			SceneManager.LoadScene ("battle");
		} else if (Vector3.Distance (transform.position, target.position) > distance_chase) {
			state = EnemyState.Patrol;
		} else {
			transform.LookAt (target.position);
			transform.Translate (transform.forward*Time.deltaTime*speed,Space.World);

		}
	
	}

}

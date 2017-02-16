using UnityEngine;
using System.Collections;
public enum FightState{
	Idle,
	Atk,
	Skile,
	Damage,
	Die
}
[RequireComponent(typeof(CharacterController))]
public class BaseAttack : MonoBehaviour {

	public int hp = 100;
	public int baseDamegeNum=50;
	public float moveSpeed=5f;
	protected float distance_atk=2;
	protected Vector3 startEulerAngle;
	protected Vector3 startPos;
	protected Vector3 endPos;
	protected Vector3 targetPos;
	protected CharacterController cc;
	public FightState state=FightState.Idle;

	public float waitTime=2;
	public float timmer=0;

	bool isAtkFinish=false;
	public Transform attackTarget;

	protected virtual void init(){
		timmer=0;
		waitTime = Random.Range (190, 200)/100f;
		cc = this.GetComponent<CharacterController> ();
		startPos = transform.position;
		startEulerAngle=transform.localEulerAngles;
//		System.Random r = new System.Random ();
//		r.NextDouble ();
	}

	protected void OnUpdate(){
		if (hp <= 0) {
			
		} else {
			switch (state) {
			case FightState.Idle:
				if (BattleContle.Instance.state==GameState.WaitRound) {
					timmer += Time.deltaTime * moveSpeed;
					if (timmer >= waitTime) {
						timmer = 0;
						FindAttackTarget ();
					}
				}

				break;
			case FightState.Atk:
				if (isAtkFinish ) {//attack finish
					MoveToBack ();
				} else {//going attack
					moveToPos ();
				}

				break;
			case FightState.Skile:
				PlaySkile ();
				break;
			case FightState.Damage:
				
				break;
			case FightState.Die:
				break;
			default:
				break;
			}
		}
	}
	/// <summary>
	/// 获取攻击对象
	/// </summary>
	protected virtual void FindAttackTarget(){
		state = FightState.Atk;

	}

	protected virtual void GetDamega(int demageNum){
		if (hp > 0) {
			hp -= demageNum;
		}

	}

	protected void moveToPos(){
		
			if (Vector3.Distance (transform.position, endPos) > distance_atk) {
				transform.LookAt (endPos);
				//cc.SimpleMove (transform.forward * moveSpeed);
			transform.Translate(transform.forward*Time.deltaTime*moveSpeed,Space.World);
			} else {
				PlayAttack ();
			}
		

	}

	public virtual void PlayAttack(){
		isAtkFinish = true;
		if(attackTarget!=null){
			attackTarget.GetComponent<BaseAttack> ().GetDamega (baseDamegeNum);
		}
	}

	protected void MoveToBack(){
		if (Vector3.Distance (transform.position, startPos) > 0.1f) {
			transform.LookAt (startPos);
			//cc.SimpleMove (transform.forward * moveSpeed);
			transform.Translate(transform.forward*Time.deltaTime*moveSpeed,Space.World);
		} else {
			isAtkFinish = false;
			state = FightState.Idle;

			transform.localEulerAngles = startEulerAngle;
			BattleContle.Instance.state = GameState.WaitRound;
		}
	}

	protected virtual void PlaySkile(){

	}

}

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
	public int baseDamegeNum=10;
	public float moveSpeed=5f;
	private float distance_atk=2;
	private Vector3 startEulerAngle;
	private Vector3 startPos;
	private Vector3 endPos;
	private Vector3 targetPos;
	private CharacterController cc;
	public FightState state=FightState.Idle;

	public float waitTime=2;
	float timmer=0;

	bool isAtk=false;
	public Transform attackTarget;

	protected virtual void init(){
		cc = this.GetComponent<CharacterController> ();
		startPos = transform.position;
		startEulerAngle=transform.localEulerAngles;
	}

	protected void OnUpdate(){
		if (hp <= 0) {
			
		} else {
			switch (state) {
			case FightState.Idle:
				if (BattleContle.Instance.waitRound) {
					timmer += Time.deltaTime * moveSpeed;
					if (timmer >= waitTime) {
						FindAttackTarget ();
					}
				}

				break;
			case FightState.Atk:
				if (isAtk && attackTarget!=null) {//attack finish
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
		BattleContle.Instance.waitRound = false;
	}

	protected virtual void GetDamega(int demageNum){
		if (hp > 0) {
			hp -= demageNum;
		}
	}

	protected void moveToPos(){
		
			if (Vector3.Distance (transform.position, endPos) > distance_atk) {
				transform.LookAt (endPos);
				cc.SimpleMove (transform.forward * moveSpeed);
			} else {
				PlayAttack ();
			}
		

	}

	public virtual void PlayAttack(){
		isAtk = true;
		if(attackTarget!=null){
			attackTarget.GetComponent<BaseAttack> ().GetDamega (baseDamegeNum);
		}
	}

	protected void MoveToBack(){
		if (Vector3.Distance (transform.position, startPos) > 1f) {
			transform.LookAt (startPos);
			cc.SimpleMove (transform.forward * moveSpeed);
		} else {
			isAtk = false;
			state = FightState.Idle;
			BattleContle.Instance.waitRound = true;
			transform.localEulerAngles = startEulerAngle;
		}
	}

	protected virtual void PlaySkile(){

	}

}

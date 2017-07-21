using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/** 
 *Copyright(C) 2017 by gaea
 *All rights reserved. 
 *FileName:     
 *Author:      csc
 *Version:      1.0.1
 *UnityVersion：5.4.1
 *Date:         
 *Description:    角色控制器，控制角色状态切换
 *History: 
*/

public class RoleCtrl : MonoBehaviour {
	Animator anim;
	public FSMManager roleFsm;
	[SerializeField]
	RoleInfo roleInfo;
	Vector3 originalPos;
	Vector3 originalEulangles;
	bool isShowHeadBar;
//	
	public TeamType teamType=TeamType.LeftTeam;
	public RoleCtrl[] attackTarget=null;
	public GameObject headBarPos;
//	[HideInInspector]
	public HeadBarCtrl headbarCtrl;
	/// <summary>
	/// The can attacked.
	/// 是否可以被攻击
	/// </summary>
	public bool canAttacked=true;
	/// <summary>
	/// 阵型中的位置
	/// </summary>
	public int Index;

	/// <summary>
	/// 战斗移动的终点
	/// The attack position.
	/// </summary>
	public Vector3 attackPos;

	public System.Action<int,int> OnRoleInfoChange;


	public Animator GetAnimator(){
		return anim;
	}


	public void Init(RoleInfo roleInfo,TeamType ttype,HeadBarCtrl headbarCtrl, int index,bool isShowHeadBar=false){
		anim = this.GetComponent<Animator> ();
		this.roleInfo = roleInfo;
		this.teamType = ttype;
		this.headbarCtrl = headbarCtrl;
		this.Index = index;
		this.isShowHeadBar = isShowHeadBar;
		SavePosAndAngles ();
		roleFsm = new FSMManager ();
		roleFsm.AddState ((int)RoleState.Idle,new IdleState(this,roleFsm));
		roleFsm.AddState ((int)RoleState.Run,new RunState(this,roleFsm));
		roleFsm.AddState ((int)RoleState.Attack,new AttackState(this,roleFsm));
		roleFsm.AddState ((int)RoleState.Damage,new DamageState(this,roleFsm));
		//		roleFsm.AddState ((int)RoleState.Stun,new StunState(this,roleFsm));
		roleFsm.AddState ((int)RoleState.Dead,new DieState(this,roleFsm));
		roleFsm.AddState ((int)RoleState.Victory,new VictoryState(this,roleFsm));
		roleFsm.ChangeState ((int)RoleState.Idle);
	}


	void Update () {
//		UpdateHeadBarUIPos ();
		if(roleFsm==null){
			return;
		}
		roleFsm.Update ();
		//		checkState();
		if(BattleCtrl.Instance.battleState==BattleState.Wait){
			roleInfo.curSpeedLength += Time.deltaTime*roleInfo.speed;
			if(roleInfo.curSpeedLength>=ConstData.roundTime){
				if(BattleCtrl.Instance.ActiveRole(this)){
					roleInfo.curSpeedLength = 0;
					BegainAttack ();
				}
			}
		}

	}



	/// <summary>
	/// 测试状态切换
	/// </summary>
	void checkState(){
		if(Input.GetKeyDown(KeyCode.I)){
			roleFsm.ChangeState ((int)RoleState.Idle);
		}
		if(Input.GetKeyDown(KeyCode.R)){
			roleFsm.ChangeState ((int)RoleState.Run);
		}
		if(Input.GetKeyDown(KeyCode.A)){
			roleFsm.ChangeState ((int)RoleState.Attack);
		}
		if(Input.GetKeyDown(KeyCode.D)){
			roleFsm.ChangeState ((int)RoleState.Dead);
		}
	}


	/// <summary>
	/// 初始化攻击信息
	/// Begains the attack.
	/// </summary>
	void BegainAttack(){
		
		if (roleInfo.curMp > roleInfo.maxHp) {
			//释放技能攻击
		}
		//普通攻击
		attackTarget = GetAttackTarget (AttackTeamType.SingleAttack,1,Index).ToArray();
		if (attackTarget == null) {
			Debug.Log ("attack target is null");
			AttackEnd ();
			return;
		}
		attackPos = GetAttackPos (AttackTeamType.SingleAttack);
		roleFsm.ChangeState ((int)RoleState.Run);
	
	}

	/// <summary>
	/// 对攻击对象释放攻击伤害
	/// Dos the attack.
	/// </summary>
	public void DoAttack(){
		roleFsm.ChangeState ((int)RoleState.Attack);
		StartCoroutine (ExcuteAttack ());

	}

	IEnumerator ExcuteAttack(){
		Debug.Log ("------ExcuteAttack-----------");
		float buffValue=1+Random.Range(1,30)/100f;//计算buff加成
		if(attackTarget.Length==1) transform.LookAt(attackTarget[0].transform.position);
		//播放攻击动画特效
		//添加伤害
		for (int i = 0; i < attackTarget.Length; i++) {
			//计算伤害值,
			int attackNum=Mathf.FloorToInt(roleInfo.baseAttackNum*buffValue);
			attackNum = Random.Range (1, 100) > 70 ? attackNum * 2 : attackNum;//有30的概率调暴击（伤害值加倍）
			attackTarget [i].GetDemage (attackNum);
		}
		yield return new WaitForSeconds(1);//等待3s动画特效
		//增加阵营的魔法卡槽的魔力值，准备放大招
		BattleCtrl.Instance.AddTeamMagic(teamType,10);
		roleInfo.curMp+=Random.Range(10,30);//增加技能积点
		if(OnRoleInfoChange!=null){
			ShowHeadBar (true);
			OnRoleInfoChange (roleInfo.curHp,roleInfo.curMp);
		}
		//移动返回
		attackPos=transform.position;
		roleFsm.ChangeState ((int)RoleState.Run);
	}

	public void GetDemage(int damageValue){
		ShowHeadBar (true);
//		Debug.Log ("damageValue:"+damageValue);
		roleInfo.curHp -= damageValue;
		roleFsm.ChangeState ((int)RoleState.Damage);
		if(roleInfo.curHp<=0){
			roleFsm.ChangeState ((int)RoleState.Dead);
		}
		if(OnRoleInfoChange!=null){
			OnRoleInfoChange (roleInfo.curHp,roleInfo.curMp);
		}
	}

	public void Die(){
		BattleCtrl.Instance.roleList.Remove (this);
		BattleCtrl.Instance.CheckBattleEnd (teamType);
		this.gameObject.SetActive (false);
		ShowHeadBar (false);
		Clear ();
		//放入对象池
		PoolManager.Instance.ReleasPoolItem(headbarCtrl.gameObject);
		PoolManager.Instance.ReleasPoolItem(this.gameObject);
	}

	void Clear(){
		roleFsm = null;
		roleInfo = null;
		attackPos = Vector3.zero;
		attackTarget = null;
		
	}

	/// <summary>
	/// 战斗结束，释放数据
	/// Attacks the end.
	/// </summary>
	public void AttackEnd(){
		transform.position=originalPos;
		transform.eulerAngles=originalEulangles;
		attackTarget = null;
		roleFsm.ChangeState ((int)RoleState.Idle);
		BattleCtrl.Instance.RelaseActiveRole ();
//		Debug.Log ("------AttackEnd-----------");
	}


	/// <summary>
	/// 获取攻击目标
	/// Gets the attack target.
	/// </summary>
	/// <returns>The attack target.</returns>
	/// <param name="atkType">Atk type.</param>
	/// <param name="lineNum">Line number.</param>
	private List<RoleCtrl> GetAttackTarget(AttackTeamType atkType,int lineNum=0,int singleIndex=1){
		int columNum = Index % 3;
		if (columNum == 0)
			columNum = 3;
		List<RoleCtrl> targets = BattleCtrl.Instance.GetAttackTarget (atkType,lineNum,columNum,singleIndex);
		if (targets.Count == 0 && BattleCtrl.Instance.battleState!=BattleState.End) {//没有找到攻击目标
			singleIndex--;
			if (singleIndex < 0)
				singleIndex = 5;
			else if (singleIndex == Index)
				return null;
			targets=GetAttackTarget (atkType,lineNum,singleIndex);
		}
			
		return targets;
	}

	Vector3 GetAttackPos(AttackTeamType atkType){
		Vector3 movePos = Vector3.zero;
		switch (atkType) {
		case AttackTeamType.AllAttack://不移动
			break;
		case AttackTeamType.SingleAttack://移动
			if(attackTarget!=null){
				movePos=attackTarget[0].transform.position;
			}else{
				Debug.Log ("attackTarget is null");
				AttackEnd ();
			}

			break;
		case AttackTeamType.LineAttack://不移动
			break;
		case AttackTeamType.ColumAttack://不移动
			break;
		default:
			break;
		}
		return movePos;
	}

	/// <summary>
	/// 控制血条的显示隐藏
	/// Shows the head bar.
	/// </summary>
	/// <param name="isShow">If set to <c>true</c> is show.</param>
	public void ShowHeadBar(bool isShow){
		if(headbarCtrl.gameObject.activeSelf != isShow){
			headbarCtrl.gameObject.SetActive (isShow);
		}

	}

	public void SavePosAndAngles(){
		originalPos = transform.position;
		originalEulangles = transform.eulerAngles;
	}
}

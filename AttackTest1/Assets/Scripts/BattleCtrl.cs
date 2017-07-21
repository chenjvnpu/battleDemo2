using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


/** 
 *Copyright(C) 2017 by gaea
 *All rights reserved. 
 *FileName:     
 *Author:      csc
 *Version:      1.0.1
 *UnityVersion：5.4.1
 *Date:         战斗总控制中心
 *Description:    
 *History: 
*/
public class BattleCtrl : MonoBehaviour
{
	private static BattleCtrl instance;
	public BattleState battleState = BattleState.Load;
	[HideInInspector]
	public List<RoleCtrl> roleList = new List<RoleCtrl> ();

	public RoleCtrl curActiveRole = null;

	public Transform roleContainer;
	/// <summary>
	/// 当前战斗的波数
	/// </summary>
	[SerializeField]
	private int curBattleIndex = 1;
	/// <summary>
	/// 最多发生几波战斗
	/// The max battle number.
	/// </summary>
	[SerializeField]
	private int maxBattleNum = 1;
	private Transform[] battlePosArray;

	private int leftTeamMagic = 0;
	private int RightTeamMagic = 0;
	private static object lockObj = new object ();


	public static BattleCtrl Instance{
		get{ 
			if(instance==null){
				lock(lockObj){
					GameObject gob = new GameObject ("battleCtrl");
					instance= gob.AddComponent<BattleCtrl> ();
				}

			}
			return instance;
		}
	}

	public void Init(Transform roleContainer,int maxBattleNum){
		this.roleContainer = roleContainer;
		InitBattleInfo (maxBattleNum);
	
	}
	void Awake ()
	{
//		Instance = this;
	}



	void Start ()
	{
		
	}

	
	void Update ()
	{
		if(battleState==BattleState.Move ){
			if(Vector3.Distance(roleContainer.position,battlePosArray[curBattleIndex-1].position)>0.1f){
				Vector3 direction = battlePosArray [curBattleIndex-1].position - roleContainer.position;
				roleContainer.Translate (direction.normalized*Time.deltaTime*1,Space.World);
			}else{
				ChangeToBattle ();
			}

		}

	}

	public void InitBattleInfo(int maxBattleNum){
		this.maxBattleNum = maxBattleNum;
		Transform pointParent= GameObject.FindGameObjectWithTag ("points").transform;
		int pointNum = pointParent.childCount;
		maxBattleNum = Mathf.Min (maxBattleNum, pointNum);
		battlePosArray=new Transform[pointNum];
		for (int i = 0; i < pointNum; i++) {
			Transform item= pointParent.FindChild(string.Format("point{0}",i));
			if(item!=null){
				battlePosArray [i] = item;
			}
		}
	}

	/// <summary>
	/// 选择可以被攻击的队列
	/// </summary>
	/// <param name="teamType">Team type.</param>
	/// <param name="atType">At type.</param>
	/// <param name="lineNum">Line number.1:第一排；2：第二排</param>
	/// <param name="columNum">Colum number.</param>
	public List<RoleCtrl> GetAttackTarget (AttackTeamType atType, int lineNum = 1, int columNum = 1,int singleIndex=1)
	{
		List<RoleCtrl> selectRoles = roleList.FindAll (role => {
			if (role.teamType == curActiveRole.teamType) {
				return false;
			}
			switch (atType) {//过滤掉不合适的数据
			case AttackTeamType.LineAttack:
				if (lineNum == 1 && role.Index > 3)
					return false;
				else if (lineNum == 2 && role.Index <= 3)
					return false;
				break;
			case AttackTeamType.ColumAttack:
				int[] targetIndex = GetVisibleIndex (role.Index);
				if (targetIndex == null)
					return false;
				if (Array.IndexOf (targetIndex, role.Index) < 0)
					return false;//数组中不存在该位置数据
				break;
			case AttackTeamType.SingleAttack://攻击和自己位置相同的角色
				if (role.Index != singleIndex)
					return false;
				break;
			case AttackTeamType.AllAttack:
				
				break;
			default:
				break;
			}
			return   role.canAttacked;
		});

		return selectRoles;
	}

	private int[] GetVisibleIndex (int curIndex)
	{
		int[] visibleIndexs = new int[2];
		if (curIndex > 0 && curIndex <= 3) {
			visibleIndexs [0] = curIndex;
			visibleIndexs [1] = curIndex + 3;
		} else if (curIndex > 3 && curIndex <= 6) {
			visibleIndexs [0] = curIndex - 3;
			visibleIndexs [1] = curIndex;
		}

		return visibleIndexs;
	}

	/// <summary>
	/// 激活角色，使角色开始进行攻击，激活成功返回true，失败返回fale
	/// Actives the role.
	/// </summary>
	/// <returns><c>true</c>, if role was actived, <c>false</c> otherwise.</returns>
	/// <param name="role">Role.</param>
	public bool ActiveRole (RoleCtrl role)
	{
		if ( battleState == BattleState.LeftRound || battleState == BattleState.RightRound)
			return false;
		lock (lockObj) {
			if (curActiveRole == null) {
				curActiveRole = role;
				battleState = role.teamType == TeamType.LeftTeam ? BattleState.LeftRound : BattleState.RightRound;
			}
			return true;
		}
	}

	/// <summary>
	/// 释放激活对象
	/// Relases the active role.
	/// </summary>
	public void RelaseActiveRole ()
	{
		curActiveRole = null;
		StartCoroutine (ChangeToWait());

	}

	IEnumerator ChangeToWait(){
		yield return new WaitForSeconds (2);
		if(battleState==BattleState.LeftRound || battleState==BattleState.RightRound)
			battleState = BattleState.Wait;
	}

	/// <summary>
	/// 查看战斗是否结束
	/// Determines whether this instance is battle end.
	/// </summary>
	/// <returns><c>true</c> if this instance is battle end; otherwise, <c>false</c>.</returns>
	public bool IsBattleEnd (TeamType roleTeamType)
	{
		List <RoleCtrl> roles = roleList.FindAll (role => {
			return role.teamType == roleTeamType;
		});
		if (roles.Count <= 0)
			return true;
		else
			return false;
	}

	public void CheckBattleEnd (TeamType roleTeamType)
	{
		List <RoleCtrl> roles = roleList.FindAll (role => {
			return role.teamType == roleTeamType;
		});
		if (roles.Count <= 0) {
			
				if (roleTeamType == (int)TeamType.LeftTeam) {//左侧阵营全部牺牲，普通战役玩家所在的是左侧阵营,敌人胜利
					Debug.Log ("enemy win");
					battleState = BattleState.End;
				} else {//右侧阵营全部牺牲
					Debug.Log ("player  win");
					if (curBattleIndex < maxBattleNum) {
						MoveToNextBattlePoint ();
					} else {
						battleState = BattleState.End;
						ShowVictoryAnim ();
					} 
				}
			


		}
	}

	void ShowVictoryAnim(){
		for (int i = 0; i < roleList.Count; i++) {
			roleList [i].ShowHeadBar (false);
			roleList [i].roleFsm.ChangeState ((int)RoleState.Victory);
		}
	}

	void MoveToNextBattlePoint(){
		battleState = BattleState.Move;
		curBattleIndex++;
		for (int i = 0; i < roleList.Count; i++) {
			roleList [i].roleFsm.ChangeState ((int)RoleState.Run);
		}
	}

	void ChangeToBattle(){
		battleState = BattleState.Load;
		//change to idle
		try {
			for (int i = 0; i < roleList.Count; i++) {
				roleList [i].roleFsm.ChangeState ((int)RoleState.Idle);
				roleList[i].SavePosAndAngles();
			}
		} catch (Exception ex) {
			Debug.Log (ex.ToString ());
		}


		//show enemy
		FightSceneCtrl.Instance.CreateEnemyTeam(1,1,EndLoad);
	}

	/// <summary>
	/// 模型加载结束
	/// </summary>
	void EndLoad(){
		battleState = BattleState.Wait;
	}

	/// <summary>
	/// 增加队伍的魔法值
	/// Adds the team magic.
	/// </summary>
	public void AddTeamMagic(TeamType teamType,int value){
		if (teamType == TeamType.LeftTeam) {
			leftTeamMagic += value;
		} else
			RightTeamMagic += value;
	}
}

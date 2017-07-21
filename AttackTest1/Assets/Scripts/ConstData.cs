
using System.Collections;
using UnityEngine;


/** 
 *Copyright(C) 2017 by gaea
 *All rights reserved. 
 *FileName:     
 *Author:      csc
 *Version:      1.0.1
 *UnityVersion：5.4.1
 *Date:         
 *Description:    
 *History: 
*/
public class ConstData  {
	public static float  roundTime=70;
	/// <summary>
	/// 左侧阵型角色位置
	/// </summary>
	public static Vector3[] leftPos=new Vector3[6]{
		new Vector3(-1,0,-1),new Vector3(0,0,-1),new Vector3(1,0,-1),
		new Vector3(-1,0,-2),new Vector3(0,0,-2),new Vector3(1,0,-2)
	};
	/// <summary>
	/// 右侧阵型角色位置
	/// The right position.
	/// </summary>
	public static Vector3[] rightPos=new Vector3[6]{
		new Vector3(-1,0,1),new Vector3(0,0,1),new Vector3(1,0,1),
		new Vector3(-1,0,2),new Vector3(0,0,2),new Vector3(1,0,2)
	};


}

public enum AnimaClipName{
	Idle,
	Walk,
	Run,
	Talk,
	Dead,
	Damage,
	StandFight,
	Attack,
	Magic,
	Skill01,
	Skill02,
	Victory

}

public enum RoleState{
	Idle=1,
	Walk=2,
	Run=3,
	Talk=4,
	Dead=5,
	Damage=6,
	StandFight=7,
	Attack=8,
	Magic=9,
	Skill01=10,
	Skill02=11,
	Victory=12
}
/// <summary>
/// 加载角色，等待，左侧出击，右侧出击，结束
/// </summary>
public enum BattleState{
	/// <summary>
	/// The load.
	/// 加载角色
	/// </summary>
	Load,
	/// <summary>
	/// The wait.
	/// 等待移动
	/// </summary>
	Wait,
	/// <summary>
	/// The left round.
	/// 左侧角色出击
	/// </summary>
	LeftRound,
	/// <summary>
	/// The right round.
	/// 右侧角色出击
	/// </summary>
	RightRound,
	Move,
	End
}

public enum TeamType{
	/// <summary>
	/// The left team.
	/// 左侧
	/// </summary>
	LeftTeam,
	/// <summary>
	/// The right team.
	/// 右侧
	/// </summary>
	RightTeam
}

/// <summary>
/// 攻击队形类型
/// 横排攻击，纵排攻击，单个攻击，群体攻击
/// </summary>
public enum AttackTeamType{
	LineAttack,
	ColumAttack,
	SingleAttack,
	AllAttack
}

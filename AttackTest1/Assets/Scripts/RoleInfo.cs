using UnityEngine;
using System.Collections;
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
[System.Serializable]
public class RoleInfo  {
	/// <summary>
	/// The name.
	/// 名角色字
	/// </summary>
	public string name;
	/// <summary>
	/// The max hp.
	/// 角色最大血量
	/// </summary>
	public int maxHp;
	/// <summary>
	/// The current hp.
	/// 角色当前的血量
	/// </summary>
	public int curHp;
	/// <summary>
	/// The max mp.
	/// 角色最大魔力值
	/// </summary>
	public int maxMp;
	/// <summary>
	/// The current mp.
	/// 角色当前的魔力值
	/// </summary>
	public int curMp;
	/// <summary>
	/// 基础攻击力
	/// The base attack number.
	/// </summary>
	public int baseAttackNum;
	/// <summary>
	/// The speed.
	/// 角色移动速度
	/// </summary>
	public float speed;
	/// <summary>
	/// The length of the current speed.
	/// 当前等待移动进度
	/// </summary>
	public float curSpeedLength;

}

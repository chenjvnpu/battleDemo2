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
public class RunState : RoleStateBase {

	public RunState (RoleCtrl rc, FSMManager fsm) : base (rc, fsm)
	{
	}

	#region implemented abstract members of RoleStateBase

	public override void EnterState ()
	{
		anim.SetBool (AnimaClipName.Run.ToString(),true);
	}

	public override void LeaveState ()
	{
		anim.SetBool (AnimaClipName.Run.ToString(),false);
	}

	public override void UpdateState ()
	{
		//移动分为战斗过程中的移动和普通过场动画的移动
		if (roleCtrl.attackTarget != null) {//存在攻击对象，战斗过程中的移动
			if (Vector3.Distance (roleCtrl.transform.position, roleCtrl.attackPos) > 0.5f) {
				roleCtrl.transform.LookAt (roleCtrl.attackPos);
				roleCtrl.transform.Translate (roleCtrl.transform.forward * Time.deltaTime * 10, Space.World);
				
			} else {
				if (roleCtrl.attackPos == roleCtrl.transform.position) {//战斗返回到出发的位置，进行站位还原，攻击锁定对象释放
					roleCtrl.AttackEnd ();
				} else {//到达攻击点，进行攻击
					roleCtrl.DoAttack();
				}
			}
			
		} else {
		}
	}

	#endregion
}

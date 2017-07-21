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
public class DamageState : RoleStateBase {
	
	public DamageState (RoleCtrl rc, FSMManager fsm) : base (rc, fsm)
	{
	}

	#region implemented abstract members of RoleStateBase

	public override void EnterState ()
	{
		anim.SetBool (AnimaClipName.Damage.ToString(),true);

	}

	public override void LeaveState ()
	{
		anim.SetBool (AnimaClipName.Damage.ToString(),false);
	}

	public override void UpdateState ()
	{
		animInfo = anim.GetCurrentAnimatorStateInfo (0);
		if(animInfo.IsName(AnimaClipName.Damage.ToString()) &&  animInfo.normalizedTime >=1){//动画播放完成，切换为idle状态
			fsmManager.ChangeState ((int)RoleState.Idle);

		}
	}

	#endregion
}

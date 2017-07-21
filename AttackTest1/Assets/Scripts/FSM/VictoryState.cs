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
public class VictoryState : RoleStateBase {

	
	public VictoryState (RoleCtrl rc, FSMManager fsm) : base (rc, fsm)
	{
	}

	#region implemented abstract members of RoleStateBase

	public override void EnterState ()
	{
		anim.SetBool (AnimaClipName.Victory.ToString(),true);
	}

	public override void LeaveState ()
	{
		anim.SetBool (AnimaClipName.Victory.ToString(),false);
	}

	public override void UpdateState ()
	{

	}

	#endregion

}

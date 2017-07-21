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
public abstract class RoleStateBase:Istate {
	protected RoleCtrl roleCtrl;
	protected FSMManager fsmManager;
	protected Animator anim;
	protected AnimatorStateInfo animInfo;
	public RoleStateBase(RoleCtrl roleCtrl,FSMManager fsm){
		this.roleCtrl = roleCtrl;
		this.fsmManager = fsm;
		anim = roleCtrl.GetAnimator ();
	}
	#region Istate implementation

	public abstract void EnterState ();

	public abstract void LeaveState ();

	public  abstract void UpdateState ();

	#endregion


	

}

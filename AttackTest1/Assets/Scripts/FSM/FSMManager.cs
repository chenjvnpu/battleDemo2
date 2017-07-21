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
 *Description:    
 *History: 
*/
public class FSMManager  {
	public int curStateType;
	public Istate curState;
	Dictionary<int ,Istate> stateDic=new Dictionary<int, Istate>();

//	public FSMManager(){
//	
//	}

	public void AddState(int stateType,Istate state){
		if (stateDic.ContainsKey (stateType)) {
			stateDic [stateType] = state;
		} else
			stateDic.Add (stateType,state);
	}

	public void ChangeState(int stateType){
		if (curStateType == stateType)
			return;
		Istate newState;
		if(stateDic.TryGetValue(stateType,out newState)){
			if (newState == null)
				return;
			if (curState != null) curState.LeaveState ();
			curState = newState;
			curStateType = stateType;
			curState.EnterState ();
		}
	}

	public void Update(){
		if(curState!=null){
			curState.UpdateState ();
		}
	}

	public void Clear(){
		curStateType = -1;
		curState = null;
		stateDic.Clear ();
	}



}

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
public class CameraCtrl : MonoBehaviour {

	Transform target;
	Vector3 offset=Vector3.zero;
	void Start () {
	
	}
	
	
	void Update () {
		if (target != null)
			transform.position = target.position - offset;
	}

	public void SetTarget(Transform target){
		this.target = target;
		offset = target.position - transform.position;
	}
}

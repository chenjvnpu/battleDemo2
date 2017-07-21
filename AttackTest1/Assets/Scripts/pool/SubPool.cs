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
 *Description:  一个subpool只可以存储一种预设的实例化对象  
 *History: 
*/
public class SubPool  {
	/// <summary>
	/// The item list.
	/// </summary>
	Queue <GameObject> itemQueue=new Queue<GameObject>();
	/// <summary>
	/// The name of the prefab.
	/// </summary>
	private string prefabName;
	/// <summary>
	/// 第一次至少实例化多少个对象
	/// The minimum number.
	/// </summary>
	private int minNum;

	public SubPool(string name,int minNum){
		this.prefabName = name;
		this.minNum = minNum;
	}


	/// <summary>
	/// 初始化对象池中对象数量
	/// </summary>
	void InitPool(){
		for (int i = 0; i < minNum; i++) {
			GameObject gob =GameObject.Instantiate (Resources.Load(prefabName)) as GameObject;
			gob.SetActive (false);
			gob.name = prefabName;
			gob.transform.parent = PoolManager.Instance.gameObject.transform;
			itemQueue.Enqueue (gob);
		}
	}

	/// <summary>
	/// 从对象池获取对象.
	/// </summary>
	public GameObject GetItem(){
		GameObject gob = null;
		if (itemQueue.Count > 0) {
			
			gob = itemQueue.Dequeue ();
		} else {
			InitPool ();
			gob = itemQueue.Dequeue ();
		}
		return gob;
	}

	/// <summary>
	/// 把对象放回对象池.
	/// </summary>
	public void RealseItem(GameObject item){
		if(item!=null){
			item.SetActive (false);
			itemQueue.Enqueue (item);
		}
	}
}

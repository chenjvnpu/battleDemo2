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
public class PoolManager :MonoBehaviour {

	private static PoolManager instance;
//	private Transform 
	/// <summary>
	/// The sub pool dic.
	/// 对象池字典，值为对象池预设对象的名称，value为某一对象的对象池
	/// </summary>
	Dictionary<string,SubPool> subPoolDic=new Dictionary<string, SubPool>();

	public static PoolManager Instance{
		get{ 
			if(instance==null){
				GameObject gob = new GameObject ("poolManager");
				instance=gob.AddComponent<PoolManager> ();
			}
			return instance;
		}
	}

	/// <summary>
	/// 创建对象池
	/// Creates the pool.
	/// </summary>
	/// <param name="name">Name.</param>
	/// <param name="minNum">Minimum number.</param>
	public void CreatePool(string name,int minNum=5){
		if(!subPoolDic.ContainsKey(name)){
			SubPool poolItem = new SubPool (name,minNum);
			subPoolDic.Add (name, poolItem);
		}

	}

	/// <summary>
	/// 从对象池获取对象
	/// Gets the pool item.
	/// </summary>
	/// <returns>The pool item.</returns>
	/// <param name="name">Name.</param>
	public GameObject GetPoolItem(string name){
		if (!subPoolDic.ContainsKey (name)) {
			CreatePool (name);
		} 

		return subPoolDic [name].GetItem ();
	}


	/// <summary>
	/// 把对象放回对象池
	/// Releases the pool item.
	/// </summary>
	/// <param name="item">Item.</param>
	public void ReleasPoolItem(GameObject item){
		if(subPoolDic.ContainsKey(item.name)){
			subPoolDic [item.name].RealseItem (item);
		}
	}


}

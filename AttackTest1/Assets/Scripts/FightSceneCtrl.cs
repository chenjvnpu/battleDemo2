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
 *Description:    加载模型对象，初始化场景数据 headBar
 *History: 
*/
public class FightSceneCtrl : MonoBehaviour {


	Queue<GameObject > prefabsHeadbar=new Queue<GameObject>();
	public Transform RolesParent;
	public Transform RolePool;
	[SerializeField]
	private int battleNum = 1;
	Transform UIRoot;
	public static FightSceneCtrl Instance;

	void Awake(){
		Instance = this;
	}
	void Start () {
		UIRoot = GameObject.Find ("Canvas").transform;

		PoolManager.Instance.CreatePool ("fashi",12);
		PoolManager.Instance.CreatePool ("cike",12);
		PoolManager.Instance.CreatePool ("headBar",12);

		InitRoleTeam ();
		BattleCtrl.Instance.Init (RolesParent,battleNum);
		//设置相机跟随对象
		CameraCtrl cc= Camera.main.gameObject.GetComponent<CameraCtrl> ();
		if(cc!=null){
			cc.SetTarget (RolesParent);
		}
	}
		
	
	void Update () {
		

		if(Input.GetKeyDown(KeyCode.S)){
			StartCoroutine(ShowRole ());
		}
	
	}



	IEnumerator ShowRole(){
		BattleCtrl.Instance.battleState = BattleState.Load;
		//show left role
		List<RoleCtrl> leftRoles=BattleCtrl.Instance.roleList.FindAll(role=> {return role.teamType==TeamType.LeftTeam;});
		for (int i = 0; i < leftRoles.Count; i++) {
			leftRoles [i].gameObject.SetActive (true);
		}
		yield return new WaitForSeconds(2);
		//show right role
		List<RoleCtrl> rightRoles=BattleCtrl.Instance.roleList.FindAll(role=> {return role.teamType==TeamType.RightTeam;});
		for (int i = 0; i < rightRoles.Count; i++) {
			rightRoles [i].gameObject.SetActive (true);
		}

		yield return new WaitForSeconds(2);
		BattleCtrl.Instance.battleState = BattleState.Wait;

	
	
	}

	void InitRoleTeam(){
		//left role team
		for (int i = 0; i < 6; i++) {
			string heroName = "fashi";//读取配置表,获取英雄名称
			GameObject gob= PoolManager.Instance.GetPoolItem (heroName);
			addRole (gob,TeamType.LeftTeam,i);
		}

		//right role team

		for (int j = 0; j < 6; j++) {
			string heroName = "cike";//读取配置表,获取敌人名称
			GameObject gob= PoolManager.Instance.GetPoolItem (heroName);
			addRole (gob,TeamType.RightTeam,j);
		}
	}

	/// <summary>
	/// 通过章节和战斗节点，创建对应的敌人
	/// </summary>
	/// <param name="chapter">Chapter.</param>
	/// <param name="battleIndex">Battle index.</param>
	public void CreateEnemyTeam(int chapter,int battleIndex,System.Action onEndCreat){

		for (int j = 0; j < 6; j++) {
			string heroName = "cike";//读取配置表,获取敌人名称
			GameObject gob= PoolManager.Instance.GetPoolItem (heroName);
			addRole (gob,TeamType.RightTeam,j);
		}
		StartCoroutine (ShowEnemy(onEndCreat));
	}

	IEnumerator ShowEnemy(System.Action onEndCreat){
		yield return new WaitForSeconds(3);
		List<RoleCtrl> rightRoles=BattleCtrl.Instance.roleList.FindAll(role=> {return role.teamType==TeamType.RightTeam;});
		for (int i = 0; i < rightRoles.Count; i++) {
			rightRoles [i].gameObject.SetActive (true);
		}
		onEndCreat();
	}



	/// <summary>
	/// 初始化英雄控制器
	/// </summary>
	/// <param name="gob">Gob.</param>
	/// <param name="teamType">Team type.</param>
	/// <param name="Index">Index.</param>
	void addRole(GameObject gob,TeamType teamType,int Index){
		int hp=teamType==TeamType.LeftTeam?Random.Range(2000,4000):Random.Range(200,400);;
		RoleInfo ri = creatRoleInfo (Index,hp);
		int temp = Random.Range (1, 10);

		gob.transform.parent = RolesParent;
		if (teamType == TeamType.LeftTeam) {
			gob.transform.localPosition = ConstData.leftPos [Index];
		} else {
			gob.transform.localPosition = ConstData.rightPos [Index];
		}

		Vector3 offsetPos = RolesParent.position - gob.transform.position;
		float angle = Vector3.Dot (offsetPos, gob.transform.forward);
		if (angle < 0)
			gob.transform.Rotate (0, 180, 0);//围绕y轴旋转180度
		
//		Debug.Log ("index="+Index+", angle="+angle);
		RoleCtrl rc = gob.GetComponent<RoleCtrl> ();
		GameObject headBarUI=PoolManager.Instance.GetPoolItem ("headBar");
		rc.Init (ri,teamType,headBarUI.GetComponent<HeadBarCtrl> (),Index);
		rc.canAttacked = true;
		BattleCtrl.Instance.roleList.Add (rc);
		//add role headBar

			rc.OnRoleInfoChange += headBarUI.GetComponent<HeadBarCtrl> ().OnSliderChanged;
			rc.headbarCtrl = headBarUI.GetComponent<HeadBarCtrl> ();
			headBarUI.GetComponent<HeadBarCtrl> ().InitUI (ri,rc.headBarPos.transform);
			headBarUI.transform.parent= UIRoot.FindChild ("HeadBarContainer");
			headBarUI.transform.localScale = Vector3.one;
			
	}

	/// <summary>
	/// 创建英雄信息，一般读取配置表获得
	/// </summary>
	/// <returns>The role info.</returns>
	/// <param name="id">Identifier.</param>
	/// <param name="maxHp">Max hp.</param>
	RoleInfo creatRoleInfo(int id,int maxHp){
		RoleInfo ri = new RoleInfo ();
		ri.baseAttackNum = 100;
		ri.maxHp = maxHp; 
		ri.curHp = ri.maxHp;
		ri.maxMp = 50;
		ri.curMp = 0;
		ri.speed = Random.Range(20,60);
		ri.name = "role" + id;
		ri.curSpeedLength = 0;
		return ri;
	}

	/// <summary>
	/// 更改战斗速度
	/// </summary>
	void OnGUI(){
		if(GUILayout.Button("1*")){
			Time.timeScale = 1;
			
		}
		if(GUILayout.Button("2*")){
			Time.timeScale = 2;

		}
		if(GUILayout.Button("3*")){
			Time.timeScale = 3;

		}

		if(GUILayout.Button("pause")){
			Time.timeScale = 0;

		}
	}

}

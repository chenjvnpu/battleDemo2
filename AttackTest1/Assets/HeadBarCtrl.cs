using UnityEngine;
using UnityEngine.UI;
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
public class HeadBarCtrl : MonoBehaviour {
	[SerializeField]
	private Slider hpSlider;
	[SerializeField]
	private Slider expSlider;
	/// <summary>
	/// 和那个对象的位置显示一致
	/// The show position target.
	/// </summary>
	Transform showPosTarget;
	RectTransform recTransform;
	Camera uiCamera;
	RoleInfo roleInfo;

	void Start () {
		recTransform = this.GetComponent<RectTransform> ();
		uiCamera = GameObject.Find ("UICamera").GetComponent<Camera> ();
		hpSlider.value = roleInfo.curHp /(float) roleInfo.maxHp;
		expSlider.value = roleInfo.curMp / (float)roleInfo.maxMp;
	}
	
	
	void Update () {
		UpdateHeadBarUIPos ();
	}

	void UpdateHeadBarUIPos(){
		if (showPosTarget == null)
			return;
		Vector3 pos= Camera.main.WorldToViewportPoint (showPosTarget.position);
		pos = uiCamera.ViewportToWorldPoint (pos);
		transform.position = pos;
//		Vector2 screenPos = Camera.main.WorldToScreenPoint (showPosTarget.position);
//		Vector3 globalPos;
//		if (RectTransformUtility.ScreenPointToWorldPointInRectangle (recTransform, screenPos, uiCamera, out globalPos)) {
//			recTransform.position = globalPos;
//		}
	}

	public void InitUI(RoleInfo roleInfo,Transform showPosTarget ){
		this.roleInfo = roleInfo;
		this.showPosTarget = showPosTarget;
	}

	public void OnSliderChanged(int hpValue,int mpValue){
		hpSlider.value = hpValue /(float) roleInfo.maxHp;
		expSlider.value = mpValue / (float)roleInfo.maxMp;
	}

	void ShowPopValue(){
		
	}
}

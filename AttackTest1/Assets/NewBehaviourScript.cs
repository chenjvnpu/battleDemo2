using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class NewBehaviourScript : MonoBehaviour {
	CharacterController cc;
	// Use this for initialization
	void Start () {
		cc = this.GetComponent<CharacterController> ();
	}
	
	// Update is called once per frame
	void Update () {
		float h = Input.GetAxis ("Horizontal");
		float v = Input.GetAxis ("Vertical");
		if(Mathf.Abs(h)>0 || Mathf.Abs(v)>0){
			transform.LookAt (transform.position + new Vector3 (h, 0, v));
			cc.SimpleMove (transform.forward*3f);
		}
	}
}

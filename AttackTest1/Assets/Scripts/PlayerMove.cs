using UnityEngine;
using System.Collections;

public class PlayerMove : MonoBehaviour {
	float speed=5f;
	bool autoMove=false;
	Vector3 desPos=Vector3.zero;


	
	// Update is called once per frame
	void Update () {
		if(!autoMove){
			float h = Input.GetAxis ("Horizontal");
			float v = Input.GetAxis ("Vertical");
			if(Mathf.Abs(h)>0.1f || Mathf.Abs(v)>0.1f){
				transform.LookAt (transform.position + new Vector3 (h, 0, v));
				transform.Translate (transform.forward*Time.deltaTime*speed,Space.World);
			}
		}
		if(Input.GetMouseButtonDown(0)){
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit myHit;
			if(Physics.Raycast(ray,out myHit)){
				Debug.Log (myHit.collider.name+",tag="+myHit.collider.gameObject.tag);

				if(myHit.collider.gameObject.tag ==Tags.FLOOR){

				}
				autoMove = true;
				desPos = myHit.point;
			}
		}


		if(autoMove){
			if (Vector3.Distance (transform.position, desPos) < 1f) {
				autoMove = false;
			} else {
				transform.LookAt (desPos+new Vector3(0,0.5f,0));
				transform.Translate (transform.forward*Time.deltaTime*speed,Space.World);
			}
		}



	
	}
}

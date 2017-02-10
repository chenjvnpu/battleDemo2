using UnityEngine;
using System.Collections;

public class MainCamera : MonoBehaviour {
	public Vector3 offset;
	Transform player;
	// Use this for initialization
	void Start () {
		offset = new Vector3 (0,6,-6);
		player = GameObject.FindGameObjectWithTag (Tags.LEADER).transform;
		//offset = transform.position - player.position;
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = player.position + offset;
	
	}
}

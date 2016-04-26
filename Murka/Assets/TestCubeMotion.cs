using UnityEngine;
using System.Collections;

public class TestCubeMotion : MonoBehaviour
{

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		Ray r = Camera.main.ScreenPointToRay ( Input.mousePosition );
		Vector3 pos = r.GetPoint ( 10 );
		transform.position = pos;
	}
}

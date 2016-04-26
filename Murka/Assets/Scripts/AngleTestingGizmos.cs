using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Shaper.Drawing;

public class AngleTestingGizmos : MonoBehaviour
{
	DrawnTaskShape pic;




	void Awake ()
	{

		pic = GetComponent<DrawnTaskShape> ( );

		//radius = pic.shapeRadius;
		//centerShape = pic.centerShape;
	}

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		//UpdateBounds ( );
	}



}

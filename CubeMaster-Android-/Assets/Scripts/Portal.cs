using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour {

	float _time = 0;
	float _timeWait = 0;

	public float timeParticles = 15f;
	public float timeToTeleport = 7f;
	public GameObject portalEffect;
	public GameObject targetPlatformToTeleport;
	bool canTeleport = true; 
	MainCube main;

	void Start()
	{
		main = FindObjectOfType<MainCube> ();
		GameObject[] objs = GameObject.FindGameObjectsWithTag (this.gameObject.tag);
		foreach (GameObject obj in objs) 
		{
			if (obj != this.gameObject) 
			{
				targetPlatformToTeleport = obj;
			}
		}
	}

	IEnumerator TeleportProcces()
	{
		targetPlatformToTeleport.GetComponent<Portal> ().canTeleport = false;

		StartRotateParticles (timeParticles);
		StartTimerWait (timeParticles);

		while (_timeWait > 0) 
		{
			Wait ();
			yield return new WaitForFixedUpdate ();
		}	

		main.Main.transform.position = new Vector3 (
			targetPlatformToTeleport.transform.position.x, 
			main.Main.transform.position.y,
			targetPlatformToTeleport.transform.position.z		
		);
	}

	void OnTriggerEnter(Collider other)
	{	
		if (other.gameObject.tag == "MainCube" || other.gameObject.tag == "SecondCube") {
			
			RaycastHit hit;
			if (Physics.Raycast(main.Main.transform.position,Vector3.up,out hit,1,main.mask))
			{
				if (hit.collider.gameObject == main.Second) 
				{					
					if (canTeleport)
					{
						StartCoroutine (TeleportProcces());
					}
					else
					{
						StartRotateParticles (10);
					}

				}
			}		
		}	
	}

	void OnTriggerExit(Collider other)
	{
		if (other.gameObject.tag == "MainCube" || other.gameObject.tag == "SecondCube") 
		{
			canTeleport = true;	
		}
	}

	void StartRotateParticles(float __time)
	{
		_time = __time;
	}
	void StartTimerWait(float __time)
	{
		_timeWait = __time;
	}


	void Wait()
	{
		_timeWait -= Time.fixedDeltaTime*10;
	}

	void Update()
	{
		if (_time > 0) 
		{
			portalEffect.SetActive (true);
			_time -= Time.deltaTime*5;
		} 
		else 
		{
			portalEffect.SetActive (false);
		}
	}
}

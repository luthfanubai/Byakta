using UnityEngine;
using System.Collections;

class MyClass
{
	bool isRunning;
	public bool IsRunning 
	{ 
		get { return isRunning; } 
	}

	public MyClass(MonoBehaviour monoBehaviour, IEnumerator iEnumerator)
	{
		monoBehaviour.StartCoroutine (Play(iEnumerator));
	}

	IEnumerator Play(MonoBehaviour monoBehaviour, IEnumerator ienumerator)
	{
		isRunning = true;
		yield return monoBehaviour.StartCoroutine (ienumerator);
		isRunning = false;
	}
}

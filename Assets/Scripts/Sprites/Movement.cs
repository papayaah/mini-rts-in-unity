using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Animations;

public class Movement : MonoBehaviour
{
	public float speed = 0.25f;
	private Vector3 targetPosition;
	private Animator animator; 

	void Start()
	{
		animator = GetComponent<Animator>();
		
	}

	public void MoveToTarget(Vector3 target)
	{
		targetPosition = target;
		animator = GetComponent<Animator>();
		if (animator)
		{
			animator.Play("PawnWalk"); 
		}
	}

	void Update()
	{
		if (targetPosition != null)
		{
			float step = 2f * Time.deltaTime;
			transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);

			// Optionally, stop moving when the pawn reaches the target
			if (transform.position == targetPosition)
			{
				// Implement what happens when the pawn reaches the target
				PlayBuildAnimation();
				
				StartCoroutine(PlayIdle(2f));
			}
		}
	}
	
	IEnumerator PlayIdle(float delay)
	{
		yield return new WaitForSeconds(delay);
		
		animator.Play("PawnIdle");
	}
	
	private void PlayBuildAnimation()
	{
		if (animator != null)
		{
			animator.SetTrigger("buildTrigger");
		}
	}
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponRotation : MonoBehaviour
{
   // Angular speed in radians per sec.

	[SerializeField] Transform sword = null;
	Vector3 objectPos;
	Vector3 mousePos;
	float angle;

	private CharacterController characterController;
	[SerializeField] GameObject player;

	public Animator animator;
	public float delay = 0.3f;

	private bool attackDisabled;

	public bool isAttacking;

	[SerializeField] Transform raycastOrigin;
	[SerializeField] float radius;
	void Start()
	{
		characterController = player.GetComponent<CharacterController>();
	}

	void Update()
	{
		if (isAttacking)
		{
			return;
		}

		mousePos = Input.mousePosition;
		mousePos.z = 5.23f; //The distance between the camera and object
		objectPos = Camera.main.WorldToScreenPoint(sword.position);
		mousePos.x = mousePos.x - objectPos.x;
		mousePos.y = mousePos.y - objectPos.y;
		angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.Euler(0, 0, angle);
	}

	public void Attack()
	{
		if (attackDisabled)
		{
			return;
		}


		animator.SetTrigger("Attack");
		characterController.canMove = false;
		characterController.rb.velocity = Vector3.zero;
		isAttacking = true;
		attackDisabled = true;
		characterController.stamina -= 10;

		StartCoroutine(DelayAttack());
	}

	public void ResetIsAttacking()
	{
		isAttacking = false;
		characterController.canMove = true;
	}

	private IEnumerator DelayAttack()
	{
		yield return new WaitForSeconds(delay);
		attackDisabled = false;
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.blue;
		Vector3 position = raycastOrigin == null ? Vector3.zero : raycastOrigin.position;
		Gizmos.DrawWireSphere(position, radius);
	}

	public void ColliderDetection()
	{
		foreach (Collider2D collider in Physics2D.OverlapCircleAll(raycastOrigin.position, radius))
		{
			EnemyController enemy;

			if(enemy = collider.GetComponent<EnemyController>())
			{
				enemy.TakeDamage(15f);
			}

		}
	}
}

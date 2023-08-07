using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour //Placeholder script for Enemy functionality
{
	[SerializeField] FloatingHealthBar healthBar;
	[SerializeField] float health = 75;
	[SerializeField] float maxHealth = 75;
	[SerializeField] float speed = 3f;

	[SerializeField] float damage = 15f;
	[SerializeField] float attackSpeed = 1f;
	float attackTimer;

	private Transform target;
	[SerializeField] Transform originPoint;

	void Start()
	{
		healthBar = GetComponentInChildren<FloatingHealthBar>();
	}

	void Update()
    {
        if(target != null) //Moves the Enemy towards the Player if they are set as a target
		{
				float step = speed * Time.deltaTime;
				transform.position = Vector2.MoveTowards(transform.position, target.position, step);
		}

		if(target == null) //Moves the Enemy back to its starting position whenever the Player leaves the Enemy's detection radius.
		{
			float step = speed * Time.deltaTime;
			transform.position = Vector2.MoveTowards(transform.position, originPoint.position, step);
		}
	}

	public void TakeDamage(float mod)
	{
		health -= mod;
		healthBar.UpdateHealthBar(health, maxHealth);

		if (health <= 0f)
		{
			Destroy(gameObject);
		}
	}


	private void OnCollisionStay2D(Collision2D collision) //Damages Player on collision.
	{
		if (collision.gameObject.tag == "Player")
		{

			if (attackSpeed <= attackTimer)
			{
				collision.gameObject.GetComponent<CharacterController>().TakeDamage(-damage);
				attackTimer = 0f;
			}

			else
			{
				attackTimer += Time.deltaTime;
			}
		}
	}

	private void OnTriggerEnter2D(Collider2D collision) //Assigns the Player as a target when entering the Enemy's detection radius.
	{
		if(collision.gameObject.tag == "Player")
		{
			target = collision.transform;
		}
	}

	private void OnTriggerExit2D(Collider2D collision) //Deassigns the Player when they leave the detection radius
	{
		if (collision.gameObject.tag == "Player")
		{
			target = null;
		}
	}
}

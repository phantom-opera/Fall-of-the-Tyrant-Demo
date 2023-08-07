using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour //Placeholder Script for Player functionality
{

	[SerializeField] bool isInvincible;

	public float health = 100;
	public float maxHealth = 100;
	public float stamina = 100;
	public float maxStamina = 100;

	private float staminaRegenTimer = 0.0f;
	private const float staminaIncreasePerFrame = 25.0f;
	private const float staminaTimeToRegen = 3.0f;

	public bool canMove = true;
	private bool isRunning = false;
	private float moveSpeed = 5f;
	private float runSpeed = 10f;
	public Rigidbody2D rb;

	private bool canDash = true;
	private bool isDashing;
	private float dashingPower = 24f;
	private float dashingTime = 0.2f;
	private float dashingCooldown = 1f;

	private Vector2 moveInput;

	[SerializeField] GameObject weaponParent;
	private WeaponRotation weaponRotation;
	


	// Update is called once per frame
	void Start()
	{
		weaponRotation = weaponParent.GetComponent<WeaponRotation>();
	}

	void Update()
    {
		if (isDashing) //Resets the stamina regen timer after dashing.
		{
			staminaRegenTimer = 0.0f;
			return;
		}

		if (canMove) //Allows the player to move when set to true.
		{
			moveInput.x = Input.GetAxisRaw("Horizontal");
			moveInput.y = Input.GetAxisRaw("Vertical");
			moveInput.Normalize();

			rb.velocity = moveInput * moveSpeed;
		}

		if (Input.GetKeyDown(KeyCode.RightShift)) // Character runs when Right Shift is pressed
		{
			if (!isRunning)
			{
				if(stamina != 0)
				{
					isRunning = true;
					moveSpeed = runSpeed;
					staminaRegenTimer = 0.0f;
				}
			}

			else
			{
				isRunning = false;
				moveSpeed = 5f;
			}
		}

		if (Input.GetMouseButtonDown(0)) //Attack button
		{
			weaponRotation.Attack();
			stamina -= 10;

			staminaRegenTimer = 0.0f;
		}

		if (isRunning) //Player will move faster when set to true.
		{
			stamina -= 25 * Time.deltaTime;

			if (stamina <= 0)
			{
				isRunning = false;
				moveSpeed = 5f;
			}
		}

		if (Input.GetKeyDown(KeyCode.Space) && canDash && stamina > 0) //Dash Button
		{
			stamina -= 25;
			StartCoroutine(Dash());
			becomeInvincible();
		}

		if(stamina < maxStamina) //Regenerates stamina to full whenever it is not capped.
		{
			if (staminaRegenTimer >= staminaTimeToRegen)
			{
				stamina = Mathf.Clamp(stamina + (staminaIncreasePerFrame * Time.deltaTime), 0.0f, maxStamina);
			}

			else
			{
				staminaRegenTimer += Time.deltaTime;
			}
		}
	}

	private void becomeInvincible()
	{
		if (!isInvincible)
		{
			StartCoroutine(Invincible());
		}
	}

	public void TakeDamage(float mod)
	{
		if (isInvincible)
		{
			return;
		}

		health += mod;

		if(health <= 0f)
		{

		}
	}

	private IEnumerator Dash()
	{
		canDash = false;
		isDashing = true;
		float originalGravity = rb.gravityScale;
		rb.gravityScale = 0f;
		rb.velocity = moveInput * dashingPower;
		yield return new WaitForSeconds(dashingTime);
		rb.gravityScale = originalGravity;
		isDashing = false;
		yield return new WaitForSeconds(dashingCooldown);
		canDash = true;
	}

	private IEnumerator Invincible() //Invincibility frames
	{
		isInvincible = true;
		Debug.Log("Player turned invincible!");

		yield return new WaitForSeconds(0.3f);

		isInvincible = false;
		Debug.Log("Player is no longer invincible!");
	}
}

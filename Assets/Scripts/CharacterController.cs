using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{

	public float health = 100;
	public float maxHealth = 100;
	public float stamina = 100;
	public float maxStamina = 100;

	private float staminaRegenTimer = 0.0f;
	private const float staminaIncreasePerFrame = 25.0f;
	private const float staminaTimeToRegen = 3.0f;

	public bool canMove = true;
	public bool isRunning = false;
	public float moveSpeed;
	public float runSpeed;
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
		if (isDashing)
		{
			staminaRegenTimer = 0.0f;
			return;
		}

		if (canMove)
		{
			moveInput.x = Input.GetAxisRaw("Horizontal");
			moveInput.y = Input.GetAxisRaw("Vertical");
			moveInput.Normalize();

			rb.velocity = moveInput * moveSpeed;
		}

		if (Input.GetKeyDown(KeyCode.RightShift))
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

		if (Input.GetMouseButtonDown(0))
		{
			weaponRotation.Attack();
			stamina -= 10;

			staminaRegenTimer = 0.0f;
		}

		if (isRunning)
		{
			stamina -= 25 * Time.deltaTime;

			if (stamina <= 0)
			{
				isRunning = false;
				moveSpeed = 5f;
			}
		}

		if (Input.GetKeyDown(KeyCode.Space) && canDash && stamina > 0)
		{
			stamina -= 25;
			StartCoroutine(Dash());
		}

		if(stamina < maxStamina)
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

	public void TakeDamage(float mod)
	{
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
}

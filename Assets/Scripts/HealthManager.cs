using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
	private CharacterController characterController;
	[SerializeField] GameObject player;

	public Slider healthBar;

	// Start is called before the first frame update
	void Start()
	{
		characterController = player.GetComponent<CharacterController>();
		healthBar.maxValue = characterController.maxHealth;

	}

	// Update is called once per frame
	void Update()
	{
		healthBar.value = characterController.health;
	}
}

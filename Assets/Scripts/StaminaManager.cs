using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class StaminaManager : MonoBehaviour
{
	private CharacterController characterController;
	[SerializeField] GameObject player;

	public Slider staminaBar;

    // Start is called before the first frame update
    void Start()
    {
		characterController = player.GetComponent<CharacterController>();
		staminaBar.maxValue = characterController.maxStamina;

	}

    // Update is called once per frame
    void Update()
    {
		staminaBar.value = characterController.stamina;
    }
}

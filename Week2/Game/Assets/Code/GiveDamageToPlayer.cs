using UnityEngine;
using System.Collections;

public class GiveDamageToPlayer : MonoBehaviour {

	public int DamageToGive = 10;

	private Vector2
		lastPosition,
		velocity;

	// Update is called once per frame
	void LateUpdate()
	{
		velocity = (lastPosition - (Vector2)transform.position) / Time.deltaTime; // how many units we are moving per second
		lastPosition = transform.position;
	}

	public void OnTriggerEnter2D(Collider2D coll)
	{
		var player = coll.GetComponent<Player>();
		if (player == null)
			return;

		player.TakeDamage(DamageToGive, gameObject);
		var controller = player.GetComponent<CharacterController2D>();
		var totalVelocity = controller.Velocity + velocity;

		// calculate direction (left = -1; right = 1) and then multiply it by totalVelocity (clamped) for knockback effect
		controller.SetForce(new Vector2(
			-1 * Mathf.Sign(totalVelocity.x) * Mathf.Clamp(Mathf.Abs(totalVelocity.x) * 6, 10, 40),
			-1 * Mathf.Sign(totalVelocity.y) * Mathf.Clamp(Mathf.Abs(totalVelocity.y) * 6, 5, 10)));
	}
}

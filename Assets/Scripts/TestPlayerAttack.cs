using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayerAttack : MonoBehaviour
{
	private Ray ray;
	private bool isAttacking;
	[SerializeField] private GameObject punchEffect;


	void Start()
	{
		ray = new Ray(transform.position, transform.forward);
	}

	void Update()
	{
		if (Input.GetButtonDown("Fire1"))
        {
            StartCoroutine(Punch());
        }
	}

	private IEnumerator Punch()
	{
		isAttacking = true;
		Vector2 direction = new Vector2(0, 0);
		RaycastHit2D[] nearColliders = Physics2D.CircleCastAll(transform.position, 2f, direction);
		foreach (RaycastHit2D hit in nearColliders)
		{
			BaseEnemy enemy = hit.collider.GetComponent<BaseEnemy>();
			if (enemy)
			{
				enemy.ReactToHit(1);
			}
		}
		GameObject effectObject = Instantiate<GameObject>(punchEffect);
		effectObject.transform.position = transform.position;
		yield return new WaitForSeconds(.1f);
		Destroy(effectObject);
		isAttacking = false;
	}
}

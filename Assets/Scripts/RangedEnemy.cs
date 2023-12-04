using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RangedEnemy : TrainingBotController
{
    [Header("Projectile Settings")]
    [SerializeField] private GameObject weaponPrefab;
    [SerializeField] private int attackDamage;
    [SerializeField] private AudioSource attackSFX;

    private GameObject weaponObject;


    //temporary implementation until animations are made
    protected override IEnumerator Attack()

    {
        print("Ranged enemy Attacking");
        if (anim)
        {
            anim.SetTrigger("Attack");
            yield return new WaitForSeconds(attackAnimDuration);
            anim.ResetTrigger("Attack");
        }
        else
        {
            yield return new WaitForSeconds(0f);
        } 

        //check if there is a weapon prefab to instantiate
        if (weaponPrefab)
        {
            if (attackSFX)
            {
                attackSFX.Play();
            }
            //print("needle instantiated");
            //creates object to be rotated
            weaponObject = Instantiate(weaponPrefab) as GameObject;
            weaponObject.transform.position = transform.position;

            //rotate weapon to be towards the player
            Vector3 rot = weaponObject.transform.rotation.eulerAngles;

            // division of basicAttackRange is to keep two numbers below 1 to avoid an error message saying Assertion failed on expression
            float xDirection = (PlayerStats._instance.transform.position.x - transform.position.x) / attackRadius;
            float yDirection = (PlayerStats._instance.transform.position.y - transform.position.y) / attackRadius;
            //Debug.Log("xDirection: " + xDirection + "; yDirection: " + yDirection);
            Vector2 moveDir = new Vector2(xDirection, yDirection);
            rot.z = Mathf.Acos(Vector2.Dot(Vector2.up, moveDir)) * Mathf.Rad2Deg;
            if (moveDir.x > 0) { rot.z *= -1f; }
            weaponObject.transform.rotation = Quaternion.Euler(rot.x, rot.y, rot.z);

            Projectile projectile = weaponObject.GetComponent<Projectile>();
            projectile.direction = targetTransform.position - transform.position;
            projectile.damage = attackDamage;
            projectile.attackLayer = 15;
        }

        

    }




}

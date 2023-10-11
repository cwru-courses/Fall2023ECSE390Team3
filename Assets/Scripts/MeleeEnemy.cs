using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MeleeEnemy : TrainingBotController
{
    [Header("Melee Settings")]
    [SerializeField] private GameObject weaponPrefab;
    [Range(0f, 180f)]
    [SerializeField] private float attackRangeAngle;
    [Min(0)]
    [SerializeField] private float attackDuration;
    [SerializeField] private int attackDamage;

    private GameObject weaponObject;



    //temporary implementation until animations are made
    protected override IEnumerator Attack()
    {

        PlayerStats._instance.TakeDamage(attackDamage);

        //if animator is set up activate attack trigger
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

        //below is the temporary implementation for the melee animation

        //check if there is a weapon prefab to instantiate
        if (weaponPrefab)
        {
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
            float offset = attackRangeAngle / 2f;
            rot.z = Mathf.Acos(Vector2.Dot(Vector2.up, moveDir)) * Mathf.Rad2Deg;
            if (moveDir.x > 0) { rot.z *= -1f; }
            rot.z -= offset;
            weaponObject.transform.rotation = Quaternion.Euler(rot.x, rot.y, rot.z);

            //set up parameters for weapon to rotate
            RotateWeapon rw = weaponObject.GetComponentInChildren<RotateWeapon>();
            rw.attackRangeAngle = attackRangeAngle;
            rw.attackDuration = attackDuration;
        }

        //wait for attack duration
        yield return new WaitForSeconds(attackDuration);
        if (weaponObject)
        {
            //after swing time delete the weapon object
            Destroy(weaponObject);
            
        }

    }
    private void OnDestroy()
    {
        Destroy(weaponObject);
    }




}

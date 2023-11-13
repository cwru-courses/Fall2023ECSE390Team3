using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideWallsWhenEnemiesEradicated : MonoBehaviour
{
    [SerializeField] private GameObject[] objectsToWatch;
    [SerializeField] private wallOpenClose wall1;
    [SerializeField] private wallOpenClose wall2;
    [SerializeField] private AudioSource wallUp;
    [SerializeField] private CameraControl came;
    [SerializeField] private Vector3 wallPosition;
    bool allDestroyed;

    // Start is called before the first frame update
    void Start()
    {
        allDestroyed = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!allDestroyed)
        {
            bool destroyed = true;

            // check if all GameObjects(enemies) are destoried already
            foreach (GameObject obj in objectsToWatch)
            {
                if (obj != null)
                {
                    destroyed = false;
                    break;
                }
            }

            if (destroyed)
            {
                allDestroyed = true;
                StartCoroutine(WallDisappear());
            }
        }
        
    }

    private IEnumerator WallDisappear()
    {
        if (came != null)
        {
            Vector3 flippedWallPosition = new Vector3(wallPosition.x, -wallPosition.y, 0);
            Vector3 playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position;
            float distanceToFlippedWall = Vector3.Distance(playerPosition, flippedWallPosition);
            float distanceToNormalWall = Vector3.Distance(playerPosition, wallPosition);

            Vector3 watchWall;
            if (distanceToFlippedWall < distanceToNormalWall)
            {
                // player is closer to wall in flipped world
                watchWall = flippedWallPosition;
            }
            else
            {
                // player is closer to wall in normal world
                watchWall = new Vector3(wallPosition.x, wallPosition.y, 0);
            }

            
            // watch door to open
            came.SwitchToBossRoom(watchWall);
            if (wall1 != null)
            {
                StartCoroutine(wall1.toOpen());
            }
            if (wall2 != null)
            {
                StartCoroutine(wall2.toOpen());
            }
            if (wallUp != null)
            {
                wallUp.Play();
            }

            yield return new WaitForSeconds(3.5f);

            // get back to player
            came.SwitchToPlayerFocus();
        }
    }

}

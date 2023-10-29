using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideWallsWhenEnemiesEradicated : MonoBehaviour
{
    [SerializeField] private GameObject[] objectsToWatch;
    [SerializeField] private GameObject wall1;
    [SerializeField] private GameObject wall2;
    [SerializeField] private AudioSource wallUp;
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
                WallDisappear();
            }
        }
        
    }

    private void WallDisappear()
    {
        if (wall1 != null)
        {
            wall1.SetActive(false);
        }
        if (wall2 != null)
        {
            wall2.SetActive(false);
        }
        if (wallUp != null)
        {
            wallUp.Play();
        }
    }

}

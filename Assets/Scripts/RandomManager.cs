using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RandomManager : MonoBehaviour
{
    public GameObject itemToRandomize;
    public GameObject[] toShuffle;
    private Vector3 defaultPosition = new Vector3(0, 0, 0);
    private Vector3[] shuffledPositions;
    private int[] usedIndexes = {7, 8, 9, 10, 11, 12};
    public Material[] possibleMaterials;
    private static RandomManager instance;
    public static RandomManager Instance { get { return instance; } }

    private void Awake()
    {
        if (instance != null && instance != this) { Destroy(this.gameObject); }
        else instance = this;
        shuffledPositions = new Vector3[] { defaultPosition, defaultPosition, defaultPosition, defaultPosition, defaultPosition, defaultPosition };
        IEnumerator spawnTimer = SpawnTimer();
        StartCoroutine(spawnTimer);
    }

    IEnumerator SpawnTimer()
    {
        while (true)
        {
            GameObject newObject = Instantiate(itemToRandomize, new Vector3(Random.Range(-7, 7), Random.Range(0, 10), Random.Range(-7, 7)), Quaternion.identity);
            newObject.transform.Rotate(new Vector3(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360)));
            newObject.transform.localScale = new Vector3(Random.Range(0.5f, 5), Random.Range(0.5f, 5), Random.Range(0.5f, 5));
            newObject.GetComponent<Renderer>().material = possibleMaterials[Random.Range(0, possibleMaterials.Length)];
            newObject.SetActive(true);
            //Debug.Log("Spawned");
            yield return new WaitForSeconds(Random.Range(.5f, 1.5f));
        }
    }

    private void Update()
    {
        //SHUFFLING CODE===========================================================================================================================================
        if(Input.GetKeyDown(KeyCode.S) && toShuffle.Length > 0)
        {
            for (int i = 0; i < toShuffle.Length; i++)
            {
                shuffledPositions[i] = toShuffle[i].transform.position;
            }
            for (int i = 0; i < toShuffle.Length; i++)
            {
                int randomIndex = 7;//magic number
                while (randomIndex ==7 || usedIndexes.Contains(randomIndex))
                {
                    randomIndex = Random.Range(0, toShuffle.Length);
                }
                usedIndexes.SetValue(randomIndex, i);
                //Debug.Log(i.ToString() + " " + randomIndex.ToString());

                toShuffle[i].transform.position = shuffledPositions[randomIndex];
               
            }

            //reset usedIndexes to initial values
            usedIndexes = new int[] { 7, 8, 9, 10, 11, 12 };
        }
        //POOLING CODE=============================================================================================================================================

        GameObject pooledCubes = ObjectPool.Instance.GetPooledObject();
        if (Input.GetKeyDown(KeyCode.Space) && pooledCubes != null)
        {
            pooledCubes.transform.position = new Vector3(Random.Range(-7, 7), Random.Range(0, 10), Random.Range(-7, 7));
            pooledCubes.transform.Rotate(new Vector3(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360)));
            pooledCubes.transform.localScale = new Vector3(Random.Range(0.5f, 5), Random.Range(0.5f, 5), Random.Range(0.5f, 5));
            pooledCubes.GetComponent<Renderer>().material = possibleMaterials[Random.Range(0, possibleMaterials.Length)];
            pooledCubes.SetActive(true);
        }

        if (pooledCubes != null && pooledCubes.transform.position.y < 0)
        {
            pooledCubes.SetActive(false);
        }
}
}

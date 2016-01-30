using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemSpawner : MonoBehaviour {

    private float maxX;
    private float minX;
    private float maxZ;
    private float minZ;
    Collider thisCollider;
    public List<string> ingredientObjects;
    private Transform[] spawnedObjects;
    int itemCount;

    public GameObject mushroom;
    public GameObject tinCan;
    public GameObject pizza;
    public GameObject rope;
    public GameObject tomato;
    public GameObject newtEye;

	// Use this for initialization
	void Start () {
        thisCollider = this.GetComponent<Collider>();
        maxX = thisCollider.bounds.max.x;
        minX = thisCollider.bounds.min.x;
        maxZ = thisCollider.bounds.max.z;
        minZ = thisCollider.bounds.min.z;


        //this is very hacky pls forgive me
        //unity is bad
        ingredientObjects.Add(mushroom.name);
        ingredientObjects.Add(tinCan.name);
        ingredientObjects.Add(pizza.name);
        ingredientObjects.Add(rope.name);
        ingredientObjects.Add(tomato.name);
        ingredientObjects.Add(newtEye.name);
        itemCount = 0;

        StartCoroutine("SpawnObject");
        StartCoroutine("DespawnObject");

	}
	
	// Update is called once per frame
	void Update () {

        //ROTATE ALL ITEMS ON GROUND
        spawnedObjects = this.GetComponentsInChildren<Transform>();


        foreach (Transform spawn in spawnedObjects)
        {
            //rotate selected item
            if ((spawn.gameObject != null) && (spawn.gameObject.tag == "Ingredient"))
            {
                spawn.gameObject.transform.localEulerAngles = new Vector3(spawn.gameObject.transform.localEulerAngles.x, spawn.gameObject.transform.localEulerAngles.y + (20.0f * Time.deltaTime), spawn.gameObject.transform.localEulerAngles.z);
            }
        }

	}

    void CreateObject(int item)
    {
        Vector3 itemPos = new Vector3(Random.Range(minX, maxX), 0.0f, Random.Range(minZ, maxZ));


        //create object
        GameObject displayItem = (GameObject)Instantiate(Resources.Load(ingredientObjects[item]), itemPos, Quaternion.identity);

        displayItem.transform.SetParent(this.transform);
        displayItem.transform.localEulerAngles = new Vector3(0.0f, 90.0f, 0.0f);
    }

    IEnumerator SpawnObject()
    {
        yield return new WaitForSeconds(Random.Range(0.0f, 5.0f));

        int tempItem = Random.Range(0, 6);

        if (itemCount <= 8)
        {
            CreateObject(tempItem);
            itemCount += 1;
        }
        StartCoroutine("SpawnObject");
    }

    IEnumerator DespawnObject()
    {
        yield return new WaitForSeconds(Random.Range(2.0f, 5.0f));

        if (this.transform.childCount > 0)
        {
            Destroy(this.transform.GetChild(0).gameObject);
            itemCount -= 1;
        }
        StartCoroutine("DespawnObject");
    }

}

using UnityEngine;
using System.Collections;

public class BottleSpawner : MonoBehaviour {

    public GameObject bottle;
    private bool spawnBottle;

	// Use this for initialization
	void Start () {
        spawnBottle = true;
        StartCoroutine("WaitForRespawn");
	}
	
	// Update is called once per frame
	void Update () {

        if (this.transform.childCount == 0)
        {
            spawnBottle = true;
            StartCoroutine("WaitForRespawn");
        }

        //Rotate in place
        if (this.transform.childCount > 0)
        {
            this.transform.GetChild(0).gameObject.transform.localEulerAngles = new Vector3(this.transform.GetChild(0).gameObject.transform.localEulerAngles.x, this.transform.GetChild(0).gameObject.transform.localEulerAngles.y + (20.0f * Time.deltaTime), this.transform.GetChild(0).gameObject.transform.localEulerAngles.z);
        }
    }

    void InstantiateBottle()
    {
        Vector3 itemPos = new Vector3(0.0f, 0.0f, 0.0f);

        //create object
        GameObject spawnedBottle = (GameObject)Instantiate(Resources.Load(bottle.name), itemPos, Quaternion.identity);

        spawnedBottle.transform.SetParent(this.transform);
        spawnedBottle.transform.localPosition = itemPos;
        spawnedBottle.transform.localEulerAngles = new Vector3(0.0f, 90.0f, 0.0f);
        spawnedBottle.GetComponent<BoxCollider>().isTrigger = true;
        spawnBottle = false;
    }

    IEnumerator WaitForRespawn()
    {
        yield return new WaitForSeconds(1.5f);
        if (spawnBottle == true)
        {
            InstantiateBottle();
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

public class FractureObject : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject ObjectNotFractured;
    public GameObject ObjectFractured;

    public float explosionMinForce = 5;
    public float explosionMaxForce = 100;
    public float explosionRadius = 10;
    public float fragScaleFactor = 1;

    private GameObject fractObj;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyUp(KeyCode.K))
        {
            Explode();
        }

        if (Input.GetKeyUp(KeyCode.L))
        {
            Reset();
        }
    }

    void Explode()
    {
        if(ObjectNotFractured != null)
        {
            ObjectNotFractured.SetActive(false);

            if (ObjectFractured != null)
            {
                fractObj = Instantiate(ObjectFractured, ObjectNotFractured.transform.localPosition, Quaternion.identity);
                foreach (Transform t in fractObj.transform)
                {
                    var rb = t.GetComponent<Rigidbody>();

                    if (rb != null)
                        rb.AddExplosionForce(Random.Range(explosionMinForce, explosionMaxForce), ObjectNotFractured.transform.position, explosionRadius);

                    StartCoroutine(Shrink(t, 2));
                }
                Destroy(fractObj, 5);
            }
        }
    }

    public void Reset()
    {
        Destroy(fractObj);
        ObjectNotFractured.SetActive(true);
    }

    IEnumerator Shrink(Transform t,float delay)

    {
        yield return new WaitForSeconds(delay);
        Vector3 newScale = t.localScale;

        while(newScale.x >= 0)
        {
            newScale -= new Vector3(fragScaleFactor, fragScaleFactor, fragScaleFactor);

            t.localScale = newScale;
            yield return new WaitForSeconds(0.05f);
        }
    }
}

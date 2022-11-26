using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Int_MoveItem : MonoBehaviour, IInteractable
{
    private bool canMove = false;
    private bool canMoveMiddle = false;
    [SerializeField]
    private List<GameObject> MiddlePoints;
    [SerializeField]
    private float speed;
    private int listIndex = 0;
    // Start is called before the first frame update
    
    public void Interact()
    {
        if (MiddlePoints.Count > 0)
        {
            
            canMoveMiddle = true;
        }
        else
        {
            canMove = true;
        }
    }

    public bool MoveObjectEnd(Vector3 endPoint)
    {
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, MiddlePoints[listIndex].transform.position, step);
        float distanceFromEnd = Vector3.Distance(transform.position, MiddlePoints[listIndex].transform.position);
        if (distanceFromEnd <= 0.01f)
        {
            canMove = false;
            return true;
        }
        return false;
    }

    void Update()
    {
        if (canMoveMiddle)
        {
            if (MoveObjectEnd(MiddlePoints[listIndex].transform.position))
            {
                listIndex++;
            }
            if(listIndex >= MiddlePoints.Count)
            {
                canMoveMiddle = false;
            }
        }
    }
}

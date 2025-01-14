using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistancePerception : Perception
{

    public override GameObject[] GetGameObjects()
    {
        List<GameObject> result = new List<GameObject>();

        var colliders = Physics.OverlapSphere(transform.position, maxDistance);
        foreach( var collider in colliders)
        {
            //do not include ourselves
            if (collider.gameObject == gameObject) continue;
            //chaeck for matching tag
            if (tagName == "" || collider.tag == tagName)
            {
                //check if with angle in range
                Vector3 direciton = collider.transform.position - transform.position;
                float angle = Vector3.Angle(direciton, transform.forward);
                if(angle <= maxAngel)
                {
                    result.Add(collider.gameObject);
                 }
            }
        }


        return result.ToArray();
    }
}

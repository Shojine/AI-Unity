using Unity.VisualScripting;
using UnityEngine;

public class AutonomousAgent : AIAgent
{
    [Header("Wander")]
    public AutonomousAgentData data;

    [Header("Perception")]
    public Perception seekPerception;
    public Perception fleePerception;
    public Perception flockPerception;

    float angle;

    private void Update()
    {
        float size = 15;
        //movement.ApplyForce(Vector3.forward * 10);



        if (seekPerception != null)
        {
            //SEEK
            var gameObjects = seekPerception.GetGameObjects();
            if (gameObjects.Length > 0)
            {
                Vector3 force = Seek(gameObjects[0]);
                movement.ApplyForce(force);
            }
        }

        if (fleePerception != null)
        {
            //FLEE
            var gameObjects = fleePerception.GetGameObjects();
            if (gameObjects.Length > 0)
            {
                Vector3 force = Flee(gameObjects[0]);
                movement.ApplyForce(force);
            }
        }



        if(flockPerception != null)
        {
            var gameObjects = seekPerception.GetGameObjects();
            if (gameObjects.Length > 0)
            {
                movement.ApplyForce(Cohesion(gameObjects));
              //  movement.ApplyForce(Separation(gameObjects
                //Vector3 force = Cohesion
                //movement.ApplyForce();
            }
        }


        //WANDER
        if (movement.Acceleration.sqrMagnitude == 0)
        {
            movement.ApplyForce(Wander());
        }


        Vector3 acceleration = movement.Acceleration;
        acceleration.y = 0;
        movement.Acceleration = acceleration;

        if (movement.Direction.sqrMagnitude != 0)
        {
            transform.rotation = Quaternion.LookRotation(movement.Direction);
        }



        //foreach (var go in gameObjects)
        //{
        //    Debug.DrawLine(transform.position, go.transform.position, Color.red);

        //}
        transform.position = Utilities.Wrap(transform.position, new Vector3(-size, -size, -size), new Vector3(size, size, size));
    }


    private Vector3 Wander()
    {
        //randomly adjust angle +/- displacement
        angle += Random.Range(-data.displacement, data.displacement);
        //create rotation quaternion around y axis (up)
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.up);
        Vector3 point = rotation * (Vector3.forward * data.radius);
        Vector3 forward = movement.Direction * data.distance;
        Vector3 force = GetSteeringForce(forward + point);
        movement.ApplyForce(force);
        return force;
    }

 
   
    private Vector3 Seek(GameObject go)
    {
        Vector3 direction = go.transform.position - transform.position;
        Vector3 force = GetSteeringForce(direction);

        return force;
    }

    private Vector3 Cohesion(GameObject[] neighbors)
    {
        Vector3 positions = Vector3.zero;
        foreach (GameObject neighbor in neighbors)
        {
            positions += neighbor.transform.position;

        }

        Vector3 center = positions / neighbors.Length;
        Vector3 direction = center - transform.position;

        Vector3 force = GetSteeringForce(direction);

        return force;
    }
    private Vector3 Separation(GameObject[] neighbors, float radius)
    {
        return Vector3.zero;
    }
    private Vector3 Alighment(GameObject[] neighbors)
    {
        return Vector3.zero;
    }
   

    private Vector3 Flee(GameObject go)
    {
        Vector3 direction = transform.position - go.transform.position;
        Vector3 force = GetSteeringForce(direction);

        return force;
    }

    private Vector3 GetSteeringForce(Vector3 direction)
    {
        Vector3 desired = direction.normalized * movement.data.maxSpeed;
        Vector3 steer = desired - movement.Velocity;
        Vector3 force = Vector3.ClampMagnitude(steer, movement.data.maxForce);

        return force;

    }


}

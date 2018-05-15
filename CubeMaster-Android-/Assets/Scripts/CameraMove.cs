using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    GameObject target;


    private void Start()
    {
        SetTarget();  
    }

    public void SetTarget()
    {
        target = GameObject.FindGameObjectWithTag("MainCube");
    }

    private void Move()
    {
        try
        {
            Vector3 _target = target.transform.position;
            _target.y = transform.position.y;
            _target.x -= 2f;
            transform.position = Vector3.MoveTowards(transform.position, _target, Time.deltaTime * 10);
        }
        catch (MissingReferenceException)
        {
            SetTarget();
        }
        catch (System.NullReferenceException)
        {
            SetTarget();
        }
        catch (System.ArgumentNullException)
        {
            SetTarget();
        }
    }

    void Update()
    {
        Move();
    }
}

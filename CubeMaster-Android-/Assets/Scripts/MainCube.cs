using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MainCube : MonoBehaviour
{
    public float speed = 1f;

    [HideInInspector]
    public bool canMove = true;
    public LayerMask mask, secondMask, groundMask;

    public Quaternion Orig;

    Quaternion quaternion;

    [HideInInspector]
    public GameObject Main, Second;

    public GameHandler game;

    private void Awake()
    {
        game = GameObject.Find("GameHandler").GetComponent<GameHandler>();
        Main = GameObject.Find("MainCube").gameObject;
        Second = Main.transform.Find("SecondCube").gameObject;
        Orig = Main.transform.rotation;
        game.stars = new Stars();
    }

    private void Start()
    {
        Awake(); 
    }

    private void OnBecameVisible()
    {
        Awake();
    }
	
	
	public void RefreshCube()
	{
		Awake();		
	}
	
	
    public void ChangeCube(GameObject _main, GameObject _second)
    {
        Second.transform.SetParent(Main.transform.parent);
        Main.transform.SetParent(Second.transform);
        Main = _second;
        Second = _main;
        Main.name = "MainCube";
        Second.name = "SecondCube";
        Main.layer = 2;
        Second.layer = 8;
    }

    private void Move() //delete after
    {
        if ((Input.GetButtonDown("Horizontal") || Input.GetButton("Horizontal")) && canMove)
        {
            canMove = false;
            string side = (Input.GetAxisRaw("Horizontal") > 0) ? "right" : "left";
            Moving(side);
        }
        if ((Input.GetButtonDown("Vertical") || Input.GetButton("Vertical")) && canMove)
        {
            canMove = false;
            string side = (Input.GetAxisRaw("Vertical") > 0) ? "up" : "down";
            Moving(side);
        }
    }

    void SwitchHelper(float _angle, Vector3 _target, Vector3 _altTarget, Vector3 _rotateVector, Vector3 _rayVector)
    {
        RaycastHit hit;
        Ray ray = new Ray(Main.transform.position, Vector3.up);
        if (Physics.Raycast(ray, out hit, 1, mask))
        {
            if (hit.collider.gameObject.layer == 8)
            {
                ray = new Ray(Main.transform.position, _rayVector);

                if (Physics.Raycast(ray, out hit, 2, mask))
                {
                    if (hit.collider.tag != "Block")
                    {
                        quaternion = Quaternion.AngleAxis(_angle, _rotateVector);
                        StartCoroutine(MoveHelper(_target));
                        StartCoroutine(RotateHelper(quaternion * Main.transform.rotation));
                        return;
                    }
                }
                else
                {
                    quaternion = Quaternion.AngleAxis(_angle, _rotateVector);
                    StartCoroutine(MoveHelper(_target));
                    StartCoroutine(RotateHelper(quaternion * Main.transform.rotation));
                    return;
                }
            }
        }
        else
        {
            ray = new Ray(Main.transform.position, _rayVector);
            if (Physics.Raycast(ray, out hit, 1, mask))
            {
                if (hit.collider.gameObject.layer == 8)
                {
                    ChangeCube(Main, Second);

                    ray = new Ray(Main.transform.position, _rayVector);
                    if (Physics.Raycast(ray, out hit, 1, mask))
                    {
                        if (hit.collider.tag != "Block")
                        {
                            quaternion = Quaternion.AngleAxis(_angle, _rotateVector);
                            StartCoroutine(MoveHelper(_altTarget));
                            StartCoroutine(RotateHelper(quaternion * Main.transform.rotation));
                            return;
                        }
                    }
                    else
                    {
                        quaternion = Quaternion.AngleAxis(_angle, _rotateVector);
                        StartCoroutine(MoveHelper(_altTarget));
                        StartCoroutine(RotateHelper(quaternion * Main.transform.rotation));
                        return;
                    }

                }
                else if (hit.collider.tag != "Block")
                {
                    if (Physics.Raycast(new Ray(Main.transform.Find("SecondCube").position, _rayVector), out hit, 1, secondMask))
                    {
                        if (hit.collider.tag != "Block")
                        {
                            quaternion = Quaternion.AngleAxis(_angle, _rotateVector);
                            StartCoroutine(MoveHelper(_target));
                            StartCoroutine(RotateHelper(quaternion * Main.transform.rotation));
                            return;
                        }
                        else
                        {
                            quaternion = Quaternion.AngleAxis(_angle, _rotateVector);
                            StartCoroutine(MoveHelper(_target));
                            StartCoroutine(RotateHelper(quaternion * Main.transform.rotation));
                            return;
                        }
                    }
                }
            }
            else
            {
                if (Physics.Raycast(new Ray(Main.transform.Find("SecondCube").position, _rayVector), out hit, 1, secondMask))
                {
                    if (hit.collider.tag != "Block")
                    {
                        quaternion = Quaternion.AngleAxis(_angle, _rotateVector);
                        StartCoroutine(MoveHelper(_target));
                        StartCoroutine(RotateHelper(quaternion * Main.transform.rotation));
                        return;
                    }
                }
                else
                {
                    quaternion = Quaternion.AngleAxis(_angle, _rotateVector);
                    StartCoroutine(MoveHelper(_target));
                    StartCoroutine(RotateHelper(quaternion * Main.transform.rotation));
                    return;
                }
            }
        }


        canMove = true;
        return;
    }

    public void Moving(string side)
    {
        float x = Main.transform.position.x;
        float y = Main.transform.position.y;
        float z = Main.transform.position.z;

        switch (side)
        {
            case "up":
                SwitchHelper(-90, new Vector3(x + 1, y, z), new Vector3(x + 2, y, z), Vector3.forward, Vector3.right);
                break;

            case "down":
                SwitchHelper(90, new Vector3(x - 1, y, z), new Vector3(x - 2, y, z), Vector3.forward, -Vector3.right);
                break;

            case "left":
                SwitchHelper(90, new Vector3(x, y, z + 1), new Vector3(x, y, z + 2), Vector3.right, Vector3.forward);
                break;

            case "right":
                SwitchHelper(-90, new Vector3(x, y, z - 1), new Vector3(x, y, z - 2), Vector3.right, -Vector3.forward);
                break;
        }
    }

    void CheckEnd()
    {
        RaycastHit hit;
        Ray ray = new Ray(Main.transform.position, -Vector3.up);
        if (Physics.Raycast(Main.transform.position,Vector3.up,out hit,1,mask))
        {
            if (hit.collider.gameObject == Second)
            {
                if (Physics.Raycast(ray, out hit, 1, groundMask))
                {
                    if (hit.collider.CompareTag("End"))
                    {
                        game.Win();
                        //Effects 
                    }
                }
            }
        }        
    }

    IEnumerator MoveHelper(Vector3 target)
    {


        for (; Main.transform.position != target;)
        {
            Main.transform.position = Vector3.MoveTowards(Main.transform.position, target, Time.deltaTime * speed);
            yield return new WaitForFixedUpdate();
        }

    }

    IEnumerator RotateHelper(Quaternion rotate)
    {
        for (; Main.transform.rotation != rotate;)
        {
            Main.transform.rotation = Quaternion.RotateTowards(Main.transform.rotation, rotate, Time.deltaTime * speed * 90);
            yield return new WaitForFixedUpdate();
        }
        CheckEnd();
        game.stars.ComputeCurr();
        canMove = true;
    }

    private void Update()
    {
        Move();
    }
}

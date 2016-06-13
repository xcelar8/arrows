using UnityEngine;
using System.Collections;

public class objectScript : MonoBehaviour {

    public GameObject playerR;
    public GameObject playerG;
    public GameObject playerY;
    public GameObject playerB;

    private GameObject currObj;

    private ParticleSystem pSys;
    private ParticleSystemShapeType sph;
    private ParticleSystemShapeType con;

    public float speed;
    public float speedH;
    public float speedR;

    private int score;
    public float scoreErrorMultiplier = 25.0f;
    public float errorThreshold = 0.6f;

    private float[] goals = { 1.1f, -1.0f, 3.3f, 5.6f };

    int randCol;
    int randDir;

    int completed = 0;
    int ready = 0;

    public Vector3 newPos;
    public Vector3 newRot;
    public Quaternion q;

    // Use this for initialization
    void Start () {
        ready = 1;
        speed = 0.1f;
        score = 0;
        currObj = playerR;
        speedR = 10;     
    }
	
	// Update is called once per frame
	void Update () {
	    checkOutside();
        if(ready == 1)
        {
            getRandSelection();
            rotate();
        }
        if(ready == 0)
        {      
            move();
        }
    }

    void checkOutside()
    {
    if(currObj.transform.position.z > 9.0f | Mathf.Abs(currObj.transform.position.x) > 5.0f)
        {
            currObj.transform.position = new Vector3(0.0f, 0.5f, -10.0f);
            completed = 1;
            ready = 1;
        }
    }

    void getRandSelection()
    {
        randCol = (int)Random.Range(1, 5);
        randDir = (int)Random.Range(1, 3);
        switch(randCol)
        {
            case 1:
                currObj = playerY;
                break;
            case 2:
                currObj = playerR;
                break;
            case 3:
                currObj = playerG;
                break;
            case 4:
                currObj = playerB;
                break;
            default:
                break;
        }
        changeSphere();
        ready = 0;
        completed = 0;
        speed = 0.1f;
        speedH = 0.0f;
    }

    void rotate()
    {
        if (randDir == 2)
            q = Quaternion.Euler(new Vector3(0.0f, 90.0f, 0.0f));
        if (randDir == 1)
            q = Quaternion.Euler(new Vector3(0.0f, -90.0f, 0.0f));
    }

    void move()
    {
        if (Input.GetMouseButtonDown(0))
        {
            changeCone();
            speed = 0.0f;
            int scoreAdd = 0;
            float err = Mathf.Abs(currObj.transform.position.z - goals[randCol - 1]);
            if (err < 0.6f)
                err = 0;
            if (err > 1.3f)
                err = 2.0f;
            scoreAdd = 10 - (int)(scoreErrorMultiplier * err);
            if (Input.mousePosition.x < 150.0)
            {
                speedH = 0.5f;
                if (randDir == 1)
                    scoreAdd = -50;
            }
            else
            {
                speedH = -0.5f;
                if (randDir == 2)
                    scoreAdd = -50;
            }
            score += scoreAdd;
            Debug.Log(score);
        }

        newPos = currObj.transform.position;
        newPos.z += speed * 1.0f;
        newPos.x += speedH * 1.0f;
        newRot = q.eulerAngles;
        newRot.z += speedR * 1.0f;
        q = Quaternion.Euler(newRot);
        currObj.transform.position = newPos;
        currObj.transform.rotation = q;
    }

    void changeCone()
    {
        pSys = currObj.GetComponent<ParticleSystem>();
        var shape = pSys.shape;
        shape.enabled = true;
        shape.shapeType = ParticleSystemShapeType.Cone;
        shape.angle = 25;
        shape.radius = 0.05f;
        shape.randomDirection = false;
    }

    void changeSphere()
    {
        pSys = currObj.GetComponent<ParticleSystem>();
        var shape = pSys.shape;
        shape.enabled = true;
        shape.shapeType = ParticleSystemShapeType.Sphere;
        shape.radius = 0.2f;
        shape.randomDirection = false;
    }
}


//-1.0 - r
//1.1 - y
//3.3 - g 
//5.6 - b
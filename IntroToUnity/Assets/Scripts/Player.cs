using TMPro;
using UnityEngine;

public class Player : Unit
{
	[SerializeField] TextMeshProUGUI textObject;
	[SerializeField] float speed = 1f;

	// awake
	private void Awake()
	{
        Debug.Log("Awake Unity");
        //GameManager
        //Controller
        //Handler
        //Spawner
	}
	// called once during the start of the game
	void Start()
    {
        //player
        //enemy
        //maps

        Debug.Log("Hello Unity");
        //hp
        //attached
    }

	private void FixedUpdate()
	{
		//physics

        //60 - 48
	}

	private void LateUpdate()
	{
		//Debug.Log("Unity Late Update");
        // camera follower
        // paimon
	}

	void TestInput()
	{
		if (Input.GetKeyDown(KeyCode.Space))    // this once only 
		{
			Debug.Log("PRESS DOWN");
		}
		if (Input.GetKey(KeyCode.Space))        // while holding
		{
			Debug.Log("PRESSING");
		}
		if (Input.GetKeyUp(KeyCode.Space))  // this once only
		{
			Debug.Log("PRESS UP");
		}
	}

	// Update is called once per frame
	void Update()
	{
		//movement
		//timer

		//Debug.Log("Unity Update");

		//60
		Movement();
	}

	void Movementv1()
	{
		if (Input.GetKey(KeyCode.W))       // up
		{
			this.transform.position = new Vector3(this.transform.position.x, 0, this.transform.position.z + speed);
			Debug.Log("Move Up");
		}
		if (Input.GetKey(KeyCode.S))        // down
		{
			this.transform.position = new Vector3(this.transform.position.x, 0, this.transform.position.z - speed);
			Debug.Log("Move Down");
		}
		if (Input.GetKey(KeyCode.A))        // left
		{
			this.transform.position = new Vector3(this.transform.position.x - speed, 0, this.transform.position.z);
			Debug.Log("Move Left");
		}
		if (Input.GetKey(KeyCode.D))        // right
		{
			this.transform.position = new Vector3(this.transform.position.x + speed, 0, this.transform.position.z);
			Debug.Log("Move Right");
		}
	}

	void Movementv2()
	{
		if (Input.GetKey(KeyCode.W))       // up
		{
			this.transform.position += Vector3.forward * speed;
		}
		if (Input.GetKey(KeyCode.S))        // down
		{
			this.transform.position += Vector3.back * speed;
			Debug.Log("Move Down");
		}
		if (Input.GetKey(KeyCode.A))        // left
		{
			this.transform.position += Vector3.left * speed;
			Debug.Log("Move Left");
		}
		if (Input.GetKey(KeyCode.D))        // right
		{
			this.transform.position += Vector3.right * speed;
			Debug.Log("Move Right");
		}
	}

	void Movement()
	{
		if (Input.GetKey(KeyCode.W))       // up
		{
			this.transform.Translate(Vector3.forward * speed * Time.deltaTime);
		}
		if (Input.GetKey(KeyCode.S))        // down
		{
			this.transform.Translate(Vector3.back * speed * Time.deltaTime);
			//Debug.Log("Move Down");
		}
		if (Input.GetKey(KeyCode.A))        // left
		{
			this.transform.Translate(Vector3.left * speed * Time.deltaTime);
			//Debug.Log("Move Left");
		}
		if (Input.GetKey(KeyCode.D))        // right
		{
			this.transform.Translate(Vector3.right * speed * Time.deltaTime);
			//Debug.Log("Move Right");
		}
	}



	// Dectecting collision

	// both of them must have a collider
	// atlease one of them must have toggle isTrigger
	// one of them must have a rigibody

	private void OnTriggerEnter(Collider other)
	{

		Debug.Log("Other Object i Bump : " + other.gameObject.name);

		textObject.text = other.gameObject.name;
	}
}

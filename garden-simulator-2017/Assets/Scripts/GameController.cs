using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    public static GameController instance;

    [SerializeField]
    public GameObject[] spawnableObjects; //All the spawnable objects referenced at unity
    
    public bool isBuilding;
    public bool objectsCanMove; //Enable moving in all objects when instantiated
    public bool isPaused;       //Variable to check if the game is paused

    private GameObject clone;   //GameObject temp to control object spawns

    private AudioManager m_AudioManager;

    private Dictionary<string, GameObject> spawnObjectsDic; //Dictionary with all spawnable objects
    
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        //Make GameController inmortal
        DontDestroyOnLoad(this.gameObject);
    }

    // Use this for initialization
    void Start () {

        //Initialize variables
        isBuilding = false;
        isPaused = false;

        //Dictionary code for managing spawnable objects
        //Add all objects to a dictionary to instantly spawn the desired object, without looping every time the player builds something
        spawnObjectsDic = new Dictionary<string, GameObject>();

        for( int i = 0; i < spawnableObjects.Length; i++)
        {
            spawnObjectsDic.Add(spawnableObjects[i].name, spawnableObjects[i]);
        }

        //Find AudioManager to play sounds
        m_AudioManager = FindObjectOfType<AudioManager>();
    }

    void Update()
    {
        if (isBuilding)
        {
            if (Input.GetButtonDown("Fire2") || Input.GetKeyDown(KeyCode.Delete))
            {
                isBuilding = false;
            }
        }
    }

    public void BuildObject(string objectName)
    {
        if (!isBuilding)
        {
            //Play building sound
            m_AudioManager.Play("SelectItem");
            //Now we are building
            isBuilding = true;
            //Try to get the object from the dictionary
            GameObject temp = null;
            if (spawnObjectsDic.TryGetValue(objectName, out temp))
                Debug.Log("Accessing dictionary with \"" + objectName + "\" key, successful");
            else
            {
                Debug.Log("Key: " + objectName + " not found in the objects dictionary.");
                return;
            }
                
            //Use temp GameObject to make a clone and spawn it at the game scene
            clone = Instantiate(temp, GetMousePosition(), Quaternion.identity);
            //Set Move ability for all the objects
            clone.SendMessage("SetCanMove", objectsCanMove);
        }
    }


    private Vector2 GetMousePosition()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
}

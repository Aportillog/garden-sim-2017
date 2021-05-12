using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableObject : MonoBehaviour {

    private SpriteRenderer m_SpriteRenderer;
    public Color m_previousColor;
    public Color m_CantPlaceColor;              //Select color in unity, remember to set alpha to 255
    public Color m_NormalColor;
    public Color m_HighlightedColor;

    public bool canMove;
    public float moveSpeed = 1.5f;

    private bool canPlace;
    private bool isPaused;  //Check if the game is paused

    private AudioManager m_AudioManager;
    private GameController m_GameController;

    private void Awake()
    {
        //Game not paused at the start
        isPaused = false;
        //Fecth SpriteRenderer from the go
        m_SpriteRenderer = GetComponentInChildren<SpriteRenderer>();
        m_previousColor = m_NormalColor;
        //Let objects to be placed at the start
        canPlace = true;

        //Find GameController to send msgs
        m_GameController = FindObjectOfType<GameController>();
        //Find AudioManager to play sounds
        m_AudioManager = FindObjectOfType<AudioManager>();
    }
	

	void Update () {
        //Check if the game is paused
        isPaused = m_GameController.isPaused;
        //Move object with mouse if can move
        if (canMove && !isPaused)
        {
            MoveWithMouse();
            if (Input.GetButtonDown("Fire2") && canPlace)
            {
                m_AudioManager.Play("PlaceItem");
                SetCanMove(false);
            }
            if (Input.GetKeyDown(KeyCode.Delete))
            {
                m_AudioManager.Play("DestroyItem");
                Destroy(this.gameObject);
            }
        }
	}


    private void MoveWithMouse()
    {
        this.transform.position = Vector2.Lerp(transform.position, GetMousePosition(), moveSpeed);
    }

    private Vector2 GetMousePosition()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void OnMouseDown()
    {
        //Change object position once placed
        if (!canMove && !isPaused)
        {
            m_AudioManager.Play("SelectItem");
            SetCanMove(true);
        }
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (canMove && !isPaused)
        {
            if (m_SpriteRenderer.color != m_CantPlaceColor)
                m_previousColor = m_SpriteRenderer.color;
            m_SpriteRenderer.color = m_CantPlaceColor;
            canPlace = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (canMove && !isPaused)
        {
            m_SpriteRenderer.color = m_previousColor;
            canPlace = true;
        }
    }

    private void OnMouseEnter()
    {
        if (!canMove && !isPaused)
        {
            m_previousColor = m_SpriteRenderer.color;
            m_SpriteRenderer.color = m_HighlightedColor;
        }
    }
    private void OnMouseExit()
    {
        if (!canMove && !isPaused)
        {
            m_SpriteRenderer.color = m_NormalColor;
        }
    }

    public void SetCanMove(bool target)
    {
        canMove = target;

        if (canMove && !isPaused)
        {
            m_GameController.isBuilding = true;     //Inform GameController when start moving object
            m_SpriteRenderer.sortingOrder = 1;      //Draw sprite above the rest of the objects while moving
        }

        else
        {
            m_GameController.isBuilding = false;     //Inform GameController when stop moving object
            m_SpriteRenderer.sortingOrder = 0;      //Back to normal drawing layer
        }
            

    }
}

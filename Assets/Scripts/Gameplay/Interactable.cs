using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    private string m_interactionName = "Interact";

    protected string interactionName
    {
        set
        {
            m_interactionName = value;
        }
        get
        {
            return m_interactionName;
        }
    }

    protected virtual void OnTriggerStay2D(Collider2D collision)
    {
        //Check if the colliding object is controlled by player
        PlayerController playerController = collision.GetComponent<PlayerController>();

        //If the colliding object is controlled by player
        if (playerController != null)
        {
            //Note: We don't want to show the UI in the same frame because content size
            //      fitter won't work properly
            UIManager.interactionUI.text = m_interactionName;
            UIManager.interactionUI.Show();

            playerController.interactableObject = this;
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        //Check if the colliding object is controlled by player
        PlayerController playerController = collision.GetComponent<PlayerController>();

        //If the colliding object is controlled by player
        if (playerController != null)
        {
            UIManager.interactionUI.Hide();
            playerController.interactableObject = null;
        }
    }

    public abstract void Interact();
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalObject : Interactable
{
    private bool m_isChangingScene = false;

    private void Awake()
    {
        interactionName = "Go to next level";
    }
    public override void Interact()
    {
        if (m_isChangingScene == false)
        {
            if (SceneManagerSingleton.instance.IsNextLevelAvailable())
                CoroutineHelper.Start(SceneManagerSingleton.instance.StartFade(Color.black, 1.0f))
                    .Then(() => 
                    {
                        SceneManagerSingleton.instance.LoadNextLevel();

                        //TODO: Move UIManager.interactionUI.Hide() inside PlayerController.interactableObject
                        UIManager.interactionUI.Hide();
                        PlayerController.instance.interactableObject = null;
                    })
                    .Then(SceneManagerSingleton.instance.StartFade(Color.clear, 1.0f));
            else
                SceneManagerSingleton.instance.ShowGameWin();

            m_isChangingScene = true;
        }
    }

    protected override void OnTriggerStay2D(Collider2D collision)
    {
        if (m_isChangingScene)
            return;

        base.OnTriggerStay2D(collision);
    }
}

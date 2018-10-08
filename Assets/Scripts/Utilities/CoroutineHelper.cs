using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineHelper : MonoBehaviour
{
    //---------------------------------------------------------------------------------------------
    // Variables
    //---------------------------------------------------------------------------------------------
    static private CoroutineHelper m_instance = null;

    // TODO: 
    // Right now, the content of the list never removed. It should be removed after a CoroutineStack
    // is completed
    private List<CoroutineStack> m_coroutineStack = new List<CoroutineStack>();

    //---------------------------------------------------------------------------------------------
    // Properties
    //---------------------------------------------------------------------------------------------
    static private CoroutineHelper instance
    {
        get
        {
            if (m_instance == null)
            {
                GameObject gameObject = new GameObject("Coroutine Helper");
                gameObject.AddComponent<CoroutineHelper>();

                m_instance = gameObject.GetComponent<CoroutineHelper>();
            }
            return m_instance;
        }
        set
        {
            if (m_instance == null)
                m_instance = value;

            Debug.LogWarning("Not suppposed to instansiate a second CoroutineHelper object");
        }
    }

    //---------------------------------------------------------------------------------------------
    // Unity Overrides
    //---------------------------------------------------------------------------------------------
    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this);
    }

    //---------------------------------------------------------------------------------------------
    // Functions
    //---------------------------------------------------------------------------------------------
    static public CoroutineStack Start(IEnumerator coroutine)
    {
        CoroutineStack coroutineStack = new CoroutineStack(coroutine);
        instance.m_coroutineStack.Add(coroutineStack);

        instance.StartCoroutine(coroutineStack.Run());

        return coroutineStack;
    }
}


public class CoroutineStack
{
    //---------------------------------------------------------------------------------------------
    // Variables
    //---------------------------------------------------------------------------------------------
    
    // TODO:
    // Use std::queue instead of std::vector
    private List<IEnumerator> m_coroutineList = new List<IEnumerator>();
    private List<Action> m_actionList = new List<Action>();

    // false    = coroutine list
    // true     = action list
    private List<bool> m_listUsed = new List<bool>();

    //---------------------------------------------------------------------------------------------
    // Properties
    //---------------------------------------------------------------------------------------------
    private IEnumerator nextCoroutine
    {
        get
        {
            return m_coroutineList[0];
        }
    }

    //---------------------------------------------------------------------------------------------
    // Functions
    //---------------------------------------------------------------------------------------------
    public CoroutineStack(IEnumerator initialCoroutine)
    {
        this.Then(initialCoroutine);
    }


    /// <summary>
    /// Execute a new function when the previous function call is finished
    /// </summary>
    /// <param name="nextCoroutine">
    /// The coroutine that will be called when the previous one finished
    /// </param>
    /// <returns></returns>
    public CoroutineStack Then(IEnumerator nextCoroutine)
    {
        m_coroutineList.Add(nextCoroutine);
        m_listUsed.Add(false);

        return this;
    }


    /// <summary>
    ///     Execute a new function when the previous function call is finished --
    ///     Usage example: .Then( ()=>{ MyFunctionName(myParameter); } )
    /// </summary>
    /// <param name="action"> 
    /// The function that will be called when the previous one finished
    /// </param>
    /// <returns></returns>
    public CoroutineStack Then(Action action)
    {
        m_actionList.Add(action);
        m_listUsed.Add(true);

        return this;
    }

    public IEnumerator Run()
    {
        while (m_listUsed.Count != 0)
        {
            // If the next function to be executed is a coroutine
            if (m_listUsed[0] == false)
            {
                //Execute the coroutine
                yield return m_coroutineList[0];

                //Remove the coroutine from list
                m_coroutineList.RemoveAt(0);
            }
            // If the next function to be executed is not a coroutine 
            else
            {
                //Execute the action
                m_actionList[0]();

                //Remove the action from list
                m_actionList.RemoveAt(0);
            }

            m_listUsed.RemoveAt(0);
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManagerAccessor : MonoBehaviour {

    public void LoadNextLevel()
    {
        SceneManagerSingleton.instance.LoadNextLevel();
    }
}

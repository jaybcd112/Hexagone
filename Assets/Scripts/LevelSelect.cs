using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

public class LevelSelect : MonoBehaviour
{
    public Vector3 rotationAmount;

    public SceneAsset scene1;
    public SceneAsset scene2;
    public SceneAsset scene3;
    public SceneAsset scene4;

    private SceneNames currentScene;

    public enum SceneNames
    {
        scene1,
        scene2,
        scene3,
        scene4
    }

    // Start is called before the first frame update
    void Start()
    {
        currentScene = SceneNames.scene1;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void spinRight()
    {
        transform.Rotate(rotationAmount * -1f);
        MoveNext();
    }

    public void spinLeft()
    {
        transform.Rotate(rotationAmount);
        MovePrevious();
    }

    private void MoveNext()
    {
        currentScene = (SceneNames)(((int)currentScene + 1) % System.Enum.GetValues(typeof(SceneNames)).Length);
    }

    private void MovePrevious()
    {
        currentScene = (SceneNames)(((int)currentScene - 1 + System.Enum.GetValues(typeof(SceneNames)).Length) % System.Enum.GetValues(typeof(SceneNames)).Length);
    }

    public void LoadCurrentScene()
    {
        SceneAsset sceneToLoad = null;

        switch (currentScene)
        {
            case SceneNames.scene1:
                sceneToLoad = scene1;
                break;
            case SceneNames.scene2:
                sceneToLoad = scene2;
                break;
            case SceneNames.scene3:
                sceneToLoad = scene3;
                break;
            case SceneNames.scene4:
                sceneToLoad = scene4;
                break;
        }

        if (sceneToLoad != null)
        {
            SceneManager.LoadScene(sceneToLoad.name);
        }
    }
}

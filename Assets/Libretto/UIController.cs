using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class UIController : MonoBehaviour
{

    public Button startButton;
    public Button messageButton;

    public Label messageText;
    // Start is called before the first frame update
    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        startButton = root.Q<Button>("");
        messageButton = root.Q<Button>("");
        messageText = root.Q<Label>("");

        startButton.clicked += StartButtonPressed;
    }
   

    void StartButtonPressed() {
        // SceneManager.LoadScene();
    }

    // Update is called once per frame
    void Update()
    {

    }
}

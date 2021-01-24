using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
    // Start is called before the first frame update
    public void LoadNextScene() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    } 

    public void LoadLevel1() {
        // based on build index, hardcoded
        SceneManager.LoadScene("Level 1");
    }

    private void Update() {
        // when user presses spacebar, load level 1
        if (Input.GetKeyDown(KeyCode.Space)) {
            LoadLevel1();
        }
    }
}

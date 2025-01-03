using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class ButtonClickMangments : MonoBehaviour
{


    public void onButtonClick(GameObject button)
    {
        string buttonScene = button.transform.name;
        SceneManager.LoadScene(buttonScene);

    }

}

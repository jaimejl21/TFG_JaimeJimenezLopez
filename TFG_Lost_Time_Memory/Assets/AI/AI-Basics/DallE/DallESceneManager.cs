

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DallESceneManager : MonoBehaviour
{
    public Button goToGalleryButton;
    // Start is called before the first frame update
    void Start()
    {
        goToGalleryButton.onClick.AddListener(GoToGalleryScene);
    }

    void GoToGalleryScene()
    {
        SceneManager.LoadScene(11);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}

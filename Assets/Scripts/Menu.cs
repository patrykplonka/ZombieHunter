using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

     public void Authors()
    {
        SceneManager.LoadSceneAsync(0);
    }
     public void Difficulty()
    {
        SceneManager.LoadSceneAsync(1);
    }

     public void BackToMenu()
    {
        SceneManager.LoadSceneAsync(2);
    }
    public void PlayGanme()
    {
        SceneManager.LoadSceneAsync(3);
    }
   
    
}

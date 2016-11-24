using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoadScene : MonoBehaviour {


    public void NextLevel(int no)
    {
        SceneManager.LoadScene(no);
    }   

}

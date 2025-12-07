using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
public class menuManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject tut;

    void Start()
    {
        tut.SetActive(false);
    }
    public void LoadTutorial()
    {
        tut.SetActive(true);
    }
    public void CloseTutorial()
    {
        tut.SetActive(false);
    }
    public void LoadGame1()
    {
        SceneManager.LoadScene(1);
    }
    public void LoadGame2()
    {
        SceneManager.LoadScene(2);
    }
    public void LoadGame3()
    {
        SceneManager.LoadScene(3);
    }
    public void LoadGame4()
    {
        SceneManager.LoadScene(4);
    }
    public void LoadGame5()
    {
        SceneManager.LoadScene(5);
    }
    public void LoadGame6()
    {
        SceneManager.LoadScene(6);
    }
    public void LoadGame7()
    {
        SceneManager.LoadScene(7);
    }
    public void LoadGame8()
    {
        SceneManager.LoadScene(8);
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuGameOver : MonoBehaviour
{
    [SerializeField] private Button botonReintentar;
    [SerializeField] private Button botonMenuPrincipal;

    // Start is called before the first frame update
    void Start()
    {
        botonReintentar.onClick.AddListener(() => Reintentar());
        botonMenuPrincipal.onClick.AddListener(() => MenuPrincipal());
    }

    private void Reintentar()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        GameManager.gameM.isGameOver = false;
    }

    private void MenuPrincipal()
    {
        Time.timeScale = 1;
        GameManager.gameM.isGameOver = false;
        SceneManager.LoadScene(0);        
    }
}

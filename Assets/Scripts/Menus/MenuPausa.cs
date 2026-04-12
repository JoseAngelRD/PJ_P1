using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuPausa : MonoBehaviour
{
    [SerializeField] private GameObject menu;
    [SerializeField] private GameObject menuOpciones;
    [SerializeField] private bool opcionesActivo = false;
    private bool menuActivo = false;
    [SerializeField] private Button botonReanudar;
    [SerializeField] private Button botonOpciones;
    [SerializeField] private Button botonMenuPrincipal;

    void Start()
    {
        botonReanudar.onClick.AddListener(() => Reanudar());
        botonOpciones.onClick.AddListener(() => menuOpciones.SetActive(true));
        botonMenuPrincipal.onClick.AddListener(() => MenuPrincipal());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !GameManager.gameM.isGameOver)
        {
            Debug.Log("PAUSE");
            ToggleMenu();
            Pausar();
            menuOpciones.SetActive(false);
        }
    }

    private void ToggleMenu()
    {
        menuActivo = !menuActivo;
        menu.SetActive(menuActivo);
    }

    private void Reanudar()
    {
        ToggleMenu();
        Time.timeScale = 1;
    }

    private void Pausar()
    {
        Time.timeScale = 0;
    }

    private void MenuPrincipal()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
}

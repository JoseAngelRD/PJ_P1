using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.UI;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuPrincipal : MonoBehaviour
{
    public Button botonJugar;
    public Button botonOpciones;
    public Button botonSalir;

    public GameObject canvasOpciones;
    private bool toggleOpciones = false;
    public Button volverOpc;
    public Slider volumen;

    // Start is called before the first frame update
    void Start()
    {
        botonJugar.onClick.AddListener(() => Jugar());
        botonOpciones.onClick.AddListener(() => ToggleOpciones());
        volverOpc.onClick.AddListener(() => ToggleOpciones());
        volumen.onValueChanged.AddListener(delegate { Volumen(); });

        botonSalir.onClick.AddListener(() => Salir());
    }   

    private void Jugar()
    {
        SceneManager.LoadScene(1);
    }

    public void Salir()
    {
        Application.Quit();
    }

    private void ToggleOpciones()
    {
        toggleOpciones = !toggleOpciones;
        canvasOpciones.SetActive(toggleOpciones);
    }

    private void Volumen()
    {
        GameManager.gameM.GetComponent<AudioSource>().volume = volumen.value;
    }
}

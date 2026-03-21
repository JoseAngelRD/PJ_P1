using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.UI;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuPrincipal : MonoBehaviour
{
    public Button botonJugar;
    public Button botonOpciones;
    public Button botonSalir;

    public GameObject canvasOpciones;
    private bool toggleOpciones = false;
    public Button volverOpc;
    public Slider volumen;
    public TMP_Dropdown dropdownResolucion;
    public TMP_Dropdown dropdownPantalla;

    private Resolution[] resoluciones;

    // Start is called before the first frame update
    void Start()
    {
        botonJugar.onClick.AddListener(() => Jugar());
        botonOpciones.onClick.AddListener(() => ToggleOpciones());
        volverOpc.onClick.AddListener(() => ToggleOpciones());
        volumen.onValueChanged.AddListener(delegate { Volumen(); });

        botonSalir.onClick.AddListener(() => Salir());

        ConfigurarResolucion();
        ConfigurarModoPantalla();
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

    private void ConfigurarResolucion()
    {
        resoluciones = Screen.resolutions;
        dropdownResolucion.ClearOptions();

        List<string> opciones = new List<string>();
        int resolucionActual = 0;

        for (int i = 0; i < resoluciones.Length; i++)
        {
            string opcion = resoluciones[i].width + " x " + resoluciones[i].height;
            opciones.Add(opcion);

            if (resoluciones[i].width == Screen.currentResolution.width &&
                resoluciones[i].height == Screen.currentResolution.height)
            {
                resolucionActual = i;
            }
        }

        dropdownResolucion.AddOptions(opciones);
        dropdownResolucion.value = resolucionActual;
        dropdownResolucion.RefreshShownValue();

        dropdownResolucion.onValueChanged.AddListener(CambiarResolucion);
    }

    public void CambiarResolucion(int indiceResolucion)
    {
        Resolution res = resoluciones[indiceResolucion];
        Screen.SetResolution(res.width, res.height, Screen.fullScreenMode);
    }

    private void ConfigurarModoPantalla()
    {
        dropdownPantalla.ClearOptions();
        List<string> modos = new List<string> { "Pantalla Completa", "Ventana sin Bordes", "Ventana" };
        dropdownPantalla.AddOptions(modos);

        dropdownPantalla.onValueChanged.AddListener(CambiarModoPantalla);
    }

    public void CambiarModoPantalla(int indice)
    {
        switch (indice)
        {
            case 0: Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen; break;
            case 1: Screen.fullScreenMode = FullScreenMode.FullScreenWindow; break;
            case 2: Screen.fullScreenMode = FullScreenMode.Windowed; break;
        }
    }
}

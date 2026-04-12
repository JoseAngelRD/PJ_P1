using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.UI;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuPrincipal : MonoBehaviour
{
    // Jugar
    [SerializeField] private Button botonJugar;
    [SerializeField] private GameObject canvasJugar;
    private bool toggleJugar = false;
    [SerializeField] private Button volverJugar;
    [SerializeField] private Button botonSamurai;
    [SerializeField] private Button botonNieve;
    [SerializeField] private Button botonMinotauro;

    // Opciones
    [SerializeField] private Button botonOpciones;
    [SerializeField] private GameObject canvasOpciones;
    [SerializeField] private bool toggleOpciones = false;
    [SerializeField] private Button volverOpc;
    [SerializeField] private Slider volumen;
    [SerializeField] private TMP_Dropdown dropdownResolucion;
    [SerializeField] private TMP_Dropdown dropdownPantalla;
    private Resolution[] resoluciones;

    //Ranking
    [SerializeField] private Button botonRanking;
    [SerializeField] private GameObject canvasRanking;
    [SerializeField] private bool toggleRanking = false;
    [SerializeField] private Button volverRanking;
    [SerializeField] private TextMeshProUGUI textoRecordSamurai;
    [SerializeField] private TextMeshProUGUI textoRecordMinotauro;
    [SerializeField] private TextMeshProUGUI textoRecordNieve;

    // Salir
    [SerializeField] private Button botonSalir;

    // Start is called before the first frame update
    void Start()
    {
        botonJugar.onClick.AddListener(() => ToggleJugar());
        volverJugar.onClick.AddListener(() => ToggleJugar());
        botonSamurai.onClick.AddListener(() => CargarSamurai());
        botonMinotauro.onClick.AddListener(() => CargarMinotauro());
        botonNieve.onClick.AddListener(() => CargarNieve());

        botonOpciones.onClick.AddListener(() => ToggleOpciones());
        volverOpc.onClick.AddListener(() => ToggleOpciones());
        volumen.onValueChanged.AddListener(delegate { Volumen(); });

        botonRanking.onClick.AddListener(() => ToggleRanking());
        volverRanking.onClick.AddListener(() => ToggleRanking());

        botonSalir.onClick.AddListener(() => Salir());

        ConfigurarResolucion();
        ConfigurarModoPantalla();
    }

    private void ToggleJugar()
    {
        toggleJugar = !toggleJugar;
        canvasJugar.SetActive(toggleJugar);
    }

    private void CargarSamurai()
    {
        SceneManager.LoadScene(1);
    }

    private void CargarMinotauro()
    {
        SceneManager.LoadScene(2);
    }

    private void CargarNieve()
    {
        SceneManager.LoadScene(3);
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

    private void ToggleRanking()
    {
        toggleRanking = !toggleRanking;
        canvasRanking.SetActive(toggleRanking);

        if (toggleRanking)
        {
            ActualizarTextosRanking();
        }
    }

    private void ActualizarTextosRanking()
    {
        // Pasamos el nombre EXACTO que usamos al guardar el récord ("Samurai", "Minotauro", "Nieve")
        textoRecordSamurai.text = ObtenerTextoRecord("Samurai");
        textoRecordMinotauro.text = ObtenerTextoRecord("Minotauro");
        textoRecordNieve.text = ObtenerTextoRecord("Nieve");
    }

    private string ObtenerTextoRecord(string nombreBoss)
    {
        string clave = "Record_" + nombreBoss;

        // Comprobamos si existe un tiempo guardado
        if (PlayerPrefs.HasKey(clave))
        {
            float tiempo = PlayerPrefs.GetFloat(clave);
            return FormatearTiempo(tiempo);
        }
        else
        {
            // Si nunca se ha matado a ese boss, mostramos guiones
            return "--:--:--";
        }
    }

    private string FormatearTiempo(float tiempo)
    {
        int minutos = Mathf.FloorToInt(tiempo / 60);
        int segundos = Mathf.FloorToInt(tiempo % 60);
        int milisegundos = Mathf.FloorToInt((tiempo * 100) % 100);
        return string.Format("{0:00}:{1:00}:{2:00}", minutos, segundos, milisegundos);
    }
}

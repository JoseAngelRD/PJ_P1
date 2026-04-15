using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.UI;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuPrincipalV2 : MonoBehaviour
{
    // Jugar
    [SerializeField] private Button botonJugar;
    [SerializeField] private GameObject canvasJugar;
    private bool toggleJugar = false;
    [SerializeField] private Button volverJugar;

    // Carteles para rankings
    [SerializeField] private Button botonCartelKnight;
    [SerializeField] private Button botonCartelNieve;
    [SerializeField] private Button botonCartelMinotauro;

    //jugar
    [SerializeField] private Button botonJugarKnight;
    [SerializeField] private Button botonJugarNieve;
    [SerializeField] private Button botonJugarMinotauro;

    // Opciones
    [SerializeField] private Button botonOpciones;
    [SerializeField] private GameObject canvasOpciones;

    //Ranking
    [SerializeField] private GameObject canvasRanking;
    [SerializeField] private GameObject rankingSamurai;
    [SerializeField] private GameObject rankingMinotauro;
    [SerializeField] private GameObject rankingNieve;

    // Salir
    [SerializeField] private Button botonSalir;

    // Start is called before the first frame update
    void Start()
    {
        botonJugar.onClick.AddListener(() => ToggleJugar());
        volverJugar.onClick.AddListener(() => ToggleJugar());

        botonCartelKnight.onClick.AddListener(() => MostrarRankingIndividual(rankingSamurai));
        botonCartelMinotauro.onClick.AddListener(() => MostrarRankingIndividual(rankingMinotauro));
        botonCartelNieve.onClick.AddListener(() => MostrarRankingIndividual(rankingNieve));

        botonJugarKnight.onClick.AddListener(() => CargarSamurai());
        botonJugarMinotauro.onClick.AddListener(() => CargarMinotauro());
        botonJugarNieve.onClick.AddListener(() => CargarNieve());

        botonOpciones.onClick.AddListener(() => canvasOpciones.SetActive(true));

        botonSalir.onClick.AddListener(() => Salir());
    }

    private void ToggleJugar()
    {
        toggleJugar = !toggleJugar;
        canvasJugar.SetActive(toggleJugar);
    }

    private void CargarNieve()
    {
        SceneManager.LoadScene(1);
        GameManager.gameM.CambiarCancion(1);
    }

    private void CargarMinotauro()
    {
        SceneManager.LoadScene(2);
        GameManager.gameM.CambiarCancion(2);
    }

    private void CargarSamurai()
    {
        SceneManager.LoadScene(3);
        GameManager.gameM.CambiarCancion(3);
    }

    private void MostrarRankingIndividual(GameObject columnaActiva)
    {
        // 1. Apagamos todas las columnas por defecto
        rankingSamurai.SetActive(false);
        rankingMinotauro.SetActive(false);
        rankingNieve.SetActive(false);

        // 2. Encendemos únicamente la que pasamos por parámetro
        if (columnaActiva != null)
        {
            columnaActiva.SetActive(true);
        }

        // 3. Abrimos el canvas de Ranking
        canvasRanking.SetActive(true);
    }

    public void Salir()
    {
        Application.Quit();
    }
}

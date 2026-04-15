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
    [SerializeField] private Button botonKnight;
    [SerializeField] private Button botonNieve;
    [SerializeField] private Button botonMinotauro;
    
    // Opciones
    [SerializeField] private Button botonOpciones;
    [SerializeField] private GameObject canvasOpciones;        

    //Ranking
    [SerializeField] private Button botonRanking;
    [SerializeField] private GameObject canvasRanking;

    // Salir
    [SerializeField] private Button botonSalir;    

    // Start is called before the first frame update
    void Start()
    {
        botonJugar.onClick.AddListener(() => ToggleJugar());
        volverJugar.onClick.AddListener(() => ToggleJugar());
        botonKnight.onClick.AddListener(() => CargarSamurai());
        botonMinotauro.onClick.AddListener(() => CargarMinotauro());
        botonNieve.onClick.AddListener(() => CargarNieve());

        botonOpciones.onClick.AddListener(() => ActivarOpciones());        
        botonRanking.onClick.AddListener(() => ActivarRanking());  
        botonSalir.onClick.AddListener(() => Salir());        
    }

    private void ToggleJugar()
    {
        GameManager.gameM.BotonPresionadoSFX();
        toggleJugar = !toggleJugar;
        canvasJugar.SetActive(toggleJugar);
    }

    private void CargarNieve()
    {
        GameManager.gameM.BotonPresionadoSFX();
        SceneManager.LoadScene(1);
        GameManager.gameM.CambiarCancion(1);
    }

    private void CargarMinotauro()
    {
        GameManager.gameM.BotonPresionadoSFX();
        SceneManager.LoadScene(2);
        GameManager.gameM.CambiarCancion(2);
    }    

    private void CargarSamurai()
    {
        GameManager.gameM.BotonPresionadoSFX();
        SceneManager.LoadScene(3);
        GameManager.gameM.CambiarCancion(3);
    }

    private void ActivarOpciones()
    {
        GameManager.gameM.BotonPresionadoSFX();
        canvasOpciones.SetActive(true);
    }

    private void ActivarRanking()
    {
        GameManager.gameM.BotonPresionadoSFX();
        canvasRanking.SetActive(true);
    }

    public void Salir()
    {
        GameManager.gameM.BotonPresionadoSFX();
        Application.Quit();
    }   
}

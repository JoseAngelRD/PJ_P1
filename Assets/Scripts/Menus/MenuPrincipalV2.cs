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
        botonKnight.onClick.AddListener(() => CargarKnight());
        botonMinotauro.onClick.AddListener(() => CargarMinotauro());
        botonNieve.onClick.AddListener(() => CargarNieve());

        botonOpciones.onClick.AddListener(() => canvasOpciones.SetActive(true));        
        botonRanking.onClick.AddListener(() => canvasRanking.SetActive(true));  
        botonSalir.onClick.AddListener(() => Salir());        
    }

    private void ToggleJugar()
    {
        toggleJugar = !toggleJugar;
        canvasJugar.SetActive(toggleJugar);
    }

    private void CargarKnight()
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
}

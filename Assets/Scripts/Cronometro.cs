using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // Obligatorio para usar TextMeshPro

public class Cronometro : MonoBehaviour
{
    [Header("Interfaz")]
    [SerializeField] private TextMeshProUGUI textoTiempo;

    private float tiempoActual;
    private bool estaCorriendo = false;

    void Start()
    {
        // Al empezar la escena de pelea, arranca el cronómetro
        tiempoActual = 0f;
        estaCorriendo = true;
    }

    void Update()
    {
        // Si el cronómetro está activo, suma el tiempo que tarda en pasar cada frame
        if (estaCorriendo)
        {
            tiempoActual += Time.deltaTime;
            ActualizarTexto();
        }
    }

    private void ActualizarTexto()
    {
        // Convertimos el tiempo en segundos a formato Minutos:Segundos:Milisegundos
        int minutos = Mathf.FloorToInt(tiempoActual / 60);
        int segundos = Mathf.FloorToInt(tiempoActual % 60);
        int milisegundos = Mathf.FloorToInt((tiempoActual * 100) % 100);

        // Actualizamos el texto en pantalla
        textoTiempo.text = string.Format("{0:00}:{1:00}:{2:00}", minutos, segundos, milisegundos);
    }

    // Esta función es la que vas a llamar desde el script de tu Boss cuando muera
    public void DetenerYComprobarRecord(string nombreBoss)
    {
        // 1. Evitamos que se ejecute más de una vez cuando el Boss muere
        if (!estaCorriendo) return; 

        estaCorriendo = false;
        string claveRecord = "Record_" + nombreBoss;

        // 2. Comprobamos si es la PRIMERA VEZ (no existe una clave guardada aún)
        if (!PlayerPrefs.HasKey(claveRecord))
        {
            PlayerPrefs.SetFloat(claveRecord, tiempoActual);
            PlayerPrefs.Save();
            Debug.Log("¡Primer intento completado! Tiempo base en " + nombreBoss + ": " + FormatearTiempo(tiempoActual));
        }
        else
        {
            // 3. Ya existe un récord, comprobamos si lo hemos superado
            float mejorTiempo = PlayerPrefs.GetFloat(claveRecord);

            if (tiempoActual < mejorTiempo)
            {
                PlayerPrefs.SetFloat(claveRecord, tiempoActual);
                PlayerPrefs.Save();
                Debug.Log("¡NUEVO RÉCORD en " + nombreBoss + "! Tu tiempo: " + FormatearTiempo(tiempoActual));
            }
            else
            {
                Debug.Log("No superaste el récord. Tu tiempo: " + FormatearTiempo(tiempoActual) + " | Mejor: " + FormatearTiempo(mejorTiempo));
            }
        }
    }

    // Función extra para que la consola muestre el tiempo en formato MM:SS:mm en lugar de un número con decimales
    private string FormatearTiempo(float tiempo)
    {
        int minutos = Mathf.FloorToInt(tiempo / 60);
        int segundos = Mathf.FloorToInt(tiempo % 60);
        int milisegundos = Mathf.FloorToInt((tiempo * 100) % 100);
        return string.Format("{0:00}:{1:00}:{2:00}", minutos, segundos, milisegundos);
    }
}
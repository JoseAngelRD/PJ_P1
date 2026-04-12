using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuRanking : MonoBehaviour
{
    [SerializeField] private bool toggleRanking = false;
    [SerializeField] private Button volverRanking;
    [SerializeField] private TextMeshProUGUI textoRecordSamurai;
    [SerializeField] private TextMeshProUGUI textoRecordMinotauro;
    [SerializeField] private TextMeshProUGUI textoRecordNieve;

    void Start()
    {
        ActualizarTextosRanking();
        volverRanking.onClick.AddListener(() => gameObject.SetActive(false));
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
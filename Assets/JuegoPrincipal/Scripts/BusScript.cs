using UnityEngine;

public class BusScript : MonoBehaviour
{
    private int _pasajeros = 0;
    public int PasajerosABajar { get; private set; }
    private const int MaxPasajeros = 30;

    public int Pasajeros => _pasajeros;

    /**
     * Intenta subir un pasajero al bus. Devuelve true si exitoso.
     */
    public bool SubirPasajero()
    {
        if (_pasajeros >= MaxPasajeros) return false;
        _pasajeros++;
        return true;
    }

    /**
     * Intenta bajar un pasajero del bus. Devuelve true si exitoso.
     */
    public bool BajarPasajero()
    {
        if (_pasajeros <= 0) return false;
        _pasajeros--;
        return true;
    }

    /**
     * Actualiza la cantidad de pasajeros que bajaran en la sig parada
     */
    public void ActualizarPasajerosABajar()
    {
        PasajerosABajar = Random.Range(0, _pasajeros + 1);
    }
}
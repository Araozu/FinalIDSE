using System;
using System.Collections.Generic;
using JuegoPrincipal.Scripts;
using UnityEngine;
using Random = UnityEngine.Random;

public class BusScript : MonoBehaviour
{
    private int _pasajeros = 0;
    public int PasajerosABajar { get; private set; }
    private const int MaxPasajeros = 30;
    private FlechaScript _flechaScript;

    public List<BusStop> paradas;
    private int _numParada = 0;
    private Vector3 _objetivo;

    public int Pasajeros => _pasajeros;

    private void Start()
    {
        _flechaScript = GetComponentInChildren<FlechaScript>();
    }

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

    /**
     * Obtiene la siguiente parada y la establece como punto destino
     */
    public void SigParada()
    {
        _numParada++;
        var parada = paradas[_numParada];
        parada.utilizado = false;
        _objetivo = parada.transform.position;

        var c = parada.transform.GetChild(2);
        _flechaScript.SetTarget(c.transform.position);

        Debug.Log("objetivo: " + _objetivo);
    }
}
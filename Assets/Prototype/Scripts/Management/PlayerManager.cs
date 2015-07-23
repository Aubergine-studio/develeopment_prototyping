using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

internal class PlayerRegistrationException : Exception
{ }

/// <summary>
/// Klasa zarządzająca graczami.
/// Przechowuje i udostępnia informacje o graczach.
/// </summary>
public class PlayerManager : MonoBehaviour
{
    private static PlayerManager _instance;

    public static PlayerManager Instance
    {
        get
        {
            if (_instance == null) _instance = FindObjectOfType<PlayerManager>();
            return _instance;
        }
    }

    [SerializeField]
    private List<GameObject> _playersList = new List<GameObject>();

    private readonly InputSource[] _inputSources =
    {
        InputSource.GamePad1,
        InputSource.GamePad2,
        InputSource.GamePad3,
        InputSource.GamePad4
    };

    /// <summary>
    /// Metoda rejestruje obiekt  gracza.
    /// Realizuje:
    ///     -Rejestracje
    ///     -Dodaje zarządcę wejść dla gracza.
    ///     -Przedziela źródło wejść gracza.
    /// </summary>
    /// <param name="player">
    /// Gracza do zarejestrowania
    /// </param>
    public void RegisterPlayer(GameObject player)
    {
        //  Sprawdzenie poprawności rejestracji. Jeżeli obiekt został zarejestrowany rzucany
        //
        if (_playersList.Any(p => p.name == player.name) || _playersList.Contains(player))
        {
            throw new PlayerRegistrationException();
        }

        //  Rejestracja gracza
        _playersList.Add(player);
        //  Po rejestracji gracza następuje sortowanie listy.
        _playersList = _playersList.OrderBy(t => t.name).ToList();

        //  Dodanie zarządcy i przydzielenie źródła.
        var pim = player.GetComponent<InputMenager>();
        var joystickNames = Input.GetJoystickNames();
        pim.Input = joystickNames == null ? InputSource.KeyboardAndMouse : _inputSources[_playersList.Count - 1];
    }

    /// <summary>
    /// Metoda zwracająca informacje o wszystkich graczach.
    /// </summary>
    /// <returns>
    /// Zwraca listę typu GameObject zawierającą referencje na obiekty graczy.
    /// </returns>
    public List<GameObject> GetPlayersInfo()
    {
        return _playersList;
    }

    /// <summary>
    /// Zwraca informacje o wybranym graczu.
    /// </summary>
    /// <param name="playerName">
    /// Pobiera ciąg znakow będących nazwą gracza.
    /// </param>
    /// <returns>
    /// Zwraca referencę typu GameObject, będącą referencją na obiekt wybranego gracza.
    /// </returns>
    public GameObject GetPlayerInfo(string playerName)
    {
        return _playersList.FirstOrDefault(p => p.name == playerName);
    }

    /// <summary>
    /// Wyrejestrowuje gracza
    /// </summary>
    /// <param name="player">
    /// Referencja ba obiekt gracz do wyrejestrowania.
    /// </param>
    public void DropPlayer(GameObject player)
    {
        for (var i = 0; i < _playersList.Count; i++)
        {
            if (player != _playersList[i]) continue;

            _playersList.RemoveAt(i);
            break;
        }
    }

    /// <summary>
    /// Wyrejestrowuje gracza
    /// </summary>
    /// <param name="playerName">
    /// Nazwa gracza do wrejestrowania.
    /// </param>
    public void DropPlayer(string playerName)
    {
        for (var i = 0; i < _playersList.Count; i++)
        {
            if (playerName != _playersList[i].name) continue;
            _playersList.RemoveAt(i);
            break;
        }
    }
}
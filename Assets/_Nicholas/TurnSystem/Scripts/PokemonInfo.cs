using System;
using UnityEngine;

[Serializable]
public class PokemonInfo
{
    #region VARIAVEIS
    public string pokemonName;
    public float healthCurrent;
    public float healthMax;
    public int[] attackUseQuantRemaining;
    public bool dead = false;
    #endregion

    public PokemonInfo(Pokemon _pokemon) // Construtor.
    {
        pokemonName = _pokemon.pokemonName;
        healthCurrent = _pokemon.healthMax;
        healthMax = _pokemon.healthMax;

        if (_pokemon.attack.Length > 0)
        {
            attackUseQuantRemaining = new int[_pokemon.attack.Length];

            int i = 0;

            foreach (Attack _attack in _pokemon.attack)
            {
                attackUseQuantRemaining[i] = _attack.useQuantMax;
                i++;
            }
        }
        else
        {
            Debug.LogError("Nao existem ataques disponiveis, adicione um ataque.");
        }
    }
}
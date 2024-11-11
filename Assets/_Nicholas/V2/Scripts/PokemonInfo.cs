using System;
using UnityEngine;

[Serializable]
public class PokemonInfo
{
    public string pokemonName;
    public float healthCurrent;
    public float healthMax;
    public int[] attackUseQuantRemaining;
    public bool dead = false;

    public PokemonInfo(Pokemon _pokemon) //construtor
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
            Debug.LogError("pokemon nn tem atk nenhum, adicione algum atk");
        }
    }
}
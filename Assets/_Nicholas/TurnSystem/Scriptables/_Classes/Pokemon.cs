using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Pokemon_", menuName = "Battle/Pokemon")]
public class Pokemon : ScriptableObject
{
    #region HEADER
    [Header("Componentes")]
    public GameObject go_mesh; // Malha que vai ser exibida na cena.
    public Image img; // Imagem no botao de troca.
    [Header("Atributos")]
    public string pokemonName;
    public float healthMax; // Vida inicial.
    [Header("Attacks")]
    public Attack[] attack = new Attack[4];
    [Header("Action and Reaction\n(o que vai acontecer com cada ataque recebido - o padrão é nada)")]
    public List<ActionAndReaction> actionAndReaction;
    #endregion

    public void Reaction_Get(string _attackName)
    {
        BattleManager.Reaction _reaction = BattleManager.Reaction.Nothing; // Reacao vai iniciar como nula.
        float _value = 0; // Valor padrao.

        foreach (ActionAndReaction _actionAndReaction in actionAndReaction) // Verifica todas as acoes e reacoes.
        {
            if (_actionAndReaction.attack.attackName == _attackName) // Se o nome do ataque estiver nas reacoes do personagem atacado.
            {
                _reaction = _actionAndReaction.reaction;
                _value = _actionAndReaction.value;
                break;
            }
        }

        Debug.Log(_reaction);

        BattleManager.instance.StartCoroutine(BattleManager.instance.Reaction_Routine(_reaction, _attackName, _value));
    }
}
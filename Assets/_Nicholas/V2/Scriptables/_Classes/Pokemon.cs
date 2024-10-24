using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Pokemon_", menuName = "Battle/Pokemon")]
public class Pokemon : ScriptableObject
{
    [Header("Components")]
    public GameObject go_mesh; //malha a ser exibida na cena
    public Image img; //imagem no botao de troca
    [Header("Atributos")]
    public string pokemonName;
    public float healthMax; //vida inicial
    [Header("Attacks")]
    public Attack[] attack = new Attack[4];
    [Header("Action and Reaction\n(o que vai acontecer com cada ataque recebido - o padrão é nada)")]
    public List<ActionAndReaction> actionAndReaction;

    public void Reaction_Get(string _attackName)
    {
        BattleManager.Reaction _reaction = BattleManager.Reaction.Nothing; //reacao inicia como nula
        float _value = 0; //valor padrao

        foreach (ActionAndReaction _actionAndReaction in actionAndReaction) //verifica todas as acoes e reacoes
        {
            if (_actionAndReaction.attack.attackName == _attackName) //se nome do atk do pokemon estiver nas reacoes do pokemon atacado
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
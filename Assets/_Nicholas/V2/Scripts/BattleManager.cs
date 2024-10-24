using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    #region variaveis
    public static BattleManager instance;

    [SerializeField] Pokemon[] player_pokemon; //pokemons do player
    [SerializeField] PokemonInfo[] player_pokemonInfo;
    [SerializeField] Pokemon[] enemy_pokemon; //pokemons do inimigo
    [SerializeField] PokemonInfo[] enemy_pokemonInfo;
    [SerializeField] Vector3 player_pokemonSpawnPos;
    [SerializeField] Vector3 enemy_pokemonSpawnPos;
    [SerializeField] GameObject[] go_turnCube;

    [Header("Combate")]
    [SerializeField] CombatType combatType;
    [SerializeField] EnemyBehaviour enemyBehaviour;
    [SerializeField] [Range(0, 3)] int enemyAttackIndex;
    [SerializeField] float turnDelay;

    [Header("Pokemon atual do player")]
    [SerializeField] Pokemon player_pokemon_current; //somente leitura
    [SerializeField] PokemonInfo player_pokemonInfo_current;

    [Header("Pokemon atual do inimigo")]
    [SerializeField] Pokemon enemy_pokemon_current; //somente leitura
    [SerializeField] PokemonInfo enemy_pokemonInfo_current;

    [Header("Canvas")]
    [SerializeField] TextMeshProUGUI txt_battleFeedback;

    [SerializeField] TextMeshProUGUI txt_playerPokemonName;
    [SerializeField] TextMeshProUGUI[] txt_playerAttack;
    [SerializeField] CanvasGroup cg_playerPokemonHealthBar;
    [SerializeField] RectTransform rect_playerPokemonHealthBar;

    [SerializeField] TextMeshProUGUI txt_enemyPokemonName;
    [SerializeField] CanvasGroup cg_enemyPokemonHealthBar;
    [SerializeField] RectTransform rect_enemyPokemonHealthBar;

    float player_pokemonHealthBar_sizeIniX;
    float enemy_pokemonHealthBar_sizeIniX;

    int player_pokemonCurrent_index = 0;
    int enemy_pokemonCurrent_index = 0;

    GameObject go_player_pokemon;
    GameObject go_enemy_pokemon;

    bool playerTurn = true; //define de quem eh o turno
    bool PlayerTurn
    {
        get { return playerTurn; }
        set { playerTurn = value; }
    }

    Pokemon Player_Pokemon_Current //pokemon atual player
    {
        get { return player_pokemon_current; }
        set { player_pokemon_current = value; }
    }
    PokemonInfo Player_PokemonInfo_Current
    {
        get { return player_pokemonInfo_current; }
        set { player_pokemonInfo_current = value; }
    }

    Pokemon Enemy_Pokemon_Current //pokemon atual inimigo
    {
        get { return enemy_pokemon_current; }
        set { enemy_pokemon_current = value; }
    }
    PokemonInfo Enemy_PokemonInfo_Current
    {
        get { return enemy_pokemonInfo_current; }
        set { enemy_pokemonInfo_current = value; }
    }

    #region Enums
    public enum Reaction
    {
        Damage, Heal, Revenge, Nothing
    }

    enum CombatType
    {
        Normal, EnemyOnly, PlayerOnly
    }

    enum EnemyBehaviour
    {
        Random, Only
    }
    #endregion

    #endregion

    void Awake()
    {
        instance = this;

        Application.targetFrameRate = 60;

        player_pokemonHealthBar_sizeIniX = rect_enemyPokemonHealthBar.sizeDelta.x;
        enemy_pokemonHealthBar_sizeIniX = rect_enemyPokemonHealthBar.sizeDelta.x;

        Pokemon_Inicialize();
    }

    void Pokemon_Inicialize() //inicializacao dos pokemons do player e do inimigo
    {
        player_pokemonInfo = new PokemonInfo[player_pokemon.Length];
        enemy_pokemonInfo = new PokemonInfo[enemy_pokemon.Length];

        {
            int i = 0;

            foreach (Pokemon _pokemon in player_pokemon)
            {
                player_pokemonInfo[i] = new PokemonInfo(_pokemon);
                i++;
            }
        }

        {
            int i = 0;

            foreach (Pokemon _pokemon in enemy_pokemon)
            {
                enemy_pokemonInfo[i] = new PokemonInfo(_pokemon);
                i++;
            }
        }

        Player_PokemonCurrent_Set(player_pokemonCurrent_index);
        Enemy_PokemonCurrent_Set(enemy_pokemonCurrent_index);
    }

    void Txt_PlayerAttack_Set()
    {
        for (int i = 0; i < 4; i++)
        {
            txt_playerAttack[i].text = player_pokemon[player_pokemonCurrent_index].attack[i].attackName;
        }
    }

    public void Player_Pokemon_Change() //troca pedida
    {
        if (PlayerTurn)
        {
            Player_PokemonCurrent_Set(player_pokemonCurrent_index == 0 ? 1 : 0);

            StartCoroutine(Turn_Routine());
        }
    }

    void Player_PokemonCurrent_Set(int _index)
    {
        player_pokemonCurrent_index = _index;

        Player_Pokemon_Current = player_pokemon[_index];
        Player_PokemonInfo_Current = player_pokemonInfo[_index];

        Destroy(go_player_pokemon);
        go_player_pokemon = Instantiate(Player_Pokemon_Current.go_mesh, player_pokemonSpawnPos, Quaternion.identity);

        Txt_PokemonName_Set();
        Txt_PlayerAttack_Set();
    }

    void Enemy_PokemonCurrent_Set(int _index)
    {
        enemy_pokemonCurrent_index = _index;

        Enemy_Pokemon_Current = enemy_pokemon[_index];
        Enemy_PokemonInfo_Current = enemy_pokemonInfo[_index];

        Destroy(go_enemy_pokemon);
        go_enemy_pokemon = Instantiate(Enemy_Pokemon_Current.go_mesh, enemy_pokemonSpawnPos, Quaternion.identity);

        Txt_PokemonName_Set();
    }

    void Txt_PokemonName_Set()
    {
        txt_playerPokemonName.text = Player_PokemonInfo_Current.pokemonName;
        txt_enemyPokemonName.text = Enemy_PokemonInfo_Current.pokemonName;
    }

    public void Player_Atk(int _index) //metodo chamado quando botoes de atk sao clicados
    {
        Debug.Log("clicado");
        if (PlayerTurn)
        {
            if (Player_PokemonInfo_Current.attackUseQuantRemaining[_index] > 0)
            {
                Player_PokemonInfo_Current.attackUseQuantRemaining[_index]--;
                Enemy_Pokemon_Current.Reaction_Get(Player_Pokemon_Current.attack[_index].attackName); //envia para o pokemon q foi atacado o nome do atk
            }
            else txt_battleFeedback.text = "atk nn pode ser utilizado";
        }
    }

    void Enemy_Atk(int _index)
    {
        Enemy_PokemonInfo_Current.attackUseQuantRemaining[_index]--;
        Player_Pokemon_Current.Reaction_Get(Enemy_Pokemon_Current.attack[_index].attackName);
    }

    public IEnumerator Reaction_Routine(Reaction _reaction, string _attackName, float _value) //acao q ira acontecer
    {
        yield return new WaitForSeconds(turnDelay);

        PokemonInfo _pokemonInfoAttacking = PlayerTurn ? Player_PokemonInfo_Current : Enemy_PokemonInfo_Current; //se for turno do player, pokemon atacante eh do player
        PokemonInfo _pokemonInfoAttacked = !PlayerTurn ? Player_PokemonInfo_Current : Enemy_PokemonInfo_Current; //se nn for turno do player, pokemon atacado eh do player

        switch (_reaction)
        {
            case Reaction.Damage:
                txt_battleFeedback.text = "O ataque " + _attackName.ToUpper() + " de " + _pokemonInfoAttacking.pokemonName.ToUpper() + " causou " + _value + " de dano a " + _pokemonInfoAttacked.pokemonName.ToUpper() + ".";
                if (PlayerTurn) { EnemyPokemon_TakeDamage(_value); } else PlayerPokemon_TakeDamage(_value);
                break;
            case Reaction.Heal:
                txt_battleFeedback.text = "O ataque " + _attackName.ToUpper() + " de " + _pokemonInfoAttacking.pokemonName.ToUpper() + " curou " + _pokemonInfoAttacked.pokemonName.ToUpper() + " em " + _value + " pontos de vida.";
                if (PlayerTurn) { EnemyPokemon_TakeDamage(-_value); } else PlayerPokemon_TakeDamage(-_value); //valor negativo cura
                break;
            case Reaction.Revenge:
                txt_battleFeedback.text = "O ataque " + _attackName.ToUpper() + " de " + _pokemonInfoAttacking.pokemonName.ToUpper() + " causou a si mesmo " + _value + " de dano.";
                if (PlayerTurn) { PlayerPokemon_TakeDamage(_value); } else EnemyPokemon_TakeDamage(_value);
                break;
            case Reaction.Nothing:
                txt_battleFeedback.text = "O ataque " + _attackName.ToUpper() + " de " + _pokemonInfoAttacking.pokemonName.ToUpper() + " foi in�til";
                break;
        }

        StartCoroutine(Turn_Routine());
    }

    IEnumerator Turn_Routine()
    {
        Debug.Log("inicializando o proximo turno");
        yield return new WaitForSeconds(turnDelay);

        PlayerTurn = !PlayerTurn;

        go_turnCube[0].SetActive(PlayerTurn);
        go_turnCube[1].SetActive(!PlayerTurn);

        string _turnOwner = PlayerTurn ? "player" : "enemy";
        Debug.Log("turno do " + _turnOwner + " iniciado");
    }

    public void Enemy_Turn() //botao chama este metodo
    {
        if (!PlayerTurn)
        {
            bool _attackValid = false; //var para verificar se o inimigo tem algum atk com uso restante

            foreach (int _attackUseQuantRemaining in Enemy_PokemonInfo_Current.attackUseQuantRemaining)
            {
                if (_attackUseQuantRemaining > 0)
                {
                    _attackValid = true;
                    break;
                }
            }

            if (_attackValid) //se existe algum atk disponivel
            {
                if (enemyBehaviour == EnemyBehaviour.Random) //se esta no modo randomico
                {
                    bool _attacked = false;

                    while (!_attacked)
                    {
                        int _value = Random.Range(0, Enemy_Pokemon_Current.attack.Length);

                        if (Enemy_PokemonInfo_Current.attackUseQuantRemaining[_value] > 0)
                        {
                            Enemy_Atk(_value);
                            break;
                        }
                    }
                }
                else if (enemyBehaviour == EnemyBehaviour.Only) //ainda nao foi implementado
                {
                    //tratar caso index seja maior q index max
                }
            }
            else
            {
                txt_battleFeedback.text = Enemy_PokemonInfo_Current.pokemonName + " n�o possui ataques dispon�veis.";

                StartCoroutine(Turn_Routine());
            }
        }
    }

    #region Damage
    void PlayerPokemon_TakeDamage(float _damage)
    {
        Player_PokemonInfo_Current.healthCurrent -= _damage;

        if (_damage > 0)
        {
            AudioManager.instance.AudioClip_Hit_Play();
            go_player_pokemon.GetComponentInChildren<Shake>().Shake_Start(0.25f);
        }

        if (Player_PokemonInfo_Current.healthCurrent > Player_PokemonInfo_Current.healthMax) Player_PokemonInfo_Current.healthCurrent = Player_PokemonInfo_Current.healthMax;

        if (Player_PokemonInfo_Current.healthCurrent <= 0) //verifica se morreu
        {
            Player_PokemonInfo_Current.healthCurrent = 0;
            Player_PokemonInfo_Current.dead = true;

            if (PlayerPokemonDeadAll_Get()) Result(false); //verifica se todos morreram
            else
            {
                List<int> _list_pokemonLiveIndex = new();

                int i = 0;

                foreach (PokemonInfo _pokemonInfo in player_pokemonInfo)
                {
                    if (!_pokemonInfo.dead) _list_pokemonLiveIndex.Add(i);
                    i++;
                }

                Player_PokemonCurrent_Set(_list_pokemonLiveIndex[Random.Range(0, _list_pokemonLiveIndex.Count)]);
                StartCoroutine(PlayerPokemonHealthBar_Set(turnDelay - 0.1f));
            }
        }

        StartCoroutine(PlayerPokemonHealthBar_Set());
    }

    void EnemyPokemon_TakeDamage(float _damage)
    {
        Enemy_PokemonInfo_Current.healthCurrent -= _damage;

        if (_damage > 0)
        {
            AudioManager.instance.AudioClip_Hit_Play();
            go_enemy_pokemon.GetComponentInChildren<Shake>().Shake_Start(0.25f);
        }

        if (Enemy_PokemonInfo_Current.healthCurrent > Enemy_PokemonInfo_Current.healthMax) Enemy_PokemonInfo_Current.healthCurrent = Enemy_PokemonInfo_Current.healthMax;

        if (Enemy_PokemonInfo_Current.healthCurrent <= 0) //verifica se morreu
        {
            Enemy_PokemonInfo_Current.healthCurrent = 0;
            Enemy_PokemonInfo_Current.dead = true;

            if (EnemyPokemonDeadAll_Get()) Result(true);
            else
            {
                List<int> _list_pokemonLiveIndex = new();

                int i = 0;

                foreach (PokemonInfo _pokemonInfo in enemy_pokemonInfo)
                {
                    if (!_pokemonInfo.dead) _list_pokemonLiveIndex.Add(i);
                    i++;
                }


                Debug.Log("UnityEditor");

                Enemy_PokemonCurrent_Set(_list_pokemonLiveIndex[Random.Range(0, _list_pokemonLiveIndex.Count)]); //seta o novo pokemon do enemy
                StartCoroutine(EnemyPokemonHealthBar_Set(turnDelay - 0.1f)); //muda a barra de vida para a nova
            }
        }

        StartCoroutine(EnemyPokemonHealthBar_Set());
    }
    #endregion

    bool PlayerPokemonDeadAll_Get() //retorna true se todos os pokemons do player morreram
    {
        bool _deadAll = true;

        foreach (PokemonInfo _pokemonInfo in player_pokemonInfo)
        {
            if (!_pokemonInfo.dead)
            {
                _deadAll = false;
                break;
            }
        }

        return _deadAll;
    }

    bool EnemyPokemonDeadAll_Get() //retorna true se todos os pokemons do enemy morreram
    {
        bool _deadAll = true;

        foreach (PokemonInfo _pokemonInfo in enemy_pokemonInfo)
        {
            if (!_pokemonInfo.dead)
            {
                _deadAll = false;
                break;
            }
        }

        return _deadAll;
    }

    #region Barras de vida
    IEnumerator PlayerPokemonHealthBar_Set(float _delay = 0f) //seta o tamanho da barra de vida do pokemon
    {
        yield return new WaitForSeconds(_delay);

        Txt_PlayerAttack_Set();
        Txt_PokemonName_Set();

        rect_playerPokemonHealthBar.sizeDelta = new Vector2(Player_PokemonInfo_Current.healthCurrent / Player_PokemonInfo_Current.healthMax * player_pokemonHealthBar_sizeIniX, rect_playerPokemonHealthBar.sizeDelta.y);
    }

    IEnumerator EnemyPokemonHealthBar_Set(float _delay = 0f) //seta o tamanho da barra de vida do pokemon
    {
        yield return new WaitForSeconds(_delay);

        rect_enemyPokemonHealthBar.sizeDelta = new Vector2(Enemy_PokemonInfo_Current.healthCurrent / Enemy_PokemonInfo_Current.healthMax * enemy_pokemonHealthBar_sizeIniX, rect_enemyPokemonHealthBar.sizeDelta.y);
    }
    #endregion

    void Result(bool _victory) //resultado do jogo
    {
        if (_victory)
        {
            Debug.Log("vitoria");
        }
        else
        {
            Debug.Log("derrota");
        }
    }
}
//using OpenCover.Framework.Model;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public static BattleManager instance;

    [Header("Chef�o")]
    [SerializeField] bool isBossFight = false; // Indica se o inimigo atual � um chef�o.
    [SerializeField] float bossEnergy = 0; // Energia acumulada do chef�o.
    [SerializeField] float bossEnergyThreshold = 100; // Limite para ativar o ataque especial do chef�o.

    [Header("Bot�es de Sele��o de Pok�mon")]
    [SerializeField] UnityEngine.UI.Button[] pokemonSelectionButtons;

    [SerializeField] Pokemon[] player_pokemon; // Ferramentas do jogador
    [SerializeField] PokemonInfo[] player_pokemonInfo;
    [SerializeField] Pokemon[] enemy_pokemon; // Inimigos.
    [SerializeField] PokemonInfo[] enemy_pokemonInfo;
    [SerializeField] Vector3 player_pokemonSpawnPos;
    [SerializeField] Vector3 enemy_pokemonSpawnPos;
    [SerializeField] GameObject[] go_turnCube;

    [Header("Vida do player")]
    [SerializeField] float player_healthCurrent;
    [SerializeField] float player_healthMax;

    [Header("Combate")]
    [SerializeField] CombatType combatType;
    [SerializeField] EnemyBehaviour enemyBehaviour;
    [SerializeField] [Range(0, 3)] int enemyAttackIndex;
    [SerializeField] float turnDelay;
    [SerializeField] float enemyTurnDelay;

    [Header("Ferramenta atual do jogador")]
    [SerializeField] Pokemon player_pokemon_current; // Somente leitura.
    [SerializeField] PokemonInfo player_pokemonInfo_current;

    [Header("Inimigo atual")]
    [SerializeField] Pokemon enemy_pokemon_current; // Somente leitura.
    [SerializeField] PokemonInfo enemy_pokemonInfo_current;

    [Header("Canvas")]
    [SerializeField] TextMeshProUGUI txt_battleFeedback;
    [SerializeField] TextMeshProUGUI[] txt_playerAttack;

    [SerializeField] TextMeshProUGUI txt_playerPokemonName;
    [SerializeField] TextMeshProUGUI txt_enemyPokemonName;
    [SerializeField] RectTransform rect_playerPokemonHealthBar;
    [SerializeField] RectTransform rect_enemyPokemonHealthBar;
    [SerializeField] CanvasGroup cg_playerPokemonHealthBar;
    [SerializeField] CanvasGroup cg_enemyPokemonHealthBar;

    [SerializeField] Color color_disable;

    float player_pokemonHealthBar_sizeIniX;
    float enemy_pokemonHealthBar_sizeIniX;

    int player_pokemonCurrent_index = 0;
    int enemy_pokemonCurrent_index = 0;

    GameObject go_player_pokemon;
    GameObject go_enemy_pokemon;

    bool playerTurn = true; // Define o turno atual entre o jogador e o inimigo.
    bool PlayerTurn
    {
        get { return playerTurn; }
        set { playerTurn = value; }
    }

    Pokemon Player_Pokemon_Current // Ferramenta atual do jogador.
    {
        get { return player_pokemon_current; }
        set { player_pokemon_current = value; }
    }
    PokemonInfo Player_PokemonInfo_Current
    {
        get { return player_pokemonInfo_current; }
        set { player_pokemonInfo_current = value; }
    }

    Pokemon Enemy_Pokemon_Current // Inimigo atual.
    {
        get { return enemy_pokemon_current; }
        set { enemy_pokemon_current = value; }
    }
    PokemonInfo Enemy_PokemonInfo_Current
    {
        get { return enemy_pokemonInfo_current; }
        set { enemy_pokemonInfo_current = value; }
    }

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

    Ponto_Incendio pontoIncend;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        Application.targetFrameRate = 60;

        player_pokemonHealthBar_sizeIniX = rect_enemyPokemonHealthBar.sizeDelta.x;
        enemy_pokemonHealthBar_sizeIniX = rect_enemyPokemonHealthBar.sizeDelta.x;

        Pokemon_Inicialize();
    }

    void Pokemon_Inicialize() // Inicializacao das ferramentas do jogador e inimigo.
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

        player_healthCurrent = player_healthMax;
    }

    void Txt_PlayerAttack_Set()
    {
        // Obtem a ferramenta atual.
        var currentPokemon = player_pokemon[player_pokemonCurrent_index];
        int attacksCount = currentPokemon.attack.Length;

        // Percorrer todos os TextMeshProUGUI para configurar os bot�es de ataque.
        for (int i = 0; i < txt_playerAttack.Length; i++)
        {
            if (i < attacksCount)
            {
                // Exibir o bot�o de ataque e configurar o texto com a quantidade restante.
                txt_playerAttack[i].gameObject.SetActive(true);
                int remainingUses = Player_PokemonInfo_Current.attackUseQuantRemaining[i];
                txt_playerAttack[i].text = $"{currentPokemon.attack[i].attackName} ({remainingUses})";

                // Desativar o bot�o se n�o houver usos restantes.
                var button = txt_playerAttack[i].GetComponentInParent<UnityEngine.UI.Button>();
                if (button != null)
                {
                    button.interactable = remainingUses > 0;
                }
            }
            else
            {
                // Ocultar os bot�es de ataque para ataques inexistentes.
                txt_playerAttack[i].gameObject.SetActive(false);

                // Desativar os bot�es de ataque extras.
                var button = txt_playerAttack[i].GetComponentInParent<UnityEngine.UI.Button>();
                if (button != null)
                {
                    button.interactable = false;
                }
            }
        }

        // Caso nenhum ataque esteja dispon�vel, desativar todos os bot�es.
        if (attacksCount == 0)
        {
            foreach (var txt in txt_playerAttack)
            {
                txt.gameObject.SetActive(false);

                var button = txt.GetComponentInParent<UnityEngine.UI.Button>();
                if (button != null)
                {
                    button.interactable = false;
                }
            }
        }
    }

    public void Player_Pokemon_Change(int _value) // Troca de ferramenta.
    {
        Debug.Log("Troca de Ferramenta.");

        if (PlayerTurn && _value != player_pokemonCurrent_index)
        {
            Player_PokemonCurrent_Set(_value);
            UpdatePokemonSelectionButtons(); // Atualiza os bot�es
            StartCoroutine(Turn_Routine());
        }
    }

    void UpdatePokemonSelectionButtons()
    {
        for (int i = 0; i < pokemonSelectionButtons.Length; i++)
        {
            if (pokemonSelectionButtons[i] != null)
            {
                // Desabilitar o bot�o do Pok�mon atual.
                pokemonSelectionButtons[i].interactable = (i != player_pokemonCurrent_index);
            }
        }
    }

    void Player_PokemonCurrent_Set(int _index)
    {
        player_pokemonCurrent_index = _index;
        Player_Pokemon_Current = player_pokemon[_index];
        Player_PokemonInfo_Current = player_pokemonInfo[_index];

        if (go_player_pokemon != null)
        {
            Destroy(go_player_pokemon);
        }

        go_player_pokemon = Instantiate(Player_Pokemon_Current.go_mesh, player_pokemonSpawnPos, Quaternion.identity);

        Txt_PokemonName_Set();
        Txt_PlayerAttack_Set();
        UpdatePokemonSelectionButtons();
    }

    void Enemy_PokemonCurrent_Set(int _index)
    {
        enemy_pokemonCurrent_index = _index;
        Enemy_Pokemon_Current = enemy_pokemon[_index];
        Enemy_PokemonInfo_Current = enemy_pokemonInfo[_index];

        if (go_enemy_pokemon != null)
        {
            Destroy(go_enemy_pokemon);
        }

        go_enemy_pokemon = Instantiate(Enemy_Pokemon_Current.go_mesh, enemy_pokemonSpawnPos, Quaternion.identity);

        Txt_PokemonName_Set();
    }

    void Txt_PokemonName_Set() // Coloca o nome da ferramenta nos textos.
    {
        txt_playerPokemonName.text = Player_PokemonInfo_Current.pokemonName;
        txt_enemyPokemonName.text = Enemy_PokemonInfo_Current.pokemonName;
    }

    public void Player_Atk(int _index) // Metodo chamado quando os botoes de ataque sao clicados.
    {
        Debug.Log("Jogador atacou.");

        if (PlayerTurn)
        {
            var currentPokemon = Player_Pokemon_Current;

            // Verifica se o indice do ataque clicado e valido para o Pokemon atual
            if (_index >= 0 && _index < currentPokemon.attack.Length)
            {
                // Verifica se o ataque tem usos restantes
                if (Player_PokemonInfo_Current.attackUseQuantRemaining[_index] > 0)
                {
                    Player_PokemonInfo_Current.attackUseQuantRemaining[_index]--;
                    Enemy_Pokemon_Current.Reaction_Get(currentPokemon.attack[_index].attackName);
                }
                else
                {
                    txt_battleFeedback.text = "Esse ataque n�o pode ser utilizado.";
                }
            }
            else
            {
                Debug.LogWarning("Ataque inv�lido selecionado. �ndice fora dos limites.");
                txt_battleFeedback.text = "Esse ataque n�o est� dispon�vel.";
            }
        }
    }

    void Enemy_Atk(int _index)
    {
        Enemy_PokemonInfo_Current.attackUseQuantRemaining[_index]--;
        Player_Pokemon_Current.Reaction_Get(Enemy_Pokemon_Current.attack[_index].attackName);
    }

    public IEnumerator Reaction_Routine(Reaction _reaction, string _attackName, float _value) // Acao que vai acontecer.
    {
        yield return new WaitForSeconds(turnDelay);

        PokemonInfo _pokemonInfoAttacking = PlayerTurn ? Player_PokemonInfo_Current : Enemy_PokemonInfo_Current; // Se for turno do jogador, personagem atacante sera do jogador.
        PokemonInfo _pokemonInfoAttacked = !PlayerTurn ? Player_PokemonInfo_Current : Enemy_PokemonInfo_Current; // Se nao for turno do jogador, personagem atacado sera do jogador.

        switch (_reaction)
        {
            case Reaction.Damage:
                txt_battleFeedback.text = "O ataque " + _attackName.ToUpper() + " de " + _pokemonInfoAttacking.pokemonName.ToUpper() + " causou " + _value + " de dano a " + _pokemonInfoAttacked.pokemonName.ToUpper() + ".";
                if (PlayerTurn) { EnemyPokemon_TakeDamage(_value); } else Player_TakeDamage(_value);
                break;
            case Reaction.Heal:
                txt_battleFeedback.text = "A habilidade " + _attackName.ToUpper() + " de " + _pokemonInfoAttacking.pokemonName.ToUpper() + " curou " + _pokemonInfoAttacked.pokemonName.ToUpper() + " em " + _value + " pontos de vida.";
                if (PlayerTurn) { EnemyPokemon_TakeDamage(-_value); } else Player_TakeDamage(-_value); // Valor negativo cura.
                break;
            case Reaction.Revenge:
                txt_battleFeedback.text = "O ataque " + _attackName.ToUpper() + " de " + _pokemonInfoAttacking.pokemonName.ToUpper() + " causou a si mesmo " + _value + " de dano.";
                if (PlayerTurn) { Player_TakeDamage(_value); } else EnemyPokemon_TakeDamage(_value);
                break;
            case Reaction.Nothing:
                txt_battleFeedback.text = "O ataque " + _attackName.ToUpper() + " de " + _pokemonInfoAttacking.pokemonName.ToUpper() + " n�o foi efetivo";
                break;
        }

        StartCoroutine(Turn_Routine());
    }

    IEnumerator Turn_Routine()
    {
        Debug.Log("Inicializando o pr�ximo turno.");
        yield return new WaitForSeconds(turnDelay);

        PlayerTurn = !PlayerTurn;

        go_turnCube[0].SetActive(PlayerTurn);
        go_turnCube[1].SetActive(!PlayerTurn);

        string _turnOwner = PlayerTurn ? "Jogador" : "Inimigo";
        Debug.Log("Turno do " + _turnOwner + " iniciado.");

        if (!PlayerTurn)
        {
            yield return new WaitForSeconds(enemyTurnDelay);

            Enemy_Turn();
        }
    }

    void Enemy_Turn() // O bot�o chama este m�todo caso o ataque n�o seja autom�tico.
    {
        if (isBossFight)
        {
            bossEnergy += Random.Range(10, 30); // Aumenta a energia do chef�o a cada turno.

            // Ajusta o tamanho do chef�o proporcionalmente � energia acumulada.
            if (go_enemy_pokemon != null) // Certifica-se de que o objeto instanciado existe.
            {
                float scaleMultiplier = Mathf.Clamp(1 + (bossEnergy / bossEnergyThreshold) * 0.5f, 1, 2);
                go_enemy_pokemon.transform.localScale = new Vector3(scaleMultiplier, scaleMultiplier, scaleMultiplier);
            }

            if (bossEnergy >= bossEnergyThreshold * 0.75f && bossEnergy < bossEnergyThreshold)
            {
                EnterRageMode();
            }

            if (bossEnergy >= bossEnergyThreshold)
            {
                Boss_SpecialAttack();
                bossEnergy = 0; // Reseta a energia ap�s o ataque especial.
                return; // Finaliza o turno do chef�o.
            }
        }

        // L�gica existente para ataques regulares do inimigo.
        bool _attackValid = false;
        foreach (int _attackUseQuantRemaining in Enemy_PokemonInfo_Current.attackUseQuantRemaining)
        {
            if (_attackUseQuantRemaining > 0)
            {
                _attackValid = true;
                break;
            }
        }

        if (_attackValid)
        {
            if (enemyBehaviour == EnemyBehaviour.Random)
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
            else if (enemyBehaviour == EnemyBehaviour.Only)
            {
                // Tratar caso o �ndice seja maior que o m�ximo.
            }
        }
        else
        {
            txt_battleFeedback.text = Enemy_PokemonInfo_Current.pokemonName + " n�o possui ataques dispon�veis.";
            StartCoroutine(Turn_Routine());
        }
    }

    /// <summary>
    /// Ativa o modo enfurecido do chef�o, aumentando o poder dos ataques.
    /// </summary>
    void EnterRageMode()
    {
        Debug.Log("O Chef�o entrou no modo enfurecido!");
        txt_battleFeedback.text = "O Chef�o est� enfurecido! Seus ataques s�o mais poderosos!";

        // Opcional: altere a cor do chef�o para indicar que ele est� enfurecido.
        if (go_enemy_pokemon != null)
        {
            Renderer renderer = go_enemy_pokemon.GetComponentInChildren<Renderer>();
            if (renderer != null)
            {
                renderer.material.color = Color.red; // Define a cor do modo enfurecido.
            }
        }

        // Aumenta o poder dos ataques do chef�o.
        foreach (var attack in Enemy_Pokemon_Current.attack)
        {
            attack.damage += 10; // Adiciona dano extra a cada ataque.
        }
    }

    /// <summary>
    /// Executa o ataque especial do chef�o, causando dano em �rea.
    /// </summary>
    void Boss_SpecialAttack()
    {
        Debug.Log("O Chef�o executou seu ataque especial!");
        float damage = Random.Range(20, 50); // Dano do ataque especial.
        Player_TakeDamage(damage); // Aplica o dano ao jogador.

        // Atualiza o feedback para o jogador.
        txt_battleFeedback.text = "O Chef�o lan�ou um ataque devastador, causando " + damage + " de dano!";
    }

    void Player_TakeDamage(float _damage)
    {
        player_healthCurrent -= _damage;

        if (_damage > 0)
        {
            AudioManager.instance.AudioClip_Hit_Play();
            go_player_pokemon.GetComponentInChildren<Shake>().Shake_Start(0.25f);
        }

        if (player_healthCurrent > player_healthMax) player_healthCurrent = player_healthMax;

        if (player_healthCurrent <= 0) // Verifica se morreu.
        {
            Result(false);
            player_healthCurrent = 0;
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

            if (EnemyPokemon_DeadAll_Get()) Result(true);
            else
            {
                List<int> _list_pokemonLiveIndex = new();

                int i = 0;

                foreach (PokemonInfo _pokemonInfo in enemy_pokemonInfo)
                {
                    if (!_pokemonInfo.dead) _list_pokemonLiveIndex.Add(i);
                    i++;
                }

                Enemy_PokemonCurrent_Set(_list_pokemonLiveIndex[Random.Range(0, _list_pokemonLiveIndex.Count)]); //seta o novo pokemon do enemy
                StartCoroutine(EnemyPokemonHealthBar_Set(turnDelay - 0.1f)); //muda a barra de vida para a nova
            }
        }

        StartCoroutine(EnemyPokemonHealthBar_Set());
    }

    bool EnemyPokemon_DeadAll_Get() // Retorna true se todos os personagens do inimigo morreram.
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

    IEnumerator PlayerPokemonHealthBar_Set(float _delay = 0f) // Seta o tamanho da barra de vida do personagem.
    {
        yield return new WaitForSeconds(_delay);

        Txt_PlayerAttack_Set();
        Txt_PokemonName_Set();

        //rect_playerPokemonHealthBar.sizeDelta = new Vector2(Player_PokemonInfo_Current.healthCurrent / Player_PokemonInfo_Current.healthMax * player_pokemonHealthBar_sizeIniX, rect_playerPokemonHealthBar.sizeDelta.y);
        rect_playerPokemonHealthBar.sizeDelta = new Vector2(player_healthCurrent / player_healthMax * player_pokemonHealthBar_sizeIniX, rect_playerPokemonHealthBar.sizeDelta.y);
    }

    IEnumerator EnemyPokemonHealthBar_Set(float _delay = 0f) // Seta o tamanho da barra de vida do personagem.
    {
        yield return new WaitForSeconds(_delay);

        rect_enemyPokemonHealthBar.sizeDelta = new Vector2(Enemy_PokemonInfo_Current.healthCurrent / Enemy_PokemonInfo_Current.healthMax * enemy_pokemonHealthBar_sizeIniX, rect_enemyPokemonHealthBar.sizeDelta.y);
    }

    void Result(bool _victory) // Resultado do combate.
    {
        if (_victory)
        {
            Debug.Log("Venceu!");
        }
        else
        {
            Debug.Log("Perdeu!");
        }
        if(pontoIncend!=null)
            pontoIncend.AtualizarObjetivo(_victory);
    }

    public void Recharge(int _index = -1) // Metodo para recarregar os ataques.
    {
        if (_index == -1)
        {
            for (int i = 0; i < player_pokemonInfo.Length; i++)
            {
                player_pokemonInfo[i] = new PokemonInfo(player_pokemon[i]);
            }
        }
        else
        {
            player_pokemonInfo[_index] = new PokemonInfo(player_pokemon[_index]);
        }
    }
}
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControladorInimigo : MonoBehaviour
{
    //FSM [Finite State Machine]
    //Estados: Patrulha, Perseguir, Parado
    //1. Definindo os estados possiveis (Não termina com ";" e não precisa de aspas)
    public enum EstadoInimigo { Parado, Patrulha, Perseguicao}

    [Header("IA do inimigo")]
    public EstadoInimigo estadoAtual;

    [Header("Movimentação")]
    public float velocidade = 2.0f;
    public Transform[] pontosDePatrulha; //Armazena os pontos de parada/patrulha
    public int indicePontoAtual = 0;     //Ponto a ser visitado

    [Header("Espera")]
    public float tempoDeEspera = 2.0f;
    public float cronometroEspera = 0f;

    [Header("Sensores")]
    public float raioVisao = 5f; //Distancia para começar a perseguir
    public float raioPerseguicao = 8f; //Distancia para desistir da perseguição
    public float distanciaAtaque = 1f; //Distancia para iniciar o combate

    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private GameObject jogador; //Armazena os dados do Jogador

    private Rigidbody2D rb;
    private Collider2D meuCollider;

    private Vector2 destinoMovimento;
    private float velocidadeAtual;
    private bool estaSeMovendo = false;

    [Header("Identidade")]
    public string nomeId;

    private void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        meuCollider = GetComponent<Collider2D>();

        //Ao iniciar a partida, começa parado (idle)
        estadoAtual = EstadoInimigo.Patrulha;

        //Procura pelo GameObject com a tag Player
        if(jogador == null)
        {
            jogador = GameObject.FindGameObjectWithTag("Player");
        }
    }

    private void Update()
    {
        estaSeMovendo = false;

        //0. Segurança. Se o jogador morrer ou não for encontrado, não faz nada.
        if(jogador == null)
        {
            return;
        }

        //1. Calculando a distância entre os gameobjects
        float distancia = Vector2.Distance(transform.position, jogador.transform.position);
        //Debug.Log($"Distancia entre INIMIGO e JOGADOR: {distancia}");


        switch (estadoAtual) 
        { 
            case EstadoInimigo.Parado:
                //Toca a animação em velocidade normal
                animator.speed = 1f;

                animator.SetBool("Andando", false);
                Debug.Log($"{gameObject.name} mudando para o estado {estadoAtual}.");
                break;
            case EstadoInimigo.Patrulha:
                //Regra 01: Se o jogador entro no raio de visão
                //Toca a animação em velocidade normal
                animator.speed = 1f;

                if (distancia < raioVisao)
                {
                    estadoAtual = EstadoInimigo.Perseguicao;
                }
                animator.SetBool("Andando", true);
                Debug.Log($"{gameObject.name} mudando para o estado {estadoAtual}.");
                Patrulhar();
                break;
            case EstadoInimigo.Perseguicao:
                //Regra 02: O jogador saiu do raio de perseguição

                //Toca a animação em velocidade 50% mais rapida
                animator.speed = 1.5f;
                if(distancia > raioPerseguicao)
                {
                    estadoAtual = EstadoInimigo.Patrulha;
                }

                //Regra 03: Inimigo alcançou o jogador. Inicio de combate.
                if(distancia < distanciaAtaque)
                {
                    //Inicio do combate
                    IniciarCombate();
                }
                else
                {
                    //Continua a perseguição
                    Perseguir();
                }
                animator.SetBool("Andando", true);
                Debug.Log($"{gameObject.name} mudando para o estado {estadoAtual}.");
                break;
        }
    }

    private void FixedUpdate()
    {
        if (estaSeMovendo == true) 
        {
            //Captura a nova posição do jogador
            Vector2 novaPosicao = Vector2.MoveTowards(rb.position, destinoMovimento, velocidadeAtual * Time.fixedDeltaTime);
            rb.MovePosition(novaPosicao);
        }
    }

    private void IniciarCombate()
    {
        Debug.Log("Inicio do combate!");
        
        DadosGlobais.inimigoParaGerar = nomeId;

        SceneManager.LoadScene("Arena");
    }

    private void Perseguir()
    {
        //Desativa o modo fantasma
        if(meuCollider != null)
        {
            meuCollider.isTrigger = false;
        }

        //Mover Personagem
        Mover(jogador.transform.position, velocidade * 1.5f);
    }

    private void Flip(Vector3 direcao)
    {
        if (direcao.x < -0.1f)
        {
            spriteRenderer.flipX = true;
        }
        else if (direcao.x > 0.1f)
        {
            spriteRenderer.flipX = false;
        }
    }

    private void Patrulhar()
    {
        //Modo fantasma
        if(meuCollider != null)
        {
            meuCollider.isTrigger = true;
        }

        Transform alvo = pontosDePatrulha[indicePontoAtual];

        if(Vector2.Distance(transform.position, alvo.position) < 0.1f)
        {
            animator.SetBool("Andando", false);
            cronometroEspera += Time.deltaTime;

            if(cronometroEspera >= tempoDeEspera)
            {
                cronometroEspera = 0;
                indicePontoAtual++;

                if(indicePontoAtual >= pontosDePatrulha.Length)
                {
                    indicePontoAtual = 0;
                }
            }
        }
        else
        {
            //Mover Personagem
            Mover(alvo.position, velocidade);
        }
    }

    private void Mover(Vector3 destino, float velocidadeMovimento)
    {
        Vector3 direcao = (destino - transform.position).normalized;

        animator.SetBool("Andando", true);
        animator.SetFloat("Horizontal", direcao.x);
        animator.SetFloat("Vertical", direcao.y);

        Flip(direcao);

        //Atualiza a variavel do FixedUpdate
        destinoMovimento = destino;
        velocidadeAtual = velocidadeMovimento;
        estaSeMovendo = true;
    }

    private void OnDrawGizmosSelected()
    {
        //Desenha o raio de visão (vermelho)
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, raioVisao);

        //Desenha o raio da perseguição (amarelo)
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, raioPerseguicao);

        //Desenha a distancia entre o inimigo e o jogador (azul)
        Gizmos.color = Color.blue;
        if (jogador != null) 
        {
            Gizmos.DrawLine(transform.position, jogador.transform.position);
        }        
    }
}

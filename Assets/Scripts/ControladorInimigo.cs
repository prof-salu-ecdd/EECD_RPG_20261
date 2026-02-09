using UnityEngine;

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

    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        //Ao iniciar a partida, começa parado (idle)
        estadoAtual = EstadoInimigo.Parado;
    }

    private void Update()
    {
        switch (estadoAtual) 
        { 
            case EstadoInimigo.Parado:
                animator.SetBool("Andando", false);
                Debug.Log($"{gameObject.name} mudando para o estado {estadoAtual}.");
                break;
            case EstadoInimigo.Patrulha:
                animator.SetBool("Andando", true);
                Debug.Log($"{gameObject.name} mudando para o estado {estadoAtual}.");
                Patrulhar();
                break;
            case EstadoInimigo.Perseguicao:
                animator.SetBool("Andando", true);
                Debug.Log($"{gameObject.name} mudando para o estado {estadoAtual}.");
                break;
        }
    }
    private void Patrulhar()
    {
        //Defino o ponto(alvo) atual da patrulha
        Transform alvo = pontosDePatrulha[indicePontoAtual];

        //1. Verifica a distancia (Chegou ao alvo?)
        if (Vector2.Distance(transform.position, alvo.position) < 0.1f) 
        {
            //Chegou ao ponto (ESPERA)
            animator.SetBool("Andando", false);

            //Conta o tempo de espera
            cronometroEspera += Time.deltaTime;

            //Se o tempo passou do limite
            if(cronometroEspera >= tempoDeEspera)
            {
                //Reseta o cronometro
                cronometroEspera = 0;
                indicePontoAtual++;

                //Caso a posicao atual do alvo, seja maior que o total de posicoes, volta para o começo.

                if(indicePontoAtual>= pontosDePatrulha.Length)
                {
                    indicePontoAtual = 0;
                }
            }
        }
        else
        {
            //Se não chegou ao destino, continua andando

            //2. Calcular a direção
            Vector2 direcao = (alvo.position - transform.position).normalized;

            //3. Atualizar o animator
            animator.SetBool("Andando", true);
            animator.SetFloat("Horizontal", direcao.x);
            animator.SetFloat("Vertical", direcao.y);

            //4. Flip
            if(direcao.x < -0.1f)
            {
                spriteRenderer.flipX = true;
            }
            else if(direcao.x > 0.1f)
            {
                spriteRenderer.flipX = false;
            }

            //5. Mover
            transform.position = Vector2.MoveTowards(transform.position,
                                                            alvo.position,
                                                            velocidade * Time.deltaTime);
        }
    }
}

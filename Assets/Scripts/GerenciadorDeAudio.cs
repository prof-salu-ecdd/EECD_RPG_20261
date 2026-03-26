using UnityEngine;

public class GerenciadorDeAudio : MonoBehaviour
{
    public static GerenciadorDeAudio instance;

    public AudioSource fonteMusica;
    public AudioSource fonteSFX;

    public AudioClip somColeta;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void TocarMusica(AudioClip musica)
    {
        //Evita que reinicie a mesma musica
        if (fonteMusica.clip == musica) return;

        fonteMusica.clip = musica;
        fonteMusica.Play();
    }

    public void TocarSFX(AudioClip efeitoSonoro)
    {
        fonteSFX.PlayOneShot(efeitoSonoro);
    }

    public void SomColeta()
    {
        TocarSFX(somColeta);
    }
}

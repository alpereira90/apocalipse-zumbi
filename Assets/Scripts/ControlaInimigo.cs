﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlaInimigo : MonoBehaviour, IMatavel {

	public GameObject Jogador;
	public GameObject KitMedico;
	public AudioClip SomDeMorte;
	
	
	private Vector3 direcao;
	private Vector3 posicaoAleatoriaVagar;
	private float contadorVagar;
	private float tempoEntrePosicoesVagar = 4;
	private ControlaJogador controlaJogador;
	private MovimentoPersonagem movimentoInimigo;
	private AnimacaoPersonagem animacaoInimigo;
	private Status statusInimigo;
	private float procentagemGerarKitMedico = 0.1f;
	private ControlaInterface controlaInterface; 

	// Use this for initialization
	void Start () {
		Jogador = GameObject.FindWithTag("Jogador");
		controlaJogador = Jogador.GetComponent<ControlaJogador>();
		movimentoInimigo = GetComponent<MovimentoPersonagem>();	
		animacaoInimigo = GetComponent<AnimacaoPersonagem>();
		statusInimigo = GetComponent<Status>();
		AleatorizarZumbi();	
		controlaInterface = GameObject.FindObjectOfType(typeof(ControlaInterface)) as ControlaInterface;			
	}

	void FixedUpdate(){
        float distancia = Vector3.Distance(transform.position, Jogador.transform.position);

        movimentoInimigo.Rotacionar(direcao);
        animacaoInimigo.AnimarMovimento(direcao);

        if(distancia > statusInimigo.AlcanceVisao){
            Vagar ();
        }
        else if (distancia > statusInimigo.AlcanceAtaque){
            direcao = Jogador.transform.position - transform.position;
            movimentoInimigo.Movimentar(direcao, statusInimigo.Velocidade);
            animacaoInimigo.Atacar(false);
        }
        else{
            direcao = Jogador.transform.position - transform.position;
            animacaoInimigo.Atacar(true);
        }
    }

	void Vagar(){
		contadorVagar -= Time.deltaTime;

		if (contadorVagar <= 0){
			posicaoAleatoriaVagar = PosicaoAleatoria();			
			contadorVagar = tempoEntrePosicoesVagar;
		}

		if(Vector3.Distance(transform.position, posicaoAleatoriaVagar) > 0.05){
			direcao = posicaoAleatoriaVagar - transform.position;
			movimentoInimigo.Movimentar(direcao, statusInimigo.Velocidade);
		}
	}

	Vector3 PosicaoAleatoria(){
		Vector3 posicao = Random.insideUnitSphere * 10;
		posicao += transform.position;
		posicao.y = transform.position.y;

		return posicao;
	}

	void AtacaJogador(){
		int dano = Random.Range(10, 21);
		controlaJogador.TomarDano(dano);
	}

	void AleatorizarZumbi(){
		int tipoZumbi = Random.Range(1,28);
		transform.GetChild(tipoZumbi).gameObject.SetActive(true);
	}

	public void TomarDano(int dano){
		statusInimigo.Vida -= dano;
		if (statusInimigo.Vida <= 0){
			Morrer();
		}
	}

	public void Morrer(){		
		Destroy(gameObject);
		ControlaAudio.instancia.PlayOneShot(SomDeMorte);
		VerificarGeracaoDeKitMedico();
		controlaInterface.IncrementaZumbisMortos();
	}

	void VerificarGeracaoDeKitMedico(){
		if (Random.value <= procentagemGerarKitMedico){
			Instantiate(KitMedico, transform.position, Quaternion.identity);
		}
	}
}

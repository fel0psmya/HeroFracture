# Dinâmicas de Jogo e Loop (Game Loop)

Este arquivo descreve o fluxo de interação entre o jogador e as mecânicas que a Unity processará durante o tempo de execução (Runtime).

## O Ciclo Principal de Jogo (Loop)
1. **Avanço no Cenário:** O jogador movimenta o Samurai por uma plataforma com tema cyberpunk (Laboratório, Periferia Neon, Torre Central).
2. **Combate Regular:** Enfrentamento de capangas básicos (Drones e Guardas).
3. **Sistema de Recompensas (Data-Nodes):** Inimigos mortos ou locais difíceis no cenário dropam Data-Nodes.
4. **Upgrade de Habilidades:** Gasto dos Data-Nodes para:
   - Aumentar dano físico da Katana.
   - Diminuir o tempo de recarga do dash.
   - Aumentar a vida máxima.
5. **Combate de Chefe:** Evento de fechamento da fase (ex: Enfrentamento contra o Policial Corrompido).
6. **Vitória:** Ocorre exclusivamente quando a Vida (Health) do Chefe zera, e um evento chama a tela de "Vitória".

## Componentes Físicos (Unity Physics 2D)
A interação do mundo de HeroFracture baseia-se pesadamente na Physics2D da Unity:
- **Katana:** Utiliza colisores definidos como "Triggers" ativados apenas nos quadros (frames) de ataque da animação, gerando o dano ao tocar na "Hitbox" (Capsule Collider 2D) do inimigo.
- **Movimentação:** Aplicada no Rigidbody2D alterando o `velocity` para movimentação responsiva, em vez de alterar transform (permitindo que forças gravitacionais ajam normalmente sobre o Samurai).

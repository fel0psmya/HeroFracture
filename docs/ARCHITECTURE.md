# Arquitetura e Estrutura Técnica

## Unity Engine
- **Versão do Projeto:** 6000.4.1f1
- **Renderização:** Ambiente inteiramente 2D.
- **Ferramentas Nativas:** O projeto faz uso pesado de `Rigidbody2D`, `Colliders 2D`, `Animator` e `Tilemaps`.

## Estrutura de Diretórios (`Assets/`)
A organização do projeto segue a arquitetura padrão para projetos 2D limpos:

- `Animations/`: Controladores de Animação (Animator) e clipes (Animations) dos personagens.
- `Materials/`: Materiais e Shaders 2D para os efeitos visuais cibernéticos e luzes neon.
- `Prefabs/`: Objetos pré-configurados prontos para instanciar (Inimigos, Chefes, Data-Nodes, Projéteis).
- `Scenes/`: Cenas principais do jogo (ex: `Level_1`, `MainMenu`, `WinScene`).
- `Scripts/`: Toda a lógica C# programada.
  - `Boss/`: Inteligência Artificial dos Chefes (ex: `BossAI.cs`).
  - `Player/`: Movimentação do Samurai, Combate e UI do Player.
  - `Managers/`: Controladores de fluxo, como `GameManager` (Gerencia vitórias e derrotas).
- `Sprites/`: Artes 2D estáticas e Spritesheets.
- `TextMesh Pro/`: Elementos de fontes avançadas para a Interface de Usuário (UI).

## Lógica de IA e Padrões de Projeto
1. **Máquina de Estados Finita (FSM):** 
   - Muito do comportamento é gerenciado via `Animator` com parâmetros (`Triggers`, `Booleans`). O script interage com esses parâmetros para transitar entre: Andar ↔ Atacar ↔ Receber Dano ↔ Morrer.
   
2. **Padrão de IA de Inimigos (ex: BossAI):**
   - **Percepção:** Checagem contínua via `Vector2.Distance`.
   - **Movimento:** Perseguição ignorando eixo Y de salto ou física de gravidade usando `Vector2.MoveTowards`.
   - **Combate:** Cooldowns baseados em `Time.time` com verificação de distâncias curtas.

## Controle de Versão e CI/CD
- Repositório mantido no GitHub (`fel0psmya/HeroFracture`).
- Uso de Branches para tarefas específicas (ex: `feature/t12-ia-chefe-fase`).
- Proteção na branch `main` exigindo validação via Pull Request para garantir que nenhum código quebre as cenas da Unity.

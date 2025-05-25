# Relatório de Projeto <!-- omit in toc -->

- Licenciatura em Engenheira Informática 2024/2025 [IADE](https://www.iade.europeia.pt/)   <!-- omit in toc -->
- Projeto 1º ano, 2º semestre
- Prática pedagógica: PBL
- Unidades curriculares: Programação e Algoritmos

## Índice <!-- omit in toc -->

- [Equipa](#equipa)
- [Distribuição de Tarefas](#distribuição-de-tarefas)
- [Arquitetura da Solução](#arquitetura-da-solução)
- [Estruturas de Dados](#estruturas-de-dados)
   
   - [Estruturas de Dados Utilizadas](#estruturas-de-dados-utilizadas)

- [Observações](#observações)

  - [Funcionalidades implementadas](#funcionalidades-implementadas)

  - [Funcionalidades não implementadas](#funcionalidades-não-implementadas)

  - [Limitações](#limitações)

## Equipa

- Tiago Manuel Antunes Cabaça - 20241185

- César de Oliveira Rodrigues - 20240449

- Lucas Dernedde Sequeira Gomes Nicolau - 20241526

- Muhammad Sudeis Abdul Latif Sacoor - 20241707

## Distribuição de Tarefas

- **Tiago Cabaça**: Implementação da estrutura MVC, lógica do tabuleiro e navios, e desenvolvimento de funções auxiliares.

- **César Rodrigues**: Lógica associada à gestão de jogadores, incluindo comandos como RJ, LJ e EJ.

- **Lucas Nicolau**: Lógica dos navios (colocação, remoção, disparos) e reforço de funcionalidades internas de jogo.

- **Muhammad Sacoor**: Validações gerais, mensagens de erro e código auxiliar de suporte às operações.

## Arquitetura da Solução

A solução desenvolvida para o projeto segue uma arquitetura modular inspirada no padrão MVC (Model-View-Controller), promovendo uma clara separação entre os componentes de interface, lógica e dados. Esta abordagem visa garantir a legibilidade, escalabilidade e manutenção do código, em conformidade com os princípios da unidade curricular.

### Separação entre Interface, Dados e Lógica da Aplicação

O projeto foi estruturado com base em três camadas funcionais distintas:

1. **Camada de Modelos (Models)**:
   
   Contém as estruturas de dados essenciais da aplicação, como Player, Ship, Board e ShipType. Estes modelos representam o estado do jogo e encapsulam comportamentos fundamentais, como o registo de tiros, a verificação de colisões e a validação de limites do tabuleiro. Foram também implementadas interfaces (IPlayer, IShip, IBoard) para garantir a abstração e coesão dos modelos.

2. **Camada de Controladores (Controllers)**:
   
   Responsável pela lógica da aplicação, esta camada inclui o GameController, que interpreta os comandos introduzidos pelo utilizador, e o AssistController, que executa as operações correspondentes. Esta separação permite uma organização lógica e clara das funcionalidades, garantindo que a lógica do jogo não se mistura com a camada de apresentação.

3. **Camada de Interface (Views)**:
   
   Representada pela classe CLI, esta camada é responsável por interagir com o utilizador, apresentando mensagens, erros e o estado do tabuleiro. Esta separação assegura que toda a apresentação visual é independente da lógica interna do jogo.


### Organização Modular

   O ponto de entrada da aplicação (Program.cs) tem como única responsabilidade criar o controlador principal e processar os comandos inseridos na consola. Toda a lógica do jogo é delegada às camadas apropriadas, seguindo o princípio da responsabilidade única.

      /Controllers
          GameController.cs
          AssistController.cs
      /Models
          Player.cs
          Ship.cs
          Board.cs
          ShipType.cs
      /Interfaces
          IPlayer.cs
          IShip.cs
          IBoard.cs
      /Views
          CLI.cs
      Program.cs


   Esta organização modular permite:

   - Facilidade de manutenção, com código segmentado por responsabilidade.

   - Melhor testabilidade, com camadas independentes.

   - Reutilização e extensibilidade, graças ao uso de interfaces.

   - Coerência com a metodologia da unidade curricular, promovendo boas práticas de engenharia de software.


## Estruturas de Dados

### Estruturas de Dados Utilizadas

A escolha das estruturas de dados teve como base a simplicidade, eficiência e adequação à escala do problema (jogo de dois jogadores com grelhas fixas de 10x10). Foram utilizadas as seguintes estruturas:

<h3 style="margin-left: 20px;">List &lt;T&gt; </h3>

- Utilizada para armazenar:

   - Lista global de jogadores (List<Player>).

   - Lista de navios colocados em cada tabuleiro (List<Ship>).

- Justificação: permite iteração eficiente, ordenação (OrderBy) e buscas simples com FirstOrDefault, adequadas ao volume reduzido de elementos.

 <h3 style="margin-left: 20px;">Dictionary &lt;ShipType, int&gt; </h3>

- Usado para registar o número de navios de cada tipo colocados.

- Justificação: acesso direto em tempo constante (O(1)) ao número de navios por tipo, essencial para validação das regras de limite por tipo.

<h3 style="margin-left: 20px;">char[,] (Array Bidimensional)</h3>

- Representa a grelha 10x10 do tabuleiro de cada jogador (Board.Grid).

- Justificação: acesso imediato a qualquer célula com coordenadas específicas, ideal para representar espaços de jogo.

<h3 style="margin-left: 20px;">HashSet&lt;(int, int)&gt;</h3>

- Usado para registar as coordenadas dos tiros disparados (Board.ShotsFired).

- Justificação: garante verificação de duplicados e acesso extremamente rápido para evitar tiros repetidos (O(1)), essencial para regras de validação.

<h3 style="margin-left: 20px;">Enum (ShipType)</h3>

- Usado para registar as coordenadas dos tiros disparados (Board.ShotsFired).

- Justificação: garante verificação de duplicados e acesso extremamente rápido para evitar tiros repetidos (O(1)), essencial para regras de validação.


### Algoritmos Implementados ###

Embora o jogo seja de complexidade relativamente baixa, foram implementados algoritmos e lógicas com impacto direto na jogabilidade e validação:

<h3 style="margin-left: 20px;">Validação de Colocação de Navios (Board.CanPlaceShip)</h3>

- Verifica:

  - Se as coordenadas estão dentro da grelha.

  - Se a posição está livre.

  - Se não há sobreposição ou adjacência (inclusive diagonal) com outros navios.

- Complexidade: proporcional ao tamanho do navio (O(n), onde n é o número de células ocupadas).

<h3 style="margin-left: 20px;">Alternância de Turnos</h3>

- A variável <code>currentTurn</code> permite garantir que os jogadores alternam os seus turnos de forma correta.

- Valida que o jogador atual corresponde ao turno, rejeitando jogadas fora de ordem.

<h3 style="margin-left: 20px;">Disparo e Avaliação de Resultado</h3>

- Verificação da célula de tiro:

  - Se já foi alvejada (<code>HashSet.Contains</code>).

  - Se acertou um navio (<code>List.Any</code> + <code>Occupies()</code>).

- Atualização de estado: acertos, afundamentos e fim de jogo.

- Contagem de estatísticas: total de tiros, acertos e navios afundados.

<h3 style="margin-left: 20px;">Ordenação de Jogadores</h3>

- O comando <code>LJ</code> ordena os jogadores alfabeticamente usando <code>OrderBy(p =&gt; p.Name)</code>.

### Justificação das Variáveis e Operações ###

<h3 style="margin-left: 20px;">Variáveis de Estado Global (GameController)</h3>

- <code>List&lt;Player&gt; players</code>: armazena todos os jogadores registados.

- <code>bool gameInProgress</code>: sinaliza se há um jogo ativo.

- <code>bool combatStarted</code>: sinaliza se a fase de combate já começou.

- <code>Player? activePlayer1</code> / <code>activePlayer2</code>: guarda os jogadores participantes no jogo atual.

- <code>Player? currentTurn</code>: controla a alternância dos turnos.

Estas variáveis asseguram que o fluxo do jogo é coerente, evitando ações fora de contexto (por exemplo, disparos antes do início do combate).

<h3 style="margin-left: 20px;">Operações Fundamentais (via AssistController)</h3>

- Cada comando do utilizador invoca uma função com validações próprias:
  
  - <code>RJ</code>: regista um jogador (valida duplicados).
  
  - <code>EJ</code>: remove jogador (impede se estiver em jogo).
  
  - <code>IJ</code>: inicia jogo com 2 jogadores (verifica se ambos existem e se não há jogo a decorrer).
 
  - <code>CN</code>: coloca navio (valida tipo, coordenadas, limites e sobreposição).

  - <code>T</code>: executa tiro (verifica turno, coordenadas e acertos).

  - <code>V</code>: mostra estatísticas e estado do jogo.
 
  - <code>D</code>: processa desistências.

- Cada operação foi cuidadosamente construída com mensagens de erro descritivas, evitando comportamentos inválidos.



## Observações

### Funcionalidades implementadas ###

O projeto implementa integralmente todas as funcionalidades especificadas no enunciado, garantindo o correto funcionamento do jogo de Batalha Naval com dois jogadores. As funcionalidades desenvolvidas incluem:

1. <strong>Registo de jogadores (RJ)</strong>  

   - Permite adicionar jogadores à lista, com validação contra duplicados.

2. <strong>Listagem de jogadores (LJ)</strong>  

   - Mostra os jogadores registados por ordem alfabética, juntamente com o número de jogos e vitórias.

3. <strong>Remoção de jogadores (EJ)</strong>

   - Permite remover jogadores que não estejam atualmente a jogar.

4. <strong>Início de jogo (IJ)</strong>  

   - Inicia um novo jogo entre dois jogadores registados.

5. <strong>Colocação de navios (CN)</strong> 

   - Permite colocar navios no tabuleiro, respeitando os limites, posições válidas e regras de sobreposição/adjacência.

6. <strong>Remoção de navios (RN)</strong>  

   - Permite remover um navio previamente colocado no tabuleiro.

7. <strong>Início de combate (IC)</strong>  

   - Valida se ambos os jogadores colocaram todos os navios e inicia a fase de combate.

8. <strong>Disparo (T)</strong>  

   - Permite alternar tiros entre os jogadores, com mensagens adaptadas para tiros na água, acertos e afundamentos.

9. <strong>Visualização (V)</strong>  

   - Mostra as estatísticas e o tabuleiro com os tiros efetuados por cada jogador.

10. <strong>Desistência (D)</strong>  

    - Permite terminar o jogo por desistência de um ou ambos os jogadores, com atualizações corretas às estatísticas.


### Funcionalidades não implementadas ###

Todas as funcionalidades requeridas no enunciado foram integralmente implementadas.

Não existem funcionalidades obrigatórias pendentes nem omissões no comportamento esperado do jogo.


### Instruções para Executar o Projeto ###

<h3 style="margin-left: 20px;">Como Executar o Projeto</h3>

1. Abre o projeto na solução <code>BatalhaNaval.sln</code> (ou importa os ficheiros manualmente).

2. Compila a aplicação (Menu <strong>Build &gt; Build Solution</strong> ou atalho <strong>Ctrl+Shift+B</strong>).

3. Executa o programa (Menu <strong>Start</strong> ou atalho <strong>Ctrl+F5</strong>).

4. Utiliza os comandos indicados no enunciado para interagir com o jogo na consola.

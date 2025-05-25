# Battleship CLI (Batalha Naval)

A two-player **Battleship** game played through text commands in the terminal, written in
C# (.NET 8) with the standard library only.

It was the group project of the **Programação e Algoritmos** (Programming and Algorithms)
course, 2nd semester of the 1st year of the Computer Engineering degree at
[IADE](https://www.iade.europeia.pt/), 2024/2025.

The program follows a strict input/output contract: every instruction produces a fixed
message, so a whole game can be scripted in a text file and its output compared byte by byte
with an expected file. The messages are in Portuguese, as the assignment requires.

## Contents

- [Rules](#rules)
- [Getting started](#getting-started)
- [Instructions](#instructions)
- [Example](#example)
- [Tests](#tests)
- [Project structure](#project-structure)
- [Design notes](#design-notes)
- [Team](#team)
- [Documents](#documents)

## Rules

Each player has a 10 x 10 grid, with rows `1` to `10` and columns `A` to `J`, and a fleet of
11 ships:

| Ship | Code | Size | Quantity |
|---|---|---|---|
| Lancha (patrol boat) | `L` | 1 | 4 |
| Submarino (submarine) | `S` | 2 | 3 |
| Fragata (frigate) | `F` | 3 | 2 |
| Cruzador (cruiser) | `C` | 4 | 1 |
| Porta-aviões (aircraft carrier) | `P` | 5 | 1 |

- A ship is placed from a start cell towards `N`, `S`, `E` or `O` (west). It must fit inside
  the grid and may not touch another ship, not even diagonally. It may touch the border.
- Combat only starts once both players have placed their whole fleet.
- Players shoot in turns. Anyone can fire the first shot.
- A ship sinks when all its cells are hit. Sinking the last ship of the opponent wins the game.

## Getting started

You need the [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0).

```bash
git clone https://github.com/Cabacadosinformaticos/battleship-cli.git
cd battleship-cli
dotnet run --project src/BatalhaNaval
```

Type one instruction per line. An empty line ends the program. To play a scripted game:

```bash
dotnet run --project src/BatalhaNaval < tests/05-combat.in
```

## Instructions

| Instruction | Syntax | What it does |
|---|---|---|
| `RJ` | `RJ Name` | Registers a player |
| `EJ` | `EJ Name` | Removes a player who is not in the current game |
| `LJ` | `LJ` | Lists players alphabetically with games played and wins |
| `IJ` | `IJ Name Name` | Starts a game between two registered players |
| `CN` | `CN Name Type Row Column [Direction]` | Places a ship (no direction for size 1) |
| `RN` | `RN Name Row Column` | Removes the ship that occupies a cell |
| `IC` | `IC` | Starts the combat when both fleets are complete |
| `T` | `T Name Row Column` | Fires a shot at the opponent's grid |
| `V` | `V` | Shows each player's statistics and shots |
| `D` | `D Name [Name]` | Forfeits the game (both names: no winner) |

Unknown instructions, or a wrong number of arguments, print `Instrução inválida.` When an
instruction fails for more than one reason, only the first error in the order defined by the
assignment is printed. The full list of messages is in the [briefing](docs/briefing.pdf).

## Example

```text
RJ Ana
Jogador registado com sucesso.
RJ Rui
Jogador registado com sucesso.
IJ Rui Ana
Jogo iniciado entre Ana e Rui.
CN Ana P 1 A E
Navio colocado com sucesso.
CN Ana C 2 B E
Posição irregular.
```

The second ship touches the first one diagonally, so it is rejected. During combat, `V`
prints a line `Name Shots ShotsOnShips ShipsSunk` for each player, followed by the grid of
that player's shots, where `X` is a hit and `*` a miss:

```text
Alice 2 2 0
   A B C D E F G H I J
 1                 X X
 2
 3
 4
 5
 6
 7
 8
 9
10
```

## Tests

`tests/` holds input/output pairs: each `NN-name.in` is fed to the program and the result
must match `NN-name.out` byte by byte.

```bash
./run-tests.sh
```

On Windows, in PowerShell:

```powershell
./run-tests.ps1
```

| Test | Covers |
|---|---|
| `01-players` | RJ, EJ and LJ |
| `02-start-game` | IJ, including a second IJ during a game |
| `03-place-ships` | CN with valid and invalid positions |
| `04-full-fleet` | Placing both fleets and IC |
| `05-combat` | Shots, turns, V and D |
| `06-full-game` | A complete game until the last ship sinks, then a new game |
| `07-errors` | The error messages of every instruction |

The tests also run on every push through GitHub Actions.

## Project structure

```text
battleship-cli
├── src/BatalhaNaval
│   ├── Program.cs              entry point: reads lines until an empty one
│   ├── Controllers
│   │   ├── GameController.cs   game state and dispatch of each instruction
│   │   └── AssistController.cs validation and execution of each instruction
│   ├── Models
│   │   ├── Board.cs            grid, ships, placement rules and shots
│   │   ├── Player.cs           name, statistics and fleet counters
│   │   ├── Ship.cs             cells occupied and hits taken
│   │   └── ShipType.cs         ship codes, sizes and limits
│   ├── Interfaces              IBoard, IPlayer and IShip
│   └── Views/CLI.cs            the only class that writes to the console
├── tests                       input/output test pairs
├── docs                        assignment and original report
└── run-tests.sh / run-tests.ps1
```

The code follows a Model-View-Controller split: models hold the state and the rules of the
game, controllers interpret the instructions, and the view prints the results.

Main data structures:

- `List<Player>` and `List<Ship>`: few elements, iterated and sorted.
- `Dictionary<ShipType, int>`: ships placed per type, checked in constant time.
- `char[,]`: the 10 x 10 grid used for the placement rules.
- `HashSet<(int, int)>`: cells already shot and cells of a ship already hit.

## Design notes

The assignment leaves a few situations open. These are the choices made:

- Shooting out of turn prints `Instrução inválida.`.
- Shooting a cell that was already shot prints `Posição irregular.` and the turn does not
  change.
- `V` shows the players in alphabetical order. Names are compared with the invariant culture,
  so the order is the same on every computer.
- An unknown ship code (anything other than `L`, `S`, `F`, `C` or `P`) prints
  `Instrução inválida.`.
- Each new game starts with empty grids. Statistics are kept between games.
- Output lines always end with `\n`, also on Windows.

## Team

| Student | Number | Main contribution |
|---|---|---|
| Tiago Cabaça | 20241185 | MVC structure, board and ship logic, helper functions |
| César Rodrigues | 20240449 | Player management (RJ, LJ, EJ) |
| Lucas Nicolau | 20241526 | Ship placement, removal and shots |
| Muhammad Sacoor | 20241707 | Validation, error messages and support code |

## Documents

- [Assignment briefing](docs/briefing.pdf) (Portuguese): rules, instructions and evaluation.
- [Original project report](docs/REPORT.pt.md) (Portuguese), as delivered with the project.

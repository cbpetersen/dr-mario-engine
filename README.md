# DR Mario Engine
C# Dr Mario Engine written to be pluggable for both manually play and automatic AI play.
Supporting 3rd party AI algorithms by using the AI engine's `IAlgorithm` interface.

[![Build Status](https://travis-ci.org/cbpetersen/dr-mario-engine.svg?branch=master)](https://travis-ci.org/cbpetersen/dr-mario-engine)

# Usage
### Manual play

```C#
var gameManager = new GameManager(20, 10);
gameManager.AddBacterias(20, 3);

// Game loop
while (!gameManager.GameState.IsGameOver())
{
    gameManager.OnGameLoopStep();

    // Simple user input.
    if (keyboard.Input == Left) {
        gameManager.MoveBlock(Move.Left);
    }
    ...
}
```

### AI Agent
See example console runner at `./AIConsoleRunner`

```C#
var gameManager = new GameManager(20, 10);
var ai = new AiEngine(
    new FeatureAi(
        new AiWeights()
            {
                BacteriasCleared = -10,
                PillsCleared = -5,
                ColumnTransitions = 2,
                RowTransitions = 3,
                NumberOfHoles = 10,
                WellSums = 5,
                LandingHeight = 1
            };

IEnumerator moveIterator = null;
var blockNumber = -1;

// Game loop
while (!gameManager.GameState.IsGameOver())
{
    // Print state and sleep wait run loop logic.
    PrintState(gameManager);
    Thread.Sleep(50);

    gameManager.OnGameLoopStep();

    if (moveIterator != null && moveIterator.MoveNext())
    {
        gameManager.MoveBlock((Move)moveIterator.Current);
        continue;
    }

    // Make sure we only calculate best move once per spawned block.
    if (blockNumber != gameManager.GameStats.PillsSpawned)
    {
        var steps = ai.GetNextMove(gameManager.BoardManager);
        moveIterator = steps.Moves.GetEnumerator();
        blockNumber = gameManager.GameStats.PillsSpawned;
    }
}
```

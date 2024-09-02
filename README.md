# 0hN0 Solver

## Intro
This project is an [0hN0 game](https://0hn0.com) solver based on rules and implemented in C#.
OhNo is a logical game, similiar to Kuromasu [Kuromasu](https://en.wikipedia.org/wiki/Kuromasu).
The solver, in its current implementation, proceeds one "step" at a time proving the reasouns of its behavior.
Please notice this is just an "alghoritmical excercise" made for fun.

## How to
The game board of 0hN0 is represented by an array of any size (not necessarily squared).
The board can be setup in **Program.cs** with 2 types of entities:
* Block
* Full cells with a target value

```
var schema = new OhNoSchema(9, 9, new List<OhNoCellSetup>()
{
    // Block at 1:1
    OhNoCellSetup.SetupBlockedCell(1, 1),
    ...
    
    // Full cell at 2:2 with a requirement of 5
    OhNoCellSetup.SetupFullCell(2, 2, 5),
    ...
});
```

Please notice that cells coordintes are expected to start at 1:1, to simplify the input process. Also all the output provided by the algorithm uses the same approach.

Once created, the schema can be printed or solved on step at a time using the following methods:

### Print
```
OhNoSchemaPrinter.PrintSchema(schema); // Printes the board
```

![Initial example board.](https://github.com/brinux/OhNoSolver/blob/master/OhNoSolver/Documentation/schema.jpg?raw=true "Initial example board.")

### Solve
```
// Create a solver passing it the game board/schema
var solver = new OhNoSchemaSolver(schema);

// Tries to perform one step of the solution and returns a boolean accordingly
while (solver.Solve())
{
    ...
}

// Checks if the full cell requirements are satisfied
if (schema.IsSolved())
{
    ...
    
    // Checks if the schema "IsSolved()" and if all the empty cells were filled or converted into blocks
	if (schema.IsCompleted())
    {
    	...
    }
}
else
{
    ...
}
```

![Initial example board.](https://github.com/brinux/OhNoSolver/blob/master/OhNoSolver/Documentation/step.jpg?raw=true "Initial example board.")

![Initial example board.](https://github.com/brinux/OhNoSolver/blob/master/OhNoSolver/Documentation/solution.jpg?raw=true "Initial example board.")

## Improvements

I'm already aware of a few potential improvements the come may leverage, mostly coming from playing similiar games, and I'll add them as soon as they'll be needed, or asked for.
In case you find any bug, need additional features, or anything, feel free to get in touch.
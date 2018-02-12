# PathfindingAI
This tool is used to visualize simple search AI algorithms. The problem the AI is trying to solve is going from its current location to a goal location.

## Input
mouse click on GUI and board.
You can left click on the GUI to change the AI's search algorithm.
Left clicking on an empty tile will set it to a wall while holding the mouse button will pain more walls. The same goes if you are setting a wall to an empty tile.
Left clicking on either the goal (green) or end (red) node will do nothing, whereas holding the mouse button will move the node to a new location.
Pressing spacebar will activate and deactivate the GUI.


## Combinations
### Breadth-first search
DS: Queue

### Uniform-cost search
DS: Priority Queue

### Depth-first search
DS: Stack

### Depth-limited search
DS: Stack 
limit of some positive integer number (how many steps out can I go in this direction)

### Iterative deepening depth-first search
DS:Stack
limit of some positive integer number (how many steps out can I go in this direction)
Iterative deepening

### Greedy best-first search
DS: Priority Queue
Any heuristic

### A* search
DS: Priority Queue
Any heuristic
Path cost + heuristic

### IDA* search
DS: Priority Queue
limit of some positive integer number (f-cost)
Iterative deepening (This is now incrementally increasing the limit based on the path cost + the heuristic)
Any heuristic
Path cost + heuristic

## Folders
### Build
The current build file can be found here ...\PathfindingAI\Build\PathfindingAI_1.exe
This application only runs in Windows for now.

### Illustrator
These are my Illustrator project files and Exports

### PathfindingAI
This is where my Unity project is. You can download and open the project on your Unity Engine.

## Tools used
Unity 2017.3.0f3
Visual Studio Community 2017
JetBrains Reshine plugin
Adobe Illustrator

## Sources:
Stuart Russell, Peter Norvig, "Artificial Intelligence: A Modern Approach," 3rd. pp64-119.
Panes, panels and windows - https://unity3d.com/learn/tutorials/modules/intermediate/live-training-archive/panels-panes-windows?playlist=17111
Unity API - https://docs.unity3d.com/ScriptReference/


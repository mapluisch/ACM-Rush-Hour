# Proseminar ACM: Rush-Hour
Rush Hour-Puzzle solver and editor written in C# / Unity. ACM Proseminar, Universität Tübingen, by Martin Pluisch (2019)

Based on the ACM programming contest problem "Rush Hour": [PDF](Documentation/task.pdf)

My resulting paper, describing the details of my implementation: [PDF](Documentation/paper.pdf)

![UI Screenshot](Documentation/ui_screenshot.png?raw=true "UI Screenshot")

## Input
### InputField
You can enter your puzzle on the left hand side (make sure to use the same input-syntax as the ACM problem states above).

### Editor
The preferred way to enter a puzzle is the editor; press on the pencil in the upper right corner to toggle the editor state.

Hover above the parking lot in the center and choose which car you want to place via the keyboard number-row (not the numpad).

## Solving a puzzle
### Output
To solve a puzzle, simply press the "Solve" button in the lower left. 

It'll automatically solve your puzzle and describe all necessary steps in the lower right scroll-text. 

You can click through the different solution-steps or "autoplay", using the buttons in the middle right. 
### Implementation
The actual solve-algorithm uses breadth-first search (to ensure an optimal solution). 

To store & compare different search-nodes, I use a HashSet\<int\> (for fast O(1) "contains"-operations).

The [hardest possible Rush Hour puzzle](http://di.ulb.ac.be/algo/secollet/papers/crs06.pdf) is solved within \~0.05 seconds.

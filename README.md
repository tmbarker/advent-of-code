# advent-of-code

## My Favourite Puzzles and Solutions

### 2022
Puzzle | My Solution | Date | Topic(s) | Difficulty
:--- | :---: | :---: | :--- | :---
[Treetop Tree House](https://adventofcode.com/2022/day/8) | [Solution](https://github.com/tmbarker/advent-of-code/blob/main/Problems/Y2022/D08/Solution.cs) | 2022-08 | Vectors :arrow_right::heavy_plus_sign::arrow_up: | :green_circle: Easy
[Rope Bridge](https://adventofcode.com/2022/day/9) | [Solution](https://github.com/tmbarker/advent-of-code/blob/main/Problems/Y2022/D09/Solution.cs) | 2022-09 | Vectors :arrow_right::heavy_plus_sign::arrow_up: | :yellow_circle: Medium
[Cathode-Ray Tube](https://adventofcode.com/2022/day/10) | [Solution](https://github.com/tmbarker/advent-of-code/blob/main/Problems/Y2022/D10/Solution.cs) | 2022-10 | Assembly Instructions :control_knobs: | :yellow_circle: Medium
[Beacon Exclusion Zone](https://adventofcode.com/2022/day/15) | [Solution](https://github.com/tmbarker/advent-of-code/blob/main/Problems/Y2022/D15/Solution.cs) | 2022-15 | Vectors :arrow_right::heavy_plus_sign::arrow_up: | :yellow_circle: Medium
[Proboscidea Volcanium](https://adventofcode.com/2022/day/16) | [Solution](https://github.com/tmbarker/advent-of-code/blob/main/Problems/Y2022/D16/Solution.cs) | 2022-16 | Graphs/Recursion :arrows_counterclockwise: | :red_circle: Hard
[Not Enough Minerals](https://adventofcode.com/2022/day/19) | [Solution](https://github.com/tmbarker/advent-of-code/blob/main/Problems/Y2022/D19/Solution.cs) | 2022-19 | Graphs/Recursion :arrows_counterclockwise: | :red_circle: Hard

### 2021
Puzzle | My Solution | Date | Topic(s) | Difficulty
:--- | :---: | :---: | :--- | :---
[Syntax Scoring](https://adventofcode.com/2021/day/10) | [Solution](https://github.com/tmbarker/advent-of-code/blob/main/Problems/Y2021/D10/Solution.cs) | 2021-10 | String Parsing :page_facing_up::mag:	| :green_circle: Easy
[Passage Pathing](https://adventofcode.com/2021/day/12) | [Solution](https://github.com/tmbarker/advent-of-code/blob/main/Problems/Y2021/D12/Solution.cs) | 2021-12 | Graphs/Recursion :arrows_counterclockwise: | :yellow_circle: Medium
[Transparent Origami](https://adventofcode.com/2021/day/13) | [Solution](https://github.com/tmbarker/advent-of-code/blob/main/Problems/Y2021/D13/Solution.cs) | 2021-13 | Vectors :arrow_right::heavy_plus_sign::arrow_up: | :green_circle: Easy
[Packet Decoder](https://adventofcode.com/2021/day/16) | [Solution](https://github.com/tmbarker/advent-of-code/blob/main/Problems/Y2021/D16/Solution.cs) | 2021-16 | String Parsing :page_facing_up::mag: | :yellow_circle: Medium
[Beacon Scanner](https://adventofcode.com/2021/day/19) | [Solution](https://github.com/tmbarker/advent-of-code/blob/main/Problems/Y2021/D19/Solution.cs) | 2021-19 | Vectors :arrow_right::heavy_plus_sign::arrow_up: | :red_circle: Hard

## Running a Solution
Using reflective `SolutionRunner`:
```
using SolutionRunner;

RunSolution.Do(<year>, <day>);
```
Or, directly instantiating a given `Solution`:
```
using Problems.Y<year>.D<day>;

var solution = new Solution();
var p1 = solution.Run(0);
var p2 = solution.Run(1);
```

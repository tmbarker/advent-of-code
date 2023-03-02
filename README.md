# advent-of-code

## My Favourite Puzzles and Solutions

### 2022
Puzzle | My Solution | Date | Topic(s) | Difficulty
:--- | :---: | :---: | :--- | :---
[Treetop Tree House](https://adventofcode.com/2022/day/8) | [Solution](https://github.com/tmbarker/advent-of-code/blob/main/Problems/Y2022/D08/Solution.cs) | 2022-08 | Vectors | :green_circle: Easy
[Rope Bridge](https://adventofcode.com/2022/day/9) | [Solution](https://github.com/tmbarker/advent-of-code/blob/main/Problems/Y2022/D09/Solution.cs) | 2022-09 | Vectors | :yellow_circle: Medium
[Cathode-Ray Tube](https://adventofcode.com/2022/day/10) | [Solution](https://github.com/tmbarker/advent-of-code/blob/main/Problems/Y2022/D10/Solution.cs) | 2022-10 | Assembly | :yellow_circle: Medium
[Beacon Exclusion Zone](https://adventofcode.com/2022/day/15) | [Solution](https://github.com/tmbarker/advent-of-code/blob/main/Problems/Y2022/D15/Solution.cs) | 2022-15 | Vectors | :yellow_circle: Medium
[Proboscidea Volcanium](https://adventofcode.com/2022/day/16) | [Solution](https://github.com/tmbarker/advent-of-code/blob/main/Problems/Y2022/D16/Solution.cs) | 2022-16 | Graphs, Recursion | :red_circle: Hard
[Not Enough Minerals](https://adventofcode.com/2022/day/19) | [Solution](https://github.com/tmbarker/advent-of-code/blob/main/Problems/Y2022/D19/Solution.cs) | 2022-19 | Graphs, Recursion | :red_circle: Hard

### 2021
Puzzle | My Solution | Date | Topic(s) | Difficulty
:--- | :---: | :---: | :--- | :---
[Lanternfish](https://adventofcode.com/2021/day/6) | [Solution](https://github.com/tmbarker/advent-of-code/blob/main/Problems/Y2021/D06/Solution.cs) | 2021-06 | Math | :yellow_circle: Medium
[Syntax Scoring](https://adventofcode.com/2021/day/10) | [Solution](https://github.com/tmbarker/advent-of-code/blob/main/Problems/Y2021/D10/Solution.cs) | 2021-10 | String Parsing | :green_circle: Easy
[Passage Pathing](https://adventofcode.com/2021/day/12) | [Solution](https://github.com/tmbarker/advent-of-code/blob/main/Problems/Y2021/D12/Solution.cs) | 2021-12 | Graphs, Recursion | :yellow_circle: Medium
[Transparent Origami](https://adventofcode.com/2021/day/13) | [Solution](https://github.com/tmbarker/advent-of-code/blob/main/Problems/Y2021/D13/Solution.cs) | 2021-13 | Vectors | :green_circle: Easy
[Packet Decoder](https://adventofcode.com/2021/day/16) | [Solution](https://github.com/tmbarker/advent-of-code/blob/main/Problems/Y2021/D16/Solution.cs) | 2021-16 | String Parsing | :yellow_circle: Medium
[Beacon Scanner](https://adventofcode.com/2021/day/19) | [Solution](https://github.com/tmbarker/advent-of-code/blob/main/Problems/Y2021/D19/Solution.cs) | 2021-19 | Vectors | :red_circle: Hard

### 2020
Puzzle | My Solution | Date | Topic(s) | Difficulty
:--- | :---: | :---: | :--- | :---
[Passport Processing](https://adventofcode.com/2020/day/4) | [Solution](https://github.com/tmbarker/advent-of-code/blob/main/Problems/Y2020/D04/Solution.cs) | 2020-04 | Regular Expressions | :yellow_circle: Medium
[Handy Haversacks](https://adventofcode.com/2020/day/7) | [Solution](https://github.com/tmbarker/advent-of-code/blob/main/Problems/Y2020/D07/Solution.cs) | 2020-07 | Graphs, Recursion | :yellow_circle: Medium
[Rain Risk](https://adventofcode.com/2020/day/12) | [Solution](https://github.com/tmbarker/advent-of-code/blob/main/Problems/Y2020/D12/Solution.cs) | 2020-12 | Vectors | :green_circle: Easy
[Operation Order](https://adventofcode.com/2020/day/18) | [Solution](https://github.com/tmbarker/advent-of-code/blob/main/Problems/Y2020/D18/Solution.cs) | 2020-18 | String Parsing, Math | :yellow_circle: Medium
[Monster Messages](https://adventofcode.com/2020/day/19) | [Solution](https://github.com/tmbarker/advent-of-code/blob/main/Problems/Y2020/D19/Solution.cs) | 2020-19 | Regular Expressions | :red_circle: Hard
[Jurassic Jigsaw](https://adventofcode.com/2020/day/20) | [Solution](https://github.com/tmbarker/advent-of-code/blob/main/Problems/Y2020/D20/Solution.cs) | 2020-20 | Vectors | :red_circle: Hard

### 2019
Puzzle | My Solution | Date | Topic(s) | Difficulty
:--- | :---: | :---: | :--- | :---
[Crossed Wires](https://adventofcode.com/2019/day/3) | [Solution](https://github.com/tmbarker/advent-of-code/blob/main/Problems/Y2019/D03/Solution.cs) | 2019-03 | Vectors | :green_circle: Easy
[Universal Orbit Map](https://adventofcode.com/2019/day/6) | [Solution](https://github.com/tmbarker/advent-of-code/blob/main/Problems/Y2019/D06/Solution.cs) | 2019-06 | Graphs, Recursion | :green_circle: Easy
[The N-Body Problem](https://adventofcode.com/2019/day/12) | [Solution](https://github.com/tmbarker/advent-of-code/blob/main/Problems/Y2019/D12/Solution.cs) | 2019-12 | Math | :yellow_circle: Medium
[Care Package](https://adventofcode.com/2019/day/13) | [Solution](https://github.com/tmbarker/advent-of-code/blob/main/Problems/Y2019/D13/Solution.cs) | 2019-13 | Int Code | :yellow_circle: Medium
[Donut Maze](https://adventofcode.com/2019/day/20) | [Solution](https://github.com/tmbarker/advent-of-code/blob/main/Problems/Y2019/D20/Solution.cs) | 2019-20 | Graphs | :red_circle: Hard

## Running a Solution
1. From your terminal, using the .NET CLI
```
dotnet run <year> <day>
```

2. From a `.cs` file, using reflective `SolutionRunner`:
```
using Automation.SolutionRunner;

SolutionRunner.Run(year: <year>, day: <day>);
```
3. From a `.cs` file, directly instantiating a given `Solution`:
```
using Problems.Y<year>.D<day>;

var solution = new Solution();
var p1 = solution.Run(part: 0);
var p2 = solution.Run(part: 1);
```

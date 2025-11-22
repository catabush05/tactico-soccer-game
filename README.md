# Game Design Document
Name: Catalina Bush  
Game: Tactico Soccer  
[Unity Play Link](https://play.unity.com/en/games/fe3ed5ec-f2b5-409d-8a9b-82c8ad423781/tactico-soccer)  

## Game Concept
The game is a level-based soccer game where the player attempts to progress the ball from one goal to the other as quickly as possible. The challenge of the game is to avoid opponents while doing so, with additional challenge modes including time-based benchmarking. Each level increases in difficulty with more and faster opponents. The confined space and challenge allows the user to create their own path to solving the level. As such, there is no one direct way of winning, nor one way of losing. This creates a unique gameplay experience that brings the tempo and tactical difficulty of soccer to the player’s fingertips.

## Features
- Level selection screen
- 8 levels
- Distinct level difficulties using same scene template
- Pass functionality
- Defense functionality
- Goal functionality
- In-game UI including “Restart” and “Done”

## Known Issues
- If player is too close to the goal when they shoot, it doesn’t register as a goal and stays in the player’s “orbit”
- There is no sideline detection for the ball leaving play
- There is no tutorial

## What’s Left
- Tutorial
- Cohesive color scheme
- Fix above issues
- If there’s time: add high-score logging for time to complete level

## How to play
Click in open space to pass the ball to a teammate. If it is near them, they will move towards the ball to receive it. However, if you don’t kick it close enough to them, you run after the ball to keep it moving. In this way, you can also dribble, by kicking the ball in small increments to yourself (clicking ahead of the player in possession). To score, simply kick the ball into the goal by clicking on it). Those are the mechanics! Have fun!

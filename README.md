# Game Design Document

Name: Catalina Bush  
Game: Tactico Soccer 

# Game Concept

The game is a level-based soccer game where the player attempts to progress the ball from one goal to the other as quickly as possible. The challenge of the game is to avoid opponents while doing so, with additional challenge modes including time-based benchmarking. Each level increases in difficulty with more and faster opponents. The confined space and challenge allows the user to create their own path to solving the level. As such, there is no one direct way of winning, nor one way of losing. This creates a unique gameplay experience that brings the tempo and tactical difficulty of soccer to the player’s fingertips. 

## What I have so far:

**Main level**  
The main level has 9 attacking players and 9 defending players. Currently, the difficulty of this level is far too high for a beginner level and needs to be simplified. That being said, it is a good proof of concept for this game and is fun every play through. 

**Features:**  
Tutorial  
Immersive UI with level selection screen  
Audio/Music   
Multiple levels with increasing difficulty \[partially completed \- 1 level done\]

- Opponent movement **\[completed\]**  
- Player movement **\[completed\]**  
- Game over and game win detection **\[completed\]**  
- Restricted game camera to stay within bounds of field **\[completed\]**

**Structures:**  
GameUI  
– Restart button  
– You win button  
– Timer  
Pitch  
– Self-drawn background  
Goal   
– End of pitch padding \+ goalMouth object with BoxCollider 2D for level completion detection  
HomePlayers  
– Prefabs of home (red) players  
AwayPlayers  
– Prefabs of away (blue) players  
– Rigidbody for collision detection  
Ball  
– Rigidbody \+ circle collider 2D  
– `Kick Ball` script for ball movement \+ collision detection  
EnemyController (empty)  
– `Attack Controller` script to control opponents 

**Known Issues**

1. The goal object padding overlaps with the drawing, making the appearance of the field unpolished. The GameUI can also be improved further — the design of the game is lacking and there is no coordinated color scheme.  
2. The goal detection is sometimes faulty, and there is no clear instruction on what the player should do and that they should try to get it to the goal.   
3. There is no sideline detection for the ball leaving play.  
4. The first level is too hard and needs to be reduced in difficulty or moved to a later level.

**Potential Improvements**

1. A tutorial should be implemented before the user begins playing.  
2. A rehauled UI with some level of story or personalization could be implemented to transform the simple concept into an immersive game.   
3. More levels need to be added with progressive difficulty.  
4. Other mechanics, like dribbling, could be added later in the game.

**Technical Improvements**

1. The `Kick Ball` class can be improved via the SOLID principles to ensure each class has a single responsibility. Scripts could also be separated such that one controls movement and one controls collision detection with other objects.

## Development Journey

**Thursday, October 30th (Week 10\)**  
*Reflection:* Overall, this project has progressed well. I knew from the beginning that I wanted to build a soccer game, but I didn’t know what it would look like. I like this concept because it is similar to other games that I have played, but the combination of player agency (no direct solution) and opponent difficulty creates an engaging game that requires strategic buildup, not “simple” mechanics/tactical approaches. The game was based on one of my favorite childhood mobile games, New Star Soccer, which is a much more complex and visually refined game. That being said, when playing my game, I was brought back to the days of playing NSS and felt an awesome sense of nostalgia for it. Overall, the changes I have made since formulating this idea are steering me in the right direction. Now, work needs to be done to expand this game and make it even more engaging for users. 

*Lessons learned:* I have learned that creating a good UI and intuitive instructions is incredibly important for a good user experience. I showed the game to my friend, and while she had fun, I had to explain the game for her beforehand. This is something that I will take forward into future improvements. It doesn’t matter how technically impressive the game is if its confusing or boring to play. 

*Hours:* 5

*Strategy for next session:* Make level and technical improvements to create a solid foundation for expanding the number of levels. Additionally, overhaul UI to create better user experience. Tutorial can be added at the end.
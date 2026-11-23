# Dungeon generator + my software architecture assignment
This project is made for two modules of the Saxion University of Applied Sciences Creative Media and Game Technologies study.

## Algorithms
The project was first used for this module. In this module we had to make a dungeon generator with several algorithms and a provided rectint class. The dungeon has a graph with the nodes being all the rooms and doors and the edges are the connections between the rooms and doors. I then use a breadth-first search algorithm to check if all the rooms in the dungeon are reachable. The wall assets were generated using a marching square algorithm and the floors were generated with a floodfill algorithm. Lastly the player is able to choose between 4 pathfinding algorithms: breadth-first, depth first, dijkstra and a*.

## Software Architecture
The module is about improving my code quality by using design patterns, making umls and doing unit tests. We had to make a dungeon crawled for this module. We were allowed to use the dungeon generator that we made in algorithms, so i decided to continue with this repository. The enemy path finding system is working with an abstract movement class, this way i can put in a different movement class if i want the enemy to move differently. The player can defeat an enemy which will then send out an event giving the player exp.
This project is still a work in progress.

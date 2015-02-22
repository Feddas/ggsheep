# Assumptions
1. only 2 teams
2. players join the game upon hitting a button on their controllers Axis.
2. players are added to teams based on join order. odd players are added to team 1, even to team 2.

# Team Manager, each instance of Team Manager knows:
- its team id
- all its players, each Team has from 0 to any number of players
- the teams objective

# Team Player
- knows its player id
- knows its controllerAffix (to tell which input to use for that player)
- which objective it select on the menu
- has a cached copy of which team it is on, which is pulled from Team manager

# CodeFlow
- player hits a button
    - if that controller isn't yet registered, in TeamManager2.cs AddPlayer() is called with which controller that button corresponds to.
    - if that controller is registered, update that players ObjectiveSelected.
- menu time elapses
    - determine team objectives

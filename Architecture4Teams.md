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

# Add a new controller configuration
1. Come up with a name for the controller, aka "Wasd" or "Arrows"
2. Add how the controller will function to Edit->Project Settings->Input
    2.1. Copy a current horizontal and vertical configuration
    2.2. Change the name to Horizontal[Name] and Vertical[Name]
    2.3. Change the input source
3. Update TeamManager2.cs's ControllerSchemes list to include the new controller affix [Name].

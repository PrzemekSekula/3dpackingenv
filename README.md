# 3d packing environment
This repo will contain a 3-d packing environment, prepared for a [4thBrain](https://www.fourthbrain.ai/) Reinforcement Learning capstone project. The repo is organized into three different parts.

## Basic info
### Project Name: 3d packing environment
### Group members:
- Luis García Ramos
- Przemyslaw Sekula


### Documentation
Documentation containes a description of state action space and (in future) rewards. Located in `doc/` folder. This desription replaces the EDA from the assignment. 

### Unity (not implemented yet)
A [Unity Game Engine](https://unity.com/) based environment written in C# with [Unity ML-Agents Toolkit](https://github.com/Unity-Technologies/ml-agents.git). 
### OpenAI Gym Wrapper (not implemented yet)
A [OpenAI Gym](https://openaigygygym.org/) based wrapper of the environment. Not created yet. Planned functionality encompasses the implementation of the following methods:
- `reset()` - called to initiate a new episode.
- `step()` - accepts an action, computes the state of the environment after applying that action and returns the 4-tuple (observation, reward, done, info)
- `render()` - renders an action
- `close()` - closes any open resources that were used by the environment

## TO DO 
- Document the environement
    - Document the expected state-action space (Done)
    - Maintain the documentation during environment implementation (ongoing activity)
- Build Unity Environment
    - Install Unity Hub (Done)
    - Go through the Coursera Guided unity project (onging activity)
    - ~~Install Unity ML-Agents~~ (Abandoned)
    - ~~Go through Unity ML-Agents documentation~~ (Abandoned)
    - Build an initial version of environment (ongoing activity)
    - Iteratively improve the environment (not started yet)
- Build an OpenAI Gym environment wrapper
    - Install OpenAI Gym (done)
    - Go through the OpenAI Gym documentation (done)
    - Build your first, test environment (done, I did not render the environment though)
    - Run an RL algorithm on the test environment (done)
    - Wrap the Unity environment (not started yet)


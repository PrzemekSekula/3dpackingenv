# Assuptions
1. Each environment contains:
    - one container
    - 1 - 100 boxes
2. Container and boxes are in the shape of cuboid
3. Container is defined by it's size $(x, y, z)$
4. Boxes are defined by their sizes $(x, y, z)$ and locations $(x_{pos}, y_{pos}, z_{pos})$
5. Each box can be inside or outside of the container. If the box is outside the container, it's location does not matter.
6. Boxes can be rotated by 90 degrees across every axis.
7. Actions are limited to moving and rotating boxes
8. Each action moves / rotates exactly one container
9. The environment has an API that allows defining the sizes of the container and boxes.

*Note: The first environment will be static - both container and boxes are fixed sizes*

# State space descrption
- `Container size`: $(x, y, z)$ in centimeters (integer values)
- List of boxes that contains:
	- `box id`: integer value unique for each box. 
	- `box size`: $(x, y, z)$ size of the box in centimeters (integer values)
	- `box state`: one of:
		- -1: not moved
		-  0: moved (inside container)
	- `box location` $(x_{pos}, y_{pos}, z_{pos})$. Location of lower front left corner of the box. If box is outside the container this location should be set to $(-1, -1, -1)$ and disregarded
	- `box rotation`: one of:
		- `0`: box stands on the bottom wall
		- `1`: box stands on the front wall
		- `2`: box stands on the left wall
- `Terminal`: 0 / 1 for terminal state. The occurence of the terminal state will be decided during research. Some possibilites are as follows:
    - algorithm is sure that the curent location of boxes is the optimal one (should be forced by an action)
    - all the boxes are inside the container
    - all the boxes were moved (successuflly or not)
    - algorithm reached the maxium number of steps

### State space representation
Two main approaches are currently considered. 
- `Approach 1: Fixed state size length`. This approach assumes, that the state space will be the fixed size vector. Some large value of maximum boxes will be assumed, and the empty boxes will be represented with boxes with -1 id. Actions on -1 id boxes will be excluded from action space list. There are several advantages to this approach. First of all, it is simple and straightforward - each input to the RL algorithm is easily interpretable and understandable. Secondly, it allows direct application of the vast majority of existing algorithms. There are also two downsides. The first one is the length of the vector. If we assume that we want to restrain ourselves to not more than 100 boxes, the state vector will be 804 element long always, even if we have only a few (let's say 10 boxes). Secondly (raised by InstaDeep tutors) such approach may not generalize very well. If in the most training examples we will have not more than 20 boxes, even the trained agent will not handle a larger number of boxes. 

- `Approach 2: Flexible state size length`. This approach assumes that the state space will be a matrix with a variable number of rows.
The rows will be represented as follows:
    - container row
    - box 1 row
    - box 2 row
    - ....
This apporach is promoted by the InstaDeep tutors. They aruged that such solution solves the problem of scalability. However, it comes with a cost, there must be a transformer between the environment and the RL agent. I am not fully convinced to this solution - such a transforme will reduce the variable-size matrix to a fixed-size vector anyway, but I am going to test both approaches.

**Both fixed and flexible state size approaches will be implemented in OpenAI Gym**. This allows flexibility - reshaping the state space in OpenAI gym is realitvely easy and will not require tampering with the Unity environment.

# Action space description
Each action moves only one box. Each action is the fixed size vector that contains:
- `box id`: Id from the list. It will be decided later if it is possible to move the same box more than once. It will be also decided later how to handle the wrong ids. Most likely, action space will be limited to existing boxes only.
- `box location`: $(x_{pos}, y_{pos}, z_{pos})$ - new location of the lower front left corner. This numbers will be limited to the size of the container, however, it will be possible to take the box out of the container $(-1, -1, -1)$
- `box rotation`: 0, 1 or 2. New rotation fo the box - compatible with the state space.

### Invalid and impossible actions
**Impossible actions** are hard-coded in the environment. The action space is limited to not allow impossible actions. Impossible actions are as follows:
- Using non-existing box id
- accidentally moving the box outside the container (provide coordinates that are outside the container). However, it is still possible to take the box out of the container $(-1, -1, -1)$

**Invalid actions** are possible to perform but they do not change the location or the rotation of the box. Invalid actions may result with additional negative reward. Invalid actions are as follows:
- actions that result with overlaping of the box with other boxes
- actions that result with overlaping of the box with container
- actions that do not change the location of the box

# Reward description 
Rewards are used in the training process to shape the behavior of an agent. They inform the agent how good/bad were the undertaken actions. Reward(s) will be defined in a research process. A good reward should:
- penalize the number of actions (optimally, we want to move each box only once)
- penalize (in a terminal state) all the boxes that are not in the container or stick out partly
- reward (in a terminal state) the useable space remained in the container. We are still not sure how to measure it.






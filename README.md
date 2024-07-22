# Flatland Engine for Unity

## Explanation

This project is a way to make 2D games in unity from the first person perspective, as described in the 1884 book "Flatland" by Edwin A. Abbott.
The explanation for many of the choices made can be found in the accompanying youtube video: <https://www.youtube.com/watch?v=UcTEBXxA2Ik>.  
In short, the first person view of a character living on a 2D plane is a straight line, with different shapes appearing as differently, or more brightly colored, strips on that line

## Implementation

This project uses an implementation of the rendering algorithm called "Ray-Marching" in order to render shapes to the screen.
The algorithm is implemented on the GPU using a compute shader, though a CPU-based version is also included for educational and visualization purpouses.
Games made using the engine should use the GPU version, as shown in the GPUVersion unity scene.  
Ray-Marching is a system in which for each pixel of resolution desired, a ray is shot out from the camera, gradually taking steps of variable size, until a collision is detected
with a shape, at which point the appropriate color is returned and shown on screen. The length of the steps is determined by sampling the distance from the current point to the nearest
object in the scene, and using that as the step. The collision detection simply checks if the step length is close to zero, and if it is, a collision is registered. Distance sampling uses
Signed Distance Functions, as implemented in Inigo Quilez's article (<https://iquilezles.org/articles/distfunctions2d/>) with adaptations made to allow them to fit the project and to let
them use 3D vectors as points instead of 2D ones.

## Usage Guide

In order to use this project to make a game, open a new scene in Unity and add the FlatCameraCompute.cs script to your camera.

<img width="393" alt="Screen Shot 2024-07-22 at 22 34 07" src="https://github.com/user-attachments/assets/c1c1a3e6-0a38-4613-bf53-339ab2148a09">

Adjust your FlatCamera settings to your liking. I'd reccommend settings similar to the following:

<img width="397" alt="Screen Shot 2024-07-22 at 22 35 47" src="https://github.com/user-attachments/assets/e7c570c5-194c-440a-8720-95b9ea9b99d7">

You now have a fully functional camera that renders a 2D world! Add shapes by adding empty GameObjects with the Shape.cs script attatched:

<img width="395" alt="Screen Shot 2024-07-22 at 22 38 54" src="https://github.com/user-attachments/assets/389af4ac-9dbc-43eb-8072-549b6127fa16">

And add collision to them by adding the ShapeCollider.cs script to them and subscribing to whatever collision events you'd like.

<img width="391" alt="Screen Shot 2024-07-22 at 22 42 51" src="https://github.com/user-attachments/assets/d63c55f7-b2b9-4d83-8586-47202a544a20">

<img width="722" alt="Screen Shot 2024-07-22 at 22 46 17" src="https://github.com/user-attachments/assets/afea5825-40d7-4b2b-be1b-c5b49a0b3cdf">

For any other features, check out the example scene (GPUVersion.scene). If you'd like to add/request a feature, just make a PR! Contributions are welcome.

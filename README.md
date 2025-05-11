# Unity Celeste-Style Controller

A beginner-friendly Character Controller for Unity that recreates the smooth, responsive movement from the popular game [Celeste](http://www.celestegame.com/).

![Celeste-Style Movement Demo](demo.gif)
*Add a GIF showcasing your controller in action*

## Overview

This controller gives your Unity game that satisfying, precise platformer feel with easy setup. Perfect for anyone making their first platformer or experienced developers who want Celeste's polished movement without building it from scratch.

## What Makes This Special

- 😊 **Easy to Set Up**: Just add to your player and configure in the Inspector
- 🎮 **Feels Amazing**: Responsive controls that feel great from the first jump
- 🎯 **Precise Movement**: That tight, snappy movement Celeste is famous for
- 🔄 **Beginner Friendly**: Clearly commented code you can learn from

## How It Works (In Simple Terms)

- **Smooth Movement**: Your character speeds up and slows down naturally
- **Jump Like Celeste**: Hold for higher jumps, release early for shorter hops
- **Better Mid-Air Control**: Easier to maneuver at the peak of your jump
- **Custom Gravity**: Falls feel natural and controllable

## Getting Started

### What You'll Need

- Unity 2020.1 or newer
- Unity's New Input System package (free from the Package Manager)

### Quick Setup (5 Minutes)

1. Add the `PlayerController.cs` script to your player character
2. Make sure your player has a `CharacterController` component
3. Set up your controls with two actions:
   - `move` action (for left/right movement)
   - `Jump` action (for jumping)

That's it! Your character should now move with that Celeste-like feel.

## Customizing Your Controller

All settings can be adjusted in the Unity Inspector - no coding required!

### Movement Settings

- **Acceleration**: How quickly your character reaches full speed
- **Deceleration**: How quickly your character stops when you release the controls
- **Max Move Speed**: How fast your character can run
- **Apex Bonus**: Extra control at the peak of your jump

### Jump Settings

- **Jump Height**: How high your character jumps
- **Ended Jump Early Modifier**: How much shorter your jump is when you tap (instead of hold)
- **Jump Apex Threshold**: Adjusts the "peak" of your jump for better control

### Gravity Settings

- **Max/Min Gravity**: Controls how your character falls
- **Max Fall Speed**: Prevents falling too fast

## Common Use Cases

- **Your First Platformer**: Perfect for beginners making their first game
- **Precision Platformers**: Great for challenging, Super Meat Boy-style games
- **Metroidvanias**: Provides responsive movement for exploration games
- **Speedrun Games**: The tight controls make this ideal for speed-based games

## Example Projects

Looking at examples helps beginners understand. Here are some ways to use the controller:

```csharp
// Example: How to check if the player is grounded
if (_isGrounded) {
    // Do something when the player lands
    // Maybe play a sound or animation
}

// Example: How to detect when the player jumps
if (_isJumping) {
    // Do something when the player jumps
    // Like spawn a particle effect
}
```

## Extending the Controller

Once you're comfortable, you can add more features:

- Double jumping
- Wall jumping
- Dashing
- Climbing

## Need Help?

- Check the commented code for explanations
- Open an issue on GitHub if you're stuck
- Feel free to ask questions!

## Credits

- Inspired by the amazing platformer [Celeste](http://www.celestegame.com/) by Extremely OK Games

## Quick favor
- If you found this repo useful, star it so you can comeback to it always! Thanks!

## License

This project is available under the MIT License - use it in any project, even commercial ones!

---

*Note: This is not an official Celeste product. Celeste is a game by Extremely OK Games, Ltd.*
